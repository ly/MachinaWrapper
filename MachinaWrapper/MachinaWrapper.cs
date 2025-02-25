﻿using Machina;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Machina.FFXIV;
using Machina.Infrastructure;
using MachinaWrapper.Common;


namespace MachinaWrapper
{
    public static class MachinaWrapper
    {

        private static uint Port = 13346U;
        private static ulong CurrentPacketIndex = 0U;
        private static PacketDispatcher PacketDispatcher;

        public static void Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            var MonitorIndex = Array.IndexOf(args, "--MonitorType");
            var PIDIndex = Array.IndexOf(args, "--ProcessID");
            var IPIndex = Array.IndexOf(args, "--LocalIP");
            var RegionIndex = Array.IndexOf(args, "--Region");
            var PortIndex = Array.IndexOf(args, "--Port");

            if (PortIndex != -1)
            {
                Port = uint.Parse(args[PortIndex + 1]);
            }

            var MonitorType = NetworkMonitorType.RawSocket;
            if (MonitorIndex != -1 && args[MonitorIndex + 1] == "WinPCap")
            {
                MonitorType = NetworkMonitorType.WinPCap;
            }

            var localRegion = Region.Global;
            if (RegionIndex != -1)
            {
                localRegion = args[RegionIndex + 1] switch
                {
                    "KR" => Region.KR,
                    "CN" => Region.CN,
                    _ => localRegion,
                };
            }
            else if (!Util.SystemHasGlobalClient())
            {
                if (Util.SystemHasKRClient())
                {
                    localRegion = Region.KR;
                }
                else if (Util.SystemHasCNClient())
                {
                    localRegion = Region.CN;
                }
            }
            
            var window = FindWindow("FFXIVGAME", null);
            GetWindowThreadProcessId(window, out var pid);
            var proc = Process.GetProcessById(Convert.ToInt32(pid));
            var gamePath = proc.MainModule?.FileName;

            var monitor = new FFXIVNetworkMonitor
            {
                MonitorType = MonitorType,
                ProcessID = PIDIndex != -1 ? uint.Parse(args[PIDIndex + 1]) : 0,
                LocalIP = IPIndex != -1 ? IPAddress.Parse(args[IPIndex + 1]) : IPAddress.None,
                UseRemoteIpFilter = Array.IndexOf(args, "--UseSocketFilter") != -1,
                MessageReceivedEventHandler = MessageReceived,
                MessageSentEventHandler = MessageSent,
                WindowName = localRegion == Region.CN ? "最终幻想XIV" : "FINAL FANTASY XIV",
            };

            if (!string.IsNullOrEmpty(gamePath))
            {
                monitor.FFXIVDX11ExecutablePath = gamePath;
            }

            PacketDispatcher = new PacketDispatcher("http://localhost:" + Port);

            var commander = new Commander("kill");
            commander.AddCommand("start", () =>
            {
                PacketDispatcher.Start();
                monitor.Start();
            });
            commander.AddCommand("stop", () =>
            {
                try
                {
                    monitor.Stop();
                    PacketDispatcher.Cancel();
                }
                catch (NullReferenceException nre) // _monitor is null, and it's a private member of monitor so I can't check if it exists beforehand.
                {
                    Console.Error.WriteLine(nre);
                }
            });
            commander.OnKill(() =>
            {
                try
                {
                    monitor.Stop();
                    PacketDispatcher.Cancel();
                }
                catch (NullReferenceException) { }
            });
            commander.Start();
        }

        /// <summary>
        /// Executes on message receipt.
        /// </summary>
        private static void MessageReceived(TCPConnection connection, long epoch, byte[] data)
        {
            PacketDispatcher.EnqueuePacket(MessageSource.Server, data);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(TCPConnection connection, long epoch, byte[] data)
        {
            PacketDispatcher.EnqueuePacket(MessageSource.Client, data);
        }
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32.dll", SetLastError=true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}

// Generated by https://github.com/zhyupe/ffxiv-opcode-worker

namespace Sapphire.Common.Packets
{
    enum ServerZoneIpcTypeCN : ushort
    {
        ActorCast = 0x0073,
        ActorControl = 0x0388,
        ActorControlSelf = 0x012C,
        ActorControlTarget = 0x00F5,
        ActorFreeSpawn = 0x0192,
        ActorGauge = 0x0159,
        ActorMove = 0x019B,
        ActorSetPos = 0x038C,
        AoeEffect16 = 0x03DB,
        AoeEffect24 = 0x00A8,
        AoeEffect32 = 0x038E,
        AoeEffect8 = 0x011A,
        CFNotify = 0x010A,
        Chat = 0x0200,
        ContainerInfo = 0x0318,
        CurrencyCrystalInfo = 0x0176,
        DirectorStart = 0x03b6,
        Effect = 0x0287,
        EffectResult = 0x00EA,
        EventFinish = 0x01AD,
        EventPlay = 0x01CD,
        EventPlay4 = 0x018B,
        EventStart = 0x0157,
        Examine = 0x039F,
        ExamineSearchInfo = 0x00A6,
        GroupMessage = 0x0065,
        InitZone = 0x00A9,
        InventoryActionAck = 0x03E5,
        InventoryTransaction = 0x0221,
        InventoryTransactionFinish = 0x0141,
        ItemInfo = 0x03AB,
        MarketBoardItemListing = 0x0215,
        MarketBoardItemListingCount = 0x02F5,
        MarketBoardItemListingHistory = 0x00C1,
        MarketBoardSearchResult = 0x03B7,
        MarketTaxRates = 0x0338,
        NpcSpawn = 0x026F,
        ObjectSpawn = 0x02AD,
        PlaceFieldMarker = 0x0306,
        PlaceFieldMarkerPreset = 0x008C,
        PlayerSetup = 0x024E,
        PlayerSpawn = 0x00C0,
        PlayerStats = 0x0142,
        Playtime = 0x03C9,
        PrepareZoning = 0x01A6,
        RetainerInformation = 0x02BF,
        SomeDirectorUnk4 = 0x0094,
        StatusEffectList = 0x00E7,
        UpdateClassInfo = 0x0370,
        UpdateHpMpTp = 0x01DD,
        UpdateInventorySlot = 0x0291,
        UpdateSearchInfo = 0x02C7,
    };

    enum ClientZoneIpcTypeCN : ushort
    {
        ChatHandler = 0x0379,
        ClientTrigger = 0x0365,
        InventoryModifyHandler = 0x0364,
        SetSearchInfoHandler = 0x0234,
        UpdatePositionHandler = 0x007E,
        UpdatePositionInstance = 0x0132,
    };

    enum ServerChatIpcTypeCN : ushort
    {

    };

    enum ClientChatIpcTypeCN : ushort
    {

    };
}

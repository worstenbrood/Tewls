﻿namespace Tewls.Windows
{
    public enum Error
    {
        Success, NoError = 0,
        AccessDenied = 5,
        NotEnoughMemory = 8,
        NotSupported = 50,
        RemoteComputerUnavailable = 51,
        NetworkPathNotFound = 53,
        AlreadyAssigned = 85,
        BadDeviceType = 66,
        BadNetName = 67,
        InvalidPassword = 86,
        InvalidParameter = 87,
        InsufficientBuffer = 122,
        Busy = 170,
        MoreData = 234,
        NoMoreItems = 259,
        InvalidAddress = 487,
        BadDevice = 1200,
        ConnectionUnavailable = 1201,
        DeviceAlreadyRemembered = 1202,
        NoNetworkOrBadPath = 1203,
        CannotOpenProfile = 1205,
        BadProfile = 1206,
        BadProvider = 1207,
        ExtendedError = 1208,
        NoNetwork = 1222,
        Cancelled = 1223,
        ServerNotStarted = 2114,
        RemoteError = 2127,
        ServiceNotInstalled = 2184,
        WkstaNotStarted = 2138,
        UnexistingNetworkConnection = 2250,
        NoBrowserServersFound = 6118,
    };
}

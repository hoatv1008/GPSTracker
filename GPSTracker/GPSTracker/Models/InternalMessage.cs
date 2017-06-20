﻿using System;
namespace GPSTracker.Models
{
    public class StartListenLocationMessage { }

    public class StopListenLocationMessage { }

    public class LocationServiceStoppedMessage { }

    public class ClearLocationMessage { }

    public class LocationServiceNotEnableMessage { }

    public class LocationServiceNotAvailableMessage { }

    public class LocationMessage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
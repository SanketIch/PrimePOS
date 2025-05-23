﻿using System;

namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface IDeviceMessage {
        bool KeepAlive { get; set; }
        bool AwaitResponse { get; set; }

        byte[] GetSendBuffer();        
    }
}

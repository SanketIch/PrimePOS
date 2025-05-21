using System;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Terminals.Builders;

namespace MMS.GlobalPayments.Api.Terminals.Genius {
    internal class GeniusInterface : DeviceInterface<GeniusController> {
        internal GeniusInterface(GeniusController controller) : base(controller) {
        }
    }
}

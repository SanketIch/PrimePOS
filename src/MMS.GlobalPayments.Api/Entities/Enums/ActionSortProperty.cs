﻿using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    [MapTarget(Target.GP_API)]
    public enum ActionSortProperty {
        [Map(Target.GP_API, "TIME_CREATED")]
        TimeCreated,
    }
}

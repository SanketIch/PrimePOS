using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    public enum EmvFallbackCondition {
        [Map(Target.Transit, "ICC_TERMINAL_ERROR")]
        ChipReadFailure,

        [Map(Target.Transit, "NO_CANDIDATE_LIST")]
        NoCandidateList
    }
}

using System;
using System.Collections.Generic;

namespace Vantiv.ResponseModels
{
    class SignatureResponse
    {
        public int laneId { get; set; }
        public Signature signature { get; set; }
        public List<ApiError> _errors { get; set; }
        public bool _hasErrors { get; set; }
        public List<ApiLink> _links { get; set; }
        public List<String> _logs { get; set; }
        public string _type { get; set; }
        public List<ApiWarning> _warnings { get; set; }
    }
}

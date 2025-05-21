using System;
using System.Collections.Generic;

namespace Vantiv.ResponseModels
{
    public class SelectionResponse
    {
        public int selectionIndex { get; set; }
        public List<ApiError> _errors { get; set; }
        public bool _hasErrors { get; set; }
        public List<ApiLink> _links { get; set; }
        public List<String> _logs { get; set; }
        public string _type { get; set; }
        public List<ApiWarning> _warnings { get; set; }
    }
}

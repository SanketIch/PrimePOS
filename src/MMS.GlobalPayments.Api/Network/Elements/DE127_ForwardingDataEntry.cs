using MMS.GlobalPayments.Api.Network.Entities;

namespace MMS.GlobalPayments.Api.Network.Elements {
    public class DE127_ForwardingDataEntry {
        public DE127_ForwardingDataTag Tag{ get; set; }
        public string RecordId{ get; set; }
        public string RecordType{ get; set; }
        public string KeyBlockDataType{ get; set; }
        public string EncryptedFieldMatrix{ get; set; }
        public EncryptionType TepType{ get; set; }
        public string CardSecurityCode{ get; set; }
        public string EtbBlock{ get; set; }
        public string EntryData{ get; set; }
    }
}

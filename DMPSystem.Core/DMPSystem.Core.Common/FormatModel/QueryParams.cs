using System.Runtime.Serialization;

namespace DMPSystem.Core.Common.FormatModel
{
    [DataContract]
    public class QueryParams
    {
        public QueryParams()
        {
            Index = 1;
            Size = 10;
        }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int Size { get; set; }

    }
}

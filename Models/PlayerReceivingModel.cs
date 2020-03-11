using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerReceivingModel : PlayerModel
    {
        [DataMember(Name = "yds")]
        public string YDS { get; set; }

        [DataMember(Name = "tds")]
        public string TDS { get; set; }

        [DataMember(Name = "rec")]
        public string REC { get; set; }
    }
}
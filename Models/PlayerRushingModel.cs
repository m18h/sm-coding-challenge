using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerRushingModel : PlayerModel
    {
        [DataMember(Name = "yds")]
        public string YDS { get; set; }

        [DataMember(Name = "att")]
        public string ATT { get; set; }

        [DataMember(Name = "tds")]
        public string TDS { get; set; }

        [DataMember(Name = "fum")]
        public string FUM { get; set; }
    }
}
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerDataModel
    {
        [DataMember(Name = "rushing")]
        public List<PlayerRushingModel> Rushing { get; set; }

        [DataMember(Name = "passing")]
        public List<PlayerPassingModel> Passing { get; set; }

        [DataMember(Name = "receiving")]
        public List<PlayerReceivingModel> Receiving { get; set; }

        [DataMember(Name = "kicking")]
        public List<PlayerKickingModel> Kicking { get; set; }
    }
}

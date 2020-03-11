using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class PlayerKickingModel : PlayerModel
    {
        [DataMember(Name = "fld_goals_made")]
        public string FieldGoalsMade { get; set; }

        [DataMember(Name = "fld_goals_att")]
        public string FieldGoalsAttempted { get; set; }

        [DataMember(Name = "extra_pt_made")]
        public string ExtraPointsMade { get; set; }

        [DataMember(Name = "extra_pt_att")]
        public string ExtraPointsAttempted { get; set; }
    }
}
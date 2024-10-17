using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class ManageSkillAndSpecsVM
    {
        public List<Skill>? Skills { get; set; }
        public List<Specialization>? Specs { get; set; }

        public Skill? NewSkill { get; set; }
        public Specialization? NewSpec { get; set; }
    }
}

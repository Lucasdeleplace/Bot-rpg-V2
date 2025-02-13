using System.Collections.Generic;

namespace Api_Bot_RPG.Models
{
    public class SkillNode
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int CoutPoints { get; set; }
        public List<int> PrerequisSkillIds { get; set; }
        public List<SkillEffect> Effets { get; set; } = new();

        public SkillNode()
        {
            PrerequisSkillIds = new List<int>();
        }
    }
} 
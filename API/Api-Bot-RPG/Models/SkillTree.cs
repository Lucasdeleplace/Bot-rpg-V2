using System.Collections.Generic;
using Api_Bot_RPG.Enums;

namespace Api_Bot_RPG.Models
{
    public class SkillTree
    {
        public int Id { get; set; }
        public RaceType Race { get; set; }
        public ClassType Classe { get; set; }
        public List<SkillNode> Nodes { get; set; }

        public SkillTree()
        {
            Nodes = new List<SkillNode>();
        }
    }
}
namespace Api_Bot_RPG.Models
{
    public class SkillEffect
    {
        public int Id { get; set; }
        public string EffectName { get; set; }
        public int Value { get; set; }
        public int SkillNodeId { get; set; }
        public SkillNode SkillNode { get; set; }
    }
} 
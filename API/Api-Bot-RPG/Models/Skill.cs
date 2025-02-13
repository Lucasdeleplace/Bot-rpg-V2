namespace Api_Bot_RPG.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int Niveau { get; set; }
        public bool EstDebloque { get; set; }
    }
} 
namespace Api_Bot_RPG.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int Quantite { get; set; }
        public ItemType Type { get; set; }
    }
} 
using System.Text.Json.Serialization;

namespace Api_Bot_RPG.Models
{
    public class Equipment
    {
        public int Id { get; set; }  // Ajout de la clé primaire
        public int CharacterId { get; set; }  // Clé étrangère pour le lien avec Character
        public int ArmeId { get; set; }
        public int ArmureId { get; set; }
        public int CasqueId { get; set; }
        public int BottesId { get; set; }
        public int AccessoireId { get; set; }

        public Item Arme { get; set; }
        public Item Armure { get; set; }
        public Item Casque { get; set; }
        public Item Bottes { get; set; }
        public Item Accessoire { get; set; }
        
        [JsonIgnore]
        public Character Character { get; set; }  // Navigation property
    }
} 
using Api_Bot_RPG.Enums;

namespace Api_Bot_RPG.Models
{
    public class Character
    {
        public int Id { get; set; }

        // Statistiques de base
        public int PV { get; set; }
        public int PVMax { get; set; }
        public int Experience { get; set; }
        public int Niveau { get; set; }
        public int Energie { get; set; }
        public int EnergieMax { get; set; }
        public int Mana { get; set; }
        public int ManaMax { get; set; }

        // Attributs
        public int Force { get; set; }
        public int Agilite { get; set; }
        public int Defense { get; set; }
        public int Vitesse { get; set; }
        public int Chance { get; set; }

        // Caractéristiques du personnage
        public required string Race { get; set; }
        public required string Classe { get; set; }
        public int PointsDeCompetences { get; set; }

        // Système d'alignement
        public enum AlignementMoral
        {
            Bien,
            Neutre,
            Mal
        }
        public AlignementMoral Alignement { get; set; }

        // Inventaire et équipement
        public List<Item> Inventaire { get; set; }
        public Equipment Equipements { get; set; }

        // Arbre de compétences
        public List<Skill> Competences { get; set; }

        // Ajout du nom du personnage
        public required string Name { get; set; }

        public Character()
        {
            Inventaire = new List<Item>();
            Competences = new List<Skill>();
            Equipements = new Equipment();
            Name = "";
            Race = "";
            Classe = "";
        }
    }
}


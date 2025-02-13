using Microsoft.AspNetCore.Mvc;
using Api_Bot_RPG.Models;
using Api_Bot_RPG.Services;
using Api_Bot_RPG.Enums;
using Microsoft.EntityFrameworkCore;
using Api_Bot_RPG.Data;

namespace Api_Bot_RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> _characters = new List<Character>();
        private readonly LevelService _levelService;
        private readonly InventoryService _inventoryService;
        private readonly SkillTreeService _skillTreeService;
        private readonly RpgContext _context;

        public CharacterController(RpgContext context)
        {
            _levelService = new LevelService();
            _inventoryService = new InventoryService();
            _skillTreeService = new SkillTreeService();
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter(string discordId, RaceType race, ClassType classe, string name)
        {
            var player = await _context.Players.Include(p => p.Characters)
                .FirstOrDefaultAsync(p => p.DiscordId == discordId);

            if (player == null)
                return NotFound("Joueur non trouvé");

            // Créer les items de base
            var itemsDeBase = new List<Item>
            {
                new Item { Nom = "Épée rouillée", Description = "Une vieille épée", Type = ItemType.Arme, Quantite = 1 },
                new Item { Nom = "Tunique en tissu", Description = "Une tunique basique", Type = ItemType.Armure, Quantite = 1 },
                new Item { Nom = "Casque en cuir", Description = "Un casque simple", Type = ItemType.Casque, Quantite = 1 },
                new Item { Nom = "Bottes en cuir", Description = "Des bottes simples", Type = ItemType.Bottes, Quantite = 1 },
                new Item { Nom = "Amulette", Description = "Une amulette basique", Type = ItemType.Accessoire, Quantite = 1 }
            };

            _context.Items.AddRange(itemsDeBase);
            await _context.SaveChangesAsync();

            var character = new Character
            {
                Name = name,
                Race = race.ToString(),
                Classe = classe.ToString(),
                PV = 100,
                PVMax = 100,
                Energie = 100,
                EnergieMax = 100,
                Mana = 100,
                ManaMax = 100,
                Niveau = 1,
                Experience = 0,
                Force = 10,
                Agilite = 10,
                Defense = 10,
                Vitesse = 10,
                Chance = 10,
                PointsDeCompetences = 0,
                Alignement = Character.AlignementMoral.Neutre
            };

            var equipment = new Equipment
            {
                Arme = itemsDeBase[0],
                Armure = itemsDeBase[1],
                Casque = itemsDeBase[2],
                Bottes = itemsDeBase[3],
                Accessoire = itemsDeBase[4],
                Character = character
            };

            character.Equipements = equipment;
            player.Characters.Add(character);
            await _context.SaveChangesAsync();
            return Ok(character);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacter(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Inventaire)
                .Include(c => c.Equipements)
                    .ThenInclude(e => e.Arme)
                .Include(c => c.Equipements)
                    .ThenInclude(e => e.Armure)
                .Include(c => c.Equipements)
                    .ThenInclude(e => e.Casque)
                .Include(c => c.Equipements)
                    .ThenInclude(e => e.Bottes)
                .Include(c => c.Equipements)
                    .ThenInclude(e => e.Accessoire)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Personnage non trouvé");

            return Ok(character);
        }

        [HttpPut("{id}/alignement")]
        public IActionResult UpdateAlignement(int id, Character.AlignementMoral nouvelAlignement)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Personnage non trouvé");

            character.Alignement = nouvelAlignement;
            return Ok(character);
        }

        [HttpPost("{id}/experience")]
        public IActionResult AddExperience(int id, int expAmount)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Personnage non trouvé");

            character.Experience += expAmount;
            // Logique de niveau à implémenter selon vos besoins
            return Ok(character);
        }

        [HttpPost("{id}/levelup")]
        public IActionResult LevelUp(int id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Personnage non trouvé");

            if (_levelService.CheckLevelUp(character))
                return Ok(new { Message = "Niveau supérieur atteint!", Character = character });

            return BadRequest("Expérience insuffisante pour monter de niveau");
        }

        [HttpPost("{id}/inventory/add")]
        public IActionResult AddItem(int id, [FromBody] Item item)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Personnage non trouvé");

            if (_inventoryService.AddItem(character, item))
                return Ok(character.Inventaire);

            return BadRequest("Inventaire plein");
        }

        [HttpPost("{characterId}/equip/{itemId}")]
        public async Task<IActionResult> EquipItem(int characterId, int itemId)
        {
            var character = await _context.Characters
                .Include(c => c.Inventaire)
                .Include(c => c.Equipements)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
                return NotFound("Personnage non trouvé");

            var item = character.Inventaire.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return NotFound("Item non trouvé dans l'inventaire");

            // Logique d'équipement selon le type d'item
            switch (item.Type)
            {
                case ItemType.Arme:
                    character.Equipements.Arme = item;
                    break;
                case ItemType.Armure:
                    character.Equipements.Armure = item;
                    break;
                default:
                    return BadRequest("Cet item ne peut pas être équipé");
            }

            _inventoryService.RemoveItem(character, itemId, 1);
            await _context.SaveChangesAsync();
            return Ok(character.Equipements);
        }

        [HttpPost("{id}/competence")]
        public IActionResult UnlockSkill(int id, int skillId)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null)
                return NotFound("Personnage non trouvé");

            if (character.PointsDeCompetences <= 0)
                return BadRequest("Points de compétences insuffisants");

            var skill = character.Competences.FirstOrDefault(s => s.Id == skillId);
            if (skill == null)
                return NotFound("Compétence non trouvée");

            if (skill.EstDebloque)
                return BadRequest("Compétence déjà débloquée");

            skill.EstDebloque = true;
            character.PointsDeCompetences--;

            return Ok(character);
        }
    }
}

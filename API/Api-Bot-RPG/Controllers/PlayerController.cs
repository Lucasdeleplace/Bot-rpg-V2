using Microsoft.AspNetCore.Mvc;
using Api_Bot_RPG.Models;
using Api_Bot_RPG.Services;
using Api_Bot_RPG.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Api_Bot_RPG.Data;

namespace Api_Bot_RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayerController : ControllerBase
    {
        private readonly RpgContext _context;

        public PlayerController(RpgContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crée un nouveau joueur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <param name="discordId">ID Discord du joueur</param>
        /// <returns>Le joueur créé</returns>
        /// <response code="200">Joueur créé avec succès</response>
        /// <response code="400">Le joueur existe déjà</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterPlayer(string username, string discordId)
        {
            if (await _context.Players.AnyAsync(p => p.DiscordId == discordId))
            {
                return BadRequest("Un joueur avec cet ID Discord existe déjà");
            }

            var player = new Player
            {
                Username = username,
                DiscordId = discordId,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpGet("{discordId}")]
        public async Task<IActionResult> GetPlayer(string discordId)
        {
            var player = await _context.Players
                .Include(p => p.Characters)
                    .ThenInclude(c => c.Equipements)
                .Include(p => p.Characters)
                    .ThenInclude(c => c.Inventaire)
                .FirstOrDefaultAsync(p => p.DiscordId == discordId);

            if (player == null)
                return NotFound("Joueur non trouvé");

            return Ok(player);
        }

        [HttpPost("{discordId}/character")]
        public IActionResult CreateCharacter(string discordId, RaceType race, ClassType classe, string name)
        {
            var player = _context.Players.FirstOrDefault(p => p.DiscordId == discordId);
            if (player == null)
                return NotFound("Joueur non trouvé");

            if (player.Characters.Count >= player.MaxCharacters)
                return BadRequest("Nombre maximum de personnages atteint");

            var character = new Character
            {
                Id = player.Characters.Count + 1,
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

            player.Characters.Add(character);
            return Ok(character);
        }

        [HttpGet("{discordId}/characters")]
        public async Task<IActionResult> GetPlayerCharacters(string discordId)
        {
            var player = await _context.Players
                .Include(p => p.Characters)
                .FirstOrDefaultAsync(p => p.DiscordId == discordId);

            if (player == null)
                return NotFound("Joueur non trouvé");

            return Ok(player.Characters);
        }

        [HttpPut("{discordId}/lastlogin")]
        public IActionResult UpdateLastLogin(string discordId)
        {
            var player = _context.Players.FirstOrDefault(p => p.DiscordId == discordId);
            if (player == null)
                return NotFound("Joueur non trouvé");

            player.LastLogin = DateTime.UtcNow;
            return Ok(player);
        }
    }
}
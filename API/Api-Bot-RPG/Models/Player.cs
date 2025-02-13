using System;
using System.Collections.Generic;

namespace Api_Bot_RPG.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string DiscordId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        public List<Character> Characters { get; set; }
        public int MaxCharacters { get; set; } = 3; // Limite de personnages par joueur

        public Player()
        {
            Id = Guid.NewGuid().ToString();
            Characters = new List<Character>();
            CreatedAt = DateTime.UtcNow;
            LastLogin = DateTime.UtcNow;
        }
    }
} 
using System;
using System.Collections.Generic;
using System.Linq;
using Api_Bot_RPG.Models;
using Api_Bot_RPG.Enums;

namespace Api_Bot_RPG.Services
{
    public class SkillTreeService
    {
        private readonly Dictionary<(RaceType, ClassType), SkillTree> _skillTrees;

        public SkillTreeService()
        {
            _skillTrees = new Dictionary<(RaceType, ClassType), SkillTree>();
            InitializeSkillTrees();
        }

        private void InitializeSkillTrees()
        {
            // Arbre de compétences Guerrier Humain
            var guerrierHumain = new SkillTree
            {
                Id = 1,
                Race = RaceType.Humain,
                Classe = ClassType.Guerrier,
                Nodes = new List<SkillNode>
                {
                    new SkillNode
                    {
                        Id = 1,
                        Nom = "Frappe Puissante",
                        Description = "Une attaque dévastatrice",
                        CoutPoints = 1,
                        Effets = new List<SkillEffect>
                        {
                            new SkillEffect { EffectName = "Degats", Value = 50 }
                        }
                    },
                    new SkillNode
                    {
                        Id = 2,
                        Nom = "Endurance du Soldat",
                        Description = "Augmente les PV maximum de 20%",
                        CoutPoints = 2,
                        PrerequisSkillIds = new List<int> { 1 },
                        Effets = new List<SkillEffect>
                        {
                            new SkillEffect { EffectName = "PVMax", Value = 20 }
                        }
                    },
                    new SkillNode
                    {
                        Id = 3,
                        Nom = "Cri de Guerre",
                        Description = "Augmente la Force de tous les alliés de 15%",
                        CoutPoints = 3,
                        PrerequisSkillIds = new List<int> { 2 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "Force", Value = 15 }, new SkillEffect { EffectName = "ZoneEffet", Value = 1 } }
                    }
                }
            };

            // Arbre de compétences Mage Elfe
            var mageElfe = new SkillTree
            {
                Id = 2,
                Race = RaceType.Elfe,
                Classe = ClassType.Mage,
                Nodes = new List<SkillNode>
                {
                    new SkillNode
                    {
                        Id = 1,
                        Nom = "Projectile Arcanique",
                        Description = "Lance un projectile magique qui inflige des dégâts de mana",
                        CoutPoints = 1,
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "DegatsMagiques", Value = 40 } }
                    },
                    new SkillNode
                    {
                        Id = 2,
                        Nom = "Méditation Elfique",
                        Description = "Régénère du mana plus rapidement",
                        CoutPoints = 2,
                        PrerequisSkillIds = new List<int> { 1 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "RegenerationMana", Value = 30 } }
                    },
                    new SkillNode
                    {
                        Id = 3,
                        Nom = "Nova Arcanique",
                        Description = "Explosion de magie qui étourdit les ennemis",
                        CoutPoints = 3,
                        PrerequisSkillIds = new List<int> { 2 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "DegatsMagiques", Value = 60 }, new SkillEffect { EffectName = "Etourdissement", Value = 2 } }
                    }
                }
            };

            // Arbre de compétences Voleur Demi-Elfe
            var voleurDemiElfe = new SkillTree
            {
                Id = 3,
                Race = RaceType.DemiElfe,
                Classe = ClassType.Voleur,
                Nodes = new List<SkillNode>
                {
                    new SkillNode
                    {
                        Id = 1,
                        Nom = "Attaque Sournoise",
                        Description = "Attaque dans le dos pour des dégâts critiques",
                        CoutPoints = 1,
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "DegatsAssassins", Value = 70 } }
                    },
                    new SkillNode
                    {
                        Id = 2,
                        Nom = "Pas de l'Ombre",
                        Description = "Permet de se déplacer furtivement",
                        CoutPoints = 2,
                        PrerequisSkillIds = new List<int> { 1 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "Furtivite", Value = 40 } }
                    },
                    new SkillNode
                    {
                        Id = 3,
                        Nom = "Danse des Lames",
                        Description = "Série d'attaques rapides qui saignent l'ennemi",
                        CoutPoints = 3,
                        PrerequisSkillIds = new List<int> { 2 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "Saignement", Value = 30 }, new SkillEffect { EffectName = "VitesseAttaque", Value = 50 } }
                    }
                }
            };

            // Arbre de compétences Prêtre Nain
            var pretreNain = new SkillTree
            {
                Id = 4,
                Race = RaceType.Nain,
                Classe = ClassType.Pretre,
                Nodes = new List<SkillNode>
                {
                    new SkillNode
                    {
                        Id = 1,
                        Nom = "Bénédiction de Pierre",
                        Description = "Augmente la défense de la cible",
                        CoutPoints = 1,
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "Defense", Value = 40 } }
                    },
                    new SkillNode
                    {
                        Id = 2,
                        Nom = "Guérison Ancestrale",
                        Description = "Soigne les blessures et augmente la régénération",
                        CoutPoints = 2,
                        PrerequisSkillIds = new List<int> { 1 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "Soin", Value = 50 }, new SkillEffect { EffectName = "RegenerationVie", Value = 20 } }
                    },
                    new SkillNode
                    {
                        Id = 3,
                        Nom = "Marteau Sanctifié",
                        Description = "Frappe divine qui étourdit et soigne les alliés",
                        CoutPoints = 3,
                        PrerequisSkillIds = new List<int> { 2 },
                        Effets = new List<SkillEffect> { new SkillEffect { EffectName = "DegatsSacres", Value = 40 }, new SkillEffect { EffectName = "SoinZone", Value = 30 } }
                    }
                }
            };

            // Ajout des arbres de compétences au dictionnaire
            _skillTrees.Add((RaceType.Humain, ClassType.Guerrier), guerrierHumain);
            _skillTrees.Add((RaceType.Elfe, ClassType.Mage), mageElfe);
            _skillTrees.Add((RaceType.DemiElfe, ClassType.Voleur), voleurDemiElfe);
            _skillTrees.Add((RaceType.Nain, ClassType.Pretre), pretreNain);
        }

        public SkillTree? GetSkillTree(RaceType race, ClassType classe)
        {
            if (_skillTrees.TryGetValue((race, classe), out var skillTree))
                return skillTree;

            return new SkillTree { Nodes = new List<SkillNode>() };
        }

        public List<SkillNode> GetAvailableSkills(Character character)
        {
            var skillTree = GetSkillTree(Enum.Parse<RaceType>(character.Race), Enum.Parse<ClassType>(character.Classe));
            if (skillTree == null) return new List<SkillNode>();

            return skillTree.Nodes.Where(node =>
                !character.Competences.Any(c => c.Id == node.Id) &&
                node.PrerequisSkillIds.All(preReqId =>
                    character.Competences.Any(c => c.Id == preReqId && c.EstDebloque))
            ).ToList();
        }
    }
}
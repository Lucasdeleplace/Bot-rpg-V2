    public class LevelService
    {
        private readonly Dictionary<int, int> _experienceRequise;

        public LevelService()
        {
            _experienceRequise = new Dictionary<int, int>();
            InitializeExperienceTable();
        }

        private void InitializeExperienceTable()
        {
            for (int level = 1; level <= 100; level++)
            {
                _experienceRequise[level] = (int)(100 * Math.Pow(level, 1.5));
            }
        }

        public bool CheckLevelUp(Character character)
        {
            if (character.Niveau >= 100) return false;

            int expRequise = _experienceRequise[character.Niveau];
            if (character.Experience >= expRequise)
            {
                LevelUp(character);
                return true;
            }
            return false;
        }

        private void LevelUp(Character character)
        {
            character.Niveau++;
            character.Experience -= _experienceRequise[character.Niveau - 1];
            character.PointsDeCompetences += 5;
            
            // Augmentation des stats de base
            character.PVMax += 20;
            character.PV = character.PVMax;
            character.ManaMax += 10;
            character.Mana = character.ManaMax;
            character.EnergieMax += 15;
            character.Energie = character.EnergieMax;
        }
    }

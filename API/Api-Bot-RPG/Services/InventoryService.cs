    public class InventoryService
    {
        private const int INVENTORY_MAX_SIZE = 50;

        public bool AddItem(Character character, Item item)
        {
            if (character.Inventaire.Count >= INVENTORY_MAX_SIZE)
                return false;

            var existingItem = character.Inventaire.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantite += item.Quantite;
            }
            else
            {
                character.Inventaire.Add(item);
            }
            return true;
        }

        public bool RemoveItem(Character character, int itemId, int quantity)
        {
            var item = character.Inventaire.FirstOrDefault(i => i.Id == itemId);
            if (item == null || item.Quantite < quantity)
                return false;

            item.Quantite -= quantity;
            if (item.Quantite <= 0)
                character.Inventaire.Remove(item);

            return true;
        }
    }

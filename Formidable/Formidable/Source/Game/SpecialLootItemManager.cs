using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formidable.Game
{

    public static class SpecialLootItemManager
    {

        public static List<string> SpecialLootItems
        {
            get => _specialLootItems;
        }

        private static bool _isInitialized;

        private static List<string> _specialLootItems;

        static SpecialLootItemManager()
        {
            _isInitialized = false;
            _specialLootItems = null;
        }

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            _specialLootItems = File.ReadAllLines(Settings.SpecialLootItemsFilePath)
                .Select((string q) => (q.ToLowerInvariant()))
                .ToList();

            _isInitialized = true;
        }

        public static bool IsSpecialLootItem(LootItem lootItem)
        {
            if ((lootItem == null) || (lootItem.Item == null))
                return false;

            if (!_isInitialized)
                return false;

            string formattedLootItemName = lootItem.Item.Name.Localized().ToLowerInvariant();
            string formattedLootItemShortName = lootItem.Item.ShortName.Localized().ToLowerInvariant();

            return (_specialLootItems.Contains(formattedLootItemName) || _specialLootItems.Contains(formattedLootItemShortName));
        }

    }

}

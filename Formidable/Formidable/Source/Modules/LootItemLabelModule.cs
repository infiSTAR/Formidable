using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT.Interactive;
using JsonType;
using UnityEngine;
using Formidable.Data;
using Formidable.Drawing;
using Formidable.Game;

namespace Formidable.Modules
{

    public class LootItemLabelModule : Module
    {

        public static readonly KeyCode _KeyCode = KeyCode.Keypad9;

        private static readonly float _renderDistance = 200f;
        private static readonly Color _specialColor = new Color(1f, 0.2f, 0.09f);
        private static readonly Color _questColor = Color.yellow;
        private static readonly Color _commonColor = Color.white;
        private static readonly Color _rareColor = new Color(0.38f, 0.43f, 1f);
        private static readonly Color _superRareColor = new Color(1f, 0.29f, 0.36f);

        public LootItemLabelState LootItemLabelState
        {
            get => this.lootItemLabelState;
        }

        private LootItemLabelState lootItemLabelState;

        public LootItemLabelModule(ModuleInformation moduleInformation) : base(moduleInformation)
        {
            this.lootItemLabelState = LootItemLabelState.Disabled;
        }

        public override void Toggle()
        {
            if (((int) this.lootItemLabelState) == (Enum.GetNames(typeof(LootItemLabelState)).Length - 1))
            {
                this.lootItemLabelState = LootItemLabelState.Disabled;
            }
            else
            {
                this.lootItemLabelState++;
            }
        }

        public override void OnGUI()
        {
            if (this.lootItemLabelState == LootItemLabelState.Disabled)
                return;

            foreach (GameLootItem gameLootItem in base.moduleInformation.GameLootItems)
            {
                if (!gameLootItem.IsOnScreen || (gameLootItem.Distance > _renderDistance))
                    continue;

                bool isSpecialLootItem = SpecialLootItemManager.IsSpecialLootItem(gameLootItem.LootItem);

                if ((this.lootItemLabelState == LootItemLabelState.Special) && !isSpecialLootItem)
                    continue;
                else if ((this.lootItemLabelState == LootItemLabelState.GameRare) && (gameLootItem.LootItem.Item.Template.Rarity < ELootRarity.Rare))
                    continue;
                else if ((this.lootItemLabelState == LootItemLabelState.GameSuperRare) && (gameLootItem.LootItem.Item.Template.Rarity < ELootRarity.Superrare))
                    continue;

                string lootItemName = $"{gameLootItem.LootItem.Item.ShortName.Localized()} [{gameLootItem.FormattedDistance}]";
                Color lootItemColor = _commonColor;

                if (isSpecialLootItem)
                    lootItemColor = _specialColor;
                else if (gameLootItem.LootItem.Item.QuestItem)
                    lootItemColor = _questColor;
                else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Rare)
                    lootItemColor = _rareColor;
                else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Superrare)
                    lootItemColor = _superRareColor;

                DrawManager.DrawShadowedText((gameLootItem.ScreenPosition.x - 50f), gameLootItem.ScreenPosition.y, lootItemName, lootItemColor, TextStyle.Small);
            }

            base.OnGUI();
        }

    }

}

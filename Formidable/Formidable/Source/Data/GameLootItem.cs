using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT.Interactive;
using Formidable.Util;
using UnityEngine;

namespace Formidable.Data
{

    public class GameLootItem
    {

        public LootItem LootItem
        {
            get => this.lootItem;
        }

        public Vector3 ScreenPosition
        {
            get => this.screenPosition;
        }

        public bool IsOnScreen
        {
            get => this.isOnScreen;
        }

        public float Distance
        {
            get => this.distance;
        }

        public string FormattedDistance
        {
            get => $"{Math.Round(this.distance, 2)}m";
        }

        private LootItem lootItem;

        private Vector3 screenPosition;

        private bool isOnScreen;
        private float distance;

        public GameLootItem(LootItem lootItem)
        {
            if (lootItem == null)
                throw new ArgumentNullException(nameof(lootItem));

            this.lootItem = lootItem;
            this.screenPosition = default;
            this.distance = 0f;
        }

        public void RecalculateDynamics()
        {
            if (!GameUtil.IsLootItemValid(this.lootItem))
                return;

            this.screenPosition = GameUtil.WorldPointToScreenPoint(this.lootItem.transform.position);

            this.isOnScreen = GameUtil.IsScreenPointVisible(this.screenPosition);
            this.distance = Vector3.Distance(Camera.main.transform.position, this.lootItem.transform.position);
        }

    }

}

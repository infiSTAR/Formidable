using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using EFT.Interactive;
using UnityEngine;

namespace Formidable.Util
{

    public static class GameUtil
    {

        static GameUtil()
        {

        }

        public static bool IsPlayerValid(Player player)
        {
            return ((player != null) && (player.Transform != null) && (player.PlayerBones != null) && (player.PlayerBones.transform != null));
        }

        public static bool IsLootItemValid(LootItem lootItem)
        {
            return ((lootItem != null) && (lootItem.Item != null) && (lootItem.Item.Template != null));
        }

        public static bool IsPlayerAlive(Player player)
        {
            if (!IsPlayerValid(player))
                return false;

            if (player.HealthController == null)
                return false;

            return player.HealthController.IsAlive;
        }

        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);

            screenPoint.y = (Screen.height - screenPoint.y);

            return screenPoint;
        }

        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return ((screenPoint.z > 0.01f) && (screenPoint.x > -5f) && (screenPoint.y > -5f) && (screenPoint.x < Screen.width) && (screenPoint.y < Screen.height));
        }

    }

}

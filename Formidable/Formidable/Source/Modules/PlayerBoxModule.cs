using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Formidable.Data;
using Formidable.Drawing;
using Formidable.Util;

namespace Formidable.Modules
{

    public class PlayerBoxModule : Module
    {

        public static readonly KeyCode _KeyCode = KeyCode.Keypad7;

        private static readonly float _renderDistance = 300f;
        private static readonly Color _playerColor = new Color(1f, 0.388f, 0.341f);
        private static readonly Color _botColor = new Color(1f, 0.968f, 0.349f);
        private static readonly Color _healthColor = Color.green;

        public bool IsActivated
        {
            get => this.isActivated;
        }

        private bool isActivated;

        public PlayerBoxModule(ModuleInformation moduleInformation) : base(moduleInformation)
        {
            this.isActivated = false;
        }

        public override void Toggle()
        {
            this.isActivated = !this.isActivated;
        }

        public override void OnGUI()
        {
            if (!this.isActivated)
                return;

            foreach (GamePlayer gamePlayer in base.moduleInformation.GamePlayers)
            {
                if (!gamePlayer.IsOnScreen || (gamePlayer.Distance > _renderDistance))
                    continue;

                Color playerColor = ((gamePlayer.IsAI) ? _botColor : _playerColor);

                float boxPositionY = (gamePlayer.HeadScreenPosition.y - 10f);
                float boxHeight = (Math.Abs(gamePlayer.HeadScreenPosition.y - gamePlayer.ScreenPosition.y) + 10f);
                float boxWidth = (boxHeight * 0.65f);

                DrawManager.DrawRectangle((gamePlayer.ScreenPosition.x - (boxWidth / 2f)), boxPositionY, boxWidth, boxHeight, playerColor);

                if (gamePlayer.Player.HealthController.IsAlive)
                {
                    float currentPlayerHealth = gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
                    float maximumPlayerHealth = gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common).Maximum;

                    float healthBarHeight = MathUtil.Map(currentPlayerHealth, 0f, maximumPlayerHealth, 0f, boxHeight);

                    DrawManager.DrawLine(new Vector2((gamePlayer.ScreenPosition.x - (boxWidth / 2f) - 3f), (boxPositionY + boxHeight - healthBarHeight)), new Vector2((gamePlayer.ScreenPosition.x - (boxWidth / 2f) - 3f), (boxPositionY + boxHeight)), _healthColor, 3f);
                }
            }

            base.OnGUI();
        }


    }

}

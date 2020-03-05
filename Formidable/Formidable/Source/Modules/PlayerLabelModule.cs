using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Formidable.Data;
using Formidable.Drawing;

namespace Formidable.Modules
{

    public class PlayerLabelModule : Module
    {

        public static readonly KeyCode _KeyCode = KeyCode.Keypad8;

        private static readonly float _renderDistance = 300f;
        private static readonly Color _playerColor = new Color(1f, 0.388f, 0.341f);
        private static readonly Color _botColor = new Color(1f, 0.968f, 0.349f);

        public bool IsActivated
        {
            get => this.isActivated;
        }

        private bool isActivated;

        public PlayerLabelModule(ModuleInformation moduleInformation) : base(moduleInformation)
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

                string playerName = null;
                Color playerColor = default;

                if (gamePlayer.IsAI)
                {
                    playerName = $"BOT [{gamePlayer.FormattedDistance}]";
                    playerColor = _botColor;
                }
                else
                {
                    playerName = $"{gamePlayer.Player.Profile.Info.Nickname} [{gamePlayer.FormattedDistance}]";
                    playerColor = _playerColor;
                }

                Vector2 textSize = DrawManager.CalculateTextSize(playerName, TextStyle.Small);

                DrawManager.DrawShadowedText((gamePlayer.ScreenPosition.x - (textSize.x / 2f)), (gamePlayer.HeadScreenPosition.y - 30f), playerName, playerColor, TextStyle.Small);
            }

            base.OnGUI();
        }

    }

}

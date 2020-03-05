using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using Formidable.Util;

namespace Formidable.Data
{

    public class GamePlayer
    {

        public Player Player
        {
            get => this.player;
        }

        public Vector3 ScreenPosition
        {
            get => this.screenPosition;
        }

        public Vector3 HeadScreenPosition
        {
            get => this.headScreenPosition;
        }

        public bool IsOnScreen
        {
            get => this.isOnScreen;
        }

        public float Distance
        {
            get => this.distance;
        }

        public bool IsAI
        {
            get => this.isAI;
        }

        public string FormattedDistance
        {
            get => $"{Math.Round(this.distance, 2)}m";
        }

        private Player player;

        private Vector3 screenPosition;
        private Vector3 headScreenPosition;

        private bool isOnScreen;
        private float distance;
        private bool isAI;

        public GamePlayer(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            this.player = player;
            this.screenPosition = default;
            this.headScreenPosition = default;
            this.isOnScreen = false;
            this.distance = 0f;
            this.isAI = true;
        }

        public void RecalculateDynamics()
        {
            if (!GameUtil.IsPlayerValid(player))
                return;

            this.screenPosition = GameUtil.WorldPointToScreenPoint(this.player.Transform.position);

            if (this.player.PlayerBones != null)
                this.headScreenPosition = GameUtil.WorldPointToScreenPoint(this.player.PlayerBones.Head.position);

            this.isOnScreen = GameUtil.IsScreenPointVisible(this.screenPosition);
            this.distance = Vector3.Distance(Camera.main.transform.position, this.player.Transform.position);

            if ((this.player.Profile != null) && (this.player.Profile.Info != null))
                this.isAI = (this.player.Profile.Info.RegistrationDate <= 0);
        }

    }

}

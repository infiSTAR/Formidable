using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using UnityEngine;
using Formidable.Data;
using Formidable.Drawing;
using Formidable.Game;
using Formidable.Modules;
using Formidable.Util;
using EFT.Interactive;

namespace Formidable
{

    public class FormidableBehaviour : MonoBehaviourSingleton<FormidableBehaviour>
    {

        private static readonly float _cacheLootItemsInterval = 2.5f;

        private static readonly float _shaderUpdateInterval = 5f;

        private static readonly float _maximumPlayerDistance = 400f;
        private static readonly float _maximumLootItemDistance = 200f;

        private static readonly float _cachePlayersInterval = 1f;
        private static readonly Color _playerColor = Color.green;
        private static readonly Color _botColor = new Color(1f, 0.968f, 0.349f);

        private float nextPlayerCacheTime;
        private float nextLootItemCacheTime;

        private List<GamePlayer> gamePlayers;
        private List<GameLootItem> gameLootItems;

        private ModuleInformation moduleInformation;

        private PlayerBoxModule playerBoxModule;
        private PlayerLabelModule playerLabelModule;
        private LootItemLabelModule lootItemLabelModule;

        private Shader shader;
        private float nextShaderUpdateTime;

        public FormidableBehaviour() : base()
        {
            this.nextPlayerCacheTime = 0f;
            this.nextLootItemCacheTime = 0f;
            this.gamePlayers = new List<GamePlayer>();
            this.gameLootItems = new List<GameLootItem>();
            this.moduleInformation = new ModuleInformation(this.gamePlayers, this.gameLootItems);
            this.playerBoxModule = new PlayerBoxModule(this.moduleInformation);
            this.playerLabelModule = new PlayerLabelModule(this.moduleInformation);
            this.lootItemLabelModule = new LootItemLabelModule(this.moduleInformation);
            this.shader = null;
            this.nextShaderUpdateTime = 0f;
        }

        private static Shader LoadShader()
        {
            return AssetBundle
                .LoadFromFile(Settings.FormidableAssetBundleFilePath)
                .LoadAsset<Shader>(Settings.ShaderFilePath);
        }

        public void Start()
        {
            this.shader = LoadShader();

            DrawManager.Initialize();
            SpecialLootItemManager.Initialize();
            ModuleManager.Initialize();

            ModuleManager.AddModule(PlayerBoxModule._KeyCode, this.playerBoxModule);
            ModuleManager.AddModule(PlayerLabelModule._KeyCode, this.playerLabelModule);
            ModuleManager.AddModule(LootItemLabelModule._KeyCode, this.lootItemLabelModule);
        }

        public void Update()
        {
            if (Time.time >= this.nextPlayerCacheTime)
            {
                GameWorld gameWorld = Singleton<GameWorld>.Instance;

                if ((gameWorld != null) && (gameWorld.RegisteredPlayers != null))
                {
                    this.gamePlayers.Clear();

                    foreach (Player player in gameWorld.RegisteredPlayers)
                    {
                        if (!GameUtil.IsPlayerAlive(player) || player.IsYourPlayer() || (Vector3.Distance(Camera.main.transform.position, player.Transform.position) > _maximumPlayerDistance))
                            continue;

                        this.gamePlayers.Add(new GamePlayer(player));
                    }

                    this.nextPlayerCacheTime = (Time.time + _cachePlayersInterval);
                }
            }

            if (Time.time >= this.nextLootItemCacheTime)
            {
                GameWorld gameWorld = Singleton<GameWorld>.Instance;

                if ((gameWorld != null) && (gameWorld.LootItems != null))
                {
                    this.gameLootItems.Clear();

                    for (int i = 0; i < gameWorld.LootItems.Count; i++)
                    {
                        LootItem lootItem = gameWorld.LootItems.GetByIndex(i);

                        if (!GameUtil.IsLootItemValid(lootItem) || (Vector3.Distance(Camera.main.transform.position, lootItem.transform.position) > _maximumLootItemDistance))
                            continue;

                        this.gameLootItems.Add(new GameLootItem(lootItem));
                    }

                    this.nextLootItemCacheTime = (Time.time + _cacheLootItemsInterval);
                }
            }

            foreach (GamePlayer gamePlayer in this.gamePlayers)
                gamePlayer.RecalculateDynamics();

            foreach (GameLootItem gameLootItem in this.gameLootItems)
                gameLootItem.RecalculateDynamics();

            ModuleManager.Update();

            if (Time.time >= this.nextShaderUpdateTime)
            {
                if (this.gamePlayers.Count > 0)
                {
                    foreach (GamePlayer gamePlayer in this.gamePlayers)
                    {
                        if (!GameUtil.IsPlayerAlive(gamePlayer.Player) || gamePlayer.Player.IsYourPlayer())
                            continue;

                        Color shaderColor = ((gamePlayer.IsAI) ? _botColor : _playerColor);

                        foreach (EBodyModelPart bodyModelPart in Enum.GetValues(typeof(EBodyModelPart)))
                        {
                            foreach (Renderer renderer in gamePlayer.Player.PlayerBody.BodySkins[bodyModelPart].GetRenderers())
                            {
                                Material material = renderer.material;
                                Shader shader = material.shader;

                                if ((shader.name == this.shader.name))
                                    continue;

                                material.shader = this.shader;

                                material.SetColor("_FirstOutlineColor", Color.red);
                                material.SetFloat("_FirstOutlineWidth", 0.02f);
                                material.SetColor("_SecondOutlineColor", shaderColor);
                                material.SetFloat("_SecondOutlineWidth", 0.0025f);
                            }
                        }
                    }

                    this.nextShaderUpdateTime = (Time.time + _shaderUpdateInterval);
                }
            }
        }

        public void OnGUI()
        {
            DrawManager.InitializeResources();

            DrawManager.DrawShadowedText(12f, 10f, this.GenerateApplicationBar(), Color.white, TextStyle.Normal);

            ModuleManager.OnGUI();
        }

        private string GenerateApplicationBar()
        {
            StringBuilder applicationBarStringBuilder = new StringBuilder();

            applicationBarStringBuilder.Append($"{Settings.ApplicationName}");

            if (ModuleManager.IsActivated)
            {
                applicationBarStringBuilder.Append(" | ");
                applicationBarStringBuilder.Append($"PlayerBox: {this.playerBoxModule.IsActivated}, ");
                applicationBarStringBuilder.Append($"PlayerLabel: {this.playerLabelModule.IsActivated}, ");
                applicationBarStringBuilder.Append($"LootItemLabel: {this.lootItemLabelModule.LootItemLabelState}");
            }

            return applicationBarStringBuilder.ToString();
        }

    }

}

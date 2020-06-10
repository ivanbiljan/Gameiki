using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Gameiki.Extensions;
using Gameiki.Framework;
using Gameiki.Framework.Commands;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Terraria;

namespace Gameiki {
    internal sealed class Gameiki {
        public Gameiki() {
            Hooks.Chat += OnChat;
            Hooks.GameInitialized += OnGameInitialized;
            Hooks.PostUpdate += OnPostUpdate;
            Hooks.PreHurt += OnPreHurt;
            Hooks.ResetEffects += OnResetPlayerEffects;
        }

        public static Player MyPlayer => Main.player[Main.myPlayer];

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Run(string[] args) {
            Terraria.Program.LaunchGame(args);
        }

        private void Help(Match match) {
            var output = new StringBuilder();
            foreach (var command in CommandManager.Instance.Commands) {
                if (string.IsNullOrWhiteSpace(command.HelpText)) {
                    continue;
                }

                output.AppendLine(command.HelpText);
            }

            MyPlayer.SendGameikiMessage("Available commands:", Color.LimeGreen);
            MyPlayer.SendGameikiMessage(output.ToString(), Color.Yellow);
        }

        private void OnChat(object sender, ChatEventArgs e) {
            if (!e.Text.StartsWith(".")) {
                return;
            }

            try {
                CommandManager.Instance.RunCommand(e.Text.Substring(1));
                e.Handled = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnGameInitialized(object sender, EventArgs e) {
            Main.versionNumber += "\nGameiki Remaster v1.0. Powered by Mono.Cecil";

            // Setup
            MyPlayer.SetData("session", new Session());

            // Initialize the GUI
            //Toolbar.Instance.Initialize();

            // Register commands
            CommandManager.Instance.RegisterCommand(new Regex("help"), Help, ".help - Lists all commands");
            CommandManager.Instance.RegisterCommand(new Regex(@"(?:item|i)(.*)"), SpawnItem,
                ".item <name or id> - Spawns the item");
        }

        private void OnPostUpdate(object sender, EventArgs e) {
            if (Main.mapFullscreen && Main.mouseRight && Main.mouseRightRelease) {
                var targetLocation = new Vector2(
                    Main.mapFullscreenPos.X + (Main.mouseX - Main.screenWidth / 2) * 0.06255F *
                    (16 / Main.mapFullscreenScale),
                    Main.mapFullscreenPos.Y + (Main.mouseY - Main.screenHeight / 2) * 0.06255F *
                    (16 / Main.mapFullscreenScale));
                Main.player[Main.myPlayer].Teleport(new Vector2(targetLocation.X * 16, targetLocation.Y * 16), 1);
            }

            var session = MyPlayer.GetData<Session>("session");
            if (!session.IsFullbright) {
                return;
            }

            
            // var tilesX = Main.screenWidth / 16 + Lighting.OffScreenTiles * 2;
            // var tilesY = Main.screenHeight / 16 + Lighting.OffScreenTiles * 2;
            // for (var i = 0; i < tilesX; ++i) {
            //     var lightingStates = Lighting[i];
            //     for (var j = 0; j < tilesY; ++j) {
            //         var state = lightingStates[j];
            //         state.r = state.r2 = state.g = state.g2 = state.b = state.b2 = 1f;
            //     }
            // }
        }

        private void OnPreHurt(object sender, HandledEventArgs e) {
            var session = MyPlayer.GetData<Session>("session");
            if (!session.IsGodmode) {
                return;
            }

            e.Handled = true;
        }

        private void OnResetPlayerEffects(object sender, EventArgs e) {
            var session = MyPlayer.GetData<Session>("session");
            if (!session.IsGodmode) {
                return;
            }

            // Max breath
            MyPlayer.breath = MyPlayer.breathMax - 1;

            // Infinite range
            Player.tileRangeX = Main.maxTilesX;
            Player.tileRangeY = Main.maxTilesY;

            // Max hp/mana stats
            MyPlayer.statMana = MyPlayer.statManaMax2;

            // Infinite wing time
            MyPlayer.wings = MyPlayer.wingsLogic = 29; // Solar wings
            MyPlayer.carpetTime = MyPlayer.rocketTime = 2;
            MyPlayer.wingTime = 2;

            // Cellphone
            MyPlayer.accCompass = 1;
            MyPlayer.accDepthMeter = 1;
            MyPlayer.accWatch = 3;
            MyPlayer.accFishFinder = true;
            MyPlayer.accWeatherRadio = true;
            MyPlayer.accCalendar = true;
            MyPlayer.accThirdEye = true;
            MyPlayer.accJarOfSouls = true;
            MyPlayer.accCritterGuide = true;
            MyPlayer.accStopwatch = true;
            MyPlayer.accOreFinder = true;
            MyPlayer.accDreamCatcher = true;

            // Builder accessory
            MyPlayer.InfoAccMechShowWires = true;

            // Debuff immunity
            for (var i = 0; i < MyPlayer.buffImmune.Length; ++i) {
                MyPlayer.buffImmune[i] = true;
            }
        }

        private void SpawnItem(Match match) {
            var itemName = match.Groups[1].Value.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(itemName)) {
                MyPlayer.SendGameikiMessage("Invalid syntax. Proper syntax: .item <name or id>", Color.Red);
                return;
            }

            var item = new Item();
            var items = new List<Item>();
            if (!int.TryParse(itemName, out var netId)) {
                for (var i = 0; i < Main.maxItemTypes; ++i) {
                    item.SetDefaults(i);
                    if (item.Name.StartsWith(itemName, StringComparison.OrdinalIgnoreCase)) {
                        items.Add(item);
                    }

                    if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)) {
                        items = new List<Item> {item};
                        break;
                    }
                }

                if (items.Count == 0) {
                    MyPlayer.SendGameikiMessage("No results found.");
                }

                if (items.Count > 1) {
                    MyPlayer.SendGameikiMessage($"Found multiple matches: {string.Join(", ", items.Select(i => i.Name))}", Color.Yellow);
                    return;
                }

                item = items[0];
                item.stack = item.maxStack;
            }

            item.SetDefaults(netId);
            MyPlayer.GetItem(Main.myPlayer, item, GetItemSettings.InventoryUIToInventorySettings);
            MyPlayer.SendGameikiMessage($"Spawned {item.Name} (x{item.stack})", Color.LimeGreen);
        }
    }
}
using System;
using Gameiki.Extensions;
using Gameiki.Framework;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Gameiki {
    internal sealed class Gameiki {
        public Gameiki() {
            Patcher.Events.Hooks.GameInitialized += OnGameInitialized;
            Patcher.Events.Hooks.PostUpdate += OnPostUpdate;
            Patcher.Events.Hooks.ResetEffects += OnResetPlayerEffects;
        }

        public static Player MyPlayer => Main.player[Main.myPlayer];

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Run(string[] args) {
            Terraria.Program.LaunchGame(args);
        }

        private void OnGameInitialized(object sender, EventArgs e) {
            Main.versionNumber += "\nGameiki Remaster v1.0. Powered by Mono.Cecil";

            // Setup
            MyPlayer.SetData("session", new Session());

            // Initialize the GUI
            Toolbar.Instance.Initialize();
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
            MyPlayer.statLife = MyPlayer.statLifeMax2;
            MyPlayer.statMana = MyPlayer.statManaMax2;

            // Infinite wing time
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
            
            var tilesX = Main.screenWidth / 16 + Lighting.offScreenTiles * 2;
            var tilesY = Main.screenHeight / 16 + Lighting.offScreenTiles * 2;
            for (var i = 0; i < tilesX; ++i)
            {
                var lightingStates = Lighting.states[i];
                for (var j = 0; j < tilesY; ++j)
                {
                    var state = lightingStates[j];
                    state.r = state.r2 = state.g = state.g2 = state.b = state.b2 = 1f;
                }
            }
        }
    }
}
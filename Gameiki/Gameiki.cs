using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Gameiki.Extensions;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Light;

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

        private void OnChat(object sender, ChatEventArgs e) {
            // TODO
        }

        private void OnGameInitialized(object sender, EventArgs e) {
            // Misc
            Main.versionNumber += "\nGameiki Remaster v1.0. Powered by Mono.Cecil";

            // Setup
            MyPlayer.SetData("session", new Session());
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
            
            for (var x = 0; x < Lighting.NewEngine._workingLightMap.Width; ++x) {
                for (var y = 0; y < Lighting.NewEngine._workingLightMap.Height; ++y) {
                    Lighting.NewEngine._workingLightMap[x, y] = new Vector3(255f, 255f, 255f);
                }
            }
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
    }
}
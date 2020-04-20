using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gameiki.Framework;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Gameiki {
    internal sealed class Gameiki {
        private readonly ISet<UserInterfaceItem> _uiItems = new HashSet<UserInterfaceItem>();
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public static Player MyPlayer => Main.player[Main.myPlayer];

        public Gameiki() {
            Hooks.GameInitialized += GameInitialized;
            Hooks.PostDraw += PostDraw;
            Hooks.PostUpdate += PostUpdate;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Run(string[] args) {
            Terraria.Program.LaunchGame(args);
        }

        private void GameInitialized(object sender, EventArgs e) {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(UserInterfaceItem)))) {
                _uiItems.Add((UserInterfaceItem) Activator.CreateInstance(type));
            }

            Main.versionNumber += "\nGameiki Remaster v1.0. Powered by Mono.Cecil";
        }

        private void PostDraw(object sender, DrawEventArgs e) {
            Main.spriteBatch.Begin();
            foreach (var item in _uiItems) {
                item.Draw(sender, e);
            }

            Main.spriteBatch.End();
        }

        private void PostUpdate(object sender, EventArgs e) {
            if (Main.mapFullscreen && _currentMouseState.RightButton == ButtonState.Released &&
                _previousMouseState.RightButton == ButtonState.Pressed) {
                var targetLocation = new Vector2(
                    Main.mapFullscreenPos.X + (Main.mouseX - Main.screenWidth / 2) * 0.06255F *
                    (16 / Main.mapFullscreenScale),
                    Main.mapFullscreenPos.Y + (Main.mouseY - Main.screenHeight / 2) * 0.06255F *
                    (16 / Main.mapFullscreenScale));
                Main.player[Main.myPlayer].Teleport(new Vector2(targetLocation.X * 16, targetLocation.Y * 16), 1);
            }

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
    }
}
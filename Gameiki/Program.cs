using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using TMain = Terraria.Main;

namespace Gameiki {
    internal static class Program {
        private static Texture2D GeneralTexture;
        
        private static Assembly _terrariaAssembly;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // Handle assembly resolution
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            
            // Setup event handlers
            Hooks.PostUpdate += PostUpdate;
            Hooks.PostDraw += PostDraw;

            // Run Terraria
            RunTerraria();
        }

        private static void PostDraw(object sender, DrawEventArgs e) {
            Terraria.Main.spriteBatch.Begin();
            GeneralTexture = new Texture2D(Terraria.Main.instance.GraphicsDevice, 1, 1);
            GeneralTexture.SetData(new[] {Color.White});
            Terraria.Main.spriteBatch.Draw(GeneralTexture, new Vector2(80, TMain.screenHeight - 50), new Rectangle(0, 0, 1000, 20),
                Color.Aqua);
            Terraria.Main.spriteBatch.End();
        }

        private static void RunTerraria() {
            Terraria.Program.LaunchGame(new string[0]);
        }

        private static void PostUpdate(object sender, EventArgs e) {
            if (Terraria.Main.mapFullscreen && _currentMouseState.RightButton == ButtonState.Released &&
                _previousMouseState.RightButton == ButtonState.Pressed)
            {
                var targetLocation = new Vector2(
                    Terraria.Main.mapFullscreenPos.X + (Terraria.Main.mouseX - Terraria.Main.screenWidth / 2) * 0.06255F *
                    (16 / Terraria.Main.mapFullscreenScale),
                    Terraria.Main.mapFullscreenPos.Y + (Terraria.Main.mouseY - Terraria.Main.screenHeight / 2) * 0.06255F *
                    (16 / Terraria.Main.mapFullscreenScale));
                Terraria.Main.player[Terraria.Main.myPlayer].Teleport(new Vector2(targetLocation.X * 16, targetLocation.Y * 16), 1);
            }
            
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args) {
            var assemblyName = new AssemblyName(args.Name);
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == assemblyName.Name);
            if (assembly != null) {
                return assembly;
            }
            
            if (assemblyName.Name == "Terraria") {
                return _terrariaAssembly = Assembly.LoadFrom("patched.exe");
            }

            var resource = _terrariaAssembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(assemblyName.Name + ".dll"));
            if (resource == null) {
                return null;
            }

            using (var stream = _terrariaAssembly.GetManifestResourceStream(resource)) {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Assembly.Load(buffer);
            }
        }
    }
}
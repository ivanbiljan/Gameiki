using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using XnaGui;
using XnaGui.Extensions;

namespace GameikiUI.Playground {
    public class Game : Microsoft.Xna.Framework.Game {
        private readonly List<Control> _controls = new List<Control>();
        private readonly GraphicsDeviceManager _graphics;
        private DynamicSpriteFont _arialFont;
        private BasicEffect _effect;
        private SpriteBatch _spriteBatch;

        public Game() {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280, 
                PreferredBackBufferHeight = 720, 
                PreferMultiSampling = true
            };

            IsMouseVisible = true;
            Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _controls.Add(new CheckBox(null, 10, 10, 50, 50));
        }

        protected override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            _spriteBatch.DrawRoundRectangle(new Rectangle(100, 100, 250, 100), 12, Color.Red);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Initialize() {
            base.Initialize();
            XnaGui.XnaGui.Initialize(this);
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _arialFont = Content.Load<DynamicSpriteFont>("Fonts\\Mouse_Text");
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            _controls[0].Update(gameTime);
        }
    }
}
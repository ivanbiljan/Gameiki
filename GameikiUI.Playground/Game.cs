using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using XnaGui;
using XnaGui.Extensions;

namespace GameikiUI.Playground {
    public class Game : Microsoft.Xna.Framework.Game {
        private readonly GraphicsDeviceManager _graphics;
        private DynamicSpriteFont _arialFont;
        private SpriteBatch _spriteBatch;
        private CheckBox _checkBox;

        public Game() {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280, 
                PreferredBackBufferHeight = 720, 
                PreferMultiSampling = true
            };

            IsMouseVisible = true;
            Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        protected override void Draw(GameTime gameTime) {
            _checkBox.Draw(gameTime, _spriteBatch);
            _spriteBatch.Begin();
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
            _checkBox = new CheckBox(null, 100, 100, 100, 25, "Hellogy", _arialFont) {
                TextAlign = TextAlign.BottomCenter
            };
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}
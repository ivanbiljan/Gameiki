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
        }

        protected override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            _spriteBatch.FillCircle(new Vector2(200, 200), 50, Color.Green);
            _spriteBatch.DrawEllipse(new Vector2(100, 100), 50, 25, Color.Red);
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
        }
    }
}
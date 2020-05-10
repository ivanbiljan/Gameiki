using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using XnaGui;

namespace GameikiUI.Playground {
    public class Game : Microsoft.Xna.Framework.Game {
        private DynamicSpriteFont _arialFont;

        private readonly List<Control> _controls = new List<Control>();
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game() {
            _graphics = new GraphicsDeviceManager(this) {PreferredBackBufferWidth = 1280, PreferredBackBufferHeight = 720};

            IsMouseVisible = true;
            Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        protected override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            foreach (var control in _controls) {
                control.Draw(gameTime, _spriteBatch);
            }

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

            _controls.Add(new TextBox(null, 100, 200, 50, 25, _arialFont) {ForegroundColor = Color.Black, PlaceholderText = "Hello"});
            _controls.Add(new Button(null, 100, 150, 30, 25, _arialFont, "Hello")
                {BackgroundColor = Color.Yellow, ForegroundColor = Color.Gray, Padding = 5});
            _controls.Add(new Label(null, 100, 100, _arialFont, "Hello, World") {ForegroundColor = Color.White});
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            foreach (var control in _controls) {
                control.Update(gameTime);
            }

            base.Update(gameTime);
        }
    }
}
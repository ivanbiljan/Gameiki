using System;
using System.Windows.Forms;
using Gameiki.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using XnaGui;
using Label = XnaGui.Label;

namespace GameikiUI.Playground {
    public class Game : Microsoft.Xna.Framework.Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private DynamicSpriteFont _arialFont;

        private Label _label;
        private XnaGui.Button _button;

        public Game() {
            _graphics = new GraphicsDeviceManager(this) {PreferredBackBufferWidth = 1280, PreferredBackBufferHeight = 720};

            IsMouseVisible = true;
            Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        protected override void Initialize() {
            base.Initialize();
            XnaGui.XnaGui.Initialize(this);
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _arialFont = Content.Load<DynamicSpriteFont>("Fonts\\Mouse_Text");
            
            _label = new Label(null, 100, 100, _arialFont, "Hello, World") {ForegroundColor = Color.White};
            _button = new XnaGui.Button(null, 100, 150, 50, 25, _arialFont, "Hello")
                {BackgroundColor = Color.Yellow, ForegroundColor = Color.Gray, Padding = 5};
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            _label.Draw(gameTime, _spriteBatch);
            _button.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EniacWar;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Renderer _renderer;
    private AudioEngine _audioEngine;
    private SpriteFont _mainFont;
    private ScreenManager _screenManager;

    private Color _backgroundColor = Color.Black;
    private Color _gridColor = new Color(0, 30, 0); 

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        Window.Title = "ENIAC WAR";
        Window.Position = new Point(
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 1280) / 2,
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 720) / 2
        );

        _audioEngine = new AudioEngine();
        _screenManager = new ScreenManager(_audioEngine);
        _screenManager.ExitCommand = () => Exit();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _renderer = new Renderer(GraphicsDevice);
        _mainFont = Content.Load<SpriteFont>("MainFont");
        
        var introScreen = new IntroScreen();
        _screenManager.ChangeScreen(introScreen);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

        _screenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_backgroundColor);

        _renderer.Begin();
        int gridSize = 40;
        for (int x = 0; x <= _graphics.PreferredBackBufferWidth; x += gridSize)
        {
            _renderer.DrawLine(new Vector2(x, 0), new Vector2(x, _graphics.PreferredBackBufferHeight), _gridColor, 1f);
        }
        for (int y = 0; y <= _graphics.PreferredBackBufferHeight; y += gridSize)
        {
            _renderer.DrawLine(new Vector2(0, y), new Vector2(_graphics.PreferredBackBufferWidth, y), _gridColor, 1f);
        }
        _renderer.End();

        _screenManager.Draw(gameTime, _renderer, _mainFont, _graphics);

        base.Draw(gameTime);
    }
}

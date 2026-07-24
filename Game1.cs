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

    private string _fullTitle = "ENIAC WAR";
    private int _visibleLetters = 0;
    private double _timeSinceLastLetter = 0;
    private double _typingDelay = 0.5;
    private Random _random = new Random();
    
    private Color _backgroundColor = Color.Black;
    private Color _textColor = new Color(50, 255, 50); 
    private Color _gridColor = new Color(0, 30, 0); 

    private double _cursorTimer = 0;
    private bool _cursorVisible = true;
    private bool _titleFinished = false;

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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _renderer = new Renderer(GraphicsDevice);
        _mainFont = Content.Load<SpriteFont>("MainFont");
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kState = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
            Exit();

        if (!_titleFinished)
        {
            _timeSinceLastLetter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastLetter >= _typingDelay)
            {
                if (_visibleLetters < _fullTitle.Length)
                {
                    _visibleLetters++;
                    if (_fullTitle[_visibleLetters - 1] != ' ')
                    {
                        _audioEngine.PlayTypingSound();
                    }
                    _typingDelay = 0.15 + _random.NextDouble() * 0.4;
                }
                else
                {
                    _titleFinished = true;
                }
                _timeSinceLastLetter = 0;
            }
        }

        _cursorTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_cursorTimer > 0.5)
        {
            _cursorVisible = !_cursorVisible;
            _cursorTimer = 0;
        }

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

        string currentText = _fullTitle.Substring(0, _visibleLetters);
        Vector2 textSize = _mainFont.MeasureString(_fullTitle);
        Vector2 position = new Vector2(
            (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
            (_graphics.PreferredBackBufferHeight - textSize.Y) / 2
        );

        _renderer.DrawString(_mainFont, currentText, position, _textColor);

        if (_cursorVisible)
        {
            if (!_titleFinished)
            {
                Vector2 currentTextSize = _mainFont.MeasureString(currentText);
                Vector2 charSize = _mainFont.MeasureString("A");
                Vector2 cursorPosition = new Vector2(position.X + currentTextSize.X + 5, position.Y + charSize.Y * 0.15f);
                _renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }

        if (_titleFinished)
        {
            string prompt = "Appuyer sur une touche";
            float scalePrompt = 0.5f;
            Vector2 promptSize = _mainFont.MeasureString(prompt) * scalePrompt;
            Vector2 promptPos = new Vector2(
                (_graphics.PreferredBackBufferWidth - promptSize.X) / 2,
                _graphics.PreferredBackBufferHeight - 150
            );

            if (_cursorVisible)
            {
                _renderer.DrawString(_mainFont, prompt, promptPos, _textColor, scalePrompt);
                
                Vector2 charSize = _mainFont.MeasureString("A") * scalePrompt;
                Vector2 cursorPosition = new Vector2(promptPos.X + promptSize.X + 5, promptPos.Y + charSize.Y * 0.15f);
                _renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }

        _renderer.End();

        base.Draw(gameTime);
    }
}

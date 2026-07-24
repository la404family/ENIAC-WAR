using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EniacWar;

public class IntroScreen : IScreen
{
    private string _fullTitle = "";
    private int _visibleLetters = 0;
    private double _timeSinceLastLetter = 0;
    private double _typingDelay = 0.5;
    private Random _random = new Random();
    
    private Color _textColor = new Color(50, 255, 50); 

    private double _cursorTimer = 0;
    private bool _cursorVisible = true;
    private bool _titleFinished = false;

    public void Initialize()
    {
        _fullTitle = LocalizationManager.GetString("TITLE");
        _visibleLetters = 0;
        _timeSinceLastLetter = 0;
        _typingDelay = 0.5;
        _titleFinished = false;
    }

    public void Update(GameTime gameTime, ScreenManager screenManager)
    {
        KeyboardState kState = Keyboard.GetState();

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
                        screenManager.Audio.PlayTypingSound();
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
        else
        {
            if (kState.GetPressedKeys().Length > 0)
            {
                screenManager.ChangeScreen(new MenuScreen());
            }
        }

        _cursorTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_cursorTimer > 0.5)
        {
            _cursorVisible = !_cursorVisible;
            _cursorTimer = 0;
        }
    }

    public void Draw(GameTime gameTime, Renderer renderer, SpriteFont font, GraphicsDeviceManager graphics)
    {
        renderer.Begin();

        string currentText = _fullTitle.Substring(0, _visibleLetters);
        Vector2 textSize = font.MeasureString(_fullTitle);
        Vector2 position = new Vector2(
            (graphics.PreferredBackBufferWidth - textSize.X) / 2,
            (graphics.PreferredBackBufferHeight - textSize.Y) / 2
        );

        renderer.DrawString(font, currentText, position, _textColor);

        if (_cursorVisible)
        {
            if (!_titleFinished)
            {
                Vector2 currentTextSize = font.MeasureString(currentText);
                Vector2 charSize = font.MeasureString("A");
                Vector2 cursorPosition = new Vector2(position.X + currentTextSize.X + 5, position.Y + charSize.Y * 0.15f);
                renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }

        if (_titleFinished)
        {
            string prompt = LocalizationManager.GetString("PRESS_ANY_KEY");
            float scalePrompt = 0.5f;
            Vector2 promptSize = font.MeasureString(prompt) * scalePrompt;
            Vector2 promptPos = new Vector2(
                (graphics.PreferredBackBufferWidth - promptSize.X) / 2,
                graphics.PreferredBackBufferHeight - 150
            );

            if (_cursorVisible)
            {
                renderer.DrawString(font, prompt, promptPos, _textColor, scalePrompt);
                
                Vector2 charSize = font.MeasureString("A") * scalePrompt;
                Vector2 cursorPosition = new Vector2(promptPos.X + promptSize.X + 5, promptPos.Y + charSize.Y * 0.15f);
                renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }

        renderer.End();
    }
}

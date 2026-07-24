using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EniacWar;

public class OptionsScreen : IScreen
{
    private string _title = "";
    
    private int _selectedIndex = 0;
    private Color _textColor = new Color(50, 255, 50); 
    private Color _modalBgColor = new Color(0, 20, 0, 230);

    private double _cursorTimer = 0;
    private bool _cursorVisible = true;
    
    private bool _keyDown = false;

    private bool _initialized = false;

    private int _resIndex = 0;
    private readonly (int w, int h, string name)[] _resolutions = {
        (1024, 768, "4:3"),
        (1280, 720, "16:9"),
        (1280, 800, "16:10"),
        (1920, 1080, "16:9"),
        (1920, 1200, "16:10")
    };
    
    private bool _isFullscreen = false;
    private int _langIndex = 0;

    private enum State { Normal, Confirmation }
    private State _currentState = State.Normal;
    private double _confirmationTimer = 10.0;
    private int _oldResIndex;
    private bool _oldFullscreen;
    
    private string[] _optionsText = new string[4] { "", "", "", "" };

    public void Initialize()
    {
    }

    public void Update(GameTime gameTime, ScreenManager screenManager)
    {
        if (!_initialized)
        {
            _isFullscreen = SettingsManager.Settings.IsFullScreen;
            int cw = SettingsManager.Settings.ResolutionWidth;
            int ch = SettingsManager.Settings.ResolutionHeight;
            
            _resIndex = 1; 
            for(int i=0; i<_resolutions.Length; i++)
            {
                if (_resolutions[i].w == cw && _resolutions[i].h == ch)
                {
                    _resIndex = i;
                    break;
                }
            }

            string currentLang = LocalizationManager.CurrentLanguage;
            for(int i = 0; i < LocalizationManager.SupportedLanguages.Length; i++)
            {
                if(LocalizationManager.SupportedLanguages[i] == currentLang)
                {
                    _langIndex = i;
                    break;
                }
            }
            _initialized = true;
        }

        KeyboardState kState = Keyboard.GetState();

        if (_currentState == State.Normal)
        {
            UpdateNormalState(kState, screenManager);
        }
        else if (_currentState == State.Confirmation)
        {
            UpdateConfirmationState(kState, screenManager, gameTime);
        }

        _cursorTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (_cursorTimer > 0.5)
        {
            _cursorVisible = !_cursorVisible;
            _cursorTimer = 0;
        }
        
        // Update strings
        _title = LocalizationManager.GetString("MENU_OPTIONS");
        _optionsText[0] = $"{LocalizationManager.GetString("OPT_RESOLUTION")}{_resolutions[_resIndex].w}x{_resolutions[_resIndex].h} ({_resolutions[_resIndex].name})";
        _optionsText[1] = $"{LocalizationManager.GetString("OPT_FULLSCREEN")}{(_isFullscreen ? LocalizationManager.GetString("YES") : LocalizationManager.GetString("NO"))}";
        _optionsText[2] = $"{LocalizationManager.GetString("OPT_LANGUAGE")}{LocalizationManager.SupportedLanguages[_langIndex]}";
        _optionsText[3] = LocalizationManager.GetString("OPT_BACK");
    }

    private void UpdateNormalState(KeyboardState kState, ScreenManager screenManager)
    {
        if (kState.IsKeyDown(Keys.Up) && !_keyDown)
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = 3;
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
        }
        else if (kState.IsKeyDown(Keys.Down) && !_keyDown)
        {
            _selectedIndex++;
            if (_selectedIndex > 3) _selectedIndex = 0;
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
        }
        else if (kState.IsKeyDown(Keys.Left) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            if (_selectedIndex < 3)
            {
                ChangeValue(-1, screenManager);
            }
        }
        else if (kState.IsKeyDown(Keys.Right) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            if (_selectedIndex < 3)
            {
                ChangeValue(1, screenManager);
            }
        }
        else if (kState.IsKeyDown(Keys.Enter) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            
            if (_selectedIndex == 3)
            {
                SaveLanguageSettings();
                screenManager.ChangeScreen(new MenuScreen());
            }
            else if (_selectedIndex == 2)
            {
                ChangeValue(1, screenManager);
            }
            else
            {
                TriggerConfirmation(screenManager.Graphics);
            }
        }
        else if (kState.GetPressedKeys().Length == 0)
        {
            _keyDown = false;
        }
    }

    private void UpdateConfirmationState(KeyboardState kState, ScreenManager screenManager, GameTime gameTime)
    {
        _confirmationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
        
        if ((kState.IsKeyDown(Keys.Enter) || kState.IsKeyDown(Keys.Y) || kState.IsKeyDown(Keys.O)) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            ConfirmSettings(screenManager.Graphics);
        }
        else if ((kState.IsKeyDown(Keys.Escape) || kState.IsKeyDown(Keys.N)) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            RevertSettings(screenManager.Graphics);
        }
        else if (kState.GetPressedKeys().Length == 0)
        {
            _keyDown = false;
        }

        if (_confirmationTimer <= 0)
        {
            RevertSettings(screenManager.Graphics);
        }
    }

    private void ChangeValue(int dir, ScreenManager screenManager)
    {
        if (_selectedIndex == 0)
        {
            _resIndex += dir;
            if (_resIndex < 0) _resIndex = _resolutions.Length - 1;
            if (_resIndex >= _resolutions.Length) _resIndex = 0;
        }
        else if (_selectedIndex == 1)
        {
            _isFullscreen = !_isFullscreen;
        }
        else if (_selectedIndex == 2)
        {
            _langIndex += dir;
            if (_langIndex < 0) _langIndex = LocalizationManager.SupportedLanguages.Length - 1;
            if (_langIndex >= LocalizationManager.SupportedLanguages.Length) _langIndex = 0;
            
            LocalizationManager.CurrentLanguage = LocalizationManager.SupportedLanguages[_langIndex];
            SaveLanguageSettings(); 
        }
    }

    private void SaveLanguageSettings()
    {
        SettingsManager.Settings.Language = LocalizationManager.CurrentLanguage;
        SettingsManager.Save();
    }

    private void TriggerConfirmation(GraphicsDeviceManager graphics)
    {
        int cw = graphics.PreferredBackBufferWidth;
        int ch = graphics.PreferredBackBufferHeight;
        
        _oldResIndex = 1; 
        for(int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].w == cw && _resolutions[i].h == ch)
            {
                _oldResIndex = i;
                break;
            }
        }
        _oldFullscreen = graphics.IsFullScreen;

        ApplyToGraphics(graphics, _resolutions[_resIndex].w, _resolutions[_resIndex].h, _isFullscreen);
        _currentState = State.Confirmation;
        _confirmationTimer = 10.0;
    }

    private void ConfirmSettings(GraphicsDeviceManager graphics)
    {
        SettingsManager.Settings.ResolutionWidth = _resolutions[_resIndex].w;
        SettingsManager.Settings.ResolutionHeight = _resolutions[_resIndex].h;
        SettingsManager.Settings.IsFullScreen = _isFullscreen;
        SettingsManager.Save();
        
        _currentState = State.Normal;
    }

    private void RevertSettings(GraphicsDeviceManager graphics)
    {
        _resIndex = _oldResIndex;
        _isFullscreen = _oldFullscreen;
        ApplyToGraphics(graphics, _resolutions[_resIndex].w, _resolutions[_resIndex].h, _isFullscreen);
        _currentState = State.Normal;
    }

    private void ApplyToGraphics(GraphicsDeviceManager graphics, int w, int h, bool fullscreen)
    {
        if (graphics == null) return;
        
        graphics.PreferredBackBufferWidth = w;
        graphics.PreferredBackBufferHeight = h;
        graphics.IsFullScreen = fullscreen;
        graphics.ApplyChanges();
    }

    public void Draw(GameTime gameTime, Renderer renderer, SpriteFont font, GraphicsDeviceManager graphics)
    {
        renderer.Begin();

        Vector2 titleSize = font.MeasureString(_title);
        Vector2 titlePos = new Vector2(
            (graphics.PreferredBackBufferWidth - titleSize.X) / 2,
            150
        );

        renderer.DrawString(font, _title, titlePos, _textColor);

        float scaleOption = 0.5f;
        float startY = 350;

        for (int i = 0; i < _optionsText.Length; i++)
        {
            Vector2 optionSize = font.MeasureString(_optionsText[i]) * scaleOption;
            Vector2 optionPos = new Vector2(
                (graphics.PreferredBackBufferWidth - optionSize.X) / 2,
                startY + i * 60
            );

            renderer.DrawString(font, _optionsText[i], optionPos, _textColor, scaleOption);

            if (_currentState == State.Normal && i == _selectedIndex && _cursorVisible)
            {
                Vector2 charSize = font.MeasureString("A") * scaleOption;
                Vector2 cursorPosition = new Vector2(optionPos.X - charSize.X - 20, optionPos.Y + charSize.Y * 0.15f);
                renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }
        
        if (_currentState == State.Confirmation)
        {
            renderer.FillRectangle(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), _modalBgColor);
            
            string modalText = LocalizationManager.GetString("MODAL_CONFIRM");
            string revertingText = LocalizationManager.GetString("MODAL_REVERTING").Replace("{0}", Math.Ceiling(_confirmationTimer).ToString());
            string yesText = LocalizationManager.GetString("YES") + " (O/Y/Enter)";
            string noText = LocalizationManager.GetString("NO") + " (N/Esc)";

            Vector2 mSize = font.MeasureString(modalText) * 0.5f;
            Vector2 rSize = font.MeasureString(revertingText) * 0.4f;
            Vector2 ySize = font.MeasureString(yesText) * 0.4f;
            Vector2 nSize = font.MeasureString(noText) * 0.4f;

            float modalY = graphics.PreferredBackBufferHeight / 2 - 50;

            renderer.DrawString(font, modalText, new Vector2((graphics.PreferredBackBufferWidth - mSize.X) / 2, modalY), _textColor, 0.5f);
            renderer.DrawString(font, revertingText, new Vector2((graphics.PreferredBackBufferWidth - rSize.X) / 2, modalY + 50), _textColor, 0.4f);
            
            renderer.DrawString(font, yesText, new Vector2(graphics.PreferredBackBufferWidth / 2 - ySize.X - 40, modalY + 120), _textColor, 0.4f);
            renderer.DrawString(font, noText, new Vector2(graphics.PreferredBackBufferWidth / 2 + 40, modalY + 120), _textColor, 0.4f);
        }

        renderer.End();
    }
}

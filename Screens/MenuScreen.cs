using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EniacWar;

public class MenuScreen : IScreen
{
    private string _title = "";
    private string[] _options = new string[6];
    private int _selectedIndex = 0;
    
    private Color _textColor = new Color(50, 255, 50); 

    private double _cursorTimer = 0;
    private bool _cursorVisible = true;
    
    private bool _keyDown = false;

    public void Initialize()
    {
        _title = LocalizationManager.GetString("TITLE");
        _options[0] = LocalizationManager.GetString("MENU_ENIAC_SOLO");
        _options[1] = LocalizationManager.GetString("MENU_ENIAC_HOTE");
        _options[2] = LocalizationManager.GetString("MENU_ENIAC_CLIENT");
        _options[3] = LocalizationManager.GetString("MENU_OPTIONS");
        _options[4] = LocalizationManager.GetString("MENU_CREDITS");
        _options[5] = LocalizationManager.GetString("MENU_EXIT");
    }

    public void Update(GameTime gameTime, ScreenManager screenManager)
    {
        KeyboardState kState = Keyboard.GetState();

        if (kState.IsKeyDown(Keys.Up) && !_keyDown)
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = _options.Length - 1;
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
        }
        else if (kState.IsKeyDown(Keys.Down) && !_keyDown)
        {
            _selectedIndex++;
            if (_selectedIndex >= _options.Length) _selectedIndex = 0;
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
        }
        else if (kState.IsKeyDown(Keys.Enter) && !_keyDown)
        {
            _keyDown = true;
            screenManager.Audio.PlayTypingSound();
            
            if (_selectedIndex == 3)
            {
                screenManager.ChangeScreen(new OptionsScreen());
            }
            else if (_selectedIndex == 5)
            {
                screenManager.ExitCommand?.Invoke();
            }
        }
        else if (kState.GetPressedKeys().Length == 0)
        {
            _keyDown = false;
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

        Vector2 titleSize = font.MeasureString(_title);
        Vector2 titlePos = new Vector2(
            (graphics.PreferredBackBufferWidth - titleSize.X) / 2,
            100
        );

        renderer.DrawString(font, _title, titlePos, _textColor);

        float scaleOption = 0.45f;
        float startY = 250;

        for (int i = 0; i < _options.Length; i++)
        {
            Vector2 optionSize = font.MeasureString(_options[i]) * scaleOption;
            Vector2 optionPos = new Vector2(
                (graphics.PreferredBackBufferWidth - optionSize.X) / 2,
                startY + i * 50
            );

            renderer.DrawString(font, _options[i], optionPos, _textColor, scaleOption);

            if (i == _selectedIndex && _cursorVisible)
            {
                Vector2 charSize = font.MeasureString("A") * scaleOption;
                Vector2 cursorPosition = new Vector2(optionPos.X - charSize.X - 20, optionPos.Y + charSize.Y * 0.15f);
                renderer.FillRectangle(new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)charSize.X, (int)(charSize.Y * 0.7f)), _textColor);
            }
        }

        renderer.End();
    }
}

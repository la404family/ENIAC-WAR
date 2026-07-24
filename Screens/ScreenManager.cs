using Microsoft.Xna.Framework;

namespace EniacWar;

public class ScreenManager
{
    private IScreen _currentScreen;
    private IScreen _nextScreen;
    private float _transitionAlpha = 0f;
    private bool _isTransitioning = false;
    private bool _isFadingOut = false;
    private float _fadeSpeed = 1f / 0.4f;
    
    private readonly AudioEngine _audioEngine;
    
    public AudioEngine Audio { get { return _audioEngine; } }
    public System.Action ExitCommand { get; set; }

    public ScreenManager(AudioEngine audioEngine)
    {
        _audioEngine = audioEngine;
    }

    public void ChangeScreen(IScreen nextScreen)
    {
        _nextScreen = nextScreen;
        _isTransitioning = true;
        _isFadingOut = true;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isTransitioning)
        {
            if (_isFadingOut)
            {
                _transitionAlpha += _fadeSpeed * dt;
                if (_transitionAlpha >= 1f)
                {
                    _transitionAlpha = 1f;
                    _isFadingOut = false;
                    _currentScreen = _nextScreen;
                    _currentScreen?.Initialize();
                }
            }
            else
            {
                _transitionAlpha -= _fadeSpeed * dt;
                if (_transitionAlpha <= 0f)
                {
                    _transitionAlpha = 0f;
                    _isTransitioning = false;
                }
            }
        }
        else
        {
            _currentScreen?.Update(gameTime, this);
        }
    }

    public void Draw(GameTime gameTime, Renderer renderer, Microsoft.Xna.Framework.Graphics.SpriteFont font, GraphicsDeviceManager graphics)
    {
        _currentScreen?.Draw(gameTime, renderer, font, graphics);

        if (_isTransitioning)
        {
            renderer.Begin();
            renderer.FillRectangle(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black * _transitionAlpha);
            renderer.End();
        }
    }
}

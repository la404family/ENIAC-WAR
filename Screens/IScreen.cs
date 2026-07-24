using Microsoft.Xna.Framework;

namespace EniacWar;

public interface IScreen
{
    void Initialize();
    void Update(GameTime gameTime, ScreenManager screenManager);
    void Draw(GameTime gameTime, Renderer renderer, Microsoft.Xna.Framework.Graphics.SpriteFont font, GraphicsDeviceManager graphics);
}

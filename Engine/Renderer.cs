using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EniacWar;

public class Renderer
{
    private readonly Texture2D _pixel;
    private readonly SpriteBatch _spriteBatch;

    public Renderer(GraphicsDevice graphicsDevice)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void Begin()
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
    }

    public void End()
    {
        _spriteBatch.End();
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Color color, float thickness = 2f)
    {
        Vector2 delta = p2 - p1;
        float angle = (float)Math.Atan2(delta.Y, delta.X);
        float length = delta.Length();
        _spriteBatch.Draw(_pixel, p1, null, color, angle, new Vector2(0, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0f);
    }

    public void FillRectangle(Rectangle rect, Color color)
    {
        _spriteBatch.Draw(_pixel, rect, color);
    }

    public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f)
    {
        _spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}

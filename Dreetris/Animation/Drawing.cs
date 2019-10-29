using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Animation
{
    public static class Drawing
    {
        public static Texture2D blank;

        public static void Initialize(GraphicsDevice gd)
        {
            blank = new Texture2D(gd, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }

        public static void DrawLongLine(SpriteBatch spriteBatch, List<Vector2> sublines, Color color)
        {
            for (int i = 0; i < sublines.Count - 1; i++)
            {
                DrawLine(spriteBatch, 1, color, sublines[i], sublines[i + 1]);
            }
        }

        public static void DrawLine(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}

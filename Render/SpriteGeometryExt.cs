using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Render
{
    static class SpriteGeometryExt
    {
        public static void SetupLine(this Sprite sprite, int color,
            float x, float y, float halfLen, float rotation)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f;
            sprite.OriginY = 0.5f;
            sprite.Left = x;
            sprite.Top = y;
            sprite.ScaleX = halfLen * 2;
            sprite.ScaleY = 1;
            sprite.Rotation = rotation;
        }

        private static void SetupLineDistance(this Sprite sprite, int color,
            float x, float y, float distanceX, float distanceY, float halfLenX, float halfLenY, float rotation)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f + distanceX;
            sprite.OriginY = 0.5f + distanceY;
            sprite.Left = x;
            sprite.Top = y;
            sprite.ScaleX = halfLenX * 2;
            sprite.ScaleY = halfLenY * 2;
            sprite.Rotation = rotation;
        }

        public static void SetupRect(this Sprite[] spriteArray, int color,
            float x, float y, float halfWidth, float halfHeight, float rotation)
        {
            spriteArray[0].SetupLineDistance(color, x, y, 0, halfHeight, halfWidth + 0.5f, 0.5f, rotation);
            spriteArray[1].SetupLineDistance(color, x, y, halfWidth, 0, 0.5f, halfHeight + 0.5f, rotation);
            spriteArray[2].SetupLineDistance(color, x, y, 0, -halfHeight, halfWidth + 0.5f, 0.5f, rotation);
            spriteArray[3].SetupLineDistance(color, x, y, -halfWidth, 0, 0.5f, halfHeight + 0.5f, rotation);
        }

        public static void Render(this Sprite[] sprites)
        {
            foreach (var s in sprites)
            {
                s.Render();
            }
        }

        public static void Setup(this Sprite sprite,
            Texture texture,
            float Left = 0, float Top = 0,
            float OriginX = 0, float OriginY = 0,
            float Rotation = 0,
            float ScaleX = 1, float ScaleY = 1
            )
        {
            sprite.Texture = texture;
            sprite.Left = Left;
            sprite.Top = Top;
            sprite.OriginX = OriginX;
            sprite.OriginY = OriginY;
            sprite.Rotation = Rotation;
            sprite.ScaleX = ScaleX;
            sprite.ScaleY = ScaleY;
        }
    }
}

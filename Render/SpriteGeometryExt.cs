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
        public static void SetupPosition(this Sprite sprite, float x, float y, float rotation, float rotation0 = 0)
        {
            sprite.Left = x;
            sprite.Top = y;
            sprite.Rotation = rotation;
            sprite.Rotation0 = rotation0;
        }

        public static void SetupLine(this Sprite sprite, int color, float halfLen)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f;
            sprite.OriginY = 0.5f;
            sprite.ScaleX = halfLen * 2;
            sprite.ScaleY = 1;
        }

        private static void SetupLineDistance(this Sprite sprite, int color,
            float distanceX, float distanceY, float halfLenX, float halfLenY)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f + distanceX;
            sprite.OriginY = 0.5f + distanceY;
            sprite.ScaleX = halfLenX * 2;
            sprite.ScaleY = halfLenY * 2;
        }

        public static void SetupRect(this Sprite[] spriteArray, int color, float halfWidth, float halfHeight)
        {
            spriteArray[0].SetupLineDistance(color, 0, halfHeight, halfWidth + 0.5f, 0.5f);
            spriteArray[1].SetupLineDistance(color, halfWidth, 0, 0.5f, halfHeight + 0.5f);
            spriteArray[2].SetupLineDistance(color, 0, -halfHeight, halfWidth + 0.5f, 0.5f);
            spriteArray[3].SetupLineDistance(color, -halfWidth, 0, 0.5f, halfHeight + 0.5f);
        }

        public static void Setup(this Sprite sprite,
            Texture texture,
            float OriginX = 0, float OriginY = 0,
            float ScaleX = 1, float ScaleY = 1
            )
        {
            sprite.Texture = texture;
            sprite.OriginX = OriginX;
            sprite.OriginY = OriginY;
            sprite.ScaleX = ScaleX;
            sprite.ScaleY = ScaleY;
        }

        public static void Render(this Sprite[] sprites)
        {
            foreach (var s in sprites)
            {
                s.Render();
            }
        }

        public static void SetupPosition(this Sprite[] sprites, float x, float y, float rotation, float rotation0 = 0)
        {
            foreach (var s in sprites)
            {
                s.SetupPosition(x, y, rotation, rotation0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Render
{
    class SpriteGeometry
    {
        public static void SetupLine(int color, Sprite sprite,
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

        private static void SetupLineDistance(int color, Sprite sprite,
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

        public static void SetupRect(int color, Sprite[] spriteArray,
            float x, float y, float halfWidth, float halfHeight, float rotation)
        {
            SetupLineDistance(color, spriteArray[0], x, y, 0, halfHeight, halfWidth, 0.5f, rotation);
            SetupLineDistance(color, spriteArray[1], x, y, halfWidth, 0, 0.5f, halfHeight, rotation);
            SetupLineDistance(color, spriteArray[2], x, y, 0, -halfHeight, halfWidth, 0.5f, rotation);
            SetupLineDistance(color, spriteArray[3], x, y, -halfWidth, 0, 0.5f, halfHeight, rotation);
        }
    }
}

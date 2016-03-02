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
            sprite.Rotation = rotation + sprite.RotationOffset;
            sprite.Rotation0 = rotation0;
        }

        public static void SetRotationOffset(this Sprite sprite, int n)
        {
            sprite.RotationOffset = 3.1415926f / 2 * n;
        }

        public static void SetupLine(this Sprite sprite, uint color, float halfLen)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f;
            sprite.OriginY = 0.5f;
            sprite.SizeX = halfLen;
            sprite.SizeY = 1;
        }

        public static void SetupDashLine(this Sprite sprite, uint color1, float halfLen, float dashLength)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color1, 0xFF000000);
            sprite.OriginX = 1.0f;
            sprite.OriginY = 0.5f;
            sprite.SizeX = halfLen;
            sprite.SizeY = 1;
            sprite.RepeatX = halfLen / dashLength;
        }

        private static void SetupLineDistance(this Sprite sprite, uint color, float distanceY, float halfLenX)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color);
            sprite.OriginX = 0.5f + 0;
            sprite.OriginY = 0.5f + distanceY;
            sprite.SizeX = halfLenX * 2;
            sprite.SizeY = 0.5f * 2;
        }

        private static void SetupDashLineDistance(this Sprite sprite, uint color, float distanceY, float halfLenX,
            float dashLength)
        {
            sprite.Texture = sprite.RenderEngine.GetColorTexture(color, 0xFF000000);
            sprite.OriginX = 1 + 0;
            sprite.OriginY = 0.5f + distanceY;
            sprite.SizeX = halfLenX * 2;
            sprite.SizeY = 0.5f * 2;
            sprite.RepeatX = halfLenX / dashLength;
        }

        public static void SetupRect(this Sprite[] spriteArray, uint color, float halfWidth, float halfHeight)
        {
            spriteArray[0].SetupLineDistance(color, halfHeight, halfWidth + 0.5f);
            spriteArray[1].SetupLineDistance(color, halfWidth, halfHeight + 0.5f);
            spriteArray[2].SetupLineDistance(color, halfHeight, halfWidth + 0.5f);
            spriteArray[3].SetupLineDistance(color, halfWidth, halfHeight + 0.5f);
            spriteArray.SetupRectRotationOffset();
        }

        public static void SetupDashRect(this Sprite[] spriteArray, uint color, float halfWidth, float halfHeight,
            float dashLength)
        {
            spriteArray[0].SetupDashLineDistance(color, halfHeight, halfWidth + 0.5f, dashLength);
            spriteArray[1].SetupDashLineDistance(color, halfWidth, halfHeight + 0.5f, dashLength);
            spriteArray[2].SetupDashLineDistance(color, halfHeight, halfWidth + 0.5f, dashLength);
            spriteArray[3].SetupDashLineDistance(color, halfWidth, halfHeight + 0.5f, dashLength);
            spriteArray.SetupRectRotationOffset();
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

        public static void SetupRectRotationOffset(this Sprite[] spriteArray)
        {
            spriteArray[0].SetRotationOffset(0);
            spriteArray[1].SetRotationOffset(1);
            spriteArray[2].SetRotationOffset(2);
            spriteArray[3].SetRotationOffset(3);
        }
    }
}

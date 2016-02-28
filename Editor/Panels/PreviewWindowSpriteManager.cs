using GS_PatEditor.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels
{
    class PreviewWindowSpriteManager
    {
        private readonly PreviewWindow _Window;
        private List<Sprite> _SpriteList = new List<Sprite>();
        private List<Sprite[]> _RectangleList = new List<Sprite[]>();

        public PreviewWindowSpriteManager(PreviewWindow window)
        {
            _Window = window;
        }

        public void ResetAll()
        {

        }

        public Sprite GetSprite(int index)
        {
            while (index >= _SpriteList.Count)
            {
                _SpriteList.Add(_Window.Render.GetSprite());
            }
            return _SpriteList[index];
        }

        public Sprite[] GetRectangle(int index)
        {
            while (index >= _RectangleList.Count)
            {
                _RectangleList.Add(new[]
                {
                    _Window.Render.GetSprite(),
                    _Window.Render.GetSprite(),
                    _Window.Render.GetSprite(),
                    _Window.Render.GetSprite(),
                });
            }
            return _RectangleList[index];
        }
    }
}

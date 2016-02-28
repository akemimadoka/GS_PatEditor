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
            foreach (var s in _SpriteList)
            {
                _Window.Render.ReturnSprite(s);
            }
            _SpriteList.Clear();

            foreach (var ss in _RectangleList)
            {
                _Window.Render.ReturnSprite(ss[0]);
                _Window.Render.ReturnSprite(ss[1]);
                _Window.Render.ReturnSprite(ss[2]);
                _Window.Render.ReturnSprite(ss[3]);
            }
            _RectangleList.Clear();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.HitAttack
{
    struct Point
    {
        public readonly float X, Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point Rotate(float r)
        {
            return new Point(
                X * (float)Math.Cos(r) - Y * (float)Math.Sin(r),
                X * (float)Math.Sin(r) + X * (float)Math.Cos(r)
            );
        }

        public Point Offset(Point p)
        {
            return new Point(X + p.X, Y + p.Y);
        }

        public Point Relative(Point p)
        {
            return new Point(X - p.X, Y - p.Y);
        }

        public static Point Center(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
    }

    class HitAttackBoxDataProvider : EditingHitAttackBox
    {
        private Editor _Editor;
        private Pat.Box _Box;

        public readonly int Index;

        private float _EditingLeft;
        private float _EditingRight;
        private float _EditingTop;
        private float _EditingBottom;

        private float _EditingRotation;

        private Point _MovingBaseLeftTop;
        private Point _MovingBaseLeftBottom;
        private Point _MovingBaseRightTop;
        private Point _MovingBaseRightBottom;
        private Point _MovingOffset;

        public HitAttackBoxDataProvider(Editor editor, Pat.Box box, int index)
        {
            _Editor = editor;
            _Box = box;
            Index = index;
        }

        #region State access

        #region Begin/Finish Editing

        private bool _IsEditing;
        public bool IsEditing
        {
            get
            {
                return _IsEditing;
            }
            set
            {
                if (_IsEditing == value)
                {
                    return;
                }

                if (value && _IsMoving)
                {
                    //this should not happen
                    throw new Exception();
                }

                _IsEditing = value;
                if (value)
                {
                    BeginEditing();
                }
                else
                {
                    FinishEditing();
                }
            }
        }

        private void BeginEditing()
        {
            _EditingLeft = _Box.X;
            _EditingRight = _Box.X + _Box.W;
            _EditingTop = _Box.Y;
            _EditingBottom = _Box.Y + _Box.H;
            _EditingRotation = _Box.R / 180.0f * 3.1415926f;
        }

        private void FinishEditing()
        {
            _Box.X = (int)Math.Round(_EditingLeft);
            _Box.Y = (int)Math.Round(_EditingTop);
            _Box.W = (int)Math.Round(_EditingRight - _EditingLeft);
            _Box.H = (int)Math.Round(_EditingBottom - _EditingTop);
            _Box.R = (int)Math.Round(_EditingRotation * 180.0f / 3.1415926f);
        }

        private bool _IsMoving;
        public bool IsMoving
        {
            get
            {
                return _IsMoving;
            }
            set
            {
                if (_IsMoving == value)
                {
                    return;
                }

                if (value && _IsEditing)
                {
                    //this should not happen
                    return;
                }

                if (value)
                {
                    BeginMoving();
                }
                else
                {
                    FinishMoving();
                }
                _IsMoving = value;
            }
        }

        private void BeginMoving()
        {
            _MovingBaseLeftTop = LeftTop;
            _MovingBaseLeftBottom = LeftBottom;
            _MovingBaseRightTop = RightTop;
            _MovingBaseRightBottom = RightBottom;
            _MovingOffset = new Point();
        }

        private void FinishMoving()
        {
            Point p;
            float l, t, r, b;
            p = PointScreenToSprite(_MovingBaseLeftTop.Offset(_MovingOffset));
            l = p.X;
            t = p.Y;
            p = PointScreenToSprite(_MovingBaseRightBottom.Offset(_MovingOffset));
            r = p.X;
            b = p.Y;
            _Box.X = (int)Math.Round(l);
            _Box.Y = (int)Math.Round(t);
            _Box.W = (int)Math.Round(r - l);
            _Box.H = (int)Math.Round(b - t);
        }

        #endregion

        #region Screen data access (editing/moving only)

        public Point MovingOffset
        {
            set
            {
                if (_IsMoving)
                {
                    _MovingOffset = value;
                }
            }
        }

        public Point LeftTop
        {
            get
            {
                if (_IsEditing)
                {
                    return PointSpriteToScreen(new Point(_EditingLeft, _EditingTop));
                }
                else if (_IsMoving)
                {
                    return _MovingBaseLeftTop.Offset(_MovingOffset);
                }
                else
                {
                    return PointSpriteToScreen(new Point(_Box.X, _Box.Y));
                }
            }
            set
            {
                if (_IsEditing)
                {
                    var p = PointScreenToSprite(value);
                    _EditingLeft = p.X;
                    _EditingTop = p.Y;
                }
            }
        }
        public Point LeftBottom
        {
            get
            {
                if (_IsEditing)
                {
                    return PointSpriteToScreen(new Point(_EditingLeft, _EditingBottom));
                }
                else if (_IsMoving)
                {
                    return _MovingBaseLeftBottom.Offset(_MovingOffset);
                }
                else
                {
                    return PointSpriteToScreen(new Point(_Box.X, _Box.Y + _Box.H));
                }
            }
            set
            {
                if (_IsEditing)
                {
                    var p = PointScreenToSprite(value);
                    _EditingLeft = p.X;
                    _EditingBottom = p.Y;
                }
            }
        }
        public Point RightTop
        {
            get
            {
                if (_IsEditing)
                {
                    return PointSpriteToScreen(new Point(_EditingRight, _EditingTop));
                }
                else if (_IsMoving)
                {
                    return _MovingBaseRightTop.Offset(_MovingOffset);
                }
                else
                {
                    return PointSpriteToScreen(new Point(_Box.X + _Box.W, _Box.Y));
                }
            }
            set
            {
                if (_IsEditing)
                {
                    var p = PointScreenToSprite(value);
                    _EditingRight = p.X;
                    _EditingTop = p.Y;
                }
            }
        }
        public Point RightBottom
        {
            get
            {
                if (_IsEditing)
                {
                    return PointSpriteToScreen(new Point(_EditingRight, _EditingBottom));
                }
                else if (_IsMoving)
                {
                    return _MovingBaseRightBottom.Offset(_MovingOffset);
                }
                else
                {
                    return PointSpriteToScreen(new Point(_Box.X + _Box.W, _Box.Y + _Box.H));
                }
            }
            set
            {
                if (_IsEditing)
                {
                    var p = PointScreenToSprite(value);
                    _EditingRight = p.X;
                    _EditingBottom = p.Y;
                }
            }
        }

        public Point CenterPoint
        {
            get
            {
                return Point.Center(LeftTop, RightBottom);
            }
        }

        #endregion

        #region Box data access

        public float Left
        {
            get
            {
                if (_IsEditing)
                {
                    return _EditingLeft;
                }
                else if (_IsMoving)
                {
                    return PointScreenToSprite(_MovingBaseLeftTop.Offset(_MovingOffset)).X;
                }
                else
                {
                    return _Box.X;
                }
            }
        }

        public float Top
        {
            get
            {
                if (_IsEditing)
                {
                    return _EditingTop;
                }
                else if (_IsMoving)
                {
                    return PointScreenToSprite(_MovingBaseLeftTop.Offset(_MovingOffset)).Y;
                }
                else
                {
                    return _Box.Y;
                }
            }
        }

        public float Width
        {
            get
            {
                if (_IsEditing)
                {
                    return _EditingRight - _EditingLeft;
                }
                else if (_IsMoving)
                {
                    var p1 = PointScreenToSprite(_MovingBaseLeftTop.Offset(_MovingOffset)).X;
                    var p2 = PointScreenToSprite(_MovingBaseRightBottom.Offset(_MovingOffset)).X;
                    return p2 - p1;
                }
                else
                {
                    return _Box.W;
                }
            }
        }

        public float Height
        {
            get
            {
                if (_IsEditing)
                {
                    return _EditingBottom - _EditingTop;
                }
                else if (_IsMoving)
                {
                    var p1 = PointScreenToSprite(_MovingBaseLeftTop.Offset(_MovingOffset)).Y;
                    var p2 = PointScreenToSprite(_MovingBaseRightBottom.Offset(_MovingOffset)).Y;
                    return p2 - p1;
                }
                else
                {
                    return _Box.H;
                }
            }
        }

        //convert degree
        public float Rotation
        {
            get
            {
                if (_IsEditing)
                {
                    return _EditingRotation;
                }
                else if (_IsMoving)
                {
                    return _Box.R / 180.0f * 3.1415926f;
                }
                else
                {
                    return _Box.R / 180.0f * 3.1415926f;
                }
            }
            set
            {
                //TODO a better rotation
                if (_IsEditing)
                {
                    _EditingRotation = value;
                }
            }
        }

        #endregion

        #endregion

        #region DataConvertion

        #region Update Screen to Editing

        private Point PointScreenToSprite(Point p)
        {
            var sx = _Editor.PreviewWindowUI.PreviewMoving.TransformXClientToSprite(p.X);
            var sy = _Editor.PreviewWindowUI.PreviewMoving.TransformYClientToSprite(p.Y);

            return new Point(
                sx * (float)Math.Cos(-Rotation) - sy * (float)Math.Sin(-Rotation),
                sx * (float)Math.Sin(-Rotation) + sy * (float)Math.Cos(-Rotation)
            );
        }
        private Point PointSpriteToScreen(Point p)
        {
            var sx = p.X * (float)Math.Cos(Rotation) - p.Y * (float)Math.Sin(Rotation);
            var sy = p.X * (float)Math.Sin(Rotation) + p.Y * (float)Math.Cos(Rotation);

            return new Point(
                _Editor.PreviewWindowUI.PreviewMoving.TransformXSpriteToClient(sx),
                _Editor.PreviewWindowUI.PreviewMoving.TransformYSpriteToClient(sy)
            );
        }

        #endregion

        #endregion

        //this property only controls the rendering style
        public bool IsSelected
        {
            get;
            set;
        }
    }
}

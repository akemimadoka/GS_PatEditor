using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Hit
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
    }

    class HitBoxDataProvider : EditingHitAttackBox
    {
        private Editor _Editor;
        private Pat.Box _Box;

        private float _EditingLeft;
        private float _EditingRight;
        private float _EditingTop;
        private float _EditingBottom;
        private float _EditingRotation;

        public HitBoxDataProvider(Editor editor, Pat.Box box)
        {
            _Editor = editor;
            _Box = box;
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
            _EditingRotation = _Box.R;
        }

        private void FinishEditing()
        {
            _Box.X = (int)Math.Round(_EditingLeft);
            _Box.Y = (int)Math.Round(_EditingLeft);
            _Box.W = (int)Math.Round(_EditingRight - _EditingLeft);
            _Box.H = (int)Math.Round(_EditingBottom - _EditingTop);
            _Box.R = (int)Math.Round(_EditingRotation);
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
                    throw new Exception();
                }

                _IsMoving = value;
                if (value)
                {

                }
                else
                {

                }
            }
        }

        private void BeginMoving()
        {
            //TODO
        }

        private void FinishMoving()
        {
            //TODO
        }

        #endregion

        #region Screen data access (editing only)

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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
                }
                else
                {
                    return _Box.H;
                }
            }
        }

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
                    throw new Exception();
                }
                else
                {
                    return _Box.R;
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
                sx * (float)Math.Cos(-_EditingRotation) - sy * (float)Math.Sin(-_EditingRotation),
                sx * (float)Math.Sin(-_EditingRotation) + sy * (float)Math.Cos(-_EditingRotation)
            );
        }
        private Point PointSpriteToScreen(Point p)
        {
            var sx = p.X * (float)Math.Cos(_EditingRotation) - p.Y * (float)Math.Sin(_EditingRotation);
            var sy = p.X * (float)Math.Sin(_EditingRotation) + p.Y * (float)Math.Cos(_EditingRotation);

            return new Point(
                _Editor.PreviewWindowUI.PreviewMoving.TransformXSpriteToClient(sx),
                _Editor.PreviewWindowUI.PreviewMoving.TransformYSpriteToClient(sy)
            );
        }

        #endregion

        #endregion
    }
}

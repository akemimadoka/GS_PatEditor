using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Render
{
    class Sprite
    {
        private bool _Dirty;

        private Texture _Texture;
        public Texture Texture
        {
            get
            {
                return _Texture;
            }
            set
            {
                _Dirty = true;
                _Texture = value;
            }
        }

        private float _Left;
        public float Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Dirty = true;
                _Left = value;
            }
        }

        private float _Top;
        public float Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Dirty = true;
                _Top = value;
            }
        }

        private float _OriginX;
        public float OriginX
        {
            get
            {
                return _OriginX;
            }
            set
            {
                _Dirty = true;
                _OriginX = value;
            }
        }

        private float _OriginY;
        public float OriginY
        {
            get
            {
                return _OriginY;
            }
            set
            {
                _Dirty = true;
                _OriginY = value;
            }
        }

        private float _ScaleX;
        public float ScaleX
        {
            get
            {
                return _ScaleX;
            }
            set
            {
                _Dirty = true;
                _ScaleX = value;
            }
        }

        private float _ScaleY;
        public float ScaleY
        {
            get
            {
                return _ScaleY;
            }
            set
            {
                _Dirty = true;
                _ScaleY = value;
            }
        }

        private float _Rotation;
        public float Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Dirty = true;
                _Rotation = value;
            }
        }

        private Device _Device;

        private VertexBuffer _Buffer;
        private VertexDeclaration _Decl;
        private int _Stride;

        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            [VertexField(DeclarationUsage.Position)]
            public Vector4 pos;
            [VertexField(DeclarationUsage.TextureCoordinate)]
            public Vector4 tex;
            //Don't add more fields
        }

        public Sprite(RenderEngine re)
        {
            _Device = re.Device;
            _ScaleX = 1.0f;
            _ScaleY = 1.0f;
        }

        public void Render()
        {
            if (_Buffer == null)
            {
                InitBuffer();
            }
            if (_Dirty)
            {
                FlushBuffer();
                _Dirty = false;
            }

            _Device.SetTexture(0, _Texture);
            _Device.SetStreamSource(0, _Buffer, 0, _Stride);
            _Device.VertexDeclaration = _Decl;
            _Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        private void InitBuffer()
        {
            _Stride = Utilities.SizeOf<Vertex>();
            _Decl = VertexReflection.CreateVertexDeclaration<Vertex>(_Device);
            _Buffer = new VertexBuffer(_Device, _Stride * 4, Usage.WriteOnly, VertexFormat.None, Pool.Managed);

            _Dirty = true;
        }

        private void FlushBuffer()
        {
            float t_l, t_t, t_r, t_b;

            float x = Left, y = Top, r = -Rotation;//left-hand -> right-hand

            //get image size
            {
                var surface = Texture.GetSurfaceLevel(0);
                var desc = surface.Description;
                t_r = desc.Width;
                t_b = desc.Height;
                t_l = t_t = 0;
            }
            //apply origin
            {
                t_l -= OriginX;
                t_r -= OriginX;
                t_t -= OriginY;
                t_b -= OriginY;
            }
            //apply scale
            {
                t_l *= ScaleX;
                t_r *= ScaleX;
                t_t *= ScaleY;
                t_b *= ScaleY;
            }

            var stream = _Buffer.Lock(0, 0, LockFlags.Discard);

            //apply rotation

            stream.Write(new Vertex { pos = MakePosition(x, y, r, t_l, t_t), tex = new Vector4(0.0f, 0.0f, 0.0f, 0.0f) });
            stream.Write(new Vertex { pos = MakePosition(x, y, r, t_r, t_t), tex = new Vector4(1.0f, 0.0f, 0.0f, 0.0f) });
            stream.Write(new Vertex { pos = MakePosition(x, y, r, t_l, t_b), tex = new Vector4(0.0f, 1.0f, 0.0f, 0.0f) });
            stream.Write(new Vertex { pos = MakePosition(x, y, r, t_r, t_b), tex = new Vector4(1.0f, 1.0f, 0.0f, 0.0f) });
            stream.Dispose();

            _Buffer.Unlock();
        }

        private static Vector4 MakePosition(float x, float y, float r, float tx, float ty)
        {
            return new Vector4(
                x + tx * (float)Math.Cos(r) - ty * (float)Math.Sin(r),
                y + tx * (float)Math.Sin(r) + ty * (float)Math.Cos(r),
                0.0f, 1.0f);
        }
    }
}

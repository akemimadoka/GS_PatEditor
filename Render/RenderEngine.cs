using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Render
{
    public class RenderEngine : IDisposable
    {
        private Direct3D _Direct3d;
        private Device _Device;

        private Effect _Effect;

        public Device Device { get { return _Device; } }
        public readonly GlobalTransform Transform = new GlobalTransform();

        public event Action OnRender;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            [VertexField(DeclarationUsage.Position)]
            public Vector4 pos;
            [VertexField(DeclarationUsage.TextureCoordinate)]
            public Vector2 tex;
        }

        public RenderEngine(Control ctrl)
        {
            var present = new PresentParameters(ctrl.ClientSize.Width, ctrl.ClientSize.Height);
            present.PresentationInterval = PresentInterval.One;
            present.BackBufferFormat = Format.A8R8G8B8;

            _Direct3d = new Direct3D();
            _Device = new Device(_Direct3d, 0,
                DeviceType.Hardware,
                ctrl.Handle,
                CreateFlags.HardwareVertexProcessing,
                present);

            _Device.SetRenderState(RenderState.CullMode, false);
            _Device.SetRenderState(RenderState.ZFunc, Compare.Always);

            _Device.SetTransform(TransformState.View, Matrix.Identity);
            _Device.SetTransform(TransformState.Projection, _Device.GetViewProjectionMatrix());

            _Device.SetRenderState(RenderState.AlphaTestEnable, false);
            _Device.SetRenderState(RenderState.AlphaTestEnable, false);
            _Device.SetRenderState(RenderState.AlphaRef, 0.1f);
            _Device.SetRenderState(RenderState.AlphaFunc, Compare.GreaterEqual);

            _Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            _Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            _Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            _Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

            _Effect = Effect.FromString(_Device, Shader.Value, ShaderFlags.None);
            _Effect.SetValue("mat_ViewProj", _Device.GetViewProjectionMatrix());

            using (var bitmap = new System.Drawing.Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                bitmap.SetPixel(0, 0, System.Drawing.Color.Black);
                _BlackTexture = CreateTextureFromBitmap(bitmap);
            }
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _Effect);
            Utilities.Dispose(ref _Device);
            Utilities.Dispose(ref _Direct3d);
        }

        public void RenderAll()
        {
            _Device.BeginScene();
            _Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);

            _Effect.SetValue("vec_Offset", new Vector4(Transform.X, Transform.Y, 0, 0));
            _Effect.SetValue("f_Scale", Transform.Scale);

            _Effect.Begin();
            _Effect.BeginPass(0);

            if (OnRender != null)
            {
                OnRender();
            }

            _Effect.EndPass();
            _Effect.End();

            _Device.EndScene();
            _Device.Present();
        }

        private MemoryStream _BitmapStream = new MemoryStream(1024 * 1024);

        public Texture CreateTextureFromBitmap(System.Drawing.Bitmap bitmap)
        {
            _BitmapStream.Seek(0, SeekOrigin.Begin);
            bitmap.Save(_BitmapStream, System.Drawing.Imaging.ImageFormat.Bmp);

            _BitmapStream.Seek(0, SeekOrigin.Begin);
            //set size in FromStream, or it will be resized
            return Texture.FromStream(_Device, _BitmapStream, bitmap.Width, bitmap.Height, 1,
                Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.Point, Filter.Point, 0);
        }

        private Texture _BlackTexture;
        public Texture GetBlackTexture()
        {
            return _BlackTexture;
        }
    }
}

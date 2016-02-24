using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Render
{
    class RenderEngine : IDisposable
    {
        private Direct3D _Direct3d;
        private Device _Device;

        private Effect _Effect;

        public Device Device { get { return _Device; } }

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
    }
}

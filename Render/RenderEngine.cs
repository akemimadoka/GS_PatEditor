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
        private Direct3D direct3d;
        private Device device;

        private Texture txt;
        private VertexBuffer data;
        private VertexDeclaration decl;
        private int stride;
        private Effect effect;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            [VertexField(DeclarationUsage.Position)]
            public Vector4 pos;
            [VertexField(DeclarationUsage.TextureCoordinate)]
            public Vector2 tex;
            //[VertexField(DeclarationUsage.Color)]
            //public Vector4 col;
        }

        public RenderEngine(Control ctrl)
        {
            var present = new PresentParameters(ctrl.ClientSize.Width, ctrl.ClientSize.Height);
            present.PresentationInterval = PresentInterval.One;
            present.BackBufferFormat = Format.A8R8G8B8;

            direct3d = new Direct3D();
            device = new Device(direct3d, 0,
                DeviceType.Hardware,
                ctrl.Handle,
                CreateFlags.HardwareVertexProcessing,
                present);

            device.SetRenderState(RenderState.CullMode, false);
            device.SetRenderState(RenderState.ZFunc, Compare.Always);

            device.SetTransform(TransformState.View, Matrix.Identity);
            device.SetTransform(TransformState.Projection, device.GetViewProjectionMatrix());

            device.SetRenderState(RenderState.AlphaTestEnable, false);
            device.SetRenderState(RenderState.AlphaTestEnable, false);
            device.SetRenderState(RenderState.AlphaRef, 0.1f);
            device.SetRenderState(RenderState.AlphaFunc, Compare.GreaterEqual);

            device.SetRenderState(RenderState.AlphaBlendEnable, true);
            device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

            effect = Effect.FromString(device, Shader.Value, ShaderFlags.None);
            effect.SetValue("mat_ViewProj", device.GetViewProjectionMatrix());

            txt = Texture.FromFile(device, @"E:\2.png", Usage.None, Pool.Default);
            {
                float x = 0, y = 0, r = 16, b = 16;
                data = VertexReflection.CreateVertexBuffer(device, new[] {
                    new Vertex { pos = new Vector4(x, y, 0, 1.0f), tex = new Vector2(0.0f, 0.0f) },
                    new Vertex { pos = new Vector4(r, y, 0, 1.0f), tex = new Vector2(1.0f, 0.0f) },
                    new Vertex { pos = new Vector4(x, b, 0, 1.0f), tex = new Vector2(0.0f, 1.0f) },
                    new Vertex { pos = new Vector4(r, b, 0, 1.0f), tex = new Vector2(1.0f, 1.0f) },
                });
                decl = VertexReflection.CreateVertexDeclaration<Vertex>(device);
                stride = Utilities.SizeOf<Vertex>();
            }
        }

        public void Dispose()
        {
        }

        private Random rand = new Random();
        public void RenderAll()
        {
            device.BeginScene();
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);

            effect.Begin();
            effect.BeginPass(0);

            device.SetTexture(0, txt);
            device.SetStreamSource(0, data, 0, stride);
            device.VertexDeclaration = decl;
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            effect.EndPass();
            effect.End();

            device.EndScene();
            device.Present();
        }
    }
}

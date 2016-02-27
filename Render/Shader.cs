using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Render
{
    class Shader
    {
        public static readonly string Value = @"

struct VS_IN
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD0;
};

struct PS_IN
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD0;
};

float4x4 mat_ViewProj;
float4 vec_Offset;
float f_Scale;

sampler2D s_2D = sampler_state
{
    //MIN_MAG_MIP_LINEAR
    //ANISOTROPIC
    //POINT
    Filter = POINT;
	AddressU = Border;
	AddressV = Border;
    
};

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;
    input.pos.x *= f_Scale;
    input.pos.y *= f_Scale;
	output.pos = mul(input.pos + vec_Offset, mat_ViewProj);
	output.tex = input.tex;
	return output;
}

float4 PS(PS_IN input) : COLOR
{
    return tex2D(s_2D, input.tex);
}

technique Main {
	pass P0 {
		VertexShader = compile vs_2_0 VS();
		PixelShader = compile ps_2_0 PS();
	}
}

";
    }
}

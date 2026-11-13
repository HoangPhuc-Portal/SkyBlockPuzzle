Shader "SkyBlock/ComboEnergy"
{
 Properties { [PerRendererData] _EnergyTint("Energy Tint",Color)=(1,1,1,1) }
 SubShader {
 Tags {"Queue"="Transparent+50" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline"}
 Cull Off ZWrite Off ZTest Always Blend SrcAlpha One
 Pass {
 HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 float4 _EnergyTint;
 struct a {float4 vertex:POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
 struct v {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
 v vert(a i){v o;o.pos=UnityObjectToClipPos(i.vertex);o.uv=i.uv;o.color=i.color*_EnergyTint;return o;}
 half4 frag(v i):SV_Target {float r=length(i.uv-.5)*2;float glow=pow(saturate(1-r),3);return half4(i.color.rgb,i.color.a*glow);}
 ENDHLSL
 }
 }
}

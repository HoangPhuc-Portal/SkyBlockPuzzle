Shader "SkyBlock/MenuSky"
{
 Properties { [PerRendererData] _MainTex("Sky artwork",2D)="white"{} }
 SubShader { Tags {"Queue"="Transparent" "RenderType"="Transparent"}
 Cull Off ZWrite Off ZTest [unity_GUIZTestMode]
 Pass { HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 sampler2D _MainTex;
 struct a {float4 vertex:POSITION;float2 uv:TEXCOORD0;};struct v {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
 v vert(a i){v o;o.pos=UnityObjectToClipPos(i.vertex);o.uv=i.uv;return o;}
 half4 frag(v i):SV_Target {float y=i.uv.y>.5 ? .998-saturate((i.uv.y-.9)*10)*.06 : .002+saturate((.1-i.uv.y)*10)*.04;return tex2D(_MainTex,float2(i.uv.x,y));}
 ENDHLSL } }
}

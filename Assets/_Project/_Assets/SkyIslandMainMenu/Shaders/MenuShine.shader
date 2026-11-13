Shader "SkyBlock/MenuShine"
{
 Properties { [PerRendererData] _MainTex("Artwork",2D)="white"{} _Period("Period",Float)=11 _Phase("Phase",Float)=0 }
 SubShader {
 Tags {"Queue"="Transparent" "RenderType"="Transparent"}
 Cull Off ZWrite Off ZTest [unity_GUIZTestMode] Blend SrcAlpha OneMinusSrcAlpha
 Pass { HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 struct a {float4 vertex:POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
 struct v {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
 sampler2D _MainTex;float _Period,_Phase;
 v vert(a i){v o;o.pos=UnityObjectToClipPos(i.vertex);o.uv=i.uv;o.color=i.color;return o;}
 half4 frag(v i):SV_Target {half4 c=tex2D(_MainTex,i.uv);float p=fmod(_Time.y+_Phase,_Period);float shine=1-smoothstep(0,.065,abs(i.uv.x+i.uv.y*.25-(p*1.6-.2)));c.rgb+=shine*.23*step(p,1);return c*i.color;}
 ENDHLSL }
 }
}

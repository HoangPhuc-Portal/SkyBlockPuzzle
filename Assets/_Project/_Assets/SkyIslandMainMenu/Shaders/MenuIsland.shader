Shader "SkyBlock/MenuIsland"
{
 Properties { [PerRendererData] _MainTex("Artwork",2D)="white"{} }
 SubShader {
 Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
 Cull Off ZWrite Off ZTest [unity_GUIZTestMode] Blend SrcAlpha OneMinusSrcAlpha
 Pass { HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 struct a { float4 vertex:POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
 struct v { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
 sampler2D _MainTex;
 v vert(a i) { v o;o.pos=UnityObjectToClipPos(i.vertex);o.uv=i.uv;o.color=i.color;return o; }
 float area(float2 p,float4 r) {float2 d=min(p-r.xy,r.zw-p);return smoothstep(0,.008,min(d.x,d.y));}
 float glow(float2 p,float2 c,float radius) { return exp(-dot(p-c,p-c)/(radius*radius)); }
 half4 frag(v i):SV_Target {
 float t=_Time.y;float2 uv=i.uv;half4 col=tex2D(_MainTex,uv);
 float region=area(uv,float4(.40,.035,.60,.27))+area(uv,float4(.37,.49,.54,.58));
 float water=saturate(region)*smoothstep(.12,.35,min(col.b,col.g)-col.r);
 // Downward travelling luminance; sample displacement stays within cyan water pixels.
 float wave=sin(uv.y*235+t*5);
 float green=smoothstep(.12,.28,col.g-max(col.r,col.b));
 float foliage=green*area(uv,float4(.01,.31,.98,.68));
 float2 offset=float2(water*.0018*sin(uv.y*190+t*3)+foliage*.0012*sin(t+uv.y*8),water*.001*wave);
 col=tex2D(_MainTex,uv+offset);col.rgb+=water*(.045+.045*wave);
 float crystals=0;
 crystals+=glow(uv,float2(.343,.522),.022)*(.5+.5*sin(t*1.65));
 crystals+=glow(uv,float2(.646,.52),.022)*(.5+.5*sin(t*1.9+2));
 crystals+=glow(uv,float2(.126,.342),.025)*(.5+.5*sin(t*1.4+4));
 crystals+=glow(uv,float2(.863,.342),.025)*(.5+.5*sin(t*2.2+1));
 col.rgb+=crystals*float3(.12,.28,.3);
 float windows=area(uv,float4(.60,.535,.85,.648))*smoothstep(.15,.4,col.r-col.b);
 col.rgb+=windows*.035*(.5+.5*sin(t*1.3));
 return col*i.color;
 }
 ENDHLSL }
 }
}

Shader "SkyBlock/IslandWaterUnlit"
{
 Properties { [PerRendererData] _MainTex("Background",2D)="white"{} _WaterStrength("Water movement",Range(0,.5))=.13 }
 SubShader
 {
  Tags {"Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "IgnoreProjector"="True"}
  Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
  Pass
  {
   Tags {"LightMode"="SRPDefaultUnlit"}
   HLSLPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "UnityCG.cginc"
   struct appdata {float4 vertex:POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
   struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float4 color:COLOR;};
   sampler2D _MainTex;float _EnvironmentTime,_WaterStrength;float4 _WaterArea0,_WaterArea1,_WaterArea2,_WaterArea3;
   v2f vert(appdata v){v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.uv;o.color=v.color;return o;}
   float maskRect(float2 uv,float4 r){float2 d=min(uv-r.xy,r.xy+r.zw-uv);return smoothstep(0,.012,d.x)*smoothstep(0,.012,d.y);}
   half4 frag(v2f i):SV_Target
   {
    half4 base=tex2D(_MainTex,i.uv);
    float area=saturate(maskRect(i.uv,_WaterArea0)+maskRect(i.uv,_WaterArea1)+maskRect(i.uv,_WaterArea2)+maskRect(i.uv,_WaterArea3));
    float water=area*smoothstep(.08,.30,base.b-base.r)*smoothstep(.05,.25,base.g-base.r);
    float t=_EnvironmentTime;
    float2 uv=i.uv+water*_WaterStrength*float2(.009*sin(i.uv.y*150+t*3),.018*sin(i.uv.y*90+t*4));
    half4 flow=tex2D(_MainTex,uv);
    flow.rgb*=1+water*_WaterStrength*.45*sin(i.uv.y*140+t*6);
    return lerp(base,flow,water)*i.color;
   }
   ENDHLSL
  }
 }
}

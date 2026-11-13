Shader "SkyBlock/GameplaySkyExtension"
{
    Properties { [PerRendererData] _MainTex("Existing island artwork",2D)="white"{} }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Tags { "LightMode"="SRPDefaultUnlit" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _ArtworkRect;
            struct Input { float4 vertex:POSITION; };
            struct Output { float4 position:SV_POSITION; float4 screen:TEXCOORD0; };
            Output vert(Input input)
            {
                Output o; o.position=UnityObjectToClipPos(input.vertex);
                o.screen=ComputeScreenPos(o.position); return o;
            }
            half4 frag(Output input):SV_Target
            {
                float2 screen=input.screen.xy/input.screen.w;
                float2 uv=(screen-_ArtworkRect.xy)/max(_ArtworkRect.zw,float2(.001,.001));
                if (uv.y>=0 && uv.y<=1) return tex2D(_MainTex,saturate(uv));
                // Sample only the sky at the edge, then fade out before any island geometry
                // can repeat on very tall displays. The authored center remains unchanged.
                float distance=max(-uv.y,uv.y-1);
                bool top=uv.y>1;
                uv.x=saturate(uv.x);
                uv.y=top ? .999 : .001;
                half4 edge=tex2D(_MainTex,uv);
                half3 sky=top ? half3(.015,.32,.70) : half3(.65,.80,.94);
                edge.rgb=lerp(edge.rgb,sky,smoothstep(0,.075,distance));
                return edge;
            }
            ENDHLSL
        }
    }
}

Shader "Hidden/MotionBulr"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _SpeedMap ("SpeedMap", 2D) = "white" { }
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            struct v2f
            {
                float4 vertex: SV_POSITION;
                float3 velocity: TEXCOORD1;
            };
            
            float4x4 _PrevMatrix;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 prevPos = mul(_PrevMatrix, v.vertex);
                o.velocity = o.vertex.xyz / o.vertex.w - prevPos.xyz / prevPos.w;
               // o.velocity *= 1;// 画面サイズを1.0としたUV座標系での速度に変換
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                fixed4 col = fixed4(i.velocity, length(i.velocity));
           

                return col;
            }
            ENDCG
            
        }
        
        pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
             #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            float4x4 _PrevMatrix;
            sampler2D _SpeedMap;
                float4 _SpeedMap_ST;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            appdata vert(appdata v)
            {
                appdata o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _SpeedMap);
                return o;
            }
            
            fixed4 frag(appdata i): SV_Target
            {
                float4 speed = tex2D(_SpeedMap, i.uv);
          
                // 露光時間比
                const float exp_rate = 0.8; 
                if(speed.a <= 0.1)                
                {
                    return 0;
                    return tex2D(_MainTex,i.uv);
                }
                fixed4 col = 0;
                // 露光遅延
                const float exp_delay = 0.3;
                for (int j = 0; j < 19; ++ j)
                {
                    col += tex2D(_MainTex,i.uv + speed);
                }
                return col / 19;
            }
            
           
            ENDCG
            
        }
    }
}

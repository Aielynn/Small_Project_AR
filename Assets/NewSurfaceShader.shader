// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "litg/Depth Write"
{
    Properties {
    }
 
    SubShader
    {
        Tags
        {
            "IgnoreProjector" = "True"
            "Queue" = "Geometry-100"
        }
 
        Pass
        {
            Cull Off
            ZWrite On
            ColorMask 0
            Lighting Off
            Fog { Mode Off }
           
 
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            struct vertexData
            {
                float4 vertex : POSITION;
            };
 
            struct fragmentData
            {
                float4 position : SV_POSITION;
            };
 
            fragmentData vert(vertexData v)
            {
                fragmentData o;
                o.position = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            float4 frag(fragmentData i) : SV_Target
            {
                return 1;
            }
 
            ENDCG
        }  
    }
}
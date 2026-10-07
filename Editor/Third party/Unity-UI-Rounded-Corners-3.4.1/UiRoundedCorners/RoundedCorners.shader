Shader "UI/RoundedCorners/RoundedCorners" {
    Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        
        // Definition in Properties section is required to Mask works properly
        _WidthHeightRadius ("WidthHeightRadius", Vector) = (0,0,0,0)
        _OuterUV ("image outer uv", Vector) = (0, 0, 1, 1)
        _BorderColor ("Border Color", Color) = (1, 1, 1, 1)
        _BorderWidth ("Border Width", Float) = 0
        // ---
    }
    
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---
        
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite Off

        Pass {
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"          
            #include "SDFUtils.cginc"
            #include "ShaderSetup.cginc"
            
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            float4 _WidthHeightRadius;
            float4 _OuterUV;
            float4 _BorderColor;
            float _BorderWidth;
            sampler2D _MainTex;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            fixed4 frag (v2f i) : SV_Target {
                // Determine normalized 0~1 coordinate across the base area
                float2 uvSample = i.uvRect;
                if (fwidth(i.uvRect.x) == 0.0 && fwidth(i.uvRect.y) == 0.0) {
                    uvSample = i.uv;
                    if (_OuterUV.z > _OuterUV.x && _OuterUV.w > _OuterUV.y) {
                        uvSample.x = (uvSample.x - _OuterUV.x) / (_OuterUV.z - _OuterUV.x);
                        uvSample.y = (uvSample.y - _OuterUV.y) / (_OuterUV.w - _OuterUV.y);
                    }
                }

                half4 spriteColor = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;

                #ifdef UNITY_UI_CLIP_RECT
                half clipFactor = UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                spriteColor.a *= clipFactor;
                #endif

                float alphaOuter = CalcAlpha(uvSample, _WidthHeightRadius.xy, _WidthHeightRadius.z);

                if (_BorderWidth <= 0.0) {
                    spriteColor.a = min(spriteColor.a, alphaOuter);
                    #ifdef UNITY_UI_ALPHACLIP
                    clip(spriteColor.a - 0.001);
                    #endif
                    return spriteColor;
                }

                float alphaInner = CalcInnerAlpha(uvSample, _WidthHeightRadius.xy, _WidthHeightRadius.z, _BorderWidth);

                half4 borderColor = _BorderColor;
                borderColor.a *= i.color.a;

                #ifdef UNITY_UI_CLIP_RECT
                borderColor.a *= clipFactor;
                #endif

                // borderWeight: 1 in border area, 0 inside content
                float borderWeight = 1.0 - alphaInner;
                // contentWeight: 0 in border area, 1 inside content
                float contentWeight = alphaInner;

                float borderA = borderColor.a * borderWeight;
                float spriteA = spriteColor.a * contentWeight;
                float totalA = borderA + spriteA;

                half3 finalRGB = (borderColor.rgb * borderA + spriteColor.rgb * spriteA) / max(0.0001, totalA);
                half4 finalColor = half4(finalRGB, min(totalA, alphaOuter));

                #ifdef UNITY_UI_ALPHACLIP
                clip(finalColor.a - 0.001);
                #endif

                return finalColor;
            }
            
            ENDCG
        }
    }
}

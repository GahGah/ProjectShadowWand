Shader "Hidden/ShadowGroup2D"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ShadowStencilGroup("__ShadowStencilGroup", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull Off
        BlendOp Add
        Blend One One
        ZWrite Off

        Pass
        {
            Stencil
            {
                Ref [_ShadowStencilGroup]
                Comp NotEqual
                Pass Replace
                Fail Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 vertex : POSITION;
                float4 tangent: TANGENT;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float4 extrusion : COLOR;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float3 _LightPos;
            uniform float  _ShadowRadius;

            // 역행렬 계산식
            float2x2 invert2x2(float2 basisX, float2 basisY)
            {
                float2x2 m = float2x2(basisX, basisY);
                return float2x2(m._m11, -m._m10, -m._m01, m._m00) / determinant(m);
            }

            Varyings vert (Attributes v)
            {
                Varyings o;

                float3 vertexWS = TransformObjectToWorld(v.vertex);  // This should be in world space
                float3 lightDir = _LightPos - vertexWS;
                lightDir.z = 0;

                // Start of code to see if this point should be extruded
                float3 lightDirection = normalize(lightDir);  

                float3 endpoint = vertexWS + (_ShadowRadius * -lightDirection);

                float3 worldTangent = TransformObjectToWorldDir(v.tangent.xyz);
                float sharedShadowTest = saturate(ceil(dot(lightDirection, worldTangent)));

                // Start of code to calculate offset
                float3 vertexWS0 = TransformObjectToWorld(float3(v.extrusion.xy, 0));
                float3 vertexWS1 = TransformObjectToWorld(float3(v.extrusion.zw, 0));
                float3 shadowDir0 = vertexWS0 - _LightPos;
                shadowDir0.z = 0;
                shadowDir0 = normalize(shadowDir0);

                float3 shadowDir1 = vertexWS1 -_LightPos;
                shadowDir1.z = 0;
                shadowDir1 = normalize(shadowDir1);

                float3 shadowDir = normalize(shadowDir0 + shadowDir1);
                float3 sharedShadowOffset = sharedShadowTest * _ShadowRadius * shadowDir;

                float3 position;
                position = vertexWS + sharedShadowOffset;


                o.uv2 = v.uv2;
                o.uv3 = v.uv3;

                // RGB - R is shadow value (to support soft shadows), G is Self Shadow Mask, B is No Shadow Mask
                o.color = 1; // v.color;
                o.color.g = 0.5;
                o.color.b = 0;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //----------------//
                float2 currentPos = position;

                float2 segStartPos = vertexWS0;
                float2 segEndPos = vertexWS1;

                float2 A = _ShadowRadius * float2(-1, 1) * normalize(segStartPos).yx;
                float2 B = _ShadowRadius * float2(-1, 1) * normalize(segEndPos).yx;
                float2 projectionOffset = lerp(A, B, sharedShadowTest);

                float2 projectionVec = currentPos - projectionOffset; // Projection Vector
                if (v.uv.y == 1) {
                    o.penumbras = mul(invert2x2(A, segStartPos), projectionVec);  // Spatial transformation, with Light-A as the X-axis and Light-Start as the Y-axis, turns the projection vector back into model space
                }
                else {
                    o.penumbras = mul(invert2x2(A, segStartPos), currentPos - segStartPos); // uv.x = 0 is float2(0,0), uv.x = 1 will turn the projection vector back into model space with the projection segment Start-End on the X-axis and Light-Start on the Y-axis.
                }



                o.vertex = TransformWorldToHClip(position);
                

                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float4 screenPos = ComputeScreenPos(i.vertex);

                float4 ndcPos = (screenPos / screenPos.w) * 2 - 1;
                float3 viewVec = float3 (unity_OrthoParams.xy * ndcPos.xy, 0);
                // 원하는 깊이에 대한 공간 z 구성 요소 할당 관찰 
                float3 viewPos = float3 (viewVec.xy, 0);
                float3 worldPos = mul(UNITY_MATRIX_I_V, float4 (viewPos, 1)).xyz;
                // 세계 좌표-광원 위치 = 모델 좌표 
                float2 objPos = worldPos.xy - _LightPos.xy;

                float4 main = tex2D(_MainTex, i.uv);
                float4 col = i.color;
                col.r *= objPos.x;
                col.g = main.a * col.g;
                return col;
            }
            ENDHLSL
        }
        Pass
        {
            Stencil
            {
                Ref [_ShadowStencilGroup]
                Comp NotEqual
                Pass Replace
                Fail Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex);

                // RGB - R is shadow value (to support soft shadows), G is Self Shadow Mask, B is No Shadow Mask
                o.color = 1; 
                o.color.g = 0.5;
                o.color.b = 1;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half4 main = tex2D(_MainTex, i.uv);
                half4 color = i.color;
                color.b = 1 - main.a;

                return color;
            }
            ENDHLSL
        }
    }
}

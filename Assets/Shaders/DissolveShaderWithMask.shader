Shader "DissolveShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Texture2D", 2D) = "white" {}
        _DissolveAmount("DissolveAmount", Range(0, 1)) = 1
        Vector1_fe78bfe4c54f4d1aa9910ad6501bd55c("NoiseScale", Float) = 49.59
        [HDR]Color_de73dd9654404bbab6ed0654319cd8fb("DissolveColor", Color) = (0.1605554, 0.8606634, 3.09434, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            // Render State
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest [unity_GUIZTestMode]
            ZWrite Off
            //ColorMask
            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]


            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM
            // Pragmas
            #pragma target 2.0
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            struct SurfaceDescriptionInputs
            {
                float3 WorldSpacePosition;
                float4 uv0;
            };

            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
            };

            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                float3 interp0 : TEXCOORD0;
                float4 interp1 : TEXCOORD1;
                float4 interp2 : TEXCOORD2;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp0.xyz = input.positionWS;
                output.interp1.xyzw = input.texCoord0;
                output.interp2.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
                #endif
                return output;
            }

            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp0.xyz;
                output.texCoord0 = input.interp1.xyzw;
                output.color = input.interp2.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_TexelSize;
            float _DissolveAmount;
            float Vector1_fe78bfe4c54f4d1aa9910ad6501bd55c;
            CBUFFER_END

            // Object and Global properties
            SAMPLER(SamplerState_Linear_Repeat);
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 Color_de73dd9654404bbab6ed0654319cd8fb;

            // Graph Functions


            inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
            {
                return (1.0 - t) * a + (t * b);
            }


            inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                Out = t;
            }

            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                UnityTexture2D _Property_fedff9d08ce246368a4da65160736958_Out_0 = UnityBuildTexture2DStructNoScale(
                    _MainTex);
                float4 _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0 = SAMPLE_TEXTURE2D(
                    _Property_fedff9d08ce246368a4da65160736958_Out_0.tex,
                    _Property_fedff9d08ce246368a4da65160736958_Out_0.samplerstate, IN.uv0.xy);
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_R_4 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.r;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_G_5 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.g;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_B_6 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.b;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_A_7 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.a;
                float _Property_94b85667ceea473a8824a33638da822f_Out_0 = Vector1_fe78bfe4c54f4d1aa9910ad6501bd55c;
                float _SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2;
                Unity_SimpleNoise_float((IN.WorldSpacePosition.xy), _Property_94b85667ceea473a8824a33638da822f_Out_0,
                                        _SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2);
                float _Property_59fa5d640d894927805d1b6d4372eed4_Out_0 = _DissolveAmount;
                float _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2;
                Unity_Step_float(_SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2,
                                 _Property_59fa5d640d894927805d1b6d4372eed4_Out_0,
                                 _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2);
                float _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2;
                Unity_Multiply_float(_SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_A_7,
                                     _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2,
                                     _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2);
                float4 _Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4;
                float3 _Combine_39a48d2861b349549a9c73c65d24970b_RGB_5;
                float2 _Combine_39a48d2861b349549a9c73c65d24970b_RG_6;
                Unity_Combine_float(_SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_R_4,
                                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_G_5,
                                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_B_6,
                                    _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RGB_5,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RG_6);
                float _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2;
                Unity_Subtract_float(_Property_59fa5d640d894927805d1b6d4372eed4_Out_0, 0.1,
                                     _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2);
                float _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2;
                Unity_Step_float(_SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2,
                                 _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2,
                                 _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2);
                float _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2;
                Unity_Subtract_float(_Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2,
                                     _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2,
                                     _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2);
                float _Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2;
                Unity_Multiply_float(_Multiply_96471088ddcb4ba684d66cabad42e275_Out_2,
                                     _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2,
                                     _Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2);
                float4 _Property_68bc14fe64ce4652b591e199b08f8ed0_Out_0 = IsGammaSpace()
                                                                              ? LinearToSRGB(
                                                                                  Color_de73dd9654404bbab6ed0654319cd8fb)
                                                                              : Color_de73dd9654404bbab6ed0654319cd8fb;
                float4 _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2;
                Unity_Multiply_float((_Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2.xxxx),
                                     _Property_68bc14fe64ce4652b591e199b08f8ed0_Out_0,
                                     _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2);
                float4 _Add_28171aa2d98042529a0e41bbc978ebca_Out_2;
                Unity_Add_float4(_Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4,
                                 _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2,
                                 _Add_28171aa2d98042529a0e41bbc978ebca_Out_2);
                float _Split_5a528ac060c34cfa978157175151a0d8_R_1 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[0];
                float _Split_5a528ac060c34cfa978157175151a0d8_G_2 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[1];
                float _Split_5a528ac060c34cfa978157175151a0d8_B_3 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[2];
                float _Split_5a528ac060c34cfa978157175151a0d8_A_4 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[3];
                surface.BaseColor = (_Add_28171aa2d98042529a0e41bbc978ebca_Out_2.xyz);
                surface.Alpha = _Split_5a528ac060c34cfa978157175151a0d8_A_4;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);


                output.WorldSpacePosition = input.positionWS;
                output.uv0 = input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM
            // Pragmas
            #pragma target 2.0
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            struct SurfaceDescriptionInputs
            {
                float3 WorldSpacePosition;
                float4 uv0;
            };

            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
            };

            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                float3 interp0 : TEXCOORD0;
                float4 interp1 : TEXCOORD1;
                float4 interp2 : TEXCOORD2;
                #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp0.xyz = input.positionWS;
                output.interp1.xyzw = input.texCoord0;
                output.interp2.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
                #endif
                return output;
            }

            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp0.xyz;
                output.texCoord0 = input.interp1.xyzw;
                output.color = input.interp2.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_TexelSize;
            float _DissolveAmount;
            float Vector1_fe78bfe4c54f4d1aa9910ad6501bd55c;
            CBUFFER_END

            // Object and Global properties
            SAMPLER(SamplerState_Linear_Repeat);
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 Color_de73dd9654404bbab6ed0654319cd8fb;

            // Graph Functions


            inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
            {
                return (1.0 - t) * a + (t * b);
            }


            inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = Unity_SimpleNoise_RandomValue_float(c0);
                float r1 = Unity_SimpleNoise_RandomValue_float(c1);
                float r2 = Unity_SimpleNoise_RandomValue_float(c2);
                float r3 = Unity_SimpleNoise_RandomValue_float(c3);

                float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
                float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
                float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                Out = t;
            }

            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                UnityTexture2D _Property_fedff9d08ce246368a4da65160736958_Out_0 = UnityBuildTexture2DStructNoScale(
                    _MainTex);
                float4 _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0 = SAMPLE_TEXTURE2D(
                    _Property_fedff9d08ce246368a4da65160736958_Out_0.tex,
                    _Property_fedff9d08ce246368a4da65160736958_Out_0.samplerstate, IN.uv0.xy);
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_R_4 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.r;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_G_5 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.g;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_B_6 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.b;
                float _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_A_7 =
                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_RGBA_0.a;
                float _Property_94b85667ceea473a8824a33638da822f_Out_0 = Vector1_fe78bfe4c54f4d1aa9910ad6501bd55c;
                float _SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2;
                Unity_SimpleNoise_float((IN.WorldSpacePosition.xy), _Property_94b85667ceea473a8824a33638da822f_Out_0,
                                        _SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2);
                float _Property_59fa5d640d894927805d1b6d4372eed4_Out_0 = _DissolveAmount;
                float _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2;
                Unity_Step_float(_SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2,
                                 _Property_59fa5d640d894927805d1b6d4372eed4_Out_0,
                                 _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2);
                float _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2;
                Unity_Multiply_float(_SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_A_7,
                                     _Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2,
                                     _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2);
                float4 _Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4;
                float3 _Combine_39a48d2861b349549a9c73c65d24970b_RGB_5;
                float2 _Combine_39a48d2861b349549a9c73c65d24970b_RG_6;
                Unity_Combine_float(_SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_R_4,
                                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_G_5,
                                    _SampleTexture2D_2f3307d226eb47fcbf088b8d06805c6c_B_6,
                                    _Multiply_96471088ddcb4ba684d66cabad42e275_Out_2,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RGB_5,
                                    _Combine_39a48d2861b349549a9c73c65d24970b_RG_6);
                float _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2;
                Unity_Subtract_float(_Property_59fa5d640d894927805d1b6d4372eed4_Out_0, 0.1,
                                     _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2);
                float _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2;
                Unity_Step_float(_SimpleNoise_bc5ae9296b104a9ca9aa435d180ff3f0_Out_2,
                                 _Subtract_1e8ce37f5a3f4ee5bc539bb0f6949084_Out_2,
                                 _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2);
                float _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2;
                Unity_Subtract_float(_Step_3294b1fbfbac4983aafad1d5792e2e74_Out_2,
                                     _Step_c348379a9f9e4f4394fa420c9b13646c_Out_2,
                                     _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2);
                float _Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2;
                Unity_Multiply_float(_Multiply_96471088ddcb4ba684d66cabad42e275_Out_2,
                                     _Subtract_73e5efdece91432c83e9d3ebf0c652c2_Out_2,
                                     _Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2);
                float4 _Property_68bc14fe64ce4652b591e199b08f8ed0_Out_0 = IsGammaSpace()
                                                                              ? LinearToSRGB(
                                                                                  Color_de73dd9654404bbab6ed0654319cd8fb)
                                                                              : Color_de73dd9654404bbab6ed0654319cd8fb;
                float4 _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2;
                Unity_Multiply_float((_Multiply_b7bc4e7617b7449ca89be6202d91e610_Out_2.xxxx),
                                     _Property_68bc14fe64ce4652b591e199b08f8ed0_Out_0,
                                     _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2);
                float4 _Add_28171aa2d98042529a0e41bbc978ebca_Out_2;
                Unity_Add_float4(_Combine_39a48d2861b349549a9c73c65d24970b_RGBA_4,
                                 _Multiply_f13342a342fa4257aebd2acb4fef2f0f_Out_2,
                                 _Add_28171aa2d98042529a0e41bbc978ebca_Out_2);
                float _Split_5a528ac060c34cfa978157175151a0d8_R_1 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[0];
                float _Split_5a528ac060c34cfa978157175151a0d8_G_2 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[1];
                float _Split_5a528ac060c34cfa978157175151a0d8_B_3 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[2];
                float _Split_5a528ac060c34cfa978157175151a0d8_A_4 = _Add_28171aa2d98042529a0e41bbc978ebca_Out_2[3];
                surface.BaseColor = (_Add_28171aa2d98042529a0e41bbc978ebca_Out_2.xyz);
                surface.Alpha = _Split_5a528ac060c34cfa978157175151a0d8_A_4;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);


                output.WorldSpacePosition = input.positionWS;
                output.uv0 = input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
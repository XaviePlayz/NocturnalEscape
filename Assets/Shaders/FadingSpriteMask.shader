Shader "Custom/FadingSpriteMask"
{
    Properties
    {
        _Color("Main Color", Color) = (.5, .5, .5, 1)
        _Radius("Flashlight Radius", Range(0, 1)) = 0.5
    }

        SubShader
    {
        Tags { "Queue" = "Overlay" }

        Stencil
        {
            Ref 1
            Comp always
            Pass replace
        }

        CGPROGRAM
        #pragma surface surf Lambert

        fixed4 _Color;
        float _Radius;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Calculate distance from the center
            float dist = distance(IN.uv_MainTex, 0.5);

            // Apply alpha based on distance
            o.Alpha = smoothstep(1.0 - _Radius, 1.0, dist);
            o.Emission = _Color.rgb;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
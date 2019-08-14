 Shader "GUI/Reverse" {
	 Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	 }
 
	 SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
    
		Pass {
		   AlphaTest Greater 0.5
		   Blend SrcColor DstColor
		   BlendOp Sub
		   SetTexture [_MainTex] {
			  combine previous, texture
		   }
		}
	 }
 }
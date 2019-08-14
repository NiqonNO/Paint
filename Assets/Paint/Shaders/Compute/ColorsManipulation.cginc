	float4 hsv_to_rgb(float4 color) {
		float R = abs(color.x * 6 - 3) - 1;
		float G = 2 - abs(color.x * 6 - 2);
		float B = 2 - abs(color.x * 6 - 4);
		float3 rgb = saturate(float3(R,G,B));
		return float4(((rgb - 1) * color.y + 1) * color.z, color.w);
	}
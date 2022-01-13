uniform extern float4x4 gWVP;

struct OutputVS
{
	float4 posH : POSITION0;
	float4 color : COLOR0;
};

OutputVS TransformVS(float3 posL : POSITION0, float4 color : COLOR0)
{
	// Zero out our output
	OutputVS outVS = (OutputVS)0;

	// Transform to homogeneous clip space
	outVS.posH = mul(float4(posL, 1.0f), gWVP);
	outVS.color = color;

	return outVS;
}

float4 TransformPS(float4 color : COLOR0) : COLOR
{
	return color;
}

technique TransformTech
{
	pass PO
	{
		vertexShader = compile vs_1_1 TransformVS();
		pixelShader = compile ps_2_0 TransformPS();
	}
};
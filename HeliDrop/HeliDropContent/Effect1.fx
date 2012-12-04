uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float brightness;

float4 PixelShaderFunction(float2 curCoord: TEXCOORD0) : COLOR
{
	float4 Color; 
    Color = tex2D( ScreenS , curCoord.xy) * brightness; 
    return Color;
}
technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}

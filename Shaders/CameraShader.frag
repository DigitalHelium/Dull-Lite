#version 430 core

layout(location=0) out vec4 FragColor;
uniform sampler2D renderTexture;
in vec2 TexCoords;

void main()
{
/*
	int idx = int(gl_FragCoord.y)*image_width+int(gl_FragCoord.x);
	float t = mix(0.0,1.0,records[idx].t/5);
	if(records[idx].hit_anything)
		//FragColor = vec4(records[idx].normal,1);
		FragColor = vec4(vec3(t),1);
		//FragColor = vec4(records[idx].mat.albedo,1);
	else
		FragColor = vec4(vec3(1),0);*/

		FragColor = texture(renderTexture,TexCoords);
}
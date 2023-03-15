#version 430 core
/*
struct Ray{
	vec4 origin;
	vec4 direction;
};

layout(std140, binding = 0) buffer Rays{
	Ray rays[];
};

struct Material{
	vec3 albedo;
	float type;
}; 

struct HitRecord{
	vec3 p;
	bool hit_anything;
	vec3 normal;
	float t;
	Material mat;
};

layout(std140, binding = 1) buffer Records{
	HitRecord records[];
};
 uniform int image_width;*/

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
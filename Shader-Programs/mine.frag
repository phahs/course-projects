#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 484
*/

varying vec3 myNormal;
varying vec4 myVertex;

uniform vec4 light2_position;
uniform vec4 light3_position;

void main()
{
	vec4 ambient = vec4(0.172, 0.171, 0.177, 1.0);
	vec4 diffuseMaterial = vec4(0.0, 0.0, 0.05, 1.0);
	vec4 specularMaterial = vec4(0.65, 0.0, 0.0, 1.0);
	float specularPower = 2.5;

	const vec3 eyePos = vec3(0, 0, 0);
	vec4 _myPos = gl_ModelViewMatrix * myVertex;
	vec3 myPos = _myPos.xyz / _myPos.w;
	vec3 myDir = normalize(eyePos - myPos);

	vec3 normal = normalize(gl_NormalMatrix * myNormal);

	//Light 2
	vec3 position2 = light2_position.xyz / light2_position.w;
	vec3 direction2 = normalize(position2 - eyePos);
	float nDotL2 = dot(direction2, normal);
	// R = (2(N dot L) * N - L)
	vec3 reflection2 = 2 * nDotL2 * normal - direction2;
	vec4 diffuse2 = clamp(diffuseMaterial * max(dot(normal, direction2), 0.0), 0.0, 1.0);
	vec4 specular2 = clamp(specularMaterial * pow(max(dot(reflection2, myDir), 0.0), specularPower), 0.0, 1.0);

	//Light 3
	vec3 position3 = light3_position.xyz / light3_position.w;
	vec3 direction3 = normalize(position3 - eyePos);
	float nDotL3 = dot(direction3, normal);
	// R = (2(N dot L) * N - L)
	vec3 reflection3 = 2 * nDotL3 * normal - direction3;
	vec4 diffuse3 = clamp(diffuseMaterial * max(dot(normal, direction3), 0.0), 0.0, 1.0);
	vec4 specular3 = clamp(specularMaterial * pow(max(dot(reflection3, myDir), 0.0), specularPower), 0.0, 1.0);

	vec4 diffuse = diffuse2 + diffuse3;
	vec4 specular = specular2 + specular3;

	gl_FragColor = ambient + diffuse + specular;
}
#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 486
*/

varying vec3 myNormal;
varying vec4 myVertex;

uniform vec4 light0_position;
uniform vec4 light1_position;
uniform vec4 ambient;
uniform vec4 diffuse;
uniform vec4 specular;

void main()
{
	float specularPower = 2.5;

	const vec3 eyePos = vec3(0, 0, 0);
	vec4 _myPos = gl_ModelViewMatrix * myVertex;
	vec3 myPos = _myPos.xyz / _myPos.w;
	vec3 myDir = normalize(eyePos - myPos);

	vec3 normal = normalize(gl_NormalMatrix * myNormal);

	//Light 0
	vec3 position0 = light0_position.xyz / light0_position.w;
	vec3 direction0 = normalize(position0 - eyePos);
	float nDotL0 = dot(direction0, normal);
	// R = (2(N dot L) * N - L)
	vec3 reflection0 = 2 * nDotL0 * normal - direction0;
	vec4 diffuse0 = clamp(diffuse * max(dot(normal, direction0), 0.0), 0.0, 1.0);
	vec4 specular0 = clamp(specular * pow(max(dot(reflection0, myDir), 0.0), specularPower), 0.0, 1.0);

	//Light 1
	vec3 position1 = light1_position.xyz / light1_position.w;
	vec3 direction1 = normalize(position1 - eyePos);
	float nDotL1 = dot(direction1, normal);
	// R = (2(N dot L) * N - L)
	vec3 reflection1 = 2 * nDotL1 * normal - direction1;
	vec4 diffuse1 = clamp(diffuse * max(dot(normal, direction1), 0.0), 0.0, 1.0);
	vec4 specular1 = clamp(specular * pow(max(dot(reflection1, myDir), 0.0), specularPower), 0.0, 1.0);

	vec4 diffuseMaterial = diffuse0 + diffuse1;
	vec4 specularMaterial = specular0 + specular1;

	gl_FragColor = ambient + diffuseMaterial + specularMaterial;
}
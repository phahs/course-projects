#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 486
*/

varying vec3 myNormal;
varying vec4 myVertex;

void main()
{
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
	
	myVertex = gl_Vertex;
	myNormal = gl_Normal;
}
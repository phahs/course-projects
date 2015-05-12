#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 484
*/
varying vec4 color;

varying vec3 N;
varying vec3 V;

uniform vec4 light0_position;
uniform vec4 light1_position;

void main(){
  V = vec3(gl_ModelViewMatrix * gl_Vertex);
  N = normalize(gl_NormalMatrix * gl_Normal);
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;  

  // light 0
  vec3 lightPosition0 = light0_position.xyz / light0_position.w;
  vec3 L0 = normalize(lightPosition0 - V);
  vec3 E0 = normalize(-V);
  vec3 R0 = normalize(reflect(-L0, N)); 

  // light 1
  vec3 lightPosition1 = light1_position.xyz / light1_position.w;
  vec3 L1 = normalize(lightPosition1 - V);
  vec3 E1 = normalize(-V);
  vec3 R1 = normalize(reflect(-L1, N)); 

  vec3 P = vec3(1.0, 2.0, 20.0);
  vec3 W = vec3(1.0, 1.0, 1.0);

  color.r = W.x * pow(((max(dot(N,L0), 0.0) + 1.0) / 2.0), P.x);
  color.g = W.y * pow(((max(dot(N,L0), 0.0) + 1.0) / 2.0), P.y);
  color.b = W.z * pow(((max(dot(N,L0), 0.0) + 1.0) / 2.0), P.z);

  color.r += W.x * pow(((max(dot(N,L1), 0.0) + 1.0) / 2.0), P.x);
  color.g += W.y * pow(((max(dot(N,L1), 0.0) + 1.0) / 2.0), P.y);
  color.b += W.z * pow(((max(dot(N,L1), 0.0) + 1.0) / 2.0), P.z);
}

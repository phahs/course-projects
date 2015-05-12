#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 484
*/
// shade both sides of the teapot
uniform vec4 light2_position;
uniform vec4 light3_position;

varying vec4 myColor;

void main( )
{
  // diffuse and specular materials are stand-ins for W (from the given color equation)
  //specularPower is the stand-in for P

  // give it a blue tinge
  vec4 diffuseMaterial = vec4(0.0, 0.0, 0.05, 1.0);
  // red shininess
  vec4 specularMaterial = vec4(0.65, 0.0, 0.0, 1.0);
  //relatively large bright spot on the teapot.
  float specularPower = 2.5;
  
  // transform the vertex
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

  // transform the normal to model view
  vec3 normal = normalize(gl_NormalMatrix * gl_Normal);
  // get the camera position
  vec3 viewer = vec3(gl_ModelViewMatrix * gl_Vertex);
  // get the gaze vector
  vec3 gaze = normalize(-viewer);

  // light 2
  // get position of light
  vec3 _light_position2 = light2_position.xyz / light2_position.w;
  vec3 light2 = normalize(_light_position2 - viewer);
  // determine the reflection vector
 */ /*
  what it does:
  (2(N dot L)N - L)
  2 times the dot product of the normal and the light vectors times the normal vector minus the light vector
  */ /*
  vec3 reflection2 = normalize(reflect(-light2, normal));
  
  // find color values for ambient, diffuse and specular
  // ambient is the "glow" the teapot will give off by existing in space, think bottom layer of paint.
  vec4 ambient2 = vec4(0.0, 0.0, 0.0, 1.0);
  // diffuse is flat color, or the uniform reflection of light
  vec4 diffuse2 = clamp(diffuseMaterial * max(dot(normal, light2), 0.0), 0.0, 1.0);
  // specular is the highlight of light on an object
  vec4 specular2 = clamp(specularMaterial * pow(max(dot(reflection2, gaze), 0.0), specularPower), 0.0, 1.0);

  // light 3
  vec3 _light_position3 = light3_position.xyz / light3_position.w;
  vec3 light3 = normalize(_light_position3 - viewer);
  vec3 reflection3 = normalize(reflect(-light3, normal));
  
  vec4 ambient3 = vec4(0.172, 0.171, 0.177, 1.0);
  vec4 diffuse3 = clamp(diffuseMaterial * max(dot(normal, light3), 0.0), 0.0, 1.0);
  vec4 specular3 = clamp(specularMaterial * pow(max(dot(reflection3, gaze), 0.0), specularPower), 0.0, 1.0);

  // transform the vertex to model view
  vec4 vertex_in_modelview_space = gl_ModelViewMatrix * gl_Vertex;

  // add the values from the two lights
  vec4 ambient = ambient2 + ambient3;
  vec4 diffuse = diffuse2 + diffuse3;
  vec4 specular = specular2 + specular3;

  // send the color to mine.frag
  myColor = ambient + diffuse + specular;
  
}

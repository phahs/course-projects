#version 120
/*
Philip Hahs
phil.hahs@gmail.com
CS 484
*/

varying vec4 myColor;

void main( )
{
// take the color calculated in mine.vert and send it to the GPU
  gl_FragColor = myColor;
}
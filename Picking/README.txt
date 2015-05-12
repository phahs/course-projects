/* =========================================
Philip Hahs
phil.hahs@gmail.com
CS 486

This README will explain any bugs and features along with a description of what
was or was not completed in the solution.
=========================================*/

What is working as I intend:
  The scene has three models in it. One bunny and two boxes. The vertice information for these models was read in from two
different files and subsequently stored in a single direction linked list. Each object has a bounding sphere. The three
models are inside of a ground plane and a skybox. Part of the ground plane, part of the skybox and all three models
are visible from the cameras' starting position. Two lights are inside the scene. When 'g' is pressed the scene switches
from openGL defined shading to GLSLProgram shading, with a shader I wrote myself. When 'b' is pressed the scene will either
allow or disallow the bounding volumes to be rendered. 

What is not working as I intend:
For openGL:
  You will be unable to pick a bounding sphere while using the openGL settings. The computer has them appearing
ridiculously far away, or so I have been lead to believe by the outputs I get for the distance the center is
from the ray.

Also, I was not able to test the make file. I used one from 484, and changed the names of the files within, but as I have
not tested it myself I don't feel comfortable saying it will work. Windows sucks that way. However, after it is made, 
the command to execute the program will be:

>picking

Any code that is included in this github folder belongs to or was written by either Michael Shafae, David Eberly, 
or Philip Hahs.



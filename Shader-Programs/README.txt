/* =========================================
Philip Hahs
phil.hahs@gmail.com
CS 484

This README will explain any bugs and features along with a description of what
was or was not completed in the solution.
=========================================*/

rotateCameraLeft(), rotateCameraUp(), myRotatef(), myScalef(), myTranslatef(), myFrustumf(),
myOrthof(), myPerspectivef(), and myLookAt() were completed in transformations.cpp. Also, key commands in
two_shaders_glut.cpp for moving the left teapot and scaling the right teapot. NOTE: I changed it so that
w and s would move the left teapot along the y-axis and a and d would move the teapot along the x-axis.

Using OpenGL a camera is set to view two teapots floating in space. When either the up, down, left, or right
arrow buttons are pushed, the camera moves 5 degrees in that direction to the effect of being able to view
the teapots at any angle from any axis. If v or b are pressed the teapot that starts on the right will be
uniformly scaled. If w, a, s, or d are pressed the teapot that starts on the left will be translated along
either the x or y axis. If o is pressed the view of the teapots will become orthographic. Pressing g will
toggle betwee myLookAt() and gluLookAt(). Pressing p will toggle between myPerspectivef() and glPerspective().

All code, except for that inside the functions mentioned above and both mine.vert and mine.frag, is from Michael Shafea. Code found in
golden.vert and golden.frag was taken from the files provided in psurfviewer.glsl. A few modifications were
made so that the light was at a fixed position and not coming from the camera.

There are no external dependancies.
/* =========================================
Philip Hahs
phil.hahs@gmail.com
CS 484

This README will explain any bugs and features along with a description of what
was or was not completed in the solution.
=========================================*/

rotateCameraLeft(), rotateCameraUp(), myRotatef(), myTranslatef(), and myLookAt() were all completed to allow
the camera to rotate around the teapot. rotateCameraLeft() and rotateCameraUp() will work properly in isolation.
If the camera has been moved via rotateCameraUp(), any more calls of rotateCameraLeft() will result in a... unique
rotation of the camera which distorts the teapot. The farther the y coordinate of the camera is from being
in line with the global x vector will result in greater distortion. After the distortion begins it is
difficult to tell if further calls to rotateCameraUp() will display the proper rotation.

Using OpenGL a camera is set to view a teapot floating in space. When either the up, down, left, or right
arrow buttons are pushed, the camera moves 5 degrees in that direction to the effect of being able to view
the teapot at any angle from any axis.

All code, except for that inside rotatCameraLeft(), rotateCameraUp(), myRotatef(), myTranslatef(), and
myLookAt() is from Michael Shafea.

There are no external dependancies.
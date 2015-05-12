// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 484
// 
// Simple header file for glut_teapot.cpp module.
//
// $Id: glut_teapot.h 4925 2014-04-16 09:45:07Z mshafae $
//
// STUDENTS DO NOT NEED TO MAKE ANY CHANGES TO THIS FILE.
//

#ifdef _WIN32
#include <Windows.h>
#endif

#ifdef __APPLE__
/* Apple's weird location of their OpenGL & GLUT implementation */
#include <OpenGL/gl.h>
#include <OpenGL/glu.h>
#else
/* Unix, Linux, and BSD */
#include <GL/gl.h>
#include <GL/glu.h>
#endif

#ifndef _GLUT_TEAPOT_H_
#define _GLUT_TEAPOT_H_

#ifdef __cplusplus 
extern "C" {
#endif

#define GLUTAPIENTRY

	void _teapot(GLint grid, GLdouble scale, GLenum type);

	void _glutSolidTeapot(GLdouble scale);

	void _glutWireTeapot(GLdouble scale);

#ifdef __cplusplus
}
#endif

#endif

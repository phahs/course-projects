// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Contains the structure that will hold the data
// pertinent to makeing the faces of an object.
//
#include "Vertex.h"

#ifndef FACE_H
#define FACE_H

struct Face
{
	Vertex* a;
	Vertex* b;
	Vertex* c;
	double fNormal_x = 0;
	double fNormal_y = 0;
	double fNormal_z = 0;

	Face* next = nullptr;
};
#endif
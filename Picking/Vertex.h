// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Contains the structure that holds the data of
// every vertex in a model.
//
#ifndef VERTEX_H
#define VERTEX_H

struct Vertex
{
	double x = 0;
	double y = 0;
	double z = 0;
	double vNormal_x = 0;
	double vNormal_y = 0;
	double vNormal_z = 0;
	int vNumber = 0;

	Vertex *next = nullptr;
};
#endif
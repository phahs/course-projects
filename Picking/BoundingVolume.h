// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Holds my Bounding Volume class and the two
// Bounding Volume sub-classes, SphereBV and BoxBV
//
#include "Vertex.h"
#include "Face.h"
#define GLFW_INCLUDE_GLU
#include <GLFW\glfw3.h>
#include <cmath>

#ifndef BOUNDINGVOLUME_H
#define BOUNDINGVOLUME_H

enum boundingType { BV_NULL, BV_SPHERE, BV_BOX };

class BoundingVolume
{
private:
	char _type;

public:
	BoundingVolume(char type)
	{
		_type = type;
	}

	char getType()
	{
		return _type;
	}
};

class SphereBV : public BoundingVolume
{
private:
	double _center_x;
	double _center_y;
	double _center_z;
	double _radius;

public:
	GLuint startList;

	SphereBV(double x, double y, double z, double radius) : BoundingVolume(BV_SPHERE)
	{
		_center_x = x;
		_center_y = y;
		_center_z = z;
		_radius = radius;
	}

	void buildBSVertices()
	{
		GLUquadricObj * pointTool;
		startList = glGenLists(1);
		pointTool = gluNewQuadric();
		gluQuadricDrawStyle(pointTool, GLU_FILL);
		gluQuadricNormals(pointTool, GLU_SMOOTH);

		glNewList(startList, GL_COMPILE);
		gluSphere(pointTool, _radius, 50, 50);
		glEndList();

		gluDeleteQuadric(pointTool);
	}

	void adjustVertices(Vertex* vhead, int vertexNum)
	{
		Vertex* tempCurrent = vhead;
		for (int i = 0; i < vertexNum; i++)
		{
			tempCurrent->x = tempCurrent->x - _center_x;
			tempCurrent->y = tempCurrent->y - _center_y;
			tempCurrent->z = tempCurrent->z - _center_z;
			tempCurrent = tempCurrent->next;
		}
	}

	void calcBoundingSphere(Vertex* vHead, int vertexNum)
	{
		Vertex* tempCurrent;
		double maxDistance = 0.0;
		tempCurrent = vHead;
		for (int i = 0; i < vertexNum - 1; i++){
			Vertex* temp = tempCurrent->next;
			for (int j = i + 1; j < vertexNum; j++){
				double dist_x = tempCurrent->x - temp->x;
				double dist_y = tempCurrent->y - temp->y;
				double dist_z = tempCurrent->z - temp->z;
				double totalDist = (dist_x * dist_x) + (dist_y * dist_y) + (dist_z * dist_z);
				if (totalDist > maxDistance)
				{
					_center_x = ((tempCurrent->x) + (temp->x)) * 0.5;
					_center_y = ((tempCurrent->y) + (temp->y)) * 0.5;
					_center_z = ((tempCurrent->z) + (temp->z)) * 0.5;
					_radius = sqrt(totalDist);
					maxDistance = totalDist;
				}
			}
			tempCurrent = tempCurrent->next;
		}
		adjustVertices(vHead, vertexNum);
		buildBSVertices();
	}

	double getRadius()
	{
		return _radius;
	}

	void getCenter(double &centerx, double &centery, double &centerz)
	{
		centerx = _center_x;
		centery = _center_y;
		centerz = _center_z;
	}
};

class BoxBV : public BoundingVolume
{
private:
	Vertex *_corner;
	double _height;
	double _base;
	double _width;

public:
	BoxBV(Vertex *corner, double height, double base, double width) : BoundingVolume(BV_BOX)
	{
		_corner = corner;
		_height = height;
		_base = base;
		_width = width;

	}
};
#endif
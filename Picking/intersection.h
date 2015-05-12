// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Used to determine if a ray cast by a mouse click
// intersects with a bounding sphere
//
#ifndef INTERSECTION_H
#define INTERSECTION_H

#ifdef _WIN32
#include <Windows.h>
#include <cstdlib>
#include <cstdio>
#define _USE_MATH_DEFINES
#include <cmath>
#define sprintf sprintf_s
#define strdup _strdup
#else
#include <cstdlib>
#include <cstdio>
#include <cstring>
#include <cmath>
#endif

#include "Model.h"

#define FP_SP_EPSILON 1e-6
#define FP_DP_EPSILON 1e-15

#define FP_EQUAL(a, b, delta) ( ((a) == (b)) || \
((a)-(delta) < (b) && \
  (a)+(delta) > (b)) )

#define FP_NOT_EQUAL(a, b, delta) ( ((a) != (b)) && \
((a)-(delta) > (b) || \
  (a)+(delta) < (b)) )

#define FP_LT(a, b, delta)  (fabs((a) - (b)) < (delta))

#define FP_GT(a, b, delta)  (fabs((a) - (b)) > (delta))

typedef struct ray3d{
	// origin
	double o[3];
	// direction
	double d[3];
}ray3d;

typedef struct hitd{
	// t value along the ray where intersection occurs
	double t;
	// coordinates of intersection
	double point[3];
}hitd;

#define msglError( ) _msglError( stderr, __FILE__, __LINE__ )

void msglPrintMatrix16dv(char *varName, double matrix[16]){
	int i = 0;
	if (varName != NULL){
		fprintf(stderr, "%s = [\n", varName);
	}
	for (i = 0; i < 4; i++){
		fprintf(stderr, "%.5f %.5f %.5f %.5f\n", matrix[i + 0], matrix[i + 4],
			matrix[i + 8], matrix[i + 12]);
	}
	if (varName != NULL){
		fprintf(stderr, " ]\n");
	}
}

void matCopy3d(double *dest, double *src){
	memcpy(dest, src, sizeof(double) * 9);
}

void vecCopy3d(double *dest, double *src){
	int i;
	for (i = 0; i < 3; i++){
		dest[i] = src[i];
	}
}

void rayEval3d(double *p, ray3d *r, double t){
	int i;
	for (i = 0; i < 3; i++){
		p[i] = r->o[i] + (t * r->d[i]);
	}
}

double vecDot3d(double *a, double *b){
	double accumulate = 0.0;
	int i;
	for (i = 0; i < 3; i++){
		accumulate += a[i] * b[i];
	}
	return(accumulate);
}

void vecCross3d(double *n, double *u, double *v){
	n[0] = u[1] * v[2] - u[2] * v[1];
	n[1] = u[2] * v[0] - u[0] * v[2];
	n[2] = u[0] * v[1] - u[1] * v[0];
}

void vecDifference3d(double *c, double *a, double *b){
	int i;
	for (i = 0; i < 3; i++){
		c[i] = a[i] - b[i];
	}
}

void matMultVec3d(double* vout, double* v, double* m){
	double c[3];
	vecCopy3d(c, v);
	vout[0] = m[0] * c[0] + m[3] * c[1] + m[6] * c[2];
	vout[1] = m[1] * c[0] + m[4] * c[1] + m[7] * c[2];
	vout[2] = m[2] * c[0] + m[5] * c[1] + m[8] * c[2];
}

bool raySphereIntersection(SphereBV *bv, ray3d *r, hitd *h, float *translate, float scale, double t1, double t2){
	double center[3]; // center of the bounding volume
	double radius; // radius of the bounding volume THIS DOES NOT WORK FOR SOME REASON
	double rSqr; // radius squared, diameter if you want to be fancy
	double trans[3]; // normalized translate for same reason why radius does not work
	bv->getCenter(center[0], center[1], center[2]);
	radius = 0.2; // THIS WORKS FOR SOME REASON
	rSqr = SQR(radius); 
	double l[3];
	// yay pythagorean theorum
	double b;
	double aSqr, bSqr, cSqr;
	for (int i = 0; i < 3; i++)
	{
		trans[i] = translate[i];
	}
//	vecNormalize3d(trans[0], trans[1], trans[2], trans[0], trans[1], trans[2]);
	for (int i = 0; i < 3; i++)
	{
		center[i] = center[i] - (trans[i] * 0.070);
	}
	for (int i = 0; i < 3; i++)
	{
		l[i] = center[i] - r->o[i]; // vector from ray's origin to the center of the bounding volume
	}

	cSqr = vecSquaredLength3d(l[0], l[1], l[2]); // gets the hypotenuse

	b = vecDot3d(l, r->d); // l dot ray's direction
	if (FP_LT(b, 0, FP_DP_EPSILON) || cSqr < rSqr)
	{
//		printf("b is negative, and therefore, is pointing the wrong way\n");
//		printf("OR cSqr: %f is less than rSqr: %f\n", cSqr, rSqr);
		return false;
	}
	bSqr = SQR(b);

	aSqr = cSqr - bSqr; // get the last leg of the triangle formed by  the direction of the ray and the vector from the origin and the center

	assert(cSqr == aSqr + bSqr);
	assert(cSqr > bSqr);
	assert(cSqr > aSqr);
	
	if (aSqr > rSqr)
	{
//		printf("aSqr is too large, you did not intersect the sphere\naSqr: %f, rSqr: %f\n", aSqr, rSqr);
		return false;
	}
	double q = sqrt(rSqr - aSqr);
	t1 = aSqr - q;
	t2 = aSqr + q;
	rayEval3d(h->point, r, t1);
	return true;
}

void pick(double x, double y, double modelView[16], double projection[16], ray3d *r){
	double xx, yy, zz;
	GLint vp[4];
	int wx = x;
	int wy;

	glGetIntegerv(GL_VIEWPORT, vp);

	wy = vp[3] - y - 1;

	glReadPixels(wx, wy, 0, 0, GL_DEPTH_COMPONENT, GL_FLOAT, &zz);

	gluUnProject(wx, wy, 0.0, modelView, projection, vp, &xx, &yy, &zz);
	float nPlane[3] = { xx, yy, zz };

	gluUnProject(wx, wy, 1.0, modelView, projection, vp, &xx, &yy, &zz);
	float fPlane[3] = { xx, yy, zz };

	double nearPlane[3], farPlane[3];

	for (int i = 0; i < 3; i++)
	{
		nearPlane[i] = nPlane[i];
		farPlane[i] = fPlane[i];
	}

	for (int i = 0; i < 3; i++)
	{
		r->o[i] = nearPlane[i];
		r->d[i] = farPlane[i] - nearPlane[i];
	}

	vecNormalize3d(r->d[0], r->d[1], r->d[2], r->d[0], r->d[1], r->d[2]);
}
#endif
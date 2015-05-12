// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Procedural module that implements transformations used in
// the homework assignment.
//
// $Id: transformations.cpp 4964 2014-04-25 04:43:20Z mshafae $
//
// STUDENTS _MUST_ ADD THEIR CODE INTO THIS FILE
//

#include "transformations.h"
#include "stdio.h"

// Just in case you have a weird cmath header file...
#ifndef M_PI
#define M_PI                3.14159265358979323846264338327950288
#endif
#ifndef PI_OVER_ONE_EIGHTY
#define PI_OVER_ONE_EIGHTY  0.01745329251994329547437168059786927188
#endif
#ifndef ONE_EIGHTY_OVER_PI
#define ONE_EIGHTY_OVER_PI  57.29577951308232286464772187173366546631
#endif

// Don't forget:
// OpenGL's unit for angles is degrees.
// C math library's unit for angles is radians.
#ifndef DEG2RAD
#define DEG2RAD( theta ) ((theta) * (PI_OVER_ONE_EIGHTY))
#endif
#ifndef RAD2DEG
#define RAD2DEG( theta ) ((theta) * (ONE_EIGHTY_OVER_PI))
#endif
// Quick-n-dirty absolute value macro
#ifndef ABS
#define ABS( x ) ((x) < (0) ? (-x) : (x))
#endif
// Don't use pow( ) to square a value.
#ifndef SQR
#define SQR( x ) ((x) * (x))
#endif


#ifndef __SOLUTION__

void rotateCameraLeft(float degrees, float *eyePosition, float *centerPosition, float *upVector){
	// Please implement this function.

	GLdouble gaze[3] = { centerPosition[0] - eyePosition[0], centerPosition[1] - eyePosition[1], centerPosition[2] - eyePosition[2] };
	GLdouble normGaze[3];
	double gazeLength;
	GLdouble normUp[3];
	double upLength;
	GLdouble right[3];
	GLdouble normRight[3];
	double rightLength;
	GLfloat rotate[16];
	GLfloat rotEye[3];
	GLfloat rotUp[3];

	gazeLength = sqrt(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2]));

	for (int i = 0; i < 3; i++)
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrt(SQR(upVector[0]) + SQR(upVector[1]) + SQR(upVector[2]));

	for (int i = 0; i < 3; i++)
	{
		normUp[i] = upVector[i] / upLength;
	}

	right[0] = normGaze[1] * normUp[2] - normGaze[2] * normUp[1];
	right[1] = normGaze[2] * normUp[0] - normGaze[0] * normUp[2];
	right[2] = normGaze[0] * normUp[1] - normGaze[1] * normUp[0];

	rightLength = sqrt(SQR(right[0]) + SQR(right[1]) + SQR(right[2]));

	for (int i = 0; i < 3; i++)
	{
		normRight[i] = right[i] / rightLength;
	}

	myRotatef(rotate, degrees, normUp[0], normUp[1], normUp[2]);

	rotEye[0] = rotate[0] * eyePosition[0] + rotate[4] * eyePosition[1] + rotate[8] * eyePosition[2];
	rotEye[1] = rotate[1] * eyePosition[0] + rotate[5] * eyePosition[1] + rotate[9] * eyePosition[2];
	rotEye[2] = rotate[2] * eyePosition[0] + rotate[6] * eyePosition[1] + rotate[10] * eyePosition[2];

	rotUp[0] = rotate[0] * normUp[0] + rotate[4] * normUp[1] + rotate[8] * normUp[2];
	rotUp[1] = rotate[1] * normUp[0] + rotate[5] * normUp[1] + rotate[9] * normUp[2];
	rotUp[2] = rotate[2] * normUp[0] + rotate[6] * normUp[1] + rotate[10] * normUp[2];

	eyePosition[0] = rotEye[0];
	eyePosition[1] = rotEye[1];
	eyePosition[2] = rotEye[2];

	upVector[0] = rotUp[0];
	upVector[1] = rotUp[1];
	upVector[2] = rotUp[2];
}

void rotateCameraUp(float degrees, float *eyePosition, float *centerPosition, float *upVector){
	// Please implement this function.

	GLdouble gaze[3] = { centerPosition[0] - eyePosition[0], centerPosition[1] - eyePosition[1], centerPosition[2] - eyePosition[2] };
	GLdouble normGaze[3];
	double gazeLength;
	GLdouble normUp[3];
	double upLength;
	GLdouble right[3];
	GLdouble normRight[3];
	double rightLength;
	GLfloat rotate[16];
	GLfloat rotEye[3];
	GLfloat rotUp[3];

	gazeLength = sqrt(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2]));

	for (int i = 0; i < 3; i++)
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrt(SQR(upVector[0]) + SQR(upVector[1]) + SQR(upVector[2]));

	for (int i = 0; i < 3; i++)
	{
		normUp[i] = upVector[i] / upLength;
	}

	right[0] = normGaze[1] * normUp[2] - normGaze[2] * normUp[1];
	right[1] = normGaze[2] * normUp[0] - normGaze[0] * normUp[2];
	right[2] = normGaze[0] * normUp[1] - normGaze[1] * normUp[0];

	rightLength = sqrt(SQR(right[0]) + SQR(right[1]) + SQR(right[2]));

	for (int i = 0; i < 3; i++)
	{
		normRight[i] = right[i] / rightLength;
	}

	myRotatef(rotate, degrees, normRight[0], normRight[1], normRight[2]);

	rotEye[0] = rotate[0] * eyePosition[0] + rotate[4] * eyePosition[1] + rotate[8] * eyePosition[2];
	rotEye[1] = rotate[1] * eyePosition[0] + rotate[5] * eyePosition[1] + rotate[9] * eyePosition[2];
	rotEye[2] = rotate[2] * eyePosition[0] + rotate[6] * eyePosition[1] + rotate[10] * eyePosition[2];

	rotUp[0] = rotate[0] * normUp[0] + rotate[4] * normUp[1] + rotate[8] * normUp[2];
	rotUp[1] = rotate[1] * normUp[0] + rotate[5] * normUp[1] + rotate[9] * normUp[2];
	rotUp[2] = rotate[2] * normUp[0] + rotate[6] * normUp[1] + rotate[10] * normUp[2];

	eyePosition[0] = rotEye[0];
	eyePosition[1] = rotEye[1];
	eyePosition[2] = rotEye[2];

	upVector[0] = rotUp[0];
	upVector[1] = rotUp[1];
	upVector[2] = rotUp[2];
}

#else
#include "transformations_solution.cpp"
#endif


void myTranslatef(GLfloat *matrix, GLfloat x, GLfloat y, GLfloat z){
	// Please implement this function.
	matrix[0] = 1; matrix[4] = 0; matrix[8] = 0; matrix[12] = x;
	matrix[1] = 0; matrix[5] = 1; matrix[9] = 0; matrix[13] = y;
	matrix[2] = 0; matrix[6] = 0; matrix[10] = 1; matrix[14] = z;
	matrix[3] = 0; matrix[7] = 0; matrix[11] = 0; matrix[15] = 1;
}

void myScalef(GLfloat *matrix, GLfloat x, GLfloat y, GLfloat z){
	// Please implement this function.

	matrix[0] = x; matrix[4] = 0; matrix[8] = 0; matrix[12] = 0;
	matrix[1] = 0; matrix[5] = y; matrix[9] = 0; matrix[13] = 0;
	matrix[2] = 0; matrix[6] = 0; matrix[10] = z; matrix[14] = 0;
	matrix[3] = 0; matrix[7] = 0; matrix[11] = 0; matrix[15] = 1;
}

void myRotatef(GLfloat *matrix,
	GLfloat angle, GLfloat x, GLfloat y, GLfloat z){
	// Remember the Rodrigues' rotation formula?
	// R = I + S * sin(theta) + Ssquared * (1 - cos(theta))
	GLfloat identity[16];
	GLfloat skewMatrix[16];
	GLfloat skewMatrixSqr[16];
	GLfloat skewXsin[16];
	GLfloat skewSqrXcos[16];
	GLfloat radAngle = DEG2RAD(angle);

	identity[0] = 1; identity[4] = 0; identity[8] = 0; identity[12] = 0;
	identity[1] = 0; identity[5] = 1; identity[9] = 0; identity[13] = 0;
	identity[2] = 0; identity[6] = 0; identity[10] = 1; identity[14] = 0;
	identity[3] = 0; identity[7] = 0; identity[11] = 0; identity[15] = 1;

	skewMatrix[0] = 0; skewMatrix[4] = z; skewMatrix[8] = -y; skewMatrix[12] = 0;
	skewMatrix[1] = -z; skewMatrix[5] = 0; skewMatrix[9] = x; skewMatrix[13] = 0;
	skewMatrix[2] = y; skewMatrix[6] = -x; skewMatrix[10] = 0; skewMatrix[14] = 0;
	skewMatrix[3] = 0; skewMatrix[7] = 0; skewMatrix[11] = 0; skewMatrix[15] = 0;

	matrixMult(skewMatrixSqr, skewMatrix, skewMatrix);

	for (int i = 0; i < 16; i++)
	{
		skewXsin[i] = skewMatrix[i] * sin(radAngle);
		skewSqrXcos[i] = skewMatrixSqr[i] * (1 - cos(radAngle));
	}

	for (int i = 0; i < 16; i++)
	{
		matrix[i] = identity[i] + skewXsin[i] + skewSqrXcos[i];
	}
}

void myLookAt(GLfloat *matrix,
	GLdouble eyeX, GLdouble eyeY, GLdouble eyeZ,
	GLdouble centerX, GLdouble centerY, GLdouble centerZ,
	GLdouble upX, GLdouble upY, GLdouble upZ){
	// Please implement this function.

	GLdouble gaze[3] = { centerX - eyeX, centerY - eyeY, centerZ - eyeZ };
	GLdouble up[3] = { upX, upY, upZ };
	GLdouble normGaze[3];
	double gazeLength;
	GLdouble normUp[3];
	double upLength;
	GLdouble right[3];
	GLdouble normRight[3];
	double rightLength;
	GLfloat translate[16];
	GLfloat modelView[16];

	gazeLength = sqrt(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2]));

	for (int i = 0; i < 3; i++)
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrt(SQR(up[0]) + SQR(up[1]) + SQR(up[2]));

	for (int i = 0; i < 3; i++)
	{
		normUp[i] = up[i] / upLength;
	}

	right[0] = normGaze[1] * normUp[2] - normGaze[2] * normUp[1];
	right[1] = normGaze[2] * normUp[0] - normGaze[0] * normUp[2];
	right[2] = normGaze[0] * normUp[1] - normGaze[1] * normUp[0];

	rightLength = sqrt(SQR(right[0]) + SQR(right[1]) + SQR(right[2]));

	for (int i = 0; i < 3; i++)
	{
		normRight[i] = right[i] / rightLength;
	}

	modelView[0] = normRight[0]; modelView[4] = normRight[1]; modelView[8] = normRight[2]; modelView[12] = 0;
	modelView[1] = normUp[0];	 modelView[5] = normUp[1];	  modelView[9] = normUp[2];	   modelView[13] = 0;
	modelView[2] = -normGaze[0]; modelView[6] = -normGaze[1]; modelView[10] = -normGaze[2]; modelView[14] = 0;
	modelView[3] = 0;			 modelView[7] = 0;			  modelView[11] = 0;			modelView[15] = 1;

	myTranslatef(translate, -eyeX, -eyeY, -eyeZ);

	matrixMult(matrix, modelView, translate);
}

void myFrustum(GLfloat *matrix,
	GLdouble left, GLdouble right, GLdouble bottom,
	GLdouble top, GLdouble zNear, GLdouble zFar){
	// Please implement this function.

	/*	// This code is just a placeholder to demonstrate how this procedure
	// returns the LookAt matrix by reference.
	// YOU MUST REMOVE THE CODE BELOW AND WRITE YOUR OWN ROUTINE.
	int mode;
	glGetIntegerv(GL_MATRIX_MODE, &mode);
	glPushMatrix();
	glLoadIdentity();
	glFrustum(left, right, bottom, top, zNear, zFar);
	glGetFloatv(mode == GL_MODELVIEW ?
	GL_MODELVIEW_MATRIX : GL_PROJECTION_MATRIX,
	matrix);
	glPopMatrix();
	*/
	GLdouble x = ((2 * zNear) / (right - left));
	GLdouble y = ((2 * zNear) / (top - bottom));
	GLdouble z = ((zFar + zNear) / (zFar - zNear));
	GLdouble A = ((right + left) / (right - left));
	GLdouble B = ((top + bottom) / (top - bottom));
	GLdouble C = -((2 * zNear * zFar) / (zFar - zNear));

	matrix[0] = x; matrix[4] = 0; matrix[8] = A; matrix[12] = 0;
	matrix[1] = 0; matrix[5] = y; matrix[9] = B; matrix[13] = 0;
	matrix[2] = 0; matrix[6] = 0; matrix[10] = C; matrix[14] = C;
	matrix[3] = 0; matrix[7] = 0; matrix[11] = -1; matrix[15] = 0;
}

void myPerspective(GLfloat *matrix,
	GLdouble fovy, GLdouble aspect,
	GLdouble zNear, GLdouble zFar){
	// Please implement this function.
	/*
	int mode;
	glGetIntegerv(GL_MATRIX_MODE, &mode);
	glMatrixMode(GL_MODELVIEW);
	glPushMatrix();
	glLoadIdentity();
	gluPerspective(fovy, aspect, zNear, zFar);
	glGetFloatv(mode == GL_MODELVIEW ?
	GL_MODELVIEW_MATRIX : GL_PROJECTION_MATRIX,
	matrix);
	glPopMatrix();
	glMatrixMode(mode);
	*/
	//	/*
	double f = 1 / tan(DEG2RAD(fovy / 2));

	matrix[0] = f / aspect; matrix[4] = 0; matrix[8] = 0; matrix[12] = 0;
	matrix[1] = 0; matrix[5] = f; matrix[9] = 0; matrix[13] = 0;
	matrix[2] = 0; matrix[6] = 0; matrix[10] = (zFar + zNear) / (zNear - zFar); matrix[14] = (2 * (zFar * zNear)) / (zNear - zFar);
	matrix[3] = 0; matrix[7] = 0; matrix[11] = -1; matrix[15] = 0;
	//	*/
}

void myOrtho(GLfloat *matrix,
	GLdouble left, GLdouble right, GLdouble bottom,
	GLdouble top, GLdouble zNear, GLdouble zFar){
	// Please implement this function.

	int mode;
	glGetIntegerv(GL_MATRIX_MODE, &mode);
	glPushMatrix();
	glLoadIdentity();
	glOrtho(left, right, bottom, top, zNear, zFar);
	glGetFloatv(mode == GL_MODELVIEW ?
	GL_MODELVIEW_MATRIX : GL_PROJECTION_MATRIX,
						  matrix);
	glPopMatrix();
	/*
	matrix[0] = 2 / (right - left); matrix[4] = 0; matrix[8] = 0; matrix[12] = -((right + left) / (right - left));
	matrix[1] = 0; matrix[5] = 2 / (top - bottom); matrix[9] = 0; matrix[13] = -((top + bottom) / (top - bottom));
	matrix[2] = 0; matrix[6] = 0; matrix[10] = -2 / (zFar - zNear); matrix[14] = -((zFar + zNear) / (zFar - zNear));
	matrix[3] = 0; matrix[7] = 0; matrix[11] = 0; matrix[15] = 1;
	*/
}

void matrixMult(GLfloat * result, GLfloat * a, GLfloat * b)
{
	/*
	float b00 = b[0]; float b01 = b[4]; float b02 = b[8]; float b03 = b[12];
	float b10 = b[1]; float b11 = b[5]; float b12 = b[9]; float b13 = b[13];
	float b20 = b[2]; float b21 = b[6]; float b22 = b[10]; float b23 = b[14];
	float b30 = b[3]; float b31 = b[7]; float b32 = b[11]; float b33 = b[15];


	for (int i = 0; i < 4; i++)
	{
	result[0 + i] = a[i] * b00 + a[i + 4] * b10 + a[i + 8] * b20 + a[i + 12] * b30;
	result[4 + i] = a[i] * b01 + a[i + 4] * b11 + a[i + 8] * b21 + a[i + 12] * b31;
	result[8 + i] = a[i] * b02 + a[i + 4] * b12 + a[i + 8] * b22 + a[i + 12] * b32;
	result[12 + i] = a[i] * b03 + a[i + 4] * b13 + a[i + 8] * b23 + a[i + 12] * b33;
	}
	*/
	//	/*
	// column one
	result[0] = a[0] * b[0] + a[4] * b[1] + a[8] * b[2] + a[12] * b[3];
	result[1] = a[1] * b[0] + a[5] * b[1] + a[9] * b[2] + a[13] * b[3];
	result[2] = a[2] * b[0] + a[6] * b[1] + a[10] * b[2] + a[14] * b[3];
	result[3] = a[3] * b[0] + a[7] * b[1] + a[11] * b[2] + a[15] * b[3];
	//column two
	result[4] = a[0] * b[4] + a[4] * b[5] + a[8] * b[6] + a[12] * b[7];
	result[5] = a[1] * b[4] + a[5] * b[5] + a[9] * b[6] + a[13] * b[7];
	result[6] = a[2] * b[4] + a[6] * b[5] + a[10] * b[6] + a[14] * b[7];
	result[7] = a[3] * b[4] + a[7] * b[5] + a[11] * b[6] + a[15] * b[7];
	// column three
	result[8] = a[0] * b[8] + a[4] * b[9] + a[8] * b[10] + a[12] * b[11];
	result[9] = a[1] * b[8] + a[5] * b[9] + a[9] * b[10] + a[13] * b[11];
	result[10] = a[2] * b[8] + a[6] * b[9] + a[10] * b[10] + a[14] * b[11];
	result[11] = a[3] * b[8] + a[7] * b[9] + a[11] * b[10] + a[15] * b[11];
	// column four
	result[12] = a[0] * b[12] + a[4] * b[13] + a[8] * b[14] + a[12] * b[15];
	result[13] = a[1] * b[12] + a[5] * b[13] + a[9] * b[14] + a[13] * b[15];
	result[14] = a[2] * b[12] + a[6] * b[13] + a[10] * b[14] + a[14] * b[15];
	result[15] = a[3] * b[12] + a[7] * b[13] + a[11] * b[14] + a[15] * b[15];
	//	*/
	/*	printf("First Matrix\n");
	printMat(a);
	printf("Second Matrix\n");
	printMat(b);
	printf("Resulting Matrix\n");
	printMat(result);
	*/
}

void printMat(GLfloat *m)
{
	for (int i = 0; i < 4; i++)
	{
		printf("%.4f, %.4f, %.4f, %.4f\n", m[i], m[i + 4], m[i + 8], m[i + 12]);
	}
}

void printVec(GLfloat *v)
{
	for (int i = 0; i < 3; i++)
	{
		printf("%.4f, %.4f, %.4f\n", v[i], v[i + 1], v[i + 2]);
	}
}
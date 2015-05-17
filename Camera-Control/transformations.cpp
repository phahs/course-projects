// 
//Philip Hahs
//phil.hahs@gmail.com
//CS 484
// 
// Procedural module that implements transformations used in
// the homework assignment.
//
// $Id: transformations.cpp 4897 2014-04-07 05:13:41Z mshafae $
//
// STUDENTS _MUST_ ADD THEIR CODE INTO THIS FILE
//

#include "transformations.h"
#include <stdio.h>

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
	GLfloat identity[16]; // will hold the identity matrix
	GLfloat rMatrix[16]; // will hold the rotation matrix found by myRotatef()
	GLfloat gaze[] = { centerPosition[0] - eyePosition[0], centerPosition[1] - eyePosition[1], centerPosition[2] - eyePosition[2] }; // z-axis vector
	GLfloat normGaze[3]; // normalized z-axis vector
	GLfloat rotEye[3]; // rotated eye position
	GLfloat normUp[3]; // normalized up vector
	GLfloat rotUp[3]; // rotated up vector
	GLfloat right[3]; // right vector
	GLfloat normRight[3]; // normalized right vector
	float gazeLength;
	float upLength;
	float rightLength;
	
	gazeLength = sqrtf(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2])); // find the length of the gaze vector
	
	for (int i = 0; i < 3; i++) // normalize the gaze vector
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrtf(SQR(upVector[0]) + SQR(upVector[1]) + SQR(upVector[2])); // find the length of the up vector

	for (int i = 0; i < 3; i++) // normlaize the up vector
	{
		normUp[i] = upVector[i] / upLength;
	}
	// cross product of the normalized gaze and up vectors to get the right vector
	right[0] = normUp[2] * normGaze[1] - normUp[1] * normGaze[2];
	right[1] = normUp[0] * normGaze[2] - normUp[2] * normGaze[0];
	right[2] = normUp[1] * normGaze[0] - normUp[0] * normGaze[1];

	rightLength = sqrtf(SQR(right[0]) + SQR(right[1]) + SQR(right[2])); // find the length of the right vector

	for (int i = 0; i < 3; i++) // normalize the right vector (even though it already should be normalized)
	{
		normRight[i] = right[i] / rightLength;
	}
	// fill out the identity matrix to pull the rotation matrix into this function
	identity[0] = 1; identity[4] = 0; identity[8] = 0; identity[12] = 0;
	identity[1] = 0; identity[5] = 1; identity[9] = 0; identity[13] = 0;
	identity[2] = 0; identity[6] = 0; identity[10] = 1; identity[14] = 0;
	identity[3] = 0; identity[7] = 0; identity[11] = 0;	identity[15] = 1;

	for (int i = 0; i < 16; i++) // set matrix to the identity matrix
	{
		rMatrix[i] = identity[i];
	}

	myRotatef(rMatrix, -degrees, normUp[0], normUp[1], normUp[2]); // get the rotation matrix for a rotation of degrees around the up vector
	// get the rotated eye position
	rotEye[0] = rMatrix[0] * eyePosition[0] + rMatrix[4] * eyePosition[1] + rMatrix[8] * eyePosition[2];
	rotEye[1] = rMatrix[1] * eyePosition[0] + rMatrix[5] * eyePosition[1] + rMatrix[9] * eyePosition[2];
	rotEye[2] = rMatrix[2] * eyePosition[0] + rMatrix[6] * eyePosition[1] + rMatrix[10] * eyePosition[2];
	// get the rotated up vector
	rotUp[0] = rMatrix[0] * normUp[0] + rMatrix[4] * normUp[1] + rMatrix[8] * normUp[2];
	rotUp[1] = rMatrix[1] * normUp[0] + rMatrix[5] * normUp[1] + rMatrix[9] * normUp[2];
	rotUp[2] = rMatrix[2] * normUp[0] + rMatrix[6] * normUp[1] + rMatrix[10] * normUp[2];
	// set the eye position to be equal to the rotated eye position
	eyePosition[0] = rotEye[0];
	eyePosition[1] = rotEye[1];
	eyePosition[2] = rotEye[2];
	// set the up vector to be equal to the rotated up vector
	upVector[0] = rotUp[0];
	upVector[1] = rotUp[1];
	upVector[2] = rotUp[2];
}

void rotateCameraUp(float degrees, float *eyePosition, float *centerPosition, float *upVector){
	GLfloat identity[16]; // will hold the identity matrix
	GLfloat rMatrix[16]; // will hold the rotation matrix found by myRotatef()
	GLfloat gaze[] = { centerPosition[0] - eyePosition[0], centerPosition[1] - eyePosition[1], centerPosition[2] - eyePosition[2] }; // z-axis vector
	GLfloat normGaze[3]; // normalized z-axis vector
	GLfloat rotEye[3]; // rotated eye position
	GLfloat normUp[3]; // normalized up vector
	GLfloat rotUp[3]; // rotated up vector
	GLfloat right[3]; // right vector
	GLfloat normRight[3]; // normalized right vector
	float gazeLength;
	float upLength;
	float rightLength;
	
	gazeLength = sqrtf(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2])); // find the length of the gaze vector

	for (int i = 0; i < 3; i++) // normalize the gaze vector
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrtf(SQR(upVector[0]) + SQR(upVector[1]) + SQR(upVector[2])); // find the length of the up vector

	for (int i = 0; i < 3; i++) // normlaize the up vector
	{
		normUp[i] = upVector[i] / upLength;
	}
	// cross product of the normalized gaze and up vectors to get the right vector
	right[0] = normUp[2] * normGaze[1] - normUp[1] * normGaze[2];
	right[1] = normUp[0] * normGaze[2] - normUp[2] * normGaze[0];
	right[2] = normUp[1] * normGaze[0] - normUp[0] * normGaze[1];

	rightLength = sqrtf(SQR(right[0]) + SQR(right[1]) + SQR(right[2])); // find the length of the right vector

	for (int i = 0; i < 3; i++) // normalize the right vector (even though it already should be normalized)
	{
		normRight[i] = right[i] / rightLength;
	}
	
	// fill out the identity matrix to pull the rotation matrix into this function
	identity[0] = 1; identity[4] = 0; identity[8] = 0; identity[12] = 0;
	identity[1] = 0; identity[5] = 1; identity[9] = 0; identity[13] = 0;
	identity[2] = 0; identity[6] = 0; identity[10] = 1; identity[14] = 0;
	identity[3] = 0; identity[7] = 0; identity[11] = 0;	identity[15] = 1;

	for (int i = 0; i < 16; i++) // set matrix to the identity matrix
	{
		rMatrix[i] = identity[i];
	}
	
	myRotatef(rMatrix, -degrees, normRight[0], normRight[1], normRight[2]); // get the rotation matrix for a rotation of degrees around the up vector
	// get the rotated eye position
	rotEye[0] = rMatrix[0] * eyePosition[0] + rMatrix[4] * eyePosition[1] + rMatrix[8] * eyePosition[2];
	rotEye[1] = rMatrix[1] * eyePosition[0] + rMatrix[5] * eyePosition[1] + rMatrix[9] * eyePosition[2];
	rotEye[2] = rMatrix[2] * eyePosition[0] + rMatrix[6] * eyePosition[1] + rMatrix[10] * eyePosition[2];
	// get the rotated up vector
	rotUp[0] = rMatrix[0] * normUp[0] + rMatrix[4] * normUp[1] + rMatrix[8] * normUp[2];
	rotUp[1] = rMatrix[1] * normUp[0] + rMatrix[5] * normUp[1] + rMatrix[9] * normUp[2];
	rotUp[2] = rMatrix[2] * normUp[0] + rMatrix[6] * normUp[1] + rMatrix[10] * normUp[2];
	// set the eye position to be equal to the rotated eye position
	eyePosition[0] = rotEye[0];
	eyePosition[1] = rotEye[1];
	eyePosition[2] = rotEye[2];
	// set the up vector to be equal to the rotated up vector
	upVector[0] = rotUp[0];
	upVector[1] = rotUp[1];
	upVector[2] = rotUp[2];
	
}

void myTranslatef(GLfloat *matrix, GLfloat x, GLfloat y, GLfloat z){

	GLfloat tempMatrix[16]; // holds the matrix sent to the function
	GLfloat translateMatrix[16]; // will hold the translation matrix

	for (int i = 0; i < 16; i++) // copy input matrix
	{
		tempMatrix[i] = matrix[i];
	}

	translateMatrix[0] = 1; translateMatrix[4] = 0; translateMatrix[8] = 0; translateMatrix[12] = x; // set translation matrix
	translateMatrix[1] = 0; translateMatrix[5] = 1; translateMatrix[9] = 0; translateMatrix[13] = y;
	translateMatrix[2] = 0; translateMatrix[6] = 0; translateMatrix[10] = 1; translateMatrix[14] = z;
	translateMatrix[3] = 0;	translateMatrix[7] = 0;	translateMatrix[11] = 0; translateMatrix[15] = 1;
	// temp goes down each column, while translate goes across each row
	// column 0
	matrix[0] = tempMatrix[0] * translateMatrix[0] + tempMatrix[4] * translateMatrix[1] + tempMatrix[8] * translateMatrix[2] + tempMatrix[12] * translateMatrix[3];
	matrix[1] = tempMatrix[1] * translateMatrix[0] + tempMatrix[5] * translateMatrix[1] + tempMatrix[9] * translateMatrix[2] + tempMatrix[13] * translateMatrix[3];
	matrix[2] = tempMatrix[2] * translateMatrix[0] + tempMatrix[6] * translateMatrix[1] + tempMatrix[10] * translateMatrix[2] + tempMatrix[14] * translateMatrix[3];
	matrix[3] = tempMatrix[3] * translateMatrix[0] + tempMatrix[7] * translateMatrix[1] + tempMatrix[11] * translateMatrix[2] + tempMatrix[15] * translateMatrix[3];
	// column 1
	matrix[4] = tempMatrix[0] * translateMatrix[4] + tempMatrix[4] * translateMatrix[5] + tempMatrix[8] * translateMatrix[6] + tempMatrix[12] * translateMatrix[7];
	matrix[5] = tempMatrix[1] * translateMatrix[4] + tempMatrix[5] * translateMatrix[5] + tempMatrix[9] * translateMatrix[6] + tempMatrix[13] * translateMatrix[7];
	matrix[6] = tempMatrix[2] * translateMatrix[4] + tempMatrix[6] * translateMatrix[5] + tempMatrix[10] * translateMatrix[6] + tempMatrix[14] * translateMatrix[7];
	matrix[7] = tempMatrix[3] * translateMatrix[4] + tempMatrix[7] * translateMatrix[5] + tempMatrix[11] * translateMatrix[6] + tempMatrix[15] * translateMatrix[7];
	// column 2
	matrix[8] = tempMatrix[0] * translateMatrix[8] + tempMatrix[4] * translateMatrix[9] + tempMatrix[8] * translateMatrix[10] + tempMatrix[12] * translateMatrix[11];
	matrix[9] = tempMatrix[1] * translateMatrix[8] + tempMatrix[5] * translateMatrix[9] + tempMatrix[9] * translateMatrix[10] + tempMatrix[13] * translateMatrix[11];
	matrix[10] = tempMatrix[2] * translateMatrix[8] + tempMatrix[6] * translateMatrix[9] + tempMatrix[10] * translateMatrix[10] + tempMatrix[14] * translateMatrix[11];
	matrix[11] = tempMatrix[3] * translateMatrix[8] + tempMatrix[7] * translateMatrix[9] + tempMatrix[11] * translateMatrix[10] + tempMatrix[15] * translateMatrix[11];
	// column 3
	matrix[12] = tempMatrix[0] * translateMatrix[12] + tempMatrix[4] * translateMatrix[13] + tempMatrix[8] * translateMatrix[14] + tempMatrix[12] * translateMatrix[15];
	matrix[13] = tempMatrix[1] * translateMatrix[12] + tempMatrix[5] * translateMatrix[13] + tempMatrix[9] * translateMatrix[14] + tempMatrix[13] * translateMatrix[15];
	matrix[14] = tempMatrix[2] * translateMatrix[12] + tempMatrix[6] * translateMatrix[13] + tempMatrix[10] * translateMatrix[14] + tempMatrix[14] * translateMatrix[15];
	matrix[15] = tempMatrix[3] * translateMatrix[12] + tempMatrix[7] * translateMatrix[13] + tempMatrix[11] * translateMatrix[14] + tempMatrix[15] * translateMatrix[15];
}

void myScalef(GLfloat *matrix, GLfloat x, GLfloat y, GLfloat z){
	// Not needed in this exercise.

}

void myRotatef(GLfloat *matrix,
	GLfloat angle, GLfloat x, GLfloat y, GLfloat z){
	// Remember the Rodrigues' rotation formula?
	// R =  I + S * sin(theta) + Ssquared * (1-cos(theta))
	// matrix is 4x4
	GLfloat radAngle = float(DEG2RAD(angle)); // angle of rotation in radians
	GLfloat tempMatrix[16]; // will hold the input matrix
	GLfloat rotationMatrix[16]; // will hold the rotation matrix
	GLfloat skewMatrix[16]; // will hold the skew matrix
	GLfloat skewMatrixSqr[16]; // will hold the skew matrix squared
	GLfloat skewXdegrees[16]; // will hold the skew matrix times sin(theta)
	GLfloat skewsqrXdegrees[16]; // will hold the skew matrix squared times (1-cos(theta))
	GLfloat identityMatrix[16]; // will hold the identity matrix
	
	for (int i = 0; i < 16; i++) // copy the input matrix
	{
		tempMatrix[i] = matrix[i];
	}
	// set the identity matrix
	identityMatrix[0] = 1; identityMatrix[3] = 0; identityMatrix[6] = 0;
	identityMatrix[1] = 0; identityMatrix[4] = 1; identityMatrix[7] = 0;
	identityMatrix[2] = 0; identityMatrix[5] = 0; identityMatrix[8] = 1;
	// set the skew matrix
	skewMatrix[0] = 0; skewMatrix[3] = z; skewMatrix[6] = -y; 
	skewMatrix[1] = -z; skewMatrix[4] = 0; skewMatrix[7] = x; 
	skewMatrix[2] = y; skewMatrix[5] = -x; skewMatrix[8] = 0;
	// find the skew matrix squared
	// column 0
	skewMatrixSqr[0] = skewMatrix[0] * skewMatrix[0] + skewMatrix[3] * skewMatrix[1] + skewMatrix[6] * skewMatrix[2];
	skewMatrixSqr[1] = skewMatrix[1] * skewMatrix[0] + skewMatrix[4] * skewMatrix[1] + skewMatrix[7] * skewMatrix[2];
	skewMatrixSqr[2] = skewMatrix[2] * skewMatrix[0] + skewMatrix[5] * skewMatrix[1] + skewMatrix[8] * skewMatrix[2];
	// column 1
	skewMatrixSqr[3] = skewMatrix[0] * skewMatrix[3] + skewMatrix[3] * skewMatrix[4] + skewMatrix[6] * skewMatrix[5];
	skewMatrixSqr[4] = skewMatrix[1] * skewMatrix[3] + skewMatrix[4] * skewMatrix[4] + skewMatrix[7] * skewMatrix[5];
	skewMatrixSqr[5] = skewMatrix[2] * skewMatrix[3] + skewMatrix[5] * skewMatrix[4] + skewMatrix[8] * skewMatrix[5];
	// column 2
	skewMatrixSqr[6] = skewMatrix[0] * skewMatrix[6] + skewMatrix[3] * skewMatrix[7] + skewMatrix[6] * skewMatrix[8];
	skewMatrixSqr[7] = skewMatrix[1] * skewMatrix[6] + skewMatrix[4] * skewMatrix[7] + skewMatrix[7] * skewMatrix[8];
	skewMatrixSqr[8] = skewMatrix[2] * skewMatrix[6] + skewMatrix[5] * skewMatrix[7] + skewMatrix[8] * skewMatrix[8];

	for (int i = 0; i < 9; i++) //multiply the skew and the skew squared matrices by sin(theta) and (1-cos(theta)) respectively
	{
		skewXdegrees[i] = skewMatrix[i] * sin(radAngle);
		skewsqrXdegrees[i] = skewMatrixSqr[i] * (1 - cos(radAngle));
	}

	for (int i = 0; i < 4; i++)
	{
		if (i < 3)
		{
			rotationMatrix[i] = identityMatrix[i] + skewXdegrees[i] + skewsqrXdegrees[i];
			rotationMatrix[i + 4] = identityMatrix[i + 3] + skewXdegrees[i + 3] + skewsqrXdegrees[i + 3];
			rotationMatrix[i + 8] = identityMatrix[i + 6] + skewXdegrees[i + 6] + skewsqrXdegrees[i + 6];
			rotationMatrix[i + 12] = 0;
		}
		else
		{
			rotationMatrix[i] = 0;
			rotationMatrix[i + 4] = 0;
			rotationMatrix[i + 8] = 0;
			rotationMatrix[i + 12] = 1;
		}
	}
//	printf("rotation matrix:\n");
//	printMat(rotationMatrix);
	// temp goes down each column, while rotate goes across each row
	// column 0
	matrix[0] = tempMatrix[0] * rotationMatrix[0] + tempMatrix[4] * rotationMatrix[1] + tempMatrix[8] * rotationMatrix[2] + tempMatrix[12] * rotationMatrix[3];
	matrix[1] = tempMatrix[1] * rotationMatrix[0] + tempMatrix[5] * rotationMatrix[1] + tempMatrix[9] * rotationMatrix[2] + tempMatrix[13] * rotationMatrix[3];
	matrix[2] = tempMatrix[2] * rotationMatrix[0] + tempMatrix[6] * rotationMatrix[1] + tempMatrix[10] * rotationMatrix[2] + tempMatrix[14] * rotationMatrix[3];
	matrix[3] = tempMatrix[3] * rotationMatrix[0] + tempMatrix[7] * rotationMatrix[1] + tempMatrix[11] * rotationMatrix[2] + tempMatrix[15] * rotationMatrix[3];
	// column 1
	matrix[4] = tempMatrix[0] * rotationMatrix[4] + tempMatrix[4] * rotationMatrix[5] + tempMatrix[8] * rotationMatrix[6] + tempMatrix[12] * rotationMatrix[7];
	matrix[5] = tempMatrix[1] * rotationMatrix[4] + tempMatrix[5] * rotationMatrix[5] + tempMatrix[9] * rotationMatrix[6] + tempMatrix[13] * rotationMatrix[7];
	matrix[6] = tempMatrix[2] * rotationMatrix[4] + tempMatrix[6] * rotationMatrix[5] + tempMatrix[10] * rotationMatrix[6] + tempMatrix[14] * rotationMatrix[7];
	matrix[7] = tempMatrix[3] * rotationMatrix[4] + tempMatrix[7] * rotationMatrix[5] + tempMatrix[11] * rotationMatrix[6] + tempMatrix[15] * rotationMatrix[7];
	// column 2
	matrix[8] = tempMatrix[0] * rotationMatrix[8] + tempMatrix[4] * rotationMatrix[9] + tempMatrix[8] * rotationMatrix[10] + tempMatrix[12] * rotationMatrix[11];
	matrix[9] = tempMatrix[1] * rotationMatrix[8] + tempMatrix[5] * rotationMatrix[9] + tempMatrix[9] * rotationMatrix[10] + tempMatrix[13] * rotationMatrix[11];
	matrix[10] = tempMatrix[2] * rotationMatrix[8] + tempMatrix[6] * rotationMatrix[9] + tempMatrix[10] * rotationMatrix[10] + tempMatrix[14] * rotationMatrix[11];
	matrix[11] = tempMatrix[3] * rotationMatrix[8] + tempMatrix[7] * rotationMatrix[9] + tempMatrix[11] * rotationMatrix[10] + tempMatrix[15] * rotationMatrix[11];
	// column 3
	matrix[12] = tempMatrix[0] * rotationMatrix[12] + tempMatrix[4] * rotationMatrix[13] + tempMatrix[8] * rotationMatrix[14] + tempMatrix[12] * rotationMatrix[15];
	matrix[13] = tempMatrix[1] * rotationMatrix[12] + tempMatrix[5] * rotationMatrix[13] + tempMatrix[9] * rotationMatrix[14] + tempMatrix[13] * rotationMatrix[15];
	matrix[14] = tempMatrix[2] * rotationMatrix[12] + tempMatrix[6] * rotationMatrix[13] + tempMatrix[10] * rotationMatrix[14] + tempMatrix[14] * rotationMatrix[15];
	matrix[15] = tempMatrix[3] * rotationMatrix[12] + tempMatrix[7] * rotationMatrix[13] + tempMatrix[11] * rotationMatrix[14] + tempMatrix[15] * rotationMatrix[15];
}

void myLookAt(GLfloat *matrix,
	GLfloat eyeX, GLfloat eyeY, GLfloat eyeZ,
	GLfloat centerX, GLfloat centerY, GLfloat centerZ,
	GLfloat upX, GLfloat upY, GLfloat upZ){

	GLfloat normGaze[3]; // will hold the normalized gaze vector
	GLfloat normUp[3]; // will hold the normalized up vector
	GLfloat normRight[3]; // will hold the normalized right vector
	GLfloat right[3]; // will hold the right vector
	GLfloat modelView[16]; // will hold the pre-translated model view matrix
	GLfloat translate[16]; // will hold the translate matrix
	GLfloat gaze[] = { centerX - eyeX, centerY - eyeY, centerZ - eyeZ }; // holds the gaze vector
	GLfloat up[] = { upX, upY, upZ }; // holds the up vector
	float gazeLength;
	float upLength;
	float rightLength;

	gazeLength = sqrt(SQR(gaze[0]) + SQR(gaze[1]) + SQR(gaze[2])); // find the length of the gaze vector

	for (int i = 0; i < 3; i++) // normalize the gaze vector
	{
		normGaze[i] = gaze[i] / gazeLength;
	}

	upLength = sqrt(SQR(up[0]) + SQR(up[1]) + SQR(up[2])); // find the length of the up vector

	for (int i = 0; i < 3; i++) // normalize the up vector
	{
		normUp[i] = up[i] / upLength;
	}
	// cross the normalized gaze vector with the normalized up vector to get the right vector
	right[0] = normUp[2] * normGaze[1] - normUp[1] * normGaze[2];
	right[1] = normUp[0] * normGaze[2] - normUp[2] * normGaze[0];
	right[2] = normUp[1] * normGaze[0] - normUp[0] * normGaze[1];

	rightLength = sqrt(SQR(right[0]) + SQR(right[1]) + SQR(right[2])); // find the length of the right vector

	for (int i = 0; i < 3; i++) // normalize the right vector because I can
	{
		normRight[i] = right[i] / rightLength;
	}
	// create the model view matrix
	matrix[0] = normRight[0]; matrix[4] = normRight[1];   matrix[8] = normRight[2];  matrix[12] = 0;
	matrix[1] = normUp[0];	  matrix[5] = normUp[1];	  matrix[9] = normUp[2];	 matrix[13] = 0;
	matrix[2] = -normGaze[0]; matrix[6] = -normGaze[1];	  matrix[10] = -normGaze[2]; matrix[14] = 0;
	matrix[3] = 0;			  matrix[7] = 0;			  matrix[11] = 0;			 matrix[15] = 1;
	
	myTranslatef(matrix, -eyeX, -eyeY, -eyeZ); // translate the camera to be at (eyeX, eyeY, eyeZ)
}

void myFrustum(GLfloat *matrix,
	GLdouble left, GLdouble right, GLdouble bottom,
	GLdouble top, GLdouble zNear, GLdouble zFar){
	// Not needed in this exercise.
}

void myPerspective(GLfloat *matrix,
	GLdouble fovy, GLdouble aspect,
	GLdouble zNear, GLdouble zFar){
	// Not needed in this exercise.
}

void myOrtho(GLfloat *matrix,
	GLdouble left, GLdouble right, GLdouble bottom,
	GLdouble top, GLdouble zNear, GLdouble zFar){
	// Not needed in this exercise.
}

void printVec(GLfloat *a)
{
	printf("( %g, %g, %g )\n", a[0], a[1], a[2]);
}

void printMat(GLfloat *m)
{
	for (int i = 0; i < 3; i++)
	{
		printf("%.4f, %.4f, %.4f, %.4f\n", m[i], m[i + 4], m[i + 8], m[i + 12]);
	}
}
#else
#include "transformations_solution.cpp"
#endif

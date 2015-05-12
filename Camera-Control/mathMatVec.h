#ifndef MATHMATVEC_H
#define MATHMATVEC_H
#include <iostream>
#include <fstream>
#include <string>

using namespace std;

typedef struct Vector
{
	float comp[4];
}Vector;

typedef struct Matrix
{
	float data[16];
}Matrix;


//adds or subtracts 2 vectors into a third
void vectorAdd(Vector *v, Vector *a, Vector *b);
//cross product of two vectors into a third
void vectorCross(Vector *v, Vector *a, Vector *b);
//dot product of 2 vectors to a float variable
float vectorDot(Vector *a, Vector *b);
//find the length of a vector
float vectorLength(Vector *v);
//normalize vector to unit length into a second
void unitVector(Vector *v, Vector *a, float length);
//check if two vectors are the same
bool isVectorEqual(Vector *a, Vector *b);
// adds two matrices into a third
void matrixSum(Matrix *m, Matrix *a, Matrix *b);
// subtracts two matrices into a third
void matrixSub(Matrix *m, Matrix *a, Matrix *b);
// multiplies a matrix by a scalar into a second
void matrixMultScalar(Matrix *m, Matrix *a, float s);
// multiplies a matrix by a vector into a second
void matrixMultVector(Vector *v, Matrix *m, Vector *u);
// multiplies two matrices into a third
void matrixMult(Matrix *c, Matrix *a, Matrix *b);
// finds the determinate of a matrix
float matrixDeterminate(Matrix *m);
// finds the cofactor of a matrix
void matrixCofactor(Matrix *cofactor, Matrix *m);
// finds the transpose of a matrix
void matrixTranspose(Matrix *adj, Matrix *cofactor);
// finds the adjoint of a matrix
void matrixAdjoint(Matrix *adj, Matrix *m);
// finds the inverse of a matrix
void matrixInverse(Matrix *inv, Matrix *m);
// checks if two matrices are the same
bool isMatrixEqual(Matrix *a, Matrix *b);

#endif // MATHMATVEC_H

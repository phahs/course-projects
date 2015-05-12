#include "mathMatVec.h"

//adds or subtracts 2 vectors into a third
void vectorAdd(Vector *v, Vector *a, Vector *b)
{
	v->comp[0] = a->comp[0] + b->comp[0];
	v->comp[1] = a->comp[1] + b->comp[1];
	v->comp[2] = a->comp[2] + b->comp[2];
}
//cross product of two vectors into a third
void vectorCross(Vector *v, Vector *a, Vector *b)
{
	v->comp[0] = (a->comp[1] * b->comp[2]) - (b->comp[1] * a->comp[2]);
	v->comp[1] = -((a->comp[0] * b->comp[2]) - (b->comp[0] * a->comp[2]));
	v->comp[2] = (a->comp[0] * b->comp[1]) - (b->comp[0] * a->comp[1]);
}
//dot product of 2 vectors to a float variable
float vectorDot(Vector *a, Vector *b)
{
	return(a->comp[0] * b->comp[1] +
		a->comp[1] * b->comp[2] +
		a->comp[2] * b->comp[0]);
}
//find the length of a vector
float vectorLength(Vector *v)
{
	return(sqrt((v->comp[0] * v->comp[0]) + (v->comp[1] * v->comp[1]) + (v->comp[2] * v->comp[2])));
}
//normalize vector to unit length into a second
void unitVector(Vector *v, Vector *a, float length)
{
	v->comp[0] = a->comp[0] / length;
	v->comp[1] = a->comp[1] / length;
	v->comp[2] = a->comp[2] / length;
}
//check if two vectors are the same
bool isVectorEqual(Vector *a, Vector *b)
{
	if (a->comp[0] == b->comp[0] && a->comp[1] == b->comp[1] && a->comp[2] == b->comp[2])
		return(true);
	else
		return(false);
}
// adds two matrices into a third
void matrixSum(Matrix *m, Matrix *a, Matrix *b)
{
	for (int i = 0; i < 16; i++)
	{
		m->data[i] = a->data[i] + b->data[i];
	}
}

// subtracts two matrices into a third
void matrixSub(Matrix *m, Matrix *a, Matrix *b)
{
	for (int i = 0; i < 16; i++)
	{
		m->data[i] = a->data[i] - b->data[i];
	}
}
// multiplies a matrix by a scalar into a second
void matrixMultScalar(Matrix *m, Matrix *a, float s)
{
	for (int i = 0; i < 16; i++)
	{
		m->data[i] = a->data[i] * s;
	}
}
// multiplies a matrix by a vector into a second
void matrixMultVector(Vector *v, Matrix *m, Vector *u)
{
	for (int i = 0; i < 4; i++)
	{
		v->comp[i] = m->data[i] * u->comp[i] +
			m->data[i + 4] * u->comp[i] +
			m->data[i + 8] * u->comp[i] +
			m->data[i + 12] * u->comp[i];
	}
}
// multiplies two matrices into a third
void matrixMult(Matrix *c, Matrix *a, Matrix *b)
{
	float b00 = b->data[0]; float b01 = b->data[4]; float b02 = b->data[8]; float b03 = b->data[12];
	float b10 = b->data[1]; float b11 = b->data[5]; float b12 = b->data[9]; float b13 = b->data[13];
	float b20 = b->data[2]; float b21 = b->data[6]; float b22 = b->data[10]; float b23 = b->data[14];
	float b30 = b->data[3]; float b31 = b->data[7]; float b32 = b->data[11]; float b33 = b->data[15];


	for (int i = 0; i < 4; i++)
	{
		c->data[0 + i] = a->data[i] * b00 + a->data[i + 4] * b10 + a->data[i + 8] * b20 + a->data[i + 12] * b30;
		c->data[4 + i] = a->data[i] * b01 + a->data[i + 4] * b11 + a->data[i + 8] * b21 + a->data[i + 12] * b31;
		c->data[8 + i] = a->data[i] * b02 + a->data[i + 4] * b12 + a->data[i + 8] * b22 + a->data[i + 12] * b32;
		c->data[12 + i] = a->data[i] * b03 + a->data[i + 4] * b13 + a->data[i + 8] * b23 + a->data[i + 12] * b33;
	}
}
// finds the determinate of a matrix
float matrixDeterminate(Matrix *m)
{
	float d = m->data[0] * ((m->data[4] * m->data[8]) - (m->data[7] * m->data[5])) +
		m->data[3] * ((m->data[7] * m->data[2]) - (m->data[1] * m->data[8])) +
		m->data[6] * ((m->data[1] * m->data[5]) - (m->data[4] * m->data[2]));

	return d;
}
// finds the cofactor of a matrix
void matrixCofactor(Matrix *cofactor, Matrix *m)
{
	cofactor->data[0] = m->data[4] * m->data[8] - m->data[7] * m->data[5];
	cofactor->data[1] = m->data[3] * m->data[8] - m->data[6] * m->data[5];
	cofactor->data[2] = m->data[3] * m->data[7] - m->data[6] * m->data[4];
	cofactor->data[3] = m->data[1] * m->data[8] - m->data[7] * m->data[2];
	cofactor->data[4] = m->data[0] * m->data[8] - m->data[6] * m->data[2];
	cofactor->data[5] = m->data[0] * m->data[7] - m->data[6] * m->data[1];
	cofactor->data[6] = m->data[1] * m->data[5] - m->data[4] * m->data[2];
	cofactor->data[7] = m->data[0] * m->data[5] - m->data[3] * m->data[2];
	cofactor->data[8] = m->data[0] * m->data[4] - m->data[3] * m->data[1];
}
// finds the transpose of a matrix
void matrixTranspose(Matrix *transpose, Matrix *m)
{
	Vector row0, row1, row2;

	for (int i = 0; i < 3; i++)
	{
		row0.comp[i] = m->data[i];
		row1.comp[i] = m->data[i + 3];
		row2.comp[i] = m->data[i + 6];
	}

	transpose->data[0] = row0.comp[0];
	transpose->data[3] = row0.comp[1];
	transpose->data[6] = row0.comp[2];
	transpose->data[1] = row1.comp[0];
	transpose->data[4] = row1.comp[1];
	transpose->data[7] = row1.comp[2];
	transpose->data[2] = row2.comp[0];
	transpose->data[5] = row2.comp[1];
	transpose->data[8] = row2.comp[2];
}
// finds the adjoint of a matrix
void matrixAdjoint(Matrix *adj, Matrix *m)
{
	Matrix *cofactor = 0;

	matrixCofactor(cofactor, m);

	matrixTranspose(adj, cofactor);
}
// finds the inverse of a matrix
void matrixInverse(Matrix *inv, Matrix *m)
{
	Matrix *adjoint = 0;
	float det;
	det = matrixDeterminate(m);

	matrixAdjoint(adjoint, m);

	float detInv = 1.0 / det;

	matrixMultScalar(inv, adjoint, detInv);
}
// checks if two matrices are the same
bool isMatrixEqual(Matrix *a, Matrix *b)
{
	bool notEqual = false;
	for (int i = 0; i < 9; i++)
	{
		if (a->data[i] != b->data[i])
		{
			notEqual = true;
		}
	}

	return notEqual;
}
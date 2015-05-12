// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Holds my Model class and its sub-class PlyModel
//
#include "BoundingVolume.h"

#include <iostream>
#include <fstream>
#include <cassert>

#ifndef MODEL_H
#define MODEL_H

#ifndef SQR
#define SQR( x ) ((x) * (x))
#endif

int vecPrint3d(FILE* f, double &u, double &v, double &w){
	return fprintf(f, "(%.16f, %.16f, %.16f)\n", u, v, w);
}

void vecCopy3d(double &dest_x, double &dest_y, double &dest_z, double &src_x, double &src_y, double &src_z){
	dest_x = src_x;
	dest_y = src_y;
	dest_z = src_z;
}

double vecSquaredLength3d(double a, double b, double c){
	double accumulate = 0.0;
	accumulate += SQR(a);
	accumulate += SQR(b);
	accumulate += SQR(c);
	return(accumulate);
}

double vecLength3d(double &a, double &b, double &c){
	return(sqrt(vecSquaredLength3d(a, b, c)));
}

void vecDifference3d(double &ret_x, double &ret_y, double &ret_z,
	double head_x, double head_y, double head_z,
	double tail_x, double tail_y, double tail_z){
	ret_x = head_x - tail_x;
	ret_y = head_y - tail_y;
	ret_z = head_z - tail_z;
}


void vecCross3d(double &ret_x, double &ret_y, double ret_z,
	double &head_x, double &head_y, double head_z,
	double &tail_x, double &tail_y, double &tail_z){
	ret_x = head_y * tail_z - head_z * tail_y;
	ret_y = head_z * tail_x - head_x * tail_z;
	ret_z = head_x * tail_y - head_y * tail_x;
}

void vecNormalize3d(double &out_x, double &out_y, double &out_z,
	double &in_x, double &in_y, double &in_z){
	double length;
	length = vecLength3d(in_x, in_y, in_z);
	if (length <= 0.0){
		out_x = 0.0;
		out_y = 0.0;
		out_z = 0.0;
	}
	else if (length == 1.0){
		vecCopy3d(out_x, out_y, out_z, in_x, in_y, in_z);
	}
	else{
		length = 1.0 / length;
		out_x = in_x * length;
		out_y = in_y * length;
		out_z = in_z * length;
	}
}
// normal, vector a, vector b, vector c
void vecCalcNormal3d(double &normal_x, double &normal_y, double &normal_z,
	Vertex *a, Vertex *b, Vertex *c){
	double pa_x, pa_y, pa_z;
	double pb_x, pb_y, pb_z;

	vecDifference3d(pa_x, pa_y, pa_z, b->x, b->y, b->z, a->x, a->y, a->z);
	vecDifference3d(pb_x, pb_y, pb_z, c->x, c->y, c->z, a->x, a->y, a->z);

	vecCross3d(normal_x, normal_y, normal_z, pa_x, pa_y, pa_z, pb_x, pb_y, pb_z);
	vecNormalize3d(normal_x, normal_y, normal_z, normal_x, normal_y, normal_z);
}

double vecDistanceBetween3d(double &a_x, double &a_y, double &a_z,
	double &b_x, double &b_y, double &b_z){
	double c_x, c_y, c_z;
	vecDifference3d(c_x, c_y, c_z, a_x, a_y, a_z, b_x, b_y, b_z);
	return(vecLength3d(c_x, c_y, c_z));
}

double vecSquaredDistanceBetween3d(double &a_x, double &a_y, double &a_z,
	double &b_x, double &b_y, double &b_z){
	double c_x, c_y, c_z;
	vecDifference3d(c_x, c_y, c_z, a_x, a_y, a_z, b_x, b_y, b_z);
	return(vecSquaredLength3d(c_x, c_y, c_z));
}

void vecSum3d(double &c_x, double &c_y, double &c_z,
	double &a_x, double &a_y, double &a_z,
	double &b_x, double &b_y, double &b_z){

	c_x = a_x + b_x;
	c_y = a_y + b_y;
	c_z = a_z + b_z;
}


enum modelType { M_NULL, M_PLY };

class Model
{
private:
	char _type;
public:
	Model(char type)
	{
		_type = type;
	}
};

class PlyModel : public Model
{
public:
	// head of vertices linked list
	Vertex* verticesHead;
	// current place in vertices linked list
	Vertex* verticesCurrent;
	// end of vertices linked list
	Vertex* verticesEnd;
	// head of face linked list
	Face* faceHead;
	//current place in face linked list
	Face* faceCurrent;
	// end of face linked list
	Face* faceEnd;
	// vertex count
	int vc;
	// face count
	int fc;

	// bounding sphere
	SphereBV* bs;

	// transforms matrix
	double _transforms[16];

	PlyModel(int numVert, int numFace) : Model(M_PLY)
	{
		vc = numVert;
		fc = numFace;
		verticesHead = new Vertex;
		verticesCurrent = verticesHead;
		verticesEnd = verticesCurrent;
		for (int i = 0; i < vc; i++)
		{
			verticesCurrent->vNumber = i;
			verticesCurrent->next = new Vertex;
			verticesEnd = verticesCurrent->next;
			verticesCurrent = verticesEnd;
		}

		faceHead = new Face;
		faceCurrent = faceHead;
		faceEnd = faceCurrent;
		for (int i = 0; i < fc; i++)
		{
			faceCurrent->next = new Face;
			faceEnd = faceCurrent->next;
			faceCurrent = faceEnd;
		}

		bs = new SphereBV(0, 0, 0, 0);
	}

	~PlyModel()
	{
		verticesCurrent = verticesHead;
		faceCurrent = faceHead;

		while (verticesCurrent != verticesEnd)
		{
			verticesCurrent = verticesHead->next;
			delete verticesHead;
			verticesHead = verticesCurrent;
		}

		while (faceCurrent != faceEnd)
		{
			faceCurrent = faceHead->next;
			delete faceHead;
			faceHead = faceCurrent;
		}

		delete verticesCurrent;
		delete faceCurrent;
		delete bs;
	}
};

PlyModel* readPlyModel(const char* filename){
	char buffer[255], type[128], c;
	std::ifstream inputfile;
	unsigned int i;
	int k;
	unsigned int nv;
	unsigned int nf;
	PlyModel *fl;
	assert(filename);
	inputfile.open(filename, std::ios::in);
	if (inputfile.fail()){
		std::cerr << "File \"" << filename << "\" not found." << std::endl;
		exit(1);
	}
	// Parse the header
	inputfile.getline(buffer, sizeof(buffer), '\n');
	if (buffer != NULL){
		if (strcmp(buffer, "ply") != 0){
			std::cerr << "Error: Input file is not of .ply type." << std::endl;
			exit(1);
		}
	}
	else{
		std::cerr << "End of input?" << std::endl;
		exit(1);
	}
	inputfile.getline(buffer, sizeof(buffer), '\n');
	if (buffer != NULL){
		if (strncmp(buffer, "format ascii", 12) != 0){
			std::cerr << "Error: Input file is not in ASCII format." << std::endl;
			exit(1);
		}
	}
	else{
		std::cerr << "End of input?" << std::endl;
		exit(1);
	}
	inputfile.getline(buffer, sizeof(buffer), '\n');
	if (buffer != NULL){
		while (strncmp(buffer, "comment", 7) == 0){
			inputfile.getline(buffer, sizeof(buffer), '\n');
		}
	}
	else{
		std::cerr << "End of input?" << std::endl;
		exit(1);
	}

	if (strncmp(buffer, "element vertex", 14) == 0){
		sscanf(buffer, "element vertex %u\n", &nv);
	}
	else{
		std::cerr << "Error: number of vertices expected." << std::endl;
		exit(1);
	}

	i = 0;
	inputfile.getline(buffer, sizeof(buffer), '\n');
	while (strncmp(buffer, "property", 8) == 0) {
		if (i < 3) {
			sscanf(buffer, "property %s %c\n", type, &c);
			switch (i) {
			case 0:
				if (c != 'x') {
					std::cerr << "Error: first coordinate is not x." << std::endl;
					exit(1);
				}
				break;
			case 1:
				if (c != 'y') {
					std::cerr << "Error: first coordinate is not y." << std::endl;
					exit(1);
				}
				break;
			case 2:
				if (c != 'z') {
					std::cerr << "Error: first coordinate is not z." << std::endl;
					exit(1);
				}
				break;
			default:
				break;
			}
			i++;
		}
		inputfile.getline(buffer, sizeof(buffer), '\n');
	}

	if (strncmp(buffer, "element face", 12) == 0)
		sscanf(buffer, "element face %u\n", &nf);
	else {
		std::cerr << "Error: number of faces expected." << std::endl;
		exit(1);
	}

	inputfile.getline(buffer, sizeof(buffer), '\n');
	if (strncmp(buffer, "property list", 13) != 0) {
		std::cerr << "Error: property list expected." << std::endl;
		exit(1);
	}

	inputfile.getline(buffer, sizeof(buffer), '\n');
	while (strncmp(buffer, "end_header", 10) != 0){
		inputfile.getline(buffer, sizeof(buffer), '\n');
	}

	// Allocate FaceList object
	if (!(fl = new PlyModel(nv, nf))){
		std::cerr << "Could not allocate a new face list for the model." << std::endl;
		exit(1);
	}

	/* Process the body of the input file*/
	fl->verticesCurrent = fl->verticesHead;
	// read vertex data from PLY file
	for (i = 0; i < nv; i++) {
		inputfile.getline(buffer, sizeof(buffer), '\n');
		sscanf(buffer, "%lf %lf %lf", &(fl->verticesCurrent->x), &(fl->verticesCurrent->y), &(fl->verticesCurrent->z));
		fl->verticesCurrent = fl->verticesCurrent->next;
	}
	fl->faceCurrent = fl->faceHead;
	// read face data from PLY file
	for (i = 0; i < nf; i++) {
		int a, b, c;
		inputfile.getline(buffer, sizeof(buffer), '\n');
		sscanf(buffer, "%d %d %d %d", &k, &(a), &(b), &(c));
		if (k != 3) {
			fprintf(stderr, "Error: not a triangular face.\n");
			exit(1);
		}

		fl->verticesCurrent = fl->verticesHead;
		for (int j = 0; j < nv; j++)
		{
			if (a == fl->verticesCurrent->vNumber)
			{
				fl->faceCurrent->a = fl->verticesCurrent;
			}
			if (b == fl->verticesCurrent->vNumber)
			{
				fl->faceCurrent->b = fl->verticesCurrent;
			}
			if (c == fl->verticesCurrent->vNumber)
			{
				fl->faceCurrent->c = fl->verticesCurrent;
			}
			fl->verticesCurrent = fl->verticesCurrent->next;
		}
		fl->faceCurrent = fl->faceCurrent->next;
	}

	inputfile.close();

	fl->bs->calcBoundingSphere(fl->verticesHead, fl->vc);

	// compute face normals
	fl->faceCurrent = fl->faceHead;
	for (i = 0; i < nf; i++){

		vecCalcNormal3d(fl->faceCurrent->fNormal_x, fl->faceCurrent->fNormal_y, fl->faceCurrent->fNormal_z,
			fl->faceCurrent->a,
			fl->faceCurrent->b,
			fl->faceCurrent->c);

		// vertex a
		fl->faceCurrent->a->vNormal_x += fl->faceCurrent->fNormal_x;
		fl->faceCurrent->a->vNormal_y += fl->faceCurrent->fNormal_y;
		fl->faceCurrent->a->vNormal_z += fl->faceCurrent->fNormal_z;
		// vertex b
		fl->faceCurrent->b->vNormal_x += fl->faceCurrent->fNormal_x;
		fl->faceCurrent->b->vNormal_y += fl->faceCurrent->fNormal_y;
		fl->faceCurrent->b->vNormal_z += fl->faceCurrent->fNormal_z;
		// vertex c
		fl->faceCurrent->c->vNormal_x += fl->faceCurrent->fNormal_x;
		fl->faceCurrent->c->vNormal_y += fl->faceCurrent->fNormal_y;
		fl->faceCurrent->c->vNormal_z += fl->faceCurrent->fNormal_z;

		fl->faceCurrent = fl->faceCurrent->next;
	}

	fl->verticesCurrent = fl->verticesHead;
	// normalize the v_normals
	for (i = 0; i < nv; i++){

		vecNormalize3d(fl->verticesCurrent->vNormal_x, fl->verticesCurrent->vNormal_y, fl->verticesCurrent->vNormal_z,
			fl->verticesCurrent->vNormal_x, fl->verticesCurrent->vNormal_y, fl->verticesCurrent->vNormal_z);

		fl->verticesCurrent = fl->verticesCurrent->next;
	}

	puts("Done");
	return(fl);
}

#endif
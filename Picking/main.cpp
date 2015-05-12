// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 486
// 
// Where the stuff happens. Draws 3 objects,
// two boxes surrounding a bunny. If you click on
// one of the objects, it is surrounded in a 
// bubble of unknown power and affect.
//
#include <cstdlib>
#include <cstdio>

#include "GLSLShader.h"
#include "Model.h"
#include "intersection.h"
#include "transformations.h"

/***
* Global variables
*/

PlyModel *boxModel;
PlyModel *bunnyModel;

GLSLProgram *boxShader;
GLSLProgram *bunnyShader;
GLSLProgram *skyShader;
GLSLProgram *groundShader;
GLSLProgram *boundedShader;

ray3d *ray = new ray3d;
hitd *hit = new hitd;

double* xPos = new double;
double* yPos = new double;

double intersect1;
double intersect2;

float boxTranslation_1[3];
float boxTranslation_2[3];
float bunnyTranslation[3];

float eyePosition[3];
float upVector[3];

float projectionTransform[16];
float viewingTransform[16];
float modelingTransform[16];

// Boolean values that determine important things.
bool useGLPipeline;
bool renderBV;
bool bvHit;
bool onClick;

// light position for entire scene
float light0_position[] = { 7.0, -5.0, 0.0, 1.0 };
float light1_position[] = { -7.0, -5.0, 0.0, 1.0 };

float light0[4];
float light1[4];

// Material Properties
// Box Properties
const float boxAmbient[] = { 1.0, 0.843, 0.0, 1 };
const float boxDiffuse[] = { 0.0, 0.0, 0.502, 1 };
const float boxSpecular[] = { 1.0, 0.7, 0.0, 1 };
// Bunny Properties
const float bunnyAmbient[] = { 0.543, 0.269, 0.074, 1 };
const float bunnyDiffuse[] = { 0.82, 0.41, 0.117, 1 };
const float bunnySpecular[] = { 0.0, 0.0, 0.0, 1 };
// World Properties
// Skybox Properties
const float skyAmbient[] = { 0.529, 0.808, 0.98, 1 };
const float skyDiffuse[] = { 0.2, 0.2, 0.5, 1 };
const float skySpecular[] = { 0.0, 0.0, 0.0, 1 };
// Ground Properties
const float groundAmbient[] = { 0.133, 0.545, 0.133, 1 };
const float groundDiffuse[] = { .03, .2, .03, 1 };
const float groundSpecular[] = { 0.0, 0.0, 0.0, 1 };
// Bounding Volume Properties
const float bsAmbient[] = { 1.0, 0.0, 0.0, 0.4 };
const float bsDiffuse[] = { 1.0, 0.0, 0.0, 0.4 };
const float bsSpecular[] = { 1.0, 0.0, 0.0, 0.4 };

// Uniform Variables for GLSL Shader Program
unsigned int uLight0_position;
unsigned int uLight1_position;
unsigned int uBoxAmbient;
unsigned int uBoxDiffuse;
unsigned int uBoxSpecular;
unsigned int uBunAmbient;
unsigned int uBunDiffuse;
unsigned int uBunSpecular;
unsigned int uSkyAmbient;
unsigned int uSkyDiffuse;
unsigned int uSkySpecular;
unsigned int uGroundAmbient;
unsigned int uGroundDiffuse;
unsigned int uGroundSpecular;
unsigned int uBSAmbient;
unsigned int uBSDiffuse;
unsigned int uBSSpecular;

bool fileExists(const char *f){
	FILE *fh;
	bool rv = true;
	if ((fh = fopen(f, "r")) == NULL){
		fprintf(stderr, "Opening file %s encountered an error.\n", f);
		rv = false;
	}
	else{
		fclose(fh);
	}
	return(rv);
}

void printHelpMessage() {
	puts("Press 'h' to print this message again.");
	puts("Press 'g' to toggle between using the gl Pipeline or the GLSLProgram.");
	puts("Press 'b' to toggle the rendering of the bounding volume.");
	puts("Press ESC or 'q' to quit.");
}

void initUniforms_Box()
{
	uLight0_position = glGetUniformLocation(boxShader->id(),
		"light0_position");
	uLight1_position = glGetUniformLocation(boxShader->id(),
		"light1_position");
	uBoxAmbient = glGetUniformLocation(boxShader->id(), "ambient");
	uBoxDiffuse = glGetUniformLocation(boxShader->id(), "diffuse");
	uBoxSpecular = glGetUniformLocation(boxShader->id(), "specular");
}

void activateUniforms_Box()
{
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uBoxAmbient, 1, boxAmbient);
	glUniform4fv(uBoxDiffuse, 1, boxDiffuse);
	glUniform4fv(uBoxSpecular, 1, boxSpecular);
}

void initUniforms_Bunny()
{
	uLight0_position = glGetUniformLocation(bunnyShader->id(),
		"light0_position");
	uLight1_position = glGetUniformLocation(bunnyShader->id(),
		"light1_position");
	uBunAmbient = glGetUniformLocation(bunnyShader->id(), "ambient");
	uBunDiffuse = glGetUniformLocation(bunnyShader->id(), "diffuse");
	uBunSpecular = glGetUniformLocation(bunnyShader->id(), "specular");
}

void activateUniforms_Bunny()
{
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uBunAmbient, 1, bunnyAmbient);
	glUniform4fv(uBunDiffuse, 1, bunnyDiffuse);
	glUniform4fv(uBunSpecular, 1, bunnySpecular);
}

void initUniforms_Sky()
{
	uLight0_position = glGetUniformLocation(skyShader->id(),
		"light0_position");
	uLight1_position = glGetUniformLocation(skyShader->id(),
		"light1_position");
	uSkyAmbient = glGetUniformLocation(skyShader->id(), "ambient");
	uSkyDiffuse = glGetUniformLocation(skyShader->id(), "diffuse");
	uSkySpecular = glGetUniformLocation(skyShader->id(), "specular");
}

void activateUniforms_Sky()
{
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uSkyAmbient, 1, skyAmbient);
	glUniform4fv(uSkyDiffuse, 1, skyDiffuse);
	glUniform4fv(uSkySpecular, 1, skySpecular);
}

void initUniforms_Ground()
{
	uLight0_position = glGetUniformLocation(groundShader->id(),
		"light0_position");
	uLight1_position = glGetUniformLocation(groundShader->id(),
		"light1_position");
	uGroundAmbient = glGetUniformLocation(groundShader->id(), "ambient");
	uGroundDiffuse = glGetUniformLocation(groundShader->id(), "diffuse");
	uGroundSpecular = glGetUniformLocation(groundShader->id(), "specular");
}

void activateUniforms_Ground()
{
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uGroundAmbient, 1, groundAmbient);
	glUniform4fv(uGroundDiffuse, 1, groundDiffuse);
	glUniform4fv(uGroundSpecular, 1, groundSpecular);
}

void initUniforms_Bound()
{
	uLight0_position = glGetUniformLocation(boundedShader->id(),
		"light0_position");
	uLight1_position = glGetUniformLocation(boundedShader->id(),
		"light1_position");
	uBSAmbient = glGetUniformLocation(boundedShader->id(), "ambient");
	uBSDiffuse = glGetUniformLocation(boundedShader->id(), "diffuse");
	uBSSpecular = glGetUniformLocation(boundedShader->id(), "specular");
}

void activateUniforms_Bound()
{
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uBSAmbient, 1, bsAmbient);
	glUniform4fv(uBSDiffuse, 1, bsDiffuse);
	glUniform4fv(uBSSpecular, 1, bsSpecular);
}

void initEyePosition(){
	eyePosition[0] = 0.0;
	eyePosition[1] = 0.0;
	eyePosition[2] = 5.0;
}

void initUpVector(){
	upVector[0] = 0.0;
	upVector[1] = 1.0;
	upVector[2] = 0.0;
}

void initTransforms(){
	boxTranslation_1[0] = 7.0;
	boxTranslation_1[1] = 0.0;
	boxTranslation_1[2] = 0.0;
	boxTranslation_2[0] = -7.0;
	boxTranslation_2[1] = 0.0;
	boxTranslation_2[2] = 0.0;
	bunnyTranslation[0] = 0.0;
	bunnyTranslation[1] = 0.0;
	bunnyTranslation[2] = 0.0;
}

void initToggles()
{
	useGLPipeline = false;
	renderBV = true;
	bvHit = false;
	onClick = false;
}

void init()
{
	initEyePosition();
	initUpVector();
	initTransforms();
	initToggles();
	// Box Shader
	const char* vertexShaderSource = "VertShader.vert";
	const char* fragmentShaderSource = "FragShader.frag";
	FragmentShader fragmentShader(fragmentShaderSource);
	VertexShader vertexShader(vertexShaderSource);
	boxShader = new GLSLProgram();
	boxShader->attach(vertexShader);
	boxShader->attach(fragmentShader);
	boxShader->link();
	boxShader->activate();
	printf("Box Shader Program built from %s and %s.\n",
		vertexShaderSource, fragmentShaderSource);
	boxShader->deactivate();
	// Bunny Shader
	bunnyShader = new GLSLProgram();
	bunnyShader->attach(vertexShader);
	bunnyShader->attach(fragmentShader);
	bunnyShader->link();
	bunnyShader->activate();
	printf("Bunny Shader Program built from %s and %s.\n",
		vertexShaderSource, fragmentShaderSource);
	bunnyShader->deactivate();
	// Sky Shader
	skyShader = new GLSLProgram();
	skyShader->attach(vertexShader);
	skyShader->attach(fragmentShader);
	skyShader->link();
	skyShader->activate();
	printf("Box Shader Program built from %s and %s.\n",
		vertexShaderSource, fragmentShaderSource);
	skyShader->deactivate();
	// Ground Shader
	groundShader = new GLSLProgram();
	groundShader->attach(vertexShader);
	groundShader->attach(fragmentShader);
	groundShader->link();
	groundShader->activate();
	printf("Box Shader Program built from %s and %s.\n",
		vertexShaderSource, fragmentShaderSource);
	groundShader->deactivate();
	//Bounding Volume Shader
	boundedShader = new GLSLProgram();
	boundedShader->attach(vertexShader);
	boundedShader->attach(fragmentShader);
	boundedShader->link();
	boundedShader->activate();
	printf("Box Shader Program built from %s and %s.\n",
		vertexShaderSource, fragmentShaderSource);
	boundedShader->deactivate();

	// Set up uniform variables
	// Box Shader
	initUniforms_Box();
	// Bunny Shader
	initUniforms_Bunny();
	// Sky Shader
	initUniforms_Sky();
	// Ground Shader
	initUniforms_Ground();
	// Bounding Volume Shader
	initUniforms_Bound();
}

void drawModel(PlyModel* model)
{
	glBegin(GL_TRIANGLES);
	model->faceCurrent = model->faceHead;
	for (int i = 0; i < model->fc; i++)
	{
		// first of the three vertices
		glNormal3d(model->faceCurrent->a->vNormal_x, model->faceCurrent->a->vNormal_y,
			model->faceCurrent->a->vNormal_z);
		glVertex3d(model->faceCurrent->a->x, model->faceCurrent->a->y, model->faceCurrent->a->z);
		// second of the three vertices
		glNormal3d(model->faceCurrent->b->vNormal_x, model->faceCurrent->b->vNormal_y,
			model->faceCurrent->b->vNormal_z);
		glVertex3d(model->faceCurrent->b->x, model->faceCurrent->b->y, model->faceCurrent->b->z);
		// third of the three vertices
		glNormal3d(model->faceCurrent->c->vNormal_x, model->faceCurrent->c->vNormal_y,
			model->faceCurrent->c->vNormal_z);
		glVertex3d(model->faceCurrent->c->x, model->faceCurrent->c->y, model->faceCurrent->c->z);

		model->faceCurrent = model->faceCurrent->next;
	}
	glEnd();
}

void drawGround()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(-10, 0, 10); // bottom left corner, near the camera
	glVertex3d(10, 0, 10); // bottom right corner, near the camera
	glVertex3d(10, 0, -10); // bottom right corner, away from the camera
	glVertex3d(-10, 0, -10); // bottom left corner away from the camera
	glEnd();
}

void drawBackWall()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(-10, 0, -10); // bottom left corner, away from the camera
	glVertex3d(10, 0, -10); // bottom right corner, away from the camera
	glVertex3d(10, 10, -10); // upper right corner, away from the camera
	glVertex3d(-10, 10, -10); // upper left corner, away from the camera
	glEnd();
}

void drawLeftWall()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(-10, 0, -10); // bottom left corner, away from the camera
	glVertex3d(-10, 10, -10); // upper left corner, away from the camera
	glVertex3d(-10, 10, 10); // upper left corner, near the camera
	glVertex3d(-10, 0, 10); // bottom left corner, near the camera
	glEnd();
}

void drawRightWall()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(10, 0, -10); // bottom right corner, away from the camera
	glVertex3d(10, 0, 10); // bottom right corner, near the camera
	glVertex3d(10, 10, 10); // upper right corner, near the camera
	glVertex3d(10, 10, -10); // upper right corner, away from the camera
	glEnd();
}

void drawCeiling()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(-10, 10, 10); // upper left corner, near the camera
	glVertex3d(10, 10, 10); // upper right corner, near the camera
	glVertex3d(10, 10, -10); // upper right corner, away from the camera
	glVertex3d(-10, 10, -10); // upper left corner, away from the camera
	glEnd();
}

void drawFrontWall()
{
	glBegin(GL_QUADS);
	glNormal3d(0.0, 1.0, 0.0);
	glVertex3d(-10, 0, 10); // bottom left corner, near the camera
	glVertex3d(10, 0, 10); // bottom right corner, near the camera
	glVertex3d(10, 10, 10); // upper right corner, near the camera
	glVertex3d(-10, 10, 10); // upper left corner, near the camera
	glEnd();
}

void drawBoundingVolume(PlyModel* model)
{
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);

	glCallList(model->bs->startList);
	
	glDisable(GL_BLEND);
}

void vecCopy4f(float *dest, float *src){
	int i;
	for (i = 0; i < 4; i++){
		dest[i] = src[i];
	}
}

void matMultVec4f(float* vout, float* v, float* m){
	float c[4];
	vecCopy4f(c, v);
	vout[0] = m[0] * c[0] + m[4] * c[1] + m[8] * c[2] + m[12] * c[3];
	vout[1] = m[1] * c[0] + m[5] * c[1] + m[9] * c[2] + m[13] * c[3];
	vout[2] = m[2] * c[0] + m[6] * c[1] + m[10] * c[2] + m[14] * c[3];
	vout[3] = m[3] * c[0] + m[7] * c[1] + m[11] * c[2] + m[15] * c[3];
}

void updateProjection(int width, int height){
	glViewport(0, 0, (GLsizei)width, (GLsizei)height);
	double ratio = double(width) / double(height);
	glMatrixMode(GL_PROJECTION);
	
	if (useGLPipeline)
	{
		// use OpenGL perspective
		glPushMatrix();
		glLoadIdentity();
		gluPerspective(90.0, ratio, 1.0, 25.0);
		glGetFloatv(GL_PROJECTION_MATRIX, projectionTransform);
		glPopMatrix();
	}
	else
	{
		// use my perspective
		myPerspective(projectionTransform, 90.0, ratio, 1.0, 25.0);
	}
	glLoadMatrixf(projectionTransform);
}

void displayCallback(){
	const float centerPosition[] = { 0.0, 0.0, 0.0 };
	bool rvm1, rvm2, rvm3;

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glMatrixMode(GL_MODELVIEW);
	
	float boxScaleFactor = 1.0 / boxModel->bs->getRadius();
	float bunScaleFactor = 1.0 / bunnyModel->bs->getRadius();

	if (useGLPipeline)
	{
		glEnable(GL_LIGHTING);
		glFrontFace(GL_CCW);
		glEnable(GL_NORMALIZE);

		glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
		glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
		glEnable(GL_LIGHT0);
		glEnable(GL_LIGHT1);

		// building the scene
		glLoadIdentity();
		gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
		glMaterialfv(GL_FRONT, GL_AMBIENT, groundAmbient);
		glMaterialfv(GL_FRONT, GL_DIFFUSE, groundDiffuse);
		glMaterialfv(GL_FRONT, GL_SPECULAR, groundSpecular);
		glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
		glTranslated(0.0, -1.5, 0.0);
		drawGround();
		glMaterialfv(GL_FRONT, GL_AMBIENT, skyAmbient);
		glMaterialfv(GL_FRONT, GL_DIFFUSE, skyDiffuse);
		glMaterialfv(GL_FRONT, GL_SPECULAR, skySpecular);
		glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
		drawBackWall();
		drawLeftWall();
		drawRightWall();
		drawCeiling();
		drawFrontWall();
		glTranslated(0.0, 1.5, 0.0);

		// box 1
		glLoadIdentity();
		gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
		glScalef(boxScaleFactor, boxScaleFactor, boxScaleFactor);
		glTranslated(boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2] + 1);
		glMaterialfv(GL_FRONT, GL_AMBIENT, boxAmbient);
		glMaterialfv(GL_FRONT, GL_DIFFUSE, boxDiffuse);
		glMaterialfv(GL_FRONT, GL_SPECULAR, boxSpecular);
		glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
		drawModel(boxModel);

		if (onClick)
		{
			glLoadIdentity();
			gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
			glScalef(boxScaleFactor, boxScaleFactor, boxScaleFactor);
			glTranslated(boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2]);
			glMaterialfv(GL_FRONT, GL_AMBIENT, bsAmbient);
			glMaterialfv(GL_FRONT, GL_DIFFUSE, bsDiffuse);
			glMaterialfv(GL_FRONT, GL_SPECULAR, bsSpecular);
			glMaterialf(GL_FRONT, GL_SHININESS, 2.5);

			rvm1 = raySphereIntersection(boxModel->bs, ray, hit, boxTranslation_1, boxScaleFactor, intersect1, intersect2);
			if (rvm1 && renderBV)
			{
				drawModel(boxModel);
				bvHit = true;
			}
		}
		
		// box 2
		glLoadIdentity();
		gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
		glScalef(boxScaleFactor, boxScaleFactor, boxScaleFactor);
		glTranslated(boxTranslation_2[0], boxTranslation_2[1], boxTranslation_2[2] + 1);
		glMaterialfv(GL_FRONT, GL_AMBIENT, boxAmbient);
		glMaterialfv(GL_FRONT, GL_DIFFUSE, boxDiffuse);
		glMaterialfv(GL_FRONT, GL_SPECULAR, boxSpecular);
		glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
		drawModel(boxModel);
		
		if (onClick)
		{
			rvm2 = raySphereIntersection(boxModel->bs, ray, hit, boxTranslation_1, boxScaleFactor, intersect1, intersect2);
			if (rvm2 && renderBV)
			{
				glLoadIdentity();
				gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
				glScalef(boxScaleFactor, boxScaleFactor, boxScaleFactor);
				glTranslated(boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2]);
				glMaterialfv(GL_FRONT, GL_AMBIENT, bsAmbient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, bsDiffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, bsSpecular);
				glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
				drawModel(boxModel);
				bvHit = true;
			}
		}

		// bunny
		glLoadIdentity();
		gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
		glScalef(bunScaleFactor, bunScaleFactor, bunScaleFactor);
		glTranslated(boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2]);
		glMaterialfv(GL_FRONT, GL_AMBIENT, bunnyAmbient);
		glMaterialfv(GL_FRONT, GL_DIFFUSE, bunnyDiffuse);
		glMaterialfv(GL_FRONT, GL_SPECULAR, bunnySpecular);
		glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
		drawModel(bunnyModel);
		if (onClick)
		{
			rvm3 = raySphereIntersection(boxModel->bs, ray, hit, boxTranslation_1, boxScaleFactor, intersect1, intersect2);
			if (rvm3 && renderBV)
			{
				glLoadIdentity();
				gluLookAt(eyePosition[0], eyePosition[1], eyePosition[2], 0.0, 0.0, 0.0, upVector[0], upVector[1], upVector[2]);
				glScalef(bunScaleFactor, bunScaleFactor, bunScaleFactor);
				glTranslated(bunnyTranslation[0], bunnyTranslation[1], bunnyTranslation[2]);
				glMaterialfv(GL_FRONT, GL_AMBIENT, bsAmbient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, bsDiffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, bsSpecular);
				glMaterialf(GL_FRONT, GL_SHININESS, 2.5);
				drawModel(bunnyModel);
				bvHit = true;
			}
		}

		if (!bvHit)
		{
			onClick = false;
		}
		else
		{
			bvHit = false;
		}
	}
	else
	{
		myLookAt(viewingTransform,
			eyePosition[0], eyePosition[1], eyePosition[2],
			centerPosition[0], centerPosition[1], centerPosition[2],
			upVector[0], upVector[1], upVector[2]);
		// light info
		matMultVec4f(light0, light0_position, viewingTransform);
		matMultVec4f(light1, light1_position, viewingTransform);

		// building the scene
		groundShader->activate();
		activateUniforms_Ground();
		glLoadIdentity();
		glMultMatrixf(viewingTransform);
		myTranslatef(modelingTransform, 0.0, -1.5, 0.0);
		glMultMatrixf(modelingTransform);
		drawGround();

		skyShader->activate();
		activateUniforms_Sky();
		glLoadIdentity();
		glMultMatrixf(viewingTransform);
		myTranslatef(modelingTransform, 0.0, -1.5, 0.0);
		glMultMatrixf(modelingTransform);
		drawBackWall();
		drawLeftWall();
		drawRightWall();
		drawCeiling();
		drawFrontWall();

		// box 1
		boxShader->activate();
		activateUniforms_Box();
		glLoadIdentity();
		glMultMatrixf(viewingTransform);
		myScalef(modelingTransform, boxScaleFactor, boxScaleFactor, boxScaleFactor);
		glMultMatrixf(modelingTransform);
		myTranslatef(modelingTransform, boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2] + 1);
		glMultMatrixf(modelingTransform);
		drawModel(boxModel);

		if (onClick)
		{
			rvm1 = raySphereIntersection(boxModel->bs, ray, hit, boxTranslation_1, boxScaleFactor, intersect1, intersect2);
			if (rvm1 && renderBV)
			{
				boundedShader->activate();
				activateUniforms_Bound();
				glLoadIdentity();
				glMultMatrixf(viewingTransform);
				myScalef(modelingTransform, boxScaleFactor, boxScaleFactor, boxScaleFactor);
				glMultMatrixf(modelingTransform);
				myTranslatef(modelingTransform, boxTranslation_1[0], boxTranslation_1[1], boxTranslation_1[2]);
				glMultMatrixf(modelingTransform);
				drawBoundingVolume(boxModel);
				bvHit = true;
			}
		}

		// box 2
		boxShader->activate();
		activateUniforms_Box();
		glLoadIdentity();
		glMultMatrixf(viewingTransform);
		myScalef(modelingTransform, boxScaleFactor, boxScaleFactor, boxScaleFactor);
		glMultMatrixf(modelingTransform);
		myTranslatef(modelingTransform, boxTranslation_2[0], boxTranslation_2[1], boxTranslation_2[2] + 1);
		glMultMatrixf(modelingTransform);
		drawModel(boxModel);

		if (onClick)
		{
			rvm2 = raySphereIntersection(boxModel->bs, ray, hit, boxTranslation_2, boxScaleFactor, intersect1, intersect2);
			if (rvm2 && renderBV)
			{
				boundedShader->activate();
				activateUniforms_Bound();
				glLoadIdentity();
				glMultMatrixf(viewingTransform);
				myScalef(modelingTransform, boxScaleFactor, boxScaleFactor, boxScaleFactor);
				glMultMatrixf(modelingTransform);
				myTranslatef(modelingTransform, boxTranslation_2[0], boxTranslation_2[1], boxTranslation_2[2]);
				glMultMatrixf(modelingTransform);
				drawBoundingVolume(boxModel);
				bvHit = true;
			}
		}

		// bunny
		bunnyShader->activate();
		activateUniforms_Bunny();
		glLoadIdentity();
		glMultMatrixf(viewingTransform);
		myScalef(modelingTransform, bunScaleFactor, bunScaleFactor, bunScaleFactor);
		glMultMatrixf(modelingTransform);
		myTranslatef(modelingTransform, bunnyTranslation[0], bunnyTranslation[1], bunnyTranslation[2]);
		glMultMatrixf(modelingTransform);
		drawModel(bunnyModel);

		if (onClick)
		{
			rvm3 = raySphereIntersection(boxModel->bs, ray, hit, bunnyTranslation, bunScaleFactor, intersect1, intersect2);
			if (rvm3 && renderBV)
			{
				boundedShader->activate();
				activateUniforms_Bound();
				glLoadIdentity();
				glMultMatrixf(viewingTransform);
				myScalef(modelingTransform, bunScaleFactor, bunScaleFactor, bunScaleFactor);
				glMultMatrixf(modelingTransform);
				myTranslatef(modelingTransform, bunnyTranslation[0], bunnyTranslation[1], bunnyTranslation[2]);
				glMultMatrixf(modelingTransform);
				drawBoundingVolume(bunnyModel);
				bvHit = true;
			}
		}

		if (!bvHit)
		{
			onClick = false;
		}
		else
		{
			bvHit = false;
		}
		boxShader->deactivate();
		bunnyShader->deactivate();
		boundedShader->deactivate();
	}
}

static void error_callback(int error, const char* description){
	fputs(description, stderr);
}

static void key_callback(GLFWwindow* window, int key,
	int scancode, int action, int mods){
	float centerPosition[] = { 0.0, 0.0, 0.0 };
	
	if ((key == GLFW_KEY_ESCAPE && action == GLFW_PRESS) || (key == GLFW_KEY_Q && action == GLFW_PRESS)){
		glfwSetWindowShouldClose(window, GL_TRUE);
	}

	if ((key == GLFW_KEY_B && action == GLFW_PRESS)){
		renderBV = !renderBV;
		printf("Rendering the bounding volume? %s\n", (renderBV ? "Yes" : "No"));
	}

	if ((key == GLFW_KEY_G && action == GLFW_PRESS)){
		useGLPipeline = !useGLPipeline;
		printf("Use OpenGL's graphics pipeline? %s\n", (useGLPipeline ? "Yes" : "No"));
	}

	if ((key == GLFW_KEY_H && action == GLFW_PRESS)){
		printHelpMessage();
	}
	/*
	if ((key == GLFW_KEY_RIGHT)){
		rotateCameraLeft(-5.0, eyePosition,
			centerPosition, upVector);
	}
	if (key == GLFW_KEY_LEFT){
		rotateCameraLeft(5.0, eyePosition,
			centerPosition, upVector);
	}
	if (key == GLFW_KEY_UP){
		rotateCameraUp(5.0, eyePosition,
			centerPosition, upVector);
	}
	if (key == GLFW_KEY_DOWN){
		rotateCameraUp(-5.0, eyePosition,
			centerPosition, upVector);
	}
	*/
}

static void mouse_callback(GLFWwindow* window, int button, int action, int mods){
	
	// if we press the left mouse button
	if (button == GLFW_MOUSE_BUTTON_1 && action == GLFW_PRESS)
	{
		double mv[16];
		double proj[16];
		glfwGetCursorPos(window, xPos, yPos);

		for (int i = 0; i < 16; i++)
		{
			mv[i] = modelingTransform[i];
			proj[i] = projectionTransform[i];
		}

//		printf("what is in xPos and yPos: %f, %f\n", xPos[0], yPos[0]);
		// pick a point in the window and see if it intersects a bounding sphere
		if (!useGLPipeline)
		{
			pick(xPos[0], yPos[0], mv, proj, ray);
			onClick = true;
		}
		else
		{
			double mv[16];
			double prj[16];
			glGetDoublev(GL_MODELVIEW_MATRIX, mv);
			glGetDoublev(GL_PROJECTION_MATRIX, prj);
			pick(xPos[0], yPos[0], mv, prj, ray);
			onClick = true;
		}
		// if bounding volume is hit, shade the bounding volume
		// how to determine which bounding volume is hit, especially between the two boxes?
		// else stop shading all bounding volumes
	}
	
}

int main()
{
	GLFWwindow* window;

	glfwSetErrorCallback(error_callback);

	if (!glfwInit()){
		exit(EXIT_FAILURE);
	}
	glfwWindowHint(GLFW_DEPTH_BITS, 16);
	window = glfwCreateWindow(800, 600, "PLY Viewer", NULL, NULL);
	if (!window){
		glfwTerminate();
		exit(1);
	}
	glfwMakeContextCurrent(window);

	glfwSetKeyCallback(window, key_callback);
	glfwSetMouseButtonCallback(window, mouse_callback);

	// Init. GLEW
	glewExperimental = true;
	if (glewInit() != GLEW_OK){
		fprintf(stderr, "GLEW init failed.\n");
		exit(1);
	}

	// initialize program specifics
	init();

	// check for model files and read them
	if (fileExists("data/box.ply")){
		boxModel = readPlyModel("data/box.ply");
	}
	else{
		exit(1);
	}
	if (fileExists("data/bun.ply")){
		bunnyModel = readPlyModel("data/bun.ply");
	}
	else{
		exit(1);
	}

	glShadeModel(GL_SMOOTH);
	printf("Press 'h' at any time to display what buttons are interactive\n\n");
	// main loop for window aka renderer
	while (!glfwWindowShouldClose(window))
	{
		int width, height, ratio;
		glfwGetFramebufferSize(window, &width, &height);

		glEnable(GL_DEPTH_TEST);
		
		updateProjection(width, height);
		
		displayCallback();
		
		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	glfwDestroyWindow(window);

	glfwTerminate();

	delete boxModel;
	delete bunnyModel;
	delete ray;
	delete hit;
	delete xPos;
	delete yPos;
	return 0;
}
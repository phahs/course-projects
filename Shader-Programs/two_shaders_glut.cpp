// 
// Philip Hahs
// phil.hahs@gmail.com
// CS 484
// 
// A really simple GLUT demo that renders an two teapots
// with two different shader programs.
//
// $Id: two_shaders_glut.cpp 4964 2014-04-25 04:43:20Z mshafae $
//

#include <cstdlib>
#include <cstdio>
#include <cassert>

#ifdef _WIN32
#include <Windows.h>
#pragma warning(disable : 4305)
#endif

#include <GL/glew.h>

#ifdef __APPLE__
#include <GLUT/glut.h>
#else
#include <GL/glut.h>
#endif

#ifdef FREEGLUT
#include <GL/freeglut_ext.h>
#endif

#include "GLSLShader.h"
#include "transformations.h"
#include "glut_teapot.h"

/***
* Global variables
*/
// Rotation delta in degrees
float rotationDelta;
// Translation delta
float translationDelta;

float teapotScale_A[3];
float teapotTranslation_A[3];
float teapotTranslation_B[3];

float eyePosition[3];
float upVector[3];

// When true, use glLookAt. When false, use user defined lookAt.
bool useGLLookAt;
bool useGLPerspective;
bool isPerspective;

GLSLProgram *shaderProgram_A;
GLSLProgram *shaderProgram_B;

// For the Blinn-Phong shader
float light0_position[] = { 0, 5, 10, 1 };
float light1_position[] = { 0, 5, -10, 1 };
const float light0_specular[] = { 0.6, 0.3, 0, 1 };
const float light1_specular[] = { 0, 0.3, 0.6, 1 };
// specular on teapot
const float one[] = { 1, 1, 1, 1 };
// diffuse on teapot
const float medium[] = { 0.5, 0.5, 0.5, 1 };
// ambient on teapot
const float littleBit[] = { 0.2, 0.2, 0.2, 1 };
// shininess of teapot
const float high[] = { 100 };

// For the other shader
float light2_position[] = { 0, 5, 10, 1 };
float light3_position[] = { 0, 5, -10, 1 };

float light0[4];
float light1[4];
float light2[4];
float light3[4];

// Variables to set uniform params for lighting fragment shader 
// For the Blinn-Phong shader
unsigned int uIsLightOn;
unsigned int uLight0_position;
unsigned int uLight0_color;
unsigned int uLight1_position;
unsigned int uLight1_color;
unsigned int uAmbient;
unsigned int uDiffuse;
unsigned int uSpecular;
unsigned int uShininess;
// For Gouraud shader
unsigned int uLight2_position;
unsigned int uLight3_position;


void msglVersion(void){
	fprintf(stderr, "OpenGL Version Information:\n");
	fprintf(stderr, "\tVendor: %s\n", glGetString(GL_VENDOR));
	fprintf(stderr, "\tRenderer: %s\n", glGetString(GL_RENDERER));
	fprintf(stderr, "\tOpenGL Version: %s\n", glGetString(GL_VERSION));
	fprintf(stderr, "\tGLSL Version: %s\n",
		glGetString(GL_SHADING_LANGUAGE_VERSION));
}

void printHelpMessage() {
	puts("Press 'h' to print this message again.");
	puts("Press 'a'/'d' to move the left teapot along it's positive/negative y-axis");
	puts("Press 'w'/'s' to move the left teapot along it's positive/negative x-axis");
	puts("Press 'v'/'b' to positively/negatively scale the right teapot");
	puts("Press 'g' to toggle between using glLookAt( ) or your own Look At.");
	puts("Press 'o' to toggle between perspective projection and orthographic projection modes");
	puts("Press 'p' to toggle between OpenGL's gluPerspective( ) or glOrtho( ) and your own implementation");
	puts("Press '+' or '-' to increase/decrease the amount of rotation that occurs with each arrow press.");
	puts("Press arrow up/down to rotate the camera up/down");
	puts("Press arrow left/right to rotate the camera left/right");
	puts("Press 'r' to reset the camera's transformation.");
	puts("Press ESC or 'q' to quit.");
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

void initDeltas(){
	rotationDelta = 5.0;
	translationDelta = 0.1;
}

void initToggles(){
	useGLLookAt = true;
	useGLPerspective = true;
	isPerspective = true;
}

void initTeapotTransforms(){
	teapotTranslation_A[0] = 2.0;
	teapotTranslation_A[1] = 0.0;
	teapotTranslation_A[2] = 0.0;
	teapotTranslation_B[0] = -2.0;
	teapotTranslation_B[1] = 0.0;
	teapotTranslation_B[2] = 0.0;
	teapotScale_A[0] = 1.0;
	teapotScale_A[1] = 1.0;
	teapotScale_A[2] = 1.0;
}

void initUniforms_A(){
	uIsLightOn = glGetUniformLocation(shaderProgram_A->id(),
		"isLightOn");
	uLight0_position = glGetUniformLocation(shaderProgram_A->id(),
		"light0_position");
	uLight0_color = glGetUniformLocation(shaderProgram_A->id(),
		"light0_color");
	uLight1_position = glGetUniformLocation(shaderProgram_A->id(),
		"light1_position");
	uLight1_color = glGetUniformLocation(shaderProgram_A->id(),
		"light1_color");
	uAmbient = glGetUniformLocation(shaderProgram_A->id(), "ambient");
	uDiffuse = glGetUniformLocation(shaderProgram_A->id(), "diffuse");
	uSpecular = glGetUniformLocation(shaderProgram_A->id(), "specular");
	uShininess = glGetUniformLocation(shaderProgram_A->id(), "shininess");
}

void activateUniforms_A(){
	glUniform4fv(uLight0_position, 1, light0);
	glUniform4fv(uLight0_color, 1, light0_specular);
	glUniform4fv(uLight1_position, 1, light1);
	glUniform4fv(uLight1_color, 1, light1_specular);
	glUniform4fv(uAmbient, 1, littleBit);
	glUniform4fv(uDiffuse, 1, medium);
	glUniform4fv(uSpecular, 1, one);
	glUniform1fv(uShininess, 1, high);
	glUniform1i(uIsLightOn, true);
}

void initUniforms_B(){
	uLight2_position = glGetUniformLocation(shaderProgram_B->id(),
		"light2_position");
	uLight3_position = glGetUniformLocation(shaderProgram_B->id(),
		"light3_position");

}

void activateUniforms_B(){
	glUniform4fv(uLight2_position, 1, light2);
	glUniform4fv(uLight3_position, 1, light3);
}

void init() {
	initEyePosition();
	initUpVector();
	initDeltas();
	initToggles();
	initTeapotTransforms();

	// Load the shader program
	// Blinn-Phong
	const char* vertexShaderSource_A = "golden.vert";
	const char* fragmentShaderSource_A = "golden.frag";
	FragmentShader fragmentShader_A(fragmentShaderSource_A);
	VertexShader vertexShader_A(vertexShaderSource_A);
	shaderProgram_A = new GLSLProgram();
	shaderProgram_A->attach(vertexShader_A);
	shaderProgram_A->attach(fragmentShader_A);
	shaderProgram_A->link();
	shaderProgram_A->activate();
	printf("Shader program A built from %s and %s.\n",
		vertexShaderSource_A, fragmentShaderSource_A);
	shaderProgram_A->deactivate();
	// My Shader
	const char* vertexShaderSource_B = "mine.vert";
	const char* fragmentShaderSource_B = "mine.frag";
	FragmentShader fragmentShader_B(fragmentShaderSource_B);
	VertexShader vertexShader_B(vertexShaderSource_B);
	shaderProgram_B = new GLSLProgram();
	shaderProgram_B->attach(vertexShader_B);
	shaderProgram_B->attach(fragmentShader_B);
	shaderProgram_B->link();
	shaderProgram_B->activate();
	printf("Shader program B built from %s and %s.\n",
		vertexShaderSource_B, fragmentShaderSource_B);
	shaderProgram_B->deactivate();

	// Set up uniform variables
	// For the Blinn-Phong shader
	initUniforms_A();
	// For the Gouraud shader
	initUniforms_B();
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

void transformVecByModelView(float outVec[4], float inVec[4]){
	float modelview[16];
	glGetFloatv(GL_MODELVIEW_MATRIX, modelview);
	matMultVec4f(outVec, inVec, modelview);
}

void updateProjection(int width, int height){
	float projectionTransform[16];
	glViewport(0, 0, (GLsizei)width, (GLsizei)height);
	double ratio = double(width) / double(height);
	glMatrixMode(GL_PROJECTION);
	if (isPerspective && useGLPerspective){
		// perspective and use OpenGL
		glPushMatrix();
		glLoadIdentity();
		gluPerspective(90.0, ratio, 1.0, 25.0);
		glGetFloatv(GL_PROJECTION_MATRIX, projectionTransform);
		glPopMatrix();
	}
	else if (isPerspective && !useGLPerspective){
		// perspective and do not use OpenGL
		myPerspective(projectionTransform, 90.0, ratio, 1.0, 25.0);
	}
	else if (!isPerspective && useGLPerspective){
		// orthographic and use OpenGL
		glPushMatrix();
		glLoadIdentity();
		glOrtho(-7.0, 7.0, -7.0, 7.0, 1.0, 25.0);
		glGetFloatv(GL_PROJECTION_MATRIX, projectionTransform);
		glPopMatrix();
	}
	else if (!isPerspective && !useGLPerspective){
		// orthographic and do not use OpenGL
		myOrtho(projectionTransform, -7.0, 7.0, -7.0, 7.0, 1.0, 25.0);
	}
	glLoadMatrixf(projectionTransform);
}

void reshapeCallback(int width, int height){
	if (height == 0){
		height = 1;
	}
	updateProjection(width, height);
}

void keyboardCallback(unsigned char key, int x, int y){
	float scale[16];
	float translate[16];
	float temp[3];

	switch (key){
	case 27: // The 'esc' key
	case 'q':
#ifdef FREEGLUT
		glutLeaveMainLoop();
#else
		exit(0);
#endif
		break;
	case 'a':
		// move the left teapot (B) along it's positive y-axis
		// Please implement this feature.
		teapotTranslation_B[0] = teapotTranslation_B[0] - 1;
		teapotTranslation_B[1] = teapotTranslation_B[1] + 0;
		teapotTranslation_B[2] = teapotTranslation_B[2] + 0;
		break;
	case 'd':
		// move the left teapot (B) along it's negative y-axis
		// Please implement this feature.
		teapotTranslation_B[0] = teapotTranslation_B[0] + 1;
		teapotTranslation_B[1] = teapotTranslation_B[1] + 0;
		teapotTranslation_B[2] = teapotTranslation_B[2] + 0;
		break;
	case 'w':
		// move the left teapot (B) along it's positive x-axis
		// Please implement this feature.
		teapotTranslation_B[0] = teapotTranslation_B[0] + 0;
		teapotTranslation_B[1] = teapotTranslation_B[1] + 1;
		teapotTranslation_B[2] = teapotTranslation_B[2] + 0;
		break;
	case 's':
		// move the left teapot (B) along it's negative x-axis
		// Please implement this feature.
		teapotTranslation_B[0] = teapotTranslation_B[0] + 0;
		teapotTranslation_B[1] = teapotTranslation_B[1] - 1;
		teapotTranslation_B[2] = teapotTranslation_B[2] + 0;
		break;
	case 'v':
		// positively scale the right teapot (A)
		// Please implement this feature.
		myScalef(scale, 1.5, 1.5, 1.5);

		for (int i = 0; i < 3; i++)
		{
			temp[i] = teapotScale_A[i];
		}

		teapotScale_A[0] = scale[0] * temp[0] + scale[4] * temp[1] + scale[8] * temp[2];
		teapotScale_A[1] = scale[1] * temp[0] + scale[5] * temp[1] + scale[9] * temp[2];
		teapotScale_A[2] = scale[2] * temp[0] + scale[6] * temp[1] + scale[10] * temp[2];
		break;
	case 'b':
		// negatively scale the right teapot (A)
		// Please implement this feature.
		myScalef(scale, .5, .5, .5);

		for (int i = 0; i < 3; i++)
		{
			temp[i] = teapotScale_A[i];
		}

		teapotScale_A[0] = scale[0] * temp[0] + scale[4] * temp[1] + scale[8] * temp[2];
		teapotScale_A[1] = scale[1] * temp[0] + scale[5] * temp[1] + scale[9] * temp[2];
		teapotScale_A[2] = scale[2] * temp[0] + scale[6] * temp[1] + scale[10] * temp[2];
		break;
	case 'o':
		isPerspective = !isPerspective;
		updateProjection(
			glutGet(GLUT_WINDOW_WIDTH),
			glutGet(GLUT_WINDOW_HEIGHT)
			);
		printf("Using perspective projection? %s\n",
			(isPerspective ? "Yes" : "No"));
		break;
	case 'p':
		useGLPerspective = !useGLPerspective;
		updateProjection(
			glutGet(GLUT_WINDOW_WIDTH),
			glutGet(GLUT_WINDOW_HEIGHT)
			);
		printf("Using OpenGL's perspective functions? %s\n",
			(useGLPerspective ? "Yes" : "No"));
		break;
	case '+':
		rotationDelta += 1.0;
		printf("Rotation delta set to %g\n", rotationDelta);
		break;
	case '-':
		rotationDelta -= 1.0;
		printf("Rotation delta set to %g\n", rotationDelta);
		break;
	case 'g':
		useGLLookAt = !useGLLookAt;
		printf("Using OpenGL's gluLookAt? %s\n",
			(useGLLookAt ? "Yes" : "No"));
		break;
	case 'h':
		printHelpMessage();
		break;
	case 'r':
		initEyePosition();
		initUpVector();
		initDeltas();
		initToggles();
		initTeapotTransforms();
		printf("Eye position, up vector and rotation delta reset.\n");
		break;
	default:
		fprintf(stderr, "You pushed '%c' (%d).\n", key, key);
		break;
	}
	glutPostRedisplay();
}

void specialCallback(int key, int x, int y){
	float centerPosition[] = { 0.0, 0.0, 0.0 };
	switch (key){
	case GLUT_KEY_LEFT:
		rotateCameraLeft(rotationDelta, eyePosition,
			centerPosition, upVector);
		break;
	case GLUT_KEY_RIGHT:
		rotateCameraLeft(-rotationDelta, eyePosition,
			centerPosition, upVector);
		break;
	case GLUT_KEY_UP:
		rotateCameraUp(rotationDelta, eyePosition,
			centerPosition, upVector);
		break;
	case GLUT_KEY_DOWN:
		rotateCameraUp(-rotationDelta, eyePosition,
			centerPosition, upVector);
		break;
	}
	glutPostRedisplay();
}

void displayCallback(){
	const float centerPosition[] = { 0.0, 0.0, 0.0 };

	float viewingTransform[16];
	float modelingTransform[16];

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glMatrixMode(GL_MODELVIEW);

	myLookAt(viewingTransform,
		eyePosition[0], eyePosition[1], eyePosition[2],
		centerPosition[0], centerPosition[1], centerPosition[2],
		upVector[0], upVector[1], upVector[2]
		);

	// Set light & material properties for the teapot;
	// lights are transformed by current modelview matrix
	// such that they are positioned correctly in the scene.
	matMultVec4f(light0, light0_position, viewingTransform);
	matMultVec4f(light1, light1_position, viewingTransform);
	matMultVec4f(light2, light2_position, viewingTransform);
	matMultVec4f(light3, light3_position, viewingTransform);

	shaderProgram_A->activate();
	activateUniforms_A();
	glLoadIdentity();
	glMultMatrixf(viewingTransform);
	myScalef(modelingTransform,
		teapotScale_A[0], teapotScale_A[1], teapotScale_A[2]);
	glMultMatrixf(modelingTransform);
	myTranslatef(modelingTransform,
		teapotTranslation_A[0],
		teapotTranslation_A[1],
		teapotTranslation_A[2]);
	glMultMatrixf(modelingTransform);
	_glutSolidTeapot(1.3);

	shaderProgram_B->activate();
	activateUniforms_B();
	glLoadIdentity();
	glMultMatrixf(viewingTransform);
	myTranslatef(modelingTransform,
		teapotTranslation_B[0],
		teapotTranslation_B[1],
		teapotTranslation_B[2]);
	glMultMatrixf(modelingTransform);
	_glutSolidTeapot(1.3);

	glutSwapBuffers();
}

void updateCallback(int x){
	glutPostRedisplay();
	glutTimerFunc(16, updateCallback, 0);
}

int main(int argc, char* argv[]){
	// Init. GLUT
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH);
	glutInitWindowSize(500, 500);
	glutCreateWindow("Two Shaders");

	// Init. GLEW
	glewExperimental = true;
	if (glewInit() != GLEW_OK){
		fprintf(stderr, "GLEW init failed.\n");
		exit(1);
	}

	// Set OpenGL State
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LESS);

	// Application specific initialization
	init();

	// Register callbacks with GLUT
	glutKeyboardFunc(keyboardCallback);
	glutSpecialFunc(specialCallback);
	glutDisplayFunc(displayCallback);
	glutReshapeFunc(reshapeCallback);
	glutTimerFunc(16, updateCallback, 0);

	msglVersion();

	puts("Press 'h' to see a help message at any time.");

	glutMainLoop();
	return(0);
}

#include "Wrapper.h"

FileManager fileManager;

void ReadInto(float objectNumber, float locationx, float locationy, float locationz, float rotationx,
	float rotationy, float rotationz, float scalex, float scaley, float scalez)
{
	fileManager.ReadInto(objectNumber, locationx, locationy, locationz, rotationx,
		rotationy, rotationz, scalex, scaley, scalez);
}

float ReadOut(int j, const char *fileName)
{
	return fileManager.ReadOut(j, fileName);
}

void startWriting(const char *fileName)
{
	fileManager.startWriting(fileName);
}

void endWriting()
{
	fileManager.endWriting();
}

int returnLines(const char *fileName)
{
	return fileManager.returnLines(fileName);
}
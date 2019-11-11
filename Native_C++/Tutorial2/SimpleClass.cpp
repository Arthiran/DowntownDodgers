#include "SimpleClass.h"

int SimpleClass::SimpleFunction()
{
	return 1;
}

void SimpleClass::SavePosition(float posX, float posY, float posZ)
{
	setX(posX);
	setY(posY);
	setZ(posZ);
}

void SimpleClass::LoadPosition(float posX, float posY, float posZ)
{
	posX = X;
	posY = Y;
	posZ = Z;
}

void SimpleClass::setX(float posX)
{
	X = posX;
}

void SimpleClass::setY(float posY)
{
	Y = posY;
}

void SimpleClass::setZ(float posZ)
{
	Z = posZ;
}

float SimpleClass::getX()
{
	return X;
}

float SimpleClass::getY()
{
	return Y;
}

float SimpleClass::getZ()
{
	return Z;
}

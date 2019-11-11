#pragma once

#include "PluginSettings.h"
#include "FileManager.h"

class PLUGIN_API SimpleClass
{
public:
	int SimpleFunction();

	void SavePosition(float posX, float posY, float posZ);

	void LoadPosition(float posX, float posY, float posZ);

	void setX(float posX);
	void setY(float posY);
	void setZ(float posZ);

	float getX();
	float getY();
	float getZ();

protected:
	float X;
	float Y;
	float Z;

private:


};
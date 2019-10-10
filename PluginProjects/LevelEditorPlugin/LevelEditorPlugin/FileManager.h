#pragma once
#include "PluginSettings.h"
#include <fstream>
#include <iostream>
#include <string>
#include <vector>
using namespace std;


class PLUGIN_API FileManager
{
public:
	ofstream myFile;

	void startWriting(const char *fileName);
	void endWriting();
	void ReadInto(float objectNumber, float locationx, float locationy, float locationz, float rotationx, 
		float rotationy, float rotationz, float scalex, float scaley, float scalez);
	float ReadOut(int j, const char *fileName);
	int returnLines(const char *fileName);

	vector <float> read;

};
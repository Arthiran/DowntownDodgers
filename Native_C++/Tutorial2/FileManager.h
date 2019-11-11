#pragma once
#include "PluginSettings.h"

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>

class PLUGIN_API FileManager
{
public:

	//This function will write the saved values to a text file
	void WriteFile(float time);
	//This function will read the text file and assign the saved values to the players position
	void ReadFile(std::string fileName);

	//Saves the values to variables and calls the writer
	void SaveTime(float aTime);

	//Calls the reader
	float LoadTime();

	//Setters
	void SetTime(float aTime);

	//Getters
	float GetTime();

	//Variables to hold the position
	float theTime = 0;

private:

	std::ofstream write;
	std::ifstream read;
};
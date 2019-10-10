#include "FileManager.h"

void FileManager::startWriting(const char *fileName)
{
	myFile.open(fileName);

}
void FileManager::endWriting()
{
	myFile.close();
}

void FileManager::ReadInto(float objectNumber, float locationx, float locationy, float locationz, float rotationx,
	float rotationy, float rotationz, float scalex, float scaley, float scalez)
{

	myFile << objectNumber << "\n";
	myFile << locationx << "\n";
	myFile << locationy << "\n";
	myFile << locationz << "\n";
	myFile << rotationx << "\n";
	myFile << rotationy << "\n";
	myFile << rotationz << "\n";
	myFile << scalex << "\n";
	myFile << scaley << "\n";
	myFile << scalez << "\n";
	

}

float FileManager::ReadOut(int j, const char *fileName)
{
	read.clear();
	ifstream myReadFile;
	myReadFile.open(fileName);
	float value;
	while (myReadFile >> value)
	{
		read.push_back(value);
		
	}
	myReadFile.close();
	return read[j];

	
}

int FileManager::returnLines(const char *fileName)
{
	read.clear();
	ifstream myReadFile;
	myReadFile.open(fileName);
	int value = 0;
	string tempString;
	while (getline(myReadFile, tempString))
	{
		value++;
	}
	myReadFile.close();
	return value;


}
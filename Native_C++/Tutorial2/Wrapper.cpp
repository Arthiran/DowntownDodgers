#include "Wrapper.h"

FileManager fileMan;

void SaveTime(float aTime)
{
	return fileMan.SaveTime(aTime);
}

float LoadTime()
{
	return fileMan.LoadTime();
}

float GetTime()
{
	return fileMan.GetTime();
}
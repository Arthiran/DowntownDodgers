#include "FileManager.h"

//This function will write the saved values to a text file
void FileManager::WriteFile(float time)
{
	//Open the file
	write.open("save.txt");
	//If the file is open
	if (write.is_open())
	{
		//Write each value to the file
		write << time << "\n";

		//Close the file
		write.close();
	}
}

//This function will read the text file and assign the saved values to the players position
void FileManager::ReadFile(std::string fileName)
{
	//float posVal;
	std::string line;
	//Open the file
	read.open(fileName);

	//myVecs = new Vec3[size];

	float temp;

	//int posCounter = 0;
	//int arrCounter = 0;

	//If the file is open
	if (read.is_open())
	{
		//Go through each line in the file
		while (std::getline(read, line))
		{
			//Go through each line in the file and covert the string of the number into a float
			temp = std::stof(line);
			SetTime(temp);
		}
		//Close the file
		read.close();
	}
}

//Saves the values to variables and calls the writer
void FileManager::SaveTime(float aTime)
{
	float tempTime;
	//Store the values passed into the variables
	SetTime(aTime);

	tempTime = aTime;

	//Call the file writer
	WriteFile(tempTime);
}

//Calls the reader
float FileManager::LoadTime()
{
	ReadFile("save.txt");

	return theTime;
}

void FileManager::SetTime(float aTime)
{
	theTime = aTime;
}

float FileManager::GetTime()
{
	return theTime;
}

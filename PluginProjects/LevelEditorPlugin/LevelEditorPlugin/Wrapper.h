#pragma once
#include "PluginSettings.h"
#include "FileManager.h"
#ifdef __cplusplus

extern "C"
{
#endif
	//I/O
	PLUGIN_API void ReadInto(float objectNumber, float locationx, float locationy, float locationz, float rotationx,
		float rotationy, float rotationz, float scalex, float scaley, float scalez);
	PLUGIN_API void startWriting(const char *fileName);
	PLUGIN_API void endWriting();
	PLUGIN_API int returnLines(const char *fileName);
	PLUGIN_API float ReadOut(int j, const char *fileName);
#ifdef __cplusplus
}
#endif

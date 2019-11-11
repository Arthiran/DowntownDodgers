#pragma once

#include "PluginSettings.h"
#include "FileManager.h"

#ifdef __cplusplus
extern "C"
{
#endif

	// Put your functions here

	PLUGIN_API void SaveTime(float aTime);

	PLUGIN_API float LoadTime();

	PLUGIN_API float GetTime();


#ifdef __cplusplus
}
#endif
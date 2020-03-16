#pragma once
#include "Stdafx.h"
LPTSTR GetDeviceWithInfo(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL GetDeviceStatus(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceWithInfo(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, LPCTSTR Info);
BOOL DumpDevice(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceDescr(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceClass(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceStatus(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceResourcesOfType(DEVINST DevInst, HMACHINE MachineHandle, LOG_CONF Config, RESOURCEID ReqResId);
BOOL DumpDeviceResources(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
UINT DumpDeviceDriversCallback(PVOID Context, UINT Notification, UINT_PTR Param1, UINT_PTR Param2);
BOOL DumpDeviceDriverFiles(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpArray(int pad, LPTSTR* Array);
BOOL DumpDeviceHwIds(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceDriverNodes(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
BOOL DumpDeviceStack(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
void FormatToStream(FILE* stream, DWORD fmt,...);
void Padding(int pad);

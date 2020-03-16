#pragma once
#include "Stdafx.h"
#define FIND_DEVICE         0x00000001 // display device
#define FIND_STATUS         0x00000002 // display status of device
#define FIND_RESOURCES      0x00000004 // display resources of device
#define FIND_DRIVERFILES    0x00000008 // display drivers used by device
#define FIND_HWIDS          0x00000010 // display hw/compat id's used by device
#define FIND_DRIVERNODES    0x00000020 // display driver nodes for a device.
#define FIND_CLASS          0x00000040 // display device's setup class
#define FIND_STACK          0x00000080 // display device's driver-stack
#define INSTANCEID_PREFIX_CHAR TEXT('@') // character used to prefix instance ID's
#define CLASS_PREFIX_CHAR      TEXT('=') // character used to prefix class name
#define WILD_CHAR              TEXT('*') // wild character
#define QUOTE_PREFIX_CHAR      TEXT('\'') // prefix character to ignore wild characters
#define SPLIT_COMMAND_SEP      TEXT(":=") // whole word, indicates end of id's
// Exit codes
#define EXIT_OK      (0)
#define EXIT_REBOOT  (1)
#define EXIT_FAIL    (2)
#define EXIT_USAGE   (3)
struct IdEntry {
    LPCTSTR String;     // string looking for
    LPCTSTR Wild;       // first wild character if any
    BOOL    InstanceId;
};
struct GenericContext {
    DWORD count;
    DWORD control;
    BOOL  reboot;
    LPCTSTR strSuccess;
    LPCTSTR strReboot;
    LPCTSTR strFail;
};
typedef int (*CallbackFunc)(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Index, LPVOID Context);
int ControlCallback(HDEVINFO Devs,PSP_DEVINFO_DATA DevInfo,DWORD Index,LPVOID Context);
void DelMultiSz(LPTSTR* Array);
int FindCallback(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Index, LPVOID Context);
BOOL FindCurrentDriver(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, PSP_DRVINFO_DATA DriverInfoData);
LPTSTR GetDeviceDescription(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo);
LPTSTR GetDeviceStringProperty(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Prop);
IdEntry GetIdType(LPCTSTR Id);
LPTSTR* GetDevMultiSz(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Prop);
LPTSTR* GetMultiSzIndexArray(LPTSTR MultiSz);
LPTSTR* GetRegMultiSz(HKEY hKey, LPCTSTR Val);
BOOL WildCardMatch(LPCTSTR Item, const IdEntry & MatchEntry);
BOOL WildCompareHwIds(LPTSTR* Array, const IdEntry & MatchEntry);
int EnumerateDevices(LPCTSTR BaseName, LPCTSTR Machine, DWORD Flags, int argc, LPTSTR argv[], CallbackFunc Callback, LPVOID Context);

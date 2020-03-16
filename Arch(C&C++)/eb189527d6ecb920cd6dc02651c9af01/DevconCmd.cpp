#include "Devcon.h"
#include "DevconDump.h"
#include "DevconDumpMsg.h"
#include "DevconCmd.h"
int CmdClassDeviceList(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[], LPTSTR* rslts[])
/*++
Routine Description:
    LISTCLASS <name>....
    lists all devices for each specified class
    there can be more than one physical class for a class name (shouldn't be
    though) in such cases, list each class
    if machine given, list devices for that machine
Arguments:
    BaseName  - name of executable
    Machine   - if non-NULL, remote machine
    argc/argv - remaining parameters - list of class names
Return Value:
    EXIT_xxxx
--*/
{
    BOOL classListed = FALSE;
    BOOL devListed = FALSE;
    DWORD reqGuids = 16;
    int argIndex;
    int failcode = EXIT_FAIL;
    LPGUID guids = NULL;
    HDEVINFO devs = INVALID_HANDLE_VALUE;
    if(!argc) 
	{
        return EXIT_USAGE;
    }
    guids = new GUID[reqGuids];
    if(!guids) 
	{
        goto final;
    }
    for(argIndex = 0; argIndex < argc; argIndex++) 
	{
        DWORD numGuids;
        DWORD index;
        if(!(argv[argIndex] && argv[argIndex][0])) 
		{
            continue;
        }
        // there could be one to many name to GUID mapping
        while(!SetupDiClassGuidsFromNameEx(argv[argIndex],guids,reqGuids,&numGuids,Machine,NULL)) 
		{
            if(GetLastError() != ERROR_INSUFFICIENT_BUFFER) 
			{
                goto final;
            }
            delete [] guids;
            reqGuids = numGuids;
            guids = new GUID[reqGuids];
            if(!guids) 
			{
                goto final;
            }
        }
        if(numGuids == 0) 
		{
            FormatToStream(stdout,Machine?MSG_LISTCLASS_NOCLASS:MSG_LISTCLASS_NOCLASS_LOCAL,argv[argIndex],Machine);
            continue;
        }
        for(index = 0; index < numGuids; index++) 
		{
            TCHAR className[MAX_CLASS_NAME_LEN];
            TCHAR classDesc[LINE_LEN];
            DWORD devCount = 0;
            SP_DEVINFO_DATA devInfo;
            DWORD devIndex;
            devs = SetupDiGetClassDevsEx(&guids[index],NULL,NULL,DIGCF_PRESENT,NULL,Machine,NULL);
            if(devs != INVALID_HANDLE_VALUE) 
			{
                // count number of devices
                devInfo.cbSize = sizeof(devInfo);
                while(SetupDiEnumDeviceInfo(devs, devCount, &devInfo)) 
				{
                    devCount++;
                }
            }
            if(!SetupDiClassNameFromGuidEx(&guids[index], className, MAX_CLASS_NAME_LEN, NULL, Machine, NULL)) 
			{
                lstrcpyn(className,TEXT("?"),MAX_CLASS_NAME_LEN);
            }
            if(!SetupDiGetClassDescriptionEx(&guids[index], classDesc, LINE_LEN, NULL, Machine, NULL)) 
			{
                lstrcpyn(classDesc,className,LINE_LEN);
            }
            // how many devices?
            if (!devCount) 
			{
                FormatToStream(stdout,Machine?MSG_LISTCLASS_HEADER_NONE:MSG_LISTCLASS_HEADER_NONE_LOCAL,className,classDesc,Machine);
            } 
			else 
			{
                FormatToStream(stdout,Machine?MSG_LISTCLASS_HEADER:MSG_LISTCLASS_HEADER_LOCAL,devCount,className,classDesc,Machine);
                for(devIndex = 0; SetupDiEnumDeviceInfo(devs, devIndex, &devInfo); devIndex++) 
				{
					rslts[devIndex] = (LPTSTR*)GetDeviceWithInfo(devs, &devInfo);
//                    DumpDevice(devs, &devInfo);
                }
            }
            if(devs != INVALID_HANDLE_VALUE) 
			{
                SetupDiDestroyDeviceInfoList(devs);
                devs = INVALID_HANDLE_VALUE;
            }
        }
    }
    failcode = 0;
final:
    if(guids) 
	{
        delete [] guids;
    }
    if(devs != INVALID_HANDLE_VALUE) 
	{
        SetupDiDestroyDeviceInfoList(devs);
    }
    return failcode;
}
int CmdClasses(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[], LPTSTR* rslts[])
/*++
Routine Description:
    CLASSES command
    lists classes on (optionally) specified machine
    format as <name>: <destination>
Arguments:
    BaseName  - name of executable
    Machine   - if non-NULL, remote machine
    argc/argv - remaining parameters - ignored
Return Value:
    EXIT_xxxx
--*/
{
    DWORD reqGuids = 128;
    DWORD numGuids;
    LPGUID guids = NULL;
    DWORD index;
    int failcode = EXIT_FAIL;
    guids = new GUID[reqGuids];
    if(!guids) 
	{
        goto final;
    }
    if(!SetupDiBuildClassInfoListEx(0, guids, reqGuids, &numGuids, Machine, NULL)) 
	{
        do 
		{
            if(GetLastError() != ERROR_INSUFFICIENT_BUFFER) 
			{
                goto final;
            }
            delete [] guids;
            reqGuids = numGuids;
            guids = new GUID[reqGuids];
            if(!guids) 
			{
                goto final;
            }
        } while(!SetupDiBuildClassInfoListEx(0, guids, reqGuids, &numGuids, Machine, NULL));
    }
    FormatToStream(stdout,Machine?MSG_CLASSES_HEADER:MSG_CLASSES_HEADER_LOCAL,numGuids,Machine);
    for(index = 0; index < numGuids; index++) 
	{
        TCHAR className[MAX_CLASS_NAME_LEN];
        TCHAR classDesc[LINE_LEN];
        if(!SetupDiClassNameFromGuidEx(&guids[index], className, MAX_CLASS_NAME_LEN, NULL, Machine, NULL)) 
		{
            lstrcpyn(className,TEXT("?"),MAX_CLASS_NAME_LEN);
        }
        if(!SetupDiGetClassDescriptionEx(&guids[index], classDesc, LINE_LEN, NULL, Machine, NULL)) 
		{
            lstrcpyn(classDesc,className,LINE_LEN);
        }
        _tprintf(TEXT("%-20s: %s\n"), className, classDesc);
    }
    failcode = EXIT_OK;
final:
    if(guids) 
	{
        delete [] guids;
    }
    return failcode;
}
int CmdDeviceEnable(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[])
/*++
Routine Description:
    ENABLE <id> ...
    use EnumerateDevices to do hardwareID matching
    for each match, attempt to enable global, and if needed, config specific
Arguments:
    BaseName  - name of executable
    Machine   - must be NULL (local machine only)
    argc/argv - remaining parameters - passed into EnumerateDevices
Return Value:
    EXIT_xxxx (EXIT_REBOOT if reboot is required)
--*/
{
    GenericContext context;
    TCHAR strEnable[80];
    TCHAR strReboot[80];
    TCHAR strFail[80];
    int failcode = EXIT_FAIL;
    if(!argc) 
	{
        // arguments required
        return EXIT_USAGE;
    }
    if(Machine) 
	{
        // must be local machine as we need to involve class/co installers
        return EXIT_USAGE;
    }
/*
    if(!LoadString(NULL,IDS_ENABLED,strEnable,ARRAYSIZE(strEnable))) 
	{
        return EXIT_FAIL;
    }
    if(!LoadString(NULL,IDS_ENABLED_REBOOT,strReboot,ARRAYSIZE(strReboot))) 
	{
        return EXIT_FAIL;
    }
    if(!LoadString(NULL,IDS_ENABLE_FAILED,strFail,ARRAYSIZE(strFail))) 
	{
        return EXIT_FAIL;
    }
*/
    context.control = DICS_ENABLE; // DICS_PROPCHANGE DICS_ENABLE DICS_DISABLE
    context.reboot = FALSE;
    context.count = 0;
    context.strReboot = strReboot;
    context.strSuccess = strEnable;
    context.strFail = strFail;
    failcode = EnumerateDevices(BaseName,Machine,DIGCF_PRESENT,argc,argv,ControlCallback,&context);
    if(failcode == EXIT_OK) 
	{
        if(!context.count) 
		{
            FormatToStream(stdout,MSG_ENABLE_TAIL_NONE);
        } 
		else if(!context.reboot) 
		{
            FormatToStream(stdout,MSG_ENABLE_TAIL,context.count);
        } 
		else 
		{
            FormatToStream(stdout,MSG_ENABLE_TAIL_REBOOT,context.count);
            failcode = EXIT_REBOOT;
        }
    }
    return failcode;
}
int CmdDeviceDisable(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[])
/*++
Routine Description:
    DISABLE <id> ...
    use EnumerateDevices to do hardwareID matching
    for each match, attempt to disable global
Arguments:
    BaseName  - name of executable
    Machine   - must be NULL (local machine only)
    argc/argv - remaining parameters - passed into EnumerateDevices
Return Value:
    EXIT_xxxx (EXIT_REBOOT if reboot is required)
--*/
{
    GenericContext context;
    TCHAR strDisable[80];
    TCHAR strReboot[80];
    TCHAR strFail[80];
    int failcode = EXIT_FAIL;
//    UNREFERENCED_PARAMETER(Flags);
	if(!argc) 
	{
        // arguments required
        return EXIT_USAGE;
    }
    if(Machine) 
	{
        // must be local machine as we need to involve class/co installers
        return EXIT_USAGE;
    }
/*
    if(!LoadString(NULL,IDS_DISABLED,strDisable,ARRAYSIZE(strDisable))) 
	{
        return EXIT_FAIL;
    }
    if(!LoadString(NULL,IDS_DISABLED_REBOOT,strReboot,ARRAYSIZE(strReboot))) 
	{
        return EXIT_FAIL;
    }
    if(!LoadString(NULL,IDS_DISABLE_FAILED,strFail,ARRAYSIZE(strFail))) 
	{
        return EXIT_FAIL;
    }
*/
    context.control = DICS_DISABLE; // DICS_PROPCHANGE DICS_ENABLE DICS_DISABLE
    context.reboot = FALSE;
    context.count = 0;
    context.strReboot = strReboot;
    context.strSuccess = strDisable;
    context.strFail = strFail;
    failcode = EnumerateDevices(BaseName,Machine,DIGCF_PRESENT,argc,argv,ControlCallback,&context);
    if(failcode == EXIT_OK) 
	{
        if(!context.count) 
		{
            FormatToStream(stdout,MSG_DISABLE_TAIL_NONE);
        } 
		else if(!context.reboot) 
		{
            FormatToStream(stdout,MSG_DISABLE_TAIL,context.count);
        } 
		else 
		{
            FormatToStream(stdout,MSG_DISABLE_TAIL_REBOOT,context.count);
            failcode = EXIT_REBOOT;
        }
    }
    return failcode;
}
int CmdDeviceStatus(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[])
/*++
Routine Description:
    STATUS <id> ...
    use EnumerateDevices to do hardwareID matching
    for each match, dump status to stdout
    note that we only enumerate present devices
Arguments:
    BaseName  - name of executable
    Machine   - if non-NULL, remote machine
    argc/argv - remaining parameters - passed into EnumerateDevices
Return Value:
    EXIT_xxxx
--*/
{
    GenericContext context;
    int failcode;
    if(!argc) 
	{
        return EXIT_USAGE;
    }
    context.count = 0;
    context.control = FIND_DEVICE | FIND_STATUS;
    failcode = EnumerateDevices(BaseName,Machine,DIGCF_PRESENT,argc,argv,FindCallback,&context);
    if(failcode == EXIT_OK) 
	{
        if(!context.count) 
		{
            FormatToStream(stdout,Machine?MSG_FIND_TAIL_NONE:MSG_FIND_TAIL_NONE_LOCAL,Machine);
        } 
		else 
		{
            FormatToStream(stdout,Machine?MSG_FIND_TAIL:MSG_FIND_TAIL_LOCAL,context.count,Machine);
        }
    }
    return failcode;
}
int CmdFind(LPCTSTR BaseName, LPCTSTR Machine, int argc, TCHAR* argv[], LPTSTR* rslts[])
/*++
Routine Description:
    FIND <id> ...
    use EnumerateDevices to do hardwareID matching
    for each match, dump to stdout
    note that we only enumerate present devices
Arguments:
    BaseName  - name of executable
    Machine   - if non-NULL, remote machine
    argc/argv - remaining parameters - passed into EnumerateDevices
Return Value:
    EXIT_xxxx
--*/
{
    GenericContext context;
    int failcode;
    if(!argc) 
	{
        return EXIT_USAGE;
    }
    context.count = 0;
    context.control = 0;
    failcode = EnumerateDevices(BaseName, Machine, DIGCF_PRESENT, argc, argv, FindCallback, &context);
    if(failcode == EXIT_OK) 
	{
        if(!context.count) 
		{
            FormatToStream(stdout,Machine?MSG_FIND_TAIL_NONE:MSG_FIND_TAIL_NONE_LOCAL,Machine);
        } 
		else 
		{
            FormatToStream(stdout,Machine?MSG_FIND_TAIL:MSG_FIND_TAIL_LOCAL,context.count,Machine);
        }
    }
    return failcode;
}

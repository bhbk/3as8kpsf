#include "Devcon.h"
#include "DevconDumpMsg.h"
#include "DevconDump.h"
int ControlCallback(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Index, LPVOID Context)
/*++
Routine Description:
    Callback for use by Enable/Disable/Restart
    Invokes DIF_PROPERTYCHANGE with correct parameters
    uses SetupDiCallClassInstaller so cannot be done for remote devices
    Don't use CM_xxx API's, they bypass class/co-installers and this is bad.
    In Enable case, we try global first, and if still disabled, enable local
Arguments:
    Devs    )_ uniquely identify the device
    DevInfo )
    Index    - index of device
    Context  - GenericContext
Return Value:
    EXIT_xxxx
--*/
{
    SP_PROPCHANGE_PARAMS pcp;
    GenericContext *pControlContext = (GenericContext*)Context;
    SP_DEVINSTALL_PARAMS devParams;
    switch(pControlContext->control) {
        case DICS_ENABLE:
            //
            // enable both on global and config-specific profile
            // do global first and see if that succeeded in enabling the device
            // (global enable doesn't mark reboot required if device is still
            // disabled on current config whereas vice-versa isn't true)
            //
            pcp.ClassInstallHeader.cbSize = sizeof(SP_CLASSINSTALL_HEADER);
            pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
            pcp.StateChange = pControlContext->control;
            pcp.Scope = DICS_FLAG_GLOBAL;
            pcp.HwProfile = 0;
            //
            // don't worry if this fails, we'll get an error when we try config-
            // specific.
            if(SetupDiSetClassInstallParams(Devs,DevInfo,&pcp.ClassInstallHeader,sizeof(pcp))) {
               SetupDiCallClassInstaller(DIF_PROPERTYCHANGE,Devs,DevInfo);
            }
            //
            // now enable on config-specific
            //
            pcp.ClassInstallHeader.cbSize = sizeof(SP_CLASSINSTALL_HEADER);
            pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
            pcp.StateChange = pControlContext->control;
            pcp.Scope = DICS_FLAG_CONFIGSPECIFIC;
            pcp.HwProfile = 0;
            break;
        default:
            //
            // operate on config-specific profile
            //
            pcp.ClassInstallHeader.cbSize = sizeof(SP_CLASSINSTALL_HEADER);
            pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
            pcp.StateChange = pControlContext->control;
            pcp.Scope = DICS_FLAG_CONFIGSPECIFIC;
            pcp.HwProfile = 0;
            break;
    }
    if(!SetupDiSetClassInstallParams(Devs,DevInfo,&pcp.ClassInstallHeader,sizeof(pcp)) ||
       !SetupDiCallClassInstaller(DIF_PROPERTYCHANGE,Devs,DevInfo)) {
        //
        // failed to invoke DIF_PROPERTYCHANGE
        //
        DumpDeviceWithInfo(Devs,DevInfo,pControlContext->strFail);
    } else {
        //
        // see if device needs reboot
        //
        devParams.cbSize = sizeof(devParams);
        if(SetupDiGetDeviceInstallParams(Devs,DevInfo,&devParams) && (devParams.Flags & (DI_NEEDRESTART|DI_NEEDREBOOT))) {
                DumpDeviceWithInfo(Devs,DevInfo,pControlContext->strReboot);
                pControlContext->reboot = TRUE;
        } else {
            //
            // appears to have succeeded
            //
            DumpDeviceWithInfo(Devs,DevInfo,pControlContext->strSuccess);
        }
        pControlContext->count++;
    }
    return EXIT_OK;
}
void DelMultiSz(LPTSTR* Array)
/*++
Routine Description:
    Deletes the string array allocated by GetDevMultiSz/GetRegMultiSz/GetMultiSzIndexArray
Arguments:
    Array - pointer returned by GetMultiSzIndexArray
Return Value:
    None
--*/
{
    if(Array) 
	{
        Array--;
        
		if(Array[0]) 
		{
            delete [] Array[0];
        }
        delete [] Array;
    }
}
int FindCallback(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Index, LPVOID Context)
/*++
Routine Description:
    Callback for use by Find/FindAll
    just simply display the device
Arguments:
    Devs    )_ uniquely identify the device
    DevInfo )
    Index    - index of device
    Context  - GenericContext
Return Value:
    EXIT_xxxx
--*/
{
    GenericContext *pFindContext = (GenericContext*)Context;
    if(!pFindContext->control) {
        DumpDevice(Devs, DevInfo);
        pFindContext->count++;
        return EXIT_OK;
    }
    if(!DumpDeviceWithInfo(Devs, DevInfo, NULL)) {
        return EXIT_OK;
    }
    if(pFindContext->control&FIND_DEVICE) {
        DumpDeviceDescr(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_CLASS) {
        DumpDeviceClass(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_STATUS) {
        DumpDeviceStatus(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_RESOURCES) {
        DumpDeviceResources(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_DRIVERFILES) {
        DumpDeviceDriverFiles(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_STACK) {
        DumpDeviceStack(Devs, DevInfo);
    }
    if(pFindContext->control&FIND_HWIDS) {
        DumpDeviceHwIds(Devs, DevInfo);
    }
    if (pFindContext->control&FIND_DRIVERNODES) {
        DumpDeviceDriverNodes(Devs, DevInfo);
    }
    pFindContext->count++;
    return EXIT_OK;
}
BOOL FindCurrentDriver(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, PSP_DRVINFO_DATA DriverInfoData)
/*++
Routine Description:
    Find the driver that is associated with the current device
    We can do this either the quick way (available in WinXP)
    or the long way that works in Win2k.
Arguments:
    Devs    )_ uniquely identify device
    DevInfo )
Return Value:
    TRUE if we managed to determine and select current driver
--*/
{
    SP_DEVINSTALL_PARAMS deviceInstallParams;
    WCHAR SectionName[LINE_LEN];
    WCHAR DrvDescription[LINE_LEN];
    WCHAR MfgName[LINE_LEN];
    WCHAR ProviderName[LINE_LEN];
    HKEY hKey = NULL;
    DWORD RegDataLength;
    DWORD RegDataType;
    DWORD c;
    BOOL match = FALSE;
    long regerr;
    ZeroMemory(&deviceInstallParams, sizeof(deviceInstallParams));
    deviceInstallParams.cbSize = sizeof(SP_DEVINSTALL_PARAMS);
    if(!SetupDiGetDeviceInstallParams(Devs, DevInfo, &deviceInstallParams)) {
        return FALSE;
    }
#ifdef DI_FLAGSEX_INSTALLEDDRIVER
    //
    // Set the flags that tell SetupDiBuildDriverInfoList to just put the
    // currently installed driver node in the list, and that it should allow
    // excluded drivers. This flag introduced in WinXP.
    //
    deviceInstallParams.FlagsEx |= (DI_FLAGSEX_INSTALLEDDRIVER | DI_FLAGSEX_ALLOWEXCLUDEDDRVS);
    if(SetupDiSetDeviceInstallParams(Devs, DevInfo, &deviceInstallParams)) {
        //
        // we were able to specify this flag, so proceed the easy way
        // we should get a list of no more than 1 driver
        //
        if(!SetupDiBuildDriverInfoList(Devs, DevInfo, SPDIT_CLASSDRIVER)) {
            return FALSE;
        }
        if (!SetupDiEnumDriverInfo(Devs, DevInfo, SPDIT_CLASSDRIVER,
                                   0, DriverInfoData)) {
            return FALSE;
        }
        //
        // we've selected the current driver
        //
        return TRUE;
    }
    deviceInstallParams.FlagsEx &= ~(DI_FLAGSEX_INSTALLEDDRIVER | DI_FLAGSEX_ALLOWEXCLUDEDDRVS);
#endif
    //
    // The following method works in Win2k, but it's slow and painful.
    //
    // First, get driver key - if it doesn't exist, no driver
    //
    hKey = SetupDiOpenDevRegKey(Devs,
                                DevInfo,
                                DICS_FLAG_GLOBAL,
                                0,
                                DIREG_DRV,
                                KEY_READ
                               );
    if(hKey == INVALID_HANDLE_VALUE) {
        //
        // no such value exists, so there can't be an associated driver
        //
        RegCloseKey(hKey);
        return FALSE;
    }
    //
    // obtain path of INF - we'll do a search on this specific INF
    //
    RegDataLength = sizeof(deviceInstallParams.DriverPath); // bytes!!!
    regerr = RegQueryValueEx(hKey,
                             REGSTR_VAL_INFPATH,
                             NULL,
                             &RegDataType,
                             (PBYTE)deviceInstallParams.DriverPath,
                             &RegDataLength
                             );
    if((regerr != ERROR_SUCCESS) || (RegDataType != REG_SZ)) {
        //
        // no such value exists, so no associated driver
        //
        RegCloseKey(hKey);
        return FALSE;
    }
    //
    // obtain name of Provider to fill into DriverInfoData
    //
    RegDataLength = sizeof(ProviderName); // bytes!!!
    regerr = RegQueryValueEx(hKey,
                             REGSTR_VAL_PROVIDER_NAME,
                             NULL,
                             &RegDataType,
                             (PBYTE)ProviderName,
                             &RegDataLength
                             );
    if((regerr != ERROR_SUCCESS) || (RegDataType != REG_SZ)) {
        //
        // no such value exists, so we don't have a valid associated driver
        //
        RegCloseKey(hKey);
        return FALSE;
    }
    //
    // obtain name of section - for final verification
    //
    RegDataLength = sizeof(SectionName); // bytes!!!
    regerr = RegQueryValueEx(hKey,
                             REGSTR_VAL_INFSECTION,
                             NULL,
                             &RegDataType,
                             (PBYTE)SectionName,
                             &RegDataLength
                             );
    if((regerr != ERROR_SUCCESS) || (RegDataType != REG_SZ)) {
        //
        // no such value exists, so we don't have a valid associated driver
        //
        RegCloseKey(hKey);
        return FALSE;
    }
    //
    // driver description (need not be same as device description)
    // - for final verification
    //
    RegDataLength = sizeof(DrvDescription); // bytes!!!
    regerr = RegQueryValueEx(hKey,
                             REGSTR_VAL_DRVDESC,
                             NULL,
                             &RegDataType,
                             (PBYTE)DrvDescription,
                             &RegDataLength
                             );
    RegCloseKey(hKey);
    if((regerr != ERROR_SUCCESS) || (RegDataType != REG_SZ)) {
        //
        // no such value exists, so we don't have a valid associated driver
        //
        return FALSE;
    }
    //
    // Manufacturer (via SPDRP_MFG, don't access registry directly!)
    //
    if(!SetupDiGetDeviceRegistryProperty(Devs,
                                        DevInfo,
                                        SPDRP_MFG,
                                        NULL,      // datatype is guaranteed to always be REG_SZ.
                                        (PBYTE)MfgName,
                                        sizeof(MfgName), // bytes!!!
                                        NULL)) {
        //
        // no such value exists, so we don't have a valid associated driver
        //
        return FALSE;
    }
    //
    // now search for drivers listed in the INF
    //
    //
    deviceInstallParams.Flags |= DI_ENUMSINGLEINF;
    deviceInstallParams.FlagsEx |= DI_FLAGSEX_ALLOWEXCLUDEDDRVS;
    if(!SetupDiSetDeviceInstallParams(Devs, DevInfo, &deviceInstallParams)) {
        return FALSE;
    }
    if(!SetupDiBuildDriverInfoList(Devs, DevInfo, SPDIT_CLASSDRIVER)) {
        return FALSE;
    }
    //
    // find the entry in the INF that was used to install the driver for
    // this device
    //
    for(c=0;SetupDiEnumDriverInfo(Devs,DevInfo,SPDIT_CLASSDRIVER,c,DriverInfoData);c++) {
        if((_tcscmp(DriverInfoData->MfgName,MfgName)==0)
            &&(_tcscmp(DriverInfoData->ProviderName,ProviderName)==0)) {
            //
            // these two fields match, try more detailed info
            // to ensure we have the exact driver entry used
            //
            SP_DRVINFO_DETAIL_DATA detail;
            detail.cbSize = sizeof(SP_DRVINFO_DETAIL_DATA);
            if(!SetupDiGetDriverInfoDetail(Devs,DevInfo,DriverInfoData,&detail,sizeof(detail),NULL)
                    && (GetLastError() != ERROR_INSUFFICIENT_BUFFER)) {
                continue;
            }
            if((_tcscmp(detail.SectionName,SectionName)==0) &&
                (_tcscmp(detail.DrvDescription,DrvDescription)==0)) {
                match = TRUE;
                break;
            }
        }
    }
    if(!match) {
        SetupDiDestroyDriverInfoList(Devs,DevInfo,SPDIT_CLASSDRIVER);
    }
    return match;
}
LPTSTR* GetDevMultiSz(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Prop)
/*++
Routine Description:
	Get a multi-sz device property
	and return as an array of strings
Arguments:
	Devs    - HDEVINFO containing DevInfo
	DevInfo - Specific device
	Prop    - SPDRP_HARDWAREID or SPDRP_COMPATIBLEIDS
Return Value:
	array of strings. last entry+1 of array contains NULL
	returns NULL on failure
--*/
{
	LPTSTR buffer;
	DWORD size;
	DWORD reqSize;
	DWORD dataType;
	LPTSTR * array;
	DWORD szChars;
	size = 8192; // initial guess, nothing magic about this
	buffer = new TCHAR[(size/sizeof(TCHAR))+2];
	if(!buffer) {
		return NULL;
	}
	while(!SetupDiGetDeviceRegistryProperty(Devs,DevInfo,Prop,&dataType,(LPBYTE)buffer,size,&reqSize)) {
		if(GetLastError() != ERROR_INSUFFICIENT_BUFFER) {
			goto failed;
		}
		if(dataType != REG_MULTI_SZ) {
			goto failed;
		}
		size = reqSize;
		delete [] buffer;
		buffer = new TCHAR[(size/sizeof(TCHAR))+2];
		if(!buffer) {
			goto failed;
		}
	}
	szChars = reqSize/sizeof(TCHAR);
	buffer[szChars] = TEXT('\0');
	buffer[szChars+1] = TEXT('\0');
	array = GetMultiSzIndexArray(buffer);
	if(array) {
		return array;
	}
failed:
	if(buffer) {
		delete [] buffer;
	}
	return NULL;
}
LPTSTR* GetMultiSzIndexArray(LPTSTR MultiSz)
/*++
Routine Description:
	Get an index array pointing to the MultiSz passed in
Arguments:
	MultiSz - well formed multi-sz string
Return Value:
	array of strings. last entry+1 of array contains NULL
	returns NULL on failure
--*/
{
	LPTSTR scan;
	LPTSTR* array;
	int elements;
	for(scan = MultiSz, elements = 0; scan[0] ;elements++) 
	{
		scan += lstrlen(scan)+1;
	}
	array = new LPTSTR[elements+2];
	if(!array) 
	{
		return NULL;
	}
	array[0] = MultiSz;
	array++;
	if(elements) 
	{
		for(scan = MultiSz, elements = 0; scan[0]; elements++) 
		{
			array[elements] = scan;
			scan += lstrlen(scan)+1;
		}
	}
	array[elements] = NULL;
	return array;
}
LPTSTR* GetRegMultiSz(HKEY hKey, LPCTSTR Val)
/*++
Routine Description:
    Get a multi-sz from registry
    and return as an array of strings
Arguments:
    hKey    - Registry Key
    Val     - Value to query
Return Value:
    array of strings. last entry+1 of array contains NULL
    returns NULL on failure
--*/
{
    LPTSTR buffer;
    DWORD size;
    DWORD reqSize;
    DWORD dataType;
    LPTSTR* array;
    DWORD szChars;
    LONG regErr;
    size = 8192; // initial guess, nothing magic about this
    buffer = new TCHAR[(size/sizeof(TCHAR))+2];
    if(!buffer) 
	{
        return NULL;
    }
    reqSize = size;
    while((regErr = RegQueryValueEx(hKey,Val,NULL,&dataType,(PBYTE)buffer,&reqSize) != NO_ERROR)) 
	{
        if(GetLastError() != ERROR_MORE_DATA) 
		{
            goto failed;
        }
        if(dataType != REG_MULTI_SZ) 
		{
            goto failed;
        }
        size = reqSize;
        delete [] buffer;
        buffer = new TCHAR[(size/sizeof(TCHAR))+2];
        if(!buffer) 
		{
            goto failed;
        }
    }
    szChars = reqSize/sizeof(TCHAR);
    buffer[szChars] = TEXT('\0');
    buffer[szChars+1] = TEXT('\0');
    array = GetMultiSzIndexArray(buffer);
    if(array) 
	{
        return array;
    }
failed:
    if(buffer) 
	{
        delete [] buffer;
    }
    return NULL;
}
LPTSTR GetDeviceDescription(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo)
/*++
Routine Description:
    Return a string containing a description of the device, otherwise NULL
    Always try friendly name first
Arguments:
    Devs    )_ uniquely identify device
    DevInfo )
Return Value:
    string containing description
--*/
{
    LPTSTR desc;
    desc = GetDeviceStringProperty(Devs, DevInfo, SPDRP_FRIENDLYNAME);
    if(!desc) 
	{
        desc = GetDeviceStringProperty(Devs, DevInfo, SPDRP_DEVICEDESC);
    }
    return desc;
}
LPTSTR GetDeviceStringProperty(HDEVINFO Devs, PSP_DEVINFO_DATA DevInfo, DWORD Prop)
/*++
Routine Description:
    Return a string property for a device, otherwise NULL
Arguments:
    Devs    )_ uniquely identify device
    DevInfo )
    Prop     - string property to obtain
Return Value:
    string containing description
--*/
{
    LPTSTR buffer;
    DWORD size;
    DWORD reqSize;
    DWORD dataType;
    DWORD szChars;
    size = 1024; // initial guess
    buffer = new TCHAR[(size/sizeof(TCHAR))+1];
    if(!buffer) {
        return NULL;
    }
    while(!SetupDiGetDeviceRegistryProperty(Devs,DevInfo,Prop,&dataType,(LPBYTE)buffer,size,&reqSize)) {
        if(GetLastError() != ERROR_INSUFFICIENT_BUFFER) {
            goto failed;
        }
        if(dataType != REG_SZ) {
            goto failed;
        }
        size = reqSize;
        delete [] buffer;
        buffer = new TCHAR[(size/sizeof(TCHAR))+1];
        if(!buffer) {
            goto failed;
        }
    }
    szChars = reqSize/sizeof(TCHAR);
    buffer[szChars] = TEXT('\0');
    return buffer;
failed:
    if(buffer) {
        delete [] buffer;
    }
    return NULL;
}
IdEntry GetIdType(LPCTSTR Id)
/*++
Routine Description:
    Determine if this is instance id or hardware id and if there's any wildcards
    instance ID is prefixed by '@'
    wildcards are '*'
Arguments:
    Id - ptr to string to check
Return Value:
    IdEntry
--*/
{
    IdEntry Entry;
    Entry.InstanceId = FALSE;
    Entry.Wild = NULL;
    Entry.String = Id;
    if(Entry.String[0] == INSTANCEID_PREFIX_CHAR) {
        Entry.InstanceId = TRUE;
        Entry.String = CharNext(Entry.String);
    }
    if(Entry.String[0] == QUOTE_PREFIX_CHAR) {
        //
        // prefix to treat rest of string literally
        //
        Entry.String = CharNext(Entry.String);
    } else {
        //
        // see if any wild characters exist
        //
        Entry.Wild = _tcschr(Entry.String,WILD_CHAR);
    }
    return Entry;
}
BOOL WildCardMatch(LPCTSTR Item, const IdEntry & MatchEntry)
/*++
Routine Description:
    Compare a single item against wildcard
    I'm sure there's better ways of implementing this
    Other than a command-line management tools
    it's a bad idea to use wildcards as it implies
    assumptions about the hardware/instance ID
    eg, it might be tempting to enumerate root\* to
    find all root devices, however there is a CfgMgr
    API to query status and determine if a device is
    root enumerated, which doesn't rely on implementation
    details.
Arguments:
    Item - item to find match for eg a\abcd\c
    MatchEntry - eg *\*bc*\*
Return Value:
    TRUE if any match, otherwise FALSE
--*/
{
    LPCTSTR scanItem;
    LPCTSTR wildMark;
    LPCTSTR nextWild;
    size_t matchlen;
    //
    // before attempting anything else
    // try and compare everything up to first wild
    //
    if(!MatchEntry.Wild) {
        return _tcsicmp(Item,MatchEntry.String) ? FALSE : TRUE;
    }
    if(_tcsnicmp(Item,MatchEntry.String,MatchEntry.Wild-MatchEntry.String) != 0) {
        return FALSE;
    }
    wildMark = MatchEntry.Wild;
    scanItem = Item + (MatchEntry.Wild-MatchEntry.String);
    for(;wildMark[0];) {
        //
        // if we get here, we're either at or past a wildcard
        //
        if(wildMark[0] == WILD_CHAR) {
            //
            // so skip wild chars
            //
            wildMark = CharNext(wildMark);
            continue;
        }
        //
        // find next wild-card
        //
        nextWild = _tcschr(wildMark,WILD_CHAR);
        if(nextWild) {
            //
            // substring
            //
            matchlen = nextWild-wildMark;
        } else {
            //
            // last portion of match
            //
            size_t scanlen = lstrlen(scanItem);
            matchlen = lstrlen(wildMark);
            if(scanlen < matchlen) {
                return FALSE;
            }
            return _tcsicmp(scanItem+scanlen-matchlen,wildMark) ? FALSE : TRUE;
        }
        if(_istalpha(wildMark[0])) {
            //
            // scan for either lower or uppercase version of first character
            //
            TCHAR u = _totupper(wildMark[0]);
            TCHAR l = _totlower(wildMark[0]);
            while(scanItem[0] && scanItem[0]!=u && scanItem[0]!=l) {
                scanItem = CharNext(scanItem);
            }
            if(!scanItem[0]) {
                //
                // ran out of string
                //
                return FALSE;
            }
        } else {
            //
            // scan for first character (no case)
            //
            scanItem = _tcschr(scanItem,wildMark[0]);
            if(!scanItem) {
                //
                // ran out of string
                //
                return FALSE;
            }
        }
        //
        // try and match the sub-string at wildMark against scanItem
        //
        if(_tcsnicmp(scanItem,wildMark,matchlen)!=0) {
            //
            // nope, try again
            //
            scanItem = CharNext(scanItem);
            continue;
        }
        //
        // substring matched
        //
        scanItem += matchlen;
        wildMark += matchlen;
    }
    return (wildMark[0] ? FALSE : TRUE);
}
BOOL WildCompareHwIds(LPTSTR* Array, const IdEntry & MatchEntry)
/*++
Routine Description:
    Compares all strings in Array against Id
    Use WildCardMatch to do real compare
Arguments:
    Array - pointer returned by GetDevMultiSz
    MatchEntry - string to compare against
Return Value:
    TRUE if any match, otherwise FALSE
--*/
{
    if(Array) 
	{
        while(Array[0]) 
		{
            if(WildCardMatch(Array[0],MatchEntry)) 
			{
                return TRUE;
            }
            Array++;
        }
    }
    return FALSE;
}
int EnumerateDevices(LPCTSTR BaseName, LPCTSTR Machine, DWORD Flags, int argc, LPTSTR argv[], CallbackFunc Callback, LPVOID Context)
/*++
Routine Description:
    Generic enumerator for devices that will be passed the following arguments:
    <id> [<id>...]
    =<class> [<id>...]
    where <id> can either be @instance-id, or hardware-id and may contain wildcards
    <class> is a class name
Arguments:
    BaseName - name of executable
    Machine  - name of machine to enumerate
    Flags    - extra enumeration flags (eg DIGCF_PRESENT)
    argc/argv - remaining arguments on command line
    Callback - function to call for each hit
    Context  - data to pass function for each hit
Return Value:
    EXIT_xxxx
--*/
{
    HDEVINFO devs = INVALID_HANDLE_VALUE;
    IdEntry * templ = NULL;
    int failcode = EXIT_FAIL;
    int retcode;
    int argIndex;
    DWORD devIndex;
    SP_DEVINFO_DATA devInfo;
    SP_DEVINFO_LIST_DETAIL_DATA devInfoListDetail;
    BOOL doSearch = FALSE;
    BOOL match;
    BOOL all = FALSE;
    GUID cls;
    DWORD numClass = 0;
    int skip = 0;
    UNREFERENCED_PARAMETER(BaseName);
	if(!argc) 
	{
        return EXIT_USAGE;
    }
    templ = new IdEntry[argc];
    if(!templ) 
	{
        goto final;
    }
    // determine if a class is specified
    if(argc > skip && argv[skip][0] == CLASS_PREFIX_CHAR && argv[skip][1]) 
	{
        if(!SetupDiClassGuidsFromNameEx(argv[skip]+1, &cls, 1, &numClass, Machine, NULL) && GetLastError() != ERROR_INSUFFICIENT_BUFFER) 
		{
            goto final;
        }
        if(!numClass) 
		{
            failcode = EXIT_OK;
            goto final;
        }
        skip++;
    }
    if(argc > skip && argv[skip][0] == WILD_CHAR && !argv[skip][1]) 
	{
        // catch convinient case of specifying a single argument '*'
        all = TRUE;
        skip++;
    } 
	else if(argc <= skip) 
	{
        // at least one parameter, but no <id>'s
        all = TRUE;
    }
    // determine if any instance id's were specified
    // note, if =<class> was specified with no id's
    // we'll mark it as not doSearch
    // but will go ahead and add them all
    for(argIndex = skip; argIndex < argc; argIndex++) 
	{
        templ[argIndex] = GetIdType(argv[argIndex]);
        if(templ[argIndex].Wild || !templ[argIndex].InstanceId) 
		{
            // anything other than simple InstanceId's require a search
            doSearch = TRUE;
        }
    }
    if(doSearch || all) 
	{
        // add all id's to list
        // if there's a class, filter on specified class
        devs = SetupDiGetClassDevsEx(numClass ? &cls : NULL,
                                     NULL,
                                     NULL,
                                     (numClass ? 0 : DIGCF_ALLCLASSES) | Flags,
                                     NULL,
                                     Machine,
                                     NULL);
    } 
	else 
	{
        // blank list, we'll add instance id's by hand
        devs = SetupDiCreateDeviceInfoListEx(numClass ? &cls : NULL,
                                             NULL,
                                             Machine,
                                             NULL);
    }
    if(devs == INVALID_HANDLE_VALUE) 
	{
        goto final;
    }
    for(argIndex = skip; argIndex < argc; argIndex++) 
	{
        // add explicit instances to list (even if enumerated all,
        // this gets around DIGCF_PRESENT)
        // do this even if wildcards appear to be detected since they
        // might actually be part of the instance ID of a non-present device
        if(templ[argIndex].InstanceId) 
		{
            SetupDiOpenDeviceInfo(devs, templ[argIndex].String, NULL, 0, NULL);
        }
    }
    devInfoListDetail.cbSize = sizeof(devInfoListDetail);
    if(!SetupDiGetDeviceInfoListDetail(devs, &devInfoListDetail)) 
	{
        goto final;
    }
    // now enumerate them
    if(all) 
	{
        doSearch = FALSE;
    }
    devInfo.cbSize = sizeof(devInfo);
    for(devIndex = 0; SetupDiEnumDeviceInfo(devs, devIndex, &devInfo); devIndex++) 
	{
        if(doSearch) 
		{
            for(argIndex = skip, match = FALSE; (argIndex < argc) && !match; argIndex++) 
			{
                TCHAR devID[MAX_DEVICE_ID_LEN];
                LPTSTR *hwIds = NULL;
                LPTSTR *compatIds = NULL;
				// determine instance ID
                if(CM_Get_Device_ID_Ex(devInfo.DevInst, devID, MAX_DEVICE_ID_LEN, 0, devInfoListDetail.RemoteMachineHandle) != CR_SUCCESS) 
				{
                    devID[0] = TEXT('\0');
                }
                if(templ[argIndex].InstanceId) 
				{
                    // match on the instance ID
                    if(WildCardMatch(devID, templ[argIndex])) 
					{
                        match = TRUE;
                    }
                } 
				else 
				{
                    // determine hardware ID's and search for matches
                    hwIds = GetDevMultiSz(devs, &devInfo, SPDRP_HARDWAREID);
                    compatIds = GetDevMultiSz(devs, &devInfo, SPDRP_COMPATIBLEIDS);
                    if(WildCompareHwIds(hwIds, templ[argIndex]) || WildCompareHwIds(compatIds, templ[argIndex])) 
					{
                        match = TRUE;
                    }
                }
                DelMultiSz(hwIds);
                DelMultiSz(compatIds);
            }
        } else 
		{
            match = TRUE;
        }
        if(match) 
		{
            retcode = Callback(devs, &devInfo, devIndex, Context);
            if(retcode) 
			{
                failcode = retcode;
                goto final;
            }
        }
    }
    failcode = EXIT_OK;
final:
    if(templ) 
	{
        delete [] templ;
    }
    if(devs != INVALID_HANDLE_VALUE) 
	{
        SetupDiDestroyDeviceInfoList(devs);
    }
    return failcode;
}

// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once
#include <windows.h>
#include "..\13d95898fe0f574d2583d7b08236e857\detours.h"

static LPTSTR TRAMPOLINE_FILESYSTEMPATH_HOOKDLL = TEXT("lib.msft.win.sys.proc.trampoline.dll");
static LPTSTR TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL = TEXT("lib.msft.win.sys.proc.trampoline.reporter.dll");
static LPTSTR TRAMPOLINE_NAMED_PIPE_PATH = TEXT("\\\\.\\Pipe\\Ludus"); 

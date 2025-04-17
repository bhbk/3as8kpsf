#ifndef __STDAFX__
#define __STDAFX__
/*******************************************************/
#pragma once
#include <windows.h>
#include <stdio.h>
#include <tchar.h>

/* Exclude rarely-used stuff from Windows headers. */
#define WIN32_LEAN_AND_MEAN

static FILE * stream;
static LONG s_nTlsThread = -1;
static LONG s_nThreadCnt = 0;
static HMODULE s_hInst = NULL;
static WCHAR s_wzDllPath[MAX_PATH];

/*******************************************************/
#endif
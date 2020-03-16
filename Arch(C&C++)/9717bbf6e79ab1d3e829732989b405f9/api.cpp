// Many of our asserts are constants.
#pragma warning(disable:4127)

#include <windows.h>
#include <stdio.h>
#include "API.h"
#include "Print.h"
#include "stdafx.h"
#include "..\13d95898fe0f574d2583d7b08236e857\detours.h"

/******************** Trampolines. **********************/
/*
BOOL __stdcall Mine_CreateProcessA(LPCSTR a0,
                                LPSTR a1,
                                LPSECURITY_ATTRIBUTES a2,
                                LPSECURITY_ATTRIBUTES a3,
                                BOOL a4,
								DWORD a5,
								LPVOID a6,
								LPCSTR a7,
								LPSTARTUPINFOA a8,
								LPROCESS_INFORMATION a9)
{
    BOOL rv = 0;
    __try {
        rv = Real_CreateProcessA(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9);
    } __finally {
//		_PrintExit("CreateProcessA(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_CreateProcessW(LPCWSTR a0,
                                LPWSTR a1,
                                LPSECURITY_ATTRIBUTES a2,
                                LPSECURITY_ATTRIBUTES a3,
                                BOOL a4,
								DWORD a5,
								LPVOID a6,
								LPCWSTR a7,
								LPSTARTUPINFOA a8,
								LPROCESS_INFORMATION a9)
{
    BOOL rv = 0;
    __try {
        rv = Real_CreateProcessW(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9);
    } __finally {
//		_PrintExit("CreateProcessW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
*/
BOOL __stdcall Mine_DrawTextA(HDC a0,
                                LPCSTR a1,
                                int a2,
                                RECT* a3,
                                UINT a4)
{
	/* Verify the string isn't zero in size & isn't bigger than the buffer size. */
	if(lstrlenA(a1) > 0 && lstrlenA(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|DrawTextA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|DrawTextA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a3->left,
				a3->top,
				a3->right,
				a3->bottom,
				a1);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_DrawTextA(a0, a1, a2, a3, a4);
    } __finally {
//		_PrintExit("DrawTextA(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_DrawTextW(HDC a0,
                                LPWSTR a1,
                                int a2,
                                RECT* a3,
                                UINT a4)
{
	/* Verify there is even a string being output. */
	if(lstrlenW(a1) > 0 && lstrlenW(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|DrawTextW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|DrawTextW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a3->left,
				a3->top,
				a3->right,
				a3->bottom,
				a1);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_DrawTextW(a0, a1, a2, a3, a4);
    } __finally {
//		_PrintExit("DrawTextW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_DrawTextExA(HDC a0,
                                LPSTR a1,
                                int a2,
                                RECT* a3,
                                UINT a4,
								LPDRAWTEXTPARAMS a5)
{
	/* Verify there is even a string being output. */
	if(lstrlenA(a1) > 0 && lstrlenA(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|DrawTextExA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|DrawTextExA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a3->left,
				a3->top,
				a3->right,
				a3->bottom,
				a1);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_DrawTextExA(a0, a1, a2, a3, a4, a5);
    } __finally {
//		_PrintExit("DrawTextExA(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_DrawTextExW(HDC a0,
                                LPWSTR a1,
                                int a2,
                                RECT* a3,
                                UINT a4,
								LPDRAWTEXTPARAMS a5)
{
	/* Verify there is even a string being output. */
	if(lstrlenW(a1) > 0 && lstrlenW(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|DrawTextExW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|DrawTextExW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a3->left,
				a3->top,
				a3->right,
				a3->bottom,
				a1);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_DrawTextExW(a0, a1, a2, a3, a4, a5);
    } __finally {
//		_PrintExit("DrawTextExW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_ExtTextOutA(HDC a0,
                                int a1,
                                int a2,
                                UINT a3,
                                CONST RECT* a4,
                                LPCSTR a5,
                                UINT a6,
                                CONST INT* a7)
{
	if(lstrlenA(a5) > 0 && lstrlenA(a5) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|ExtTextOutA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a5);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|ExtTextOutA|%d:%d:0:0|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a5);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_ExtTextOutA(a0, a1, a2, a3, a4, a5, a6, a7);
    } __finally {
//		_PrintExit("ExtTextOutA(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_ExtTextOutW(HDC a0,
                                int a1,
                                int a2,
                                UINT a3,
                                CONST RECT* a4,
                                LPCWSTR a5,
                                UINT a6,
                                CONST INT* a7)
{
	if(lstrlenW(a5) > 0 && lstrlenW(a5) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|ExtTextOutA|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a5);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|ExtTextOutA|%d:%d:0:0|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a5);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_ExtTextOutW(a0, a1, a2, a3, a4, a5, a6, a7);
    } __finally {
//		_PrintExit("ExtTextOutW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_PolyTextOutA(HDC a0, 
								 const POLYTEXTA* a1, 
								 int a2)
{
	if(lstrlenA(a1->lpstr) > 0 && lstrlenA(a1->lpstr) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|PolyTextOutA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1->lpstr);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|PolyTextOutA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1->rcl.left,
				a1->rcl.top,
				a1->rcl.right,
				a1->rcl.bottom,
				a1->lpstr);
		}
	}

	BOOL rv = 0;

    __try {
        rv = Real_PolyTextOutA(a0, a1, a2);
    } __finally {
//		_PrintExit("PolyTextOutA(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_PolyTextOutW(HDC a0, 
								 const POLYTEXTW* a1, 
								 int a2)
{
	if(lstrlenW(a1->lpstr) > 0 && lstrlenW(a1->lpstr) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|PolyTextOutW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a1->lpstr);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|PolyTextOutW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1->rcl.left,
				a1->rcl.top,
				a1->rcl.right,
				a1->rcl.bottom,
				a1->lpstr);
		}
	}

	BOOL rv = 0;

    __try {
        rv = Real_PolyTextOutW(a0, a1, a2);
    } __finally {
//		_PrintExit("PolyTextOutW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_SetWindowTextA(HWND a0,
                                LPCSTR a1)
{
	/* Verify the string isn't zero in size & isn't bigger than the buffer size. */
	if(lstrlenA(a1) > 0 && lstrlenA(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = a0;

		WINDOWINFO p, c;
		GetWindowInfo(hwnd, &p);
		GetWindowInfo(chwnd, &c);

		_PrintEnter("REPORT|%d|%d|%d|SetWindowTextA|%d:%d:%d:%d|%hs", 
			GetCurrentProcessId(),
			hwnd,
			chwnd,
			c.rcClient.left -= p.rcWindow.left,
			c.rcClient.top -= p.rcWindow.top,
			c.rcClient.right -= p.rcWindow.left,
			c.rcClient.bottom -= p.rcWindow.top,
			a1);
	}

    BOOL rv = 0;
    __try {
        rv = Real_SetWindowTextA(a0, a1);
    } __finally {
//		_PrintExit("SetWindowTextA(,,,,,,,) -> %p", rv);
    };
    return rv;
}

BOOL __stdcall Mine_SetWindowTextW(HWND a0,
                                LPWSTR a1)
{
	/* Verify there is even a string being output. */
	if(lstrlenW(a1) > 0 && lstrlenW(a1) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = a0;

		WINDOWINFO p, c;
		GetWindowInfo(hwnd, &p);
		GetWindowInfo(chwnd, &c);

		_PrintEnter("REPORT|%d|%d|%d|SetWindowTextW|%d:%d:%d:%d|%ls", 
			GetCurrentProcessId(),
			hwnd,
			chwnd,
			c.rcClient.left -= p.rcWindow.left,
			c.rcClient.top -= p.rcWindow.top,
			c.rcClient.right -= p.rcWindow.left,
			c.rcClient.bottom -= p.rcWindow.top,
			a1);
	}

    BOOL rv = 0;
    __try {
        rv = Real_SetWindowTextW(a0, a1);
    } __finally {
//		_PrintExit("SetWindowTextW(,,,,,,,) -> %p", rv);
    };
    return rv;
}
LONG __stdcall Mine_TabbedTextOutA(HDC a0,
								 int a1,
								 int a2,
								 LPCSTR a3,
								 int a4,
								 int a5,
								 const INT* a6,
								 int a7)
{
	if(lstrlenA(a3) > 0 && lstrlenA(a3) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|TabbedTextOutA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a3);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|TabbedTextOutA|%d:%d:0:0|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a3);
		}
	}

    LONG rv = 0;
    __try {
        rv = Real_TabbedTextOutA(a0, a1, a2, a3, a4, a5, a6, a7);
    } __finally {
//		_PrintExit("TabbedTextOutA(,,,,) -> %p", rv);
    };
    return rv;
}
LONG __stdcall Mine_TabbedTextOutW(HDC a0,
								 int a1,
								 int a2,
								 LPCWSTR a3,
								 int a4,
								 int a5,
								 const INT* a6,
								 int a7)
{
	if(lstrlenW(a3) > 0 && lstrlenW(a3) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|TabbedTextOutW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a3);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|TabbedTextOutW|%d:%d:0:0|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a3);
		}
	}

    LONG rv = 0;
    __try {
        rv = Real_TabbedTextOutW(a0, a1, a2, a3, a4, a5, a6, a7);
    } __finally {
//		_PrintExit("TabbedTextOutW(,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_TextOutA(HDC a0,
                             int a1,
                             int a2,
                             LPCSTR a3,
                             int a4)
{
	if(lstrlenA(a3) > 0 && lstrlenA(a3) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|TextOutA|%d:%d:%d:%d|%hs", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a3);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|TextOutA|%d:%d:0:0|%hs", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a3);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_TextOutA(a0, a1, a2, a3, a4);
    } __finally {
//		_PrintExit("TextOutA(,,,,) -> %p", rv);
    };
    return rv;
}
BOOL __stdcall Mine_TextOutW(HDC a0,
                             int a1,
                             int a2,
                             LPCWSTR a3,
                             int a4)
{
	if(lstrlenW(a3) > 0 && lstrlenW(a3) < PIPE_BUFFERSIZE && GetActiveWindow() != 0)
	{
		HWND hwnd = GetActiveWindow();
		HWND chwnd = WindowFromDC(a0);

		if(chwnd != 0)
		{
			WINDOWINFO p, c;
			GetWindowInfo(hwnd, &p);
			GetWindowInfo(chwnd, &c);

			_PrintEnter("REPORT|%d|%d|%d|TextOutW|%d:%d:%d:%d|%ls", 
				GetCurrentProcessId(),
				hwnd,
				chwnd,
				c.rcClient.left -= p.rcWindow.left,
				c.rcClient.top -= p.rcWindow.top,
				c.rcClient.right -= p.rcWindow.left,
				c.rcClient.bottom -= p.rcWindow.top,
				a3);
		}
		else
		{
			_PrintEnter("REPORT|%d|%d|%d|TextOutW|%d:%d:0:0|%ls", 
				GetCurrentProcessId(),
				hwnd,
				0,
				a1, 
				a2, 
				a3);
		}
	}

    BOOL rv = 0;
    __try {
        rv = Real_TextOutW(a0, a1, a2, a3, a4);
    } __finally {
//		_PrintExit("TextOutW(,,,,) -> %p", rv);
    };
    return rv;
}
/***************** Attach detours. ********************/
PCHAR DetRealName(PCHAR psz)
{
    PCHAR pszBeg = psz;
    /* Move to end of name. */
    while (*psz) {
        psz++;
    }
    /* Move back through A-Za-z0-9 names. */
    while (psz > pszBeg &&
           ((psz[-1] >= 'A' && psz[-1] <= 'Z') ||
            (psz[-1] >= 'a' && psz[-1] <= 'z') ||
            (psz[-1] >= '0' && psz[-1] <= '9'))) {
        psz--;
    }
    return psz;
}
VOID DetAttach(PVOID *ppbReal, PVOID pbMine, PCHAR psz)
{
    LONG l = DetourAttach(ppbReal, pbMine);
    if (l != 0) {
        _Print("INFO| ATTACH FAILED:`%s': ERROR %d", DetRealName(psz), l);
    }
}
VOID DetDetach(PVOID *ppbReal, PVOID pbMine, PCHAR psz)
{
    LONG l = DetourDetach(ppbReal, pbMine);
    if (l != 0) {
        _Print("INFO| DETACH FAILED:`%s': ERROR %d", DetRealName(psz), l);
    }
}
LONG AttachDetours(VOID)
{
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());

//	ATTACH(&(PVOID&)Real_CreateProcessA, Mine_CreateProcessA);
//	ATTACH(&(PVOID&)Real_CreateProcessW, Mine_CreateProcessW);
	ATTACH(&(PVOID&)Real_DrawTextA, Mine_DrawTextA);
	ATTACH(&(PVOID&)Real_DrawTextW, Mine_DrawTextW);
	ATTACH(&(PVOID&)Real_DrawTextExA, Mine_DrawTextExA);
	ATTACH(&(PVOID&)Real_DrawTextExW, Mine_DrawTextExW);
	ATTACH(&(PVOID&)Real_ExtTextOutA, Mine_ExtTextOutA);
	ATTACH(&(PVOID&)Real_ExtTextOutW, Mine_ExtTextOutW);
	ATTACH(&(PVOID&)Real_PolyTextOutA, Mine_PolyTextOutA);
	ATTACH(&(PVOID&)Real_PolyTextOutW, Mine_PolyTextOutW);
	ATTACH(&(PVOID&)Real_SetWindowTextA, Mine_SetWindowTextA);
	ATTACH(&(PVOID&)Real_SetWindowTextW, Mine_SetWindowTextW);
	ATTACH(&(PVOID&)Real_TabbedTextOutA, Mine_TabbedTextOutA);
	ATTACH(&(PVOID&)Real_TabbedTextOutW, Mine_TabbedTextOutW);
	ATTACH(&(PVOID&)Real_TextOutA, Mine_TextOutA);
	ATTACH(&(PVOID&)Real_TextOutW, Mine_TextOutW);

    return DetourTransactionCommit();
}
LONG DetachDetours(VOID)
{
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());

//	DETACH(&(PVOID&)Real_CreateProcessA, Mine_CreateProcessA);
//	DETACH(&(PVOID&)Real_CreateProcessW, Mine_CreateProcessW);
	DETACH(&(PVOID&)Real_DrawTextA, Mine_DrawTextA);
	DETACH(&(PVOID&)Real_DrawTextW, Mine_DrawTextW);
	DETACH(&(PVOID&)Real_DrawTextExA, Mine_DrawTextExA);
	DETACH(&(PVOID&)Real_DrawTextExW, Mine_DrawTextExW);
	DETACH(&(PVOID&)Real_ExtTextOutA, Mine_ExtTextOutA);
	DETACH(&(PVOID&)Real_ExtTextOutW, Mine_ExtTextOutW);
	DETACH(&(PVOID&)Real_PolyTextOutA, Mine_PolyTextOutA);
	DETACH(&(PVOID&)Real_PolyTextOutW, Mine_PolyTextOutW);
	DETACH(&(PVOID&)Real_SetWindowTextA, Mine_SetWindowTextA);
	DETACH(&(PVOID&)Real_SetWindowTextW, Mine_SetWindowTextW);
	DETACH(&(PVOID&)Real_TabbedTextOutA, Mine_TabbedTextOutA);
	DETACH(&(PVOID&)Real_TabbedTextOutW, Mine_TabbedTextOutW);
	DETACH(&(PVOID&)Real_TextOutA, Mine_TextOutA);
	DETACH(&(PVOID&)Real_TextOutW, Mine_TextOutW);

    return DetourTransactionCommit();
}
VOID AssertMessage(CONST PCHAR pszMsg, CONST PCHAR pszFile, ULONG nLine)
{
    _Print("ASSERT(%s) failed in %s, line %d.", pszMsg, pszFile, nLine);
}
VOID NullExport()
{
}
PIMAGE_NT_HEADERS NtHeadersForInstance(HINSTANCE hInst)
{
    PIMAGE_DOS_HEADER pDosHeader = (PIMAGE_DOS_HEADER)hInst;
    __try {
        if (pDosHeader->e_magic != IMAGE_DOS_SIGNATURE) {
            SetLastError(ERROR_BAD_EXE_FORMAT);
            return NULL;
        }

        PIMAGE_NT_HEADERS pNtHeader = (PIMAGE_NT_HEADERS)((PBYTE)pDosHeader + pDosHeader->e_lfanew);
        if (pNtHeader->Signature != IMAGE_NT_SIGNATURE) {
            SetLastError(ERROR_INVALID_EXE_SIGNATURE);
            return NULL;
        }
        if (pNtHeader->FileHeader.SizeOfOptionalHeader == 0) {
            SetLastError(ERROR_EXE_MARKED_INVALID);
            return NULL;
        }
        return pNtHeader;
    } __except(EXCEPTION_EXECUTE_HANDLER) {
    }
    SetLastError(ERROR_EXE_MARKED_INVALID);

    return NULL;
}
BOOL InstanceEnumerate(HINSTANCE hInst)
{
    WCHAR wzDllName[MAX_PATH];

    PIMAGE_NT_HEADERS pinh = NtHeadersForInstance(hInst);
    if (pinh && Real_GetModuleFileNameW(hInst, wzDllName, ARRAYOF(wzDllName))) {
        _Print("INFO| %08lx: %-43.43ls %08x", hInst, wzDllName, pinh->OptionalHeader.CheckSum);
        return TRUE;
    }
    return FALSE;
}
BOOL ProcessEnumerate()
{
    PBYTE pbNext;
    for (PBYTE pbRegion = (PBYTE)0x10000;; pbRegion = pbNext) {
        MEMORY_BASIC_INFORMATION mbi;
        ZeroMemory(&mbi, sizeof(mbi));

        if (VirtualQuery((PVOID)pbRegion, &mbi, sizeof(mbi)) <= 0) {
            break;
        }
        pbNext = (PBYTE)mbi.BaseAddress + mbi.RegionSize;

        /* Skip free regions, reserver regions, and guard pages. */
        if (mbi.State == MEM_FREE || mbi.State == MEM_RESERVE) {
            continue;
        }
        if (mbi.Protect & PAGE_GUARD || mbi.Protect & PAGE_NOCACHE) {
            continue;
        }
        if (mbi.Protect == PAGE_NOACCESS) {
            continue;
        }

        /* Skip over regions from the same allocation... */
        {
            MEMORY_BASIC_INFORMATION mbiStep;

            while (VirtualQuery((PVOID)pbNext, &mbiStep, sizeof(mbiStep)) > 0) {
                if ((PBYTE)mbiStep.AllocationBase != pbRegion) {
                    break;
                }
                pbNext = (PBYTE)mbiStep.BaseAddress + mbiStep.RegionSize;
                mbi.Protect |= mbiStep.Protect;
            }
        }

        WCHAR wzDllName[MAX_PATH];
        PIMAGE_NT_HEADERS pinh = NtHeadersForInstance((HINSTANCE)pbRegion);

        if (pinh &&
            Real_GetModuleFileNameW((HINSTANCE)pbRegion,wzDllName,ARRAYOF(wzDllName))) {

			_Print("INFO| %p..%p: %ls", pbRegion, pbNext, wzDllName);
        }
        else {
			_Print("INFO| %p..%p: State=%04x, Protect=%08x", pbRegion, pbNext, mbi.State, mbi.Protect);
        }
    }

    LPVOID lpvEnv = Real_GetEnvironmentStrings();
	_Print("INFO| Env=%08x [%08x %08x]", lpvEnv, ((PVOID*)lpvEnv)[0], ((PVOID*)lpvEnv)[1]);

    return TRUE;
}
/******************* DLL module information. ***********************/
BOOL ThreadAttach(HMODULE hDll)
{
    (void)hDll;

    if (s_nTlsThread >= 0) {
        LONG nThread = InterlockedIncrement(&s_nThreadCnt);
        TlsSetValue(s_nTlsThread, (PVOID)(LONG_PTR)nThread);
    }

	_Print("INFO| ATTACHING THREAD */");

    return TRUE;
}
BOOL ThreadDetach(HMODULE hDll)
{
    (void)hDll;

    if (s_nTlsThread >= 0) {
        TlsSetValue(s_nTlsThread, (PVOID)0);
    }

	_Print("INFO| DETACHING THREAD */");

	return TRUE;
}
BOOL ProcessAttach(HMODULE hDll)
{
    s_nTlsThread = TlsAlloc();
    ThreadAttach(hDll);

    WCHAR wzExeName[MAX_PATH];

    s_hInst = hDll;
    Real_GetModuleFileNameW(hDll, s_wzDllPath, ARRAYOF(s_wzDllPath));
    Real_GetModuleFileNameW(NULL, wzExeName, ARRAYOF(wzExeName));

    ProcessEnumerate();

    LONG error = AttachDetours();

    if (error != NO_ERROR) {
        _Print("INFO| ATTACHING DETOURS ERROR: %d */", error);
	} else {
		_Print("INFO| ATTACHING DETOURS SUCCESSFULL */");
	}

    return TRUE;
}
BOOL ProcessDetach(HMODULE hDll)
{
    ThreadDetach(hDll);

    LONG error = DetachDetours();
    if (error != NO_ERROR) {
        _Print("INFO| DETACHING DETOURS ERROR: %d */", error);
	} else {
		_Print("INFO| DETACHING DETOURS SUCCESSFULL */");
	}

    if (s_nTlsThread >= 0) {
        TlsFree(s_nTlsThread);
    }

	return TRUE;
}
BOOL APIENTRY DllMain(HINSTANCE hModule, DWORD dwReason, PVOID lpReserved)
{
    (void)hModule;
    (void)lpReserved;

    switch (dwReason) {
      case DLL_PROCESS_ATTACH:
        DetourRestoreAfterWith();
        return ProcessAttach(hModule);
      case DLL_PROCESS_DETACH:
        return ProcessDetach(hModule);
      case DLL_THREAD_ATTACH:
        return ThreadAttach(hModule);
      case DLL_THREAD_DETACH:
        return ThreadDetach(hModule);
    }
    return TRUE;
}
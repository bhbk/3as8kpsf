#ifndef __API__
#define __API__
/*******************************************************/

#define ATTACH(x,y)	DetAttach(x,y,#x)
#define DETACH(x,y)	DetDetach(x,y,#x)
#define _WIN32_WINNT	0x0400
#define NT
#define DBG_TRACE	0
#define UNUSED(c)	(c) = (c)
#define ARRAYOF(x)	(sizeof(x)/sizeof(x[0]))
#define ASSERT_ALWAYS(x)   \
    do {												\
		if (!(x)) {                                     \
			    AssertMessage(#x, __FILE__, __LINE__);  \
				DebugBreak();                           \
		}                                               \
    } while (0)

#ifndef NDEBUG
#define ASSERT(x)	ASSERT_ALWAYS(x)
#else
#define ASSERT(x)
#endif
/*******************************************************/
#define PIPE_BUFFERSIZE 4095
RECT* aRect = new RECT;
RECT* hRect = new RECT;
RECT* pRect = new RECT;

HWND GetHWNDFromHDC(HDC hdc);
RECT* GetRECTFromHDC(HDC hdc);
RECT* GetParentRECTFromHDC(HDC hdc);
BOOL ProcessEnumerate();
BOOL InstanceEnumerate(HINSTANCE hInst);
BOOL ImportEnumerate(HINSTANCE hInst);
VOID AssertMessage(CONST PCHAR pszMsg, CONST PCHAR pszFile, ULONG nLine);

/*******************************************************/
BOOL (__stdcall * Real_DrawTextA)(HDC a0, 
									LPCSTR a1,
									int a2,
									RECT* a3,
									UINT a4) = DrawTextA;
BOOL (__stdcall * Real_DrawTextW)(HDC a0, 
									LPCWSTR a1,
									int a2,
									RECT* a3,
									UINT a4) = DrawTextW;
BOOL (__stdcall * Real_DrawTextExA)(HDC a0, 
									LPSTR a1,
									int a2,
									RECT* a3,
									UINT a4,
									LPDRAWTEXTPARAMS a5) = DrawTextExA;
BOOL (__stdcall * Real_DrawTextExW)(HDC a0, 
									LPWSTR a1,
									int a2,
									RECT* a3,
									UINT a4,
									LPDRAWTEXTPARAMS a5) = DrawTextExW;
BOOL (__stdcall * Real_ExtTextOutA)(HDC a0, 
									int a1, 
									int a2, 
									UINT a3, 
									CONST RECT* a4, 
									LPCSTR a5, 
									UINT a6, 
									CONST INT* a7) = ExtTextOutA;
BOOL (__stdcall * Real_ExtTextOutW)(HDC a0, 
									int a1, 
									int a2, 
									UINT a3, 
									CONST RECT* a4, 
									LPCWSTR a5, 
									UINT a6, 
									CONST INT* a7) = ExtTextOutW;
LPSTR (__stdcall * Real_GetEnvironmentStrings)(void) = GetEnvironmentStrings;
LPWSTR (__stdcall * Real_GetEnvironmentStringsW)(void) = GetEnvironmentStringsW;
DWORD (__stdcall * Real_GetModuleFileNameW)(HMODULE a0, 
										 LPWSTR a1,
										 DWORD a2) = GetModuleFileNameW;
HMODULE (__stdcall * Real_GetModuleHandleW)(LPCWSTR a0) = GetModuleHandleW;
BOOL (__stdcall * Real_PolyTextOutA)(HDC a0, 
									 const POLYTEXTA* a1, 
									 int a2) = PolyTextOutA;
BOOL (__stdcall * Real_PolyTextOutW)(HDC a0, 
									 const POLYTEXTW* a1, 
									 int a2) = PolyTextOutW;

BOOL (__stdcall * Real_SetWindowTextA)(HWND a0, 
								 LPCSTR a1) = SetWindowTextA;
BOOL (__stdcall * Real_SetWindowTextW)(HWND a0, 
								 LPCWSTR a1) = SetWindowTextW;


LONG (__stdcall * Real_TabbedTextOutA)(HDC a0, 
									 int a1, 
									 int a2, 
									 LPCSTR a3,
									 int a4,
									 int a5,
									 const INT* a6,
									 int a7) = TabbedTextOutA;
LONG (__stdcall * Real_TabbedTextOutW)(HDC a0, 
									 int a1, 
									 int a2, 
									 LPCWSTR a3,
									 int a4,
									 int a5,
									 const INT* a6,
									 int a7) = TabbedTextOutW;
BOOL (__stdcall * Real_TextOutA)(HDC a0, 
								 int a1, 
								 int a2, 
								 LPCSTR a3, 
								 int a4) = TextOutA;
BOOL (__stdcall * Real_TextOutW)(HDC a0, 
								 int a1, 
								 int a2, 
								 LPCWSTR a3, 
								 int a4) = TextOutW;

/*******************************************************/
#endif
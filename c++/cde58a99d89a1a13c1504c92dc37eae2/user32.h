#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;

namespace lib {

	namespace msft {

		namespace dotnet {

			namespace interop {

				namespace user32 {

					delegate BOOL EnumChildWndDelegate(System::IntPtr lpEnumFunc, System::IntPtr lParam);
					delegate BOOL EnumDesktopWndDelegate(System::IntPtr lpEnumFunc, System::IntPtr lParam);
					delegate BOOL EnumThreadWndDelegate(System::IntPtr lpEnumFunc, System::IntPtr lParam);
					delegate BOOL EnumTopLvlWndDelegate(System::IntPtr lpEnumFunc, System::IntPtr lParam);
				
					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL DestroyIcon(System::IntPtr hHandle);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL DestroyWindow(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)] 
					extern "C" BOOL EnumWindows(EnumTopLvlWndDelegate^ lpEnumFunc, System::IntPtr lParam); 

					[DllImport("user32.dll", SetLastError = true)] 
					extern "C" BOOL EnumChildWindows(System::IntPtr hWnd, EnumChildWndDelegate^ lpEnumFunc, System::IntPtr lParam); 

					[DllImport("user32.dll", SetLastError = true)] 
					extern "C" BOOL EnumDesktopWindows(EnumDesktopWndDelegate^ lpEnumFunc, System::IntPtr lParam); 

					[DllImport("user32.dll", SetLastError = true)] 
					extern "C" BOOL EnumThreadWindows(System::UInt32 dwThreadId, EnumThreadWndDelegate^ lpEnumFunc, System::IntPtr lParam); 

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" UINT FindWindow(System::String^ lpClassName, System::String^ lpWindowName);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" HDC GetDC(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)] 
					extern "C" INT GetClassName(System::IntPtr hWnd, System::Text::StringBuilder^ className, INT nMaxCount);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" HDC GetWindowDC(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL GetWindowInfo(System::IntPtr hWnd, WINDOWINFO &pWi);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL GetWindowRect(System::IntPtr hWnd, RECT &lpRect);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL GetWindowText(System::IntPtr hWnd, System::Text::StringBuilder^ lpString, INT nMaxCount);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL GetWindowThreadProcessId(System::IntPtr hWnd, UINT &nMaxCount);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL IsIconic(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL IsWindow(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT IsWindowEnabled(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT IsWindowVisible(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL IsZoomed(System::IntPtr hWnd);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL PostMessage(System::IntPtr hWnd, UINT msg, INT lParam, System::IntPtr wParam);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL RegisterHotKey(System::IntPtr hWnd, INT id, UINT fsModifiers, INT vk);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT ReleaseDC(System::IntPtr hWnd, System::IntPtr hDc);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT SendMessage(System::IntPtr hWnd, INT msg, INT wParam, System::Text::StringBuilder^ lParam);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT SendMessageTimeout(System::IntPtr hWnd, INT msg, INT wParam, System::Text::StringBuilder^ lParam, INT fuFlags, INT uTimeout, PDWORD_PTR lpdwResult);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" INT SetWindowPos(System::IntPtr hWnd, System::IntPtr hWndAfter, System::Int32 x, System::Int32 y, System::Int32 cx, System::Int32 cy, System::UInt32 uFlags);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL ShowWindow(System::IntPtr hWnd, INT nCmdShow);

					[DllImport("user32.dll", SetLastError = true)]
					extern "C" BOOL UnregisterHotKey(System::IntPtr hWnd, INT id);
				}
			}
		}
	}
}

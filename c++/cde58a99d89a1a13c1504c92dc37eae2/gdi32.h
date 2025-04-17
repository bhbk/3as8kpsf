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

				namespace gdi32 {

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" BOOL BitBlt(System::IntPtr hDCDest, System::Int32 nXDest, System::Int32 nYDest, System::Int32 nWidth, System::Int32 nHeight, System::IntPtr hDCSrc, System::Int32 nXSrc, System::Int32 nYSrc, System::Int32 dwRop);

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" HDC CreateCompatibleDC(System::IntPtr hDc);

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" HBITMAP CreateCompatibleBitmap(System::IntPtr hDc, System::Int32 nWidth, System::Int32 nHeight);

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" BOOL DeleteDC(System::IntPtr hDc);

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" BOOL PatBlt(System::IntPtr hDC, System::Int32 nXLeft, System::Int32 nYLeft, System::Int32 nWidth, System::Int32 nHeight, System::Int32 dwRop);

					[DllImport("gdi32.dll", SetLastError = true)]
					extern "C" HGDIOBJ SelectObject(System::IntPtr hDC, System::IntPtr hgdiobj);
				}
			}
		}
	}
}

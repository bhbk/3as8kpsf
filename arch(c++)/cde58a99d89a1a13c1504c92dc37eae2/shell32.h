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

				namespace shell32 {

					[DllImport("shell32.dll", SetLastError = true)]
					extern "C" UINT SHGetFileInfo(System::String^ pszPath, UINT dwFileAttributes, SHFILEINFO psfi, UINT cbSizeFileInfo, UINT uFlags);
				}
			}
		}
	}
}
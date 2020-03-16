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

				namespace psapi {

					[DllImport("psapi.dll", SetLastError = true)]
					extern "C" UINT GetModuleFileNameEx(System::IntPtr hProcess, System::IntPtr hModule, System::Text::StringBuilder^ lpFileName, INT nSize);
				}
			}
		}
	}
}
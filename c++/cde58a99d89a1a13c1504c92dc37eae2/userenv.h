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

				namespace userenv {

					[DllImport("userenv.dll", SetLastError = true)]
					extern "C" BOOL CreateEnvironmentBlock(LPVOID &lpEnvironment, HANDLE hToken, BOOL bInherit);
				}
			}
		}
	}
}

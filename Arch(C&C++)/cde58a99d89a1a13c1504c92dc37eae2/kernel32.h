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

				namespace kernel32 {

					[DllImport("kernel32.dll", SetLastError = true)]
					extern "C" BOOL CloseHandle(System::IntPtr hHandle);

					[DllImport("kernel32.dll", SetLastError = true)]
					extern "C" BOOL DuplicateTokenEx(HANDLE hExistingToken, DWORD dwDesiredAccess, LPSECURITY_ATTRIBUTES lpTokenAttributes, SECURITY_IMPERSONATION_LEVEL impersonationLevel, TOKEN_TYPE tokenType, HANDLE &hToken);

					[DllImport("kernel32.dll", SetLastError = true)]
					extern "C" BOOL OpenProcess(UINT dwDesiredAccess, BOOL bInheritHandle, INT dwProcessId);

					[DllImport("kernel32.dll", SetLastError = true)]
					extern "C" INT WTSGetActiveConsoleSessionId();

					[DllImport("kernel32.dll", SetLastError = true)]
					extern "C" BOOL WTSQueryUserToken(ULONG sessionId, HANDLE &phToken);
				}
			}
		}
	}
}

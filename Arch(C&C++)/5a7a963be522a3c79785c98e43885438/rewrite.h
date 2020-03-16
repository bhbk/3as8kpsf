#pragma once
#include "stdafx.h"
#include "rewrite_native.h"
#include "..\cde58a99d89a1a13c1504c92dc37eae2\kernel32.h"
#include "..\cde58a99d89a1a13c1504c92dc37eae2\userenv.h"

#define arrayof(x) (sizeof(x)/sizeof(x[0]))
#define LOG_ServiceTrampoline_POSTLOADPEREWRITE 1032
#define LOG_ServiceTrampoline_POSTLOADPEREWRITEASUSER 1034
#define LOG_ServiceTrampoline_PRELOADPEREWRITE 1030
#define LOG_ServiceTrampoline_PRELOADPEREWRITEASUSER 1031

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Text;

namespace lib {

	namespace msft {

		namespace win {

			namespace proc {

				namespace trampoline {

					namespace pe {
						/// <summary>
						/// Summary for rewrite
						/// </summary>
						public ref class rewrite :  public System::ComponentModel::Component
						{
						public:
							rewrite(void)
							{
								InitializeComponent();
								//
								//TODO: Add the constructor code here
								//
							}
							rewrite(System::ComponentModel::IContainer ^container)
							{
								/// <summary>
								/// Required for Windows.Forms Class Composition Designer support
								/// </summary>

								container->Add(this);
								InitializeComponent();
							}
							static BOOL PreLoad(String^ pbTargetExecutable, String^ pbTargetExecutableArgs)
							{
								LPTSTR pbTargetExe = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToCoTaskMemAnsi(pbTargetExecutable).ToPointer();
								LPTSTR pbTargetExeArgs = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToCoTaskMemAnsi(pbTargetExecutableArgs).ToPointer();
								STARTUPINFO si;
								PROCESS_INFORMATION pi;

								StringBuilder^ msg = gcnew StringBuilder(String::Empty);
								
								if (!DoesDllExportOrdinal1(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL)) {
									msg->Append(Environment::NewLine 
										+ L"DoesDllExportOrdinal1(" + gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed. Does not export function with ordinal #1.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ZeroMemory(&si, sizeof(si));
								ZeroMemory(&pi, sizeof(pi));

								// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2581371&SiteID=1 The executable is started within the environment of the service, thus 
								// in the (invisible) desktop of your service. In order to start the application in an interactive desktop you have one option: Set the 'lpDesktop' member
								// of the 'STARTUPINFO' structure accordingly (go with 'winsta0\default' for the default one).
								si.cb = sizeof(si);
								si.lpDesktop = TEXT("winsta0\\default");

								if(!DetourCreateProcessWithDll(pbTargetExe,
																pbTargetExeArgs,
																NULL, 
																NULL, 
																TRUE, 
																NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT, 
																NULL, 
																NULL,
																&si, 
																&pi, 
																TRAMPOLINE_FILESYSTEMPATH_HOOKDLL, 
																TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL, 
																NULL)) 
								{
									msg->Append(Environment::NewLine + L"DetourCreateProcessWithDll(" 
										+ gcnew String(pbTargetExe) + ", "
										+ gcnew String(pbTargetExeArgs) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ResumeThread(pi.hThread);

								DWORD dwResult = 0;

								if (!GetExitCodeProcess(pi.hProcess, &dwResult)) 
								{
									msg->Append(Environment::NewLine + L"GetExitCodeProcess(" + gcnew String(pbTargetExe) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								msg->Append(Environment::NewLine + L"DetourCreateProcessWithDll(" 
									+ gcnew String(pbTargetExe) + ", "
									+ gcnew String(pbTargetExeArgs) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") succeeded.");

								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

								return TRUE;
							}
							static BOOL PreLoad(LPTSTR pbTargetExecutable, LPTSTR pbTargetExecutableArgs)
							{
								STARTUPINFO si;
								PROCESS_INFORMATION pi;

								StringBuilder^ msg = gcnew StringBuilder(String::Empty);

								if (!DoesDllExportOrdinal1(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL)) {
									msg->Append(Environment::NewLine 
										+ L"DoesDllExportOrdinal1(" + gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed. Does not export function with ordinal #1.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ZeroMemory(&si, sizeof(si));
								ZeroMemory(&pi, sizeof(pi));

								// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2581371&SiteID=1 The executable is started within the environment of the service, thus 
								// in the (invisible) desktop of your service. In order to start the application in an interactive desktop you have one option: Set the 'lpDesktop' member
								// of the 'STARTUPINFO' structure accordingly (go with 'winsta0\default' for the default one).
								si.cb = sizeof(si);
								si.lpDesktop = TEXT("winsta0\\default");

								if(!DetourCreateProcessWithDll(pbTargetExecutable,
																pbTargetExecutableArgs,
																NULL, 
																NULL, 
																TRUE, 
																NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT, 
																NULL, 
																NULL,
																&si, 
																&pi, 
																TRAMPOLINE_FILESYSTEMPATH_HOOKDLL, 
																TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL, 
																NULL)) 
								{
									msg->Append(Environment::NewLine + L"DetourCreateProcessWithDll(" 
										+ gcnew String(pbTargetExecutable) + ", "
										+ gcnew String(pbTargetExecutableArgs) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ResumeThread(pi.hThread);

								DWORD dwResult = 0;

								if (!GetExitCodeProcess(pi.hProcess, &dwResult)) 
								{
									msg->Append(Environment::NewLine + L"GetExitCodeProcess(" + gcnew String(pbTargetExecutable) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								msg->Append(Environment::NewLine + L"DetourCreateProcessWithDll(" 
									+ gcnew String(pbTargetExecutable) + ", "
									+ gcnew String(pbTargetExecutableArgs) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") succeeded.");

								// Write event to log.
								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

								return TRUE;
							}
							// http://social.msdn.microsoft.com/forums/en-US/windowssecurity/thread/31bfa13d-982b-4b1a-bff3-2761ade5214f/
							// http://social.msdn.microsoft.com/Forums/en-US/windowssecurity/thread/1a6705e8-510e-411d-8c6e-e916a292f2e1
							static BOOL PreLoadAsUser(LPTSTR pbTargetExecutable, LPTSTR pbTargetExecutableArgs)
							{
								STARTUPINFO si;
								PROCESS_INFORMATION pi;
								LPVOID hEnvironment = NULL;
								HANDLE hToken = NULL;
								HANDLE hTokenDup = NULL;
								DWORD dwSessionId = NULL;

								dwSessionId = lib::msft::dotnet::interop::kernel32::WTSGetActiveConsoleSessionId();
								lib::msft::dotnet::interop::kernel32::WTSQueryUserToken(dwSessionId, hToken);
								lib::msft::dotnet::interop::kernel32::DuplicateTokenEx(hToken, MAXIMUM_ALLOWED, NULL,	SecurityIdentification, TokenPrimary, hTokenDup);
								lib::msft::dotnet::interop::userenv::CreateEnvironmentBlock(hEnvironment, hTokenDup, FALSE);

								StringBuilder^ msg = gcnew StringBuilder(String::Empty);

								if (!DoesDllExportOrdinal1(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL)) {
									msg->Append(Environment::NewLine + L"DoesDllExportOrdinal1(" 
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed. Does not export function with ordinal #1.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ZeroMemory(&si, sizeof(si));
								ZeroMemory(&pi, sizeof(pi));

								// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2581371&SiteID=1 The executable is started within the environment of the service, thus 
								// in the (invisible) desktop of your service. In order to start the application in an interactive desktop you have one option: Set the 'lpDesktop' member
								// of the 'STARTUPINFO' structure accordingly (go with 'winsta0\default' for the default one).
								si.cb = sizeof(si);
								si.lpDesktop = TEXT("winsta0\\default");

								if(!DetourCreateProcessWithDllAsUser(hTokenDup,
																pbTargetExecutable,
																pbTargetExecutableArgs,
																NULL, 
																NULL, 
																TRUE, 
																NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT, 
																hEnvironment, 
																NULL,
																&si, 
																&pi, 
																TRAMPOLINE_FILESYSTEMPATH_HOOKDLL, 
																TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL, 
																NULL)) 
								{
									msg->Append(Environment::NewLine + L"DetourCreateProcessWithDllAsUser(" 
										+ gcnew String("CONSOLE_USER") + ", "
										+ gcnew String(pbTargetExecutable) + ", "
										+ gcnew String(pbTargetExecutableArgs) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
										+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								ResumeThread(pi.hThread);

								DWORD dwResult = 0;

								if (!GetExitCodeProcess(pi.hProcess, &dwResult)) {
									msg->Append(Environment::NewLine 
										+ L"GetExitCodeProcess("+ gcnew String(pbTargetExecutable) + ") failed.");

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								msg->Append(Environment::NewLine + L"DetourCreateProcessWithDllAsUser(" 
									+ gcnew String(pbTargetExecutable) + ", "
									+ gcnew String(pbTargetExecutableArgs) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_HOOKDLL) + ", "
									+ gcnew String(TRAMPOLINE_FILESYSTEMPATH_REPORTERDLL) + ") succeeded.");

								// Write event to log.
								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

								return TRUE;
							}

						protected:
							/// <summary>
							/// Clean up any resources being used.
							/// </summary>
							~rewrite()
							{
								if (components)
								{
									delete components;
								}
							}

						private:
							static BOOL DoesDllExportOrdinal1(LPCSTR pszDllPath)
							{
								HMODULE hDll = LoadLibraryEx(pszDllPath, NULL, DONT_RESOLVE_DLL_REFERENCES);

								if (hDll == NULL) {
									StringBuilder^ msg = gcnew StringBuilder(String::Empty);

									Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
										System::Reflection::MethodBase::GetCurrentMethod()->ToString(), gcnew Exception(msg->ToString()));

									return FALSE;
								}

								BOOL validFlag = FALSE;

								DetourEnumerateExports(hDll, &validFlag, ExportCallback);
							    
								FreeLibrary(hDll);

								return validFlag;
							}
							/// <summary>
							/// Required designer variable.
							/// </summary>
							System::ComponentModel::Container ^components;

					#pragma region Windows Form Designer generated code
							/// <summary>
							/// Required method for Designer support - do not modify
							/// the contents of this method with the code editor.
							/// </summary>
							void InitializeComponent(void)
							{
								components = gcnew System::ComponentModel::Container();
							}
					#pragma endregion
						};
					}
				}
			}
		}
	}
}

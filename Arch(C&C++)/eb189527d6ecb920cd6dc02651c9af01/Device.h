#pragma once
#include "Stdafx.h"
#include "DevconCmd.h"
using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Text;
using namespace System::Text::RegularExpressions;
namespace lib {
namespace msft {
	namespace win {
		namespace sys {
		namespace dev_mgr {
			/// <summary>
			/// Summary for Device
			/// </summary>
			public ref class Device :  public System::ComponentModel::Component
			{
			public:
				Device(void)
				{
					InitializeComponent();
					//TODO: Add the constructor code here
				}
				Device(System::ComponentModel::IContainer ^container)
				{
					/// <summary>
					/// Required for Windows.Forms Class Composition Designer support
					/// </summary>
					container->Add(this);
					InitializeComponent();
				}
				static int Disable(String^ pnpid)
				{
					try
					{
						TCHAR* argv[3];
//						argv[0] = TEXT("lib.msft.win.sys.dev_mgr.dll");
						System::String^ self = gcnew String(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name + ".dll");
						argv[0] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(self).ToPointer();
						argv[1] = TEXT("disable");
						argv[2] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(pnpid).ToPointer();
						int r = CmdDeviceDisable(NULL, NULL, length(argv), argv);
						return r;
					}
					catch(Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
				static bool Enable(String^ pnpid)
				{
					try
					{
						TCHAR* argv[3];
//						argv[0] = System::Reflection::Assembly::GetExecutingAssembly()->Location;
						System::String^ self = gcnew String(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name + ".dll");
						argv[0] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(self).ToPointer();
						argv[1] = TEXT("enable");
						argv[2] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(pnpid).ToPointer();
						int r = CmdDeviceEnable(NULL, NULL, length(argv), argv);
						return true;
					}
					catch(Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
				static bool Status(String^ pnpid)
				{
					try
					{
						TCHAR* argv[3];
//						argv[0] = System::Reflection::Assembly::GetExecutingAssembly()->Location;
						System::String^ self = gcnew String(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name + ".dll");
						argv[0] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(self).ToPointer();
						argv[1] = TEXT("status");
						argv[2] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(pnpid).ToPointer();
						int r = CmdDeviceStatus(NULL, NULL, length(argv), argv);
						return true;
					}
					catch(Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
				static bool UpdatePnpDeviceId(String^ type, String^ pnpid, String^ %dev)
				{
					try
					{
						TCHAR* argv[3];
						LPTSTR* rslts[16];
						dev = "|";
						for(int i = 0; i < length(rslts); i++)
						{
							rslts[i] = (LPTSTR*)TEXT("EMPTY|EMPTY");
						}
						argv[0] = NULL;
						argv[1] = TEXT("listclass");
						argv[2] = (LPTSTR)System::Runtime::InteropServices::Marshal::StringToHGlobalAuto(type).ToPointer();
						CmdClassDeviceList(NULL, NULL, length(argv), argv, rslts);
						for(int i = 0; i < length(rslts); i++)
						{
							String^ input = gcnew String((const wchar_t *)rslts[i]);
							if(input->Contains(pnpid))
							{
								dev = input;
								break;
							}
						}
						return true;
					}
					catch(Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
				static bool FormatPnpDeviceId(String^ %pnpid)
				{
					try
					{
						Regex^ deviceMatch = gcnew Regex("ven_[0-9|a-z][0-9|a-z][0-9|a-z][0-9|a-z]&dev_[0-9|a-z][0-9|a-z][0-9|a-z][0-9|a-z]", RegexOptions::IgnoreCase);
						if(deviceMatch->IsMatch(pnpid))
						{
							if(pnpid->ToLower()->Contains("hdaudio"))
							{
								// Cut off everything we don't need, I think.
								pnpid = pnpid->Substring(0, 49);
								return true;
							}
							else if(pnpid->ToLower()->Contains("pci"))
							{
								// Cut off everything we don't need, I think.
								pnpid = pnpid->Substring(0, 37);
								return true;
							}
							else
							{
								return false;
							}
						}
						else
						{
							return false;
						}
					}
					catch (Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
				static bool ValidatePnpDeviceId(String^ %pnpid)
				{
					try
					{
						Regex^ deviceMatch = gcnew Regex("ven_[0-9|a-z][0-9|a-z][0-9|a-z][0-9|a-z]&dev_[0-9|a-z][0-9|a-z][0-9|a-z][0-9|a-z]", RegexOptions::IgnoreCase);
						if(deviceMatch->IsMatch(pnpid))
						{
							if(pnpid->ToLower()->Contains("hdaudio"))
							{
								return true;
							}
							else if(pnpid->ToLower()->Contains("pci"))
							{
								return true;
							}
							else
							{
								return false;
							}
						}
						else
						{
							return false;
						}
					}
					catch (Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
						return false;
					}
				}
			protected:
				/// <summary>
				/// Clean up any resources being used.
				/// </summary>
				~Device()
				{
					if (components)
					{
						delete components;
					}
				}
			private:
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
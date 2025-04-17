#pragma once
#include "Stdafx.h"
#include "DevconCmd.h"
using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Data;
using namespace System::Diagnostics;
using namespace System::Text;
using namespace System::Text::RegularExpressions;
namespace lib {
namespace msft {
	namespace win {
		namespace sys {
		namespace dev_mgr {
			/// <summary>
			/// Summary for Class
			/// </summary>
			public ref class Class :  public System::ComponentModel::Component
			{
			public:
				Class(void)
				{
					InitializeComponent();
					//TODO: Add the constructor code here
				}
				Class(System::ComponentModel::IContainer ^container)
				{
					/// <summary>
					/// Required for Windows.Forms Class Composition Designer support
					/// </summary>
					container->Add(this);
					InitializeComponent();
				}
				static ArrayList^ GetAllTypes(void)
				{
					TCHAR* argv[3];
	//				LPTSTR* rslts[32];
					
					argv[0] = NULL;
					argv[1] = TEXT("classes");
					argv[2] = NULL;
	//				CmdClasses(TEXT("lib.msft.win.sys.dev_mgr.dll"), NULL, length(argv), argv, rslts); 
					return gcnew ArrayList();
				}
				static bool GetByType(String ^type, DataTable^ %devs)
				{
					try
					{
						TCHAR* argv[3];
						LPTSTR* rslts[16];
						devs->Clear();
						devs->Columns->Clear();
						devs->Columns->Add("pnpdevid");
						devs->Columns->Add("desc");
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
							if(input->ToLower()->Contains("pci") || input->ToLower()->Contains("hdaudio"))
							{
								cli::array<System::String^, 1>^ data = input->Split('|');
								devs->Rows->Add(data[0], data[1]);
							}
						}
					}
					catch(Exception^ ex)
					{
						Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name, 
							System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);
					}
			
					return true;
				}
			protected:
				/// <summary>
				/// Clean up any resources being used.
				/// </summary>
				~Class()
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

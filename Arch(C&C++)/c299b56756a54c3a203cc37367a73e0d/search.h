#pragma once
#include "stdafx.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;

namespace lib {

	namespace msft {

		namespace win {

			namespace proc {
				/// <summary>
				/// Summary for search
				/// </summary>
				public ref class search :  public System::ComponentModel::Component
				{
				public:
					search(void)
					{
						InitializeComponent();
						//
						//TODO: Add the constructor code here
						//
					}
					search(System::ComponentModel::IContainer ^container)
					{
						/// <summary>
						/// Required for Windows.Forms Class Composition Designer support
						/// </summary>

						container->Add(this);
						InitializeComponent();
					}
					static BOOL ForIDByName(System::String^ pbTargetProcess, System::Collections::ArrayList^ &pResult)
					{
						// Declare needed structures for process enumeration.
						PROCESSENTRY32 p;
						HANDLE hProcessSnap = INVALID_HANDLE_VALUE;
						HANDLE hProcess = INVALID_HANDLE_VALUE;

						// Take process snapshot from system.
						hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

						// If no snapshot is taken... exit.
						if (hProcessSnap == INVALID_HANDLE_VALUE)
						{
							//Console.WriteLine("Unable to create process snapshot!");
							return(FALSE);
						}

						// Remember to initialize the PROCESSENTRY32 structure.
						p.dwSize = sizeof(PROCESSENTRY32);
						BOOL pRslt = Process32First(hProcessSnap, &p);

						// Iterate through the process list.
						while(pRslt != FALSE)
						{
							// Check if process module filename is...
							System::String^ pbExecutable = gcnew System::String(p.szExeFile);

							pbExecutable = pbExecutable->Trim()->ToLower();
							pbTargetProcess = pbTargetProcess->Trim()->ToLower();

							if (pbExecutable->Equals(pbTargetProcess)) 
							{
								pResult->Add(System::UInt32(p.th32ProcessID).ToString());
							}

							pRslt = Process32Next(hProcessSnap, &p);
						}

						// Close the snapshot handle to avoid memory leaks.
						CloseHandle(hProcessSnap);

						return(TRUE);
					}

				protected:
					/// <summary>
					/// Clean up any resources being used.
					/// </summary>
					~search()
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

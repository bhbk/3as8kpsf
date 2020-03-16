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

				namespace thread {
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
						static BOOL ByPid(System::UInt32 pid, System::Collections::ArrayList^ &tResult)
						{
							// Declare needed structures for thread enumeration.
							THREADENTRY32 t;
							HANDLE hThreadSnap;

							// Take thread snapshot from system.
							hThreadSnap = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);

							// If no snapshot is taken... exit.
							if (hThreadSnap == INVALID_HANDLE_VALUE)
							{
								return(FALSE);
							}

							// Remember to initialize the THREADENTRY32 structure.
							t.dwSize = sizeof(THREADENTRY32);
							BOOL tRslt = Thread32First(hThreadSnap, &t);

							// Iterate through the thread list.
							while(tRslt != FALSE)
							{
								// Found another thread.
								tRslt = Thread32Next(hThreadSnap, &t);

								if(pid.Equals(t.th32OwnerProcessID))
								{
									tResult->Add(System::UInt32(t.th32ThreadID).ToString());
								}
							}

							// Close the snapshot handle to avoid memory leaks.
							CloseHandle(hThreadSnap);

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
}

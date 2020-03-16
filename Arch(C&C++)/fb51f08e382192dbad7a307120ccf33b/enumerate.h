#pragma once
#include "..\cde58a99d89a1a13c1504c92dc37eae2\user32.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Text;

namespace lib {

	namespace msft {

		namespace win {

			namespace proc {

				namespace thread {

					/// <summary>
					/// Summary for enumerate
					/// </summary>
					public ref class enumerate :  public System::ComponentModel::Component
					{
					public:
						enumerate(void)
						{
							InitializeComponent();
							//
							//TODO: Add the constructor code here
							//
						}
						enumerate(System::ComponentModel::IContainer ^container)
						{
							/// <summary>
							/// Required for Windows.Forms Class Composition Designer support
							/// </summary>

							container->Add(this);
							InitializeComponent();
						}
						static BOOL TopLvlWnd(System::Collections::ArrayList^ &tlResult)
						{
							topLvlWnd = tlResult;

							lib::msft::dotnet::interop::user32::EnumTopLvlWndDelegate^ callback = gcnew lib::msft::dotnet::interop::user32::EnumTopLvlWndDelegate(EnumTopLvlWndProc);
							EnumWindows(callback, System::IntPtr::Zero);

							return(TRUE);
						}
						static BOOL ThreadWnd(System::UInt32 thread, System::Collections::ArrayList^ &wResult)
						{
							threadWnd = wResult;

							lib::msft::dotnet::interop::user32::EnumThreadWndDelegate^ callback = gcnew lib::msft::dotnet::interop::user32::EnumThreadWndDelegate(EnumThreadWndProc);
							EnumThreadWindows(thread, callback, System::IntPtr::Zero);

							return(TRUE);
						}
						static BOOL ChildWnd(System::IntPtr phwnd, System::Collections::Hashtable^ &cResult)
						{
							phWnd = phwnd;
							childWnd = cResult;

							lib::msft::dotnet::interop::user32::EnumChildWndDelegate^ callback = gcnew lib::msft::dotnet::interop::user32::EnumChildWndDelegate(EnumChildWndProc);
							EnumChildWindows(phWnd, callback, System::IntPtr::Zero);

							return(TRUE);
						}
					protected:
						/// <summary>
						/// Clean up any resources being used.
						/// </summary>
						~enumerate()
						{
							if (components)
							{
								delete components;
							}
						}

					private:
						static BOOL EnumChildWndProc(System::IntPtr chWnd, System::IntPtr lParam)
						{
							// http://msdn.microsoft.com/en-us/library/ms632610(VS.85).aspx
							try
							{
/*								WINDOWINFO c, p;
								GetWindowInfo(chWnd, c);
								GetWindowInfo(phWnd, p);

								// Subtract parent window coordinate (includes border) from child window coordinate to get relative location.
								c.rcClient.left -= p.rcWindow.left;
								c.rcClient.top -= p.rcWindow.top;
								c.rcClient.right -= p.rcWindow.left;
								c.rcClient.bottom -= p.rcWindow.top;

								// Send message to hWnd asking for caption length.
								INT length = SendMessage(chWnd, WM_GETTEXTLENGTH, 0, gcnew StringBuilder());

								// Instantiate a buffer large enough to hold text that will be returned.
								StringBuilder^ value = gcnew StringBuilder(length + 1);

								// Allocate buffer space & send message to hWnd to copy caption to buffer.
								INT results = SendMessage(chWnd, WM_GETTEXT, length + 1, value);

								// Assign relative coordinates for control within client window.
								teknistar::ludus::trampoline::Objects::XY^ xy = gcnew teknistar::ludus::trampoline::Objects::XY(c.rcClient.left, c.rcClient.top, c.rcClient.right, c.rcClient.bottom);
								teknistar::ludus::trampoline::Objects::Element^ element = gcnew teknistar::ludus::trampoline::Objects::Element(xy);

								// Assign handle for control of window.
								element->chwnd = chWnd;

								// Assign current displayed value for control in client window.
								element->value = value->ToString();

								// Clear out any chars that aren't worth printing out.
								teknistar::ludus::trampoline::Filter::NonPrintableASCII(element->value);

								// TEMPORARY, STILL GETTING HASHTABLE COLLISIONS!!!
								if(!childWnd->ContainsKey(element->xy->ToString()->GetHashCode()))
								{
									childWnd->Add(element->xy->ToString()->GetHashCode(), element);
								}*/
							}
							catch(System::Exception^ ex)
							{
								// Log the exception.
								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);

								// Return true so subsequent callbacks continue.
								return(TRUE);
							}

							return(TRUE);
						}
						static BOOL EnumThreadWndProc(System::IntPtr chWnd, System::IntPtr lParam)
						{
							try
							{
								StringBuilder^ wndClassName = gcnew StringBuilder(256 + 1);
								StringBuilder^ wndText = gcnew StringBuilder(256 + 1);
								INT wndClassRtrn;
								INT wndTextLength;
								INT wndTextRtrn;

								// Allocate for wide character count plus terminal null.
								wndClassRtrn = lib::msft::dotnet::interop::user32::GetClassName(chWnd, wndClassName, 256 + 1);

								// Send message to hWnd asking for caption length.
								wndTextLength = lib::msft::dotnet::interop::user32::SendMessage(chWnd, WM_GETTEXTLENGTH, 0, gcnew StringBuilder());

								// Allocate buffer space & send message to hWnd to copy caption to buffer.
								wndTextRtrn = lib::msft::dotnet::interop::user32::SendMessage(chWnd, WM_GETTEXT, (wndTextLength + 1), wndText);

								if (lib::msft::dotnet::interop::user32::IsWindowVisible(chWnd) != FALSE) 
								{
									threadWnd->Add(chWnd.ToString());
								}
							}
							catch(System::Exception^ ex)
							{
								// Log the exception.
								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);

								// Return true so subsequent callbacks continue.
								return(TRUE);
							}

							return(TRUE);
						}
						static BOOL EnumTopLvlWndProc(System::IntPtr chWnd, System::IntPtr lParam)
						{
							try
							{
								StringBuilder^ wndClassName;
								StringBuilder^ wndText;
								INT wndClassRtrn;
								INT wndTextLength;
								INT wndTextRtrn;

								wndClassName = gcnew StringBuilder(256 + 1);

								// Allocate for wide character count plus terminal null.
								wndClassRtrn = lib::msft::dotnet::interop::user32::GetClassName(chWnd, wndClassName, 256);

								// Send message to hWnd asking for caption length.
								wndTextLength = lib::msft::dotnet::interop::user32::SendMessage(chWnd, WM_GETTEXTLENGTH, 0, gcnew StringBuilder());

								wndText = gcnew StringBuilder(wndTextLength + 1);

								// Allocate buffer space & send message to hWnd to copy caption to buffer.
								wndTextRtrn = lib::msft::dotnet::interop::user32::SendMessage(chWnd, WM_GETTEXT, (wndTextLength + 1), wndText);

								if (lib::msft::dotnet::interop::user32::IsWindowVisible(chWnd) != FALSE) 
								{
									topLvlWnd->Add(chWnd.ToString() + L"|" + wndText->ToString());
								}
							}
							catch(System::Exception^ ex)
							{
								// Log the exception.
								Bhbk::Lib::Msft::Win::Sys::Log::application::write(System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Name,
									System::Reflection::MethodBase::GetCurrentMethod()->ToString(), ex);

								// Return true so subsequent callbacks continue.
								return(TRUE);
							}

							return(TRUE);
						}
						static System::Collections::ArrayList^ topLvlWnd;
						static System::Collections::ArrayList^ threadWnd;
						static System::Collections::Hashtable^ childWnd;
						static System::IntPtr phWnd;
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

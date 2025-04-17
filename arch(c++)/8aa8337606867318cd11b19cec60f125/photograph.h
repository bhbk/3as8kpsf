#pragma once
#include "stdafx.h"
#include "..\cde58a99d89a1a13c1504c92dc37eae2\gdi32.h"
#include "..\cde58a99d89a1a13c1504c92dc37eae2\user32.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Text;

namespace lib {

	namespace msft {

		namespace win {
			
			namespace gui {

				/// <summary>
				/// Summary for Photograph
				/// </summary>
				public ref class Photograph :  public System::ComponentModel::Component
				{
				public:
					Photograph(void)
					{
						InitializeComponent();
						//
						//TODO: Add the constructor code here
						//
					}
					Photograph(System::ComponentModel::IContainer ^container)
					{
						/// <summary>
						/// Required for Windows.Forms Class Composition Designer support
						/// </summary>

						container->Add(this);
						InitializeComponent();
					}
					static System::Drawing::Bitmap^ Rectangle(System::IntPtr hWnd)
					{
						// Get window dimensions.
						WINDOWINFO w;
						lib::msft::dotnet::interop::user32::GetWindowInfo(hWnd, w);

						return Rectangle(hWnd, w.rcWindow.left, w.rcWindow.top, w.rcWindow.right - w.rcWindow.left, w.rcWindow.bottom - w.rcWindow.top);
					}
					static System::Drawing::Bitmap^ Rectangle(System::IntPtr hWnd, RECT r)
					{
						return Rectangle(hWnd, r.left, r.top, r.right - r.left, r.bottom - r.top);
					}
					static System::Drawing::Bitmap^ Rectangle(System::IntPtr hWnd, System::Int32 x1, System::Int32 y1, System::Int32 x2, System::Int32 y2)
					{
						// http://support.microsoft.com/kb/147810
						try
						{
							// Get device context for the entire window, not just the client area.
							System::IntPtr hDCSrc = System::IntPtr(lib::msft::dotnet::interop::user32::GetWindowDC(hWnd));

							// Create compatibile context.
							System::IntPtr hDCDest = System::IntPtr(lib::msft::dotnet::interop::gdi32::CreateCompatibleDC(hDCSrc));

							// Create reference to bitmap.
							System::IntPtr hBitmap = System::IntPtr(lib::msft::dotnet::interop::gdi32::CreateCompatibleBitmap(hDCSrc, x2, y2));

							if (hBitmap.Equals(System::IntPtr::Zero))	
							{
								// Release context.
								lib::msft::dotnet::interop::user32::ReleaseDC(hWnd, hDCSrc);

								// Delete context;
								lib::msft::dotnet::interop::gdi32::DeleteDC(hDCDest);

								// Throw an exception that we catch and log.
								throw gcnew Exception();
							}
							else
							{
								// Select compatible bitmap in compatible context. 
								// Copy window context to compatible context.
								// Select previous bitmap back into compatible context.
								System::IntPtr hTmp = System::IntPtr(lib::msft::dotnet::interop::gdi32::SelectObject(hDCDest, hBitmap)); 

								// Transfer pixels from source to destination.
								lib::msft::dotnet::interop::gdi32::BitBlt(hDCDest, 0, 0, x2, y2, hDCSrc, x1, y1, SRCCOPY); 
					
								// Create GDI+ bitmap for context.
								System::Drawing::Bitmap^ b = System::Drawing::Image::FromHbitmap(hBitmap); 

								// Release context.
								lib::msft::dotnet::interop::user32::ReleaseDC(hWnd, hDCSrc);

								// Delete context;
								lib::msft::dotnet::interop::gdi32::DeleteDC(hDCDest);

								return b;
							}
						}
						catch(Exception^ ex)
						{
							// Log the exception.
							StringBuilder^ evtEntry = gcnew StringBuilder(L"Console::Agent::Photograph::Rectangle()" + Environment::NewLine);
							EventLog^ evtLog = gcnew EventLog(L"Application", L".", L"Ludus SnapShot");
							evtEntry->Append(Environment::NewLine + String::Format("Exception: {0}. Stack trace: {1}.", ex->Message, ex->StackTrace));
			//				evtLog->WriteEntry(evtEntry->ToString(), EventLogEntryType::Error, log->photographrectangle);
							evtLog->Close();

							return nullptr;
						}
					}
					static System::UInt64 PixelHashCode(System::Drawing::Bitmap^ b)
					{
						System::UInt64 h = 0;

						try
						{
							for(int y = 0; y < b->Height; y++)
							{
								for(int x = 0; x < b->Width; x++)
								{
									h += b->GetPixel(x, y).ToArgb();
								}
							}

							return h;
						}
						catch(Exception^ ex)
						{
							// Log the exception.
							StringBuilder^ evtEntry = gcnew StringBuilder(L"Console::Agent::Photograph::PixelHashCode()" + Environment::NewLine);
							EventLog^ evtLog = gcnew EventLog(L"Application", L".", L"Ludus SnapShot");
							evtEntry->Append(Environment::NewLine + String::Format("Exception: {0}. Stack trace: {1}.", ex->Message, ex->StackTrace));
			//					evtLog->WriteEntry(evtEntry->ToString(), EventLogEntryType::Error, log->photographformat1bppindexed);
							evtLog->Close();

							return h;
						}
					}
					static System::Drawing::Bitmap^ Format1BPPIndexed(System::Drawing::Bitmap^ ob)
					{
						// http://social.msdn.microsoft.com/Forums/en-US/Vsexpressvb/thread/e5e994e0-bf0b-4c66-83a4-2b7a6f48919e/
						try
						{
							if (ob->PixelFormat == System::Drawing::Imaging::PixelFormat::Format1bppIndexed)
							{
								return ob;
							}

							System::Drawing::Bitmap^ b = gcnew System::Drawing::Bitmap(ob->Width, ob->Height, System::Drawing::Imaging::PixelFormat::Format1bppIndexed);
							System::Drawing::Imaging::BitmapData^ bd = b->LockBits(System::Drawing::Rectangle(0, 0, b->Width, b->Height), System::Drawing::Imaging::ImageLockMode::ReadWrite, System::Drawing::Imaging::PixelFormat::Format1bppIndexed);

							for(int y = 0; y < ob->Height; y++)
							{
								for(int x = 0; x < ob->Width; x++)
								{
									// I didn't keep record or where I found example code for this function.
									if(ob->GetPixel(x, y).GetBrightness() > 0.625f)
									{
										BYTE* p = (BYTE*) bd->Scan0.ToPointer();
										INT index = y * bd->Stride + (x>>3);
										BYTE mask = (BYTE)(0x80>>(x&0x7));

										p[index] |= mask;
									}
								}
							}

							b->UnlockBits(bd);

							return b;
						}
						catch(Exception^ ex)
						{
							// Log the exception.
							StringBuilder^ evtEntry = gcnew StringBuilder(L"Console::Agent::Photograph::Format1BPPIndexed()" + Environment::NewLine);
							EventLog^ evtLog = gcnew EventLog(L"Application", L".", L"Ludus SnapShot");
							evtEntry->Append(Environment::NewLine + String::Format("Exception: {0}. Stack trace: {1}.", ex->Message, ex->StackTrace));
			//					evtLog->WriteEntry(evtEntry->ToString(), EventLogEntryType::Error, log->photographformat1bppindexed);
							evtLog->Close();

							return nullptr;
						}
					}
					static System::Drawing::Bitmap^ Format8BPPIndexed(System::Drawing::Bitmap^ ob)
					{
						// http://support.microsoft.com/kb/319061
						/*
						if (ob.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
						{
							return ob;
						}

						System.Drawing.Bitmap bm = new  System.Drawing.Bitmap(ob.Width, ob.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
						System.Drawing.Imaging.BitmapData bmd = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

						IntPtr pixels = bmd.Scan0;

						unsafe 
						{ 
							// Get the pointer to the image bits.
							// This is the unsafe operation.
							byte* pBits;
							if (bmd.Stride > 0)
							{
								pBits = (byte*) pixels.ToPointer();
							}
							else
							{
								// If the Stride is negative, Scan0 points to the last 
								// scanline in the buffer. To normalize the loop, obtain
								// a pointer to the front of the buffer that is located 
								// (Height-1) scanlines previous.
								pBits = (byte*) pixels.ToPointer() + bmd.Stride * (ob.Height - 1);
								uint stride = (uint)Math.Abs(bmd.Stride);
							}

							for (uint row = 0; row < ob.Height; ++row)
							{
								for (uint col = 0; col < ob.Width; ++col)
								{
									// Map palette indexes for a gray scale.
									// If you use some other technique to color convert,
									// put your favorite color reduction algorithm here.
									System.Drawing.Color pixel;    // The source pixel.

									// The destination pixel.
									// The pointer to the color index byte of the
									// destination; this real pointer causes this
									// code to be considered unsafe.
									byte* p8bppPixel = pBits + row * bmd.Stride + col;

									pixel = ob.GetPixel((int)col, (int)row);

									// Use luminance/chrominance conversion to get grayscale.
									// Basically, turn the image into black and white TV.
									// Do not calculate Cr or Cb because you 
									// discard the color anyway.
									// Y = Red * 0.299 + Green * 0.587 + Blue * 0.114

									// This expression is best as integer math for performance,
									// however, because GetPixel listed earlier is the slowest 
									// part of this loop, the expression is left as 
									// floating point for clarity.
									double luminance = (pixel.R * 0.299) + (pixel.G * 0.587) + (pixel.B * 0.114);

									// Gray scale is an intensity map from black to white.
									// Compute the index to the grayscale entry that
									// approximates the luminance, and then round the index.
									// Also, constrain the index choices by the number of
									// colors to do, and then set that pixel's index to the 
									// byte value.
									*p8bppPixel = (byte)(luminance * (256-1) / 255 + 0.5);
								}
							} 
						}			
						bm.UnlockBits(bmd);
						return bm;
						*/
						return ob;
					}
				protected:
					/// <summary>
					/// Clean up any resources being used.
					/// </summary>
					~Photograph()
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

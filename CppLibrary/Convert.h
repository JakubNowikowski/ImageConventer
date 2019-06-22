#pragma once

using namespace System;
using namespace cli;
using namespace System::Text;

namespace CliNamespace {
	class UnmanagedClass {
	public:
		void ToMainColors(unsigned char a[], int n) {
			int i = 0;
			while (i < n)
			{
				int B = a[i];
				int G = a[i + 1];
				int R = a[i + 2];

				if (B > G & B > R)
				{
					a[i] = 255;
					a[i + 1] = 0;
					a[i + 2] = 0;
					a[i + 3] = 255;
				}
				else if (G > B & G > R)
				{
					a[i] = 0;
					a[i + 1] = 255;
					a[i + 2] = 0;
					a[i + 3] = 255;
				}
				else if (R > B & R > G)
				{
					a[i] = 0;
					a[i + 1] = 0;
					a[i + 2] = 255;
					a[i + 3] = 255;
				}
				i += 4;
			}

		}

	};

	public ref class ManagedClass {
	private:
		UnmanagedClass *uc;
	public:
		ManagedClass() {
			uc = new UnmanagedClass();
		}

		void ToMainColorsCPP(array<unsigned char>^ a) { // handler
			//auto sb = gcnew StringBuilder(); //c# object
			pin_ptr<unsigned char> p = &a[0];
			uc->ToMainColors(p, a->Length);
		}
	};
}

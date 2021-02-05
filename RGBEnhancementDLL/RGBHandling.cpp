#using <mscorlib.dll>
#include "StdAfx.h"


using namespace System;
using namespace System::Runtime::InteropServices;

typedef unsigned char byte;

namespace ToolkitRGBEnhancement
{
	namespace RGBHandling
	{
		//
		// The actual transformation is performed here
		//
		public class TransformLoop
		{
			// The plane0 pointer must point to a GBR formattet bitmap (Despite the namespace :-)  )
			//
			public: byte* Doit(byte *plane0, int red, int green, int blue, int offset, int stride, int width, int height) {
					int ofz = 0;
					int line = 0;
					int pos = 0;
					byte *newPlanes = new byte[stride*height];
					while (line<height)
					{
						while (pos<width)
						{
							int newBlue = ((plane0[ofz] * blue) / 100) +offset;
							if (newBlue<0) newBlue = 0;
							if (newBlue>255) newBlue = 255;
							newPlanes[ofz] = newBlue;

							int newGreen = ((plane0[ofz+1] * green) / 100) +offset;
							if (newGreen<0) newGreen = 0;
							if (newGreen>255) newGreen = 255;
							newPlanes[ofz+1] = newGreen;

							int newRed = ((plane0[ofz+2] * red) / 100) +offset;
							if (newRed<0) newRed = 0;
							if (newRed>255) newRed = 255;
							newPlanes[ofz+2] = newRed;
							
							pos++;
							ofz += 3;
						}
						pos = 0;
						line++;
						ofz = line * stride;
					}

					/*

					int max = size/3;
					while (ofz< size)
					{
						int newGreen = ((plane0[ofz] * green) / 100) +offset;
						if (newGreen<0) newGreen = 0;
						if (newGreen>255) newGreen = 255;
						newPlanes[ofz] = newGreen;

						int newBlue = ((plane0[ofz+1] * blue) / 100) +offset;
						if (newBlue<0) newBlue = 0;
						if (newBlue>255) newBlue = 255;
						newPlanes[ofz+1] = newBlue;

						int newRed = ((plane0[ofz+2] * red) / 100) +offset;
						if (newRed<0) newRed = 0;
						if (newRed>255) newRed = 255;
						newPlanes[ofz+2] = newRed;
						ofz += 3;
					}
					*/
					return &newPlanes[0];
				}

			public: void Release(byte *plane0) {
					delete[] plane0;
				}
		};

		//
		// Managed wrapper for our color transformation sample
		//
		public ref class Transform
		{
			public: Transform()
				{
					loop = new TransformLoop();
					red = 100;
					green = 100;
					blue = 100;
					offset = 50;
				}

			public: ~Transform()
				{
				}

    public: !Transform()
    {
      delete loop;
    }

    public: void SetVectors(int r, int g, int b, int ofz)
				{
					red = r;
					green = g;
					blue = b;
					offset = ofz;
				}

			public:	IntPtr Perform(IntPtr plane0, int stride, int width, int height) {

				unsigned char* oldPlanes = static_cast<unsigned char*>(plane0.ToPointer() );

				byte* result = loop->Doit(oldPlanes, red, green, blue, offset, stride, width, height);

				return (IntPtr)&result[0];
			}

			public: void Release(IntPtr plane0) {
				unsigned char* oldPlanes = static_cast<unsigned char*>(plane0.ToPointer() );
				loop->Release(oldPlanes);
			}

			private: TransformLoop *loop;
			private: int red;
			private: int green;
			private: int blue;
			private: int offset;
		};


	}
}

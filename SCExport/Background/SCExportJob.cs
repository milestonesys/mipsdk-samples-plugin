using System;
using System.Drawing;
using VideoOS.Platform;

namespace SCExport.Background
{
    internal class SCExportJob
	{
		internal Item Item { get; set; }
		internal DateTime StartTime { get; set; }
		internal DateTime EndTime { get; set; }
		internal string Path { get; set; }
		internal string FileName { get; set; }
		internal bool AVIexport { get; set; }
        internal bool MKVexport { get; set; }
        internal bool PreventReExport { get; set; }
        internal bool SignExport { get; set; }

        internal Bitmap OverlayImage { get; set; }
        internal int VerticalOverlayPosition { get; set; }
        internal int HorizontalOverlayPosition { get; set; }
        internal double ScaleFactor { get; set; }
        internal bool IgnoreAspect { get; set; }

        public override string ToString()
		{
			return StartTime.ToShortTimeString() + " for " + Item.Name;
		}
	}
}

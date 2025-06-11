using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoPreview.Admin
{
	public partial class ImagePreviewForm : Form
	{
		public ImagePreviewForm()
		{
			InitializeComponent();
		}

		public Image Image
		{
			set
			{
				if (value == null)
				{
					pictureBox1.Image = new Bitmap(20, 20);
				}
				else
				{
					this.ClientSize = value.Size;
					pictureBox1.Image = value;
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AtaLogger;

namespace AtaLoggerStoreDataApp
{
	public partial class Form1 : Form
	{
		private AtaLogger.AtaLogger l;
		private AtaLoggerFinder lf = new AtaLoggerFinder();

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			l = lf.FindLoggerPort("COM3");
			if(l==null)
			MessageBox.Show("Logger not found !");
			else
			{
				label1.Text = l.SerialNumber + " on " + l.SerialPortName;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var ld = l.GetDetailsFromDevice();
			label2.Text = ld.SerialNumber + " " + ld.Description + ":" + ld.NumberOfSamples.ToString() + " samples";
		}

		private void button3_Click(object sender, EventArgs e)
		{

		}
	}
}

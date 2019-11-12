using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AtalLogger;

namespace AtalLoggerStoreDataApp
{
	public partial class Form1 : Form
	{
		
		public Form1()
		{
			InitializeComponent();

			AtalLoggerDataRetrievalCycle.Instance.OnProgress += Instance_OnProgress;
		}

	
		private void button1_Click(object sender, EventArgs e)
		{
			//l = lf.FindLoggerPort("COM3");
			//if(l==null)
			//MessageBox.Show("Logger not found !");
			//else
			//{
			//	label1.Text = l.SerialNumber + " on " + l.SerialPortName;
			//}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//var ld = l.GetDetailsFromDevice();
			//label2.Text = ld.SerialNumber + " " + ld.Description + ":" + ld.NumberOfSamples.ToString() + " samples";
		}

		private void button3_Click(object sender, EventArgs e)
		{

		}

		private void button6_Click(object sender, EventArgs e)
		{
			try
			{
				AtalLoggerDataRetrievalCycle.Instance.Start(this);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Fout", MessageBoxButtons.OK);
			}
		}

		private void Instance_OnProgress(object sender, ProgressEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}

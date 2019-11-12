using System;
using System.Threading;
using AtalLogger;

namespace AtalLoggerDump
{
	class Program
	{
		static void Main(string[] args)
		{
			AtalLogger.AtalLogger logger = null;
			AtalLoggerFinder finder = new AtalLoggerFinder();
			
			Console.WriteLine("Detecting logger for 10s.  Press Ctrl-C to interrupt.");

			var attempts = 20;
			do
			{
				
				logger = finder.FindLoggerPort("COM3");
				Thread.Sleep(500);
				Console.Write(".");

			} while (logger == null && --attempts>0);

			if (logger == null)
				return;

			Console.WriteLine("Logger detected on {0}", logger.SerialPortName);
			var detail = logger.GetDetailsFromDevice();
			Console.WriteLine("Serial number     : {0}",detail.SerialNumber);
			Console.WriteLine("Number of samples : {0}", detail.NumberOfSamples);

			var data = logger.GetSamplesFromDevice(detail.NumberOfSamples,null);

			foreach (var d in data)
			{
				Console.WriteLine("{0}\t{1}", d.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"), d.Temperature);
			}
			logger.Dispose();
		}
	}
}

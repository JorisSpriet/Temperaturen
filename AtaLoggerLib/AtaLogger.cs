using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using log4net;

namespace AtaLogger
{
	public class AtaLogger : IDisposable, IAtaLogger
	{
		private static ILog logger = LogManager.GetLogger(typeof(AtaLogger));

		private readonly SerialPort serialPort;

		public string SerialNumber { get; private set; }

		public string SerialPortName
		{
			get { return serialPort?.PortName ?? "-"; }
		}

		public LoggerDetails GetDetailsFromDevice()
		{
			serialPort.Open();
			var getinfomessage = Messages.GetDataInfo1Message(SerialNumber);
			serialPort.Write(getinfomessage, 0, getinfomessage.Length);

			Thread.Sleep(500);
			var buffer = new byte[Marshal.SizeOf(typeof(AnswerGetInfoDetails1Message))];
			var offset = 0;

			while (serialPort.BytesToRead > 0)
			{
				var a = serialPort.BytesToRead;
				int r = serialPort.Read(buffer, offset, a);
				offset += r;
			}
			serialPort.Close();

			var details1 = Utils.Map<AnswerGetInfoDetails1Message>(buffer);

			//System.IO.File.WriteAllBytes("d:\\buffer.bin",buffer);
			//Environment.Exit(0);

			if (!details1.IsValid())
				throw new Exception("Received invalid details from device.");

			var result = new LoggerDetails
			{
				Description = Encoding.ASCII.GetString(details1.Description),
				SerialNumber = Encoding.ASCII.GetString(details1.SerialNumber),
				Info = Encoding.ASCII.GetString(details1.Info),
				NumberOfSamples = details1.NumberOfSamples
			};

			if (SerialNumber != result.SerialNumber)
				throw new Exception("Received invalid details from device : different serial number");

			return result;
		}

		public LoggerSample[] GetSamplesFromDevice(int numberOfSamples)
		{
			var result = new List<LoggerSamplePacket>();
			
			var fs = File.OpenWrite(".\\dump.bin");


			serialPort.Open();
			serialPort.Write(Messages.GetDataMessage(SerialNumber));
			Thread.Sleep(500);

			if (numberOfSamples == 0)
				return new LoggerSample[0];

			int numberOfPackets = (numberOfSamples / 15) + (numberOfSamples % 15 != 0 ? 1 : 0);

			int offset = 0;
			var buffer = new byte[1024];
			int addpLength = Utils.LengthOf<AnswerDownloadDataPacket>();
			int messagesToRead = 0;
			int messagesRead = 0;
			var lastPacketProcessed = false;
			do
			{
				var bytesToRead = addpLength - offset;
				var bytesRead = serialPort.Read(buffer, offset, bytesToRead);
				offset += bytesRead;
				if (offset < addpLength)
				{
					Thread.Sleep(100);
					continue;
				}

				fs.Write(buffer,0,offset);

				var addlp = Utils.Map<AnswerDownloadDataPacket>(buffer);
				if (addlp.NumberOfPackets != numberOfPackets)
				{
					logger.WarnFormat(
						"Logger {0} announces {1} packets to be received; based on number of samples {2} were expected",
						SerialNumber, addlp.NumberOfPackets, numberOfPackets);
				}

				var extraBytesToRead = addlp.NumberOfSamples * 6 + 5;
				var extraBytesRead = serialPort.Read(buffer, 0, extraBytesToRead);

				fs.Write(buffer,0,extraBytesRead);

				var samplebytes = new byte[extraBytesToRead - 5];
				Array.Copy(buffer, 0, samplebytes, 0, extraBytesToRead - 5);
				var samples = Utils.MapArray<LoggerSamplePacket>(samplebytes, addlp.NumberOfSamples);
				result.AddRange(samples);
				lastPacketProcessed = addlp.NumberOfPackets == addlp.PacketNumber;
				offset = 0;

			} while (!lastPacketProcessed);

			serialPort.Close();
			fs.Close();

			var data = new LoggerSample[numberOfSamples];
			for (int i = 0; i < numberOfSamples; i++)
			{
				data[i] = result[i].Map();
			}

			return data;
		}

		public void ClearDataOnDevice()
		{
			//serialPort.Write(Messages.ClearDataMessage());
		}



		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				serialPort.Dispose();

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Logger() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}


		#endregion

		public AtaLogger(string serialNumber, SerialPort port)
		{
			SerialNumber = serialNumber;
			serialPort = port;
		}
	}

}

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using log4net.Repository.Hierarchy;

namespace AtaLogger
{
	public interface  IAtaLogger
	{
		string SerialNumber { get; }

		LoggerDetails GetDetailsFromDevice();

		LoggerSample[] GetSamplesFromDevice(int numberOfSamples);

		void ClearDataOnDevice();

		void Dispose();
	}

	public delegate void SamplesReadingCallback(LoggerSample[] readSamples, int totalNumberOfSamples, int offSet);

}

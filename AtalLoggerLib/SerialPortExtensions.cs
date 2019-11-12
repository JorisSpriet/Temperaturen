﻿using System.IO.Ports;

namespace AtalLogger
{
	public static class SerialPortExtensions
	{
		public static void Write(this SerialPort port, byte[] data)
		{
			int length = data.Length;
			port.Write(data,0,length);
		}

	}
}

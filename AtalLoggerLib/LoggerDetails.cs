﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AtaLogger
{
	public class LoggerDetails
	{
		public string SerialNumber { get; set; }

		public string Description { get; set; }

		public string Info { get; set; }

		public int NumberOfSamples { get; set; }
	}

}

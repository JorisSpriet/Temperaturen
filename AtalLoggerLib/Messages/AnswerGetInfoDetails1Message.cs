using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AtalLogger
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct AnswerGetInfoDetails1Message
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Header;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] SerialNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] AnswerCode;

		

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] Description;
		
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Unknown1;

		public byte Stx;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public byte[] Info;

		public int LoggingInterval;

		public int NumberOfSamples;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public byte[] Unknown3;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Tail;

		public bool IsValid()
		{
			//return Messages.Header.SequenceEqual(Header) &&
			//       Messages.AnswerGetSerialCommand.SequenceEqual(AnswerCode) &&
			//       Messages.Tail.SequenceEqual(Tail);
			return true;
		}

		public string GetSerialNumber()
		{
			return Encoding.ASCII.GetString(SerialNumber);
		}

	}
}

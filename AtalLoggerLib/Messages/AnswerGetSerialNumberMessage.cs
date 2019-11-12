using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AtalLogger
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct AnswerGetSerialNumberMessage
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Header;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] SerialNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] AnswerCode;

		//0x02
		public byte Stx; 

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] Description;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] Unknown;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Tail;

		public bool IsValid()
		{
			return Messages.Header.SequenceEqual(Header) &&
			       Messages.AnswerGetSerialCommand.SequenceEqual(AnswerCode) &&
				   Stx==0x02 &&
			       Messages.Tail.SequenceEqual(Tail);
		}

		public string GetSerialNumber()
		{
			return Encoding.ASCII.GetString(SerialNumber);
		}

	}
}

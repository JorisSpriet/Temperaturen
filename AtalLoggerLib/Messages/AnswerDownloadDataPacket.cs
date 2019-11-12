using System.Runtime.InteropServices;

namespace AtalLogger
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class AnswerDownloadDataPacket
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Header;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] SerialNumber;

		//0x58 0x02
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] AnswerCode;

		public ushort NumberOfPackets;

		public ushort PacketNumber;

		public byte NumberOfSamples;


		/* after this : NumberOfSamples times:
		 
		public int TimeStamp;

		//in 0.1 °C
		public short Temperature; 

		 */

		/* after this :
		 
		//checksum ?
		byte Unknown; 

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		byte[] Tail;
		 */
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	
	public class LoggerSamplePacket
	{
		[MarshalAs(UnmanagedType.I4)]
		public int TimeStamp;
		[MarshalAs(UnmanagedType.I2)]
		public short Temperature;
	}
}

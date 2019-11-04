using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AtaLogger
{
	public static class Utils
	{
		public static T Map<T>(byte[] data)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);		
			var lfh = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();
			return lfh;
		}

		public static T[] MapArray<T>(byte[] data, int length) where T : new()
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var lfh = new T[length];
			var addr = handle.AddrOfPinnedObject();
			var size = Marshal.SizeOf(typeof(T));
			for (int i = 0; i < length; i++)
			{
				lfh[i]=new T();
				Marshal.PtrToStructure(addr+(i*size), lfh[i]);
			}
			
			
			handle.Free();
			return lfh;

		}

		

		public static int LengthOf<T>()
		{
			return Marshal.SizeOf(typeof(T));
		}


		private static readonly DateTime epoch = new DateTime(1993,01,15);

		public static DateTime ConvertToDateTime(this int timeStamp)
		{
			var ts =timeStamp;
			
			var mmss = ts & 0xfff;
			var sec = (mmss & 0x03f) ;
			var mm = (0xfc0 & mmss) >> 6;
			var dddhh = ts >> 12;
			var h = dddhh % 0x20;

			var date = (ts & 0xffff0000) >> 17;
			var day = date & 0x1f;
			var month = (date >> 5) & 0x00f;
			var year = (date >> 9) & 0xfffff;


			return new DateTime(2000+(int)year,(int)month,(int)day,h,mm,sec);
		}

		public static LoggerSample Map(this LoggerSamplePacket lsp)
		{
			return new LoggerSample
			{
				TimeStamp = lsp.TimeStamp.ConvertToDateTime(),
				Temperature = Convert.ToDecimal(lsp.Temperature) / 10M
			};
		}
	}
}

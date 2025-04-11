using System.Runtime.InteropServices;

namespace JiaoLongWMI.tools
{
	public class WinIo : IDisposable
	{
		private string dllName = "WinIo64.dll";

		// 导入 WinIo64.dll 中的函数
		[DllImport("WinIo64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool InitializeWinIo();

		[DllImport("WinIo64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern void ShutdownWinIo();


		[DllImport("WinIo64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool GetPortVal(ushort wPortAddr, ref byte pdwPortVal, byte bSize);

		[DllImport("WinIo64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool SetPortVal(ushort wPortAddr, byte dwPortVal, byte bSize);

		[DllImport("kernel32")]
		public static extern IntPtr LoadLibrary(string lpFileName);


		[DllImport("kernel32", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		private IntPtr winioHandle;

		private ushort EC_ADDR_PORT = 0x4E;
		private ushort EC_DATA_PORT = 0x4F;
		public bool WinIoState { get; set; }
		public WinIo()
		{
			winioHandle = LoadLibrary(dllName);
			WinIoState = InitializeWinIo();
		}

		public bool EC_RAM_WRITE(ushort iIndex, byte data)
		{
			byte highByte = (byte)(iIndex >> 8);
			byte lowByte = (byte)(iIndex & 0xFF);
			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x11);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			WRITE_PORT(EC_DATA_PORT, highByte); // High byte

			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x10);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			WRITE_PORT(EC_DATA_PORT, lowByte); // Low byte

			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x12);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			return WRITE_PORT(EC_DATA_PORT, data);
		}

		public byte EC_RAM_READ(ushort iIndex)
		{
			byte highByte = (byte)(iIndex >> 8);
			byte lowByte = (byte)(iIndex & 0xFF);
			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x11);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			WRITE_PORT(EC_DATA_PORT, highByte); // High byte

			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x10);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			WRITE_PORT(EC_DATA_PORT, lowByte); // Low byte

			WRITE_PORT(EC_ADDR_PORT, 0x2E);
			WRITE_PORT(EC_DATA_PORT, 0x12);
			WRITE_PORT(EC_ADDR_PORT, 0x2F);
			return READ_PORT(EC_DATA_PORT);
		}

		private bool WRITE_PORT(ushort iIndex, byte data)
		{
			return SetPortVal(iIndex, data, 1);
		}

		private byte READ_PORT(ushort iIndex)
		{
			byte data = 0;
			GetPortVal(iIndex, ref data, 1);
			return data;
		}

		public void Dispose()
		{
			ShutdownWinIo();
			FreeLibrary(winioHandle);
		}
	}
}

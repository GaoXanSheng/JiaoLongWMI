using System;
using WinRing0Driver.Driver;

namespace JiaoLongWMI.Utils
{
    public class WinRing0EC : IDisposable
    {
        private readonly OLS _ols;

        private readonly ushort EC_ADDR_PORT = 0x4E;
        private readonly ushort EC_DATA_PORT = 0x4F;

        public bool State => _ols != null && _ols.IsOpen;

        public WinRing0EC()
        {
            _ols = new OLS();
            if (!State)
                throw new Exception("WinRing0 初始化失败，请以管理员身份运行。");
        }

        /// <summary>
        /// 向 EC RAM 写入数据
        /// </summary>
        public bool EC_RAM_WRITE(ushort iIndex, byte data)
        {
            byte highByte = (byte)(iIndex >> 8);
            byte lowByte = (byte)(iIndex & 0xFF);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x11);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            WRITE_PORT(EC_DATA_PORT, highByte);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x10);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            WRITE_PORT(EC_DATA_PORT, lowByte);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x12);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            WRITE_PORT(EC_DATA_PORT, data);

            return true;
        }

        /// <summary>
        /// 从 EC RAM 读取数据
        /// </summary>
        public byte EC_RAM_READ(ushort iIndex)
        {
            byte highByte = (byte)(iIndex >> 8);
            byte lowByte = (byte)(iIndex & 0xFF);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x11);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            WRITE_PORT(EC_DATA_PORT, highByte);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x10);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            WRITE_PORT(EC_DATA_PORT, lowByte);

            WRITE_PORT(EC_ADDR_PORT, 0x2E);
            WRITE_PORT(EC_DATA_PORT, 0x12);
            WRITE_PORT(EC_ADDR_PORT, 0x2F);
            return READ_PORT(EC_DATA_PORT);
        }

        private void WRITE_PORT(ushort port, byte value)
        {
            _ols.WriteIoPortByte(port, value);
        }

        private byte READ_PORT(ushort port)
        {
            return _ols.ReadIoPortByte(port);
        }

        public void Dispose()
        {
            _ols?.Dispose();
        }
    }
}

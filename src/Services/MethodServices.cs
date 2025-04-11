using System.Management;
using System.Runtime.InteropServices;
using JiaoLongWMI.tools;

namespace JiaoLongWMI.WMIOperation.Method
{
    public static class MethodServices
    {
        private const int BufferLength = 32;

        private static byte[] MakeMethodParams(MethodType methodType, MethodName methodName)
        {
            var buffer = new byte[BufferLength];
            buffer[1] = (byte)methodType;
            buffer[3] = (byte)methodName;
            return buffer;
        }

        public static T GetValue<T>(MethodName methodName)
        {
            var result = ExecuteMethod(MakeMethodParams(MethodType.Get, methodName));

            if (!result.Item1 || result.Item2 == null)
                return GetDefaultValue<T>();

            var data = result.Item2;
            return typeof(T) switch
            {
                var t when t == typeof(Tuple<int, int>) =>
                    (T)(object)new Tuple<int, int>((data[5] << 8) + data[4], (data[7] << 8) + data[6]),
                var t when t == typeof(Tuple<int, int, int>) =>
                    (T)(object)new Tuple<int, int, int>(data[4], data[5], data[6]),
                _ => (T)(object)data[4]
            };
        }

        public static bool SetValue(MethodName methodName, object value)
        {
            var data = MakeMethodParams(MethodType.Set, methodName);
            data[4] = Convert.ToByte(value);
            return ExecuteMethod(data).Item1;
        }

        public static bool SetValue(MethodName methodName, byte[] values)
        {
            if (values == null || values.Length + 4 > BufferLength)
                throw new ArgumentException("Invalid value length.");

            var data = MakeMethodParams(MethodType.Set, methodName);
            Array.Copy(values, 0, data, 4, values.Length);
            return ExecuteMethod(data).Item1;
        }

        private static Tuple<bool, byte[]> ExecuteMethod(byte[] inData)
        {
	        if (inData.Length != BufferLength)
		        return new Tuple<bool, byte[]>(false, null);

	        try
	        {
		        // 使用 Task.Run 来异步执行 WMI 调用，并通过 Result 获取同步结果
		        var result = Task.Run(() =>
		        {
			        try
			        {
				        var mo = new ManagementObject("root\\WMI", "MICommonInterface.InstanceName='ACPI\\PNP0C14\\MIFS_0'", null);
				        var parameters = mo.GetMethodParameters("MiInterface");
				        parameters["InData"] = inData;
				        var output = mo.InvokeMethod("MiInterface", parameters, null)?["OutData"] as byte[];
				        mo.Dispose();
				        return new Tuple<bool, byte[]>(output != null, output);
			        }
			        catch (ManagementException ex)
			        {
				        CliProgramEnumerationType.ErrMag = ex.Message;
				        return new Tuple<bool, byte[]>(false, null);
			        }
		        }).Result; // 使用 Result 来同步等待 Task 完成
		        return result; // 返回最终的同步结果
	        }
	        catch (Exception ex)
	        {
		        // 捕获 Task.Run 相关的异常
		        CliProgramEnumerationType.ErrMag = ex.Message;
		        return new Tuple<bool, byte[]>(false, null);
	        }
        }


        private static T GetDefaultValue<T>()
        {
            return typeof(T) switch
            {
                var t when t == typeof(Tuple<int, int>) =>
                    (T)(object)new Tuple<int, int>(-1, -1),
                var t when t == typeof(Tuple<int, int, int>) =>
                    (T)(object)new Tuple<int, int, int>(-1, -1, -1),
                _ => (T)(object)byte.MaxValue
            };
        }
    }
}

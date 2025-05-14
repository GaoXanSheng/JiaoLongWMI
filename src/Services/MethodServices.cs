using System.Management;
using JiaoLongWMI.Constants;
using JiaoLongWMI.Controllers;

namespace JiaoLongWMI.Services
{
    /// <summary>
    /// 方法服务类，用于执行 WMI 方法。
    /// </summary>
    public static class MethodServices
    {
        private const int BufferLength = 32;

        /// <summary>
        /// 创建方法参数。
        /// </summary>
        /// <param name="methodType">方法类型。</param>
        /// <param name="methodName">方法名称。</param>
        /// <returns>包含方法类型和方法名称的字节数组。</returns>
        private static byte[] MakeMethodParams(MethodType methodType, MethodName methodName)
        {
            var buffer = new byte[BufferLength];
            buffer[1] = (byte)methodType;
            buffer[3] = (byte)methodName;
            return buffer;
        }

        /// <summary>
        /// 获取值。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <param name="methodName">方法名称。</param>
        /// <returns>获取到的值。</returns>
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

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="methodName">方法名称。</param>
        /// <param name="value">要设置的值。</param>
        /// <returns>是否设置成功。</returns>
        public static bool SetValue(MethodName methodName, object value)
        {
            var data = MakeMethodParams(MethodType.Set, methodName);
            data[4] = Convert.ToByte(value);
            return ExecuteMethod(data).Item1;
        }

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="methodName">方法名称。</param>
        /// <param name="values">要设置的值的字节数组。</param>
        /// <returns>是否设置成功。</returns>
        public static bool SetValue(MethodName methodName, byte[] values)
        {
            if (values == null || values.Length + 4 > BufferLength)
                throw new ArgumentException("Invalid value length.");

            var data = MakeMethodParams(MethodType.Set, methodName);
            Array.Copy(values, 0, data, 4, values.Length);
            return ExecuteMethod(data).Item1;
        }

        /// <summary>
        /// 执行 WMI 方法。
        /// </summary>
        /// <param name="inData">输入数据。</param>
        /// <returns>包含执行结果和输出数据的元组。</returns>
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


        /// <summary>
        /// 获取默认值。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <returns>默认值。</returns>
        private static T GetDefaultValue<T>()
        {
            object value = typeof(T) switch
            {
                var t when t == typeof(Tuple<int, int>) =>
                    new Tuple<int, int>(-1, -1),
                var t when t == typeof(Tuple<int, int, int>) =>
                    new Tuple<int, int, int>(-1, -1, -1),
                var t when t == typeof(byte) =>
                    byte.MaxValue,
                _ => default(T)
            };

            return (T)value!;
        }
    }
}

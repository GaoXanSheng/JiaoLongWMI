using System.Management;

namespace JiaoLong16Pro.BLD.WMIOperation
{
    public static class WMIMethodServices
    {
        private static byte[] _MakeMethodPrams(WMIMethodType wMIMethodType, WMIMethodName wMIMethodName)
        {
            byte[] prams = new byte[32];
            for (int i = 0; i < prams.Length; i++)
            {
                prams[i] = 0;
            }
            prams[1] = (byte)wMIMethodType;
            prams[3] = (byte)wMIMethodName;
            return prams;
        }
        public static T GetValue<T>(WMIMethodName wMIMethodName)
        {
            Tuple<bool, byte[]> tuple = WMIMethodServices.ExcMethod(WMIMethodServices._MakeMethodPrams(WMIMethodType.Get, wMIMethodName));
            if (!tuple.Item1)
            {
                if (typeof(T) == typeof(Tuple<int, int>))
                {
                    return (T)((object)new Tuple<int, int>(-1, -1));
                }
                if (typeof(T) == typeof(Tuple<int, int, int>))
                {
                    return (T)((object)new Tuple<int, int, int>(-1, -1, -1));
                }
                return (T)((object)byte.MaxValue);
            }
            else
            {
                if (typeof(T) == typeof(Tuple<int, int>))
                {
                    int item = ((int)tuple.Item2[5] << 8) + (int)tuple.Item2[4];
                    int gpufanspeed = ((int)tuple.Item2[7] << 8) + (int)tuple.Item2[6];
                    return (T)((object)new Tuple<int, int>(item, gpufanspeed));
                }
                if (typeof(T) == typeof(Tuple<int, int, int>))
                {
                    int item2 = (int)tuple.Item2[4];
                    int RGBKeyboardColor_G = (int)tuple.Item2[5];
                    int RGBKeyboardColor_B = (int)tuple.Item2[6];
                    return (T)((object)new Tuple<int, int, int>(item2, RGBKeyboardColor_G, RGBKeyboardColor_B));
                }
                return (T)((object)tuple.Item2[4]);
            }
        }
        public static bool SetValue(WMIMethodName wMIMethodName, object setvalue)
        {
            bool ret = true;
            byte[] array = WMIMethodServices._MakeMethodPrams(WMIMethodType.Set, wMIMethodName);
            array[4] = (byte)setvalue;
            if (!WMIMethodServices.ExcMethod(array).Item1)
            {
                ret = false;
            }
            return ret;
        }
        public static bool SetValue(WMIMethodName wMIMethodName, byte[] setvalue)
        {
            bool ret = true;
            byte[] inparms = WMIMethodServices._MakeMethodPrams(WMIMethodType.Set, wMIMethodName);
            for (int i = 0; i < setvalue.Length; i++)
            {
                inparms[4 + i] = setvalue[i];
            }
            if (!WMIMethodServices.ExcMethod(inparms).Item1)
            {
                ret = false;
            }
            return ret;
        }

        public static Tuple<bool, byte[]> ExcMethod(byte[] inData)
        {
            if (inData == null)
            {
                return new Tuple<bool, byte[]>(false, null);
            }
            if (inData.Length != 32)
            {
                return new Tuple<bool, byte[]>(false, null);
            }
            Tuple<bool, byte[]> result;
            try
            {
                ManagementObject managementObject = new ManagementObject("root\\WMI", "MICommonInterface.InstanceName='ACPI\\PNP0C14\\MIFS_0'", null);
                ManagementBaseObject inParams = managementObject.GetMethodParameters("MiInterface");
                inParams["InData"] = inData;
                byte[] outData = managementObject.InvokeMethod("MiInterface", inParams, null)["OutData"] as byte[];
                result = new Tuple<bool, byte[]>(true, outData);
            }
            catch (ManagementException err)
            {
                Console.WriteLine(err.Message);
                result = new Tuple<bool, byte[]>(false, null);
            }
            return result;
        }
    }
}

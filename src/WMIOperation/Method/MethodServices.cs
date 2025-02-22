using System.Management;
using JiaoLongWMI.tools;

namespace JiaoLongWMI.WMIOperation.Method
{
   public static class MethodServices
  {

    private static byte[] _MakeMethodPrams(MethodType wMIMethodType, MethodName wMIMethodName)
    {
      byte[] numArray = new byte[32];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] =  0;
      numArray[1] = (byte) wMIMethodType;
      numArray[3] = (byte) wMIMethodName;
      return numArray;
    }

    public static T GetValue<T>(MethodName wMIMethodName)
    {
      Tuple<bool, byte[]> tuple = MethodServices.ExcMethod(MethodServices._MakeMethodPrams(MethodType.Get, wMIMethodName));
      if (!tuple.Item1)
      {
        if (typeof (T) == typeof (Tuple<int, int>))
          return (T)(object) new Tuple<int, int>(-1, -1);
        return typeof (T) == typeof (Tuple<int, int, int>) ? (T)(object) new Tuple<int, int, int>(-1, -1, -1) : (T)(object) byte.MaxValue;
      }
      if (typeof (T) == typeof (Tuple<int, int>))
        return (T)(object) new Tuple<int, int>((tuple.Item2[5] << 8) +tuple.Item2[4], (tuple.Item2[7] << 8) + tuple.Item2[6]);
      if (!(typeof (T) == typeof (Tuple<int, int, int>)))
        return (T)(object)tuple.Item2[4];
      int num1 = tuple.Item2[4];
      int num2 = tuple.Item2[5];
      int num3 = tuple.Item2[6];
      int num4 = num2;
      int num5 = num3;
      return (T)(object) new Tuple<int, int, int>(num1, num4, num5);
    }

    public static bool SetValue(MethodName wMIMethodName, object setvalue)
    {
      bool flag = true;
      byte[] numArray = _MakeMethodPrams(MethodType.Set, wMIMethodName);
      numArray[4] = (byte) setvalue;
      if (!ExcMethod(numArray).Item1)
        flag = false;
      return flag;
    }

    public static bool SetValue(MethodName wMIMethodName, byte[] setvalue)
    {
      bool flag = true;
      byte[] numArray = MethodServices._MakeMethodPrams(MethodType.Set, wMIMethodName);
      for (int index = 0; index < setvalue.Length; ++index)
        numArray[4 + index] = setvalue[index];
      if (!MethodServices.ExcMethod(numArray).Item1)
        flag = false;
      return flag;
    }

    public static Tuple<bool, byte[]> ExcMethod(byte[] inData)
    {
      if (inData.Length != 32)
        return new Tuple<bool, byte[]>(false,null);
      try
      {
        ManagementObject managementObject = new ManagementObject("root\\WMI", "MICommonInterface.InstanceName='ACPI\\PNP0C14\\MIFS_0'", null);
        ManagementBaseObject methodParameters = managementObject.GetMethodParameters("MiInterface");
        methodParameters["InData"] = inData;
        return new Tuple<bool, byte[]>(true, managementObject.InvokeMethod("MiInterface", methodParameters, null)["OutData"] as byte[]);
      }
      catch (ManagementException ex)
      {
        Logger.Info(ex.Message);
        return new Tuple<bool, byte[]>(false,null);
      }
    }
  }
}
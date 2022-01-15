using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Utubz.Internal.Native
{
    /// <summary>
    /// From CppSharp by Mono: <see href="https://github.com/mono/CppSharp/tree/main/src/Runtime"/>
    /// </summary>
    internal class UTF8Marshaller : ICustomMarshaler
    {
        public static UTF8Marshaller marshaler;

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData) => Marshal.FreeHGlobal(pNativeData);

        public int GetNativeDataSize() => -1;

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            if (managedObj == null)
                return IntPtr.Zero;
            byte[] source = managedObj is string ? Encoding.UTF8.GetBytes((string)managedObj) : throw new MarshalDirectiveException("UTF8Marshaler must be used on a string.");
            IntPtr destination = Marshal.AllocHGlobal(source.Length + 1);
            Marshal.Copy(source, 0, destination, source.Length);
            Marshal.WriteByte(destination + source.Length, (byte)0);
            return destination;
        }

        public unsafe object MarshalNativeToManaged(IntPtr str)
        {
            if (str == IntPtr.Zero)
                return (object)null;
            int byteCount = 0;
            byte* numPtr = (byte*)(void*)str;
            while (*numPtr++ > (byte)0)
                ++byteCount;
            return (object)Encoding.UTF8.GetString((byte*)(void*)str, byteCount);
        }

        public static ICustomMarshaler GetInstance(string pstrCookie)
        {
            if (UTF8Marshaller.marshaler == null)
                UTF8Marshaller.marshaler = new UTF8Marshaller();
            return (ICustomMarshaler)UTF8Marshaller.marshaler;
        }
    }
}

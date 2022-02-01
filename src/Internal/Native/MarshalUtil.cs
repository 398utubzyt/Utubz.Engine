using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Internal.Native
{
    /// <summary>
    /// From CppSharp by Mono: <see href="https://github.com/mono/CppSharp/tree/main/src/Runtime"/>
    /// </summary>
    internal static class MarshalUtil
    {
        public static unsafe string GetString(Encoding encoding, IntPtr str)
        {
            if (str == IntPtr.Zero)
                return (string)null;
            int byteCount = 0;
            if (encoding == Encoding.UTF32)
            {
                int* numPtr = (int*)(void*)str;
                while ((uint)*numPtr++ > 0U)
                    byteCount += 4;
            } else if (encoding == Encoding.Unicode || encoding == Encoding.BigEndianUnicode)
            {
                short* numPtr = (short*)(void*)str;
                while ((uint)*numPtr++ > 0U)
                    byteCount += 2;
            } else
            {
                byte* numPtr = (byte*)(void*)str;
                while (*numPtr++ > (byte)0)
                    ++byteCount;
            }
            return encoding.GetString((byte*)(void*)str, byteCount);
        }

        public static unsafe T[] GetArray<T>(void* array, int size) where T : unmanaged
        {
            if ((IntPtr)array == IntPtr.Zero)
                return (T[])null;
            T[] objArray = new T[size];
            fixed (T* objPtr = objArray)
                Buffer.MemoryCopy(array, (void*)objPtr, (long)(sizeof(T) * size), (long)(sizeof(T) * size));
            return objArray;
        }

        public static unsafe char[] GetCharArray(sbyte* array, int size)
        {
            if ((IntPtr)array == IntPtr.Zero)
                return (char[])null;
            char[] chArray = new char[size];
            for (int index = 0; index < size; ++index)
                chArray[index] = Convert.ToChar(array[index]);
            return chArray;
        }

        public static unsafe IntPtr[] GetIntPtrArray(IntPtr* array, int size) => MarshalUtil.GetArray<IntPtr>((void*)array, size);

        public static unsafe T GetDelegate<T>(IntPtr[] vtables, short table, int i) where T : class => Marshal.GetDelegateForFunctionPointer<T>(*(IntPtr*)(void*)(vtables[(int)table] + i * sizeof(IntPtr)));
    }
}
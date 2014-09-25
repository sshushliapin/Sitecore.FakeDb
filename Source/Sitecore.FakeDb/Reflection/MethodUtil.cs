using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Sitecore.FakeDb.Reflection
{
    /// <summary>
    ///     From http://www.codeproject.com/Articles/37549/CLR-Injection-Runtime-Method-Replacer
    /// </summary>
    internal static class MethodUtil
    {
        /// <summary>
        ///     Replaces the method.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dest">The dest.</param>
        public static void ReplaceMethod(MethodBase source, MethodBase dest)
        {
            if (!MethodSignaturesEqual(source, dest))
            {
                throw new ArgumentException("The method signatures are not the same.", "source");
            }

            ReplaceMethod(GetMethodAddress(source), dest);
        }

        /// <summary>
        ///     Replaces the method.
        /// </summary>
        /// <param name="srcAdr">The SRC adr.</param>
        /// <param name="dest">The dest.</param>
        public static void ReplaceMethod(IntPtr srcAdr, MethodBase dest)
        {
            var destAdr = GetMethodAddressRef(dest);

            unsafe
            {
                if (IntPtr.Size == 8)
                {
                    var d = (ulong*)destAdr.ToPointer();
                    *d = (ulong)srcAdr.ToInt64();
                }
                else
                {
                    var d = (uint*)destAdr.ToPointer();
                    *d = (uint)srcAdr.ToInt32();
                }
            }
        }

        private static IntPtr GetMethodAddressRef(MethodBase sourceMethod)
        {
            if ((sourceMethod is DynamicMethod))
            {
                return GetDynamicMethodAddress(sourceMethod);
            }

            // Prepare the method so it gets jited
            RuntimeHelpers.PrepareMethod(sourceMethod.MethodHandle);

            return GetMethodAddress20SP2(sourceMethod);
        }

        /// <summary>
        ///     Gets the address of the method stub
        /// </summary>
        /// <param name="method">The method handle.</param>
        /// <returns></returns>
        public static IntPtr GetMethodAddress(MethodBase method)
        {
            if ((method is DynamicMethod))
            {
                return GetDynamicMethodAddress(method);
            }

            // Prepare the method so it gets jited
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            return method.MethodHandle.GetFunctionPointer();
        }

        private static IntPtr GetDynamicMethodAddress(MethodBase method)
        {
            unsafe
            {
                var handle = GetDynamicMethodRuntimeHandle(method);

                RuntimeHelpers.PrepareMethod(handle);
                return handle.GetFunctionPointer();
            }
        }

        private static RuntimeMethodHandle GetDynamicMethodRuntimeHandle(MethodBase method)
        {
            RuntimeMethodHandle handle;

            if (Environment.Version.Major == 4)
            {
                var getMethodDescriptorInfo = typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.NonPublic | BindingFlags.Instance);

                handle = (RuntimeMethodHandle)getMethodDescriptorInfo.Invoke(method, null);
            }
            else
            {
                var fieldInfo = typeof(DynamicMethod).GetField("m_method", BindingFlags.NonPublic | BindingFlags.Instance);

                handle = ((RuntimeMethodHandle)fieldInfo.GetValue(method));
            }

            return handle;
        }

        private static IntPtr GetMethodAddress20SP2(MethodBase method)
        {
            unsafe
            {
                return new IntPtr(((int*)method.MethodHandle.Value.ToPointer() + 2));
            }
        }

        private static bool MethodSignaturesEqual(MethodBase x, MethodBase y)
        {
            if (x.CallingConvention != y.CallingConvention)
            {
                return false;
            }

            Type returnX = GetMethodReturnType(x), returnY = GetMethodReturnType(y);

            if (returnX != returnY)
            {
                return false;
            }

            ParameterInfo[] xParams = x.GetParameters(), yParams = y.GetParameters();

            if (xParams.Length != yParams.Length)
            {
                return false;
            }

            for (var i = 0; i < xParams.Length; i++)
            {
                if (xParams[i].ParameterType != yParams[i].ParameterType)
                {
                    return false;
                }
            }

            return true;
        }

        private static Type GetMethodReturnType(MethodBase method)
        {
            var methodInfo = method as MethodInfo;

            if (methodInfo == null)
            {
                // Constructor info.
                throw new ArgumentException("Unsupported MethodBase : " + method.GetType().Name, "method");
            }
            return methodInfo.ReturnType;
        }
    }
}
using System.Reflection;
using Sitecore.SecurityModel.License;

namespace Sitecore.FakeDb.Reflection
{
    public static class NullLicenseManager
    {
        public static void DemandRuntime(bool acceptExpress, bool forceUpdate)
        {
        }

        public static void Activate()
        {
            MethodBase originalMethod = typeof(LicenseManager).GetMethod("DemandRuntime", BindingFlags.Static | BindingFlags.NonPublic, null, CallingConventions.Any, new[] { typeof(bool), typeof(bool) }, new[] { new ParameterModifier(2) });
            MethodBase newMethod = typeof(NullLicenseManager).GetMethod("DemandRuntime", BindingFlags.Static | BindingFlags.Public);

            MethodUtil.ReplaceMethod(newMethod, originalMethod);
        }
    }
}
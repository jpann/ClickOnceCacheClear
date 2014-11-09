using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace mClickOnceCacheClear.Utilities
{
    public static class WindowsUtils
    {
        [DllImport("Shell32.DLL")]
        public static extern bool IsUserAnAdmin();

        public static bool IsUserLocalAdmin()
        {
            WindowsIdentity oUser = WindowsIdentity.GetCurrent();

            WindowsPrincipal oPrincipal = new WindowsPrincipal(oUser);

            bool value = oPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

            return value;
        }
    }
}

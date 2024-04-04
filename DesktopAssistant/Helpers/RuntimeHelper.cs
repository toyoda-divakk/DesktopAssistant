using System.Runtime.InteropServices;
using System.Text;

namespace DesktopAssistant.Helpers;

public class RuntimeHelper
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    // MSIXは、アプリケーションをパッケージ化し、配布、インストール、アップデートするための包括的なソリューションを提供します。これにより、アプリケーションのインストールやアンインストールが簡単になり、システムへの影響も最小限に抑えることができます。
    /// <summary>
    /// 現在のアプリケーションがMSIXパッケージであるかどうかを判定する
    /// </summary>
    public static bool IsMSIX
    {
        get
        {
            var length = 0;

            return GetCurrentPackageFullName(ref length, null) != 15700L;
        }
    }
}

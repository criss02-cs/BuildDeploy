using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace BuildDeployWpf.Utils;

public static class WindowDropShadow
{
    private static Window _win;

    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    [DllImport("dwmapi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

    /// <summary>
    /// Drops a standard shadow to a WPF Window, even if the window is borderless. Only works with DWM (Windows Vista or newer).
    /// This method is much more efficient than setting AllowsTransparency to true and using the DropShadow effect,
    /// as AllowsTransparency involves a huge performance issue (hardware acceleration is turned off for all the window).
    /// </summary>
    /// <param name="window">Window to which the shadow will be applied</param>
    public static void DropShadowToWindow(Window window)
    {
        _win = window;
        var source = PresentationSource.FromVisual(_win) as HwndSource;
        source?.AddHook(WndProc);
    }

    private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
    {
        var helper = new WindowInteropHelper(_win);
        var val = 2;
        var ret1 = DwmSetWindowAttribute(helper.Handle, 2, ref val, 4);

        if (ret1 != 0) return IntPtr.Zero;
        var m = new Margins { Bottom = 1, Left = 1, Right = 1, Top = 1 };
        DwmExtendFrameIntoClientArea(helper.Handle, ref m);
        return IntPtr.Zero;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace WpfApp2.Tools
{
    public static class FullScreenHelper
    {
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromRect([In] ref RECT lprc, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor; // 整个屏幕
            public RECT rcWork;    // 工作区（除任务栏）
            public uint dwFlags;
        }

        const uint MONITOR_DEFAULTTONEAREST = 2;

        /// <summary>
        /// 设置窗口全屏（不遮挡任务栏）
        /// </summary>
        public static void EnterFullScreen(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            GetWindowRect(hwnd, out RECT rect);

            IntPtr hMonitor = MonitorFromRect(ref rect, MONITOR_DEFAULTTONEAREST);

            MONITORINFO mi = new MONITORINFO();
            mi.cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            if (GetMonitorInfo(hMonitor, ref mi))
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = WindowState.Normal;

                window.Left = mi.rcWork.Left;
                window.Top = mi.rcWork.Top;
                window.Width = mi.rcWork.Right - mi.rcWork.Left;
                window.Height = mi.rcWork.Bottom - mi.rcWork.Top;
            }
        }

        /// <summary>
        /// 退出全屏（保持无边框，居中显示）
        /// </summary>
        public static void ExitFullScreen(Window window, double width = 800, double height = 600)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            GetWindowRect(hwnd, out RECT rect);
            IntPtr hMonitor = MonitorFromRect(ref rect, MONITOR_DEFAULTTONEAREST);

            MONITORINFO mi = new MONITORINFO();
            mi.cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            if (GetMonitorInfo(hMonitor, ref mi))
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = WindowState.Normal;

                window.Width = width;
                window.Height = height;
                window.Left = mi.rcMonitor.Left + (mi.rcMonitor.Right - mi.rcMonitor.Left - width) / 2;
                window.Top = mi.rcMonitor.Top + (mi.rcMonitor.Bottom - mi.rcMonitor.Top - height) / 2;
            }
        }
    }
}

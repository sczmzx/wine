using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MoveTouchAndPenToExtendScreen.Native;
using Size = System.Windows.Size;

namespace MoveTouchAndPenToExtendScreen
{
    /// <summary>
    /// 屏幕帮助类
    /// <para>bb 2024-05-31 17:51:11</para>
    /// </summary>
    public static class ScreenHelper
    {
        // 定义回调函数 MonitorEnumProc
        public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, ref RECT rect, IntPtr param);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc proc, IntPtr dwData);

        /// <summary>
        /// 获取显示设备句柄
        /// </summary>
        /// <param name="isMain"></param>
        /// <returns></returns>
        public static IntPtr GetDisplayDeviceHandle(bool isMain)
        {
            IntPtr monitor = new IntPtr();
            MonitorEnumProc callback = (IntPtr hMonitor, IntPtr hdc, ref RECT rect, IntPtr param) =>
            {
                if (isMain && rect.Left == 0)
                {
                    monitor = hMonitor;
                    return false;
                }

                if (!isMain && rect.Left != 0)
                {
                    monitor = hMonitor;
                    return false;
                }

                return true;
            };

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);
            return monitor;
        }
    }
}

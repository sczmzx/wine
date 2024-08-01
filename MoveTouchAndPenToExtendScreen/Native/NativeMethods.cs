using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MoveTouchAndPenToExtendScreen.Native
{
    internal static class NativeMethods
    {
        #region 模拟鼠标

        /// <summary>
        /// 模拟鼠标消息扩展信息
        /// </summary>
        public const uint InjectMouseExtraInfo = 10;

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll", EntryPoint = "SetPhysicalCursorPos")]
        public static extern int SetPhysicalCursorPos(int x, int y);


        //移动鼠标 
        public const int MOUSEEVENTF_MOVE = 0x0001;

        //模拟鼠标左键按下 
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;

        //模拟鼠标左键抬起 
        public const int MOUSEEVENTF_LEFTUP = 0x0004;

        //模拟鼠标右键按下 
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;

        //模拟鼠标右键抬起 
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        //模拟鼠标中键按下 
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;

        //模拟鼠标中键抬起 
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;

        //标示是否采用绝对坐标 
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        //模拟鼠标滚轮滚动操作，必须配合dwData参数
        public const int MOUSEEVENTF_WHEEL = 0x0800;

        #endregion


        #region const definitions

        internal const int
            WM_PARENTNOTIFY = 0x0210;

        internal const int
            WM_NCPOINTERUPDATE = 0x0241;

        internal const int
            WM_NCPOINTERDOWN = 0x0242;

        internal const int
            WM_NCPOINTERUP = 0x0243;

        internal const int
            WM_POINTERUPDATE = 0x0245;

        internal const int
            WM_POINTERDOWN = 0x0246;

        internal const int
            WM_POINTERUP = 0x0247;

        internal const int
            WM_POINTERENTER = 0x0249;

        internal const int
            WM_POINTERLEAVE = 0x024A;

        internal const int
            WM_POINTERACTIVATE = 0x024B;

        internal const int
            WM_POINTERCAPTURECHANGED = 0x024C;

        internal const int
            WM_POINTERWHEEL = 0x024E;

        internal const int
            WM_POINTERHWHEEL = 0x024F;

        internal const uint ANRUS_TOUCH_MODIFICATION_ACTIVE = 0x0000002;

        internal const int RIDEV_REMOVE = 0x00000001;
        internal const int RIDEV_INPUTSINK = 0x00000100;
        internal const int RIDEV_DEVNOTIFY = 0x00002000;
        internal const int RID_INPUT = 0x10000003;

        internal const int RIM_TYPEHID = 2;

        internal const uint RIDI_DEVICENAME = 0x20000007;
        internal const uint RIDI_DEVICEINFO = 0x2000000b;
        internal const uint RIDI_PREPARSEDDATA = 0x20000005;

        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_SYSKEYDOWN = 0x0104;
        internal const int WM_INPUT = 0x00FF;
        internal const int WM_INPUT_DEVICE_CHANGE = 0x00FE;
        internal const int VK_OEM_CLEAR = 0xFE;
        internal const int VK_LAST_KEY = VK_OEM_CLEAR; // this is a made up value used as a sentinal

        internal const int WmClose = 0x0010;

        internal const ushort GenericDesktopPage = 0x01;
        internal const ushort DigitizerUsagePage = 0x0D;
        internal const ushort ContactIdentifierId = 0x51;
        internal const ushort ContactCountId = 0x54;
        internal const ushort ScanTimeId = 0x56;
        internal const ushort TipId = 0x42;
        internal const ushort XCoordinateId = 0x30;
        internal const ushort YCoordinateId = 0x31;
        internal const ushort InRangeId = 0x32;
        internal const ushort BarrelButtonId = 0x44;
        internal const ushort InvertId = 0x3C;
        internal const ushort EraserId = 0x45;

        internal const ushort TouchPadUsage = 0x05;
        internal const ushort TouchScreenUsage = 0x04;
        internal const ushort ExternalPenUsage = 0x01;
        internal const ushort PenUsage = 0x02;


        public enum DwmWindowAttribute
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,

            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        };

        #endregion const definitions

        #region DllImports

        /// <summary>
        /// 确定给定窗口是否是最小化（图标化）的窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetCurrentThreadId();

        [DllImport("user32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterPointerInputTarget(IntPtr handle, POINTER_INPUT_TYPE pointerType);

        [DllImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterPointerInputTarget(IntPtr hwnd, POINTER_INPUT_TYPE pointerType);

        [DllImport("Oleacc.dll")]
        internal static extern int AccSetRunningUtilityState(IntPtr hWnd, uint dwUtilityStateMask, uint dwUtilityState);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameInfo(int pointerID, ref int pointerCount,
            [MarshalAs(UnmanagedType.LPArray)] [In] [Out] POINTER_INFO[] pointerInfo);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool InjectTouchInput(int count,
            [MarshalAs(UnmanagedType.LPArray)] [In] POINTER_TOUCH_INFO[] contacts);


  
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        internal static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize,
            uint cbSizeHeader);

        [DllImport("User32.dll")]
        internal static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices,
            uint cbSize);

        [DllImport("User32.dll")]
        internal static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint uiNumDevices,
            uint cbSize);

        [DllImport("User32.dll")]
        internal static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData,
            ref uint pcbSize);

        [DllImport("User32.dll")]
        public static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

        [DllImport("Shcore.dll")]
        public static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] MonitorDpiType dpiType,
            [Out] out uint dpiX, [Out] out uint dpiY);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool InitializeTouchInjection(int maxCount, TOUCH_FEEDBACK feedbackMode);


        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();


        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32")]
        public static extern int EnumWindows(WindowEnumProc x, int y);


        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute,
            out bool pvAttribute, int cbAttribute);

        /// <summary>
        /// 获取窗口title
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="title"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        /// <summary>
        /// 获取窗口文本长度
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);


        /// <summary>
        /// 返回创建指定窗口线程的标识和创建窗口的进程的标识符，后一项是可选的。
        /// </summary>
        /// <param name="hWnd">窗口句柄。</param>
        /// <param name="processId">返回进程id</param>
        /// <returns>创建窗口的线程id</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);


        #endregion DllImports

        #region window

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        /// <summary>
        /// 返回包含了指定点的窗口的句柄。忽略屏蔽、隐藏以及透明窗口
        /// </summary>
        /// <param name="point">指定的鼠标坐标</param>
        /// <returns>鼠标坐标处的窗口句柄，如果没有，返回</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(System.Drawing.Point point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 设置目标窗体大小，位置
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="x">目标窗体新位置X轴坐标</param>
        /// <param name="y">目标窗体新位置Y轴坐标</param>
        /// <param name="nWidth">目标窗体新宽度</param>
        /// <param name="nHeight">目标窗体新高度</param>
        /// <param name="isRefresh">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool isRefresh);

        #region ShowWindowAsync  ShowWindow

        #region nCmdShow参数

        /// <summary>
        /// 隐藏窗口并激活其他窗口
        /// </summary>
        public const int SW_HIDE = 0;

        /// <summary>
        /// 激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志
        /// </summary>
        public const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// 激活窗口并将其最小化
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;

        /// <summary>
        /// 激活窗口并将其最大化
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;

        /// <summary>
        /// 以窗口最近一次的大小和状态显示窗口。此值与SW_SHOWNORMAL相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

        /// <summary>
        /// 在窗口原来的位置以原来的尺寸激活和显示窗口
        /// </summary>
        public const int SW_SHOW = 5;

        /// <summary>
        /// 最小化指定的窗口并且激活在Z序中的下一个顶层窗口
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// 最小化的方式显示窗口，此值与SW_SHOWMINIMIZED相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;

        /// <summary>
        /// 以窗口原来的状态显示窗口。此值与SW_SHOW相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWNA = 8;

        /// <summary>
        /// 激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志
        /// </summary>
        public const int SW_RESTORE = 9;

        /// <summary>
        /// 依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的
        /// </summary>
        public const int SW_SHOWDEFAULT = 10;

        /// <summary>
        /// 最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;

        #endregion

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        #endregion

        #region setwindowpos

        /// <summary>
        /// 该函数改变一个子窗口，弹出式窗口式顶层窗口的尺寸，位置和Z序。子窗口，弹出式窗口，及顶层窗口根据它们在屏幕上出现的顺序排序、顶层窗口设置的级别最高，并且被设置为Z序的第一个窗口。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="hWndInsertAfter">在z序中的位于被置位的窗口前的窗口句柄。</param>
        /// <param name="x">以客户坐标指定窗口新位置的左边界。</param>
        /// <param name="y">以客户坐标指定窗口新位置的顶边界</param>
        /// <param name="cx">以像素指定窗口的新的宽度</param>
        /// <param name="cy">以像素指定窗口的新的高度</param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            SetWindowPosFlags uFlags);

        /// <summary>
        /// 在前面
        /// </summary>
        public const int HWND_TOP = 0;

        /// <summary>
        /// 在后面
        /// </summary>
        public const int HWND_BOTTOM = 1;

        /// <summary>
        /// 在前面, 位于任何顶部窗口的前面
        /// </summary>
        public const int HWND_TOPMOST = -1;

        /// <summary>
        /// 在前面, 位于其他顶部窗口的后面
        /// </summary>
        public const int HWND_NOTOPMOST = -2;

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>如果调用进程不拥有窗口，系统会向拥有窗口的线程发出需求。这就防止调用线程在其他线程处理需求的时候发生死锁</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            SynchronousWindowPosition = 0x4000,

            /// <summary>防止产生WM_SYNCPAINT消息</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,

            /// <summary>在窗口周围画一个边框（定义在窗口类描述中）</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,

            /// <summary>给窗口发送WM_NCCALCSIZE消息，即使窗口尺寸没有改变也会发送该消息。如果未指定这个标志，只有在改变了窗口尺寸时才发送WM_NCCALCSIZE</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,

            /// <summary>隐藏窗口</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,

            /// <summary>不激活窗口。如果未设置标志，则窗口被激活，并被设置到其他最高级窗口或非最高级组的顶部（根据参数hWndlnsertAfter设置）</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,

            /// <summary>清除客户区的所有内容。如果未设置该标志，客户区的有效内容被保存并且在窗口尺寸更新和重定位后拷贝回客户区</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,

            /// <summary>维持当前位置（忽略X和Y参数）</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,

            /// <summary>不改变z序中的所有者窗口的位置</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,

            /// <summary>不重画改变的内容。如果设置了这个标志，则不发生任何重画动作。适用于客户区和非客户区（包括标题栏和滚动条）和任何由于窗回移动而露出的父窗口的所有部分。如果设置了这个标志，应用程序必须明确地使窗口无效并区重画窗口的任何部分和父窗口需要重画的部分。</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,

            /// <summary>与SWP_NOOWNERZORDER标志相同</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,

            /// <summary>防止窗口接收WM_WINDOWPOSCHANGING消息</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,

            /// <summary>维持当前尺寸（忽略cx和Cy参数）</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,

            /// <summary>维持当前Z序（忽略hWndlnsertAfter参数）</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,

            /// <summary>显示窗口。</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040
        }

        #endregion

        /// <summary>
        /// 查找windows活动窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// 判断窗口是否可见
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        /// <summary>
        /// 获取窗口名称
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public static string GetWindowText(IntPtr wnd)
        {
            if (!IsWindowVisible(wnd)) return null;
            var length = GetWindowTextLength(wnd) + 1;
            var sb = new StringBuilder(length);
            GetWindowText(wnd, sb, length);
            var title = sb.ToString();
            if (string.IsNullOrWhiteSpace(title)) return null;
            return title;
        }

        #endregion

        #region 模拟鼠标

        [DllImport("user32", EntryPoint = "mouse_event")]
        public static extern int MouseEvent(int dwFlags, int dx, int dy, int dwData, uint dwExtraInfo);

        #endregion

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, uint lParam);


        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);


        public static bool HasSomeExtendedWindowsStyles(IntPtr hwnd)
        {
            const int GWL_EXSTYLE = -20;
            const uint WS_EX_TOOLWINDOW = 0x00000080U;

            var i = GetWindowLong(hwnd, GWL_EXSTYLE);
            if ((i & WS_EX_TOOLWINDOW) != 0) return true;

            return false;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);


        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hDestDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDc, int xSrc,
            int ySrc, int dwRop);


        private const string User32Dll = "user32.dll";

        /// <summary>
        /// Represents the different types of scaling.
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511.aspx"/>
        public enum MonitorDpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2
        }

        public const uint PM_REMOVE = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }


        [Flags]
        public enum SWP
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020, /* The frame changed: send WM_NCCALCSIZE */
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOOWNERZORDER = 0x0200, /* Don't do owner Z ordering */
            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_NOSTARTUP = 0x04000000,
            SWP_STARTUP = 0x08000000
        }

        [DllImport(User32Dll)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SWP uFlags);

        [DllImport(User32Dll)]
        public static extern bool UpdateWindow(IntPtr hwnd);


        [DllImport(User32Dll)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd,
            uint wMsgFilterMin, uint wMsgFilterMax,
            uint wRemoveMsg);

        [DllImport(User32Dll)]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport(User32Dll)]
        public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport(User32Dll)]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        //[StructLayout(LayoutKind.Sequential)]
        //public struct MSG
        //{
        //    public IntPtr hwnd;
        //    public UInt32 message;
        //    public IntPtr wParam;
        //    public IntPtr lParam;
        //    public UInt32 time;
        //    public Point pt;
        //}


        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public Int32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public Int32 biCompression;
            public Int32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public Int32 biClrUsed;
            public Int32 biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            /// <summary>
            /// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
            /// </summary>
            public BITMAPINFOHEADER bmiHeader;

            /// <summary>
            /// An array of RGBQUAD. The elements of the array that make up the color table.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public
                RGBQUAD[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }


        #region GDI

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFO pbmi,
            uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, IntPtr lpvObject);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport(User32Dll, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(User32Dll, ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(User32Dll)]
        public static extern bool UpdateLayeredWindowIndirect(IntPtr hwnd, ref UPDATELAYEREDWINDOWINFO pULWInfo);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(int cx, int cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT // : IEquatable<RECT>
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public void Shift(int x, int y)
            {
                Left += x;
                Right += x;
                Top += y;
                Bottom += y;
            }

            //public bool Equals(RECT other)
            //{
            //    return Left == other.Left &&
            //           Top == other.Top &&
            //           Right == other.Right &&
            //           Bottom == other.Bottom;
            //}

            //public static bool operator ==(RECT thiz, RECT other)
            //{
            //    return thiz.Equals(other);
            //}

            //public static bool operator !=(RECT thiz, RECT other)
            //{
            //    return !(thiz == other);
            //}

            public override string ToString()
            {
                return $"({Left}, {Top}, {Right}, {Bottom})";
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct UPDATELAYEREDWINDOWINFO
        {
            public uint cbSize;
            public IntPtr hdcDst;
            public unsafe Point* pptDst;
            public unsafe Size* psize;
            public IntPtr hdcSrc;
            public unsafe Point* pptSrc;
            public uint crKey;
            public unsafe BLENDFUNCTION* pblend;
            public uint dwFlags;
            public unsafe RECT* prcDirty;
        }

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DiGetClassFlags : uint
        {
            DIGCF_PRESENT = 0x00000002,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }


        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private IntPtr reserved;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        public struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public UInt16 VendorID;
            public UInt16 ProductID;
            public UInt16 VersionNumber;
        }

        //------------------------------------------------------------------------------------------------------

        [DllImport("SetupApi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("Setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            out uint requiredSize,
            SP_DEVINFO_DATA deviceInfoData);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        public static extern void HidD_GetHidGuid(out Guid ClassGuid);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        public static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            //UInt32 fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            //[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            uint flagsAndAttributes,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ReadFileEx(
            SafeFileHandle hFile,
            [Out] byte[] lpbuffer,
            [In] uint nNumberOfBytesToRead,
            [In, Out] ref NativeOverlapped lpOverlapped,
            IntPtr lpCompletionRoutine);

        // 定义 GetRawInputDeviceList 函数
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceList([Out] RAWINPUTDEVICELIST[] pRawInputDeviceList, out uint puiNumDevices, uint cbSize);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, IntPtr lpProcName);
        // 定义LoadLibrary函数的P/Invoke签名
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        // 定义FreeLibrary函数的P/Invoke签名
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);
        // 定义NtUserSetDisplayMapping函数的委托类型
        delegate void NtUserSetDisplayMappingDelegate(IntPtr touch, IntPtr display);

        /// <summary>
        /// 设置显示匹配
        /// </summary>
        /// <param name="deviceHwnd">设备句柄</param>
        /// <param name="displayHwnd">显示句柄</param>
        public static void SetDisplayMapping(IntPtr deviceHwnd, IntPtr displayHwnd)
        {
            IntPtr user32Handle = LoadLibrary("user32.dll");
            try
            {
                IntPtr functionPointer = GetProcAddress(user32Handle, (IntPtr)2532);
                NtUserSetDisplayMappingDelegate funcDelegate =
                    Marshal.GetDelegateForFunctionPointer<NtUserSetDisplayMappingDelegate>(functionPointer);
                funcDelegate(deviceHwnd, displayHwnd);
            }
            finally
            {
                FreeLibrary(user32Handle);
            }
        }
    }
}
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoveTouchAndPenToExtendScreen.Native;
using static MoveTouchAndPenToExtendScreen.Native.NativeMethods;
namespace MoveTouchAndPenToExtendScreen
{
    /// <summary>
    /// 触控设备帮助类
    /// </summary>
    internal class TouchDeviceHelper
    {

        /// <summary>
        /// 获取触控设备名称
        /// </summary>
        /// <returns></returns>
        public static string GetTouchDeviceName()
        {
            return GetDeviceName(0x04);
        }

        /// <summary>
        /// 获取笔设备名称
        /// </summary>
        /// <returns></returns>
        public static string GetPenDeviceName()
        {
            return GetDeviceName(0x02);
        }

        /// <summary>
        /// 纠正设备名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string AlterDeviceName(string name)
        {
            return $"20-{name}";
        }

        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <param name="usage"></param>
        /// <returns></returns>
        private static string GetDeviceName(int usage)
        {
            HidD_GetHidGuid(out var hidGuid);

            IntPtr pnPHandle = SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero,
                (int)(DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE));
            if (pnPHandle == (IntPtr)INVALID_HANDLE_VALUE)
            {
                // LoggingService.LogError("Connect: SetupDiGetClassDevs failed.");
                return string.Empty;
            }

            //we coudld use a lot more failure and exception logging during this loop
            Boolean bFoundADevice = false;
            Boolean bFoundMyDevice = false;
            uint i = 0;
            do
            {
                SP_DEVICE_INTERFACE_DATA devInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                devInterfaceData.cbSize = (uint)Marshal.SizeOf(devInterfaceData);
                bFoundADevice =
                    SetupDiEnumDeviceInterfaces(pnPHandle, IntPtr.Zero, ref hidGuid, i, ref devInterfaceData);
                if (bFoundADevice)
                {
                    SP_DEVINFO_DATA devInfoData = new SP_DEVINFO_DATA();
                    devInfoData.cbSize = (uint)Marshal.SizeOf(devInfoData);
                    uint needed;
                    bool result3 = SetupDiGetDeviceInterfaceDetail(pnPHandle, devInterfaceData, IntPtr.Zero, 0,
                        out needed, devInfoData);
                    if (!result3)
                    {
                        int error = Marshal.GetLastWin32Error();
                        if (error == 122)
                        {
                            //it's supposed to give an error 122 as we just only retrieved the data size needed, so this is as designed
                            IntPtr deviceInterfaceDetailData = Marshal.AllocHGlobal((int)needed);
                            try
                            {
                                uint size = needed;
                                Marshal.WriteInt32(deviceInterfaceDetailData, IntPtr.Size == 8 ? 8 : 6);
                                bool result4 = SetupDiGetDeviceInterfaceDetail(pnPHandle, devInterfaceData,
                                    deviceInterfaceDetailData, size, out needed, devInfoData);
                                if (!result4)
                                {
                                    //shouldn't be an error here
                                    int error1 = Marshal.GetLastWin32Error();
                                    //todo: go +1 and contine the loop...this exception handing is incomplete
                                }

                                IntPtr pDevicePathName = new IntPtr(deviceInterfaceDetailData.ToInt64() + 4);
                                var fDevicePathName = Marshal.PtrToStringAuto(pDevicePathName);
                                //see if this driver has readwrite access
                                var fDevHandle = CreateFile(fDevicePathName, System.IO.FileAccess.ReadWrite,
                                    System.IO.FileShare.ReadWrite, IntPtr.Zero, System.IO.FileMode.Open, 0,
                                    IntPtr.Zero);
                                if (fDevHandle.IsInvalid)
                                {
                                    fDevHandle = CreateFile(fDevicePathName, 0, System.IO.FileShare.ReadWrite,
                                        IntPtr.Zero, System.IO.FileMode.Open, 0, IntPtr.Zero);
                                }

                                if (!fDevHandle.IsInvalid)
                                {
                                    IntPtr preparsedPtr = default;
                                    HidNativeApi.HidD_GetPreparsedData(fDevHandle, ref preparsedPtr);
                                    HidNativeApi.HIDP_CAPS caps = default;
                                    HidNativeApi.HidP_GetCaps(preparsedPtr, ref caps);
                                    fDevHandle.Close();
                                    if (caps.Usage == usage && caps.UsagePage is 0x0d)
                                    {
                                        return AlterDeviceName(fDevicePathName);
                                    }

                                }
                            }
                            finally
                            {
                                Marshal.FreeHGlobal(deviceInterfaceDetailData);
                            }
                        }
                    }
                }

                i++;
            } while ((bFoundADevice) & (!bFoundMyDevice));

            return string.Empty;
        }
        /// <summary>
        /// 获取触控屏设备
        /// </summary>
        /// <param name="isTouch">true:触控  false：笔</param>
        /// <returns></returns>
        public static IntPtr GetTouchDeviceHandle(bool isTouch)
        {
            var usage = isTouch
                ? Native.NativeMethods.TouchScreenUsage//0x04
                : Native.NativeMethods.PenUsage;//0x02
            uint count = 0;
            NativeMethods.GetRawInputDeviceList(null, out count, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));

            RAWINPUTDEVICELIST[] list = new RAWINPUTDEVICELIST[count];
            if (GetRawInputDeviceList(list, out count, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST))) != 0)
            {
                foreach (RAWINPUTDEVICELIST device in list)
                {
                    if (ValidateDevice(device.hDevice,usage))
                    {
                        return device.hDevice;
                    }
                }
            }

            return IntPtr.Zero;
        }
        /// <summary>
        /// 验证设备是否有效
        /// </summary>
        /// <param name="hDevice"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        private static bool ValidateDevice(IntPtr hDevice,ushort usage)
        {
            uint pcbSize = 0;
            Native.NativeMethods.GetRawInputDeviceInfo(hDevice, Native.NativeMethods.RIDI_DEVICEINFO, IntPtr.Zero, ref pcbSize);
            if (pcbSize <= 0)
                return false;

            IntPtr pInfo = Marshal.AllocHGlobal((int)pcbSize);
            using (new SafeUnmanagedMemoryHandle(pInfo))
            {
                Native.NativeMethods.GetRawInputDeviceInfo(hDevice, Native.NativeMethods.RIDI_DEVICEINFO, pInfo, ref pcbSize);
                var info = (RID_DEVICE_INFO)Marshal.PtrToStructure(pInfo, typeof(RID_DEVICE_INFO));
                return info.hid.usUsage == usage;
            }
        }
    }
}

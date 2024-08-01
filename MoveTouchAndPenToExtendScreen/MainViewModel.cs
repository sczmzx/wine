using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using MoveTouchAndPenToExtendScreen.Native;
using Mvvm.Common;
using Mvvm.Common.Commands;
using WindowsDisplayAPI;
using WindowsDisplayAPI.DisplayConfig;

namespace MoveTouchAndPenToExtendScreen
{
    public class MainViewModel : ViewModelBase, IViewModel,IModelLoaded
    {
        /// <summary>
        /// 设置触控主屏
        /// </summary>
        public ICommand SetMainCommand { get; set; }


        /// <summary>
        /// 设置触控扩展屏
        /// </summary>
        public ICommand SetExtendCommand { get; set; }


        public MainViewModel()
        {
            SetMainCommand = new DelegateCommand(SetMain);
            SetExtendCommand = new DelegateCommand(SetExtend);
        }


        /// <summary>
        /// 设置扩展屏
        /// </summary>
        private void SetExtend()
        {
            //设置扩展屏时，需要分别将触控和笔设备映射到扩展屏，并发送通知
            SetMapping(false);
        }


        /// <summary>
        /// 设置主屏
        /// </summary>
        private void SetMain()
        {
            //设置为主屏时，先移除配置，再发送改变通知即可
            SetMapping(true);
        }

        /// <summary>
        /// 设置匹配
        /// </summary>
        /// <param name="isPrimary"></param>
        private void SetMapping(bool isPrimary)
        {
            SetTouchDeviceNameMapping(isPrimary);
            SetDisplayMapping(isPrimary);
        }

        /// <summary>
        /// 设置触控设备匹配
        /// </summary>
        /// <param name="isPrimary"></param>
        private void SetTouchDeviceNameMapping(bool isPrimary)
        {
            ClearRegistry();
            //主屏设备直接清空注册表就可以了，不设置默认映射到主屏
            if (isPrimary) return;
            var displayName = GetScreenDevicePath(false);
            foreach (var deviceName in GetTouchDeviceNames())
            {
                SetRegistry(deviceName, displayName);
            }
        }

        /// <summary>
        /// 获取触控设备的名称列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetTouchDeviceNames()
        {
            yield return GetTouchDeviceName(true);
            yield return GetTouchDeviceName(false);
        }

        /// <summary>
        /// 清理注册表设置
        /// </summary>
        private void ClearRegistry()
        {
            Registry.LocalMachine.DeleteSubKey("SOFTWARE\\Microsoft\\Wisp\\Pen\\Digimon",false);
        }

        /// <summary>
        /// 设置注册表
        /// </summary>
        /// <param name="touchDeviceName"></param>
        /// <param name="displayDeviceName"></param>
        private void SetRegistry(string touchDeviceName, string displayDeviceName)
        {
            var key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Wisp\\Pen\\Digimon");
            key.SetValue(touchDeviceName, displayDeviceName);
        }

        /// <summary>
        /// 设置显示匹配
        /// </summary>
        /// <param name="isPrimary"></param>
        private void SetDisplayMapping(bool isPrimary)
        {
            SetDisplayMapping(true, isPrimary);
            SetDisplayMapping(false, isPrimary);
        }

        /// <summary>
        /// 设置显示匹配同时
        /// </summary>
        /// <param name="isTouch">是否触控 true：触控  false:笔</param>
        /// <param name="isPrimary">是否主屏</param>
        /// <returns></returns>
        private void SetDisplayMapping(bool isTouch, bool isPrimary)
        {
            var deviceHwnd = GetTouchDeviceHwnd(isTouch);
            var displayHwnd = GetScreenDeviceHwnd(isPrimary);
            NativeMethods.SetDisplayMapping(deviceHwnd,displayHwnd);
        }

        /// <summary>
        /// 获取设备句柄
        /// </summary>
        /// <param name="isTouch"></param>
        /// <returns></returns>
        private IntPtr GetTouchDeviceHwnd(bool isTouch)
        {
            return TouchDeviceHelper.GetTouchDeviceHandle(isTouch);
        }

        /// <summary>
        /// 获取设备名称
        /// </summary>
        /// <param name="isTouch"></param>
        /// <returns></returns>
        private string GetTouchDeviceName(bool isTouch)
        {
            if (isTouch) return TouchDeviceHelper.GetTouchDeviceName();
            return TouchDeviceHelper.GetPenDeviceName();
        }

        /// <summary>
        /// 获取屏幕设备路径
        /// </summary>
        /// <param name="isPrimary">是否主屏</param>
        /// <returns></returns>
        private string GetScreenDevicePath(bool isPrimary)
        {
            foreach (var display in Display.GetDisplays())
            {
                if (display.IsGDIPrimary == isPrimary) return display.DevicePath;
            }
            return null;
        }

        /// <summary>
        /// 获取屏幕设备句柄
        /// </summary>
        /// <param name="isMain"></param>
        /// <returns></returns>
        private IntPtr GetScreenDeviceHwnd(bool isMain)
        {
            return ScreenHelper.GetDisplayDeviceHandle(isMain);
        }

        /// <summary>
        /// load
        /// </summary>
        public void Loaded()
        {
            Debug.WriteLine(TouchDeviceHelper.GetPenDeviceName());
            Debug.WriteLine(TouchDeviceHelper.GetTouchDeviceName());
        }
    }
}

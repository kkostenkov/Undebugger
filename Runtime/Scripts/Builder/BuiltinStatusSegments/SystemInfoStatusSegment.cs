﻿using Undebugger.Model.Status;
using Undebugger.UI;
using UnityEngine;

namespace Undebugger.Builder.BuiltinStatusSegments
{
    internal class SystemInfoStatusSegment : StaticStatusSegmentDriver
    {
        private const string LabelColor = "#A2A2A2";

        public static SystemInfoStatusSegment Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemInfoStatusSegment();
                }

                return instance;
            }
        }

        private static SystemInfoStatusSegment instance;

        public override int Priority
        { get { return -9000; } }

        public SystemInfoStatusSegment()
            : base("system_info", "System", GenerateString())
        { }

        private static string GenerateString()
        {
            return
$@"<color={LabelColor}>OS:</color> ({SystemInfo.operatingSystemFamily}) {SystemInfo.operatingSystem}
<color={LabelColor}>CPU Type:</color> {SystemInfo.processorType}
<color={LabelColor}>CPU Count:</color> {SystemInfo.processorCount}
<color={LabelColor}>System Memory:</color> {UIUtility.ConvertMegabyteSizeToReadableString(SystemInfo.systemMemorySize)}
<color={LabelColor}>GPU Vendor:</color> {SystemInfo.graphicsDeviceVendor} (0x{SystemInfo.graphicsDeviceVendorID:X})
<color={LabelColor}>GPU:</color> {SystemInfo.graphicsDeviceName}
<color={LabelColor}>Graphics Memory:</color> {UIUtility.ConvertMegabyteSizeToReadableString(SystemInfo.graphicsMemorySize)}
<color={LabelColor}>Graphics API:</color> {SystemInfo.graphicsDeviceVersion}
<color={LabelColor}>Display Resolution:</color> {Screen.currentResolution}
<color={LabelColor}>DPI:</color> {Screen.dpi}";
        }
    }
}

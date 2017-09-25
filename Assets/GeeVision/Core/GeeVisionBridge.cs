using System.Runtime.InteropServices;
using UnityEngine;

namespace GeeVision.Core
{
    public class GeeVisionBridge
    {
#if UNITY_STANDALONE_WIN

        private const string GeeVisionDLL = "GeeVisionCore";

        [DllImport(GeeVisionDLL)]
        public static extern int Mars_Initialize(ref DataListener listener);

        [DllImport(GeeVisionDLL)]
        public static extern void Mars_Start();

        [DllImport(GeeVisionDLL)]
        public static extern void Mars_Stop();

        [DllImport(GeeVisionDLL)]
        public static extern void Mars_Destroy();

#elif UNITY_ANDROID

        private const string GeeVisionLibrary = "GeeVision";

        [DllImport(GeeVisionLibrary)]
        public static extern void GeeVisionAndroid_Initialize(ref DataListener listener);

        [DllImport(GeeVisionLibrary)]
        public static extern void GeeVisionAndroid_Destroy();

#endif
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FrameInfo
    {
        public HandInfo LeftHand;
        public HandInfo RightHand;

        public static FrameInfo Create()
        {
            FrameInfo frameInfo = new FrameInfo
            {
                LeftHand = new HandInfo { Joints = new JointInfo[22] },
                RightHand = new HandInfo { Joints = new JointInfo[22] }
            };
            return frameInfo;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HandInfo
    {
        public int TypeID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public JointInfo[] Joints;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JointInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NodeInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveFrame(ref FrameInfo frameInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveInstant(int whichHand, int typeID, float confidence);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveRealTimeTrack(int type, float x, float y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnReceiveTrack(int type);

    [StructLayout(LayoutKind.Sequential)]
    public struct DataListener
    {
        public OnReceiveFrame OnReceiveFrame;
        public OnReceiveInstant OnReceiveInstant;
        public OnReceiveRealTimeTrack OnReceiveRealTimeTrack;
        public OnReceiveTrack OnReceiveTrack;

        public DataListener(OnReceiveFrame onReceiveFrame, OnReceiveInstant onReceiveInstant,
            OnReceiveRealTimeTrack onReceiveRealTimeTrack, OnReceiveTrack onReceiveTrack)
        {
            OnReceiveFrame = onReceiveFrame;
            OnReceiveInstant = onReceiveInstant;
            OnReceiveRealTimeTrack = onReceiveRealTimeTrack;
            OnReceiveTrack = onReceiveTrack;
        }
    }
}

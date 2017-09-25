using System.Collections.Generic;
using UnityEngine;

namespace GeeVision.Core
{
    public enum RealTimeGestureType
    {
        Off,
        Stay,
        Start,
        Move
    }

    public enum StaticGestureType : int
    {
        None = 0,
        ArrowLeft = 5,
        ArrowRight = 6,
    }

    public interface IGeeVisionListener
    {
        void OnReceiveFrame(ref FrameInfo frameInfo);
        void OnReceiveInstant(int whichHand, int typeID, float cofidence);
        void OnReceiveRealTimeTrack(int type, float x, float y);
        void OnReceiveTrack(int type);
    }

    public class GeeVisionListener : IGeeVisionListener
    {
        public static GeeVisionListener Instance = new GeeVisionListener();
        
        public DataListener DataListener;
        public FrameInfo FrameInfo = FrameInfo.Create();

        public RealTimeGestureType RealTimeGestureType;

        public StaticGestureType StaticGestureType;

        public Vector2 LastPoint;

        protected GeeVisionListener()
        {
            DataListener = new DataListener(OnReceiveFrame, OnReceiveInstant, OnReceiveRealTimeTrack, OnReceiveTrack);
        }

        public void OnReceiveFrame(ref FrameInfo frameInfo)
        {

        }
        
        public void OnReceiveInstant(int whichHand, int typeID, float cofidence)
        {
            if (whichHand == 1)
            {
                FrameInfo.LeftHand.TypeID = typeID;
            }
            else if (whichHand == 2)
            {
                FrameInfo.RightHand.TypeID = typeID;
            }
        }

        public void OnReceiveRealTimeTrack(int type, float x, float y)
        {
            RealTimeGestureType = (RealTimeGestureType) type;
            LastPoint = new Vector2(x, y);
        }
        
        public void OnReceiveTrack(int type)
        {
            StaticGestureType = (StaticGestureType) type;
        }
    }

    public interface IGeeVisionPathTrackListener
    {
        
    }

    public class GeeVisionPathTrackObserver : MonoBehaviour, IGeeVisionPathTrackListener
    {
        public static GeeVisionPathTrackObserver Instance;
        protected List<IGeeVisionPathTrackListener> Listeners;
        public void Awake()
        {
            Instance = this;
            Listeners = new List<IGeeVisionPathTrackListener>();
        }

        public void Update()
        {
            
        }

        public void AddListener(IGeeVisionPathTrackListener listener)
        {
            if (!Listeners.Contains(listener)) Listeners.Add(listener);
        }
    }
}

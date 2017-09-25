using UnityEngine;
using UnityEngine.UI;

namespace GeeVision.Core
{
    public class GeeVisionManager : MonoBehaviour
    {
        public static GeeVisionManager Instance { private set; get; }
        private GeeVisionListener _listener;

#if UNITY_ANDROID
        
        public AndroidJavaObject GeeVision;

#endif

        public Text StatusText;
        protected void Awake()
        {
            Instance = this;
            _listener = GeeVisionListener.Instance;
            
#if UNITY_STANDALONE_WIN
            
            int status = GeeVisionBridge.Mars_Initialize(ref _listener.DataListener);
            if (status == 0)
            {
                GeeVisionBridge.Mars_Start();
            }

#elif UNITY_ANDROID
            
            GeeVision = new AndroidJavaObject("com.geevision.deepfish.geevisionandroid.GeeVisionBridge",
                new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
            status = 1;
            GeeVisionBridge.GeeVisionAndroid_Initialize(ref _listener.DataListener);
            status = 2;
#endif
            StatusText.text = status.ToString();
        }

        protected void OnDestroy()
        {
#if UNITY_STANDALONE_WIN

            GeeVisionBridge.Mars_Destroy();

#elif UNITY_ANDROID

            GeeVision.Call("Destroy");
            GeeVisionBridge.GeeVisionAndroid_Destroy();

#endif
        }
    }
}

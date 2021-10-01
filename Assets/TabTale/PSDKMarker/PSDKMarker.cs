using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class PSDKMarker : MonoBehaviour
{
#if UNITY_IOS

    [DllImport("__Internal")]
    private static extern void psdkMarkFirstRun();

#endif

    void Start()
    {

#if UNITY_IOS
        psdkMarkFirstRun();
#elif UNITY_ANDROID
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        if(unityPlayerClass != null){
            AndroidJavaObject currentActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            if(currentActivityObject != null){
                AndroidJavaClass psdkMarkerClass = new AndroidJavaClass("com.tabtale.psdkmarker.PSDKMarker");
                if (psdkMarkerClass != null)
                {
                    psdkMarkerClass.CallStatic("markAppWasRun", new object[] { currentActivityObject });
                }
                else
                {
                    Debug.LogError("PSDKMarker:: couldnt not find PSDKMarker class");
                }
            }
            else {
//                Debug.LogError("PSDKMarker:: couldnt not find currentActivityObject");
            }
        }
        else {
            Debug.LogError("PSDKMarker:: couldnt not find unityPlayerClass");
        }
#endif
    }
	
}

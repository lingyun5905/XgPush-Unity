using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class PushManager : MonoBehaviour, IPush
{
    private static IPush m_Instance;
    private Action<bool, string> onStartFinish;
    private Action<bool, string> onStopFinish;

    public static IPush Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject("[PushManager]").AddComponent<PushManager>();
            }
            return m_Instance;
        }
    }

    protected void Awake()
    {

        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(this.gameObject);
#if UNITY_IOS
        __PushUnity3DBridge_init(this.gameObject.name, "OnCallBack");
#elif UNITY_ANDROID
        _androidPushManager.Call("init", this.gameObject.name, "OnCallBack");
#endif
    }


    private class MethodInfo
    {
        public string method;
        public bool success;
        public string msg;
    }

    protected void OnCallBack(string data)
    {
        var methodInfo = JsonUtility.FromJson<MethodInfo>(data);
        if (methodInfo.method == "OnStartFinish")
        {
            if (onStartFinish != null)
            {
                onStartFinish(methodInfo.success, methodInfo.msg);
            }
        }
        else if (methodInfo.method == "OnStopFinish")
        {
            if (onStopFinish != null)
            {
                onStopFinish(methodInfo.success, methodInfo.msg);
            }
        }
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_init(string gameobject, string callback);

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_enableDebug(bool enable);

    [DllImport("__Internal")]
    private static extern string __PushUnity3DBridge_getToken();

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_start(long appId, string appKey);

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_stop();

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_bindAccount(string account);

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_unbindAccount(string account);

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_bindTag(string tag);

    [DllImport("__Internal")]
    private static extern void __PushUnity3DBridge_unbindTag(string tag);

    private long m_AccessId;
    private string m_AccessKey;

    private PushManager()
    {
    }

    public void SetAccessId(long accessId)
    {
        m_AccessId = accessId;
    }

    public long GetAccessId()
    {
        return m_AccessId;
    }

    public void SetAccessKey(string accessKey)
    {
        m_AccessKey = accessKey;
    }

    public string GetAccessKey()
    {
        return m_AccessKey;
    }

    public string GetToken()
    {
        return __PushUnity3DBridge_getToken();
    }

    public void EnableDebug(bool isEnable)
    {
        __PushUnity3DBridge_enableDebug(isEnable);
    }

    public void StartPushService(Action<bool, string> onFinish = null)
    {
        onStartFinish = onFinish;
        __PushUnity3DBridge_start(m_AccessId, m_AccessKey);
    }

    public void StopPushService(Action<bool, string> onFinish = null)
    {
        onStopFinish = onFinish;
        __PushUnity3DBridge_stop();
    }

    public void BindAccount(string account)
    {
        __PushUnity3DBridge_bindAccount(account);
    }

    public void UnBindAccount(string account)
    {
        __PushUnity3DBridge_unbindAccount(account);
    }

    public void BindTag(string tagStr)
    {
        __PushUnity3DBridge_bindTag(tagStr);
    }

    public void UnBindTag(string tagStr)
    {
        __PushUnity3DBridge_unbindTag(tagStr);
    }

#elif UNITY_ANDROID
    private static AndroidJavaObject _androidPushManager;

    private PushManager()
    {
        try
        {
            var ajc = new AndroidJavaClass("com.hk.sdk.push.PushManager");
            _androidPushManager = ajc.CallStatic<AndroidJavaObject>("getInstance");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SetAccessId(long accessId)
    {
        _androidPushManager.Call("setAccessId", accessId);
    }

    public long GetAccessId()
    {
        return _androidPushManager.Call<long>("getAccessId");
    }

    public void SetAccessKey(string accessKey)
    {
        _androidPushManager.Call("setAccessKey", accessKey);
    }

    public string GetAccessKey()
    {
        return _androidPushManager.Call<string>("getAccessKey");
    }

    public void SetMiPushAppId(string appId)
    {
        _androidPushManager.Call("setMiPushAppId", appId);
    }

    public void SetMiPushAppKey(string appkey)
    {
        _androidPushManager.Call("setMiPushAppKey", appkey);
    }

    public void SetMzPushAppId(string appId)
    {
        _androidPushManager.Call("setMzPushAppId", appId);
    }

    public void SetMzPushAppKey(string appKey)
    {
        _androidPushManager.Call("setMzPushAppKey", appKey);
    }

    public void SetInstallChannel(string channel)
    {
        _androidPushManager.Call("setInstallChannel", channel);
    }

    public string GetInstallChannel()
    {
        return _androidPushManager.Call<string>("getInstallChannel");
    }

    public void SetHuaweiDebug(bool isDebug)
    {
        _androidPushManager.Call("setHuaweiDebug", isDebug);
    }

    public void EnableDebug(bool isEnable)
    {
        _androidPushManager.Call("enableDebug", isEnable);
    }

    public string GetToken()
    {
        return _androidPushManager.Call<string>("getToken");
    }

    public void StartPushService(Action<bool, string> onFinish = null)
    {
        onStartFinish = onFinish;
        _androidPushManager.Call("registerPush");
    }

    public void StopPushService(Action<bool, string> onFinish = null)
    {
        onStopFinish = onFinish;
        _androidPushManager.Call("unregisterPush");
    }

    public void BindAccount(string account)
    {
        _androidPushManager.Call("bindAccount", account);
    }

    public void UnBindAccount(string account)
    {
        _androidPushManager.Call("delAccount", account);
    }

    public void BindTag(string tag)
    {
        _androidPushManager.Call("setTag", tag);
    }

    public void UnBindTag(string tag)
    {
        _androidPushManager.Call("deleteTag", tag);
    }
#endif
}

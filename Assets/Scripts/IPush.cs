
using System;

public interface IPush
{
    /// <summary>
    /// 设置accessId
    /// </summary>
    /// <param name="accessId"></param>
    void SetAccessId(long accessId);

    /// <summary>
    /// 获得accessId
    /// </summary>
    /// <returns></returns>
    long GetAccessId();

    /// <summary>
    /// 设置accessKey
    /// </summary>
    /// <param name="accessKey"></param>
    void SetAccessKey(string accessKey);

    /// <summary>
    /// 获得accessKey
    /// </summary>
    /// <returns></returns>
    string GetAccessKey();

    /// <summary>
    /// 是否开启调试
    /// </summary>
    /// <param name="isEnable"></param>
    void EnableDebug(bool isEnable);

    /// <summary>
    /// 获取token
    /// </summary>
    /// <returns></returns>
    string GetToken();

    /// <summary>
    /// 注册开启推送
    /// </summary>
    void StartPushService(Action<bool, string> onFinish = null);

    /// <summary>
    /// 反注册关闭推送
    /// </summary>
    void StopPushService(Action<bool, string> onFinish = null);

    /// <summary>
    /// 绑定账户
    /// </summary>
    /// <param name="account"></param>
    void BindAccount(string account);

    /// <summary>
    /// 解绑账户
    /// </summary>
    /// <param name="account"></param>
    void UnBindAccount(string account);

    /// <summary>
    /// 绑定标签
    /// </summary>
    /// <param name="tag"></param>
    void BindTag(string tag);

    /// <summary>
    /// 解绑标签
    /// </summary>
    /// <param name="tag"></param>
    void UnBindTag(string tag);

#if UNITY_ANDROID

    /// <summary>
    /// 设置小米推送appId
    /// </summary>
    /// <param name="appId"></param>
    void SetMiPushAppId(string appId);

    /// <summary>
    /// 设置小米推送appKey
    /// </summary>
    /// <param name="appkey"></param>
    void SetMiPushAppKey(string appkey);

    /// <summary>
    /// 设置魅族推送AppId
    /// </summary>
    /// <param name="appId"></param>
    void SetMzPushAppId(string appId);

    /// <summary>
    /// 设置魅族推送appkey
    /// </summary>
    /// <param name="appKey"></param>
    void SetMzPushAppKey(string appKey);

    /// <summary>
    /// 设置安装渠道
    /// </summary>
    /// <param name="channel"></param>
    void SetInstallChannel(string channel);

    /// <summary>
    /// 获得安装渠道
    /// </summary>
    /// <returns></returns>
    string GetInstallChannel();

    /// <summary>
    /// 设置是否开启华为调试
    /// </summary>
    /// <param name="isDebug"></param>
    void SetHuaweiDebug(bool isDebug);
#endif
}

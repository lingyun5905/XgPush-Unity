using UnityEngine;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PushManager.Instance.EnableDebug(true);
		PushManager.Instance.SetAccessId(2100279402);
		PushManager.Instance.SetAccessKey("A8K5A7NFQ38J");
		PushManager.Instance.StartPushService(delegate(bool success, string msg)
		{
		    if (success)
		    {
                Debug.Log(PushManager.Instance.GetToken());
            }
		});
    }
}

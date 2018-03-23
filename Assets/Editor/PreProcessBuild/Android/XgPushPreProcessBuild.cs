#if UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class XgPushPreProcessBuild : IPreprocessBuild
{
    public int callbackOrder
    {
        get { return 1; }
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        string manifest = Path.Combine(Application.dataPath, "Plugins/Android/XgPush/AndroidManifest.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(manifest);

        XmlElement actionNode = AndroidManifestHelper.SearchNode(doc, "manifest/application/service/intent-filter/action");
        actionNode.SetAttribute("android:name", Application.identifier + ".PUSH_ACTION");

        XmlElement provider1 = AndroidManifestHelper.SearchNode(doc, "manifest/application/provider",
            new KeyValuePair<string, string>("android:name", "com.tencent.android.tpush.XGPushProvider"));
        provider1.SetAttribute("android:authorities", Application.identifier + ".AUTH_XGPUSH");

        XmlElement provider2 = AndroidManifestHelper.SearchNode(doc, "manifest/application/provider",
            new KeyValuePair<string, string>("android:name", "com.tencent.android.tpush.SettingsContentProvider"));
        provider2.SetAttribute("android:authorities", Application.identifier + ".TPUSH_PROVIDER");

        XmlElement provider3 = AndroidManifestHelper.SearchNode(doc, "manifest/application/provider",
            new KeyValuePair<string, string>("android:name", "com.tencent.mid.api.MidProvider"));
        provider3.SetAttribute("android:authorities", Application.identifier + ".TENCENT.MID.V3");

        doc.Save(manifest);
    }
}
#endif
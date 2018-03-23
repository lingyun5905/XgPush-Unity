#if UNITY_IPHONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class XgPushPostProcessBuild {

	[PostProcessBuildAttribute(100)]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
	{
		// BuildTarget需为iOS
		if (buildTarget != BuildTarget.iOS)
			return;
		ModifyXCodeProj (pathToBuiltProject);
	}

	/// <summary>
	/// 修改Xcode proj
	/// </summary>
	/// <param name="pathToBuiltProject"></param>
	static void ModifyXCodeProj(string pathToBuiltProject)
	{
		new XCodeProj (pathToBuiltProject)
			.AddFramework ("CoreTelephony.framework")
			.AddFramework ("SystemConfiguration.framework")
			.AddFramework ("UserNotifications.framework")
            .AddFramework ("libz.tbd")
            .AddFramework ("libsqlite3.tbd")
			.AddBuildProperty ("OTHER_LDFLAGS", "-ObjC")
			.Save ();
	}
}
#endif
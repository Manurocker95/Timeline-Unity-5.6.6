using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200004E RID: 78
	internal interface IClipSceneView
	{
		// Token: 0x060002C1 RID: 705
		bool OnSceneGUI(TimelineClip clip, SceneView sceneView, GameObject sceneReference);
	}
}

using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A7 RID: 167
	[DisplayName("Find Source Asset")]
	internal class FindSourceAsset : ClipAction
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x0002AC84 File Offset: 0x00029084
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			MenuActionDisplayState result;
			if (clips.Length > 1)
			{
				result = MenuActionDisplayState.Disabled;
			}
			else if (clips[0].underlyingAsset == null || clips[0].underlyingAsset is TimelineAsset)
			{
				result = MenuActionDisplayState.Disabled;
			}
			else
			{
				string assetPath = AssetDatabase.GetAssetPath(clips[0].underlyingAsset);
				result = ((assetPath == null || assetPath.Length <= 0) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible);
			}
			return result;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002ACFC File Offset: 0x000290FC
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			EditorGUIUtility.PingObject(clips[0].underlyingAsset);
			return true;
		}
	}
}

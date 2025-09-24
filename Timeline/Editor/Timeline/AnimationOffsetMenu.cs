using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000024 RID: 36
	internal static class AnimationOffsetMenu
	{
		// Token: 0x06000176 RID: 374 RVA: 0x0000EBC0 File Offset: 0x0000CFC0
		private static bool EnforcePreviewMode(ITimelineState state)
		{
			state.previewMode = true;
			bool result;
			if (!state.previewMode)
			{
				Debug.LogError("Match clips cannot be completed because preview mode cannot be enabed");
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000EBFC File Offset: 0x0000CFFC
		private static void MatchClipsToPrevious(ITimelineState state, TimelineClip[] clips)
		{
			if (AnimationOffsetMenu.EnforcePreviewMode(state))
			{
				foreach (TimelineClip timelineClip in clips)
				{
					TimelineHelpers.PushUndo(timelineClip.asset, "match.clip");
					GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(state.currentDirector, timelineClip.parentTrack);
					TimelineAnimationUtilities.MatchPrevious(timelineClip, sceneGameObject.transform, state.currentDirector);
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000EC6C File Offset: 0x0000D06C
		private static void MatchClipsToNext(ITimelineState state, TimelineClip[] clips)
		{
			if (AnimationOffsetMenu.EnforcePreviewMode(state))
			{
				foreach (TimelineClip timelineClip in clips)
				{
					TimelineHelpers.PushUndo(timelineClip.asset, "match.clip");
					GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(state.currentDirector, timelineClip.parentTrack);
					TimelineAnimationUtilities.MatchNext(timelineClip, sceneGameObject.transform, state.currentDirector);
				}
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000ECDC File Offset: 0x0000D0DC
		private static void ResetClipOffsets(ITimelineState state, TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (timelineClip.asset is AnimationPlayableAsset)
				{
					AnimationPlayableAsset animationPlayableAsset = (AnimationPlayableAsset)timelineClip.asset;
					animationPlayableAsset.ResetOffsets();
				}
			}
			state.rebuildGraph = true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000ED34 File Offset: 0x0000D134
		public static void OnClipMenu(ITimelineState state, TimelineClip[] clips, GenericMenu menu)
		{
			if (!(state.currentDirector == null))
			{
				TimelineClip[] array = (from c in clips
				where c.asset as AnimationPlayableAsset != null && c.parentTrack.clips.Any((TimelineClip x) => x.start < c.start)
				select c).ToArray<TimelineClip>();
				TimelineClip[] array2 = (from c in clips
				where c.asset as AnimationPlayableAsset != null && c.parentTrack.clips.Any((TimelineClip x) => x.start > c.start)
				select c).ToArray<TimelineClip>();
				if (array.Any<TimelineClip>() || array2.Any<TimelineClip>())
				{
					if (array.Any<TimelineClip>())
					{
						menu.AddItem(AnimationOffsetMenu.MatchPreviousMenuItem, false, delegate(object x)
						{
							AnimationOffsetMenu.MatchClipsToPrevious(state, (TimelineClip[])x);
						}, array);
					}
					if (array2.Any<TimelineClip>())
					{
						menu.AddItem(AnimationOffsetMenu.MatchNextMenuItem, false, delegate(object x)
						{
							AnimationOffsetMenu.MatchClipsToNext(state, (TimelineClip[])x);
						}, array2);
					}
					menu.AddItem(AnimationOffsetMenu.ResetOffsetMenuItem, false, delegate()
					{
						AnimationOffsetMenu.ResetClipOffsets(state, clips);
					});
				}
			}
		}

		// Token: 0x0400015D RID: 349
		public static GUIContent MatchPreviousMenuItem = EditorGUIUtility.TextContent("Match Offsets To Previous Clip");

		// Token: 0x0400015E RID: 350
		public static GUIContent MatchNextMenuItem = EditorGUIUtility.TextContent("Match Offsets To Next Clip");

		// Token: 0x0400015F RID: 351
		public static string MatchFieldsPrefix = "Match Offsets Fields/";

		// Token: 0x04000160 RID: 352
		public static GUIContent ResetOffsetMenuItem = EditorGUIUtility.TextContent("Reset Offsets");
	}
}

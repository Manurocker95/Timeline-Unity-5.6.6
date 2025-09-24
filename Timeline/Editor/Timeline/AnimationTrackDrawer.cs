using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000052 RID: 82
	[CustomTrackDrawer(typeof(AnimationTrack))]
	internal class AnimationTrackDrawer : TrackDrawer
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00019034 File Offset: 0x00017434
		public override Color trackColor
		{
			get
			{
				return DirectorStyles.Instance.customSkin.colorAnimation;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00019058 File Offset: 0x00017458
		public override Color GetTrackBackgroundColor(TrackAsset trackAsset)
		{
			return DirectorStyles.Instance.customSkin.colorTrackBackground;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001907C File Offset: 0x0001747C
		public override Color GetClipBaseColor(TimelineClip clip)
		{
			Color result;
			if (clip.recordable)
			{
				result = DirectorStyles.Instance.customSkin.colorAnimationRecorded;
			}
			else
			{
				result = DirectorStyles.Instance.customSkin.colorAnimation;
			}
			return result;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000190C4 File Offset: 0x000174C4
		public override GUIContent GetIcon()
		{
			if (AnimationTrackDrawer.s_AnimationTrackIcon == null)
			{
				AnimationTrackDrawer.s_AnimationTrackIcon = new GUIContent(EditorGUIUtility.LoadIcon("AnimationClip Icon"));
			}
			return AnimationTrackDrawer.s_AnimationTrackIcon;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00019100 File Offset: 0x00017500
		public override void OnBuildTrackContextMenu(GenericMenu menu, TrackAsset track, ITimelineState state)
		{
			base.OnBuildTrackContextMenu(menu, track, state);
			bool flag = false;
			AnimationTrack animTrack = track as AnimationTrack;
			if (animTrack != null)
			{
				if (animTrack.CanConvertFromClipMode() || animTrack.CanConvertToClipMode())
				{
					bool flag2 = animTrack.CanConvertFromClipMode();
					bool flag3 = animTrack.CanConvertToClipMode();
					if (flag2)
					{
						menu.AddItem(EditorGUIUtility.TextContent("Convert To Infinite Clip"), false, delegate(object parentTrack)
						{
							animTrack.ConvertFromClipMode(state.timeline);
						}, track);
						flag = true;
					}
					if (flag3)
					{
						menu.AddItem(EditorGUIUtility.TextContent("Convert To Clip Track"), false, delegate(object parentTrack)
						{
							animTrack.ConvertToClipMode();
						}, track);
					}
				}
			}
			if (!track.isSubTrack)
			{
				if (flag)
				{
					menu.AddSeparator("");
				}
				menu.AddItem(EditorGUIUtility.TextContent("Add Override Track"), false, delegate(object parentTrack)
				{
					this.AddSubTrack(state, typeof(AnimationTrack), "Override " + track.subTracks.Count.ToString(), track);
				}, track);
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00019240 File Offset: 0x00017640
		private void AddSubTrack(ITimelineState state, Type trackOfType, string trackName, TrackAsset track)
		{
			TrackAsset childAsset = state.timeline.CreateTrack(track, trackName, TimelineHelpers.TrackTypeFromType(trackOfType));
			TimelineHelpers.SaveAssetIntoObject(childAsset, track);
			track.collapsed = false;
			state.Refresh();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001927C File Offset: 0x0001767C
		public override void DrawClip(TrackDrawer.ClipDrawData drawData)
		{
			TimelineClip clip = drawData.clip;
			if (clip.animationClip == null)
			{
				base.DrawClip(drawData);
			}
			else
			{
				bool flag = false;
				if (clip.asset != null)
				{
					AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
					if (animationPlayableAsset != null)
					{
						flag = (animationPlayableAsset.clip == null);
					}
				}
				if (flag)
				{
					if (AnimationTrackDrawer.s_MissingIcon == null)
					{
						Texture2D errorIcon = EditorGUIUtility.errorIcon;
						AnimationTrackDrawer.s_MissingIcon = new GUIContent(errorIcon, "This clip has no animation assigned");
					}
					base.DrawClipErrorIcon(drawData, AnimationTrackDrawer.s_MissingIcon);
				}
				base.DrawClip(drawData);
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00019329 File Offset: 0x00017729
		public override void OnBuildClipContextMenu(GenericMenu menu, TimelineClip[] clips, ITimelineState state)
		{
			AnimationOffsetMenu.OnClipMenu(state, clips, menu);
		}

		// Token: 0x0400021E RID: 542
		public static GUIContent s_MissingIcon = null;

		// Token: 0x0400021F RID: 543
		public static GUIContent s_NonHumanIcon = null;

		// Token: 0x04000220 RID: 544
		private static GUIContent s_AnimationTrackIcon = null;
	}
}

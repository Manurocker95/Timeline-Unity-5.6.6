using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BE RID: 190
	[DisplayName("Edit in Animation Window")]
	[SeparatorMenuItem(SeparatorMenuItemPosition.After)]
	internal class EditTrackInAnimationWindow : TrackAction
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x0002D0D0 File Offset: 0x0002B4D0
		public static bool Do(TimelineWindow.TimelineState state, TrackAsset track)
		{
			AnimationTrack animationTrack = track as AnimationTrack;
			bool result;
			if (animationTrack == null)
			{
				result = false;
			}
			else if (!animationTrack.CanConvertToClipMode())
			{
				result = false;
			}
			else
			{
				Component bindingForTrack = state.GetBindingForTrack(animationTrack);
				TimelineWindowTimeControl timeController = TimelineAnimationUtilities.CreateTimeController(state, EditTrackInAnimationWindow.CreateTimeControlClipData(animationTrack));
				TimelineAnimationUtilities.EditAnimationClipWithTimeController(animationTrack.animClip, timeController, (!(bindingForTrack != null)) ? null : bindingForTrack.gameObject);
				result = true;
			}
			return result;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002D14C File Offset: 0x0002B54C
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			MenuActionDisplayState result;
			if (tracks.Length == 0)
			{
				result = MenuActionDisplayState.Hidden;
			}
			else
			{
				if (tracks[0] is AnimationTrack)
				{
					AnimationTrack track = tracks[0] as AnimationTrack;
					if (track.CanConvertToClipMode())
					{
						return MenuActionDisplayState.Visible;
					}
				}
				result = MenuActionDisplayState.Hidden;
			}
			return result;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002D19C File Offset: 0x0002B59C
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			return EditTrackInAnimationWindow.Do(state, tracks[0]);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002D1BC File Offset: 0x0002B5BC
		private static TimelineWindowTimeControl.ClipData CreateTimeControlClipData(AnimationTrack track)
		{
			return new TimelineWindowTimeControl.ClipData
			{
				track = track,
				start = (float)track.start,
				duration = (float)track.duration
			};
		}
	}
}

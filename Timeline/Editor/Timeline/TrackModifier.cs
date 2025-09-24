using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000CA RID: 202
	internal static class TrackModifier
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x0002D94C File Offset: 0x0002BD4C
		public static bool AddTrack(TimelineAsset timeline, PlayableDirector director, TrackType trackType, TrackAsset parent)
		{
			return TrackModifier.AddTrack(timeline, director, trackType, parent, "");
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002D970 File Offset: 0x0002BD70
		public static bool AddTrack(TimelineAsset timeline, PlayableDirector director, TrackType trackType, TrackAsset parent, string name)
		{
			TimelineHelpers.PushUndo(timeline, "create.track");
			PlayableAsset playableAsset = (!(parent != null)) ? timeline : parent;
			string name2 = TimelineHelpers.GenerateUniqueActorName(timeline, (name.Length <= 0) ? trackType.m_TrackType.Name : name);
			TrackAsset trackAsset = timeline.CreateTrack(playableAsset, name2, trackType);
			if (trackAsset != null)
			{
				trackAsset.name = name2;
				TimelineHelpers.AddRequiredComponent(TimelineUtility.GetSceneGameObject(director, trackAsset), trackAsset);
				TimelineHelpers.SaveAssetIntoObject(trackAsset, playableAsset);
				trackAsset.GetOrCreateClip();
			}
			return true;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002DA08 File Offset: 0x0002BE08
		private static void DeleteRecordingClip(TimelineAsset asset, TrackAsset track)
		{
			AnimationTrack animationTrack = track as AnimationTrack;
			if (!(animationTrack == null) && !(animationTrack.animClip == null))
			{
				TimelineHelpers.PushDestroyUndo(asset, track, animationTrack.animClip, "delete.track");
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002DA54 File Offset: 0x0002BE54
		public static bool DeleteTrack(TimelineAsset timeline, TrackAsset track)
		{
			TimelineHelpers.PushUndo(track, "delete.track");
			TimelineHelpers.PushUndo(timeline, "delete.track");
			TrackAsset trackAsset = track.parent as TrackAsset;
			if (trackAsset != null)
			{
				TimelineHelpers.PushUndo(trackAsset, "delete.track");
			}
			if (track.subTracks != null)
			{
				List<TrackAsset> list = new List<TrackAsset>(track.subTracks);
				foreach (TrackAsset track2 in list)
				{
					DeleteTracks.Do(timeline, track2);
				}
			}
			TrackModifier.DeleteRecordingClip(timeline, track);
			List<TimelineClip> list2 = new List<TimelineClip>(track.clips);
			foreach (TimelineClip clip in list2)
			{
				ClipModifier.Delete(timeline, clip);
			}
			timeline.RemoveTrack(track);
			TimelineHelpers.PushDestroyUndo(timeline, timeline, track, "delete.track");
			return true;
		}
	}
}

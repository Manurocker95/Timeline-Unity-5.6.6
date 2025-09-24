using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200003A RID: 58
	internal static class TimelineExtensions
	{
		// Token: 0x06000208 RID: 520 RVA: 0x000128EC File Offset: 0x00010CEC
		internal static T CreateTrack<T>(this TimelineAsset timeline, PlayableAsset parent, string name) where T : TrackAsset
		{
			return (T)((object)timeline.CreateTrack(parent, name, typeof(T)));
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00012918 File Offset: 0x00010D18
		internal static TrackAsset CreateTrack(this TimelineAsset timeline, PlayableAsset parent, string name, Type trackType)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(trackType);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				TrackMediaType trackMediaType = customAttributes[i] as TrackMediaType;
				if (trackMediaType != null)
				{
					TrackType trackDesc = new TrackType(trackType, trackMediaType.m_MediaType);
					return timeline.CreateTrack(parent, name, trackDesc);
				}
			}
			throw new InvalidOperationException("Could not find a valid TrackMediaType attribute on type" + trackType);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00012984 File Offset: 0x00010D84
		internal static TrackAsset CreateTrack(this TimelineAsset timeline, PlayableAsset parent, string name, TrackType trackDesc)
		{
			TrackAsset trackAsset = parent as TrackAsset;
			bool flag = false;
			TrackAsset trackAsset2 = ScriptableObject.CreateInstance(trackDesc.m_TrackType) as TrackAsset;
			trackAsset2.parent = ((!flag) ? (parent ?? timeline) : null);
			trackAsset2.collapsed = true;
			trackAsset2.mediaType = trackDesc.m_MediaType;
			trackAsset2.name = name;
			if (trackAsset != null && !flag)
			{
				trackAsset.subTracks.Add(trackAsset2);
			}
			else if (flag && timeline.tracks.Count<TrackAsset>() > 0)
			{
				timeline.AddTrackBefore(trackAsset2, timeline.tracks[0]);
			}
			else
			{
				timeline.AddTrack(trackAsset2);
			}
			timeline.Invalidate();
			return trackAsset2;
		}
	}
}

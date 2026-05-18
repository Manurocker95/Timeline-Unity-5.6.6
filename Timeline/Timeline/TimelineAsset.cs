using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>A PlayableAsset that represents a timeline.</para>
	/// </summary>
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class TimelineAsset : PlayableAsset, ISerializationCallbackReceiver, ITimelineClipAsset, IPropertyPreview
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003DAC File Offset: 0x00001FAC
		public TimelineAsset.EditorSettings editorSettings
		{
			get
			{
				return this.m_EditorSettings;
			}
		}

		/// <summary>
		///   <para>The length in seconds of the timeline.</para>
		/// </summary>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public override double duration
		{
			get
			{
				double result;
				if (this.m_DurationMode == TimelineAsset.DurationMode.BasedOnClips)
				{
					result = this.CalculateDuration();
				}
				else
				{
					result = this.m_FixedDuration;
				}
				return result;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003DFC File Offset: 0x00001FFC
		private double CalculateDuration()
		{
			List<TrackAsset> flattenedTracks = this.flattenedTracks;
			double result;
			if (flattenedTracks == null || flattenedTracks.Count == 0)
			{
				result = 0.0;
			}
			else
			{
				result = flattenedTracks.Max((TrackAsset t) => t.end);
			}
			return result;
		}

		/// <summary>
		///   <para>The length of the timeline when the durationMode is set to fixed length.</para>
		/// </summary>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003E5C File Offset: 0x0000205C
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003E77 File Offset: 0x00002077
		public double fixedDuration
		{
			get
			{
				return this.m_FixedDuration;
			}
			set
			{
				this.m_FixedDuration = Math.Max(0.0, value);
			}
		}

		/// <summary>
		///   <para>How the duration of a timeline is calculated.</para>
		/// </summary>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003E90 File Offset: 0x00002090
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003EAB File Offset: 0x000020AB
		public TimelineAsset.DurationMode durationMode
		{
			get
			{
				return this.m_DurationMode;
			}
			set
			{
				this.m_DurationMode = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003EB8 File Offset: 0x000020B8
		public override PlayableBinding[] outputs
		{
			get
			{
				return this.GetTopLevelTracks().SelectMany((TrackAsset x) => x.outputs).ToArray<PlayableBinding>();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003EFC File Offset: 0x000020FC
		public ClipCaps clipCaps
		{
			get
			{
				ClipCaps clipCaps = ClipCaps.All;
				foreach (TrackAsset trackAsset in this.m_Tracks)
				{
					foreach (TimelineClip timelineClip in trackAsset.clips)
					{
						clipCaps &= timelineClip.clipCaps;
					}
				}
				return clipCaps;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003F94 File Offset: 0x00002194
		internal TrackAsset[] GetTopLevelTracks()
		{
			return (from x in this.outputTracks
			where !x.isSubTrack
			select x).ToArray<TrackAsset>();
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003FD8 File Offset: 0x000021D8
		internal TrackAsset[] outputTracks
		{
			get
			{
				if (this.m_CacheOutputTracks == null)
				{
					this.m_CacheOutputTracks = this.GetOutputTracks().ToArray();
				}
				return this.m_CacheOutputTracks;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004010 File Offset: 0x00002210
		private List<TrackAsset> GetOutputTracks()
		{
			List<TrackAsset> result = new List<TrackAsset>();
			try
			{
				result = (from t in this.flattenedTracks
				where t.mediaType != TimelineAsset.MediaType.Group
				select t).ToList<TrackAsset>();
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000407C File Offset: 0x0000227C
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00004097 File Offset: 0x00002297
		internal int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000040A4 File Offset: 0x000022A4
		internal List<TrackAsset> flattenedTracks
		{
			get
			{
				List<TrackAsset> result = this.m_Tracks.ToList<TrackAsset>();
				foreach (TrackAsset track in this.m_Tracks)
				{
					TimelineAsset.AddSubTracksRecursive(track, ref result);
				}
				return result;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000411C File Offset: 0x0000231C
		private static void AddSubTracksRecursive(TrackAsset track, ref List<TrackAsset> allTracks)
		{
			if (!(track == null))
			{
				if (track.subTracks != null && track.subTracks.Count != 0)
				{
					allTracks.AddRange(track.subTracks);
					foreach (TrackAsset track2 in track.subTracks)
					{
						TimelineAsset.AddSubTracksRecursive(track2, ref allTracks);
					}
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000041BC File Offset: 0x000023BC
		internal List<TrackAsset> tracks
		{
			get
			{
				return this.m_Tracks;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000041D7 File Offset: 0x000023D7
		internal void AddTrack(TrackAsset track)
		{
			this.m_Tracks.Add(track);
			track.parent = this;
			this.m_CacheOutputTracks = null;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000041F4 File Offset: 0x000023F4
		internal void AddTrackBefore(TrackAsset track, TrackAsset before)
		{
			this.AddTrack(track);
			this.MoveTracks(new List<TrackAsset>
			{
				track
			}, before, true);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004220 File Offset: 0x00002420
		internal void AddTrackAfter(TrackAsset track, TrackAsset after)
		{
			this.AddTrack(track);
			this.MoveTracks(new List<TrackAsset>
			{
				track
			}, after, false);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000424C File Offset: 0x0000244C
		internal void MoveTracks(List<TrackAsset> tracks, TrackAsset insertAfterAsset, bool up)
		{
			List<TrackAsset> list = new List<TrackAsset>(this.m_Tracks);
			for (int i = 0; i < tracks.Count; i++)
			{
				list.Remove(tracks[i]);
			}
			int num = list.IndexOf(insertAfterAsset);
			num = ((!up) ? (num + 1) : Math.Max(num - 1, 0));
			if (num < 0)
			{
				Debug.LogError("target track not found");
			}
			else
			{
				for (int j = 0; j < tracks.Count; j++)
				{
					num = ((!up) ? Math.Min(num, list.Count) : Math.Max(num, 0));
					list.Insert(num, tracks[j]);
				}
				this.m_Tracks = list;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004314 File Offset: 0x00002514
		public void OnEnable()
		{
			if (this.m_Tracks == null)
			{
				this.m_Tracks = new List<TrackAsset>();
			}
			this.m_Tracks.RemoveAll((TrackAsset t) => t == null);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004364 File Offset: 0x00002564
		internal void RemoveTrack(TrackAsset track)
		{
			this.m_Tracks.Remove(track);
			this.m_CacheOutputTracks = null;
			TrackAsset trackAsset = track.parent as TrackAsset;
			if (trackAsset != null)
			{
				trackAsset.RemoveSubTrack(track);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000043A8 File Offset: 0x000025A8
		internal int GenerateNewId()
		{
			this.m_NextId++;
			return base.GetInstanceID().GetHashCode() ^ this.m_NextId.GetHashCode();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000043F4 File Offset: 0x000025F4
		public override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
		{
			bool autoRebalance = true;
			bool createOutputs = graph.playableCount == 0;
			TimelinePlayable timelinePlayable = TimelinePlayable.Create(graph, this.flattenedTracks, go, autoRebalance, createOutputs);
			return (timelinePlayable == null) ? PlayableHandle.Null : timelinePlayable.handle;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000443E File Offset: 0x0000263E
		public void OnAfterDeserialize()
		{
			this.m_CacheOutputTracks = null;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004448 File Offset: 0x00002648
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000444B File Offset: 0x0000264B
		internal void Invalidate()
		{
			this.m_CacheOutputTracks = null;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004458 File Offset: 0x00002658
		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			foreach (TrackAsset trackAsset in this.outputTracks)
			{
				trackAsset.GatherProperties(director, driver);
			}
		}

		// Token: 0x0400001A RID: 26
		[HideInInspector]
		[SerializeField]
		private int m_Id;

		// Token: 0x0400001B RID: 27
		[HideInInspector]
		[SerializeField]
		private int m_NextId = 0;

		// Token: 0x0400001C RID: 28
		[HideInInspector]
		[SerializeField]
		private List<TrackAsset> m_Tracks;

		// Token: 0x0400001D RID: 29
		[HideInInspector]
		[SerializeField]
		private double m_FixedDuration = 0.0;

		// Token: 0x0400001E RID: 30
		[HideInInspector]
		[NonSerialized]
		private TrackAsset[] m_CacheOutputTracks;

		// Token: 0x0400001F RID: 31
		[HideInInspector]
		[SerializeField]
		private TimelineAsset.EditorSettings m_EditorSettings = new TimelineAsset.EditorSettings();

		// Token: 0x04000020 RID: 32
		[SerializeField]
		public DirectorWrapMode m_ExtrapolationMode = (DirectorWrapMode)1;

		// Token: 0x04000021 RID: 33
		[SerializeField]
		public TimelineAsset.SequenceUpdateMode m_UpdateMode = TimelineAsset.SequenceUpdateMode.TimeBased;

		// Token: 0x04000022 RID: 34
		[SerializeField]
		public string m_ParameterName = "";

		// Token: 0x04000023 RID: 35
		[SerializeField]
		public TimelineAsset.DurationMode m_DurationMode;

		// Token: 0x0200000B RID: 11
		public enum SequenceUpdateMode
		{
			// Token: 0x0400002A RID: 42
			TimeBased,
			// Token: 0x0400002B RID: 43
			ParameterBased,
			// Token: 0x0400002C RID: 44
			ParentSequence
		}

		// Token: 0x0200000C RID: 12
		public enum MediaType
		{
			// Token: 0x0400002E RID: 46
			Animation,
			// Token: 0x0400002F RID: 47
			Audio,
			// Token: 0x04000030 RID: 48
			Video,
			// Token: 0x04000031 RID: 49
			Script,
			// Token: 0x04000032 RID: 50
			Hybrid,
			// Token: 0x04000033 RID: 51
			Group
		}

		/// <summary>
		///   <para>How the duration of the timeline is determined.</para>
		/// </summary>
		// Token: 0x0200000D RID: 13
		public enum DurationMode
		{
			/// <summary>
			///   <para>The duration of the timeline is determined based on the clips present.</para>
			/// </summary>
			// Token: 0x04000035 RID: 53
			BasedOnClips,
			/// <summary>
			///   <para>The duration of the timeline is a fixed length.</para>
			/// </summary>
			// Token: 0x04000036 RID: 54
			FixedLength
		}

		// Token: 0x0200000E RID: 14
		[Serializable]
		public class EditorSettings
		{
			// Token: 0x04000037 RID: 55
			internal static readonly float kDefaultFPS = 60f;

			// Token: 0x04000038 RID: 56
			internal static readonly Vector2 kTimeAreaDefaultRange = new Vector2(-5f, 5f);

			// Token: 0x04000039 RID: 57
			[SerializeField]
			public float fps = TimelineAsset.EditorSettings.kDefaultFPS;

			// Token: 0x0400003A RID: 58
			[SerializeField]
			public Vector2 timeAreaShownRange = TimelineAsset.EditorSettings.kTimeAreaDefaultRange;
		}
	}
}

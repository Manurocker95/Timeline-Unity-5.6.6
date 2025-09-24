using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>A PlayableAsset Representing a track inside a timeline.</para>
	/// </summary>
	// Token: 0x0200001D RID: 29
	[Serializable]
	public abstract class TrackAsset : PlayableAsset, IInterval, ISerializationCallbackReceiver, IPropertyPreview
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00002194 File Offset: 0x00000394
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000021AE File Offset: 0x000003AE
		public int intervalBit { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000021B8 File Offset: 0x000003B8
		public double start
		{
			get
			{
				this.UpdateDuration();
				return this.m_Start;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000021DC File Offset: 0x000003DC
		public double end
		{
			get
			{
				this.UpdateDuration();
				return this.m_End;
			}
		}

		/// <summary>
		///   <para>Mutes the track, excluding it from the generated PlayableGraph.</para>
		/// </summary>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002200 File Offset: 0x00000400
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000221B File Offset: 0x0000041B
		public bool muted
		{
			get
			{
				return this.m_Muted;
			}
			set
			{
				this.m_Muted = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002228 File Offset: 0x00000428
		public sealed override double duration
		{
			get
			{
				this.UpdateDuration();
				return this.m_End - this.m_Start;
			}
		}

		/// <summary>
		///   <para>The TimelineAsset this track belongs to.</para>
		/// </summary>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00002250 File Offset: 0x00000450
		public TimelineAsset timelineAsset
		{
			get
			{
				TrackAsset trackAsset = this;
				while (trackAsset != null)
				{
					TimelineAsset result;
					if (trackAsset.parent == null)
					{
						result = null;
					}
					else
					{
						TimelineAsset timelineAsset = trackAsset.parent as TimelineAsset;
						if (!(timelineAsset != null))
						{
							trackAsset = (trackAsset.parent as TrackAsset);
							continue;
						}
						result = timelineAsset;
					}
					return result;
				}
				return null;
			}
		}

		/// <summary>
		///   <para>The owner of this track.</para>
		/// </summary>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000022C0 File Offset: 0x000004C0
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x000022DB File Offset: 0x000004DB
		public PlayableAsset parent
		{
			get
			{
				return this.m_Parent;
			}
			internal set
			{
				this.m_Parent = value;
			}
		}

		/// <summary>
		///   <para>The clips owned by this TrackAsset.</para>
		/// </summary>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000022E8 File Offset: 0x000004E8
		public TimelineClip[] clips
		{
			get
			{
				if (this.m_Clips == null)
				{
					this.m_Clips = new List<TimelineClip>();
				}
				if (this.m_ClipsCache == null)
				{
					this.m_ClipsCache = this.m_Clips.ToArray();
				}
				return this.m_ClipsCache;
			}
		}

		/// <summary>
		///   <para>Does this track have any clips?</para>
		/// </summary>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00002338 File Offset: 0x00000538
		public bool isEmpty
		{
			get
			{
				return this.m_Clips == null || this.m_Clips.Count == 0;
			}
		}

		/// <summary>
		///   <para>Is this track a subtrack?</para>
		/// </summary>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000236C File Offset: 0x0000056C
		public bool isSubTrack
		{
			get
			{
				TrackAsset trackAsset = this.parent as TrackAsset;
				return trackAsset != null && trackAsset.GetType() == base.GetType();
			}
		}

		/// <summary>
		///   <para>A description of the outputs of the instantiated Playable.</para>
		/// </summary>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000023AC File Offset: 0x000005AC
		public override PlayableBinding[] outputs
		{
			get
			{
				TrackBindingTypeAttribute trackBindingTypeAttribute = Attribute.GetCustomAttribute(base.GetType(), typeof(TrackBindingTypeAttribute)) as TrackBindingTypeAttribute;
				Type sourceBindingType = (trackBindingTypeAttribute == null) ? null : trackBindingTypeAttribute.type;
				PlayableBinding[] array = new PlayableBinding[1];
				int num = 0;
				PlayableBinding playableBinding = default(PlayableBinding);
				playableBinding.sourceObject = this;
				playableBinding.streamName = base.name;
				playableBinding.streamType = (DirectorStreamType)3;
				playableBinding.sourceBindingType = sourceBindingType;
				array[num] = playableBinding;
				return array;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x060000DF RID: 223 RVA: 0x0000244B File Offset: 0x0000064B
		internal string customPlayableTypename
		{
			get
			{
				return this.m_CustomPlayableFullTypename;
			}
			set
			{
				this.m_CustomPlayableFullTypename = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00002458 File Offset: 0x00000658
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00002473 File Offset: 0x00000673
		internal AnimationClip animClip
		{
			get
			{
				return this.m_AnimClip;
			}
			set
			{
				this.m_AnimClip = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00002480 File Offset: 0x00000680
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000249A File Offset: 0x0000069A
		internal bool displayCascadeClips { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000024A4 File Offset: 0x000006A4
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000024BF File Offset: 0x000006BF
		internal bool soloed
		{
			get
			{
				return this.m_Soloed;
			}
			set
			{
				this.m_Soloed = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000024CC File Offset: 0x000006CC
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000024E7 File Offset: 0x000006E7
		internal float height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000024F4 File Offset: 0x000006F4
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000250F File Offset: 0x0000070F
		internal bool showInlineCurves
		{
			get
			{
				return this.m_ShowInlineCurves;
			}
			set
			{
				this.m_ShowInlineCurves = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000251C File Offset: 0x0000071C
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00002537 File Offset: 0x00000737
		internal float inlineAnimationCurveHeight
		{
			get
			{
				return this.m_InlineAnimationCurveHeight;
			}
			set
			{
				this.m_InlineAnimationCurveHeight = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00002544 File Offset: 0x00000744
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000255F File Offset: 0x0000075F
		internal List<TrackAsset> subTracks
		{
			get
			{
				return this.m_Children;
			}
			set
			{
				this.m_Children = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000256C File Offset: 0x0000076C
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00002587 File Offset: 0x00000787
		internal bool locked
		{
			get
			{
				return this.m_Locked;
			}
			set
			{
				this.m_Locked = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002594 File Offset: 0x00000794
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000025AF File Offset: 0x000007AF
		internal TimelineAsset.MediaType mediaType
		{
			get
			{
				return this.m_MediaType;
			}
			set
			{
				this.m_MediaType = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000025BC File Offset: 0x000007BC
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000025D7 File Offset: 0x000007D7
		internal bool collapsed
		{
			get
			{
				return this.m_Collapsed;
			}
			set
			{
				this.m_Collapsed = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000025E4 File Offset: 0x000007E4
		internal virtual bool compilable
		{
			get
			{
				return !this.muted && !this.isEmpty;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00002610 File Offset: 0x00000810
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00002613 File Offset: 0x00000813
		public void OnAfterDeserialize()
		{
			this.m_ClipsCache = null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00002620 File Offset: 0x00000820
		public virtual void OnEnable()
		{
			if (this.m_Children == null)
			{
				this.m_Children = new List<TrackAsset>();
			}
			if (this.m_Clips == null)
			{
				this.m_Clips = new List<TimelineClip>();
			}
			if (this.m_Children.Count > 0)
			{
				List<TrackAsset> children = (from t in this.m_Children
				where t != null
				select t).ToList<TrackAsset>();
				this.m_Children = children;
			}
			this.displayCascadeClips = false;
		}

		/// <summary>
		///   <para>Creates a mixer used to blend playables generated by clips on the track.</para>
		/// </summary>
		/// <param name="graph">The graph to inject playables to.</param>
		/// <param name="go">The GameObject that requested the graph.</param>
		/// <param name="inputCount">The number of playables from clips that will be mixed.</param>
		/// <returns>
		///   <para>A handle to the Playable representing the mixer.</para>
		/// </returns>
		// Token: 0x060000F8 RID: 248 RVA: 0x000026AC File Offset: 0x000008AC
		public virtual PlayableHandle CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return graph.CreateGenericMixerPlayable(inputCount);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000026CC File Offset: 0x000008CC
		public sealed override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return PlayableHandle.Null;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000026E6 File Offset: 0x000008E6
		internal void AddClip(TimelineClip newClip)
		{
			if (!this.m_Clips.Contains(newClip))
			{
				this.m_Clips.Add(newClip);
				this.m_ClipsCache = null;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00002710 File Offset: 0x00000910
		internal PlayableHandle CreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree tree)
		{
			return this.OnCreatePlayableGraph(graph, go, tree);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00002730 File Offset: 0x00000930
		internal virtual PlayableHandle OnCreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree tree)
		{
			if (tree == null)
			{
				throw new ArgumentException("IntervalTree argument cannot be null", "tree");
			}
			if (go == null)
			{
				throw new ArgumentException("GameObject argument cannot be null", "go");
			}
			PlayableHandle playableHandle = this.CreateTrackMixer(graph, go, this.clips.Length);
			for (int i = 0; i < this.clips.Length; i++)
			{
				PlayableHandle playableHandle2 = TrackAsset.CreatePlayable(graph, go, this.clips[i]);
				if (playableHandle2.IsValid())
				{
					playableHandle2.duration = this.clips[i].duration;
					tree.Add(new RuntimeClip(this.clips[i], playableHandle2, playableHandle)
					{
						inclusiveDuration = this.HasClipInclusiveDuration(i)
					});
					graph.Connect(playableHandle2, 0, playableHandle, i);
					playableHandle.SetInputWeight(i, 0f);
				}
			}
			return playableHandle;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00002818 File Offset: 0x00000A18
		internal bool IsCompatibleWithClip(TimelineClip clip)
		{
			return this.ValidateClipType(clip.underlyingAsset.GetType());
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000283E File Offset: 0x00000A3E
		internal void SortClips()
		{
			this.m_Clips.Sort((TimelineClip clip1, TimelineClip clip2) => clip1.start.CompareTo(clip2.start));
			this.m_ClipsCache = null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00002870 File Offset: 0x00000A70
		internal void SetClips(List<TimelineClip> clips)
		{
			this.m_Clips = clips;
			this.m_ClipsCache = clips.ToArray();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00002888 File Offset: 0x00000A88
		internal TimelineClip CreateClipFromAsset(Object asset)
		{
			TimelineClip result;
			if (asset == null)
			{
				result = null;
			}
			else
			{
				TimelineAsset timelineAsset = asset as TimelineAsset;
				if (timelineAsset == null && !this.ValidateClipType(asset.GetType()))
				{
					throw new InvalidOperationException(string.Format("Cannot create a clip for track {0} with clip type: {1}. Did you forget a ClipClass attribute?", base.GetType(), asset.GetType()));
				}
				TimelineClip timelineClip = this.CreateNewClipContainerInternal();
				timelineClip.underlyingAsset = asset;
				timelineClip.displayName = asset.name;
				timelineClip.asset = asset;
				if (timelineAsset == null)
				{
					try
					{
						this.OnCreateClipFromAsset(asset, timelineClip);
					}
					catch (Exception ex)
					{
						Debug.LogError(ex.Message, asset);
						this.RemoveClip(timelineClip);
						return null;
					}
				}
				else
				{
					timelineClip.duration = timelineAsset.duration;
					timelineClip.asset = asset;
					timelineClip.isNestedAsset = true;
				}
				result = timelineClip;
			}
			return result;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000297C File Offset: 0x00000B7C
		internal TimelineClip CreateNestedClipFromPlayableAsset(PlayableAsset asset)
		{
			return this.NestPlayableAsset(asset);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00002998 File Offset: 0x00000B98
		internal virtual void OnCreateClipFromAsset(Object asset, TimelineClip clip)
		{
			clip.asset = asset;
			clip.underlyingAsset = asset;
			clip.displayName = asset.name;
			IPlayableAsset playableAsset = asset as IPlayableAsset;
			if (playableAsset != null)
			{
				double duration = playableAsset.duration;
				if (duration > 0.0 && !double.IsInfinity(duration))
				{
					clip.duration = duration;
				}
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000029F8 File Offset: 0x00000BF8
		internal TimelineClip CreateNewClipContainerInternal()
		{
			TimelineClip timelineClip = new TimelineClip(this);
			timelineClip.asset = null;
			double num = 0.0;
			for (int i = 0; i < this.m_Clips.Count - 1; i++)
			{
				double num2 = this.m_Clips[i].duration;
				if (double.IsInfinity(num2))
				{
					num2 = (double)TimelineClip.kDefaultClipDurationInSeconds;
				}
				num = Math.Max(num, this.m_Clips[i].start + num2);
			}
			timelineClip.mixInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			timelineClip.mixOutCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
			timelineClip.start = num;
			timelineClip.duration = (double)TimelineClip.kDefaultClipDurationInSeconds;
			timelineClip.displayName = "untitled";
			return timelineClip;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00002AE2 File Offset: 0x00000CE2
		internal void AddChild(TrackAsset child)
		{
			if (!(child == null))
			{
				this.m_Children.Add(child);
				child.parent = this;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00002B0C File Offset: 0x00000D0C
		internal bool AddChildAfter(TrackAsset child, TrackAsset other)
		{
			bool result;
			if (child == null)
			{
				result = false;
			}
			else
			{
				int num = this.m_Children.IndexOf(other);
				if (num >= 0 && num != this.m_Children.Count - 1)
				{
					this.m_Children.Insert(num + 1, child);
				}
				else
				{
					this.m_Children.Add(child);
				}
				child.parent = this;
				result = true;
			}
			return result;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00002B84 File Offset: 0x00000D84
		internal bool RemoveSubTrack(TrackAsset child)
		{
			bool result;
			if (this.m_Children.Remove(child))
			{
				child.parent = null;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00002BBA File Offset: 0x00000DBA
		internal virtual void RemoveClip(TimelineClip clip)
		{
			this.m_Clips.Remove(clip);
			this.m_ClipsCache = null;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00002BD4 File Offset: 0x00000DD4
		internal virtual void GetEvaluationTime(out double outStart, out double outDuration)
		{
			if (this.clips.Length == 0)
			{
				outStart = 0.0;
				outDuration = 0.0;
			}
			else
			{
				outStart = double.MaxValue;
				double num = 0.0;
				for (int i = 0; i < this.clips.Length; i++)
				{
					outStart = Math.Min(this.clips[i].start, outStart);
					num = Math.Max(this.clips[i].start + this.clips[i].duration, num);
				}
				outStart = Math.Max(outStart, 0.0);
				outDuration = num - outStart;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00002C8B File Offset: 0x00000E8B
		internal virtual void GetSequenceTime(out double outStart, out double outDuration)
		{
			this.GetEvaluationTime(out outStart, out outDuration);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00002C98 File Offset: 0x00000E98
		internal bool HasClipInclusiveDuration(int clipIndex)
		{
			return !this.clips[clipIndex].hasPostExtrapolation && (clipIndex + 1 == this.clips.Length || (!this.clips[clipIndex].hasPostExtrapolation && !this.clips[clipIndex + 1].hasPreExtrapolation && this.clips[clipIndex].end < this.clips[clipIndex + 1].start));
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00002D2C File Offset: 0x00000F2C
		public virtual void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			GameObject gameObjectBinding = this.GetGameObjectBinding(director);
			if (gameObjectBinding != null)
			{
				driver.PushActiveGameObject(gameObjectBinding);
			}
			if (this.animClip != null)
			{
				driver.AddFromClip(this.animClip);
			}
			foreach (TimelineClip timelineClip in this.clips)
			{
				if (timelineClip.curves != null && timelineClip.asset != null)
				{
					driver.AddObjectProperties(timelineClip.asset, timelineClip.curves);
				}
				IPropertyPreview propertyPreview = timelineClip.asset as IPropertyPreview;
				if (propertyPreview != null)
				{
					propertyPreview.GatherProperties(director, driver);
				}
			}
			foreach (TrackAsset trackAsset in this.subTracks)
			{
				if (trackAsset != null)
				{
					trackAsset.GatherProperties(director, driver);
				}
			}
			if (gameObjectBinding != null)
			{
				driver.PopActiveGameObject();
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00002E5C File Offset: 0x0000105C
		internal GameObject GetGameObjectBinding(PlayableDirector director)
		{
			GameObject result;
			if (director == null)
			{
				result = null;
			}
			else
			{
				GameObject gameObject = director.GetGenericBinding(this) as GameObject;
				if (gameObject != null)
				{
					result = gameObject;
				}
				else
				{
					Component component = director.GetGenericBinding(this) as Component;
					if (component != null)
					{
						result = component.gameObject;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00002ECC File Offset: 0x000010CC
		private bool ValidateClipType(Type clipType)
		{
			foreach (object obj in base.GetType().GetCustomAttributes(typeof(TrackClipTypeAttribute), true))
			{
				TrackClipTypeAttribute trackClipTypeAttribute = obj as TrackClipTypeAttribute;
				if (trackClipTypeAttribute.inspectedType.IsAssignableFrom(clipType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00002F38 File Offset: 0x00001138
		private TimelineClip NestPlayableAsset(PlayableAsset asset)
		{
			DirectorStreamType directorStreamType = (DirectorStreamType)3;
			switch (this.mediaType)
			{
			case TimelineAsset.MediaType.Animation:
				directorStreamType = (DirectorStreamType)0;
				break;
			case TimelineAsset.MediaType.Audio:
				directorStreamType = (DirectorStreamType)1;
				break;
			case TimelineAsset.MediaType.Video:
				directorStreamType = (DirectorStreamType)2;
				break;
			case TimelineAsset.MediaType.Script:
				directorStreamType = (DirectorStreamType)3;
				break;
			}
			int num = -1;
			for (int i = 0; i < asset.outputs.Length; i++)
			{
				if (asset.outputs[i].streamType == directorStreamType)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				throw new InvalidOperationException("Cannot nest asset because it does not contain any compatible tracks");
			}
			TimelineClip timelineClip = this.CreateNewClipContainerInternal();
			timelineClip.asset = asset;
			timelineClip.displayName = asset.name;
			timelineClip.duration = asset.duration;
			timelineClip.asset = asset;
			timelineClip.isNestedAsset = true;
			timelineClip.nestedOutputIndex = num;
			return timelineClip;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003024 File Offset: 0x00001224
		private void UpdateDuration()
		{
			if (this.clips.Length == 0)
			{
				this.m_ClipsHash = 0;
				this.m_Start = 0.0;
				this.m_End = (double)((!(this.m_AnimClip != null)) ? 0f : this.m_AnimClip.length);
			}
			else
			{
				int num = this.clips.Aggregate(0, (int current, TimelineClip clip) => current ^ clip.Hash());
				if (num != this.m_ClipsHash)
				{
					this.m_ClipsHash = num;
					double num2;
					double num3;
					this.GetSequenceTime(out num2, out num3);
					this.m_Start = num2;
					this.m_End = num2 + num3;
					this.CalculateExtrapolationTimes();
				}
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000030EC File Offset: 0x000012EC
		private static PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
		{
			IPlayableAsset playableAsset = clip.asset as IPlayableAsset;
			PlayableHandle result;
			if (playableAsset != null)
			{
				PlayableHandle playableHandle = playableAsset.CreatePlayable(graph, go);
				if (playableHandle.IsValid())
				{
					AnimationPlayableExtensions.SetAnimatedProperties(playableHandle, clip.curves);
				}
				result = playableHandle;
			}
			else
			{
				result = PlayableHandle.Null;
			}
			return result;
		}

		// Token: 0x0400007D RID: 125
		[SerializeField]
		private bool m_Locked;

		// Token: 0x0400007E RID: 126
		[SerializeField]
		private bool m_Muted;

		// Token: 0x0400007F RID: 127
		[SerializeField]
		private bool m_Soloed;

		// Token: 0x04000080 RID: 128
		[SerializeField]
		private float m_Height;

		// Token: 0x04000081 RID: 129
		[SerializeField]
		private bool m_ShowInlineCurves;

		// Token: 0x04000082 RID: 130
		[SerializeField]
		private float m_InlineAnimationCurveHeight = 100f;

		// Token: 0x04000083 RID: 131
		[SerializeField]
		private string m_CustomPlayableFullTypename = string.Empty;

		// Token: 0x04000084 RID: 132
		[NonSerialized]
		private double m_Start;

		// Token: 0x04000085 RID: 133
		[NonSerialized]
		private double m_End;

		// Token: 0x04000086 RID: 134
		[SerializeField]
		[FormerlySerializedAs("m_animClip")]
		private AnimationClip m_AnimClip;

		// Token: 0x04000087 RID: 135
		[NonSerialized]
		private TimelineClip[] m_ClipsCache;

		// Token: 0x04000088 RID: 136
		[NonSerialized]
		private int m_ClipsHash;

		// Token: 0x04000089 RID: 137
		[SerializeField]
		private bool m_Collapsed;

		// Token: 0x0400008A RID: 138
		[SerializeField]
		private PlayableAsset m_Parent;

		// Token: 0x0400008B RID: 139
		[SerializeField]
		private List<TrackAsset> m_Children;

		// Token: 0x0400008C RID: 140
		[SerializeField]
		private TimelineAsset.MediaType m_MediaType;

		// Token: 0x0400008D RID: 141
		[SerializeField]
		protected internal List<TimelineClip> m_Clips = new List<TimelineClip>();
	}
}

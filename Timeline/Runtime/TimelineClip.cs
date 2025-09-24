using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Represents a clip on the timeline.</para>
	/// </summary>
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class TimelineClip
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0000459C File Offset: 0x0000279C
		internal TimelineClip(TrackAsset parent)
		{
			this.parentTrack = parent;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000463C File Offset: 0x0000283C
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00004656 File Offset: 0x00002856
		internal int dirtyHash { get; set; }

		/// <summary>
		///   <para>Is the clip being extrapolated before it's start time?</para>
		/// </summary>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00004660 File Offset: 0x00002860
		public bool hasPreExtrapolation
		{
			get
			{
				return this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None && this.m_PreExtrapolationTime > 0.0;
			}
		}

		/// <summary>
		///   <para>Is the clip being extrapolated past it's end time.</para>
		/// </summary>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00004694 File Offset: 0x00002894
		public bool hasPostExtrapolation
		{
			get
			{
				return this.m_PostExtrapolationMode != TimelineClip.ClipExtrapolation.None && this.m_PostExtrapolationTime > 0.0;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000046C8 File Offset: 0x000028C8
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000046E3 File Offset: 0x000028E3
		public bool isNestedAsset
		{
			get
			{
				return this.m_NestedAsset;
			}
			set
			{
				this.m_NestedAsset = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000046F0 File Offset: 0x000028F0
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000470B File Offset: 0x0000290B
		internal int nestedOutputIndex
		{
			get
			{
				return this.m_NestedOutputIndex;
			}
			set
			{
				this.m_NestedOutputIndex = value;
			}
		}

		/// <summary>
		///   <para>The time scale of the clip.</para>
		/// </summary>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004718 File Offset: 0x00002918
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00004766 File Offset: 0x00002966
		public double timeScale
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.SpeedMultiplier)) ? 1.0 : Math.Max(TimelineClip.kTimeScaleMin, Math.Min(this.m_TimeScale, TimelineClip.kTimeScaleMax));
			}
			set
			{
				this.m_TimeScale = ((!this.clipCaps.HasAny(ClipCaps.SpeedMultiplier)) ? 1.0 : Math.Max(TimelineClip.kTimeScaleMin, Math.Min(value, TimelineClip.kTimeScaleMax)));
			}
		}

		/// <summary>
		///   <para>The start time of the clip.</para>
		/// </summary>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000047A4 File Offset: 0x000029A4
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000047BF File Offset: 0x000029BF
		public double start
		{
			get
			{
				return this.m_Start;
			}
			set
			{
				this.m_Start = TimelineClip.SanitizeTimeValue(value, this.m_Start);
			}
		}

		/// <summary>
		///   <para>The length in seconds of the clip.</para>
		/// </summary>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000047D4 File Offset: 0x000029D4
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000047EF File Offset: 0x000029EF
		public double duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = Math.Max(TimelineClip.SanitizeTimeValue(value, this.m_Duration), double.Epsilon);
			}
		}

		/// <summary>
		///   <para>The end time of the clip.</para>
		/// </summary>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004814 File Offset: 0x00002A14
		public double end
		{
			get
			{
				return this.m_Start + this.m_Duration;
			}
		}

		/// <summary>
		///   <para>Time to start playing the clip at.</para>
		/// </summary>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00004838 File Offset: 0x00002A38
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00004874 File Offset: 0x00002A74
		public double clipIn
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.ClipIn)) ? 0.0 : this.m_ClipIn;
			}
			set
			{
				this.m_ClipIn = ((!this.clipCaps.HasAny(ClipCaps.ClipIn)) ? 0.0 : Math.Max(Math.Min(TimelineClip.SanitizeTimeValue(value, this.m_ClipIn), this.clipAssetDuration), 0.0));
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000048CC File Offset: 0x00002ACC
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000048E7 File Offset: 0x00002AE7
		public string displayName
		{
			get
			{
				return this.m_DisplayName;
			}
			set
			{
				this.m_DisplayName = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000048F4 File Offset: 0x00002AF4
		public double clipAssetDuration
		{
			get
			{
				IPlayableAsset playableAsset = this.m_Asset as IPlayableAsset;
				return (playableAsset == null) ? double.MaxValue : playableAsset.duration;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00004930 File Offset: 0x00002B30
		public AnimationClip curves
		{
			get
			{
				return this.m_AnimationCurves;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000494C File Offset: 0x00002B4C
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00004967 File Offset: 0x00002B67
		public bool selected
		{
			get
			{
				return this.m_Selected;
			}
			set
			{
				this.m_Selected = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004974 File Offset: 0x00002B74
		// (set) Token: 0x0600007B RID: 123 RVA: 0x0000498F File Offset: 0x00002B8F
		public bool inlineCurvesSelected
		{
			get
			{
				return this.m_InlineCurvesSelected;
			}
			set
			{
				this.m_InlineCurvesSelected = value;
			}
		}

		/// <summary>
		///   <para>Reference to a serializable IPlayableAsset representing the specialization of the clip.</para>
		/// </summary>
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000499C File Offset: 0x00002B9C
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000049B7 File Offset: 0x00002BB7
		public Object asset
		{
			get
			{
				return this.m_Asset;
			}
			set
			{
				this.m_Asset = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000049C4 File Offset: 0x00002BC4
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000049FB File Offset: 0x00002BFB
		public Object underlyingAsset
		{
			get
			{
				return (!(this.m_UnderlyingAsset != null)) ? this.m_Asset : this.m_UnderlyingAsset;
			}
			set
			{
				this.m_UnderlyingAsset = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004A08 File Offset: 0x00002C08
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00004A24 File Offset: 0x00002C24
		internal TrackAsset parentTrack
		{
			get
			{
				return this.m_ParentTrack;
			}
			set
			{
				if (!(value == null))
				{
					if (!(this.m_ParentTrack == value))
					{
						if (this.m_ParentTrack != null)
						{
							this.m_ParentTrack.RemoveClip(this);
						}
						this.m_ParentTrack = value;
						this.m_ParentTrack.AddClip(this);
					}
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004A8C File Offset: 0x00002C8C
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00004AEA File Offset: 0x00002CEA
		public double easeInDuration
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : Math.Min(Math.Max(this.m_EaseInDuration, 0.0), this.duration * 0.49);
			}
			set
			{
				this.m_EaseInDuration = ((!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : TimelineClip.SanitizeTimeValue(value, this.m_EaseInDuration));
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004B20 File Offset: 0x00002D20
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00004B7E File Offset: 0x00002D7E
		public double easeOutDuration
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : Math.Min(Math.Max(this.m_EaseOutDuration, 0.0), this.duration * 0.49);
			}
			set
			{
				this.m_EaseOutDuration = ((!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : TimelineClip.SanitizeTimeValue(value, this.m_EaseOutDuration));
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public double eastOutTime
		{
			get
			{
				return this.duration - this.easeOutDuration + this.m_Start;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004BE0 File Offset: 0x00002DE0
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00004C1B File Offset: 0x00002E1B
		public double blendInDuration
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : this.m_BlendInDuration;
			}
			set
			{
				this.m_BlendInDuration = ((!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : TimelineClip.SanitizeTimeValue(value, this.m_BlendInDuration));
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004C50 File Offset: 0x00002E50
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00004C8B File Offset: 0x00002E8B
		public double blendOutDuration
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : this.m_BlendOutDuration;
			}
			set
			{
				this.m_BlendOutDuration = ((!this.clipCaps.HasAny(ClipCaps.Blending)) ? 0.0 : TimelineClip.SanitizeTimeValue(value, this.m_BlendOutDuration));
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004CC0 File Offset: 0x00002EC0
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00004CDB File Offset: 0x00002EDB
		public TimelineClip.BlendCurveMode blendInCurveMode
		{
			get
			{
				return this.m_BlendInCurveMode;
			}
			set
			{
				this.m_BlendInCurveMode = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004CE8 File Offset: 0x00002EE8
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00004D03 File Offset: 0x00002F03
		public TimelineClip.BlendCurveMode blendOutCurveMode
		{
			get
			{
				return this.m_BlendOutCurveMode;
			}
			set
			{
				this.m_BlendOutCurveMode = value;
			}
		}

		/// <summary>
		///   <para>Does the clip have blend in?</para>
		/// </summary>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004D10 File Offset: 0x00002F10
		public bool hasBlendIn
		{
			get
			{
				return this.clipCaps.HasAny(ClipCaps.Blending) && this.m_BlendInDuration > 0.0;
			}
		}

		/// <summary>
		///   <para>Does the clip have a non-zero blend out?</para>
		/// </summary>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004D4C File Offset: 0x00002F4C
		public bool hasBlendOut
		{
			get
			{
				return this.clipCaps.HasAny(ClipCaps.Blending) && this.m_BlendOutDuration > 0.0;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004D88 File Offset: 0x00002F88
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004DCB File Offset: 0x00002FCB
		public AnimationCurve mixInCurve
		{
			get
			{
				if (this.m_MixInCurve == null || this.m_MixInCurve.length < 2)
				{
					this.m_MixInCurve = this.GetDefaultMixInCurve();
				}
				return this.m_MixInCurve;
			}
			set
			{
				this.m_MixInCurve = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004DD8 File Offset: 0x00002FD8
		public float mixInPercentage
		{
			get
			{
				return (float)(this.mixInDuration / this.duration);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004DFC File Offset: 0x00002FFC
		public double mixInDuration
		{
			get
			{
				return (!this.hasBlendIn) ? this.easeInDuration : this.blendInDuration;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004E30 File Offset: 0x00003030
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004E73 File Offset: 0x00003073
		public AnimationCurve mixOutCurve
		{
			get
			{
				if (this.m_MixOutCurve == null || this.m_MixOutCurve.length < 2)
				{
					this.m_MixOutCurve = this.GetDefaultMixOutCurve();
				}
				return this.m_MixOutCurve;
			}
			set
			{
				this.m_MixOutCurve = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004E80 File Offset: 0x00003080
		public double mixOutTime
		{
			get
			{
				return this.duration - this.mixOutDuration + this.m_Start;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004EAC File Offset: 0x000030AC
		public double mixOutDuration
		{
			get
			{
				return (!this.hasBlendOut) ? this.easeOutDuration : this.blendOutDuration;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004EE0 File Offset: 0x000030E0
		public float mixOutPercentage
		{
			get
			{
				return (float)(this.mixOutDuration / this.duration);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004F04 File Offset: 0x00003104
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004F1F File Offset: 0x0000311F
		public bool recordable
		{
			get
			{
				return this.m_Recordable;
			}
			internal set
			{
				this.m_Recordable = value;
			}
		}

		/// <summary>
		///   <para>Is the clip locked for editing?</para>
		/// </summary>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004F2C File Offset: 0x0000312C
		public bool locked
		{
			get
			{
				return this.parentTrack != null && this.parentTrack.locked;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004F60 File Offset: 0x00003160
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004F7A File Offset: 0x0000317A
		public bool recordingSelected { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004F84 File Offset: 0x00003184
		public List<string> exposedParameters
		{
			get
			{
				List<string> result;
				if ((result = this.m_ExposedParameterNames) == null)
				{
					result = (this.m_ExposedParameterNames = new List<string>());
				}
				return result;
			}
		}

		/// <summary>
		///   <para>Feature capabilities supported by this clip.</para>
		/// </summary>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004FB4 File Offset: 0x000031B4
		public ClipCaps clipCaps
		{
			get
			{
				ITimelineClipAsset timelineClipAsset = this.asset as ITimelineClipAsset;
				return (timelineClipAsset == null) ? TimelineClip.kDefaultClipCaps : timelineClipAsset.clipCaps;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004FEC File Offset: 0x000031EC
		internal int Hash()
		{
			return this.m_ID.GetHashCode() ^ this.m_Start.GetHashCode() ^ this.m_Duration.GetHashCode() ^ this.m_TimeScale.GetHashCode() ^ this.m_ClipIn.GetHashCode() ^ this.m_PreExtrapolationMode.GetHashCode() ^ this.m_PostExtrapolationMode.GetHashCode();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005080 File Offset: 0x00003280
		public float EvaluateMixOut(double localTime)
		{
			float result;
			if (!this.clipCaps.HasAny(ClipCaps.Blending))
			{
				result = 1f;
			}
			else if (this.mixOutDuration > (double)Mathf.Epsilon)
			{
				float num = (float)(localTime - this.mixOutTime) / (float)this.mixOutDuration;
				num = Mathf.Clamp01(this.mixOutCurve.Evaluate(num));
				result = num;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000050F4 File Offset: 0x000032F4
		public float EvaluateMixIn(double localTime)
		{
			float result;
			if (!this.clipCaps.HasAny(ClipCaps.Blending))
			{
				result = 1f;
			}
			else if (this.mixInDuration > (double)Mathf.Epsilon)
			{
				float num = (float)(localTime - this.m_Start) / (float)this.mixInDuration;
				num = Mathf.Clamp01(this.mixInCurve.Evaluate(num));
				result = num;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005168 File Offset: 0x00003368
		private AnimationCurve GetDefaultMixInCurve()
		{
			return AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005198 File Offset: 0x00003398
		private AnimationCurve GetDefaultMixOutCurve()
		{
			return AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
		}

		/// <summary>
		///   <para>Converts from global time to a clips local time.</para>
		/// </summary>
		/// <param name="time">Global time value.</param>
		/// <returns>
		///   <para>Time local to the clip.</para>
		/// </returns>
		// Token: 0x060000A6 RID: 166 RVA: 0x000051C8 File Offset: 0x000033C8
		public double ToLocalTime(double time)
		{
			double result;
			if (time < 0.0)
			{
				result = time;
			}
			else
			{
				if (this.IsPreExtrapolatedTime(time))
				{
					time = TimelineClip.GetExtrapolatedTime(time - this.m_Start, this.m_PreExtrapolationMode, this.m_Duration);
				}
				else if (this.IsPostExtrapolatedTime(time))
				{
					time = TimelineClip.GetExtrapolatedTime(time - this.m_Start, this.m_PostExtrapolationMode, this.m_Duration);
				}
				else
				{
					time -= this.m_Start;
				}
				time += this.clipIn;
				time *= this.timeScale;
				result = time;
			}
			return result;
		}

		/// <summary>
		///   <para>Converts from global time to a clips local time, ignoring extrapolation.</para>
		/// </summary>
		/// <param name="time">Global time value.</param>
		/// <returns>
		///   <para>Unbound time value relative to the clip.</para>
		/// </returns>
		// Token: 0x060000A7 RID: 167 RVA: 0x0000526C File Offset: 0x0000346C
		public double ToLocalTimeUnbound(double time)
		{
			return (time - this.m_Start + this.clipIn) * this.timeScale;
		}

		/// <summary>
		///   <para>An AnimationClip containing animated parameters.</para>
		/// </summary>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005298 File Offset: 0x00003498
		public AnimationClip animationClip
		{
			get
			{
				AnimationClip result;
				if (this.m_Asset == null)
				{
					result = null;
				}
				else
				{
					AnimationPlayableAsset animationPlayableAsset = this.m_Asset as AnimationPlayableAsset;
					result = ((!(animationPlayableAsset != null)) ? null : animationPlayableAsset.clip);
				}
				return result;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000052EC File Offset: 0x000034EC
		private static double SanitizeTimeValue(double value, double defaultValue)
		{
			double result;
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				Debug.LogError("Invalid time value assigned");
				result = defaultValue;
			}
			else
			{
				result = Math.Max(-TimelineClip.kMaxTimeValue, Math.Min(TimelineClip.kMaxTimeValue, value));
			}
			return result;
		}

		/// <summary>
		///   <para>The extrapolation mode for time beyond the end of the clip.</para>
		/// </summary>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005340 File Offset: 0x00003540
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00005372 File Offset: 0x00003572
		public TimelineClip.ClipExtrapolation postExtrapolationMode
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Extrapolation)) ? TimelineClip.ClipExtrapolation.None : this.m_PostExtrapolationMode;
			}
			internal set
			{
				this.m_PostExtrapolationMode = ((!this.clipCaps.HasAny(ClipCaps.Extrapolation)) ? TimelineClip.ClipExtrapolation.None : value);
			}
		}

		/// <summary>
		///   <para>The extrapolation mode for the time before the start of the clip.</para>
		/// </summary>
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005394 File Offset: 0x00003594
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000053C6 File Offset: 0x000035C6
		public TimelineClip.ClipExtrapolation preExtrapolationMode
		{
			get
			{
				return (!this.clipCaps.HasAny(ClipCaps.Extrapolation)) ? TimelineClip.ClipExtrapolation.None : this.m_PreExtrapolationMode;
			}
			internal set
			{
				this.m_PreExtrapolationMode = ((!this.clipCaps.HasAny(ClipCaps.Extrapolation)) ? TimelineClip.ClipExtrapolation.None : value);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000053E7 File Offset: 0x000035E7
		internal void SetPostExtrapolationTime(double time)
		{
			this.m_PostExtrapolationTime = time;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000053F1 File Offset: 0x000035F1
		internal void SetPreExtrapolationTime(double time)
		{
			this.m_PreExtrapolationTime = time;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000053FC File Offset: 0x000035FC
		public bool IsExtrapolatedTime(double sequenceTime)
		{
			return this.IsPreExtrapolatedTime(sequenceTime) || this.IsPostExtrapolatedTime(sequenceTime);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005428 File Offset: 0x00003628
		public bool IsPreExtrapolatedTime(double sequenceTime)
		{
			return this.preExtrapolationMode != TimelineClip.ClipExtrapolation.None && sequenceTime < this.m_Start && sequenceTime >= this.m_Start - this.m_PreExtrapolationTime;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005470 File Offset: 0x00003670
		public bool IsPostExtrapolatedTime(double sequenceTime)
		{
			return this.postExtrapolationMode != TimelineClip.ClipExtrapolation.None && sequenceTime > this.end && sequenceTime - this.end < this.m_PostExtrapolationTime;
		}

		/// <summary>
		///   <para>The start time of the clip when extrapolation is considered.</para>
		/// </summary>
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000054B0 File Offset: 0x000036B0
		public double extrapolatedStart
		{
			get
			{
				double result;
				if (this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					result = this.m_Start - this.m_PreExtrapolationTime;
				}
				else
				{
					result = this.m_Start;
				}
				return result;
			}
		}

		/// <summary>
		///   <para>The length of the clip including extrapolation.</para>
		/// </summary>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000054EC File Offset: 0x000036EC
		public double extrapolatedDuration
		{
			get
			{
				double num = this.m_Duration;
				if (this.m_PostExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					num += Math.Min(this.m_PostExtrapolationTime, TimelineClip.kMaxTimeValue);
				}
				if (this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					num += this.m_PreExtrapolationTime;
				}
				return num;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000553C File Offset: 0x0000373C
		private static double GetExtrapolatedTime(double time, TimelineClip.ClipExtrapolation mode, double duration)
		{
			double result;
			if (duration == 0.0)
			{
				result = 0.0;
			}
			else
			{
				switch (mode)
				{
				case TimelineClip.ClipExtrapolation.Hold:
					if (time < 0.0)
					{
						return 0.0;
					}
					if (time > duration)
					{
						return duration;
					}
					break;
				case TimelineClip.ClipExtrapolation.Loop:
					if (time < 0.0)
					{
						time = duration - -time % duration;
					}
					else if (time > duration)
					{
						time %= duration;
					}
					break;
				case TimelineClip.ClipExtrapolation.PingPong:
					if (time < 0.0)
					{
						time = duration * 2.0 - -time % (duration * 2.0);
						time = duration - Math.Abs(time - duration);
					}
					else
					{
						time %= duration * 2.0;
						time = duration - Math.Abs(time - duration);
					}
					break;
				}
				result = time;
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005657 File Offset: 0x00003857
		internal void AllocateAnimatedParameterCurves()
		{
			if (this.m_AnimationCurves == null)
			{
				this.m_AnimationCurves = new AnimationClip();
				this.m_AnimationCurves.legacy = true;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005684 File Offset: 0x00003884
		internal void ClearAnimatedParameterCurves()
		{
			this.m_AnimationCurves = null;
		}

		// Token: 0x0400003E RID: 62
		public static readonly ClipCaps kDefaultClipCaps = ClipCaps.Blending;

		// Token: 0x0400003F RID: 63
		public static readonly float kDefaultClipDurationInSeconds = 5f;

		// Token: 0x04000040 RID: 64
		public static readonly double kTimeScaleMin = 0.001;

		// Token: 0x04000041 RID: 65
		public static readonly double kTimeScaleMax = 1000.0;

		// Token: 0x04000042 RID: 66
		internal static readonly double kMinDuration = 0.016666666666666666;

		// Token: 0x04000043 RID: 67
		internal static readonly double kMaxTimeValue = 1000000.0;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private double m_Start;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		private double m_ClipIn;

		// Token: 0x04000046 RID: 70
		[SerializeField]
		private Object m_Asset;

		// Token: 0x04000047 RID: 71
		[SerializeField]
		private Object m_UnderlyingAsset;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		[FormerlySerializedAs("m_HackDuration")]
		private double m_Duration;

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private int m_ParentID;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		private double m_TimeScale = 1.0;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		private AnimationCurve m_BlendCurve;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		private TrackAsset m_ParentTrack;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		private double m_EaseInDuration = 0.0;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		private double m_EaseOutDuration = 0.0;

		// Token: 0x0400004F RID: 79
		[SerializeField]
		private double m_BlendInDuration = -1.0;

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private double m_BlendOutDuration = -1.0;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private AnimationCurve m_MixInCurve = null;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		private AnimationCurve m_MixOutCurve = null;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private TimelineClip.BlendCurveMode m_BlendInCurveMode = TimelineClip.BlendCurveMode.Auto;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private TimelineClip.BlendCurveMode m_BlendOutCurveMode = TimelineClip.BlendCurveMode.Auto;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		private List<string> m_ExposedParameterNames;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		private AnimationClip m_AnimationCurves;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		public int m_ID = -1;

		// Token: 0x04000058 RID: 88
		[SerializeField]
		private bool m_Recordable = false;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		private bool m_NestedAsset = false;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		private int m_NestedOutputIndex = 0;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		private TimelineClip.ClipExtrapolation m_PostExtrapolationMode;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private TimelineClip.ClipExtrapolation m_PreExtrapolationMode;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private double m_PostExtrapolationTime;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private double m_PreExtrapolationTime;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		private bool m_Selected;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private bool m_InlineCurvesSelected;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private string m_DisplayName;

		/// <summary>
		///   <para>How the clip handles time outside it's start and end range.</para>
		/// </summary>
		// Token: 0x02000016 RID: 22
		public enum ClipExtrapolation
		{
			/// <summary>
			///   <para>No extrapolation is applied.</para>
			/// </summary>
			// Token: 0x04000065 RID: 101
			None,
			/// <summary>
			///   <para>Hold the time at the end value of the clip.</para>
			/// </summary>
			// Token: 0x04000066 RID: 102
			Hold,
			/// <summary>
			///   <para>Loop time values outside the start/end range.</para>
			/// </summary>
			// Token: 0x04000067 RID: 103
			Loop,
			// Token: 0x04000068 RID: 104
			PingPong,
			/// <summary>
			///   <para>Lets the underlying asset handle extrapolated time values.</para>
			/// </summary>
			// Token: 0x04000069 RID: 105
			Continue
		}

		// Token: 0x02000017 RID: 23
		public enum BlendCurveMode
		{
			// Token: 0x0400006B RID: 107
			Auto,
			// Token: 0x0400006C RID: 108
			Manual
		}
	}
}

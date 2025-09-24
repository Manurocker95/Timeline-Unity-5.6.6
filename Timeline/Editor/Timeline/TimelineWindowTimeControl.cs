using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000D0 RID: 208
	internal class TimelineWindowTimeControl : IAnimationWindowControl
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00035E94 File Offset: 0x00034294
		public float start
		{
			get
			{
				float result;
				if (this.m_Clip != null)
				{
					result = (float)this.m_Clip.start;
				}
				else
				{
					result = this.m_ClipData.start;
				}
				return result;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00035ED4 File Offset: 0x000342D4
		public float duration
		{
			get
			{
				float result;
				if (this.m_Clip != null)
				{
					result = (float)this.m_Clip.duration;
				}
				else
				{
					result = this.m_ClipData.duration;
				}
				return result;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00035F14 File Offset: 0x00034314
		public TrackAsset track
		{
			get
			{
				TrackAsset result;
				if (this.m_Clip != null)
				{
					result = this.m_Clip.parentTrack;
				}
				else
				{
					result = this.m_ClipData.track;
				}
				return result;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00035F54 File Offset: 0x00034354
		public TimelineWindow window
		{
			get
			{
				return TimelineWindow.instance;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00035F70 File Offset: 0x00034370
		public TimelineWindow.TimelineState state
		{
			get
			{
				TimelineWindow.TimelineState result;
				if (this.window != null)
				{
					result = this.window.state;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00035FA8 File Offset: 0x000343A8
		private void OnStateChange(object sender, TimelineWindow.StateEventArgs arg)
		{
			if (arg.state.dirtyStamp > 0 && this.m_AnimWindowState != null)
			{
				this.m_AnimWindowState.Repaint();
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00035FD8 File Offset: 0x000343D8
		public void Init(TimelineWindow window, AnimationWindowState state, TimelineClip clip)
		{
			this.m_Clip = clip;
			this.m_AnimWindowState = state;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00035FE9 File Offset: 0x000343E9
		public void Init(TimelineWindow window, AnimationWindowState state, TimelineWindowTimeControl.ClipData clip)
		{
			this.m_ClipData = clip;
			this.m_AnimWindowState = state;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00035FFA File Offset: 0x000343FA
		public override void OnEnable()
		{
			if (this.window != null)
			{
				this.window.OnStateChange += this.OnStateChange;
			}
			base.OnEnable();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0003602B File Offset: 0x0003442B
		public void OnDisable()
		{
			if (this.window != null)
			{
				this.window.OnStateChange -= this.OnStateChange;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00036058 File Offset: 0x00034458
		public override AnimationKeyTime time
		{
			get
			{
				AnimationKeyTime result;
				if (this.state == null)
				{
					result = AnimationKeyTime.Time(0f, 0f);
				}
				else
				{
					result = AnimationKeyTime.Time((float)this.state.time - this.start, this.state.frameRate);
				}
				return result;
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000360B0 File Offset: 0x000344B0
		private void ChangeTime(float time)
		{
			if (this.state != null && this.state.currentDirector != null)
			{
				this.state.time = (double)time + (double)this.start;
				this.window.Repaint();
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00036101 File Offset: 0x00034501
		private void ChangeFrame(int frame)
		{
			if (this.state != null)
			{
				this.state.frame = frame;
				this.window.Repaint();
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00036128 File Offset: 0x00034528
		public override void GoToTime(float time)
		{
			this.ChangeTime(time);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00036132 File Offset: 0x00034532
		public override void GoToFrame(int frame)
		{
			this.ChangeFrame(frame);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0003613C File Offset: 0x0003453C
		public override void StartScrubTime()
		{
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0003613F File Offset: 0x0003453F
		public override void EndScrubTime()
		{
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00036142 File Offset: 0x00034542
		public override void ScrubTime(float time)
		{
			this.ChangeTime(time);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0003614C File Offset: 0x0003454C
		public override void GoToPreviousFrame()
		{
			if (this.state != null)
			{
				this.ChangeTime((float)(this.state.frame - 1));
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0003616E File Offset: 0x0003456E
		public override void GoToNextFrame()
		{
			if (this.state != null)
			{
				this.ChangeTime((float)(this.state.frame + 1));
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00036190 File Offset: 0x00034590
		private AnimationWindowCurve[] GetCurves()
		{
			List<AnimationWindowCurve> list = (!this.m_AnimWindowState.showCurveEditor || this.m_AnimWindowState.activeCurves.Count <= 0) ? this.m_AnimWindowState.allCurves : this.m_AnimWindowState.activeCurves;
			return list.ToArray();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000361F0 File Offset: 0x000345F0
		public override void GoToPreviousKeyframe()
		{
			float previousKeyframeTime = AnimationWindowUtility.GetPreviousKeyframeTime(this.GetCurves(), this.time.time, this.m_AnimWindowState.clipFrameRate);
			this.GoToTime(this.m_AnimWindowState.SnapToFrame(previousKeyframeTime, 2));
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00036238 File Offset: 0x00034638
		public override void GoToNextKeyframe()
		{
			float nextKeyframeTime = AnimationWindowUtility.GetNextKeyframeTime(this.GetCurves(), this.time.time, this.m_AnimWindowState.clipFrameRate);
			this.GoToTime(this.m_AnimWindowState.SnapToFrame(nextKeyframeTime, 2));
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0003627E File Offset: 0x0003467E
		public override void GoToFirstKeyframe()
		{
			this.GoToTime(0f);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0003628C File Offset: 0x0003468C
		public override void GoToLastKeyframe()
		{
			this.GoToTime(this.duration);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0003629C File Offset: 0x0003469C
		public override bool canPlay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x000362B4 File Offset: 0x000346B4
		public override bool playing
		{
			get
			{
				return this.state != null && this.state.playing;
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000362E2 File Offset: 0x000346E2
		private void SetPlaybackState(bool playbackState)
		{
			if (this.state != null && playbackState != this.state.playing)
			{
				TimelineWindow.instance.Simulate(playbackState);
				this.state.playing = playbackState;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0003631D File Offset: 0x0003471D
		public override void StartPlayback()
		{
			this.SetPlaybackState(true);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00036327 File Offset: 0x00034727
		public override void StopPlayback()
		{
			this.SetPlaybackState(false);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00036334 File Offset: 0x00034734
		public override bool PlaybackUpdate()
		{
			return false;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0003634C File Offset: 0x0003474C
		public override bool canRecord
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00036364 File Offset: 0x00034764
		public override bool recording
		{
			get
			{
				return this.state != null && this.state.recording;
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00036392 File Offset: 0x00034792
		public override void StartRecording(Object targetObject)
		{
			if (this.canRecord)
			{
				if (!Application.isPlaying)
				{
					if (this.state != null)
					{
						this.state.ArmForRecord(this.track);
					}
				}
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000363D0 File Offset: 0x000347D0
		public override void StopRecording()
		{
			if (!Application.isPlaying)
			{
				if (this.state != null)
				{
					this.state.UnarmForRecord(this.track);
				}
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000363FE File Offset: 0x000347FE
		public override void OnSelectionChanged()
		{
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00036401 File Offset: 0x00034801
		public override void ResampleAnimation()
		{
		}

		// Token: 0x04000446 RID: 1094
		[SerializeField]
		private TimelineWindowTimeControl.ClipData m_ClipData;

		// Token: 0x04000447 RID: 1095
		[SerializeField]
		private TimelineClip m_Clip;

		// Token: 0x04000448 RID: 1096
		[SerializeField]
		private AnimationWindowState m_AnimWindowState;

		// Token: 0x020000D1 RID: 209
		[Serializable]
		public struct ClipData
		{
			// Token: 0x04000449 RID: 1097
			public float start;

			// Token: 0x0400044A RID: 1098
			public float duration;

			// Token: 0x0400044B RID: 1099
			public TrackAsset track;
		}
	}
}

using System;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x02000083 RID: 131
	internal class TimelineClipTimeField : Control, IBounds
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x00021665 File Offset: 0x0001FA65
		public TimelineClipTimeField(TimelineClipGUI theClip, TimelineClipTimeField.Mode mode)
		{
			this.m_Mode = mode;
			this.m_Clip = theClip;
			this.m_Styles = DirectorStyles.Instance;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00021688 File Offset: 0x0001FA88
		public override Rect bounds
		{
			get
			{
				return this.m_Bounds;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000216A4 File Offset: 0x0001FAA4
		public Rect boundingRect
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000216C0 File Offset: 0x0001FAC0
		public TimelineClipGUI clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000216DC File Offset: 0x0001FADC
		public override bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			this.Draw(this.m_Bounds, state);
			return true;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00021700 File Offset: 0x0001FB00
		public void Draw(Rect clientRect, TimelineWindow.TimelineState state)
		{
			this.m_Bounds = clientRect;
			string text = state.TimeAsString((this.m_Mode != TimelineClipTimeField.Mode.Start) ? (this.m_Clip.clip.start + this.m_Clip.clip.duration) : this.m_Clip.clip.start, "F2");
			this.m_Bounds.width = this.m_Styles.timecodeSmall.CalcSize(new GUIContent(text)).x;
			GUI.SetNextControlName("sequencerKeyboardFocus");
			text = EditorGUI.DelayedTextField(this.m_Bounds, GUIContent.none, this.m_Clip.clip.m_ID, text, this.m_Styles.timecodeSmall);
			double num = 0.0;
			bool flag = TimelineClipTimeField.parseTime(out num, text, state.frameRate, state.timeInFrames);
			if (flag)
			{
				if (this.m_Mode == TimelineClipTimeField.Mode.Start)
				{
					this.m_Clip.clip.start = num;
				}
				else
				{
					this.m_Clip.clip.duration = num - this.m_Clip.clip.start;
				}
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00021830 File Offset: 0x0001FC30
		private static bool parseTime(out double timeResult, string timeValue, float frameRate, bool timeInFrames)
		{
			if (timeInFrames)
			{
				try
				{
					timeResult = Convert.ToDouble(timeValue) / (double)frameRate;
					return true;
				}
				catch
				{
					timeResult = 0.0;
					return false;
				}
			}
			string value = timeValue.Replace(":", ".");
			bool result;
			try
			{
				timeResult = Convert.ToDouble(value);
				result = true;
			}
			catch
			{
				timeResult = 0.0;
				result = false;
			}
			return result;
		}

		// Token: 0x040002C7 RID: 711
		private Rect m_Bounds;

		// Token: 0x040002C8 RID: 712
		private TimelineClipGUI m_Clip;

		// Token: 0x040002C9 RID: 713
		private DirectorStyles m_Styles;

		// Token: 0x040002CA RID: 714
		private TimelineClipTimeField.Mode m_Mode;

		// Token: 0x02000084 RID: 132
		public enum Mode
		{
			// Token: 0x040002CC RID: 716
			Start,
			// Token: 0x040002CD RID: 717
			End
		}
	}
}

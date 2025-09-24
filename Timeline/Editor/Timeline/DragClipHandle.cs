using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000060 RID: 96
	internal class DragClipHandle : Manipulator
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0001BA50 File Offset: 0x00019E50
		protected virtual void OnAttractedEdge(TimelineClip clip, AttractedEdge edge, double time, double duration)
		{
			if (edge == AttractedEdge.Right)
			{
				clip.duration = time - clip.start;
				clip.duration = Math.Max(clip.duration, TimelineClip.kMinDuration);
			}
			else if (edge == AttractedEdge.Left)
			{
				double num = time - clip.start;
				double newValue = clip.clipIn + num;
				if (this.SetClipIn(clip, newValue, num))
				{
					this.m_FrameSnap.Reset();
				}
			}
			else
			{
				clip.start = time;
				clip.duration = duration;
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001BADC File Offset: 0x00019EDC
		private void ManipulateBlending(TimelineClipHandle handle, Event evt, float scale)
		{
			if (handle.direction == TimelineClipHandle.DragDirection.Right)
			{
				float num = evt.delta.x / scale;
				handle.clip.clip.easeOutDuration -= (double)num;
			}
			else
			{
				float num2 = evt.delta.x / scale;
				handle.clip.clip.easeInDuration += (double)num2;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001BB54 File Offset: 0x00019F54
		public override void Init(IControl parent)
		{
			this.m_IsCaptured = false;
			this.m_Ripple = null;
			this.m_OriginalDuration = 0.0;
			this.m_OriginalTimeScale = 0.0;
			this.m_UndoSaved = false;
			this.m_MagnetEngine = null;
			this.m_FrameSnap = new FrameSnap();
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineClipHandle timelineClipHandle = target as TimelineClipHandle;
				bool result;
				if (timelineClipHandle == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.OnMouseDown(evt, state, timelineClipHandle);
					result = base.ConsumeEvent();
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineClipHandle timelineClipHandle = target as TimelineClipHandle;
				bool result;
				if (!this.m_IsCaptured || timelineClipHandle == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.OnMouseUp(evt, state, timelineClipHandle);
					result = base.ConsumeEvent();
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				this.m_OverlayText = "";
				this.m_OverlayStrings.Clear();
				TimelineClipHandle timelineClipHandle = target as TimelineClipHandle;
				bool result;
				if (!this.m_IsCaptured || timelineClipHandle == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.OnMouseDrag(evt, state, timelineClipHandle);
					this.RefreshOverlayStrings(timelineClipHandle);
					if (Selection.activeObject != null)
					{
						EditorUtility.SetDirty(Selection.activeObject);
					}
					state.Evaluate();
					result = base.ConsumeEvent();
				}
				return result;
			};
			parent.Overlay += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineClipHandle timelineClipHandle = target as TimelineClipHandle;
				bool result;
				if (timelineClipHandle == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.OnOverlay(evt, state, timelineClipHandle);
					result = base.ConsumeEvent();
				}
				return result;
			};
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001BBEF File Offset: 0x00019FEF
		protected virtual void OnMouseUp(Event evt, TimelineWindow.TimelineState state, TimelineClipHandle handle)
		{
			this.m_UndoSaved = false;
			this.m_MagnetEngine = null;
			state.captured.Clear();
			this.m_Ripple = null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001BC14 File Offset: 0x0001A014
		protected virtual void OnMouseDown(Event evt, TimelineWindow.TimelineState state, TimelineClipHandle handle)
		{
			if (!state.captured.Contains(handle))
			{
				state.captured.Add(handle);
			}
			this.m_IsCaptured = true;
			this.m_UndoSaved = false;
			if (!handle.clip.selected)
			{
				handle.clip.selected = true;
			}
			if (state.edgeSnaps)
			{
				this.m_MagnetEngine = new MagnetEngine(state, handle.clip.clip, new OnAttractedEdge(this.OnAttractedEdge));
			}
			this.m_OriginalDuration = handle.clip.clip.duration;
			this.m_OriginalTimeScale = handle.clip.clip.timeScale;
			this.m_FrameSnap.Reset();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0001BCD0 File Offset: 0x0001A0D0
		protected virtual void OnMouseDrag(Event evt, TimelineWindow.TimelineState state, TimelineClipHandle handle)
		{
			if (evt.modifiers == 2)
			{
				this.ManipulateBlending(handle, evt, state.timeAreaScale.x);
			}
			else
			{
				if (!this.m_UndoSaved)
				{
					TimelineHelpers.PushUndo(handle.clip.parentTrack.track, "clip.trimclip");
					this.m_UndoSaved = true;
				}
				float num = evt.delta.x / state.timeAreaScale.x;
				if (this.m_Ripple != null)
				{
					this.m_Ripple.Run(num, state);
				}
				if (handle.direction == TimelineClipHandle.DragDirection.Right)
				{
					double val = this.m_FrameSnap.ApplyOffset(handle.clip.clip.duration, num, state);
					handle.clip.clip.duration = Math.Max(val, TimelineClip.kMinDuration);
					if (TimelineClipCapsExtensions.SupportsSpeedMultiplier(handle.clip.clip) && evt.modifiers == 1)
					{
						double num2 = this.m_OriginalDuration / handle.clip.clip.duration;
						handle.clip.clip.timeScale = this.m_OriginalTimeScale * num2;
					}
				}
				else
				{
					double clipIn = handle.clip.clip.clipIn;
					if (clipIn > 0.0 && clipIn + (double)num < 0.0)
					{
						num = (float)(-(float)clipIn);
					}
					if (this.m_MagnetEngine == null || !this.m_MagnetEngine.isAttracted)
					{
						double newValue = this.m_FrameSnap.ApplyOffset(clipIn, num, state);
						this.SetClipIn(handle.clip.clip, newValue, this.m_FrameSnap.lastOffsetApplied);
					}
				}
				if (this.m_MagnetEngine != null && evt.modifiers != 1)
				{
					this.m_MagnetEngine.Snap(evt.delta.x);
				}
				handle.clip.clip.duration = Math.Max(handle.clip.clip.duration, TimelineClip.kMinDuration);
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001BEF4 File Offset: 0x0001A2F4
		protected virtual void OnOverlay(Event evt, TimelineWindow.TimelineState state, TimelineClipHandle handle)
		{
			if (this.m_OverlayStrings.Count > 0)
			{
				Vector2 vector = state.styles.tinyFont.CalcSize(new GUIContent(this.m_OverlayStrings[0]));
				Rect rect;
				rect..ctor(evt.mousePosition.x - vector.x / 2f, handle.clip.parentTrack.boundingRect.y + handle.clip.parentTrack.boundingRect.height + 42f, vector.x, 40f);
				GUILayout.BeginArea(rect);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUI.Label(rect, GUIContent.none, state.styles.sequenceClip);
				foreach (string text in this.m_OverlayStrings)
				{
					GUILayout.Label(text, state.styles.tinyFont, new GUILayoutOption[0]);
				}
				GUILayout.EndVertical();
				GUILayout.EndArea();
			}
			if (this.m_MagnetEngine != null)
			{
				this.m_MagnetEngine.OnGUI();
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001C04C File Offset: 0x0001A44C
		protected void RefreshOverlayStrings(TimelineClipHandle handle)
		{
			double num = (this.m_OriginalTimeScale - handle.clip.clip.timeScale) * 100.0;
			double num2 = handle.clip.clip.duration - this.m_OriginalDuration;
			this.m_OverlayText = "";
			if (Math.Abs(num) > DragClipHandle.kEpsilon)
			{
				this.m_OverlayText = "speed: " + (handle.clip.clip.timeScale * 100.0).ToString("f2") + "%";
				this.m_OverlayText += " (";
				if (num > 0.0)
				{
					this.m_OverlayText += "+";
				}
				this.m_OverlayText = this.m_OverlayText + num.ToString("f2") + "%";
				this.m_OverlayText += ")";
				this.m_OverlayStrings.Add(this.m_OverlayText);
			}
			this.m_OverlayText = "";
			if (Math.Abs(num2) > DragClipHandle.kEpsilon)
			{
				this.m_OverlayText = this.m_OverlayText + " duration: " + handle.clip.clip.duration.ToString("f2") + "s";
				this.m_OverlayText += " (";
				if (num2 > 0.0)
				{
					this.m_OverlayText += "+";
				}
				this.m_OverlayText = this.m_OverlayText + num2.ToString("f2") + "s";
				this.m_OverlayText += ")";
				this.m_OverlayStrings.Add(this.m_OverlayText);
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001C24C File Offset: 0x0001A64C
		private bool SetClipIn(TimelineClip sourceClip, double newValue, double startDelta)
		{
			bool result;
			if (sourceClip.start + startDelta < 0.0)
			{
				result = false;
			}
			else
			{
				float num = (float)sourceClip.clipAssetDuration;
				double num2 = sourceClip.duration - startDelta;
				if (newValue >= 0.0 && newValue < (double)num && num2 > 0.0)
				{
					sourceClip.clipIn = newValue;
					sourceClip.start += startDelta;
					sourceClip.duration = num2;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04000267 RID: 615
		protected FrameSnap m_FrameSnap;

		// Token: 0x04000268 RID: 616
		protected bool m_IsCaptured;

		// Token: 0x04000269 RID: 617
		protected Ripple m_Ripple;

		// Token: 0x0400026A RID: 618
		protected string m_OverlayText = "";

		// Token: 0x0400026B RID: 619
		protected double m_OriginalDuration;

		// Token: 0x0400026C RID: 620
		protected double m_OriginalTimeScale;

		// Token: 0x0400026D RID: 621
		protected bool m_UndoSaved;

		// Token: 0x0400026E RID: 622
		protected MagnetEngine m_MagnetEngine;

		// Token: 0x0400026F RID: 623
		protected List<string> m_OverlayStrings = new List<string>();

		// Token: 0x04000270 RID: 624
		private static readonly double kEpsilon = 1E-07;
	}
}

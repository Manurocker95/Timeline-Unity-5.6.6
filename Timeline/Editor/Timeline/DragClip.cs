using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200005B RID: 91
	internal class DragClip : Manipulator
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0001A3CF File Offset: 0x000187CF
		private static void OnAttractedEdge(TimelineClip clip, AttractedEdge edge, double time, double duration)
		{
			if (edge == AttractedEdge.Left || edge == AttractedEdge.None)
			{
				clip.start = time;
			}
			else
			{
				clip.start += time - clip.end;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001A404 File Offset: 0x00018804
		private bool IsDriver(TimelineClipGUI target, Event evt, TimelineWindow.TimelineState state)
		{
			return !state.selection.isEmpty && state.selection.Contains(target);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001A43C File Offset: 0x0001883C
		public override void Init(IControl parent)
		{
			bool isCaptured = false;
			MagnetEngine magnetEngine = null;
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				this.m_UndoSet = false;
				bool result;
				if (evt.modifiers == 4 || evt.button == 2 || evt.button == 1)
				{
					result = this.IgnoreEvent();
				}
				else if (!state.selection.IsMouseHovering())
				{
					result = this.IgnoreEvent();
				}
				else
				{
					TimelineClipGUI timelineClipGUI = target as TimelineClipGUI;
					if (!timelineClipGUI.selected)
					{
						result = this.IgnoreEvent();
					}
					else
					{
						this.m_IsDragDriver = this.IsDriver(timelineClipGUI, evt, state);
						this.m_FrameSnap.Reset();
						state.captured.Add(target as IControl);
						isCaptured = true;
						this.m_MouseDownPosition = evt.mousePosition;
						if (this.m_IsDragDriver)
						{
							this.m_DragPixelOffset = Vector2.zero;
							this.m_IsDragDriverActive = false;
							if (!state.selection.isMultiSelect && state.edgeSnaps)
							{
								TimelineClip clip = timelineClipGUI.clip;
								if (DragClip.<>f__mg$cache0 == null)
								{
									DragClip.<>f__mg$cache0 = new OnAttractedEdge(DragClip.OnAttractedEdge);
								}
								magnetEngine = new MagnetEngine(state, clip, DragClip.<>f__mg$cache0);
							}
							if (TimelineClipCapsExtensions.SupportsSpeedMultiplier(timelineClipGUI.clip) && evt.modifiers == 1)
							{
								this.m_Ripple = new Ripple(Ripple.RippleDirection.All);
								this.m_Ripple.Init(timelineClipGUI, state);
							}
						}
						result = this.ConsumeEvent();
					}
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!isCaptured)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					magnetEngine = null;
					if (this.m_IsVerticalDrag)
					{
						state.captured.Clear();
						this.m_IsVerticalDrag = false;
						state.isDragging = false;
						if (this.m_HasValidDropTarget && this.m_DropTarget != null)
						{
							TimelineClipGUI gui = (TimelineClipGUI)target;
							if (TrackExtensions.MoveClipToTrack(gui.clip, this.m_DropTarget.track))
							{
								gui.clip.start = (double)state.PixelToTime(this.m_PreviewRect.x);
								bool flag = !this.m_DropTarget.track.clips.Any((TimelineClip c) => c != gui.clip && c.start > gui.clip.start + gui.clip.duration);
								if (flag)
								{
									double num = double.MinValue;
									bool flag2 = false;
									foreach (TimelineClip timelineClip in this.m_DropTarget.track.clips)
									{
										if (timelineClip != gui.clip)
										{
											num = Math.Max(num, timelineClip.start + timelineClip.duration);
											flag2 = true;
										}
									}
									if (flag2)
									{
										gui.clip.start = num;
									}
								}
								gui.parentTrack.SortClipsByStartTime();
								this.m_DropTarget.SortClipsByStartTime();
								state.Refresh();
							}
						}
					}
					state.Evaluate();
					state.captured.Remove(target as IControl);
					isCaptured = false;
					this.m_Ripple = null;
					this.m_IsDragDriver = false;
					result = this.ConsumeEvent();
				}
				return result;
			};
			parent.DragExited += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				this.m_IsVerticalDrag = false;
				return this.IgnoreEvent();
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!isCaptured)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					TimelineClipGUI timelineClipGUI = (TimelineClipGUI)target;
					if (state.selection.Count == 1 && this.m_Ripple == null)
					{
						TimelineClipGUI timelineClipGUI2 = state.selection[0] as TimelineClipGUI;
						if (timelineClipGUI2 != null)
						{
							TimelineTrackGUI dropTargetAt = this.GetDropTargetAt(state, evt.mousePosition);
							if (dropTargetAt != null)
							{
								bool flag = dropTargetAt != timelineClipGUI2.parentTrack;
								if (flag && !this.m_IsVerticalDrag)
								{
									state.isDragging = true;
									this.m_HasValidDropTarget = false;
									this.m_DropTarget = null;
									this.m_MouseDownPosition.x = evt.mousePosition.x;
									this.m_PreviewOffset = this.m_MouseDownPosition - timelineClipGUI.bounds.position;
								}
								else if (this.m_IsVerticalDrag && !flag)
								{
									state.isDragging = false;
									this.m_HasValidDropTarget = false;
									this.m_DropTarget = null;
									this.m_DragPixelOffset = evt.mousePosition - this.m_MouseDownPosition - evt.delta;
								}
								this.m_IsVerticalDrag = flag;
							}
						}
					}
					if (this.m_IsVerticalDrag)
					{
						this.m_PreviewRect = new Rect(evt.mousePosition.x, evt.mousePosition.y, timelineClipGUI.bounds.width, timelineClipGUI.bounds.height);
						DragClip $this = this;
						$this.m_PreviewRect.position = $this.m_PreviewRect.position - this.m_PreviewOffset;
						this.UpdateDragTarget(timelineClipGUI, evt.mousePosition, state);
						result = this.ConsumeEvent();
					}
					else
					{
						if (this.m_IsDragDriver)
						{
							this.m_DragPixelOffset += evt.delta;
							this.m_IsDragDriverActive |= (Math.Abs(this.m_DragPixelOffset.x) > DragClip.kDragBufferInPixels);
							if (this.m_IsDragDriverActive)
							{
								float delta = this.m_DragPixelOffset.x / state.timeAreaScale.x;
								this.m_DragPixelOffset = Vector3.zero;
								this.SetUndo(state);
								if (state.selection.isMultiSelect)
								{
									double currentValue = (from x in state.selection.EditableClips()
									select x.clip.start).DefaultIfEmpty(timelineClipGUI.clip.start).Min();
									this.m_FrameSnap.ApplyOffset(currentValue, delta, state);
									foreach (TimelineClipGUI timelineClipGUI3 in state.selection.EditableClips())
									{
										if (timelineClipGUI3.clip.start + this.m_FrameSnap.lastOffsetApplied < 0.0)
										{
											break;
										}
										timelineClipGUI3.clip.start += this.m_FrameSnap.lastOffsetApplied;
									}
								}
								else
								{
									double num = this.m_FrameSnap.ApplyOffset(timelineClipGUI.clip.start, delta, state);
									if (num < 0.0)
									{
										num = 0.0;
									}
									timelineClipGUI.clip.start = num;
								}
								if (magnetEngine != null)
								{
									magnetEngine.Snap(evt.delta.x);
								}
								if (this.m_Ripple != null)
								{
									this.m_Ripple.Run((float)this.m_FrameSnap.lastOffsetApplied, state);
								}
								if (timelineClipGUI.editorClip != null)
								{
									EditorUtility.SetDirty(timelineClipGUI.editorClip);
								}
								timelineClipGUI.parentTrack.SortClipsByStartTime();
								state.Evaluate();
							}
						}
						result = this.ConsumeEvent();
					}
				}
				return result;
			};
			parent.Overlay += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				if (magnetEngine != null)
				{
					magnetEngine.OnGUI();
				}
				bool result;
				if (this.m_IsVerticalDrag)
				{
					TimelineClipGUI timelineClipGUI = (TimelineClipGUI)target;
					Color color = (!this.m_HasValidDropTarget) ? DirectorStyles.Instance.customSkin.colorInvalidDropTarget : DirectorStyles.Instance.customSkin.colorValidDropTarget;
					timelineClipGUI.DrawDragPreview(this.m_PreviewRect, color);
					result = this.ConsumeEvent();
				}
				else if (this.m_IsDragDriver)
				{
					IEnumerable<TimelineClipGUI> enumerable = state.selection.EditableClips();
					double num = double.MaxValue;
					double num2 = double.MinValue;
					foreach (TimelineClipGUI timelineClipGUI2 in enumerable)
					{
						if (timelineClipGUI2.start < num)
						{
							num = timelineClipGUI2.start;
						}
						if (timelineClipGUI2.end > num2)
						{
							num2 = timelineClipGUI2.end;
						}
					}
					this.m_SelectionIndicator.Draw(num, num2, magnetEngine);
					result = this.ConsumeEvent();
				}
				else
				{
					result = this.IgnoreEvent();
				}
				return result;
			};
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001A4C0 File Offset: 0x000188C0
		private void SetUndo(TimelineWindow.TimelineState state)
		{
			if (!this.m_UndoSet)
			{
				IEnumerable<TrackAsset> enumerable = state.selection.TracksWithSelectedClips();
				foreach (TrackAsset thingToDirty in enumerable)
				{
					TimelineHelpers.PushUndo(thingToDirty, "clip.drag");
				}
				this.m_UndoSet = true;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0001A540 File Offset: 0x00018940
		private void UpdateDragTarget(TimelineClipGUI uiClip, Vector2 point, TimelineWindow.TimelineState state)
		{
			List<IBounds> source = state.quadTree.ContainedBy(new Rect(point.x, point.y, 1f, 1f));
			TimelineTrackGUI timelineTrackGUI = source.OfType<TimelineTrackGUI>().FirstOrDefault((TimelineTrackGUI t) => this.ValidateClipDrag(t.track, uiClip.clip));
			this.m_HasValidDropTarget = (timelineTrackGUI != null && timelineTrackGUI.track.IsCompatibleWithClip(uiClip.clip));
			if (this.m_HasValidDropTarget)
			{
				this.m_DropTarget = timelineTrackGUI;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0001A5DC File Offset: 0x000189DC
		private TimelineTrackGUI GetDropTargetAt(TimelineWindow.TimelineState state, Vector2 point)
		{
			List<IBounds> source = state.quadTree.ContainedBy(new Rect(point.x, point.y, 1f, 1f));
			return source.OfType<TimelineTrackGUI>().FirstOrDefault<TimelineTrackGUI>();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0001A628 File Offset: 0x00018A28
		private bool ValidateClipDrag(TrackAsset target, TimelineClip clip)
		{
			TrackAsset parentTrack = clip.parentTrack;
			return !target.locked && target.mediaType == parentTrack.mediaType;
		}

		// Token: 0x04000248 RID: 584
		private static readonly float kDragBufferInPixels = 3f;

		// Token: 0x04000249 RID: 585
		private Rect m_PreviewRect;

		// Token: 0x0400024A RID: 586
		private Vector2 m_PreviewOffset = Vector2.zero;

		// Token: 0x0400024B RID: 587
		private Vector2 m_MouseDownPosition = Vector2.zero;

		// Token: 0x0400024C RID: 588
		private bool m_HasValidDropTarget;

		// Token: 0x0400024D RID: 589
		private TimelineTrackGUI m_DropTarget;

		// Token: 0x0400024E RID: 590
		private Ripple m_Ripple;

		// Token: 0x0400024F RID: 591
		private readonly FrameSnap m_FrameSnap = new FrameSnap();

		// Token: 0x04000250 RID: 592
		private bool m_IsDragDriver;

		// Token: 0x04000251 RID: 593
		private bool m_IsVerticalDrag;

		// Token: 0x04000252 RID: 594
		private Vector2 m_DragPixelOffset;

		// Token: 0x04000253 RID: 595
		private bool m_IsDragDriverActive;

		// Token: 0x04000254 RID: 596
		private bool m_UndoSet;

		// Token: 0x04000255 RID: 597
		private readonly DragClip.ClipSelectionIndicator m_SelectionIndicator = new DragClip.ClipSelectionIndicator();

		// Token: 0x04000256 RID: 598
		[CompilerGenerated]
		private static OnAttractedEdge <>f__mg$cache0;

		// Token: 0x0200005C RID: 92
		internal class ClipSelectionIndicator
		{
			// Token: 0x06000357 RID: 855 RVA: 0x0001A6C4 File Offset: 0x00018AC4
			public void Draw(double beginTime, double endTime, MagnetEngine magnetEngine)
			{
				TimelineWindow.TimelineState state = TimelineWindow.instance.state;
				Rect timeAreaBounds = TimelineWindow.instance.timeAreaBounds;
				timeAreaBounds.xMin = Mathf.Max(TimelineWindow.instance.timeAreaBounds.xMin, state.TimeToTimeAreaPixel(beginTime));
				timeAreaBounds.xMax = state.TimeToTimeAreaPixel(endTime);
				Rect position = TimelineWindow.instance.position;
				using (new GUIViewportScope(TimelineWindow.instance.timeAreaBounds))
				{
					Color textColor = DirectorStyles.Instance.selectedStyle.focused.textColor;
					textColor.a = 0.12f;
					EditorGUI.DrawRect(timeAreaBounds, textColor);
					this.m_BeginSelectionTooltip.text = state.TimeAsString(beginTime, "F2");
					this.m_EndSelectionTooltip.text = state.TimeAsString(endTime, "F2");
					Rect bounds = this.m_BeginSelectionTooltip.bounds;
					bounds.xMin = timeAreaBounds.xMin - bounds.width / 2f;
					bounds.y = timeAreaBounds.y;
					this.m_BeginSelectionTooltip.bounds = bounds;
					bounds = this.m_EndSelectionTooltip.bounds;
					bounds.xMin = timeAreaBounds.xMax - bounds.width / 2f;
					bounds.y = timeAreaBounds.y;
					this.m_EndSelectionTooltip.bounds = bounds;
					if (beginTime >= 0.0)
					{
						this.m_BeginSelectionTooltip.Draw();
					}
					this.m_EndSelectionTooltip.Draw();
				}
				if (beginTime >= 0.0)
				{
					if (magnetEngine == null || !magnetEngine.IsSnappedAtTime((double)state.PixelToTime(timeAreaBounds.xMin)))
					{
						Graphics.DrawDottedLine(new Vector3(timeAreaBounds.xMin, timeAreaBounds.yMax, 0f), new Vector3(timeAreaBounds.xMin, timeAreaBounds.yMax + position.height), 4f, Color.black);
					}
					if (magnetEngine == null || !magnetEngine.IsSnappedAtTime((double)state.PixelToTime(timeAreaBounds.xMax)))
					{
						Graphics.DrawDottedLine(new Vector3(timeAreaBounds.xMax, timeAreaBounds.yMax, 0f), new Vector3(timeAreaBounds.xMax, timeAreaBounds.yMax + position.height), 4f, Color.black);
					}
				}
			}

			// Token: 0x04000257 RID: 599
			private Tooltip m_BeginSelectionTooltip = new Tooltip(DirectorStyles.Instance.sequenceClip, DirectorStyles.Instance.tinyFont);

			// Token: 0x04000258 RID: 600
			private Tooltip m_EndSelectionTooltip = new Tooltip(DirectorStyles.Instance.sequenceClip, DirectorStyles.Instance.tinyFont);
		}
	}
}

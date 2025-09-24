using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000016 RID: 22
	internal class TrackheadContextMenu : Manipulator
	{
		// Token: 0x0600011E RID: 286 RVA: 0x0000A3C6 File Offset: 0x000087C6
		public override void Init(IControl parent)
		{
			parent.ContextClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				float tolerance = 0.25f / state.frameRate;
				GenericMenu genericMenu = new GenericMenu();
				genericMenu.AddItem(EditorGUIUtility.TextContent("Insert/Frame/Single"), false, delegate()
				{
					Gaps.Insert(state.timeline, state.time, (double)(1f / state.frameRate), tolerance);
					state.Refresh(true);
				});
				int[] array = new int[]
				{
					5,
					10,
					25,
					100
				};
				for (int num = 0; num != array.Length; num++)
				{
					float f = (float)array[num];
					genericMenu.AddItem(EditorGUIUtility.TextContent("Insert/Frame/" + array[num] + " Frames"), false, delegate()
					{
						Gaps.Insert(state.timeline, state.time, (double)(f / state.frameRate), tolerance);
						state.Refresh(true);
					});
				}
				if (state.playRangeTime.y > state.playRangeTime.x)
				{
					genericMenu.AddItem(EditorGUIUtility.TextContent("Insert/Selected Time"), false, delegate()
					{
						Gaps.Insert(state.timeline, (double)state.playRangeTime.x, (double)(state.playRangeTime.y - state.playRangeTime.x), tolerance);
						state.Refresh(true);
					});
				}
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Clips Ending Before"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => x.end < state.time + (double)tolerance, state);
				});
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Clips Starting Before"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => x.start < state.time + (double)tolerance, state);
				});
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Clips Ending After"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => x.end - state.time >= (double)(-(double)tolerance), state);
				});
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Clips Starting After"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => x.start - state.time >= (double)(-(double)tolerance), state);
				});
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Clips Intersecting"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => x.start <= state.time && state.time <= x.end, state);
				});
				genericMenu.AddItem(EditorGUIUtility.TextContent("Select/Blends Intersecting"), false, delegate()
				{
					this.SelectMenuCallback((TimelineClip x) => this.SelectBlendingIntersecting(x, state.time), state);
				});
				genericMenu.ShowAsContext();
				return base.ConsumeEvent();
			};
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000A3DC File Offset: 0x000087DC
		private bool SelectBlendingIntersecting(TimelineClip clip, double time)
		{
			return clip.start <= time && time <= clip.end && (time <= clip.start + clip.blendInDuration || time >= clip.end - clip.blendOutDuration);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000A438 File Offset: 0x00008838
		private void SelectMenuCallback(Func<TimelineClip, bool> selector, TimelineWindow.TimelineState state)
		{
			List<TimelineClipGUI> allClipGuis = state.GetWindow().treeView.allClipGuis;
			if (allClipGuis != null)
			{
				state.selection.Clear();
				for (int num = 0; num != allClipGuis.Count; num++)
				{
					TimelineClipGUI timelineClipGUI = allClipGuis[num];
					if (timelineClipGUI != null && timelineClipGUI.clip != null && selector(timelineClipGUI.clip))
					{
						state.selection.Add(timelineClipGUI);
					}
				}
			}
		}
	}
}

using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000066 RID: 102
	internal class Jog : Manipulator
	{
		// Token: 0x0600038C RID: 908 RVA: 0x0001D218 File Offset: 0x0001B618
		public override void Init(IControl parent)
		{
			Vector2 mouseDownOrigin = Vector2.zero;
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.keyCode != 106)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					state.isJogging = true;
					result = this.ConsumeEvent();
				}
				return result;
			};
			parent.KeyUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.keyCode != 106)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					state.playbackSpeed = 0f;
					state.playing = false;
					state.isJogging = false;
					result = this.ConsumeEvent();
				}
				return result;
			};
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!state.isJogging)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					mouseDownOrigin = evt.mousePosition;
					state.playbackSpeed = 0f;
					state.playing = true;
					evt.Use();
					result = this.IgnoreEvent();
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!state.isJogging)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					mouseDownOrigin = evt.mousePosition;
					state.playbackSpeed = 0f;
					state.playing = true;
					result = this.IgnoreEvent();
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!state.isJogging)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					state.playbackSpeed = (evt.mousePosition - mouseDownOrigin).x * 0.002f;
					state.playing = true;
					result = this.ConsumeEvent();
				}
				return result;
			};
		}
	}
}

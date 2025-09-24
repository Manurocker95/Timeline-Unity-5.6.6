using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000057 RID: 87
	internal class Control : IControl
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000A9C8 File Offset: 0x00008DC8
		public Control()
		{
			this.MouseMove += this.NoOp;
			this.MouseDown += this.NoOp;
			this.MouseDrag += this.NoOp;
			this.MouseUp += this.NoOp;
			this.DoubleClick += this.NoOp;
			this.KeyDown += this.NoOp;
			this.KeyUp += this.NoOp;
			this.DragPerform += this.NoOp;
			this.DragExited += this.NoOp;
			this.DragUpdated += this.NoOp;
			this.MouseWheel += this.NoOp;
			this.ContextClick += this.NoOp;
			this.ValidateCommand += this.NoOp;
			this.ExecuteCommand += this.NoOp;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000303 RID: 771 RVA: 0x0000AAF0 File Offset: 0x00008EF0
		// (remove) Token: 0x06000304 RID: 772 RVA: 0x0000AB28 File Offset: 0x00008F28
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDown;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000305 RID: 773 RVA: 0x0000AB60 File Offset: 0x00008F60
		// (remove) Token: 0x06000306 RID: 774 RVA: 0x0000AB98 File Offset: 0x00008F98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDrag;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000307 RID: 775 RVA: 0x0000ABD0 File Offset: 0x00008FD0
		// (remove) Token: 0x06000308 RID: 776 RVA: 0x0000AC08 File Offset: 0x00009008
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseWheel;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000309 RID: 777 RVA: 0x0000AC40 File Offset: 0x00009040
		// (remove) Token: 0x0600030A RID: 778 RVA: 0x0000AC78 File Offset: 0x00009078
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseMove;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600030B RID: 779 RVA: 0x0000ACB0 File Offset: 0x000090B0
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x0000ACE8 File Offset: 0x000090E8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseUp;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600030D RID: 781 RVA: 0x0000AD20 File Offset: 0x00009120
		// (remove) Token: 0x0600030E RID: 782 RVA: 0x0000AD58 File Offset: 0x00009158
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DoubleClick;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600030F RID: 783 RVA: 0x0000AD90 File Offset: 0x00009190
		// (remove) Token: 0x06000310 RID: 784 RVA: 0x0000ADC8 File Offset: 0x000091C8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyDown;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000311 RID: 785 RVA: 0x0000AE00 File Offset: 0x00009200
		// (remove) Token: 0x06000312 RID: 786 RVA: 0x0000AE38 File Offset: 0x00009238
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyUp;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000313 RID: 787 RVA: 0x0000AE70 File Offset: 0x00009270
		// (remove) Token: 0x06000314 RID: 788 RVA: 0x0000AEA8 File Offset: 0x000092A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragPerform;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000315 RID: 789 RVA: 0x0000AEE0 File Offset: 0x000092E0
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x0000AF18 File Offset: 0x00009318
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragExited;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000317 RID: 791 RVA: 0x0000AF50 File Offset: 0x00009350
		// (remove) Token: 0x06000318 RID: 792 RVA: 0x0000AF88 File Offset: 0x00009388
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragUpdated;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000319 RID: 793 RVA: 0x0000AFC0 File Offset: 0x000093C0
		// (remove) Token: 0x0600031A RID: 794 RVA: 0x0000AFF8 File Offset: 0x000093F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent Overlay;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x0600031B RID: 795 RVA: 0x0000B030 File Offset: 0x00009430
		// (remove) Token: 0x0600031C RID: 796 RVA: 0x0000B068 File Offset: 0x00009468
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ContextClick;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600031D RID: 797 RVA: 0x0000B0A0 File Offset: 0x000094A0
		// (remove) Token: 0x0600031E RID: 798 RVA: 0x0000B0D8 File Offset: 0x000094D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ValidateCommand;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600031F RID: 799 RVA: 0x0000B110 File Offset: 0x00009510
		// (remove) Token: 0x06000320 RID: 800 RVA: 0x0000B148 File Offset: 0x00009548
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ExecuteCommand;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000B180 File Offset: 0x00009580
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0000B19B File Offset: 0x0000959B
		public Control parentControl
		{
			get
			{
				return this.m_ParentControl;
			}
			set
			{
				this.m_ParentControl = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000B1A8 File Offset: 0x000095A8
		public virtual Rect bounds
		{
			get
			{
				return default(Rect);
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000B1C8 File Offset: 0x000095C8
		public virtual bool IsMouseOver(Vector2 mousePosition)
		{
			return this.bounds.Contains(mousePosition);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000B1EC File Offset: 0x000095EC
		public static void AddCursor(CursorInfo ci)
		{
			for (int num = 0; num != Control.m_Cursors.Count; num++)
			{
				if (Control.m_Cursors[num].ID == ci.ID)
				{
					return;
				}
			}
			Control.m_Cursors.Add(ci);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000B244 File Offset: 0x00009644
		public static void RemoveCursor(CursorInfo ci)
		{
			int num = -1;
			for (int num2 = 0; num2 != Control.m_Cursors.Count; num2++)
			{
				if (Control.m_Cursors[num2].ID == ci.ID)
				{
					num = num2;
					break;
				}
			}
			if (num > -1)
			{
				Control.m_Cursors.RemoveAt(num);
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000B2A8 File Offset: 0x000096A8
		public static void DrawCursors()
		{
			Event current = Event.current;
			Rect rect;
			rect..ctor(current.mousePosition.x - 100f, current.mousePosition.y - 100f, 200f, 200f);
			foreach (CursorInfo cursorInfo in Control.m_Cursors)
			{
				EditorGUIUtility.AddCursorRect(rect, cursorInfo.cursor);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000B350 File Offset: 0x00009750
		public List<Control> children
		{
			get
			{
				return this.m_Children;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000B36B File Offset: 0x0000976B
		public void AddChild(Control child)
		{
			this.m_Children.Add(child);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000B37C File Offset: 0x0000977C
		public virtual bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			bool flag = false;
			if (!isCaptureSession)
			{
				foreach (Control control in this.m_Children)
				{
					if (!flag && control.bounds.Contains(evt.mousePosition))
					{
						flag = control.OnEvent(evt, state, false);
					}
				}
			}
			bool result;
			if (flag)
			{
				result = true;
			}
			else if (evt == null)
			{
				result = false;
			}
			else
			{
				switch (evt.type)
				{
				case 0:
					if (evt.clickCount < 2)
					{
						flag |= this.MouseDown(this, evt, state);
					}
					else
					{
						flag |= this.DoubleClick(this, evt, state);
					}
					break;
				case 1:
					flag |= this.MouseUp(this, evt, state);
					break;
				case 3:
					flag |= this.MouseDrag(this, evt, state);
					break;
				case 4:
					flag |= this.KeyDown(this, evt, state);
					break;
				case 5:
					flag |= this.KeyUp(this, evt, state);
					break;
				case 6:
					flag |= this.MouseWheel(this, evt, state);
					break;
				case 9:
					flag |= this.DragUpdated(this, evt, state);
					break;
				case 10:
					flag |= this.DragPerform(this, evt, state);
					break;
				case 13:
					flag |= this.ValidateCommand(this, evt, state);
					break;
				case 14:
					flag |= this.ExecuteCommand(this, evt, state);
					break;
				case 15:
					flag |= this.DragExited(this, evt, state);
					break;
				case 16:
					flag |= this.ContextClick(this, evt, state);
					break;
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000B5B8 File Offset: 0x000099B8
		public virtual void DrawOverlays(Event evt, TimelineWindow.TimelineState state)
		{
			if (this.Overlay != null)
			{
				this.Overlay(this, evt, state);
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000B5DA File Offset: 0x000099DA
		public void ClearManipulators()
		{
			this.m_Manipulators.Clear();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000B5E8 File Offset: 0x000099E8
		public void AddManipulator(Manipulator m)
		{
			m.Init(this);
			this.m_Manipulators.Add(m);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000B600 File Offset: 0x00009A00
		private bool NoOp(object target, Event e, TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000B618 File Offset: 0x00009A18
		public static bool InvokeEvents(TimelineUIEvent eventList, object target, Event evt, TimelineWindow.TimelineState state)
		{
			bool flag = false;
			bool result;
			if (eventList == null)
			{
				result = false;
			}
			else
			{
				Delegate[] invocationList = eventList.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					if (@delegate != null)
					{
						flag = (bool)@delegate.DynamicInvoke(new object[]
						{
							target,
							evt,
							state
						});
						if (flag)
						{
							break;
						}
					}
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000B6A0 File Offset: 0x00009AA0
		public static bool IsMouseContainsInControls(IEnumerable<IControl> controls)
		{
			return controls.Any((IControl c) => c.IsMouseOver(Event.current.mousePosition));
		}

		// Token: 0x04000239 RID: 569
		private List<Manipulator> m_Manipulators = new List<Manipulator>();

		// Token: 0x0400023A RID: 570
		private List<Control> m_Children = new List<Control>();

		// Token: 0x0400023B RID: 571
		private Control m_ParentControl;

		// Token: 0x0400023C RID: 572
		private static List<CursorInfo> m_Cursors = new List<CursorInfo>();
	}
}

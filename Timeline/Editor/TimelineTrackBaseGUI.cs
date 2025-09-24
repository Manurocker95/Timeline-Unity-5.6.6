using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x0200008B RID: 139
	internal abstract class TimelineTrackBaseGUI : TreeViewItem, IControl, IBounds, ISelectable
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x00023650 File Offset: 0x00021A50
		protected TimelineTrackBaseGUI(int id, int depth, TreeViewItem parent, string displayName, TrackAsset trackAsset, TreeViewController tv, TimelineTreeViewGUI tvgui) : base(id, depth, parent, displayName)
		{
			this.m_Drawer = TrackDrawer.CreateInstance(trackAsset);
			this.m_Drawer.track = trackAsset;
			this.m_Drawer.sequencerState = tvgui.TimelineWindow.state;
			this.m_Drawer.ConfigureUITrack(this);
			this.isExpanded = false;
			this.isDropTarget = false;
			this.track = trackAsset;
			this.treeView = tv;
			this.m_Selected = false;
			this.m_TreeViewGUI = tvgui;
			if (TimelineTrackBaseGUI.<>f__mg$cache0 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache0 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseDown += TimelineTrackBaseGUI.<>f__mg$cache0;
			if (TimelineTrackBaseGUI.<>f__mg$cache1 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache1 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseDrag += TimelineTrackBaseGUI.<>f__mg$cache1;
			if (TimelineTrackBaseGUI.<>f__mg$cache2 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache2 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseUp += TimelineTrackBaseGUI.<>f__mg$cache2;
			if (TimelineTrackBaseGUI.<>f__mg$cache3 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache3 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DoubleClick += TimelineTrackBaseGUI.<>f__mg$cache3;
			if (TimelineTrackBaseGUI.<>f__mg$cache4 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache4 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.KeyDown += TimelineTrackBaseGUI.<>f__mg$cache4;
			if (TimelineTrackBaseGUI.<>f__mg$cache5 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache5 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.KeyUp += TimelineTrackBaseGUI.<>f__mg$cache5;
			if (TimelineTrackBaseGUI.<>f__mg$cache6 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache6 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragPerform += TimelineTrackBaseGUI.<>f__mg$cache6;
			if (TimelineTrackBaseGUI.<>f__mg$cache7 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache7 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragExited += TimelineTrackBaseGUI.<>f__mg$cache7;
			if (TimelineTrackBaseGUI.<>f__mg$cache8 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache8 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragUpdated += TimelineTrackBaseGUI.<>f__mg$cache8;
			if (TimelineTrackBaseGUI.<>f__mg$cache9 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache9 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseWheel += TimelineTrackBaseGUI.<>f__mg$cache9;
			if (TimelineTrackBaseGUI.<>f__mg$cacheA == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheA = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.ContextClick += TimelineTrackBaseGUI.<>f__mg$cacheA;
			if (TimelineTrackBaseGUI.<>f__mg$cacheB == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheB = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseMove += TimelineTrackBaseGUI.<>f__mg$cacheB;
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000491 RID: 1169 RVA: 0x00023894 File Offset: 0x00021C94
		// (remove) Token: 0x06000492 RID: 1170 RVA: 0x000238CC File Offset: 0x00021CCC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDown;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000493 RID: 1171 RVA: 0x00023904 File Offset: 0x00021D04
		// (remove) Token: 0x06000494 RID: 1172 RVA: 0x0002393C File Offset: 0x00021D3C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDrag;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000495 RID: 1173 RVA: 0x00023974 File Offset: 0x00021D74
		// (remove) Token: 0x06000496 RID: 1174 RVA: 0x000239AC File Offset: 0x00021DAC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseWheel;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000497 RID: 1175 RVA: 0x000239E4 File Offset: 0x00021DE4
		// (remove) Token: 0x06000498 RID: 1176 RVA: 0x00023A1C File Offset: 0x00021E1C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseMove;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000499 RID: 1177 RVA: 0x00023A54 File Offset: 0x00021E54
		// (remove) Token: 0x0600049A RID: 1178 RVA: 0x00023A8C File Offset: 0x00021E8C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseUp;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600049B RID: 1179 RVA: 0x00023AC4 File Offset: 0x00021EC4
		// (remove) Token: 0x0600049C RID: 1180 RVA: 0x00023AFC File Offset: 0x00021EFC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DoubleClick;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600049D RID: 1181 RVA: 0x00023B34 File Offset: 0x00021F34
		// (remove) Token: 0x0600049E RID: 1182 RVA: 0x00023B6C File Offset: 0x00021F6C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyDown;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x0600049F RID: 1183 RVA: 0x00023BA4 File Offset: 0x00021FA4
		// (remove) Token: 0x060004A0 RID: 1184 RVA: 0x00023BDC File Offset: 0x00021FDC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyUp;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060004A1 RID: 1185 RVA: 0x00023C14 File Offset: 0x00022014
		// (remove) Token: 0x060004A2 RID: 1186 RVA: 0x00023C4C File Offset: 0x0002204C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragPerform;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060004A3 RID: 1187 RVA: 0x00023C84 File Offset: 0x00022084
		// (remove) Token: 0x060004A4 RID: 1188 RVA: 0x00023CBC File Offset: 0x000220BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragExited;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060004A5 RID: 1189 RVA: 0x00023CF4 File Offset: 0x000220F4
		// (remove) Token: 0x060004A6 RID: 1190 RVA: 0x00023D2C File Offset: 0x0002212C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragUpdated;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060004A7 RID: 1191 RVA: 0x00023D64 File Offset: 0x00022164
		// (remove) Token: 0x060004A8 RID: 1192 RVA: 0x00023D9C File Offset: 0x0002219C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent Overlay;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060004A9 RID: 1193 RVA: 0x00023DD4 File Offset: 0x000221D4
		// (remove) Token: 0x060004AA RID: 1194 RVA: 0x00023E0C File Offset: 0x0002220C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ContextClick;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060004AB RID: 1195 RVA: 0x00023E44 File Offset: 0x00022244
		// (remove) Token: 0x060004AC RID: 1196 RVA: 0x00023E7C File Offset: 0x0002227C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ValidateCommand;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060004AD RID: 1197 RVA: 0x00023EB4 File Offset: 0x000222B4
		// (remove) Token: 0x060004AE RID: 1198 RVA: 0x00023EEC File Offset: 0x000222EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ExecuteCommand;

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00023F24 File Offset: 0x00022324
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00023F3E File Offset: 0x0002233E
		public bool isExpanded { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00023F48 File Offset: 0x00022348
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00023F62 File Offset: 0x00022362
		public bool isDropTarget { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00023F6C File Offset: 0x0002236C
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00023F86 File Offset: 0x00022386
		public TrackAsset track { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00023F90 File Offset: 0x00022390
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00023FAA File Offset: 0x000223AA
		public TreeViewController treeView { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00023FB4 File Offset: 0x000223B4
		public TimelineTreeViewGUI treeViewGUI
		{
			get
			{
				return this.m_TreeViewGUI;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00023FD0 File Offset: 0x000223D0
		public TimelineWindow TimelineWindow
		{
			get
			{
				TimelineWindow result;
				if (this.m_TreeViewGUI == null)
				{
					result = null;
				}
				else
				{
					result = this.m_TreeViewGUI.TimelineWindow;
				}
				return result;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00024004 File Offset: 0x00022404
		public bool isRoot
		{
			get
			{
				return this.m_IsRoot;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00024020 File Offset: 0x00022420
		public bool selectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00024038 File Offset: 0x00022438
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00024053 File Offset: 0x00022453
		public virtual bool selected
		{
			get
			{
				return this.m_Selected;
			}
			set
			{
				this.m_Selected = value;
				this.OnSelectedChanged(value);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00024064 File Offset: 0x00022464
		public object selectableObject
		{
			get
			{
				return this.track;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00024080 File Offset: 0x00022480
		public TrackDrawer drawer
		{
			get
			{
				return this.m_Drawer;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0002409C File Offset: 0x0002249C
		public TimelineTrackBaseGUI parentTrackGUI
		{
			get
			{
				TimelineTrackBaseGUI result;
				if (this.parent != null && this.parent is TimelineTrackBaseGUI)
				{
					result = (this.parent as TimelineTrackBaseGUI);
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x000240E0 File Offset: 0x000224E0
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00024100 File Offset: 0x00022500
		public bool locked
		{
			get
			{
				return this.track.locked;
			}
			set
			{
				if (this.track.locked != value)
				{
					this.OnLockedChanged(value);
					this.track.locked = value;
					TimelineHelpers.PushUndo(this.track, "track.lock");
					TimelineWindow.instance.state.Refresh(true);
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00024154 File Offset: 0x00022554
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00024174 File Offset: 0x00022574
		public bool muted
		{
			get
			{
				return this.track.muted;
			}
			set
			{
				this.track.muted = value;
			}
		}

		// Token: 0x060004C4 RID: 1220
		protected abstract void OnSelectedChanged(bool value);

		// Token: 0x060004C5 RID: 1221
		protected abstract void OnLockedChanged(bool value);

		// Token: 0x060004C6 RID: 1222
		protected abstract bool DetectProblems(TimelineWindow.TimelineState state);

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004C7 RID: 1223
		public abstract Rect boundingRect { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004C8 RID: 1224
		public abstract Rect headerBounds { get; }

		// Token: 0x060004C9 RID: 1225
		public abstract bool IsMouseOver(Vector2 mousePosition);

		// Token: 0x060004CA RID: 1226
		public abstract float GetHeight(TimelineWindow.TimelineState state);

		// Token: 0x060004CB RID: 1227
		public abstract void SetHeight(float height);

		// Token: 0x060004CC RID: 1228
		public abstract void Draw(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state, float identWidth);

		// Token: 0x060004CD RID: 1229
		public abstract bool CanBeSelected(Vector2 mousePosition);

		// Token: 0x060004CE RID: 1230
		public abstract void OnGraphRebuilt();

		// Token: 0x060004CF RID: 1231 RVA: 0x00024184 File Offset: 0x00022584
		public static TimelineTrackBaseGUI FindGUITrack(TrackAsset track)
		{
			List<TimelineTrackBaseGUI> allTracks = TimelineWindow.instance.allTracks;
			return allTracks.Find((TimelineTrackBaseGUI x) => x.track == track);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x000241C3 File Offset: 0x000225C3
		public virtual void Delete(ITimelineState state)
		{
			DeleteTracks.Do(state.timeline, this.track);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000241D7 File Offset: 0x000225D7
		public void DrawOverlays(Event evt, TimelineWindow.TimelineState state)
		{
			if (this.Overlay != null)
			{
				this.Overlay(this, evt, state);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000241FC File Offset: 0x000225FC
		protected static bool NoOp(object target, Event e, TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00024214 File Offset: 0x00022614
		private bool HandleChildrenControls(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			for (int num = 0; num != this.m_ChildrenControls.Count; num++)
			{
				if (this.m_ChildrenControls[num].OnEvent(evt, state, isCaptureSession))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00024268 File Offset: 0x00022668
		public virtual bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			bool flag = false;
			switch (evt.type)
			{
			case 0:
				if (evt.clickCount < 2)
				{
					flag = Control.InvokeEvents(this.MouseDown, this, evt, state);
				}
				else
				{
					flag = Control.InvokeEvents(this.DoubleClick, this, evt, state);
				}
				break;
			case 1:
				flag = Control.InvokeEvents(this.MouseUp, this, evt, state);
				break;
			case 3:
				flag = Control.InvokeEvents(this.MouseDrag, this, evt, state);
				break;
			case 4:
				flag = Control.InvokeEvents(this.KeyDown, this, evt, state);
				break;
			case 5:
				flag = Control.InvokeEvents(this.KeyUp, this, evt, state);
				break;
			case 6:
				flag = Control.InvokeEvents(this.MouseWheel, this, evt, state);
				break;
			case 9:
				flag = Control.InvokeEvents(this.DragUpdated, this, evt, state);
				break;
			case 10:
				flag = Control.InvokeEvents(this.DragPerform, this, evt, state);
				break;
			case 13:
				flag = Control.InvokeEvents(this.ValidateCommand, this, evt, state);
				break;
			case 14:
				flag = Control.InvokeEvents(this.ExecuteCommand, this, evt, state);
				break;
			case 15:
				flag = Control.InvokeEvents(this.DragExited, this, evt, state);
				break;
			case 16:
				flag = Control.InvokeEvents(this.ContextClick, this, evt, state);
				break;
			}
			if (!flag)
			{
				flag = this.HandleChildrenControls(evt, state, isCaptureSession);
			}
			return flag;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000243FC File Offset: 0x000227FC
		public void ClearManipulators()
		{
			this.MouseDown = null;
			this.MouseDrag = null;
			this.MouseWheel = null;
			this.MouseMove = null;
			this.MouseUp = null;
			this.DoubleClick = null;
			this.KeyDown = null;
			this.KeyUp = null;
			this.DragPerform = null;
			this.DragExited = null;
			this.DragUpdated = null;
			this.Overlay = null;
			this.ContextClick = null;
			if (TimelineTrackBaseGUI.<>f__mg$cacheC == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheC = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseDown += TimelineTrackBaseGUI.<>f__mg$cacheC;
			if (TimelineTrackBaseGUI.<>f__mg$cacheD == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheD = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseDrag += TimelineTrackBaseGUI.<>f__mg$cacheD;
			if (TimelineTrackBaseGUI.<>f__mg$cacheE == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheE = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseUp += TimelineTrackBaseGUI.<>f__mg$cacheE;
			if (TimelineTrackBaseGUI.<>f__mg$cacheF == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cacheF = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DoubleClick += TimelineTrackBaseGUI.<>f__mg$cacheF;
			if (TimelineTrackBaseGUI.<>f__mg$cache10 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache10 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.KeyDown += TimelineTrackBaseGUI.<>f__mg$cache10;
			if (TimelineTrackBaseGUI.<>f__mg$cache11 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache11 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.KeyUp += TimelineTrackBaseGUI.<>f__mg$cache11;
			if (TimelineTrackBaseGUI.<>f__mg$cache12 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache12 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragPerform += TimelineTrackBaseGUI.<>f__mg$cache12;
			if (TimelineTrackBaseGUI.<>f__mg$cache13 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache13 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragExited += TimelineTrackBaseGUI.<>f__mg$cache13;
			if (TimelineTrackBaseGUI.<>f__mg$cache14 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache14 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.DragUpdated += TimelineTrackBaseGUI.<>f__mg$cache14;
			if (TimelineTrackBaseGUI.<>f__mg$cache15 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache15 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseWheel += TimelineTrackBaseGUI.<>f__mg$cache15;
			if (TimelineTrackBaseGUI.<>f__mg$cache16 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache16 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.ContextClick += TimelineTrackBaseGUI.<>f__mg$cache16;
			if (TimelineTrackBaseGUI.<>f__mg$cache17 == null)
			{
				TimelineTrackBaseGUI.<>f__mg$cache17 = new TimelineUIEvent(TimelineTrackBaseGUI.NoOp);
			}
			this.MouseMove += TimelineTrackBaseGUI.<>f__mg$cache17;
			this.m_Manipulators.Clear();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00024614 File Offset: 0x00022A14
		public void AddManipulator(Manipulator m)
		{
			m.Init(this);
			this.m_Manipulators.Add(m);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0002462C File Offset: 0x00022A2C
		protected void GetChildrenHeight(ref float height, TreeViewItem item)
		{
			TimelineTrackBaseGUI timelineTrackBaseGUI = item as TimelineTrackBaseGUI;
			bool flag = timelineTrackBaseGUI != null && timelineTrackBaseGUI.track.collapsed;
			if (item.children != null && !flag)
			{
				bool flag2 = false;
				IList<TreeViewItem> rows = this.treeView.data.GetRows();
				for (int num = 0; num != item.children.Count; num++)
				{
					TreeViewItem treeViewItem = item.children[num];
					if (this.treeView.data.IsRevealed(treeViewItem.id))
					{
						int num2 = rows.IndexOf(treeViewItem);
						if (num2 >= 0)
						{
							flag2 = true;
							height += this.m_TreeViewGUI.GetRowRect(num2).height;
							TimelineGroupGUI timelineGroupGUI = treeViewItem as TimelineGroupGUI;
							if (timelineGroupGUI != null)
							{
								if (timelineGroupGUI.track != null)
								{
									TrackAsset trackAsset = timelineGroupGUI.track.parent as TrackAsset;
									if (trackAsset != null)
									{
										height += 3f;
									}
								}
							}
						}
					}
					this.GetChildrenHeight(ref height, treeViewItem);
					if (flag2)
					{
						height += 2f;
					}
				}
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00024765 File Offset: 0x00022B65
		public void DisplayTrackMenu(TimelineWindow.TimelineState state)
		{
			SequencerContextMenu.Show(this.drawer, this.track, Event.current.mousePosition);
		}

		// Token: 0x040002FD RID: 765
		protected List<Control> m_ChildrenControls = new List<Control>();

		// Token: 0x040002FE RID: 766
		protected bool m_IsRoot = false;

		// Token: 0x040002FF RID: 767
		private readonly List<Manipulator> m_Manipulators = new List<Manipulator>();

		// Token: 0x04000300 RID: 768
		private readonly TimelineTreeViewGUI m_TreeViewGUI;

		// Token: 0x04000301 RID: 769
		private readonly TrackDrawer m_Drawer;

		// Token: 0x04000302 RID: 770
		private bool m_Selected;

		// Token: 0x04000307 RID: 775
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache0;

		// Token: 0x04000308 RID: 776
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache1;

		// Token: 0x04000309 RID: 777
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache2;

		// Token: 0x0400030A RID: 778
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache3;

		// Token: 0x0400030B RID: 779
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache4;

		// Token: 0x0400030C RID: 780
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache5;

		// Token: 0x0400030D RID: 781
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache6;

		// Token: 0x0400030E RID: 782
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache7;

		// Token: 0x0400030F RID: 783
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache8;

		// Token: 0x04000310 RID: 784
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache9;

		// Token: 0x04000311 RID: 785
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheA;

		// Token: 0x04000312 RID: 786
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheB;

		// Token: 0x04000313 RID: 787
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheC;

		// Token: 0x04000314 RID: 788
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheD;

		// Token: 0x04000315 RID: 789
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheE;

		// Token: 0x04000316 RID: 790
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheF;

		// Token: 0x04000317 RID: 791
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache10;

		// Token: 0x04000318 RID: 792
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache11;

		// Token: 0x04000319 RID: 793
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache12;

		// Token: 0x0400031A RID: 794
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache13;

		// Token: 0x0400031B RID: 795
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache14;

		// Token: 0x0400031C RID: 796
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache15;

		// Token: 0x0400031D RID: 797
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache16;

		// Token: 0x0400031E RID: 798
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache17;
	}
}

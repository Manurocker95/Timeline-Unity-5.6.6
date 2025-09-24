using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000010 RID: 16
	internal class Selection : IEnumerable<ISelectable>, IEnumerable
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x0000876C File Offset: 0x00006B6C
		public Selection()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00008799 File Offset: 0x00006B99
		public Selection(TimelineWindow.TimelineState state)
		{
			this.m_State = state;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000087D0 File Offset: 0x00006BD0
		public bool isEmpty
		{
			get
			{
				return this.m_Selection.Count == 0;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000087F4 File Offset: 0x00006BF4
		public bool isMultiSelect
		{
			get
			{
				return this.m_Selection.Count > 1;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00008818 File Offset: 0x00006C18
		public List<T> FilterByType<T>()
		{
			return this.m_Selection.OfType<T>().ToList<T>();
		}

		// Token: 0x17000021 RID: 33
		public ISelectable this[int index]
		{
			get
			{
				return this.m_Selection[index];
			}
			set
			{
				this.m_Selection[index] = value;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00008874 File Offset: 0x00006C74
		public void Add(ISelectable item)
		{
			if (item != null && !this.m_Selection.Contains(item) && item.selectable)
			{
				this.m_Selection.Add(item);
				item.selected = true;
				if (Selection.showDebugLog)
				{
					Debug.LogFormat("Selection.Add ({0})", new object[]
					{
						item.GetType().Name
					});
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000088E4 File Offset: 0x00006CE4
		public void SelectInEditor(Object obj)
		{
			if (!this.m_IgnoreEditorSet)
			{
				if (this.m_Selection.Count<ISelectable>() == 1)
				{
					Selection.activeObject = obj;
				}
				else if (!Selection.Contains(obj))
				{
					List<Object> list = Selection.objects.ToList<Object>();
					list.Add(obj);
					Selection.objects = list.ToArray();
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000894C File Offset: 0x00006D4C
		public void Clear()
		{
			if (this.m_Selection.Count != 0)
			{
				IEnumerable<ISelectable> enumerable = this.m_Selection.ToArray();
				foreach (ISelectable selectable in enumerable)
				{
					if (selectable != null && selectable.selected)
					{
						selectable.selected = false;
					}
				}
				if (this.m_State != null && this.m_State.GetWindow() != null && this.m_State.GetWindow().treeView != null)
				{
					this.m_State.GetWindow().treeView.SetSelection(new int[0], false);
				}
				this.m_Selection.Clear();
				if (Selection.showDebugLog)
				{
					Debug.Log("Selection.Clear");
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00008A48 File Offset: 0x00006E48
		public bool Contains(ISelectable item)
		{
			return this.m_Selection.Contains(item);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00008A6C File Offset: 0x00006E6C
		public int Count
		{
			get
			{
				return this.m_Selection.Count;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00008A8C File Offset: 0x00006E8C
		public bool Remove(ISelectable item)
		{
			bool result;
			if (item != null)
			{
				if (Selection.showDebugLog)
				{
					Debug.LogFormat("Selection.Remove ({0})", new object[]
					{
						item.GetType().Name
					});
				}
				item.selected = false;
				result = this.m_Selection.Remove(item);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00008AEC File Offset: 0x00006EEC
		public IEnumerator<ISelectable> GetEnumerator()
		{
			return this.m_Selection.GetEnumerator();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008B14 File Offset: 0x00006F14
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008B30 File Offset: 0x00006F30
		public bool IsMouseHovering()
		{
			return this.FilterByType<IBounds>().Any((IBounds b) => b.boundingRect.Contains(Event.current.mousePosition));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008B70 File Offset: 0x00006F70
		public void SaveSelection()
		{
			this.m_SavedActiveObject = null;
			if (Selection.activeObject is EditorClip || Selection.activeObject is TrackAsset)
			{
				this.m_SavedActiveObject = Selection.activeObject;
			}
			this.m_SavedSelection.Clear();
			for (int i = 0; i < this.m_Selection.Count; i++)
			{
				object selectableObject = this.m_Selection[i].selectableObject;
				this.m_SavedSelection.Add(selectableObject);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008BF8 File Offset: 0x00006FF8
		public void RestoreSelection()
		{
			if (this.m_State != null && !(this.m_State.GetWindow() == null) && this.m_State.GetWindow().treeView != null)
			{
				this.m_IgnoreEditorSet = true;
				if (this.m_SavedActiveObject != null)
				{
					Selection.activeObject = this.m_SavedActiveObject;
				}
				List<TimelineTrackBaseGUI> allTrackGuis = this.m_State.GetWindow().treeView.allTrackGuis;
				List<TimelineClipGUI> allClipGuis = this.m_State.GetWindow().treeView.allClipGuis;
				for (int i = 0; i < this.m_SavedSelection.Count; i++)
				{
					TrackAsset trackAsset = this.m_SavedSelection[i] as TrackAsset;
					if (trackAsset != null)
					{
						TimelineTrackBaseGUI timelineTrackBaseGUI = allTrackGuis.SingleOrDefault((TimelineTrackBaseGUI t) => t.track == trackAsset);
						if (timelineTrackBaseGUI != null)
						{
							this.Add(timelineTrackBaseGUI);
						}
					}
				}
				foreach (TimelineClipGUI timelineClipGUI in allClipGuis)
				{
					if (timelineClipGUI.clip.selected || this.m_SavedSelection.Contains(timelineClipGUI.clip))
					{
						this.Add(timelineClipGUI);
					}
				}
				this.m_IgnoreEditorSet = false;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008D84 File Offset: 0x00007184
		public static List<IClipCurveEditorOwner> GetClipCurveEditorOwners()
		{
			IEnumerable<TimelineTrackGUI> enumerable = (from t in TimelineWindow.instance.allTracks
			where t is TimelineTrackGUI
			select t).Cast<TimelineTrackGUI>();
			List<IClipCurveEditorOwner> list = new List<IClipCurveEditorOwner>();
			foreach (TimelineTrackGUI timelineTrackGUI in enumerable)
			{
				list.Add(timelineTrackGUI);
				list.AddRange(timelineTrackGUI.clips.Cast<IClipCurveEditorOwner>());
			}
			return list;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00008E30 File Offset: 0x00007230
		public IClipCurveEditorOwner currentInlineCurveEditorOwner
		{
			get
			{
				return Selection.GetClipCurveEditorOwners().FirstOrDefault((IClipCurveEditorOwner o) => o.inlineCurvesSelected);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008E6C File Offset: 0x0000726C
		public static void UnselectInlineCurves()
		{
			Selection.GetClipCurveEditorOwners().ForEach(delegate(IClipCurveEditorOwner o)
			{
				o.inlineCurvesSelected = false;
			});
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008E98 File Offset: 0x00007298
		public static void UnselectAll()
		{
			Selection.UnselectInlineCurves();
			List<TimelineTrackBaseGUI> allTracks = TimelineWindow.instance.allTracks;
			foreach (TimelineTrackBaseGUI timelineTrackBaseGUI in allTracks)
			{
				timelineTrackBaseGUI.selected = false;
			}
			IEnumerable<TimelineClip> enumerable = allTracks.SelectMany((TimelineTrackBaseGUI t) => t.track.clips);
			foreach (TimelineClip timelineClip in enumerable)
			{
				timelineClip.selected = false;
			}
			IEnumerable<EditorClip> enumerable2 = Selection.objects.OfType<EditorClip>();
			foreach (EditorClip editorClip in enumerable2)
			{
				Selection.Remove(editorClip);
			}
		}

		// Token: 0x04000112 RID: 274
		public static bool showDebugLog = false;

		// Token: 0x04000113 RID: 275
		private bool m_IgnoreEditorSet = false;

		// Token: 0x04000114 RID: 276
		private readonly TimelineWindow.TimelineState m_State;

		// Token: 0x04000115 RID: 277
		private readonly List<ISelectable> m_Selection = new List<ISelectable>();

		// Token: 0x04000116 RID: 278
		private readonly List<object> m_SavedSelection = new List<object>();

		// Token: 0x04000117 RID: 279
		private Object m_SavedActiveObject = null;
	}
}

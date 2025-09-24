using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000CD RID: 205
	internal class TimelineWindow : EditorWindow, IControl
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x0002DC1C File Offset: 0x0002C01C
		public TimelineWindow()
		{
			this.InitializeIControl();
			this.InitializeManipulators();
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002DCD0 File Offset: 0x0002C0D0
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0002DCE9 File Offset: 0x0002C0E9
		public static TimelineWindow instance { get; private set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0002DCF4 File Offset: 0x0002C0F4
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0002DD0E File Offset: 0x0002C10E
		public Rect clientArea { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0002DD18 File Offset: 0x0002C118
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0002DD32 File Offset: 0x0002C132
		public bool isDragging { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0002DD3C File Offset: 0x0002C13C
		public static DirectorStyles styles
		{
			get
			{
				return DirectorStyles.Instance;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002DD58 File Offset: 0x0002C158
		public List<TimelineTrackBaseGUI> allTracks
		{
			get
			{
				return (this.treeView == null) ? new List<TimelineTrackBaseGUI>() : this.treeView.allTrackGuis;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0002DD90 File Offset: 0x0002C190
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x0002DDAA File Offset: 0x0002C1AA
		public TimelineWindow.TimelineState state { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0002DDB4 File Offset: 0x0002C1B4
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0002DDE7 File Offset: 0x0002C1E7
		public bool locked
		{
			get
			{
				return !(this.rootTimeline == null) && this.m_Locked;
			}
			set
			{
				this.m_Locked = value;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002DDF4 File Offset: 0x0002C1F4
		private void OnEnable()
		{
			string text = "Timeline Editor";
			if (this.state != null && this.state.timeline != null)
			{
				text = text + " - " + this.state.timeline.name;
			}
			TimelineWindowStyles.sequenceAsset.text = text;
			base.titleContent = TimelineWindowStyles.sequenceAsset;
			this.m_PreviewResizer.Init("TimelineWindow");
			TimelineWindow.instance = this;
			AnimationClipCurveCache.Instance.OnEnable();
			if (this.currentMode == null)
			{
				this.EnableInactiveMode();
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002DE8E File Offset: 0x0002C28E
		private void OnDisable()
		{
			TimelineWindow.instance = null;
			if (this.state != null)
			{
				this.state.Reset();
			}
			AnimationClipCurveCache.Instance.OnDisable();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002DEB7 File Offset: 0x0002C2B7
		private void OnDestroy()
		{
			if (this.state != null)
			{
				this.state.OnDestroy();
			}
			this.m_HasBeenInitialized = false;
			this.RemoveEditorCallbacks();
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002DEDF File Offset: 0x0002C2DF
		private void OnLostFocus()
		{
			this.isDragging = false;
			if (this.state != null)
			{
				this.state.captured.Clear();
			}
			base.Repaint();
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002DF0C File Offset: 0x0002C30C
		private void OnGUI()
		{
			if (!TimelineWindowStyles.initialized)
			{
				TimelineWindowStyles.Initialize();
			}
			this.DetectActiveSceneChanges();
			this.DetectStateChanges();
			if (Event.current.commandName == "ObjectSelectorClosed" && this.state.ObjectPickerSelectionCallback != null)
			{
				this.state.ObjectPickerSelectionCallback(this.state, EditorGUIUtility.GetObjectPickerObject());
			}
			bool flag = Event.current.type == 7;
			if (Event.current.type == 3 && this.state != null && this.state.mouseDragLag > 0f)
			{
				this.state.mouseDragLag -= Time.deltaTime;
			}
			else if (!this.PerformUndo())
			{
				if (EditorApplication.isPlaying)
				{
					if (this.state != null)
					{
						if (this.state.recording)
						{
							this.state.recording = false;
						}
					}
					base.Repaint();
				}
				this.clientArea = base.position;
				this.DoLayout();
				if (this.state.captured.Count > 0)
				{
					foreach (IControl control in this.state.captured)
					{
						control.DrawOverlays(Event.current, this.state);
					}
					base.Repaint();
				}
				if (this.state.showQuadTree)
				{
					this.state.quadTree.DebugDraw();
				}
				if (flag)
				{
					foreach (Action action in this.m_NextRepaint)
					{
						action();
					}
					this.m_NextRepaint.Clear();
				}
				if (Event.current.type == 7)
				{
					this.RebuildGraphIfNecessary(true);
					if (this.state != null)
					{
						this.state.ProcessPendingUpdates();
					}
					Control.DrawCursors();
					this.RestoreSelectionIfNecessary();
				}
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002E16C File Offset: 0x0002C56C
		public void RepaintNow()
		{
			base.RepaintImmediately();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002E175 File Offset: 0x0002C575
		public void RemoveItFromDockArea()
		{
			base.RemoveFromDockArea();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002E17E File Offset: 0x0002C57E
		public void NextRepaint(Action a)
		{
			if (this.m_NextRepaint == null)
			{
				this.m_NextRepaint = new List<Action>();
			}
			this.m_NextRepaint.Add(a);
			base.Repaint();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002E1AC File Offset: 0x0002C5AC
		private void DetectActiveSceneChanges()
		{
			if (this.m_CurrentSceneHashCode == -1)
			{
				this.m_CurrentSceneHashCode = SceneManager.GetActiveScene().GetHashCode();
			}
			if (this.m_CurrentSceneHashCode != SceneManager.GetActiveScene().GetHashCode())
			{
				bool flag = false;
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					if (sceneAt.GetHashCode() == this.m_CurrentSceneHashCode && sceneAt.isLoaded)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.SetCurrentTimeline(null, null);
					this.m_CurrentSceneHashCode = SceneManager.GetActiveScene().GetHashCode();
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002E27C File Offset: 0x0002C67C
		private void DetectStateChanges()
		{
			if (this.m_LastFrameHadSequence && this.timeline == null)
			{
				this.SetCurrentTimeline(null, null);
			}
			this.m_LastFrameHadSequence = (this.timeline != null);
			if (this.state != null && this.state.currentDirector == null)
			{
				this.state.recording = false;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002E2F4 File Offset: 0x0002C6F4
		private void Initialize()
		{
			bool flag = false;
			if (!this.m_HasBeenInitialized)
			{
				this.InitializeStateChange();
				this.InitializeEditorCallbacks();
				this.m_HasBeenInitialized = true;
			}
			if (this.state == null)
			{
				this.state = new TimelineWindow.TimelineState(this);
				this.OnSelectionChange();
				this.ResetBreadCrumbs();
			}
			this.InitializeViews();
			this.InitializeTimeArea();
			if (this.treeView == null && this.timeline != null)
			{
				flag = true;
				this.treeView = new TimelineTreeViewGUI(this, this.timeline, default(Rect));
				this.state.Refresh(false);
			}
			if (flag)
			{
				TimelineWindow.StateEventArgs e = new TimelineWindow.StateEventArgs
				{
					state = this.state,
					propertyChanged = ""
				};
				if (this.OnStateChange != null)
				{
					this.OnStateChange(this, e);
				}
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002E3DC File Offset: 0x0002C7DC
		private bool PerformUndo()
		{
			return Event.current.isKey && Event.current.keyCode == 122 && EditorGUI.actionKey;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002E434 File Offset: 0x0002C834
		public void RebuildGraphIfNecessary(bool evaluate = true)
		{
			if (this.state != null && !(this.state.currentDirector == null) && !(this.m_Timeline == null))
			{
				if (!EditorApplication.isPlaying)
				{
					if (this.state.rebuildGraph)
					{
						double time = this.state.time;
						this.state.GatherProperties(this.state.currentDirector);
						this.state.Stop();
						this.state.Play();
						this.state.time = time;
						if (evaluate)
						{
							this.state.EvaluateImmediate();
						}
						base.Repaint();
					}
					this.state.rebuildGraph = false;
				}
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002E504 File Offset: 0x0002C904
		public TrackAsset AddTrack(Type type)
		{
			return this.AddTrack(type, null);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002E524 File Offset: 0x0002C924
		public TrackAsset AddTrack(Type type, TrackAsset parent)
		{
			return this.AddTrack(TimelineHelpers.TrackTypeFromType(type), parent);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002E548 File Offset: 0x0002C948
		public TrackAsset AddTrack(TrackType trackType)
		{
			return this.AddTrack(trackType, null, "");
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002E56C File Offset: 0x0002C96C
		public TrackAsset AddTrack(TrackType trackType, TrackAsset parent)
		{
			return this.AddTrack(trackType, parent, "");
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002E590 File Offset: 0x0002C990
		public TrackAsset AddTrack(TrackType trackType, TrackAsset parent, string name)
		{
			PlayableAsset playableAsset = (!(parent != null)) ? this.m_Timeline : parent;
			TimelineHelpers.PushUndo(playableAsset, "create.track");
			string name2 = TimelineHelpers.GenerateUniqueActorName(this.timeline, (name.Length <= 0) ? ObjectNames.NicifyVariableName(trackType.m_TrackType.Name) : name);
			TrackAsset trackAsset = this.timeline.CreateTrack(playableAsset, name2, trackType);
			if (trackAsset != null)
			{
				trackAsset.name = name2;
				TimelineHelpers.AddRequiredComponent(this.state.GetSceneReference(trackAsset), trackAsset);
				TimelineHelpers.SaveAssetIntoObject(trackAsset, playableAsset);
				this.state.Refresh();
			}
			return trackAsset;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002E644 File Offset: 0x0002CA44
		private static TimelineAsset CreateSequence(string sequenceTitle)
		{
			if (string.IsNullOrEmpty(sequenceTitle))
			{
				sequenceTitle = "timeline";
			}
			return TimelineUtility.CreateAsset<TimelineAsset>(sequenceTitle + ".playable");
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002E67D File Offset: 0x0002CA7D
		[MenuItem("Assets/Create/Timeline/Timeline", false, 450)]
		public static void CreateNewSequence()
		{
			TimelineWindow.CreateSequence("New Timeline");
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002E68B File Offset: 0x0002CA8B
		[MenuItem("Window/Timeline Editor", false, 2016)]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(TimelineWindow));
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002E6A0 File Offset: 0x0002CAA0
		[OnOpenAsset(1)]
		public static bool OnDoubleClick(int instanceID, int line)
		{
			TimelineAsset timelineAsset = EditorUtility.InstanceIDToObject(instanceID) as TimelineAsset;
			bool result;
			if (timelineAsset == null)
			{
				result = false;
			}
			else
			{
				TimelineWindow.ShowWindow();
				TimelineWindow.instance.SetCurrentTimeline(timelineAsset, null);
				result = true;
			}
			return result;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002E6E8 File Offset: 0x0002CAE8
		protected virtual void ShowButton(Rect r)
		{
			bool flag = this.state == null || this.state.timeline == null;
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(flag);
			try
			{
				EditorGUI.BeginChangeCheck();
				bool locked = GUI.Toggle(r, this.locked, GUIContent.none, DirectorStyles.Instance.lockButton);
				if (EditorGUI.EndChangeCheck())
				{
					this.locked = locked;
					if (!this.locked)
					{
						this.OnSelectionChange();
					}
				}
			}
			finally
			{
				disabledScope.Dispose();
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0002E78C File Offset: 0x0002CB8C
		public TimelineAsset timeline
		{
			get
			{
				return (this.context != null) ? (this.context.currentAsset as TimelineAsset) : null;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002E7C4 File Offset: 0x0002CBC4
		public TimelineAsset rootTimeline
		{
			get
			{
				return this.m_Timeline;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002E7DF File Offset: 0x0002CBDF
		private void InitWithSequence(TimelineAsset seq)
		{
			this.m_LastFrameHadSequence = (seq != null);
			this.state.Reset();
			this.treeView = null;
			this.m_Timeline = seq;
			this.ResetBreadCrumbs();
			this.EnableEditAssetMode();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002E814 File Offset: 0x0002CC14
		public void SetCurrentTimeline(TimelineAsset seq, PlayableDirector instanceOfDirector)
		{
			if (this.state != null)
			{
				bool flag = seq == null || instanceOfDirector == null || this.m_Timeline != seq || this.state.currentDirector != instanceOfDirector;
				if (flag)
				{
					this.InitWithSequence(seq);
				}
				if (seq != null && instanceOfDirector != null)
				{
					this.BindToDirector(instanceOfDirector);
					this.EnableActiveMode();
				}
				this.Upgrade(seq);
				base.Repaint();
				if (instanceOfDirector != null)
				{
					this.m_LastSelectedObjectID = instanceOfDirector.gameObject.GetInstanceID();
				}
				else if (seq != null)
				{
					this.m_LastSelectedObjectID = seq.GetInstanceID();
				}
				else
				{
					this.m_LastSelectedObjectID = 0;
				}
				this.m_LastFrameHadSequence = (seq != null);
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002E904 File Offset: 0x0002CD04
		private void BindToDirector(PlayableDirector obj)
		{
			if (this.state != null && obj != null)
			{
				if (this.state.currentDirector != obj)
				{
					this.state.Stop();
					this.state.currentDirector = obj;
				}
				this.state.rebuildGraph = true;
				base.Repaint();
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0002E96C File Offset: 0x0002CD6C
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0002E986 File Offset: 0x0002CD86
		public TimelineWindow.SequenceContext context { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0002E990 File Offset: 0x0002CD90
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x0002E9AC File Offset: 0x0002CDAC
		private List<BreadcrumbElement> breadcrumbPath
		{
			get
			{
				return this.m_BreadcrumbPath;
			}
			set
			{
				this.m_BreadcrumbPath = new List<BreadcrumbElement>(value);
				if (this.context == null)
				{
					this.context = new TimelineWindow.SequenceContext();
				}
				PlayableAsset currentAsset = this.context.currentAsset;
				this.context.rootTimeline = this.m_Timeline;
				this.context.globalDuration = ((!(this.m_Timeline != null)) ? ((double)TimelineWindowStyles.kSequenceDefaultDuration) : this.m_Timeline.duration);
				this.context.globalStart = 0.0;
				this.context.currentAsset = this.m_Timeline;
				this.context.localDuration = this.context.globalDuration;
				this.context.localStart = this.context.globalStart;
				if (this.m_BreadcrumbPath.Count > 0)
				{
					BreadcrumbElement breadcrumbElement = this.m_BreadcrumbPath.Last<BreadcrumbElement>();
					this.context.currentAsset = breadcrumbElement.asset;
					this.context.localDuration = this.context.currentAsset.duration;
					double num = 0.0;
					for (int i = 0; i < this.m_BreadcrumbPath.Count; i++)
					{
						if (this.m_BreadcrumbPath[i].clip != null)
						{
							num += this.m_BreadcrumbPath[i].clip.start;
						}
					}
					this.context.localStart = num;
				}
				if (currentAsset != this.context.currentAsset && this.treeView != null)
				{
					this.treeView.Reload();
					this.state.selection.Clear();
					this.state.SetTimeAreaShownRange((float)this.context.localStart, (float)(this.context.localStart + this.context.localDuration));
				}
				if (this.m_BreadcrumbPath.Count<BreadcrumbElement>() == 2)
				{
					this.context.currentBinding = this.state.GetBindingForTrack(this.m_BreadcrumbPath.Last<BreadcrumbElement>().clip.parentTrack);
					TrackAsset group = TimelineHelpers.GetGroup(this.m_BreadcrumbPath.Last<BreadcrumbElement>().clip.parentTrack);
					this.context.currentGameObject = TimelineUtility.GetSceneGameObject(this.state.currentDirector, group);
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0002EC20 File Offset: 0x0002D020
		private float breadCrumbWidth
		{
			get
			{
				return this.timeAreaBounds.width - TimelineWindow.kBreadCrumbDelta;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002EC49 File Offset: 0x0002D049
		private void ResetBreadCrumbs()
		{
			this.state.BreadcrumbSetRoot(this.m_Timeline);
			base.Repaint();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002EC63 File Offset: 0x0002D063
		public void GoToBreadCrumbTarget(PlayableAsset target)
		{
			this.state.Refresh();
			base.Repaint();
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002EC78 File Offset: 0x0002D078
		private void DoBreadcrumbGUI()
		{
			if (this.currentMode.headerState.breadCrumb == TimelineModeGUIState.Hidden)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[]
				{
					GUILayout.Width(this.breadCrumbWidth)
				});
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			else
			{
				if (TimelineWindow.s_Styles == null)
				{
					TimelineWindow.s_Styles = new TimelineWindow.BreadcrumbStyles();
				}
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(this.currentMode.headerState.breadCrumb == TimelineModeGUIState.Disabled);
				try
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[]
					{
						GUILayout.Width(this.breadCrumbWidth)
					});
					int num = 0;
					for (int num2 = 0; num2 != this.breadcrumbPath.Count; num2++)
					{
						if (!(this.breadcrumbPath[num2].asset == null))
						{
							BreadcrumbElement breadcrumbElement = this.breadcrumbPath[num2];
							string text = (breadcrumbElement.clip != null) ? breadcrumbElement.clip.displayName : breadcrumbElement.asset.name;
							EditorGUI.BeginChangeCheck();
							GUILayout.Toggle(num2 == this.breadcrumbPath.Count - 1, text, (num2 != 0) ? TimelineWindow.s_Styles.breadCrumbMid : TimelineWindow.s_Styles.breadCrumbLeft, new GUILayoutOption[0]);
							if (EditorGUI.EndChangeCheck())
							{
								this.state.BreadcrumbGoto(breadcrumbElement.asset, breadcrumbElement.clip);
								this.state.Refresh();
							}
							num++;
						}
					}
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
				finally
				{
					disabledScope.Dispose();
				}
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002EE4C File Offset: 0x0002D24C
		private void DoSequenceSelectorGUI()
		{
			if (this.currentMode.headerState.sequenceSelector != TimelineModeGUIState.Hidden)
			{
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(this.currentMode.headerState.sequenceSelector == TimelineModeGUIState.Disabled);
				try
				{
					if (GUILayout.Button(DirectorStyles.Instance.sequenceSelectorIcon, EditorStyles.toolbarButton, new GUILayoutOption[0]))
					{
						PlayableDirector[] directorsInSceneUsingAsset = TimelineUtility.GetDirectorsInSceneUsingAsset(null);
						GenericMenu genericMenu = new GenericMenu();
						if (this.state.timeline != null)
						{
							genericMenu.AddItem(TimelineWindowStyles.createNewSequenceText, false, delegate(object userData)
							{
								this.SetCurrentTimeline(null, null);
							}, null);
							if (directorsInSceneUsingAsset.Length > 0)
							{
								genericMenu.AddSeparator(string.Empty);
							}
						}
						TimelineWindow.SequnenceMenuNameFormater sequnenceMenuNameFormater = new TimelineWindow.SequnenceMenuNameFormater();
						foreach (PlayableDirector playableDirector in directorsInSceneUsingAsset)
						{
							if (playableDirector.playableAsset is TimelineAsset)
							{
								string text = sequnenceMenuNameFormater.Format(playableDirector.playableAsset.name + " (" + playableDirector.name + ")");
								bool flag = this.state.currentDirector == playableDirector;
								genericMenu.AddItem(new GUIContent(text), flag, delegate(object arg)
								{
									PlayableDirector playableDirector2 = (PlayableDirector)arg;
									if (playableDirector2)
									{
										this.SetCurrentTimeline(playableDirector2.playableAsset as TimelineAsset, playableDirector2);
									}
								}, playableDirector);
							}
						}
						if (directorsInSceneUsingAsset.Length == 0)
						{
							genericMenu.AddDisabledItem(TimelineWindowStyles.noSequencesInScene);
						}
						genericMenu.ShowAsContext();
					}
				}
				finally
				{
					disabledScope.Dispose();
				}
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002EFF4 File Offset: 0x0002D3F4
		private bool CanShowDurationControl()
		{
			return !(this.timeline == null) && this.state.TimeIsInRange((float)this.state.duration);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002F048 File Offset: 0x0002D448
		private void DurationGUI(TimelineWindow.TimelineItemArea area)
		{
			if (!(this.state.timeline == null))
			{
				if (this.state.timeline.m_DurationMode != null || this.state.duration != 0.0)
				{
					if (this.m_SequenceDuration == null)
					{
						this.m_SequenceDuration = new TimelineItem(TimelineWindow.styles.endmarker, new Action<TimelineWindow.TimelineState, double, bool>(this.OnTrackDurationDrag))
						{
							tooltip = "End of sequence marker",
							boundOffset = new Vector2(0f, -TimelineWindowStyles.kDurationGuiThickness)
						};
					}
					bool flag = area == TimelineWindow.TimelineItemArea.Header;
					this.DrawDuration(flag, !flag);
				}
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002F108 File Offset: 0x0002D508
		private void DrawDuration(bool drawhead, bool drawline)
		{
			double duration = this.state.duration;
			if (this.CanShowDurationControl())
			{
				Color colorEndmarker = DirectorStyles.Instance.customSkin.colorEndmarker;
				Color headColor = Color.white;
				bool flag = !EditorApplication.isPlaying && this.state.timeline.durationMode == 1;
				if (flag)
				{
					if (this.m_SequenceDuration.bounds.Contains(Event.current.mousePosition))
					{
						if (this.m_PlayHead != null && this.m_PlayHead.bounds.Contains(Event.current.mousePosition))
						{
							flag = false;
						}
						else if (this.m_SequenceDuration.OnEvent(Event.current, this.state, false))
						{
							Event.current.Use();
						}
					}
				}
				else
				{
					colorEndmarker.a *= 0.66f;
					headColor = DirectorStyles.Instance.customSkin.colorDuration;
				}
				this.m_SequenceDuration.lineColor = colorEndmarker;
				this.m_SequenceDuration.headColor = headColor;
				this.m_SequenceDuration.drawHead = drawhead;
				this.m_SequenceDuration.drawLine = drawline;
				this.m_SequenceDuration.canMoveHead = flag;
				Rect timeAreaBounds = this.timeAreaBounds;
				timeAreaBounds.height = this.clientArea.height;
				this.m_SequenceDuration.Draw(timeAreaBounds, this.state, duration);
			}
			if (this.timeline != null && drawhead)
			{
				float num = this.state.TimeToPixel(duration);
				if (num > this.state.timeAreaRect.xMin)
				{
					Color colorDurationLine = DirectorStyles.Instance.customSkin.colorDurationLine;
					Rect rect = Rect.MinMaxRect(this.state.timeAreaRect.xMin, this.timeAreaBounds.y - TimelineWindowStyles.kDurationGuiThickness + this.timeAreaBounds.height, num, this.timeAreaBounds.y + this.timeAreaBounds.height);
					EditorGUI.DrawRect(rect, colorDurationLine);
				}
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002F34C File Offset: 0x0002D74C
		private void OnTrackDurationDrag(TimelineWindow.TimelineState state, double newTime, bool initialFrame)
		{
			if (state.timeline.durationMode == 1)
			{
				state.timeline.fixedDuration = newTime;
				if (this.state.currentDirector != null)
				{
					if (state.currentDirector.playableGraph.IsValid())
					{
						if (state.currentDirector.playableGraph.rootPlayableCount > 0)
						{
							PlayableHandle rootPlayable = state.currentDirector.playableGraph.GetRootPlayable(0);
							if (rootPlayable.IsValid())
							{
								rootPlayable.duration = newTime;
							}
						}
					}
				}
			}
			this.m_SequenceDuration.showTooltip = true;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002F3FC File Offset: 0x0002D7FC
		private void InitializeEditorCallbacks()
		{
			Undo.postprocessModifications = (Undo.PostprocessModifications)Delegate.Combine(Undo.postprocessModifications, new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications));
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.OnUndoRedo));
			EditorApplication.contextualPropertyMenu = (EditorApplication.SerializedPropertyCallbackFunction)Delegate.Combine(EditorApplication.contextualPropertyMenu, new EditorApplication.SerializedPropertyCallbackFunction(this.OnPropertyContextMenu));
			EditorApplication.playmodeStateChanged = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.playmodeStateChanged, new EditorApplication.CallbackFunction(this.OnPlayModeChanged));
			AnimationUtility.onCurveWasModified = (AnimationUtility.OnCurveWasModified)Delegate.Combine(AnimationUtility.onCurveWasModified, new AnimationUtility.OnCurveWasModified(this.OnCurveModified));
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002F4AC File Offset: 0x0002D8AC
		private void RemoveEditorCallbacks()
		{
			EditorApplication.contextualPropertyMenu = (EditorApplication.SerializedPropertyCallbackFunction)Delegate.Remove(EditorApplication.contextualPropertyMenu, new EditorApplication.SerializedPropertyCallbackFunction(this.OnPropertyContextMenu));
			EditorApplication.playmodeStateChanged = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.playmodeStateChanged, new EditorApplication.CallbackFunction(this.OnPlayModeChanged));
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.OnUndoRedo));
			Undo.postprocessModifications = (Undo.PostprocessModifications)Delegate.Remove(Undo.postprocessModifications, new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications));
			AnimationUtility.onCurveWasModified = (AnimationUtility.OnCurveWasModified)Delegate.Remove(AnimationUtility.onCurveWasModified, new AnimationUtility.OnCurveWasModified(this.OnCurveModified));
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002F55A File Offset: 0x0002D95A
		private void OnCurveModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type)
		{
			InspectorWindow.RepaintAllInspectors();
			this.state.Evaluate();
			if (this.state != null && type != 1)
			{
				this.state.rebuildGraph = true;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002F590 File Offset: 0x0002D990
		private void OnPlayModeChanged()
		{
			bool flag = EditorApplication.isPlayingOrWillChangePlaymode != EditorApplication.isPlaying;
			if (flag && this.state != null)
			{
				this.state.Stop();
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002F5CC File Offset: 0x0002D9CC
		private UndoPropertyModification[] PostprocessAnimationRecordingModifications(UndoPropertyModification[] modifications)
		{
			UndoPropertyModification[] result;
			if (!this.state.recording)
			{
				result = modifications;
			}
			else
			{
				UndoPropertyModification[] array = TimelineRecording.ProcessUndoModification(modifications, this.state);
				if (array != modifications)
				{
					bool flag = EditorWindow.focusedWindow == null || EditorWindow.focusedWindow is InspectorWindow || EditorWindow.focusedWindow is TimelineWindow;
					if (flag)
					{
						base.Repaint();
					}
				}
				result = array;
			}
			return result;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002F64C File Offset: 0x0002DA4C
		private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (this.state.recording && TimelineRecording.CanRecord(property, this.state))
			{
				SerializedProperty propertyCopy = property.Copy();
				if (TimelineRecording.HasKey(property, this.state))
				{
					menu.AddItem(EditorGUIUtility.TextContent("Remove Key"), false, delegate()
					{
						TimelineRecording.RemoveKey(propertyCopy, this.state);
						this.Repaint();
					});
				}
				else
				{
					menu.AddItem(EditorGUIUtility.TextContent("Add Key"), false, delegate()
					{
						TimelineRecording.AddKey(propertyCopy, this.state);
						this.Repaint();
					});
				}
				if (TimelineRecording.HasCurve(property, this.state))
				{
					menu.AddSeparator(string.Empty);
					menu.AddItem(EditorGUIUtility.TextContent("Remove All Keys"), false, delegate()
					{
						TimelineRecording.RemoveCurve(propertyCopy, this.state);
						this.Repaint();
					});
				}
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002F728 File Offset: 0x0002DB28
		private void OnUndoRedo()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			Undo.GetRecords(list, list2);
			bool flag = false;
			for (int i = 0; i < list2.Count; i++)
			{
				string text = list2[i];
				if (text.IndexOf("sequence") >= 0)
				{
					if (text.IndexOf("sequence.norefresh") < 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j];
					if (text2.IndexOf("sequence") >= 0)
					{
						if (text2.IndexOf("sequence.norefresh") < 0)
						{
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				if (this.state != null)
				{
					this.state.RebindAnimators();
					this.state.Refresh();
				}
				base.Repaint();
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0002F820 File Offset: 0x0002DC20
		private TimelineMode currentMode
		{
			get
			{
				switch (this.m_CurrentMode)
				{
				case SequencerModeType.Active:
					if (TimelineWindow.m_ActiveMode == null)
					{
						TimelineWindow.m_ActiveMode = new TimelineActiveMode();
					}
					return TimelineWindow.m_ActiveMode;
				case SequencerModeType.EditAsset:
					if (TimelineWindow.m_EditAssetMode == null)
					{
						TimelineWindow.m_EditAssetMode = new TimelineAssetEditionMode();
					}
					return TimelineWindow.m_EditAssetMode;
				}
				if (TimelineWindow.m_InactiveMode == null)
				{
					TimelineWindow.m_InactiveMode = new TimelineInactiveMode();
				}
				return TimelineWindow.m_InactiveMode;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002F8AC File Offset: 0x0002DCAC
		private void InitializeViews()
		{
			if (this.m_Views == null)
			{
				this.m_Views = new List<TimelineWindow.TimelineView>();
				this.m_Views.Add(new TimelineWindow.TimelineView("Timeline Editor", new TimelineWindow.TimelineViewType(this.TracksGUI)));
				this.m_Views.Add(new TimelineWindow.TimelineView("Curves", new TimelineWindow.TimelineViewType(this.EmptyViewGUI)));
				this.state.activeView = 0;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002F920 File Offset: 0x0002DD20
		private void EnableActiveMode()
		{
			this.m_CurrentMode = SequencerModeType.Active;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002F92A File Offset: 0x0002DD2A
		private void EnableInactiveMode()
		{
			this.m_CurrentMode = SequencerModeType.Inactive;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002F934 File Offset: 0x0002DD34
		private void EnableEditAssetMode()
		{
			this.m_CurrentMode = SequencerModeType.EditAsset;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002F940 File Offset: 0x0002DD40
		private void DoLayout()
		{
			this.Initialize();
			this.HandleSplitterResize();
			this.RunCaptureSession();
			this.ProcessManipulators();
			this.state.sequencerHeaderWidth = base.position.width * this.m_HierarchySplitterPerc;
			this.SequencerGUI();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0002F98C File Offset: 0x0002DD8C
		private void TimelineSectionGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
			this.DoSequenceSelectorGUI();
			this.DoBreadcrumbGUI();
			this.SearchFilterGUI();
			this.OptionsGUI();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			this.TimelineGUI();
			GUILayout.EndVertical();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002F9E8 File Offset: 0x0002DDE8
		private void SplitterGUI()
		{
			if (this.timeline != null && this.timeline.tracks.Count > 0)
			{
				this.m_SplitterLineRect.Set(this.state.sequencerHeaderWidth - 1f, 0f, 1f, this.clientArea.height);
				EditorGUI.DrawRect(this.m_SplitterLineRect, DirectorStyles.Instance.customSkin.colorTopOutline3);
				this.m_SplitterLineRect.Set(this.state.sequencerHeaderWidth, 0f, 1f, this.clientArea.height);
				EditorGUI.DrawRect(this.m_SplitterLineRect, DirectorStyles.Instance.customSkin.colorTopOutline3);
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002FAB5 File Offset: 0x0002DEB5
		private void DrawTimelineBackgroundColor()
		{
			EditorGUI.DrawRect(this.timeAreaBounds, DirectorStyles.Instance.customSkin.colorTimelineBackground);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002FAD2 File Offset: 0x0002DED2
		private void TrackViewsGUI()
		{
			this.m_Views[this.state.activeView].m_Callback(this.treeviewBounds, this.state, this.currentMode.TrackState(this.state));
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002FB14 File Offset: 0x0002DF14
		private void SequencerGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			this.DrawTimelineBackgroundColor();
			this.DurationGUI(TimelineWindow.TimelineItemArea.Header);
			this.PlayRangeGUI(TimelineWindow.TimelineItemArea.Header);
			this.TimeCursorGUI(TimelineWindow.TimelineItemArea.Header);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.SequencerHeaderGUI();
			this.TimelineSectionGUI();
			GUILayout.EndHorizontal();
			this.TrackViewsGUI();
			this.DurationGUI(TimelineWindow.TimelineItemArea.Lines);
			this.PlayRangeGUI(TimelineWindow.TimelineItemArea.Lines);
			this.TimeCursorGUI(TimelineWindow.TimelineItemArea.Lines);
			GUILayout.EndVertical();
			this.SplitterGUI();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002FB90 File Offset: 0x0002DF90
		private void RunCaptureSession()
		{
			if (Event.current.isKey && Event.current.keyCode == 27)
			{
				this.state.captured.Clear();
			}
			bool flag = false;
			if (this.state.captured.Count > 0)
			{
				List<IControl> list = new List<IControl>();
				foreach (IControl item in this.state.captured)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				for (int num = 0; num != list.Count; num++)
				{
					IControl control = list[num];
					bool flag2 = control.OnEvent(Event.current, this.state, true);
					if (flag2)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (Event.current.type == 1)
				{
					GUIUtility.hotControl = 0;
				}
				Event.current.Use();
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002FCC0 File Offset: 0x0002E0C0
		private void HandleSplitterResize()
		{
			this.state.mainAreaWidth = base.position.width;
			if (!(this.state.timeline == null))
			{
				Rect rect;
				rect..ctor(this.state.sequencerHeaderWidth - 3f, 0f, 6f, this.clientArea.height);
				EditorGUIUtility.AddCursorRect(rect, 19);
				if (Event.current.type == null)
				{
					if (rect.Contains(Event.current.mousePosition))
					{
						this.m_SplitterCaptured = 1;
					}
				}
				if (this.m_SplitterCaptured > 0)
				{
					if (Event.current.type == 1)
					{
						this.m_SplitterCaptured = 0;
						Event.current.Use();
					}
					if (Event.current.type == 3)
					{
						if (this.m_SplitterCaptured == 1)
						{
							float num = Event.current.delta.x / base.position.width;
							this.m_HierarchySplitterPerc = Mathf.Clamp(this.m_HierarchySplitterPerc + num, this.m_HierachySplitterMinMax.x, this.m_HierachySplitterMinMax.y);
						}
						Event.current.Use();
					}
				}
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002FE10 File Offset: 0x0002E210
		private void SearchFilterGUI()
		{
			if (this.currentMode.headerState.searchFilter != TimelineModeGUIState.Hidden)
			{
				using (new EditorGUI.DisabledGroupScope(this.currentMode.headerState.searchFilter == TimelineModeGUIState.Disabled))
				{
					EditorGUI.BeginChangeCheck();
					string searchFilter = EditorGUILayout.ToolbarSearchField(this.state.searchFilter, new GUILayoutOption[]
					{
						GUILayout.MinWidth(90f)
					});
					if (EditorGUI.EndChangeCheck())
					{
						this.state.searchFilter = searchFilter;
					}
				}
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002FEBC File Offset: 0x0002E2BC
		public void ShowNewTracksContextMenu(TimelineWindow.TimelineState state, TrackAsset parentTrack)
		{
			this.ShowNewTracksContextMenu(state, parentTrack, null);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002FEC8 File Offset: 0x0002E2C8
		private void AddMenuItem(GenericMenu menu, TrackAsset parentTrack, TimelineGroupGUI parentGroup, TrackType type)
		{
			GenericMenu.MenuFunction2 menuFunction = delegate(object arg)
			{
				this.state.selection.Clear();
				if (parentTrack is GroupTrack)
				{
					parentTrack.collapsed = false;
				}
				TrackAsset trackAsset = this.state.GetWindow().AddTrack(arg as TrackType, parentTrack);
				if (parentGroup != null)
				{
					this.treeView.data.SetExpanded(parentGroup, true);
				}
				if (trackAsset.GetType() == typeof(ActivationTrack))
				{
					TimelineClip timelineClip = trackAsset.CreateClip(0.0);
					timelineClip.displayName = ActivationTrackDrawer.Styles.ClipText.text;
					this.state.Refresh();
				}
			};
			string text = TimelineHelpers.GetTrackCategoryName(type);
			if (!string.IsNullOrEmpty(text))
			{
				text += "/";
			}
			menu.AddItem(new GUIContent(text + TimelineHelpers.GetTrackMenuName(type)), false, menuFunction, type);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002FF3C File Offset: 0x0002E33C
		public void ShowNewTracksContextMenu(TimelineWindow.TimelineState state, TrackAsset parentTrack, TimelineGroupGUI parentGroup)
		{
			GenericMenu genericMenu = new GenericMenu();
			string title = (!(parentTrack == null)) ? "Track Sub-Group" : "Track Group";
			genericMenu.AddItem(new GUIContent(title), false, delegate(object f)
			{
				state.selection.Clear();
				TimelineGroupGUI.Create(parentTrack, title);
				state.Refresh();
			}, null);
			genericMenu.AddSeparator("");
			IEnumerable<TrackType> enumerable = from x in TimelineHelpers.GetMixableTypes()
			where x.m_TrackType != typeof(GroupTrack)
			select x;
			IEnumerable<TrackType> enumerable2 = from x in enumerable
			where x.m_TrackType.FullName.Contains("UnityEngine.Timeline")
			select x;
			IEnumerable<TrackType> enumerable3 = enumerable.Except(enumerable2);
			foreach (TrackType type in enumerable2)
			{
				this.AddMenuItem(genericMenu, parentTrack, parentGroup, type);
			}
			genericMenu.AddSeparator("");
			foreach (TrackType type2 in enumerable3)
			{
				this.AddMenuItem(genericMenu, parentTrack, parentGroup, type2);
			}
			genericMenu.ShowAsContext();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000300C8 File Offset: 0x0002E4C8
		private void OptionsGUI()
		{
			if (this.currentMode.headerState.options != TimelineModeGUIState.Hidden && !(this.timeline == null))
			{
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(this.currentMode.headerState.options == TimelineModeGUIState.Disabled);
				try
				{
					if (GUILayout.Button(TimelineWindow.styles.optionsStyle.normal.background, EditorStyles.toolbarButton, new GUILayoutOption[0]))
					{
						GenericMenu genericMenu = new GenericMenu();
						genericMenu.AddItem(EditorGUIUtility.TextContent("Seconds"), !this.state.timeInFrames, new GenericMenu.MenuFunction2(this.ChangeTimeCode), "seconds");
						genericMenu.AddItem(EditorGUIUtility.TextContent("Frames"), this.state.timeInFrames, new GenericMenu.MenuFunction2(this.ChangeTimeCode), "frames");
						genericMenu.AddSeparator("");
						genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Frame rate"));
						genericMenu.AddItem(EditorGUIUtility.TextContent("Film (24)"), this.state.frameRate.Equals(24f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 24f);
						genericMenu.AddItem(EditorGUIUtility.TextContent("PAL (25)"), this.state.frameRate.Equals(25f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 25f);
						genericMenu.AddItem(EditorGUIUtility.TextContent("NTSC (29.97)"), this.state.frameRate.Equals(29.97f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 29.97f);
						genericMenu.AddItem(EditorGUIUtility.TextContent("30"), this.state.frameRate.Equals(30f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 30f);
						genericMenu.AddItem(EditorGUIUtility.TextContent("50"), this.state.frameRate.Equals(50f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 50f);
						genericMenu.AddItem(EditorGUIUtility.TextContent("60"), this.state.frameRate.Equals(60f), delegate(object r)
						{
							this.state.frameRate = (float)r;
						}, 60f);
						genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Custom"));
						genericMenu.AddSeparator("");
						genericMenu.AddItem(EditorGUIUtility.TextContent("Snap to Frame"), this.state.frameSnap, delegate()
						{
							this.state.frameSnap = !this.state.frameSnap;
						});
						genericMenu.AddItem(EditorGUIUtility.TextContent("Edge Snap"), this.state.edgeSnaps, delegate()
						{
							this.state.edgeSnaps = !this.state.edgeSnaps;
						});
						if (Unsupported.IsDeveloperBuild())
						{
							genericMenu.AddItem(EditorGUIUtility.TextContent("Debug TimeArea"), false, delegate()
							{
								Debug.LogFormat("translation: {0}   scale: {1}   rect: {2}   shownRange: {3}", new object[]
								{
									this.m_TimeArea.translation,
									this.m_TimeArea.scale,
									this.m_TimeArea.rect,
									this.m_TimeArea.shownArea
								});
							});
							genericMenu.AddItem(EditorGUIUtility.TextContent("Edit Skin"), false, delegate()
							{
								Selection.activeObject = DirectorStyles.Instance.customSkin;
							});
						}
						genericMenu.ShowAsContext();
					}
				}
				finally
				{
					disabledScope.Dispose();
				}
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0003043C File Offset: 0x0002E83C
		private void ChangeTimeCode(object obj)
		{
			string a = obj.ToString();
			if (a == "frames")
			{
				this.state.timeInFrames = true;
			}
			else
			{
				this.state.timeInFrames = false;
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00030480 File Offset: 0x0002E880
		private void EmptyViewGUI(Rect clientRect, TimelineWindow.TimelineState state, TimelineModeGUIState trackState)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Box("Hierarchy Pane", new GUILayoutOption[]
			{
				GUILayout.Width(state.sequencerHeaderWidth),
				GUILayout.Height(clientRect.height)
			});
			GUILayout.Box("Main Pane", new GUILayoutOption[]
			{
				GUILayout.Width(state.mainAreaWidth - state.sequencerHeaderWidth),
				GUILayout.Height(clientRect.height)
			});
			GUILayout.EndHorizontal();
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00030500 File Offset: 0x0002E900
		public Rect sequenceHeaderBounds
		{
			get
			{
				return Rect.MinMaxRect(0f, this.timeAreaBounds.yMax, this.state.sequencerHeaderWidth, base.position.yMax - 10f);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0003054C File Offset: 0x0002E94C
		private void ClearSequencerHeaderGUI()
		{
			Rect rect = Rect.MinMaxRect(0f, 0f, this.sequenceHeaderBounds.width, this.sequenceHeaderBounds.height);
			EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorTimecodeBackground);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0003059C File Offset: 0x0002E99C
		private void SequencerHeaderGUI()
		{
			bool flag = this.timeline == null;
			this.ClearSequencerHeaderGUI();
			EditorGUI.BeginDisabledGroup(flag);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (this.m_CurrentMode == SequencerModeType.EditAsset)
			{
				this.EditAssetModeToolbarGUI();
			}
			else
			{
				this.TransportToolbarGUI();
			}
			this.TrackOptionsGUI();
			GUILayout.EndVertical();
			EditorGUI.EndDisabledGroup();
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00030600 File Offset: 0x0002EA00
		private void EditAssetModeToolbarGUI()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[]
			{
				GUILayout.Width(this.sequenceHeaderBounds.width)
			});
			GUILayout.Space(TimelineWindowStyles.kBaseIndent - (float)EditorStyles.toolbarButton.border.left);
			GUILayout.Label(TimelineWindowStyles.sequenceAssetEditModeTitle, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0003066C File Offset: 0x0002EA6C
		private void TransportToolbarGUI()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[]
			{
				GUILayout.Width(this.sequenceHeaderBounds.width)
			});
			if (this.currentMode.ToolbarState(this.state) == TimelineModeGUIState.Hidden)
			{
				GUILayout.FlexibleSpace();
			}
			else
			{
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(this.currentMode.ToolbarState(this.state) == TimelineModeGUIState.Disabled);
				try
				{
					this.PreviewModeButtonGUI();
					this.GotoBeginingSequenceGUI();
					this.PreviousEventButtonGUI();
					this.PlayButtonGUI();
					this.NextEventButtonGUI();
					this.GotoEndSequenceGUI();
					GUILayout.Space(15f);
					this.PlayRangeButtonGUI();
					GUILayout.FlexibleSpace();
					this.TimeCodeGUI();
					GUILayout.Space(10f);
				}
				finally
				{
					disabledScope.Dispose();
				}
				GUILayout.EndHorizontal();
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00030754 File Offset: 0x0002EB54
		private void PreviewModeButtonGUI()
		{
			EditorGUI.BeginChangeCheck();
			bool flag = this.state.previewMode;
			flag = GUILayout.Toggle(flag, TimelineWindowStyles.previewContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.state.previewMode = flag;
				if (!flag)
				{
					this.state.playing = false;
				}
				else if (this.state.previewMode)
				{
					this.state.rebuildGraph = true;
				}
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000307D8 File Offset: 0x0002EBD8
		private void TrackOptionsGUI()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(this.sequenceHeaderBounds.width)
			});
			if (this.state.timeline != null)
			{
				GUILayout.Space(TimelineWindowStyles.kBaseIndent);
				this.AddButtonGUI();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0003083B File Offset: 0x0002EC3B
		private void GotoBeginingSequenceGUI()
		{
			if (GUILayout.Button(TimelineWindowStyles.gotoBeginingContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.state.time = 0.0;
				this.state.EnsurePlayHeadIsVisible();
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0003087C File Offset: 0x0002EC7C
		private void PlayButtonGUIEditor()
		{
			EditorGUI.BeginChangeCheck();
			bool flag = GUILayout.Toggle(this.state.playing, TimelineWindowStyles.playContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.state.GetWindow().Simulate(flag);
				this.state.playing = flag;
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000308DC File Offset: 0x0002ECDC
		private void PlayButtonGUIPlayMode()
		{
			bool flag = this.state.currentDirector != null && this.state.currentDirector.isActiveAndEnabled;
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(!flag);
			try
			{
				this.PlayButtonGUIEditor();
			}
			finally
			{
				disabledScope.Dispose();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0003094C File Offset: 0x0002ED4C
		private void PlayButtonGUI()
		{
			if (!Application.isPlaying)
			{
				this.PlayButtonGUIEditor();
			}
			else
			{
				this.PlayButtonGUIPlayMode();
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0003096A File Offset: 0x0002ED6A
		private void NextEventButtonGUI()
		{
			if (GUILayout.Button(TimelineWindowStyles.nextFrameContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.state.frame = this.state.frame + 1;
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0003099F File Offset: 0x0002ED9F
		private void PreviousEventButtonGUI()
		{
			if (GUILayout.Button(TimelineWindowStyles.previousFrameContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.state.frame = this.state.frame - 1;
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000309D4 File Offset: 0x0002EDD4
		private void GotoEndSequenceGUI()
		{
			if (GUILayout.Button(TimelineWindowStyles.gotoEndContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.state.time = this.state.timeline.duration;
				this.state.EnsurePlayHeadIsVisible();
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00030A24 File Offset: 0x0002EE24
		private void PlayRangeButtonGUI()
		{
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(EditorApplication.isPlaying);
			try
			{
				this.state.playRangeEnabled = GUILayout.Toggle(this.state.playRangeEnabled, DirectorStyles.Instance.playrangeContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			}
			finally
			{
				disabledScope.Dispose();
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00030A94 File Offset: 0x0002EE94
		private void AddButtonGUI()
		{
			if (this.currentMode.trackOptionsState.newButton != TimelineModeGUIState.Hidden)
			{
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(this.currentMode.trackOptionsState.newButton == TimelineModeGUIState.Disabled);
				try
				{
					if (GUILayout.Button(TimelineWindowStyles.newContent, new GUILayoutOption[0]))
					{
						TrackAsset parentTrack = null;
						List<TrackAsset> source = this.state.selection.SelectedTracks().ToList<TrackAsset>();
						if (source.Count<TrackAsset>() == 1)
						{
							parentTrack = (source.First<TrackAsset>() as GroupTrack);
						}
						this.ShowNewTracksContextMenu(this.state, parentTrack);
					}
				}
				finally
				{
					disabledScope.Dispose();
				}
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00030B5C File Offset: 0x0002EF5C
		private void TimeCodeGUI()
		{
			string text;
			if (this.state.timeline != null)
			{
				text = this.state.TimeAsString(this.state.time, "F2");
				bool flag = TimeUtility.OnFrameBoundary(this.state.time, (double)this.state.frameRate);
				if (this.state.timeInFrames)
				{
					if (flag)
					{
						text = this.state.frame.ToString();
					}
					else
					{
						text = TimeUtility.ToExactFrames(this.state.time, (double)this.state.frameRate).ToString("F2");
					}
				}
			}
			else
			{
				text = "0";
			}
			EditorGUI.BeginChangeCheck();
			string text2 = EditorGUILayout.DelayedTextField(text, EditorStyles.toolbarTextField, new GUILayoutOption[]
			{
				GUILayout.Width(70f)
			});
			bool flag2 = EditorGUI.EndChangeCheck();
			if (flag2)
			{
				if (this.state.timeInFrames)
				{
					int frame = this.state.frame;
					double d = 0.0;
					if (double.TryParse(text2, out d))
					{
						frame = Math.Max(0, (int)Math.Floor(d));
					}
					this.state.frame = frame;
				}
				else
				{
					double num = TimeUtility.ParseTimeCode(text2, (double)this.state.frameRate, -1.0);
					if (num > 0.0)
					{
						this.state.time = num;
					}
				}
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000710 RID: 1808 RVA: 0x00030CF8 File Offset: 0x0002F0F8
		// (remove) Token: 0x06000711 RID: 1809 RVA: 0x00030D30 File Offset: 0x0002F130
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDown;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000712 RID: 1810 RVA: 0x00030D68 File Offset: 0x0002F168
		// (remove) Token: 0x06000713 RID: 1811 RVA: 0x00030DA0 File Offset: 0x0002F1A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseDrag;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000714 RID: 1812 RVA: 0x00030DD8 File Offset: 0x0002F1D8
		// (remove) Token: 0x06000715 RID: 1813 RVA: 0x00030E10 File Offset: 0x0002F210
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseWheel;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000716 RID: 1814 RVA: 0x00030E48 File Offset: 0x0002F248
		// (remove) Token: 0x06000717 RID: 1815 RVA: 0x00030E80 File Offset: 0x0002F280
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseMove;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000718 RID: 1816 RVA: 0x00030EB8 File Offset: 0x0002F2B8
		// (remove) Token: 0x06000719 RID: 1817 RVA: 0x00030EF0 File Offset: 0x0002F2F0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent MouseUp;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x0600071A RID: 1818 RVA: 0x00030F28 File Offset: 0x0002F328
		// (remove) Token: 0x0600071B RID: 1819 RVA: 0x00030F60 File Offset: 0x0002F360
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DoubleClick;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600071C RID: 1820 RVA: 0x00030F98 File Offset: 0x0002F398
		// (remove) Token: 0x0600071D RID: 1821 RVA: 0x00030FD0 File Offset: 0x0002F3D0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyDown;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600071E RID: 1822 RVA: 0x00031008 File Offset: 0x0002F408
		// (remove) Token: 0x0600071F RID: 1823 RVA: 0x00031040 File Offset: 0x0002F440
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent KeyUp;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000720 RID: 1824 RVA: 0x00031078 File Offset: 0x0002F478
		// (remove) Token: 0x06000721 RID: 1825 RVA: 0x000310B0 File Offset: 0x0002F4B0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragPerform;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000722 RID: 1826 RVA: 0x000310E8 File Offset: 0x0002F4E8
		// (remove) Token: 0x06000723 RID: 1827 RVA: 0x00031120 File Offset: 0x0002F520
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragExited;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000724 RID: 1828 RVA: 0x00031158 File Offset: 0x0002F558
		// (remove) Token: 0x06000725 RID: 1829 RVA: 0x00031190 File Offset: 0x0002F590
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent DragUpdated;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000726 RID: 1830 RVA: 0x000311C8 File Offset: 0x0002F5C8
		// (remove) Token: 0x06000727 RID: 1831 RVA: 0x00031200 File Offset: 0x0002F600
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent Overlay;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000728 RID: 1832 RVA: 0x00031238 File Offset: 0x0002F638
		// (remove) Token: 0x06000729 RID: 1833 RVA: 0x00031270 File Offset: 0x0002F670
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ContextClick;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x0600072A RID: 1834 RVA: 0x000312A8 File Offset: 0x0002F6A8
		// (remove) Token: 0x0600072B RID: 1835 RVA: 0x000312E0 File Offset: 0x0002F6E0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ValidateCommand;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600072C RID: 1836 RVA: 0x00031318 File Offset: 0x0002F718
		// (remove) Token: 0x0600072D RID: 1837 RVA: 0x00031350 File Offset: 0x0002F750
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event TimelineUIEvent ExecuteCommand;

		// Token: 0x0600072E RID: 1838 RVA: 0x00031388 File Offset: 0x0002F788
		public virtual bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			return false;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0003139E File Offset: 0x0002F79E
		public void DrawOverlays(Event evt, TimelineWindow.TimelineState state)
		{
			if (this.Overlay != null)
			{
				this.Overlay(this, evt, state);
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000313C0 File Offset: 0x0002F7C0
		public bool IsMouseOver(Vector2 mousePosition)
		{
			return base.position.Contains(mousePosition);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000313E4 File Offset: 0x0002F7E4
		private static bool NoOp(object target, Event e, TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000313FC File Offset: 0x0002F7FC
		private void InitializeIControl()
		{
			if (TimelineWindow.<>f__mg$cache0 == null)
			{
				TimelineWindow.<>f__mg$cache0 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.MouseMove += TimelineWindow.<>f__mg$cache0;
			if (TimelineWindow.<>f__mg$cache1 == null)
			{
				TimelineWindow.<>f__mg$cache1 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.MouseDown += TimelineWindow.<>f__mg$cache1;
			if (TimelineWindow.<>f__mg$cache2 == null)
			{
				TimelineWindow.<>f__mg$cache2 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.MouseDrag += TimelineWindow.<>f__mg$cache2;
			if (TimelineWindow.<>f__mg$cache3 == null)
			{
				TimelineWindow.<>f__mg$cache3 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.MouseUp += TimelineWindow.<>f__mg$cache3;
			if (TimelineWindow.<>f__mg$cache4 == null)
			{
				TimelineWindow.<>f__mg$cache4 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.DoubleClick += TimelineWindow.<>f__mg$cache4;
			if (TimelineWindow.<>f__mg$cache5 == null)
			{
				TimelineWindow.<>f__mg$cache5 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.KeyDown += TimelineWindow.<>f__mg$cache5;
			if (TimelineWindow.<>f__mg$cache6 == null)
			{
				TimelineWindow.<>f__mg$cache6 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.KeyUp += TimelineWindow.<>f__mg$cache6;
			if (TimelineWindow.<>f__mg$cache7 == null)
			{
				TimelineWindow.<>f__mg$cache7 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.DragPerform += TimelineWindow.<>f__mg$cache7;
			if (TimelineWindow.<>f__mg$cache8 == null)
			{
				TimelineWindow.<>f__mg$cache8 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.DragExited += TimelineWindow.<>f__mg$cache8;
			if (TimelineWindow.<>f__mg$cache9 == null)
			{
				TimelineWindow.<>f__mg$cache9 = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.DragUpdated += TimelineWindow.<>f__mg$cache9;
			if (TimelineWindow.<>f__mg$cacheA == null)
			{
				TimelineWindow.<>f__mg$cacheA = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.MouseWheel += TimelineWindow.<>f__mg$cacheA;
			if (TimelineWindow.<>f__mg$cacheB == null)
			{
				TimelineWindow.<>f__mg$cacheB = new TimelineUIEvent(TimelineWindow.NoOp);
			}
			this.ContextClick += TimelineWindow.<>f__mg$cacheB;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x000315B0 File Offset: 0x0002F9B0
		public Rect clipArea
		{
			get
			{
				return new Rect(this.state.sequencerHeaderWidth, this.timeAreaBounds.yMax, this.clientArea.width - this.state.sequencerHeaderWidth - TimelineWindow.kScrollbarSize, this.clientArea.height - this.timeAreaBounds.height - TimelineWindow.kScrollbarSize);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00031628 File Offset: 0x0002FA28
		private void InitializeManipulators()
		{
			this.AddManipulator(new RectangleSelect());
			this.AddManipulator(new TrackZoom());
			this.AddManipulator(new FrameAll());
			this.AddManipulator(new FrameSelection());
			this.AddManipulator(new Jog());
			this.AddManipulator(new PrevNextFrame());
			this.AddManipulator(new PrevNextKey());
			this.AddManipulator(new TimelinePanManipulator());
			this.AddManipulator(new MouseWheelHorizontalScroll());
			this.AddManipulator(new GotoStartEnd());
			this.AddManipulator(new NudgeClips());
			this.AddManipulator(new TimelineZoomManipulator());
			this.AddManipulator(new TimelineShortcutManipulator());
			this.AddManipulator(new NewTrackContextMenu());
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000316D0 File Offset: 0x0002FAD0
		private void AddManipulator(Manipulator manipulator)
		{
			manipulator.Init(this);
			this.m_Manipulators.Add(manipulator);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000316E8 File Offset: 0x0002FAE8
		private void ProcessManipulators()
		{
			if (!(this.state.timeline == null))
			{
				if (Event.current.type == null && EditorGUI.IsEditingTextField())
				{
					EditorGUI.EndEditingActiveTextField();
				}
				Event current = Event.current;
				bool flag = false;
				switch (Event.current.type)
				{
				case 0:
					if (Event.current.clickCount < 2)
					{
						flag = Control.InvokeEvents(this.MouseDown, this, current, this.state);
					}
					else
					{
						flag = Control.InvokeEvents(this.DoubleClick, this, current, this.state);
					}
					break;
				case 1:
					flag = Control.InvokeEvents(this.MouseUp, this, current, this.state);
					break;
				case 2:
					flag = Control.InvokeEvents(this.MouseMove, this, current, this.state);
					break;
				case 3:
					flag = Control.InvokeEvents(this.MouseDrag, this, current, this.state);
					break;
				case 4:
					flag = Control.InvokeEvents(this.KeyDown, this, current, this.state);
					break;
				case 5:
					flag = Control.InvokeEvents(this.KeyUp, this, current, this.state);
					break;
				case 6:
					flag = Control.InvokeEvents(this.MouseWheel, this, current, this.state);
					break;
				case 9:
					flag = Control.InvokeEvents(this.DragUpdated, this, current, this.state);
					break;
				case 10:
					flag = Control.InvokeEvents(this.DragPerform, this, current, this.state);
					break;
				case 13:
					flag = Control.InvokeEvents(this.ValidateCommand, this, current, this.state);
					break;
				case 14:
					flag = Control.InvokeEvents(this.ExecuteCommand, this, current, this.state);
					break;
				case 15:
					flag = Control.InvokeEvents(this.DragExited, this, current, this.state);
					break;
				case 16:
					flag = Control.InvokeEvents(this.ContextClick, this, current, this.state);
					break;
				}
				if (flag)
				{
					if (Event.current.type == 1)
					{
						GUIUtility.hotControl = 0;
					}
					Event.current.Use();
				}
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0003192C File Offset: 0x0002FD2C
		private void PlayRangeGUI(TimelineWindow.TimelineItemArea area)
		{
			if (this.currentMode.ShouldShowPlayRange(this.state) && this.treeView != null)
			{
				if (!(this.timeline != null) || this.timeline.tracks.Count != 0)
				{
					if (this.m_PlayRangeStart == null)
					{
						this.m_PlayRangeStart = new TimelineItem(TimelineWindow.styles.playTimeRangeStart, new Action<TimelineWindow.TimelineState, double, bool>(this.OnTrackHeadMinSelectDrag));
						Vector2 boundOffset;
						boundOffset..ctor(-2f, 0f);
						this.m_PlayRangeStart.boundOffset = boundOffset;
					}
					if (this.m_PlayRangeEnd == null)
					{
						this.m_PlayRangeEnd = new TimelineItem(TimelineWindow.styles.playTimeRangeEnd, new Action<TimelineWindow.TimelineState, double, bool>(this.OnTrackHeadMaxSelectDrag));
						Vector2 boundOffset2;
						boundOffset2..ctor(2f, 0f);
						this.m_PlayRangeEnd.boundOffset = boundOffset2;
					}
					if (area == TimelineWindow.TimelineItemArea.Header)
					{
						this.DrawPlayRange(true, false);
					}
					else if (area == TimelineWindow.TimelineItemArea.Lines)
					{
						this.DrawPlayRange(false, true);
					}
				}
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00031A44 File Offset: 0x0002FE44
		private void DrawPlayRange(bool drawHeads, bool drawLines)
		{
			Rect timeAreaBounds = this.timeAreaBounds;
			timeAreaBounds.height = this.clientArea.height;
			if (Event.current.type == null)
			{
				if (this.m_PlayRangeEnd.bounds.Contains(Event.current.mousePosition))
				{
					if (this.m_PlayRangeEnd.OnEvent(Event.current, this.state, false))
					{
						Event.current.Use();
					}
				}
				else if (this.m_PlayRangeStart.bounds.Contains(Event.current.mousePosition))
				{
					if (this.m_PlayRangeStart.OnEvent(Event.current, this.state, false))
					{
						Event.current.Use();
					}
				}
			}
			if (this.state.playRangeNeedsReset)
			{
				float num = 1f;
				float num2 = Mathf.Max(0f, this.state.PixelToTime(this.state.timeAreaRect.xMin));
				float num3 = Mathf.Min((float)this.state.duration, this.state.PixelToTime(this.state.timeAreaRect.xMax));
				if (Mathf.Abs(num3 - num2) <= num)
				{
					this.state.playRangeTime = new Vector2(num2, num3);
					return;
				}
				float num4 = (num3 - num2) * 0.25f / 2f;
				num2 += num4;
				num3 -= num4;
				if (num3 < num2)
				{
					float num5 = num2;
					num2 = num3;
					num3 = num5;
				}
				if (Mathf.Abs(num3 - num2) < num)
				{
					if (num2 - num > 0f)
					{
						num2 -= num;
					}
					else if ((double)(num3 + num) < this.state.duration)
					{
						num3 += num;
					}
				}
				this.state.playRangeTime = new Vector2(num2, num3);
			}
			this.m_PlayRangeStart.drawHead = drawHeads;
			this.m_PlayRangeStart.drawLine = drawLines;
			this.m_PlayRangeEnd.drawHead = drawHeads;
			this.m_PlayRangeEnd.drawLine = drawLines;
			this.m_PlayRangeStart.Draw(timeAreaBounds, this.state, (double)this.state.playRangeTime.x);
			this.m_PlayRangeEnd.Draw(timeAreaBounds, this.state, (double)this.state.playRangeTime.y);
			if (this.state.playRangeEnabled)
			{
				Rect rect = Rect.MinMaxRect(Mathf.Clamp(this.state.TimeToPixel((double)this.state.playRangeTime.x), this.timeAreaBounds.xMin, this.timeAreaBounds.xMax), this.m_PlayHead.bounds.yMax, Mathf.Clamp(this.state.TimeToPixel((double)this.state.playRangeTime.y), this.timeAreaBounds.xMin, this.timeAreaBounds.xMax), timeAreaBounds.height + this.timeAreaBounds.height);
				EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorRange);
				rect.height = 3f;
				EditorGUI.DrawRect(rect, Color.white);
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00031DC8 File Offset: 0x000301C8
		private void OnTrackHeadMinSelectDrag(TimelineWindow.TimelineState state, double newTime, bool initialFrame)
		{
			Vector2 playRangeTime = state.playRangeTime;
			playRangeTime.x = (float)newTime;
			state.playRangeTime = playRangeTime;
			this.m_PlayRangeStart.showTooltip = true;
			state.ValidatePlayRange();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00031E00 File Offset: 0x00030200
		private void OnTrackHeadMaxSelectDrag(TimelineWindow.TimelineState state, double newTime, bool initialFrame)
		{
			Vector2 playRangeTime = state.playRangeTime;
			playRangeTime.y = (float)newTime;
			state.playRangeTime = playRangeTime;
			this.m_PlayRangeEnd.showTooltip = true;
			state.ValidatePlayRange();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00031E38 File Offset: 0x00030238
		public void Simulate(bool start)
		{
			if (start && this.state.currentDirector != null && this.state.currentDirector.state == null)
			{
				this.state.currentDirector.Play();
			}
			if (!start && this.state.currentDirector != null && this.state.currentDirector.state == 1)
			{
				if (Application.isPlaying)
				{
					this.state.currentDirector.Pause();
				}
			}
			this.state.playing = start;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00031EE4 File Offset: 0x000302E4
		public void SimulateFrames(int frames)
		{
			for (int i = 0; i < frames; i++)
			{
				base.RepaintImmediately();
				if (this.state.playing)
				{
					this.OnPreviewPlay();
				}
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00031F24 File Offset: 0x00030324
		private void OnPreviewPlayModeChanged(bool active)
		{
			if (!active || !EditorApplication.isPlaying)
			{
				this.m_PreviousTime = (double)Time.realtimeSinceStartup;
				if (active)
				{
					EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.OnPreviewPlay));
				}
				else
				{
					EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.OnPreviewPlay));
				}
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00031FA0 File Offset: 0x000303A0
		internal void OnPreviewPlay()
		{
			if (this.state != null)
			{
				if (!(this.m_Timeline == null))
				{
					if (!(this.state.currentDirector == null))
					{
						bool flag = this.state.currentDirector.timeUpdateMode == 3;
						if (flag)
						{
							base.Repaint();
						}
						else
						{
							DirectorWrapMode wrapMode = this.state.currentDirector.wrapMode;
							double num = (double)((!this.state.playRangeEnabled) ? 0f : this.state.playRangeTime.x);
							double num2 = (!this.state.playRangeEnabled) ? this.m_Timeline.duration : ((double)this.state.playRangeTime.y);
							if (this.state.isJogging)
							{
								this.state.time = Math.Max(this.state.time + (double)this.state.playbackSpeed, num);
								this.state.time = Math.Min(this.state.time, num2);
							}
							else
							{
								double deltaTime = (double)Time.realtimeSinceStartup - this.m_PreviousTime;
								this.m_PreviousTime = (double)Time.realtimeSinceStartup;
								double num3 = TimelineWindow.IncrementTime(this.state.time, deltaTime, this.state.currentDirector.timeUpdateMode);
								if (wrapMode != null)
								{
									if (wrapMode != 1)
									{
										if (wrapMode == 2)
										{
											if (num3 > num2)
											{
												this.state.time = 0.0;
												this.state.playing = false;
											}
										}
									}
									else if (num3 > num2)
									{
										this.state.time = num;
									}
								}
								else if (num3 > num2)
								{
									base.Repaint();
									return;
								}
								num3 = TimelineWindow.IncrementTime(this.state.time, deltaTime, this.state.currentDirector.timeUpdateMode);
								this.state.time = Math.Min(Math.Max(num3, num), num2);
							}
							base.Repaint();
						}
					}
				}
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000321F0 File Offset: 0x000305F0
		private static double IncrementTime(double time, double deltaTime, DirectorUpdateMode timeUpdateMode)
		{
			float val = (timeUpdateMode != 1) ? 1f : Time.timeScale;
			float num = Math.Max(val, 0f);
			return time + Math.Abs(deltaTime) * (double)num;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00032234 File Offset: 0x00030634
		public void OnSelectionChange()
		{
			if (this.locked || (this.state != null && this.state.recording))
			{
				this.RestoreLastSelection();
			}
			else
			{
				Object @object = Selection.activeObject as TimelineAsset;
				if (@object != null)
				{
					this.SetCurrentSelection(@object);
				}
				else
				{
					@object = Selection.activeGameObject;
					if (@object != null)
					{
						if (!TimelineUtility.IsPrefabOrAsset(@object))
						{
							this.SetCurrentSelection(@object);
							return;
						}
					}
					this.RestoreLastSelection();
				}
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000322CC File Offset: 0x000306CC
		private void RestoreLastSelection()
		{
			Object @object = EditorUtility.InstanceIDToObject(this.m_LastSelectedObjectID);
			if (@object != null)
			{
				this.SetCurrentSelection(@object);
			}
			else
			{
				this.SetCurrentTimeline(null, null);
				this.locked = false;
			}
			base.Repaint();
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00032318 File Offset: 0x00030718
		private void SetCurrentSelection(Object obj)
		{
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				PlayableDirector directorComponentForGameObject = TimelineUtility.GetDirectorComponentForGameObject(gameObject);
				TimelineAsset timelineAssetForDirectorComponent = TimelineUtility.GetTimelineAssetForDirectorComponent(directorComponentForGameObject);
				if (this.state == null || directorComponentForGameObject != this.state.currentDirector || timelineAssetForDirectorComponent != this.state.timeline)
				{
					this.SetCurrentTimeline(timelineAssetForDirectorComponent, directorComponentForGameObject);
				}
			}
			else
			{
				TimelineAsset timelineAsset = obj as TimelineAsset;
				if (timelineAsset != null)
				{
					this.SetCurrentTimeline(timelineAsset, null);
				}
			}
			base.Repaint();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000323B4 File Offset: 0x000307B4
		private void RestoreSelectionIfNecessary()
		{
			if (this.state.restoreSelection)
			{
				this.state.selection.RestoreSelection();
				this.state.restoreSelection = false;
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000744 RID: 1860 RVA: 0x000323E8 File Offset: 0x000307E8
		// (remove) Token: 0x06000745 RID: 1861 RVA: 0x00032420 File Offset: 0x00030820
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<TimelineWindow.StateEventArgs> OnStateChange;

		// Token: 0x06000746 RID: 1862 RVA: 0x00032456 File Offset: 0x00030856
		private void InitializeStateChange()
		{
			this.OnStateChange += delegate(object sender, TimelineWindow.StateEventArgs stateChangeEventArgs)
			{
				string propertyChanged = stateChangeEventArgs.propertyChanged;
				switch (propertyChanged)
				{
				case "playing":
					this.OnPreviewPlayModeChanged(stateChangeEventArgs.state.playing);
					break;
				case "dirtyStamp":
					this.state.UpdateRecordingState();
					if (this.treeView != null)
					{
						this.treeView.Reload();
					}
					if (this.state.timeline != null)
					{
						this.state.soloTracks = (from t in this.state.timeline.flattenedTracks
						where t.soloed
						select t).ToList<TrackAsset>();
					}
					break;
				case "rebuildGraph":
					if (!this.state.rebuildGraph)
					{
						if (this.treeView != null)
						{
							List<TimelineTrackBaseGUI> allTrackGuis = this.treeView.allTrackGuis;
							if (allTrackGuis != null)
							{
								for (int i = 0; i < allTrackGuis.Count; i++)
								{
									allTrackGuis[i].OnGraphRebuilt();
								}
							}
						}
					}
					break;
				case "trackHeight":
				case "trackScale":
					this.treeView.CalculateRowRects();
					break;
				case "currentDirector":
					if (stateChangeEventArgs.state.currentDirector == null)
					{
						this.state.playRangeEnabled = false;
					}
					else
					{
						stateChangeEventArgs.state.time = stateChangeEventArgs.state.currentDirector.time;
						this.state.ResetPlayRange();
					}
					break;
				case "time":
					if (!EditorApplication.isPlaying)
					{
						foreach (Camera camera in Camera.allCameras)
						{
							float num2 = (float)stateChangeEventArgs.state.time;
							EditorUtility.SetCameraAnimateMaterials(camera, true);
							EditorUtility.SetCameraAnimateMaterialsTime(camera, num2);
						}
						this.state.UpdateRecordingState();
						EditorApplication.SetSceneRepaintDirty();
					}
					this.state.Evaluate();
					InspectorWindow.RepaintAllInspectors();
					break;
				case "recording":
					if (!this.state.recording)
					{
						TrackAssetRecordingExtensions.ClearRecordingState();
					}
					break;
				case "searchFilter":
					this.state.Refresh();
					break;
				}
			};
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0003246C File Offset: 0x0003086C
		public Rect timeAreaBounds
		{
			get
			{
				return new Rect(this.state.sequencerHeaderWidth, TimelineWindow.kTimeAreaYPosition, Mathf.Max(base.position.width - this.state.sequencerHeaderWidth, TimelineWindow.kTimeAreaMinWidth), TimelineWindow.kTimeAreaHeight);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x000324C0 File Offset: 0x000308C0
		public Rect tracksBounds
		{
			get
			{
				Rect timeAreaBounds = this.timeAreaBounds;
				return Rect.MinMaxRect(timeAreaBounds.xMin, timeAreaBounds.yMax, timeAreaBounds.xMax - 1f, timeAreaBounds.yMax + base.position.height);
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00032514 File Offset: 0x00030914
		private void InitializeTimeArea()
		{
			if (this.m_TimeArea == null)
			{
				this.m_TimeArea = new TimeArea(false)
				{
					hRangeLocked = false,
					vRangeLocked = true,
					margin = 10f,
					scaleWithWindow = true,
					hSlider = false,
					vSlider = false,
					hRangeMin = 0f,
					rect = this.timeAreaBounds
				};
				this.InitTimeAreaFrameRate();
				this.InitTimeAreaShownRange();
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00032590 File Offset: 0x00030990
		private void TimelineGUI()
		{
			if (this.currentMode.ShouldShowTimeArea(this.state))
			{
				Rect timeAreaBounds = this.timeAreaBounds;
				this.m_TimeArea.rect = new Rect(timeAreaBounds.x, timeAreaBounds.y, timeAreaBounds.width, this.clientArea.height - timeAreaBounds.y);
				if (this.m_LastFrameRate != this.state.frameRate)
				{
					this.InitTimeAreaFrameRate();
				}
				if (this.m_LastShownRange != this.state.timeAreaShownRange)
				{
					this.InitTimeAreaShownRange();
					this.state.TimeAreaChanged();
				}
				this.m_TimeArea.TimeRuler(timeAreaBounds, this.state.frameRate, true, false, 1f, (!this.state.timeInFrames) ? 1 : 2);
				this.ContextMenuTimelineGUI(timeAreaBounds);
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00032681 File Offset: 0x00030A81
		private void InitTimeAreaFrameRate()
		{
			this.m_LastFrameRate = this.state.frameRate;
			this.m_TimeArea.hTicks.SetTickModulosForFrameRate(this.m_LastFrameRate);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000326AB File Offset: 0x00030AAB
		private void InitTimeAreaShownRange()
		{
			this.m_LastShownRange = this.state.timeAreaShownRange;
			this.m_TimeArea.SetShownHRange(this.m_LastShownRange.x, this.m_LastShownRange.y);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000326E0 File Offset: 0x00030AE0
		private void ContextMenuTimelineGUI(Rect rect)
		{
			if (Event.current.type == 16)
			{
				if (rect.Contains(Event.current.mousePosition))
				{
					GenericMenu genericMenu = new GenericMenu();
					IEnumerator enumerator = Enum.GetValues(typeof(TimelineAsset.DurationMode)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							TimelineAsset.DurationMode mode = (TimelineAsset.DurationMode)obj;
							GUIContent guicontent = EditorGUIUtility.TextContent("Duration Mode/" + ObjectNames.NicifyVariableName(mode.ToString()));
							if (this.state.recording)
							{
								genericMenu.AddDisabledItem(guicontent);
							}
							else
							{
								genericMenu.AddItem(guicontent, this.state.timeline.durationMode == mode, delegate()
								{
									TimelineWindow.SelectDurationCallback(this.state, mode);
								});
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					genericMenu.ShowAsContext();
					Event.current.Use();
				}
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00032814 File Offset: 0x00030C14
		private static void SelectDurationCallback(TimelineWindow.TimelineState state, TimelineAsset.DurationMode mode)
		{
			if (state.timeline.durationMode == null && mode == 1)
			{
				state.timeline.fixedDuration = state.duration;
			}
			state.timeline.durationMode = mode;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0003284C File Offset: 0x00030C4C
		private void TimeCursorGUI(TimelineWindow.TimelineItemArea area)
		{
			if (this.CanDrawTimeCursor(area))
			{
				if (this.m_PlayHead == null)
				{
					this.m_PlayHead = new TimelineItem(TimelineWindow.styles.timeCursor, new Action<TimelineWindow.TimelineState, double, bool>(this.OnTrackHeadDrag));
					this.m_PlayHead.AddManipulator(new TrackheadContextMenu());
				}
				bool flag = area == TimelineWindow.TimelineItemArea.Header;
				this.DrawTimeCursor(flag, !flag);
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000328BC File Offset: 0x00030CBC
		private bool CanDrawTimeCursor(TimelineWindow.TimelineItemArea area)
		{
			return this.currentMode.ShouldShowTimeCursor(this.state) && this.treeView != null && !(this.timeline == null) && (!(this.timeline != null) || this.timeline.tracks.Count != 0) && (area != TimelineWindow.TimelineItemArea.Lines || this.state.TimeIsInRange((float)this.state.time));
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00032964 File Offset: 0x00030D64
		private void DrawTimeCursor(bool drawHead, bool drawline)
		{
			Rect timeAreaBounds = this.timeAreaBounds;
			timeAreaBounds.height = this.clientArea.height;
			timeAreaBounds.y -= 3f;
			if (this.m_PlayHead.bounds.Contains(Event.current.mousePosition))
			{
				if (this.m_PlayHead.OnEvent(Event.current, this.state, false))
				{
					Event.current.Use();
				}
			}
			if (Event.current.type == null && Event.current.button == 0)
			{
				if (this.timeAreaBounds.Contains(Event.current.mousePosition))
				{
					this.state.playing = false;
					this.m_PlayHead.OnEvent(Event.current, this.state, false);
					if (EditorApplication.isPlaying && this.state.currentDirector != null)
					{
						if (this.state.currentDirector.state == 1)
						{
							this.state.currentDirector.Pause();
						}
					}
					this.state.time = (double)Mathf.Max(0f, (float)this.state.GetSnappedTimeAtMousePosition(Event.current.mousePosition));
				}
			}
			this.m_PlayHead.dottedLine = this.state.isClipSnapping;
			this.state.isClipSnapping = false;
			this.m_PlayHead.drawLine = drawline;
			this.m_PlayHead.drawHead = drawHead;
			this.m_PlayHead.Draw(timeAreaBounds, this.state, this.state.time);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00032B19 File Offset: 0x00030F19
		private void OnTrackHeadDrag(TimelineWindow.TimelineState state, double newTime, bool initialFrame)
		{
			state.time = Math.Max(0.0, newTime);
			this.m_PlayHead.showTooltip = true;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00032B40 File Offset: 0x00030F40
		public Rect treeviewBounds
		{
			get
			{
				return new Rect(0f, 42f, base.position.width, this.clientArea.height - 42f);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00032B88 File Offset: 0x00030F88
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00032BA2 File Offset: 0x00030FA2
		public TimelineTreeViewGUI treeView { get; private set; }

		// Token: 0x06000756 RID: 1878 RVA: 0x00032BAC File Offset: 0x00030FAC
		private void TracksGUI(Rect clientRect, TimelineWindow.TimelineState state, TimelineModeGUIState trackState)
		{
			if (Event.current.type == 7 && this.treeView != null)
			{
				state.quadTree.SetSize(new Rect(0f, 0f, base.position.width, this.treeView.contentSize.y));
				state.quadTree.screenSpaceOffset = new Vector2(0f, 42f - this.treeView.scrollPosition.y);
			}
			EditorGUI.DrawRect(clientRect, DirectorStyles.Instance.customSkin.colorSequenceBackground);
			if (state != null && state.timeline != null && state.timeline.tracks.Count > 0)
			{
				this.m_TimeArea.DrawMajorTicks(this.tracksBounds, state.frameRate);
			}
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.timeline == null)
			{
				this.DrawNoSequenceGUI(clientRect, state);
			}
			else if (this.timeline.tracks.Count == 0)
			{
				this.DrawEmptySequenceGUI(clientRect, trackState);
			}
			else
			{
				this.DrawTracksGUI(clientRect, trackState);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			this.DrawShadowUnderTimeline();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00032D14 File Offset: 0x00031114
		private void DrawShadowUnderTimeline()
		{
			Rect timeAreaBounds = this.timeAreaBounds;
			timeAreaBounds.xMin = 0f;
			timeAreaBounds.yMin = this.timeAreaBounds.yMax;
			timeAreaBounds.height = 15f;
			GUI.Box(timeAreaBounds, GUIContent.none, DirectorStyles.Instance.bottomShadow);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00032D6C File Offset: 0x0003116C
		private void DrawEmptySequenceGUI(Rect clientRect, TimelineModeGUIState trackState)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawTracksGUI(clientRect, trackState);
			GUILayout.Label(TimelineWindowStyles.emptySequenceMessage, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00032DC8 File Offset: 0x000311C8
		private void DrawNoSequenceGUI(Rect clientRect, TimelineWindow.TimelineState state)
		{
			bool flag = false;
			GameObject gameObject = (!(Selection.activeObject != null)) ? null : (Selection.activeObject as GameObject);
			GUIContent guicontent = TimelineWindowStyles.noSequenceAssetSelected;
			PlayableDirector playableDirector = (!(gameObject != null)) ? null : gameObject.GetComponent<PlayableDirector>();
			PlayableAsset playableAsset = (!(playableDirector != null)) ? null : playableDirector.playableAsset;
			if (gameObject != null && !TimelineUtility.IsPrefabOrAsset(gameObject) && playableAsset == null)
			{
				flag = true;
				guicontent = new GUIContent(string.Format(TimelineWindowStyles.createSequenceOnSelection.text, gameObject.name, "a Director component and a Timeline asset"));
			}
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Label(guicontent, new GUILayoutOption[0]);
			if (flag)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(GUI.skin.label.CalcSize(guicontent).x / 2f - 35f);
				if (GUILayout.Button("Create", new GUILayoutOption[]
				{
					GUILayout.Width(70f)
				}))
				{
					string text = EditorUtility.SaveFilePanelInProject(TimelineWindowStyles.createNewSequenceText.text, gameObject.name + "Timeline", "playable", TimelineWindowStyles.createNewSequenceText.text, ProjectWindowUtil.GetActiveFolderPath());
					if (!string.IsNullOrEmpty(text))
					{
						TimelineAsset timelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();
						AssetDatabase.CreateAsset(timelineAsset, text);
						if (playableDirector == null)
						{
							playableDirector = gameObject.AddComponent<PlayableDirector>();
						}
						playableDirector.playableAsset = timelineAsset;
						this.SetCurrentTimeline(timelineAsset, playableDirector);
						TrackAsset asset = state.GetWindow().AddTrack(typeof(AnimationTrack));
						TimelineUtility.SetSceneGameObject(state.currentDirector, asset, gameObject);
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00032FAC File Offset: 0x000313AC
		private void DrawTracksGUI(Rect clientRect, TimelineModeGUIState trackState)
		{
			if (trackState != TimelineModeGUIState.Hidden)
			{
				GUILayout.BeginVertical(new GUILayoutOption[]
				{
					GUILayout.Height(clientRect.height)
				});
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(trackState == TimelineModeGUIState.Disabled);
				try
				{
					this.TreeViewGUI(this.treeviewBounds);
				}
				finally
				{
					disabledScope.Dispose();
				}
				GUILayout.EndVertical();
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00033024 File Offset: 0x00031424
		private void TreeViewGUI(Rect area)
		{
			if (this.state.captured.Count > 0 && Event.current.type == 7)
			{
				Rect rect = area;
				rect.width = this.state.sequencerHeaderWidth;
				float num = 0f;
				if (rect.Contains(Event.current.mousePosition))
				{
					if (this.state.timeAreaRect.x >= 0f)
					{
						num = 2f;
					}
				}
				else
				{
					Rect rect2 = area;
					rect2.x = this.clientArea.width;
					rect2.width = 50f;
					if (rect2.Contains(Event.current.mousePosition))
					{
						num = -2f;
					}
				}
				if (Mathf.Abs(num) > 1E-45f)
				{
					Vector3 vector = this.state.timeAreaTranslation;
					vector.x += num;
					this.state.SetTimeAreaTransform(vector, this.state.timeAreaScale);
					Event @event = new Event();
					@event.type = 3;
					@event.mousePosition = Event.current.mousePosition;
					@event.delta = new Vector2(-num, 0f);
					for (int num2 = 0; num2 != this.state.captured.Count; num2++)
					{
						this.state.captured[num2].OnEvent(@event, this.state, true);
					}
				}
			}
			if (this.treeView != null)
			{
				this.treeView.OnGUI(area);
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000331DC File Offset: 0x000315DC
		private void Upgrade(TimelineAsset timeline)
		{
			if (!(timeline == null))
			{
				string text = "";
				foreach (TrackAsset trackAsset in timeline.tracks)
				{
					if (trackAsset != null && trackAsset.clips != null)
					{
						foreach (TimelineClip timelineClip in trackAsset.clips)
						{
							if (timelineClip.parentTrack == null)
							{
								timelineClip.parentTrack = trackAsset;
								Debug.LogWarning(timelineClip.displayName + " was parented to " + trackAsset.name);
								EditorUtility.SetDirty(trackAsset);
								text = "PARENT TRACK FIX-UP";
							}
							if (timelineClip.asset != null && timelineClip.asset is AnimationClip)
							{
								Debug.LogWarning("AnimationClips should be wrapped in an AnimationPlayableAsset object - Auto fixing");
								AnimationPlayableAsset animationPlayableAsset = ScriptableObject.CreateInstance<AnimationPlayableAsset>();
								animationPlayableAsset.clip = (timelineClip.asset as AnimationClip);
								timelineClip.asset = animationPlayableAsset;
								TimelineHelpers.SaveAssetIntoObject(animationPlayableAsset, trackAsset);
							}
						}
					}
				}
				for (int j = timeline.tracks.Count - 1; j >= 0; j--)
				{
					TrackAsset trackAsset2 = timeline.tracks[j];
					TrackAsset trackAsset3 = trackAsset2.parent as TrackAsset;
					if (trackAsset3 != null)
					{
						timeline.tracks.Remove(trackAsset2);
					}
				}
				string assetPath = AssetDatabase.GetAssetPath(timeline);
				foreach (TrackAsset trackAsset4 in timeline.tracks)
				{
					if (AssetDatabase.GetAssetPath(trackAsset4) == assetPath)
					{
						trackAsset4.hideFlags |= 1;
						foreach (TimelineClip timelineClip2 in trackAsset4.clips)
						{
							if (timelineClip2.asset != null)
							{
								string assetPath2 = AssetDatabase.GetAssetPath(timelineClip2.asset);
								if (assetPath2 == assetPath)
								{
									timelineClip2.asset.hideFlags |= 1;
								}
							}
							if (timelineClip2.curves != null)
							{
								timelineClip2.curves.hideFlags |= 1;
							}
						}
					}
				}
				foreach (TrackAsset trackAsset5 in timeline.flattenedTracks)
				{
					AnimationTrack animationTrack = trackAsset5 as AnimationTrack;
					if (animationTrack != null)
					{
						if (animationTrack.openClipTimeOffset > 0.0)
						{
							text += " time offset fixup";
							if (animationTrack.animClip != null)
							{
								animationTrack.animClip.ShiftBySeconds((float)(-(float)animationTrack.openClipTimeOffset));
							}
							animationTrack.openClipTimeOffset = 0.0;
						}
					}
				}
				IEnumerable<TimelineClip> enumerable = from x in timeline.flattenedTracks.SelectMany((TrackAsset x) => x.clips)
				where double.IsInfinity(x.start)
				select x;
				foreach (TimelineClip timelineClip3 in enumerable)
				{
					Debug.LogWarning("Removing " + timelineClip3.displayName + " becaue it has an invalid start time ");
					text = text + "Removing " + timelineClip3.displayName + " becaue it has an invalid start time ";
					timelineClip3.parentTrack.RemoveClip(timelineClip3);
				}
				foreach (TrackAsset trackAsset6 in timeline.tracks)
				{
					if (trackAsset6.parent == null)
					{
						trackAsset6.parent = this.state.timeline;
						text += " Fix parenting error";
					}
				}
				if (text.Length > 0)
				{
					Debug.LogWarning("SEQUENCE WAS UPGRADED (" + text + ") PLEASE RE-SAVE!");
					EditorUtility.SetDirty(timeline);
				}
				timeline.Invalidate();
			}
		}

		// Token: 0x040003C0 RID: 960
		[SerializeField]
		private TimelineWindow.SequenceWindowPreferences m_Preferences = new TimelineWindow.SequenceWindowPreferences();

		// Token: 0x040003C1 RID: 961
		[SerializeField]
		private bool m_Locked = false;

		// Token: 0x040003C2 RID: 962
		private readonly PreviewResizer m_PreviewResizer = new PreviewResizer();

		// Token: 0x040003C3 RID: 963
		private List<Action> m_NextRepaint = new List<Action>();

		// Token: 0x040003C4 RID: 964
		private bool m_LastFrameHadSequence = false;

		// Token: 0x040003C5 RID: 965
		private int m_CurrentSceneHashCode = -1;

		// Token: 0x040003C6 RID: 966
		[NonSerialized]
		private bool m_HasBeenInitialized = false;

		// Token: 0x040003CB RID: 971
		private TimelineAsset m_Timeline;

		// Token: 0x040003CC RID: 972
		private static TimelineWindow.BreadcrumbStyles s_Styles;

		// Token: 0x040003CD RID: 973
		[SerializeField]
		private List<BreadcrumbElement> m_BreadcrumbPath = new List<BreadcrumbElement>();

		// Token: 0x040003CF RID: 975
		public static readonly float kBreadcrumbHeight = 28f;

		// Token: 0x040003D0 RID: 976
		public static readonly float kBreadCrumbDelta = 270f;

		// Token: 0x040003D1 RID: 977
		private TimelineItem m_SequenceDuration;

		// Token: 0x040003D2 RID: 978
		private static TimelineMode m_ActiveMode;

		// Token: 0x040003D3 RID: 979
		private static TimelineMode m_InactiveMode;

		// Token: 0x040003D4 RID: 980
		private static TimelineMode m_EditAssetMode;

		// Token: 0x040003D5 RID: 981
		private Vector2 m_HierachySplitterMinMax = new Vector2(0.15f, 0.3f);

		// Token: 0x040003D6 RID: 982
		[SerializeField]
		private float m_HierarchySplitterPerc = 0.2f;

		// Token: 0x040003D7 RID: 983
		private int m_SplitterCaptured = 0;

		// Token: 0x040003D8 RID: 984
		private Rect m_SplitterLineRect = Rect.zero;

		// Token: 0x040003D9 RID: 985
		private List<TimelineWindow.TimelineView> m_Views;

		// Token: 0x040003DA RID: 986
		[SerializeField]
		private SequencerModeType m_CurrentMode;

		// Token: 0x040003DB RID: 987
		internal const int kTimeCodeFieldWidth = 70;

		// Token: 0x040003DC RID: 988
		internal const float kScrollBarHeight = 10f;

		// Token: 0x040003EC RID: 1004
		private static readonly float kScrollbarSize = 25f;

		// Token: 0x040003ED RID: 1005
		private List<Manipulator> m_Manipulators = new List<Manipulator>();

		// Token: 0x040003EE RID: 1006
		private TimelineItem m_PlayRangeEnd;

		// Token: 0x040003EF RID: 1007
		private TimelineItem m_PlayRangeStart;

		// Token: 0x040003F0 RID: 1008
		private double m_PreviousTime = 0.0;

		// Token: 0x040003F1 RID: 1009
		[SerializeField]
		private int m_LastSelectedObjectID;

		// Token: 0x040003F3 RID: 1011
		private TimeArea m_TimeArea;

		// Token: 0x040003F4 RID: 1012
		private static readonly float kTimeAreaYPosition = 17f;

		// Token: 0x040003F5 RID: 1013
		private static readonly float kTimeAreaHeight = 25f;

		// Token: 0x040003F6 RID: 1014
		private static readonly float kTimeAreaMinWidth = 50f;

		// Token: 0x040003F7 RID: 1015
		private float m_LastFrameRate;

		// Token: 0x040003F8 RID: 1016
		private Vector2 m_LastShownRange;

		// Token: 0x040003F9 RID: 1017
		private TimelineItem m_PlayHead;

		// Token: 0x040003FA RID: 1018
		private Vector2 m_ScrollPosition;

		// Token: 0x040003FB RID: 1019
		private const float kTracksYPosition = 42f;

		// Token: 0x040003FC RID: 1020
		private const float kOneSecond = 1f;

		// Token: 0x040003FD RID: 1021
		private const float kCreateButtonWidth = 70f;

		// Token: 0x040003FE RID: 1022
		private const float kTrackHeaderEndZoneWidth = 50f;

		// Token: 0x040003FF RID: 1023
		private const float k_TimeAreaMarkerStrength = 0.692836f;

		// Token: 0x04000404 RID: 1028
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache0;

		// Token: 0x04000405 RID: 1029
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache1;

		// Token: 0x04000406 RID: 1030
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache2;

		// Token: 0x04000407 RID: 1031
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache3;

		// Token: 0x04000408 RID: 1032
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache4;

		// Token: 0x04000409 RID: 1033
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache5;

		// Token: 0x0400040A RID: 1034
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache6;

		// Token: 0x0400040B RID: 1035
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache7;

		// Token: 0x0400040C RID: 1036
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache8;

		// Token: 0x0400040D RID: 1037
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cache9;

		// Token: 0x0400040E RID: 1038
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheA;

		// Token: 0x0400040F RID: 1039
		[CompilerGenerated]
		private static TimelineUIEvent <>f__mg$cacheB;

		// Token: 0x020000CE RID: 206
		public class TimelineState : ITimelineState
		{
			// Token: 0x06000770 RID: 1904 RVA: 0x00033C08 File Offset: 0x00032008
			public TimelineState(TimelineWindow w)
			{
				this.m_Window = w;
				this.m_Playing = false;
				this.m_Preferences = w.m_Preferences;
				this.selection = new Selection(this);
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x06000771 RID: 1905 RVA: 0x00033D30 File Offset: 0x00032130
			public static double kTimeEpsilon
			{
				get
				{
					return TimeUtility.kTimeEpsilon;
				}
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x06000772 RID: 1906 RVA: 0x00033D4C File Offset: 0x0003214C
			// (set) Token: 0x06000773 RID: 1907 RVA: 0x00033D66 File Offset: 0x00032166
			public ObjectPickerCallback ObjectPickerSelectionCallback { get; set; }

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x06000774 RID: 1908 RVA: 0x00033D70 File Offset: 0x00032170
			public EditorWindow editorWindow
			{
				get
				{
					return this.m_Window;
				}
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000775 RID: 1909 RVA: 0x00033D8C File Offset: 0x0003218C
			// (set) Token: 0x06000776 RID: 1910 RVA: 0x00033DA7 File Offset: 0x000321A7
			public bool rebuildGraph
			{
				get
				{
					return this.m_MustRebuildGraph;
				}
				set
				{
					this.SyncNotifyValue<bool>(ref this.m_MustRebuildGraph, value, "rebuildGraph");
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000777 RID: 1911 RVA: 0x00033DBC File Offset: 0x000321BC
			// (set) Token: 0x06000778 RID: 1912 RVA: 0x00033DD6 File Offset: 0x000321D6
			public bool restoreSelection { get; set; }

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x06000779 RID: 1913 RVA: 0x00033DE0 File Offset: 0x000321E0
			public double duration
			{
				get
				{
					double result;
					if (this.timeline == null)
					{
						result = 0.0;
					}
					else
					{
						result = ((this.timeline.durationMode != 1) ? this.timeline.duration : this.timeline.fixedDuration);
					}
					return result;
				}
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x0600077A RID: 1914 RVA: 0x00033E44 File Offset: 0x00032244
			// (set) Token: 0x0600077B RID: 1915 RVA: 0x00033E5E File Offset: 0x0003225E
			public float mouseDragLag { get; set; }

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x0600077C RID: 1916 RVA: 0x00033E68 File Offset: 0x00032268
			// (set) Token: 0x0600077D RID: 1917 RVA: 0x00033E83 File Offset: 0x00032283
			public Vector2 playRangeTime
			{
				get
				{
					return this.m_PlayRangeTime;
				}
				set
				{
					this.m_PlayRangeTime = value;
				}
			}

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x0600077E RID: 1918 RVA: 0x00033E90 File Offset: 0x00032290
			public bool playRangeNeedsReset
			{
				get
				{
					return this.m_PlayRangeTime == TimelineWindow.TimelineState.kNoPlayRangeSet;
				}
			}

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x0600077F RID: 1919 RVA: 0x00033EB8 File Offset: 0x000322B8
			public Vector2 playRangePixel
			{
				get
				{
					this.m_PlayRangePixel.x = this.TimeToPixel((double)this.m_PlayRangeTime.x);
					this.m_PlayRangePixel.y = this.TimeToPixel((double)this.m_PlayRangeTime.y);
					if (this.m_PlayRangePixel.x < this.timeAreaRect.xMin)
					{
						this.m_PlayRangePixel.x = this.timeAreaRect.xMin;
					}
					return this.m_PlayRangePixel;
				}
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x06000780 RID: 1920 RVA: 0x00033F44 File Offset: 0x00032344
			public DirectorStyles styles
			{
				get
				{
					return TimelineWindow.styles;
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000781 RID: 1921 RVA: 0x00033F60 File Offset: 0x00032360
			// (set) Token: 0x06000782 RID: 1922 RVA: 0x00033F7B File Offset: 0x0003237B
			public int keyboardControl
			{
				get
				{
					return this.m_TreeViewKeyboardControlId;
				}
				set
				{
					this.m_TreeViewKeyboardControlId = value;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000783 RID: 1923 RVA: 0x00033F88 File Offset: 0x00032388
			public QuadTree<IBounds> quadTree
			{
				get
				{
					return this.m_QuadTree;
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000784 RID: 1924 RVA: 0x00033FA4 File Offset: 0x000323A4
			public List<IControl> captured
			{
				get
				{
					return this.m_CaptureSession;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000785 RID: 1925 RVA: 0x00033FC0 File Offset: 0x000323C0
			// (set) Token: 0x06000786 RID: 1926 RVA: 0x00033FDB File Offset: 0x000323DB
			public PlayableDirector currentDirector
			{
				get
				{
					return this.m_CurrentDirector;
				}
				set
				{
					if (value != this.m_CurrentDirector)
					{
						this.OnCurrentDirectorWillChange(value);
					}
					this.SyncNotifyValue<PlayableDirector>(ref this.m_CurrentDirector, value, "currentDirector");
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000787 RID: 1927 RVA: 0x00034008 File Offset: 0x00032408
			// (set) Token: 0x06000788 RID: 1928 RVA: 0x00034023 File Offset: 0x00032423
			public int activeView
			{
				get
				{
					return this.m_ActiveView;
				}
				set
				{
					this.SyncNotifyValue<int>(ref this.m_ActiveView, value, "activeView");
				}
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000789 RID: 1929 RVA: 0x00034038 File Offset: 0x00032438
			// (set) Token: 0x0600078A RID: 1930 RVA: 0x00034053 File Offset: 0x00032453
			public List<TrackAsset> soloTracks
			{
				get
				{
					return this.m_SoloTracks;
				}
				set
				{
					this.m_SoloTracks = value;
				}
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x0600078B RID: 1931 RVA: 0x00034060 File Offset: 0x00032460
			public TimelineAsset timeline
			{
				get
				{
					return this.m_Window.timeline;
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x0600078C RID: 1932 RVA: 0x00034080 File Offset: 0x00032480
			public TrackAsset rootTrack
			{
				get
				{
					return this.m_RootTrack;
				}
			}

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x0600078D RID: 1933 RVA: 0x0003409C File Offset: 0x0003249C
			public TimelineAsset rootTimeline
			{
				get
				{
					return this.m_Window.rootTimeline;
				}
			}

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x0600078E RID: 1934 RVA: 0x000340BC File Offset: 0x000324BC
			// (set) Token: 0x0600078F RID: 1935 RVA: 0x000340D7 File Offset: 0x000324D7
			public bool isJogging
			{
				get
				{
					return this.m_IsJogging;
				}
				set
				{
					this.m_IsJogging = value;
				}
			}

			// Token: 0x1700010B RID: 267
			// (get) Token: 0x06000790 RID: 1936 RVA: 0x000340E4 File Offset: 0x000324E4
			// (set) Token: 0x06000791 RID: 1937 RVA: 0x000340FF File Offset: 0x000324FF
			public bool isDragging
			{
				get
				{
					return this.m_IsDragging;
				}
				set
				{
					this.m_IsDragging = value;
				}
			}

			// Token: 0x1700010C RID: 268
			// (get) Token: 0x06000792 RID: 1938 RVA: 0x0003410C File Offset: 0x0003250C
			// (set) Token: 0x06000793 RID: 1939 RVA: 0x00034126 File Offset: 0x00032526
			public Selection selection { get; private set; }

			// Token: 0x1700010D RID: 269
			// (get) Token: 0x06000794 RID: 1940 RVA: 0x00034130 File Offset: 0x00032530
			// (set) Token: 0x06000795 RID: 1941 RVA: 0x0003414B File Offset: 0x0003254B
			public float bindingAreaWidth
			{
				get
				{
					return this.m_BindingAreaWidth;
				}
				set
				{
					this.m_BindingAreaWidth = value;
				}
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000796 RID: 1942 RVA: 0x00034158 File Offset: 0x00032558
			// (set) Token: 0x06000797 RID: 1943 RVA: 0x00034173 File Offset: 0x00032573
			public float sequencerHeaderWidth
			{
				get
				{
					return this.m_SequencerHeaderWidth;
				}
				set
				{
					this.m_SequencerHeaderWidth = Mathf.Max(value, 268f);
				}
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000798 RID: 1944 RVA: 0x00034188 File Offset: 0x00032588
			// (set) Token: 0x06000799 RID: 1945 RVA: 0x000341A2 File Offset: 0x000325A2
			public float mainAreaWidth { get; set; }

			// Token: 0x17000110 RID: 272
			// (get) Token: 0x0600079A RID: 1946 RVA: 0x000341AC File Offset: 0x000325AC
			// (set) Token: 0x0600079B RID: 1947 RVA: 0x000341C7 File Offset: 0x000325C7
			public float trackHeight
			{
				get
				{
					return this.m_TrackHeight;
				}
				set
				{
					this.SyncNotifyValue<float>(ref this.m_TrackHeight, value, "trackHeight");
				}
			}

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x0600079C RID: 1948 RVA: 0x000341DC File Offset: 0x000325DC
			// (set) Token: 0x0600079D RID: 1949 RVA: 0x000341F7 File Offset: 0x000325F7
			public float trackScale
			{
				get
				{
					return this.m_TrackScale;
				}
				set
				{
					this.SyncNotifyValue<float>(ref this.m_TrackScale, value, "trackScale");
				}
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x0600079E RID: 1950 RVA: 0x0003420C File Offset: 0x0003260C
			// (set) Token: 0x0600079F RID: 1951 RVA: 0x00034227 File Offset: 0x00032627
			public int dirtyStamp
			{
				get
				{
					return this.m_DirtyStamp;
				}
				set
				{
					this.SyncNotifyValue<int>(ref this.m_DirtyStamp, value, "dirtyStamp");
				}
			}

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0003423C File Offset: 0x0003263C
			// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00034256 File Offset: 0x00032656
			public bool showQuadTree { get; set; }

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00034260 File Offset: 0x00032660
			public bool canRecord
			{
				get
				{
					return this.m_PreviewMode.enable;
				}
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00034280 File Offset: 0x00032680
			// (set) Token: 0x060007A4 RID: 1956 RVA: 0x000342B4 File Offset: 0x000326B4
			public bool recording
			{
				get
				{
					if (!this.m_PreviewMode.enable)
					{
						this.m_Recording = false;
					}
					return this.m_Recording;
				}
				set
				{
					if (value)
					{
						this.previewMode = true;
					}
					bool flag = value;
					if (!this.m_PreviewMode.enable)
					{
						flag = false;
					}
					if (flag && this.m_armedTracks.Count == 0)
					{
						Debug.LogError("Cannot enable recording without an armed track");
						flag = false;
					}
					if (!flag)
					{
						this.m_armedTracks.Clear();
					}
					this.SyncNotifyValue<bool>(ref this.m_Recording, flag, "recording");
				}
			}

			// Token: 0x17000116 RID: 278
			// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0003432C File Offset: 0x0003272C
			// (set) Token: 0x060007A6 RID: 1958 RVA: 0x0003434C File Offset: 0x0003274C
			public bool previewMode
			{
				get
				{
					return this.m_PreviewMode.enable;
				}
				set
				{
					if (!value)
					{
						this.m_PreviewMode.enable = false;
						if (this.m_CurrentDirector != null)
						{
							this.m_CurrentDirector.Stop();
						}
					}
					else if (!this.m_PreviewMode.enable)
					{
						this.EvaluateImmediate();
					}
				}
			}

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000343AC File Offset: 0x000327AC
			// (set) Token: 0x060007A8 RID: 1960 RVA: 0x000343F9 File Offset: 0x000327F9
			public bool playing
			{
				get
				{
					bool result;
					if (Application.isPlaying)
					{
						result = (this.currentDirector != null && this.currentDirector.state == 1);
					}
					else
					{
						result = this.m_Playing;
					}
					return result;
				}
				set
				{
					if (!Application.isPlaying)
					{
						this.SyncNotifyValue<bool>(ref this.m_Playing, value, "playing");
					}
				}
			}

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00034418 File Offset: 0x00032818
			// (set) Token: 0x060007AA RID: 1962 RVA: 0x00034433 File Offset: 0x00032833
			public float playbackSpeed
			{
				get
				{
					return this.m_PlaybackSpeed;
				}
				set
				{
					this.SyncNotifyValue<float>(ref this.m_PlaybackSpeed, value, "playbackSpeed");
				}
			}

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x060007AB RID: 1963 RVA: 0x00034448 File Offset: 0x00032848
			// (set) Token: 0x060007AC RID: 1964 RVA: 0x00034468 File Offset: 0x00032868
			public bool timeInFrames
			{
				get
				{
					return this.m_Preferences.timeInFrames;
				}
				set
				{
					this.SyncNotifyValue<bool>(ref this.m_Preferences.timeInFrames, value, "timeInFrames");
				}
			}

			// Token: 0x1700011A RID: 282
			// (get) Token: 0x060007AD RID: 1965 RVA: 0x00034484 File Offset: 0x00032884
			// (set) Token: 0x060007AE RID: 1966 RVA: 0x000344A4 File Offset: 0x000328A4
			public bool frameSnap
			{
				get
				{
					return this.m_Preferences.frameSnap;
				}
				set
				{
					this.SyncNotifyValue<bool>(ref this.m_Preferences.frameSnap, value, "frameSnap");
				}
			}

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x060007AF RID: 1967 RVA: 0x000344C0 File Offset: 0x000328C0
			// (set) Token: 0x060007B0 RID: 1968 RVA: 0x000344E0 File Offset: 0x000328E0
			public bool edgeSnaps
			{
				get
				{
					return this.m_Preferences.edgeSnaps;
				}
				set
				{
					this.m_Preferences.edgeSnaps = value;
				}
			}

			// Token: 0x1700011C RID: 284
			// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000344F0 File Offset: 0x000328F0
			// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0003450B File Offset: 0x0003290B
			public string searchFilter
			{
				get
				{
					return this.m_SearchFilter;
				}
				set
				{
					this.SyncNotifyValue<string>(ref this.m_SearchFilter, value, "searchFilter");
				}
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00034520 File Offset: 0x00032920
			// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0003455D File Offset: 0x0003295D
			public double time
			{
				get
				{
					if (this.currentDirector != null)
					{
						this.m_Time = this.currentDirector.time;
					}
					return this.m_Time;
				}
				set
				{
					if (this.currentDirector != null)
					{
						this.currentDirector.time = value;
					}
					this.SyncNotifyValue<double>(ref this.m_Time, value, "time");
				}
			}

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00034590 File Offset: 0x00032990
			public double localStart
			{
				get
				{
					return this.GetWindow().context.localStart;
				}
			}

			// Token: 0x1700011F RID: 287
			// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000345B8 File Offset: 0x000329B8
			// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000345D3 File Offset: 0x000329D3
			public bool isClipSnapping
			{
				get
				{
					return this.m_IsClipSnapping;
				}
				set
				{
					this.m_IsClipSnapping = value;
				}
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x000345DD File Offset: 0x000329DD
			public void OnDestroy()
			{
				if (this.m_CurrentDirector != null)
				{
					this.m_CurrentDirector.Stop();
				}
				this.m_PreviewMode.Dispose();
			}

			// Token: 0x060007B9 RID: 1977 RVA: 0x00034609 File Offset: 0x00032A09
			public void SetCurrentSequence(TimelineAsset asset)
			{
				Selection.activeObject = asset;
				this.GetWindow().OnSelectionChange();
				this.GetWindow().SetCurrentTimeline(asset, null);
			}

			// Token: 0x060007BA RID: 1978 RVA: 0x0003462C File Offset: 0x00032A2C
			public double SnapToFrameIfRequired(double time)
			{
				double result;
				if (this.frameSnap)
				{
					result = TimeUtility.FromFrames(TimeUtility.ToFrames(time, (double)this.frameRate), (double)this.frameRate);
				}
				else
				{
					result = time;
				}
				return result;
			}

			// Token: 0x060007BB RID: 1979 RVA: 0x0003466D File Offset: 0x00032A6D
			public void Reset()
			{
				this.recording = false;
				this.currentDirector = null;
				this.Stop();
				this.Refresh();
				this.ResetPlayRange();
				this.playRangeEnabled = false;
				this.playing = false;
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x000346A0 File Offset: 0x00032AA0
			public double GetSnappedTimeAtMousePosition(Vector2 mousePos)
			{
				return this.SnapToFrameIfRequired((double)this.ScreenSpacePixelToTimeAreaTime(mousePos.x));
			}

			// Token: 0x060007BD RID: 1981 RVA: 0x000346CC File Offset: 0x00032ACC
			private void SyncNotifyValue<T>(ref T oldValue, T newValue, string propertyChanged)
			{
				bool flag = false;
				if (oldValue == null)
				{
					oldValue = newValue;
					flag = true;
				}
				else if (!oldValue.Equals(newValue))
				{
					oldValue = newValue;
					flag = true;
				}
				if (flag && this.m_Window != null && this.m_Window.OnStateChange != null)
				{
					TimelineWindow.StateEventArgs stateEventArgs = new TimelineWindow.StateEventArgs();
					stateEventArgs.state = this;
					stateEventArgs.propertyChanged = propertyChanged;
					this.m_Window.OnStateChange(this.m_Window, stateEventArgs);
				}
			}

			// Token: 0x17000120 RID: 288
			// (get) Token: 0x060007BE RID: 1982 RVA: 0x00034774 File Offset: 0x00032B74
			// (set) Token: 0x060007BF RID: 1983 RVA: 0x0003479B File Offset: 0x00032B9B
			public int frame
			{
				get
				{
					return TimeUtility.ToFrames(this.time, (double)this.frameRate);
				}
				set
				{
					this.time = TimeUtility.FromFrames(Mathf.Max(0, value), (double)this.frameRate);
				}
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x000347B8 File Offset: 0x00032BB8
			public string TimeAsString(double timeValue, string format = "F2")
			{
				string result;
				if (this.timeInFrames)
				{
					result = TimeUtility.TimeAsFrames(timeValue, (double)this.frameRate, format);
				}
				else
				{
					result = TimeUtility.TimeAsTimeCode(timeValue, (double)this.frameRate, format);
				}
				return result;
			}

			// Token: 0x060007C1 RID: 1985 RVA: 0x000347FA File Offset: 0x00032BFA
			public void SetTimeAreaTransform(Vector2 newTranslation, Vector2 newScale)
			{
				this.m_Window.m_TimeArea.SetTransform(newTranslation, newScale);
				this.TimeAreaChanged();
			}

			// Token: 0x060007C2 RID: 1986 RVA: 0x00034815 File Offset: 0x00032C15
			public void SetTimeAreaScaleFocused(Vector2 focalPoint, Vector2 newScale, bool lockHorizontal = false, bool lockVertical = false)
			{
				this.m_Window.m_TimeArea.SetScaleFocused(focalPoint, newScale, lockHorizontal, lockVertical);
				this.TimeAreaChanged();
			}

			// Token: 0x060007C3 RID: 1987 RVA: 0x00034833 File Offset: 0x00032C33
			public void SetTimeAreaShownRange(float min, float max)
			{
				this.m_Window.m_TimeArea.SetShownHRange(min, max);
				this.TimeAreaChanged();
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x00034850 File Offset: 0x00032C50
			internal void TimeAreaChanged()
			{
				if (this.m_Window.m_TimeArea.scale.x > TimelineWindow.TimelineState.kMaxTimeAreaScaling)
				{
					Vector2 vector;
					vector..ctor(TimelineWindow.TimelineState.kMaxTimeAreaScaling, this.m_Window.m_TimeArea.scale.y);
					this.m_Window.m_TimeArea.SetTransform(this.m_Window.m_TimeArea.translation, vector);
				}
				if (this.timeline != null)
				{
					TimelineAsset.EditorSettings editorSettings = this.timeline.editorSettings;
					Vector2 vector2;
					vector2..ctor(this.m_Window.m_TimeArea.shownArea.x, this.m_Window.m_TimeArea.shownArea.xMax);
					if (editorSettings.timeAreaShownRange != vector2)
					{
						editorSettings.timeAreaShownRange = vector2;
						EditorUtility.SetDirty(this.timeline);
						EditorUtility.SetDirty(this.rootTimeline);
					}
				}
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x00034954 File Offset: 0x00032D54
			public bool TimeIsInRange(float value)
			{
				Rect shownArea = this.m_Window.m_TimeArea.shownArea;
				return value >= shownArea.x && value <= shownArea.xMax;
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x00034998 File Offset: 0x00032D98
			public void EnsurePlayHeadIsVisible()
			{
				double num = (double)this.PixelToTime(this.timeAreaRect.xMin);
				double num2 = (double)this.PixelToTime(this.timeAreaRect.xMax);
				double time = this.time;
				if (time < num || time > num2)
				{
					float num3 = (float)(num2 - num);
					float min = (float)time - num3 / 2f;
					float max = (float)time + num3 / 2f;
					this.SetTimeAreaShownRange(min, max);
				}
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00034A1C File Offset: 0x00032E1C
			// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00034A78 File Offset: 0x00032E78
			public float frameRate
			{
				get
				{
					float result;
					if (this.m_Window != null && this.m_Window.timeline != null)
					{
						result = this.m_Window.timeline.editorSettings.fps;
					}
					else
					{
						result = TimelineAsset.EditorSettings.kDefaultFPS;
					}
					return result;
				}
				set
				{
					TimelineAsset.EditorSettings editorSettings = this.timeline.editorSettings;
					if (editorSettings.fps != value)
					{
						editorSettings.fps = value;
						EditorUtility.SetDirty(this.timeline);
						EditorUtility.SetDirty(this.rootTimeline);
					}
				}
			}

			// Token: 0x17000122 RID: 290
			// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00034AC0 File Offset: 0x00032EC0
			public Vector2 timeAreaShownRange
			{
				get
				{
					Vector2 result;
					if (this.m_Window != null && this.m_Window.timeline != null)
					{
						result = this.m_Window.timeline.editorSettings.timeAreaShownRange;
					}
					else
					{
						result = TimelineAsset.EditorSettings.kTimeAreaDefaultRange;
					}
					return result;
				}
			}

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x060007CA RID: 1994 RVA: 0x00034B1C File Offset: 0x00032F1C
			public Vector2 timeAreaTranslation
			{
				get
				{
					return this.m_Window.m_TimeArea.translation;
				}
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x060007CB RID: 1995 RVA: 0x00034B44 File Offset: 0x00032F44
			public Vector2 timeAreaScale
			{
				get
				{
					return this.m_Window.m_TimeArea.scale;
				}
			}

			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060007CC RID: 1996 RVA: 0x00034B6C File Offset: 0x00032F6C
			public Rect timeAreaRect
			{
				get
				{
					return this.m_Window.timeAreaBounds;
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x060007CD RID: 1997 RVA: 0x00034B8C File Offset: 0x00032F8C
			public float windowHeight
			{
				get
				{
					return this.m_Window.position.height;
				}
			}

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060007CE RID: 1998 RVA: 0x00034BB4 File Offset: 0x00032FB4
			// (set) Token: 0x060007CF RID: 1999 RVA: 0x00034BCF File Offset: 0x00032FCF
			public bool playRangeEnabled
			{
				get
				{
					return this.m_PlayRangeEnabled;
				}
				set
				{
					this.m_PlayRangeEnabled = value;
					this.OnPlayRangeEnabledChanged();
				}
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x00034BDF File Offset: 0x00032FDF
			private void OnPlayRangeEnabledChanged()
			{
				this.ValidatePlayRange();
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x00034BE8 File Offset: 0x00032FE8
			public void ValidatePlayRange()
			{
				if (!this.playRangeNeedsReset)
				{
					float num = 1f / Mathf.Max(1f, this.frameRate);
					Vector2 playRangeTime = this.playRangeTime;
					if (playRangeTime.y - playRangeTime.x < num)
					{
						playRangeTime.x = playRangeTime.y - num;
					}
					if (playRangeTime.x < 0f)
					{
						playRangeTime.x = 0f;
					}
					if ((double)playRangeTime.y > this.duration)
					{
						playRangeTime.y = (float)this.duration;
					}
					if (playRangeTime.y - playRangeTime.x < num)
					{
						playRangeTime.y = Mathf.Min(playRangeTime.x + num, (float)this.duration);
					}
					this.m_PlayRangeTime = playRangeTime;
				}
			}

			// Token: 0x060007D2 RID: 2002 RVA: 0x00034CBF File Offset: 0x000330BF
			public void ResetPlayRange()
			{
				this.m_PlayRangeTime = TimelineWindow.TimelineState.kNoPlayRangeSet;
			}

			// Token: 0x060007D3 RID: 2003 RVA: 0x00034CD0 File Offset: 0x000330D0
			public TimelineWindow GetWindow()
			{
				return this.m_Window;
			}

			// Token: 0x060007D4 RID: 2004 RVA: 0x00034CEB File Offset: 0x000330EB
			public void Stop()
			{
				if (this.currentDirector != null)
				{
					this.currentDirector.Stop();
				}
			}

			// Token: 0x060007D5 RID: 2005 RVA: 0x00034D0A File Offset: 0x0003310A
			public void Play()
			{
				if (!(this.currentDirector == null))
				{
					this.currentDirector.Evaluate();
				}
			}

			// Token: 0x060007D6 RID: 2006 RVA: 0x00034D30 File Offset: 0x00033130
			public void BreadcrumbSetRoot(PlayableAsset asset)
			{
				if (asset == null)
				{
					this.currentDirector = null;
				}
				List<BreadcrumbElement> breadcrumbPath = new List<BreadcrumbElement>();
				this.m_Window.breadcrumbPath = breadcrumbPath;
				this.BreadcrumbGoto(asset, null);
			}

			// Token: 0x060007D7 RID: 2007 RVA: 0x00034D6C File Offset: 0x0003316C
			public void BreadcrumbGoto(PlayableAsset asset, TimelineClip instance)
			{
				if (!(asset == null))
				{
					BreadcrumbElement item = default(BreadcrumbElement);
					List<BreadcrumbElement> breadcrumbPath = this.m_Window.breadcrumbPath;
					if (breadcrumbPath.Count == 0)
					{
						item.asset = asset;
						item.clip = instance;
						breadcrumbPath.Add(item);
						this.m_Window.breadcrumbPath = breadcrumbPath;
					}
					else
					{
						item.asset = asset;
						item.clip = instance;
						int num = breadcrumbPath.IndexOf(item);
						if (num < 0)
						{
							Debug.LogWarning("Attempting to jump into a clip that was not part of the breadcrumb list");
						}
						else
						{
							try
							{
								if (num == 0)
								{
									TimelineAsset timelineAsset = Selection.activeObject as TimelineAsset;
									if (timelineAsset)
									{
										this.m_Window.SetCurrentTimeline(timelineAsset, this.currentDirector);
									}
									this.m_RootTrack = null;
								}
								this.m_Window.breadcrumbPath = breadcrumbPath.Take(num + 1).ToList<BreadcrumbElement>();
							}
							catch (Exception ex)
							{
								Debug.LogError("Exception in BreadcrumbGoto: " + ex.Message);
							}
						}
					}
				}
			}

			// Token: 0x060007D8 RID: 2008 RVA: 0x00034E8C File Offset: 0x0003328C
			public void BreadcrumbDrillInto(PlayableAsset asset)
			{
				this.BreadcrumbDrillInto(asset, null);
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x00034E98 File Offset: 0x00033298
			public void BreadcrumbDrillInto(PlayableAsset asset, TimelineClip instance)
			{
				if (asset == null)
				{
					Debug.LogWarning("Attempting to drill into a clip that is not nested or is null");
				}
				else
				{
					if (this.m_RootTrack == null)
					{
						this.m_RootTrack = instance.parentTrack;
					}
					this.selection.Clear();
					List<BreadcrumbElement> breadcrumbPath = this.m_Window.breadcrumbPath;
					breadcrumbPath.Add(new BreadcrumbElement
					{
						asset = asset,
						clip = instance
					});
					this.m_Window.breadcrumbPath = breadcrumbPath;
				}
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x00034F24 File Offset: 0x00033324
			public void Evaluate()
			{
				if (!(this.currentDirector == null))
				{
					if (!EditorApplication.isPlaying && !this.m_PreviewMode.enable)
					{
						this.GatherProperties(this.currentDirector);
					}
					this.currentDirector.DeferredEvaluate();
					if (!EditorApplication.isPlaying)
					{
						GameView.RepaintAll();
						SceneView.RepaintAll();
					}
				}
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x00034F90 File Offset: 0x00033390
			public void EvaluateImmediate()
			{
				if (!(this.currentDirector == null))
				{
					if (!EditorApplication.isPlaying && !this.m_PreviewMode.enable)
					{
						this.GatherProperties(this.currentDirector);
					}
					if (this.m_PreviewMode.enable)
					{
						this.currentDirector.Evaluate();
					}
				}
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x00034FF7 File Offset: 0x000333F7
			public void Refresh()
			{
				this.Refresh(true);
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x00035004 File Offset: 0x00033404
			public void Refresh(bool dirtyAsset)
			{
				this.selection.SaveSelection();
				this.selection.Clear();
				this.captured.Clear();
				this.CheckRecordingState();
				this.dirtyStamp++;
				if (dirtyAsset && this.m_Window.rootTimeline != null)
				{
					EditorUtility.SetDirty(this.m_Window.rootTimeline);
				}
				this.rebuildGraph = true;
				this.restoreSelection = true;
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00035084 File Offset: 0x00033484
			public bool IsEditingASubItem()
			{
				return this.IsCurrentEditingASequencerTextField() || this.IsCurrentEditingAnInlineCurve();
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x000350B0 File Offset: 0x000334B0
			public bool IsCurrentEditingASequencerTextField()
			{
				bool result;
				if (this.timeline == null)
				{
					result = false;
				}
				else if (TimelineWindow.TimelineState.kTimeCodeTextFieldId == GUIUtility.keyboardControl)
				{
					result = true;
				}
				else
				{
					result = (this.timeline.flattenedTracks.Count((TrackAsset t) => t.GetInstanceID() == GUIUtility.keyboardControl) != 0);
				}
				return result;
			}

			// Token: 0x060007E0 RID: 2016 RVA: 0x00035128 File Offset: 0x00033528
			public bool IsCurrentEditingAnInlineCurve()
			{
				bool result;
				if (this.timeline == null)
				{
					result = false;
				}
				else if (this.m_Window == null)
				{
					result = false;
				}
				else if ((from t in this.m_Window.allTracks
				where t is TimelineTrackGUI
				select t).Any((TimelineTrackBaseGUI t) => ((TimelineTrackGUI)t).inlineCurvesSelected))
				{
					result = true;
				}
				else
				{
					result = this.timeline.flattenedTracks.Any((TrackAsset t) => t.clips.Any((TimelineClip c) => c.inlineCurvesSelected));
				}
				return result;
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x00035204 File Offset: 0x00033604
			public float TimeToTimeAreaPixel(double time)
			{
				float num = (float)time;
				num *= this.timeAreaScale.x;
				return num + (this.timeAreaTranslation.x + this.sequencerHeaderWidth);
			}

			// Token: 0x060007E2 RID: 2018 RVA: 0x00035248 File Offset: 0x00033648
			public float TimeToScreenSpacePixel(double time)
			{
				float num = (float)time;
				num *= this.timeAreaScale.x;
				return num + this.timeAreaTranslation.x;
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x00035284 File Offset: 0x00033684
			public float TimeToPixel(double time)
			{
				return this.m_Window.m_TimeArea.TimeToPixel((float)time, this.m_Window.timeAreaBounds);
			}

			// Token: 0x060007E4 RID: 2020 RVA: 0x000352B8 File Offset: 0x000336B8
			public float PixelToTime(float pixel)
			{
				return this.m_Window.m_TimeArea.PixelToTime(pixel, this.m_Window.timeAreaBounds);
			}

			// Token: 0x060007E5 RID: 2021 RVA: 0x000352EC File Offset: 0x000336EC
			public float TimeAreaPixelToTime(float pixel)
			{
				return this.PixelToTime(pixel);
			}

			// Token: 0x060007E6 RID: 2022 RVA: 0x00035308 File Offset: 0x00033708
			public float ScreenSpacePixelToTimeAreaTime(float p)
			{
				p -= this.m_Window.timeAreaBounds.x;
				return this.TrackSpacePixelToTimeAreaTime(p);
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x0003533C File Offset: 0x0003373C
			public float TrackSpacePixelToTimeAreaTime(float p)
			{
				p -= this.timeAreaTranslation.x;
				float result;
				if (this.timeAreaScale.x > 0f)
				{
					result = p / this.timeAreaScale.x;
				}
				else
				{
					result = p;
				}
				return result;
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x00035394 File Offset: 0x00033794
			public void OffsetTimeArea(int pixels)
			{
				Vector3 vector = this.timeAreaTranslation;
				vector.x += (float)pixels;
				this.SetTimeAreaTransform(vector, this.timeAreaScale);
			}

			// Token: 0x060007E9 RID: 2025 RVA: 0x000353D0 File Offset: 0x000337D0
			public Component GetBindingForTrack(TrackAsset trackAsset)
			{
				Component result;
				if (this.currentDirector == null)
				{
					result = null;
				}
				else if (trackAsset.mediaType != null)
				{
					result = null;
				}
				else
				{
					GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(this.currentDirector, trackAsset);
					if (sceneGameObject == null)
					{
						result = null;
					}
					else
					{
						result = sceneGameObject.GetComponent<Animator>();
					}
				}
				return result;
			}

			// Token: 0x060007EA RID: 2026 RVA: 0x00035438 File Offset: 0x00033838
			public GameObject GetSceneReference(TrackAsset asset)
			{
				GameObject result;
				if (this.currentDirector == null)
				{
					result = null;
				}
				else
				{
					result = TimelineUtility.GetSceneGameObject(this.currentDirector, asset);
				}
				return result;
			}

			// Token: 0x060007EB RID: 2027 RVA: 0x00035474 File Offset: 0x00033874
			public void ArmForRecord(TrackAsset track)
			{
				this.m_armedTracks[TimelineUtility.GetSceneReferenceTrack(track)] = track;
				if (track != null && !this.recording)
				{
					this.recording = true;
				}
				if (this.recording)
				{
					track.OnRecordingArmed(this.currentDirector);
					if (this.m_Window != null && this.m_Window.treeView != null)
					{
						this.m_Window.treeView.CalculateRowRects();
					}
				}
			}

			// Token: 0x060007EC RID: 2028 RVA: 0x000354FF File Offset: 0x000338FF
			public void UnarmForRecord(TrackAsset track)
			{
				this.m_armedTracks.Remove(TimelineUtility.GetSceneReferenceTrack(track));
				if (this.m_armedTracks.Count == 0)
				{
					this.recording = false;
				}
				track.OnRecordingUnarmed(this.currentDirector);
			}

			// Token: 0x060007ED RID: 2029 RVA: 0x00035538 File Offset: 0x00033938
			public void UpdateRecordingState()
			{
				if (this.recording)
				{
					foreach (TrackAsset trackAsset in this.m_armedTracks.Values)
					{
						if (trackAsset != null)
						{
							trackAsset.OnRecordingTimeChanged(this.currentDirector);
						}
					}
				}
			}

			// Token: 0x060007EE RID: 2030 RVA: 0x000355BC File Offset: 0x000339BC
			public bool IsArmedForRecord(TrackAsset track)
			{
				return track == this.GetArmedTrack(track);
			}

			// Token: 0x060007EF RID: 2031 RVA: 0x000355E0 File Offset: 0x000339E0
			public TrackAsset GetArmedTrack(TrackAsset track)
			{
				TrackAsset result = null;
				this.m_armedTracks.TryGetValue(TimelineUtility.GetSceneReferenceTrack(track), out result);
				return result;
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x0003560C File Offset: 0x00033A0C
			private void CheckRecordingState()
			{
				if (this.m_armedTracks.Any((KeyValuePair<TrackAsset, TrackAsset> t) => t.Value == null))
				{
					this.m_armedTracks = (from t in this.m_armedTracks
					where t.Value != null
					select t).ToDictionary((KeyValuePair<TrackAsset, TrackAsset> t) => t.Key, (KeyValuePair<TrackAsset, TrackAsset> t) => t.Value);
					if (this.m_armedTracks.Count == 0)
					{
						this.recording = false;
					}
				}
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x000356CD File Offset: 0x00033ACD
			private void OnCurrentDirectorWillChange(PlayableDirector value)
			{
				if (this.m_CurrentDirector != null)
				{
					this.m_CurrentDirector.Stop();
				}
				this.m_PreviewMode.enable = false;
				this.rebuildGraph = true;
				this.GatherProperties(value);
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x00035708 File Offset: 0x00033B08
			public void GatherProperties(PlayableDirector director)
			{
				if (!(director == null))
				{
					if (!this.m_PreviewMode.enable)
					{
						if (!this.m_PreviewMode.canEnable)
						{
							return;
						}
						this.m_PreviewMode.enable = true;
					}
					TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
					if (timelineAsset != null)
					{
						PropertyCollector propertyCollector = new PropertyCollector();
						propertyCollector.PushActiveGameObject(null);
						timelineAsset.GatherProperties(director, propertyCollector);
					}
				}
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x00035790 File Offset: 0x00033B90
			public void RebindAnimators()
			{
				if (this.currentDirector != null)
				{
					IEnumerable<AnimationTrack> enumerable = this.timeline.GetTopLevelTracks().OfType<AnimationTrack>();
					foreach (AnimationTrack asset in enumerable)
					{
						GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(this.currentDirector, asset);
						if (sceneGameObject != null)
						{
							Animator component = sceneGameObject.GetComponent<Animator>();
							if (component != null)
							{
								component.Rebind();
							}
						}
					}
				}
			}

			// Token: 0x060007F4 RID: 2036 RVA: 0x00035840 File Offset: 0x00033C40
			public void RebindAnimators(List<GameObject> objects)
			{
				if (objects != null && objects.Count != 0)
				{
					foreach (GameObject gameObject in objects)
					{
						Animator component = gameObject.GetComponent<Animator>();
						if (component != null)
						{
							component.Rebind();
						}
					}
				}
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x000358C8 File Offset: 0x00033CC8
			public void ProcessPendingUpdates()
			{
				if (this.onEndFrame != null)
				{
					this.onEndFrame(this);
					this.onEndFrame = null;
				}
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x000358EB File Offset: 0x00033CEB
			public void AddEndFrameDelegate(PendingUpdateDelegate updateDelegate)
			{
				if (this.onEndFrame == null || !this.onEndFrame.GetInvocationList().Contains(updateDelegate))
				{
					this.onEndFrame = (PendingUpdateDelegate)Delegate.Combine(this.onEndFrame, updateDelegate);
				}
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x0003592C File Offset: 0x00033D2C
			internal TrackBindingValidationResult ValidateBindingForTrack(TrackAsset track)
			{
				TrackBindingValidationResult result;
				if (this.currentDirector == null)
				{
					result = new TrackBindingValidationResult(TimelineTrackBindingState.NoGameObjectBound, null);
				}
				else
				{
					PlayableBinding[] outputs = track.outputs;
					if (outputs == null || outputs.Length == 0)
					{
						result = new TrackBindingValidationResult(TimelineTrackBindingState.Valid, null);
					}
					else
					{
						Object genericBinding = this.currentDirector.GetGenericBinding(outputs[0].sourceObject);
						if (outputs[0].streamType == null)
						{
							GameObject gameObject = genericBinding as GameObject;
							if (gameObject == null)
							{
								return new TrackBindingValidationResult(TimelineTrackBindingState.NoGameObjectBound, null);
							}
							Animator component = gameObject.GetComponent<Animator>();
							if (component == null)
							{
								return new TrackBindingValidationResult(TimelineTrackBindingState.NoValidComponentOnBoundGameObject, genericBinding.name);
							}
							if (!gameObject.activeInHierarchy)
							{
								return new TrackBindingValidationResult(TimelineTrackBindingState.BoundGameObjectIsDisabled, genericBinding.name);
							}
							if (!component.enabled)
							{
								return new TrackBindingValidationResult(TimelineTrackBindingState.RequiredComponentOnBoundGameObjectIsDisabled, null);
							}
						}
						result = new TrackBindingValidationResult(TimelineTrackBindingState.Valid, null);
					}
				}
				return result;
			}

			// Token: 0x04000414 RID: 1044
			private TimelineWindow m_Window;

			// Token: 0x04000415 RID: 1045
			private readonly AnimationRecordMode m_PreviewMode = new AnimationRecordMode();

			// Token: 0x04000416 RID: 1046
			private bool m_Recording = false;

			// Token: 0x04000417 RID: 1047
			private bool m_Playing;

			// Token: 0x04000418 RID: 1048
			private float m_PlaybackSpeed = 0f;

			// Token: 0x04000419 RID: 1049
			private string m_SearchFilter = "";

			// Token: 0x0400041A RID: 1050
			private double m_Time = 0.0;

			// Token: 0x0400041B RID: 1051
			private bool m_IsJogging = false;

			// Token: 0x0400041C RID: 1052
			private readonly QuadTree<IBounds> m_QuadTree = new QuadTree<IBounds>();

			// Token: 0x0400041D RID: 1053
			private readonly List<IControl> m_CaptureSession = new List<IControl>();

			// Token: 0x0400041E RID: 1054
			private int m_DirtyStamp = 0;

			// Token: 0x0400041F RID: 1055
			private float m_TrackHeight = TimelineWindowStyles.kDefaultTrackHeight;

			// Token: 0x04000420 RID: 1056
			private float m_TrackScale = 1f;

			// Token: 0x04000421 RID: 1057
			private float m_SequencerHeaderWidth = 220f;

			// Token: 0x04000422 RID: 1058
			private float m_BindingAreaWidth = 40f;

			// Token: 0x04000423 RID: 1059
			private int m_ActiveView = 0;

			// Token: 0x04000424 RID: 1060
			private PlayableDirector m_CurrentDirector = null;

			// Token: 0x04000425 RID: 1061
			private bool m_MustRebuildGraph = false;

			// Token: 0x04000426 RID: 1062
			private TrackAsset m_RootTrack = null;

			// Token: 0x04000427 RID: 1063
			private bool m_PlayRangeEnabled = false;

			// Token: 0x04000428 RID: 1064
			private bool m_IsClipSnapping = false;

			// Token: 0x04000429 RID: 1065
			private static readonly Vector2 kNoPlayRangeSet = new Vector2(float.MaxValue, float.MaxValue);

			// Token: 0x0400042A RID: 1066
			private Vector2 m_PlayRangeTime = TimelineWindow.TimelineState.kNoPlayRangeSet;

			// Token: 0x0400042B RID: 1067
			private List<TrackAsset> m_SoloTracks = new List<TrackAsset>();

			// Token: 0x0400042C RID: 1068
			public static readonly int kTimeCodeTextFieldId = 3790;

			// Token: 0x0400042D RID: 1069
			private static readonly float kMaxTimeAreaScaling = 90000f;

			// Token: 0x0400042E RID: 1070
			private Dictionary<TrackAsset, TrackAsset> m_armedTracks = new Dictionary<TrackAsset, TrackAsset>();

			// Token: 0x0400042F RID: 1071
			private int m_TreeViewKeyboardControlId = 0;

			// Token: 0x04000430 RID: 1072
			private TimelineWindow.SequenceWindowPreferences m_Preferences;

			// Token: 0x04000431 RID: 1073
			public PendingUpdateDelegate onEndFrame;

			// Token: 0x04000435 RID: 1077
			private Vector2 m_PlayRangePixel = Vector2.zero;

			// Token: 0x04000436 RID: 1078
			private bool m_IsDragging = false;
		}

		// Token: 0x020000CF RID: 207
		[Serializable]
		public class SequenceWindowPreferences
		{
			// Token: 0x04000443 RID: 1091
			public bool timeInFrames = true;

			// Token: 0x04000444 RID: 1092
			public bool frameSnap = true;

			// Token: 0x04000445 RID: 1093
			public bool edgeSnaps = true;
		}

		// Token: 0x020000D2 RID: 210
		public class SequenceContext
		{
			// Token: 0x0400044C RID: 1100
			public PlayableAsset currentAsset;

			// Token: 0x0400044D RID: 1101
			public Component currentBinding = null;

			// Token: 0x0400044E RID: 1102
			public GameObject currentGameObject = null;

			// Token: 0x0400044F RID: 1103
			public double globalDuration = 0.0;

			// Token: 0x04000450 RID: 1104
			public double globalStart = 0.0;

			// Token: 0x04000451 RID: 1105
			public double localDuration = 0.0;

			// Token: 0x04000452 RID: 1106
			public double localStart = 0.0;

			// Token: 0x04000453 RID: 1107
			public TimelineAsset rootTimeline;
		}

		// Token: 0x020000D3 RID: 211
		private class SequnenceMenuNameFormater
		{
			// Token: 0x0600082B RID: 2091 RVA: 0x00035C24 File Offset: 0x00034024
			public string Format(string text)
			{
				int hashCode = text.GetHashCode();
				int num = 0;
				string result;
				if (this.m_UniqueItem.ContainsKey(hashCode))
				{
					num = this.m_UniqueItem[hashCode];
					num++;
					this.m_UniqueItem[hashCode] = num;
					result = string.Format("{0}{1}", text, num);
				}
				else
				{
					this.m_UniqueItem.Add(hashCode, num);
					result = text;
				}
				return result;
			}

			// Token: 0x04000454 RID: 1108
			private Dictionary<int, int> m_UniqueItem = new Dictionary<int, int>();
		}

		// Token: 0x020000D4 RID: 212
		private class BreadcrumbStyles
		{
			// Token: 0x04000455 RID: 1109
			public readonly GUIStyle breadCrumbLeft = "GUIEditor.BreadcrumbLeft";

			// Token: 0x04000456 RID: 1110
			public readonly GUIStyle breadCrumbMid = "GUIEditor.BreadcrumbMid";
		}

		// Token: 0x020000D5 RID: 213
		internal enum PlayModeState
		{
			// Token: 0x04000458 RID: 1112
			Paused,
			// Token: 0x04000459 RID: 1113
			Stopped,
			// Token: 0x0400045A RID: 1114
			Playing
		}

		// Token: 0x020000D6 RID: 214
		private enum TimelineItemArea
		{
			// Token: 0x0400045C RID: 1116
			Header,
			// Token: 0x0400045D RID: 1117
			Lines
		}

		// Token: 0x020000D7 RID: 215
		// (Invoke) Token: 0x0600082E RID: 2094
		internal delegate void TimelineViewType(Rect clientRect, TimelineWindow.TimelineState state, TimelineModeGUIState trackState);

		// Token: 0x020000D8 RID: 216
		internal class TimelineView
		{
			// Token: 0x06000831 RID: 2097 RVA: 0x00035CC6 File Offset: 0x000340C6
			public TimelineView(string name, TimelineWindow.TimelineViewType callback)
			{
				this.m_Name = name;
				this.m_Callback = callback;
			}

			// Token: 0x0400045E RID: 1118
			public TimelineWindow.TimelineViewType m_Callback;

			// Token: 0x0400045F RID: 1119
			public string m_Name;
		}

		// Token: 0x020000D9 RID: 217
		internal class StateEventArgs : EventArgs
		{
			// Token: 0x04000460 RID: 1120
			public string propertyChanged;

			// Token: 0x04000461 RID: 1121
			public TimelineWindow.TimelineState state;
		}
	}
}

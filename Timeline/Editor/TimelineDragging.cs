using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x02000087 RID: 135
	internal class TimelineDragging : TreeViewDragging
	{
		// Token: 0x06000462 RID: 1122 RVA: 0x00022371 File Offset: 0x00020771
		public TimelineDragging(TreeViewController treeView, TimelineWindow window, TimelineAsset data) : base(treeView)
		{
			this.m_Timeline = data;
			this.m_Window = window;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00022390 File Offset: 0x00020790
		internal static int GetItemControlID(TreeViewItem item)
		{
			return ((item == null) ? 0 : item.id) + 10000000;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000223C0 File Offset: 0x000207C0
		public override bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition)
		{
			bool result;
			if (this.m_Window.state.isDragging)
			{
				result = false;
			}
			else
			{
				TimelineTrackBaseGUI timelineTrackBaseGUI = targetItem as TimelineTrackBaseGUI;
				result = (timelineTrackBaseGUI != null && !(timelineTrackBaseGUI.track == null) && !timelineTrackBaseGUI.track.locked && Event.current.modifiers == null && (Event.current.type != 3 || Mathf.Abs(Event.current.delta.y) >= (float)this.kDragSensitivity));
			}
			return result;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00022488 File Offset: 0x00020888
		public override void StartDrag(TreeViewItem draggedNode, List<int> draggedItemIDs)
		{
			DragAndDrop.PrepareStartDrag();
			List<TreeViewItem> draggedItems = TimelineWindow.instance.state.selection.FilterByType<TreeViewItem>();
			DragAndDrop.SetGenericData("SequencerDragging", new TimelineDragging.TimelineDragData(draggedItems));
			DragAndDrop.objectReferences = new Object[0];
			string text = draggedItemIDs.Count + ((draggedItemIDs.Count <= 1) ? "" : "s");
			TimelineGroupGUI timelineGroupGUI = draggedNode as TimelineGroupGUI;
			if (timelineGroupGUI != null)
			{
				text = timelineGroupGUI.displayName;
			}
			DragAndDrop.StartDrag(text);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00022514 File Offset: 0x00020914
		public TrackType ResolveTypeAmbiguity(TrackType[] types)
		{
			TrackType returnedType = null;
			GenericMenu genericMenu = new GenericMenu();
			foreach (TrackType trackType in types)
			{
				genericMenu.AddItem(new GUIContent(trackType.m_TrackType.Name), false, delegate(object s)
				{
					returnedType = (TrackType)s;
				}, trackType);
			}
			genericMenu.ShowAsContext();
			return returnedType;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00022590 File Offset: 0x00020990
		public override bool DragElement(TreeViewItem targetItem, Rect targetItemRect, bool firstItem)
		{
			TimelineTrackGUI timelineTrackGUI = targetItem as TimelineTrackGUI;
			bool flag = !(DragAndDrop.GetGenericData("SequencerDragging") is TimelineDragging.TimelineDragData) && (DragAndDrop.objectReferences.Any<Object>() || DragAndDrop.paths.Any<string>());
			return (timelineTrackGUI == null || !flag || targetItemRect.Contains(Event.current.mousePosition)) && base.DragElement(targetItem, targetItemRect, firstItem);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00022614 File Offset: 0x00020A14
		public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
		{
			DragAndDropVisualMode dragAndDropVisualMode = 0;
			DragAndDropVisualMode result;
			if (targetItem is TimelineSpacerGui)
			{
				result = dragAndDropVisualMode;
			}
			else
			{
				this.m_Window.isDragging = false;
				if (!perform && parentItem == null && targetItem == null)
				{
					if (this.m_Window.state.currentDirector == null)
					{
						if (DragAndDrop.objectReferences.Any((Object x) => x is GameObject))
						{
							return 32;
						}
					}
				}
				TimelineDragging.TimelineDragData timelineDragData = DragAndDrop.GetGenericData("SequencerDragging") as TimelineDragging.TimelineDragData;
				if (timelineDragData != null)
				{
					dragAndDropVisualMode = this.HandleTrackDrop(parentItem, targetItem, perform, dropPos);
				}
				else if (DragAndDrop.objectReferences.Any<Object>() || DragAndDrop.paths.Any<string>())
				{
					dragAndDropVisualMode = this.HandleGameObjectDrop(parentItem, targetItem, perform, dropPos);
					if (dragAndDropVisualMode == null)
					{
						dragAndDropVisualMode = this.HandleAudioMixerGroupDrop(parentItem, targetItem, perform, dropPos);
					}
					if (dragAndDropVisualMode == null)
					{
						dragAndDropVisualMode = this.HandleObjectDrop(parentItem, targetItem, perform, dropPos);
					}
				}
				this.m_Window.isDragging = false;
				if (dragAndDropVisualMode == 1 && targetItem != null)
				{
					TimelineGroupGUI timelineGroupGUI = targetItem as TimelineGroupGUI;
					if (timelineGroupGUI != null)
					{
						timelineGroupGUI.isDropTarget = true;
					}
				}
				result = dragAndDropVisualMode;
			}
			return result;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00022758 File Offset: 0x00020B58
		public DragAndDropVisualMode HandleAudioMixerGroupDrop(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
		{
			DragAndDropVisualMode result;
			if (parentItem != null || targetItem != null)
			{
				result = 0;
			}
			else
			{
				IEnumerable<AudioMixerGroup> enumerable = DragAndDrop.objectReferences.OfType<AudioMixerGroup>();
				if (!enumerable.Any<AudioMixerGroup>())
				{
					result = 0;
				}
				else if (this.m_Window.state.currentDirector == null)
				{
					result = 32;
				}
				else
				{
					if (perform)
					{
						foreach (AudioMixerGroup audioMixerGroup in enumerable)
						{
							TrackAsset trackAsset = this.m_Window.AddTrack(typeof(AudioTrack), null);
							this.m_Window.state.currentDirector.SetGenericBinding(trackAsset, audioMixerGroup);
						}
						this.m_Window.state.Refresh();
					}
					result = 1;
				}
			}
			return result;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0002284C File Offset: 0x00020C4C
		public DragAndDropVisualMode HandleGameObjectDrop(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
		{
			DragAndDropVisualMode result;
			if (parentItem != null || targetItem != null)
			{
				result = 0;
			}
			else if (!DragAndDrop.objectReferences.Any((Object x) => x is GameObject))
			{
				result = 0;
			}
			else if (this.m_Window.state.currentDirector == null)
			{
				result = 32;
			}
			else
			{
				if (perform)
				{
					Object[] objectReferences = DragAndDrop.objectReferences;
					for (int i = 0; i < objectReferences.Length; i++)
					{
						Object @object = objectReferences[i];
						GameObject go = @object as GameObject;
						if (!(go == null))
						{
							PrefabType prefabType = PrefabUtility.GetPrefabType(go);
							if (prefabType != 1 && prefabType != 2)
							{
								IEnumerable<TrackType> enumerable = from x in TimelineHelpers.GetMixableTypes()
								where x.requiresGameObjectBinding
								select x;
								GenericMenu genericMenu = new GenericMenu();
								foreach (TrackType trackType in enumerable)
								{
									genericMenu.AddItem(new GUIContent(TimelineHelpers.GetTrackMenuName(trackType)), false, delegate(object e)
									{
										TrackType trackType2 = e as TrackType;
										TrackAsset trackAsset = this.m_Window.AddTrack(trackType2, null);
										if (trackAsset.GetType() == typeof(ActivationTrack))
										{
											TimelineClip timelineClip = trackAsset.CreateClip(0.0);
											timelineClip.displayName = ActivationTrackDrawer.Styles.ClipText.text;
										}
										TimelineUtility.SetSceneGameObject(this.m_Window.state.currentDirector, trackAsset, go);
									}, trackType);
								}
								genericMenu.ShowAsContext();
							}
							this.m_Window.state.Refresh();
						}
					}
				}
				result = 1;
			}
			return result;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00022A04 File Offset: 0x00020E04
		private bool ValidateObjectDrop(TrackType trackType, Object obj)
		{
			AnimationClip animationClip = obj as AnimationClip;
			return !(animationClip != null) || !animationClip.legacy;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00022A40 File Offset: 0x00020E40
		public DragAndDropVisualMode HandleObjectDrop(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
		{
			DragAndDropVisualMode result = 0;
			if (!perform)
			{
				List<Object> list = new List<Object>();
				if (DragAndDrop.objectReferences.Any<Object>())
				{
					list.AddRange(DragAndDrop.objectReferences);
					using (List<Object>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Object o = enumerator.Current;
							TimelineDragging $this = this;
							TrackType[] trackTypeHandle = TimelineHelpers.GetTrackTypeHandle(o.GetType());
							if (!trackTypeHandle.Any<TrackType>() || trackTypeHandle.Any((TrackType t) => !$this.ValidateObjectDrop(t, o)))
							{
								return 0;
							}
						}
					}
					return 1;
				}
				if (DragAndDrop.paths.Any<string>())
				{
					TimelineGroupGUI timelineGroupGUI = targetItem as TimelineGroupGUI;
					if (timelineGroupGUI == null)
					{
						return 1;
					}
					if (timelineGroupGUI.track == null)
					{
						return 0;
					}
					TrackType trackType = TimelineHelpers.TrackTypeFromType(timelineGroupGUI.track.GetType());
					foreach (string text in DragAndDrop.paths)
					{
						list.AddRange(AssetDatabase.LoadAllAssetsAtPath(text));
					}
					bool flag = false;
					foreach (Object @object in list)
					{
						if (!(@object == null))
						{
							if (TimelineHelpers.IsTypeSupportedByTrack(trackType, @object.GetType()))
							{
								flag = true;
								break;
							}
						}
					}
					return (!flag) ? 0 : 1;
				}
			}
			if (perform)
			{
				List<Object> list2 = new List<Object>();
				list2.AddRange(DragAndDrop.objectReferences);
				TrackAsset trackAsset = null;
				if (targetItem != null && (targetItem as TimelineGroupGUI).track != null)
				{
					trackAsset = (targetItem as TimelineGroupGUI).track;
				}
				Vector2 mousePosition = Event.current.mousePosition;
				foreach (Object object2 in list2)
				{
					if (!(object2 == null))
					{
						if (object2 is TimelineAsset)
						{
							if (TimelineHelpers.IsCircularRef(this.m_Timeline, object2 as TimelineAsset))
							{
								Debug.LogError("Cannot add " + object2.name + " to the sequence because it would cause a circular reference");
								continue;
							}
							TimelineAsset timelineAsset = object2 as TimelineAsset;
							for (int j = 0; j < timelineAsset.tracks.Count; j++)
							{
								if (timelineAsset.tracks[j].mediaType == 5)
								{
									this.m_Timeline.AddTrack(timelineAsset.tracks[j]);
								}
							}
							this.m_Window.state.Refresh();
							EditorUtility.SetDirty(this.m_Timeline);
						}
						else if (typeof(TrackAsset).IsAssignableFrom(object2.GetType()))
						{
							this.m_Timeline.AddTrack(object2 as TrackAsset);
						}
						else
						{
							TrackType trackType2 = null;
							TrackAsset trackAsset2 = null;
							TrackType[] trackTypeHandle2 = TimelineHelpers.GetTrackTypeHandle(object2.GetType());
							if (trackAsset != null)
							{
								foreach (TrackType trackType3 in trackTypeHandle2)
								{
									if (trackAsset.GetType() == trackType3.m_TrackType)
									{
										trackType2 = trackType3;
										break;
									}
								}
							}
							if (trackType2 == null)
							{
								if (trackTypeHandle2.Count<TrackType>() == 1)
								{
									trackType2 = trackTypeHandle2[0];
								}
								else if (trackTypeHandle2.Count<TrackType>() > 1)
								{
									trackType2 = this.ResolveTypeAmbiguity(trackTypeHandle2);
								}
							}
							if (trackType2 == null)
							{
								continue;
							}
							if (!this.ValidateObjectDrop(trackType2, object2))
							{
								continue;
							}
							if (trackAsset != null && trackAsset.GetType() == trackType2.m_TrackType)
							{
								trackAsset2 = trackAsset;
							}
							if (trackAsset2 == null)
							{
								trackAsset2 = this.m_Window.AddTrack(trackType2);
							}
							if (trackAsset2 == null)
							{
								return 0;
							}
							result = 1;
							Object object3 = this.TransformObjectBeingDroppedAccordingToTrackRules(trackAsset2, object2);
							if (object3 == null)
							{
								continue;
							}
							TimelineHelpers.PushUndo(trackAsset2, "create.clip");
							AnimationTrack animationTrack = trackAsset2 as AnimationTrack;
							if (animationTrack != null)
							{
								animationTrack.ConvertToClipMode();
							}
							TimelineClip timelineClip = TimelineHelpers.CreateClipOnTrack(object3, trackAsset2, this.m_Window.state, mousePosition);
							if (timelineClip != null)
							{
								float num = this.m_Window.state.TimeToPixel(1.0) - this.m_Window.state.TimeToPixel(0.0);
								mousePosition.x += (float)timelineClip.duration * num;
								if (timelineClip.asset is ScriptableObject)
								{
									string assetPath = AssetDatabase.GetAssetPath(timelineClip.asset);
									if (assetPath.Length == 0)
									{
										TimelineHelpers.SaveAssetIntoObject(timelineClip.asset, trackAsset2);
									}
								}
								TimelineDragging.FrameClips(this.m_Window.state);
								trackAsset2.collapsed = false;
							}
						}
						this.m_Window.state.Refresh();
						EditorUtility.SetDirty(this.m_Timeline);
					}
				}
			}
			return result;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0002304C File Offset: 0x0002144C
		private Object TransformObjectBeingDroppedAccordingToTrackRules(TrackAsset trackToReceiveClip, Object obj)
		{
			if (trackToReceiveClip is PlayableTrack && obj is MonoScript)
			{
				MonoScript monoScript = obj as MonoScript;
				if (!typeof(IPlayableAsset).IsAssignableFrom(monoScript.GetClass()) || !typeof(Object).IsAssignableFrom(monoScript.GetClass()))
				{
					Debug.LogError("The MonoScript " + monoScript.name + " is not a valid PlayableAsset");
					return null;
				}
				int num = InternalEditorUtility.CreateScriptableObjectUnchecked(monoScript);
				AssetDatabase.AddInstanceIDToAssetWithRandomFileId(num, trackToReceiveClip, true);
				obj = EditorUtility.InstanceIDToObject(num);
				if (obj == null)
				{
					Debug.LogError("Unable to create PlayableAsset from MonoScript " + monoScript.name);
				}
			}
			return obj;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00023114 File Offset: 0x00021514
		public DragAndDropVisualMode HandleTrackDrop(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
		{
			((SequenceTreeView)this.m_Window.treeView.gui).showInsertionMarker = false;
			TimelineDragging.TimelineDragData timelineDragData = DragAndDrop.GetGenericData("SequencerDragging") as TimelineDragging.TimelineDragData;
			DragAndDropVisualMode result;
			if (!this.ValidDrag(targetItem, timelineDragData.m_DraggedItems))
			{
				result = 0;
			}
			else
			{
				TimelineGroupGUI timelineGroupGUI = targetItem as TimelineGroupGUI;
				TimelineGroupGUI timelineGroupGUI2 = parentItem as TimelineGroupGUI;
				if (timelineGroupGUI != null && timelineGroupGUI.track != null && timelineGroupGUI.track.mediaType != 5)
				{
					((SequenceTreeView)this.m_Window.treeView.gui).showInsertionMarker = true;
				}
				TimelineGroupGUI timelineGroupGUI3 = targetItem as TimelineGroupGUI;
				if (timelineGroupGUI3 != null)
				{
					timelineGroupGUI3.isDropTarget = true;
				}
				if (perform)
				{
					List<TrackAsset> draggedActors = (from x in timelineDragData.m_DraggedItems
					where x is TimelineGroupGUI
					select ((TimelineGroupGUI)x).track).ToList<TrackAsset>();
					if (draggedActors.Count == 0)
					{
						return 0;
					}
					PlayableAsset playableAsset = this.m_Timeline;
					if (timelineGroupGUI2 != null && timelineGroupGUI2.track != null)
					{
						playableAsset = timelineGroupGUI2.track;
					}
					TrackAsset trackAsset = (timelineGroupGUI == null) ? null : timelineGroupGUI.track;
					if (playableAsset == this.m_Timeline && dropPos == 1 && trackAsset == null)
					{
						trackAsset = this.m_Timeline.tracks.LastOrDefault((TrackAsset x) => !draggedActors.Contains(x));
					}
					if (TrackExtensions.ReparentTracks(draggedActors.ToArray(), playableAsset, trackAsset, dropPos == 2))
					{
						this.m_Window.state.Refresh(true);
					}
				}
				result = 16;
			}
			return result;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00023314 File Offset: 0x00021714
		private static void FrameClips(TimelineWindow.TimelineState state)
		{
			int num = 0;
			float num2 = float.MaxValue;
			float num3 = float.MinValue;
			foreach (TrackAsset trackAsset in state.timeline.tracks)
			{
				num += trackAsset.clips.Length;
				if (num > 1)
				{
					return;
				}
				foreach (TimelineClip timelineClip in trackAsset.clips)
				{
					num2 = Mathf.Min(num2, (float)timelineClip.start);
					num3 = Mathf.Max(num3, (float)timelineClip.start + (float)timelineClip.duration);
				}
			}
			if (num == 1)
			{
				float num4 = num3 - num2;
				if (num4 > 0f)
				{
					state.SetTimeAreaShownRange(Mathf.Max(0f, num2 - num4), num3 + num4);
				}
				else
				{
					state.SetTimeAreaShownRange(0f, 100f);
				}
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00023438 File Offset: 0x00021838
		private bool ValidDrag(TreeViewItem target, List<TreeViewItem> draggedItems)
		{
			for (TreeViewItem treeViewItem = target; treeViewItem != null; treeViewItem = treeViewItem.parent)
			{
				if (draggedItems.Contains(treeViewItem))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00023478 File Offset: 0x00021878
		private List<TreeViewItem> GetItemsFromIDs(IEnumerable<int> draggedItemIDs)
		{
			return TreeViewUtility.FindItemsInList(draggedItemIDs, this.m_TreeView.data.GetRows());
		}

		// Token: 0x040002DA RID: 730
		private const string kGenericDragId = "SequencerDragging";

		// Token: 0x040002DB RID: 731
		private readonly int kDragSensitivity = 2;

		// Token: 0x040002DC RID: 732
		private readonly TimelineAsset m_Timeline;

		// Token: 0x040002DD RID: 733
		private readonly TimelineWindow m_Window;

		// Token: 0x02000088 RID: 136
		private class TimelineDragData
		{
			// Token: 0x06000477 RID: 1143 RVA: 0x00023540 File Offset: 0x00021940
			public TimelineDragData(List<TreeViewItem> draggedItems)
			{
				this.m_DraggedItems = draggedItems;
			}

			// Token: 0x040002E3 RID: 739
			public readonly List<TreeViewItem> m_DraggedItems;
		}
	}
}

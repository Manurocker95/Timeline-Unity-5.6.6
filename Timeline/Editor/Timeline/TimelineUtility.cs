using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000012 RID: 18
	internal static class TimelineUtility
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00009234 File Offset: 0x00007634
		public static void SaveSequence(TimelineAsset timeline, string path)
		{
			string assetPath = AssetDatabase.GetAssetPath(timeline);
			if (assetPath.Length == 0)
			{
				AssetDatabase.CreateAsset(timeline, AssetDatabase.GenerateUniqueAssetPath(path + "/" + timeline.name + ".playable"));
			}
			foreach (TrackAsset trackAsset in timeline.tracks)
			{
				string assetPath2 = AssetDatabase.GetAssetPath(trackAsset);
				if (assetPath2.Length == 0)
				{
					TimelineHelpers.SaveAssetIntoObject(trackAsset, timeline);
					foreach (TimelineClip timelineClip in trackAsset.clips)
					{
						string assetPath3 = AssetDatabase.GetAssetPath(timelineClip.asset);
						if (assetPath3.Length == 0)
						{
							TimelineHelpers.SaveAssetIntoObject(timelineClip.asset, trackAsset);
						}
						if (timelineClip.curves != null)
						{
							string assetPath4 = AssetDatabase.GetAssetPath(timelineClip.curves);
							if (assetPath4.Length == 0)
							{
								TimelineHelpers.SaveAssetIntoObject(timelineClip.curves, trackAsset);
							}
						}
					}
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000936C File Offset: 0x0000776C
		public static void SaveSequence(TimelineAsset timeline)
		{
			string text = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (text == "")
			{
				text = "Assets";
			}
			else if (Path.GetExtension(text) != "")
			{
				text = text.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}
			TimelineUtility.SaveSequence(timeline, text);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000093DC File Offset: 0x000077DC
		public static List<TrackAsset> ReorderTracks(List<TrackAsset> allTracks, List<TrackAsset> tracks, TrackAsset insertAfterAsset, bool up)
		{
			foreach (TrackAsset item in tracks)
			{
				allTracks.Remove(item);
			}
			int num = allTracks.IndexOf(insertAfterAsset);
			if (!up)
			{
				num++;
			}
			if (up)
			{
				num = Math.Max(num, 0);
			}
			List<TrackAsset> result;
			if (num < 0)
			{
				Debug.LogError("target track not found");
				result = allTracks;
			}
			else
			{
				foreach (TrackAsset item2 in tracks)
				{
					if (!up)
					{
						num = Math.Min(num, allTracks.Count);
					}
					if (up)
					{
						num = Math.Max(num, 0);
					}
					allTracks.Insert(num, item2);
				}
				result = allTracks;
			}
			return result;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000094EC File Offset: 0x000078EC
		public static TrackAsset GetSceneReferenceTrack(TrackAsset asset)
		{
			TrackAsset result;
			if (asset == null)
			{
				result = null;
			}
			else if (asset.isSubTrack)
			{
				result = TimelineUtility.GetSceneReferenceTrack(asset.parent as TrackAsset);
			}
			else
			{
				result = asset;
			}
			return result;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00009538 File Offset: 0x00007938
		public static bool TrackHasAnimationCurves(TrackAsset track)
		{
			for (int i = 0; i < track.clips.Length; i++)
			{
				AnimationClip animationClip = track.clips[i].curves;
				AnimationClip animationClip2 = track.clips[i].animationClip;
				if (animationClip != null && animationClip.empty)
				{
					animationClip = null;
				}
				if (animationClip2 != null && animationClip2.empty)
				{
					animationClip2 = null;
				}
				if (animationClip2 != null && (animationClip2.hideFlags & 8) != null)
				{
					animationClip2 = null;
				}
				if (!track.clips[i].recordable)
				{
					animationClip2 = null;
				}
				if (animationClip != null || animationClip2 != null)
				{
					return true;
				}
			}
			return track.animClip != null;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00009620 File Offset: 0x00007A20
		public static GameObject GetSceneGameObject(PlayableDirector director, TrackAsset asset)
		{
			GameObject result;
			if (director == null || asset == null)
			{
				result = null;
			}
			else
			{
				asset = TimelineUtility.GetSceneReferenceTrack(asset);
				GameObject gameObject = director.GetGenericBinding(asset) as GameObject;
				Component component = director.GetGenericBinding(asset) as Component;
				if (component != null)
				{
					gameObject = component.gameObject;
				}
				result = gameObject;
			}
			return result;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000968C File Offset: 0x00007A8C
		public static void SetSceneGameObject(PlayableDirector director, TrackAsset asset, GameObject go)
		{
			if (!(director == null) && !(asset == null))
			{
				asset = TimelineUtility.GetSceneReferenceTrack(asset);
				PlayableBinding[] outputs = asset.outputs;
				if (outputs.Length != 0)
				{
					if (outputs[0].streamType == null || outputs[0].sourceBindingType == typeof(GameObject))
					{
						TimelineHelpers.AddRequiredComponent(go, asset);
						TimelineUtility.SetBindingInDirector(director, asset, go);
					}
					else
					{
						TimelineUtility.SetBindingInDirector(director, asset, TimelineHelpers.AddRequiredComponent(go, asset));
					}
				}
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009726 File Offset: 0x00007B26
		public static void SetBindingInDirector(PlayableDirector director, Object bindTo, Object objectToBind)
		{
			if (!(director == null) && !(bindTo == null))
			{
				director.SetGenericBinding(bindTo, objectToBind);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009750 File Offset: 0x00007B50
		public static PlayableDirector[] GetDirectorsInSceneUsingAsset(PlayableAsset asset)
		{
			List<PlayableDirector> list = new List<PlayableDirector>();
			PlayableDirector[] array = Resources.FindObjectsOfTypeAll(typeof(PlayableDirector)) as PlayableDirector[];
			foreach (PlayableDirector playableDirector in array)
			{
				if (playableDirector.hideFlags != 8 && playableDirector.hideFlags != 61)
				{
					string assetPath = AssetDatabase.GetAssetPath(playableDirector.transform.root.gameObject);
					if (string.IsNullOrEmpty(assetPath))
					{
						if (asset == null || (asset != null && playableDirector.playableAsset == asset))
						{
							list.Add(playableDirector);
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009824 File Offset: 0x00007C24
		public static PlayableDirector GetDirectorComponentForGameObject(GameObject gameObject)
		{
			return (!(gameObject != null)) ? null : gameObject.GetComponent<PlayableDirector>();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009854 File Offset: 0x00007C54
		public static TimelineAsset GetTimelineAssetForDirectorComponent(PlayableDirector director)
		{
			return (!(director != null)) ? null : (director.playableAsset as TimelineAsset);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009888 File Offset: 0x00007C88
		public static bool IsPrefabOrAsset(Object obj)
		{
			return EditorUtility.IsPersistent(obj) || (obj.hideFlags & 8) != 0;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000098BC File Offset: 0x00007CBC
		internal static ScriptableObject CreateAsset(Type t, string assetName)
		{
			string text = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (text == "")
			{
				text = "Assets";
			}
			else if (Path.GetExtension(text) != "")
			{
				text = text.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}
			ScriptableObject scriptableObject = ScriptableObject.CreateInstance(t);
			AssetDatabase.CreateAsset(scriptableObject, AssetDatabase.GenerateUniqueAssetPath(text + "/" + assetName));
			Selection.activeObject = scriptableObject;
			return scriptableObject;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00009950 File Offset: 0x00007D50
		internal static T CreateAsset<T>(string assetName) where T : ScriptableObject
		{
			return TimelineUtility.CreateAsset(typeof(T), assetName) as T;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00009980 File Offset: 0x00007D80
		internal static string PropertyToString(SerializedProperty property)
		{
			SerializedPropertyType propertyType = property.propertyType;
			string result;
			switch (propertyType + 1)
			{
			case 0:
				result = string.Empty;
				break;
			case 1:
				result = property.intValue.ToString();
				break;
			case 2:
				result = ((!property.boolValue) ? "0" : "1");
				break;
			case 3:
				result = property.floatValue.ToString();
				break;
			case 4:
				result = property.stringValue;
				break;
			case 5:
				result = property.colorValue.ToString();
				break;
			case 6:
				result = string.Empty;
				break;
			case 7:
				result = property.intValue.ToString();
				break;
			case 8:
				result = property.intValue.ToString();
				break;
			case 9:
				result = property.vector2Value.ToString();
				break;
			case 10:
				result = property.vector3Value.ToString();
				break;
			case 11:
				result = property.vector4Value.ToString();
				break;
			case 12:
				result = property.rectValue.ToString();
				break;
			case 13:
				result = property.intValue.ToString();
				break;
			case 14:
				result = property.intValue.ToString();
				break;
			case 15:
				result = property.animationCurveValue.ToString();
				break;
			case 16:
				result = property.boundsValue.ToString();
				break;
			case 17:
				result = property.gradientValue.ToString();
				break;
			case 18:
				result = property.quaternionValue.ToString();
				break;
			default:
				Debug.LogWarning("Unknown Property Type: " + property.propertyType);
				result = string.Empty;
				break;
			}
			return result;
		}
	}
}

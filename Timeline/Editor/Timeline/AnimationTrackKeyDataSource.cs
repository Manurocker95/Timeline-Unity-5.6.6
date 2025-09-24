using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000093 RID: 147
	internal class AnimationTrackKeyDataSource : IPropertyKeyDataSource
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x00028D95 File Offset: 0x00027195
		public AnimationTrackKeyDataSource(AnimationTrack track)
		{
			this.m_Track = track;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00028DA8 File Offset: 0x000271A8
		public float[] GetKeys()
		{
			float[] result;
			if (this.m_Track == null || this.m_Track.animClip == null)
			{
				result = null;
			}
			else
			{
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(this.m_Track.animClip);
				result = (from x in curveInfo.keyTimes
				select x + (float)this.m_Track.openClipTimeOffset).ToArray<float>();
			}
			return result;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00028E20 File Offset: 0x00027220
		public Dictionary<float, string> GetDescriptions()
		{
			Dictionary<float, string> dictionary = new Dictionary<float, string>();
			AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(this.m_Track.animClip);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (EditorCurveBinding binding in curveInfo.bindings)
			{
				string groupID = binding.GetGroupID();
				if (!hashSet.Contains(groupID))
				{
					CurveBindingGroup groupBinding = curveInfo.GetGroupBinding(groupID);
					string nicePropertyGroupDisplayName = AnimationWindowUtility.GetNicePropertyGroupDisplayName(binding.type, binding.propertyName);
					foreach (float num in curveInfo.keyTimes)
					{
						float num2 = num + (float)this.m_Track.openClipTimeOffset;
						string text = nicePropertyGroupDisplayName + " : " + groupBinding.GetDescription(num2);
						if (dictionary.ContainsKey(num2))
						{
							Dictionary<float, string> dictionary2;
							float key;
							(dictionary2 = dictionary)[key = num2] = dictionary2[key] + '\n' + text;
						}
						else
						{
							dictionary.Add(num2, text);
						}
					}
					hashSet.Add(groupID);
				}
			}
			return dictionary;
		}

		// Token: 0x04000374 RID: 884
		private readonly AnimationTrack m_Track;
	}
}

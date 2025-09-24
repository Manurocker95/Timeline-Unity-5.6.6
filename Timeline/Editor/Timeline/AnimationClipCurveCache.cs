using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000021 RID: 33
	internal class AnimationClipCurveCache
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000E78C File Offset: 0x0000CB8C
		public static AnimationClipCurveCache Instance
		{
			get
			{
				if (AnimationClipCurveCache.s_Instance == null)
				{
					AnimationClipCurveCache.s_Instance = new AnimationClipCurveCache();
				}
				return AnimationClipCurveCache.s_Instance;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000E7BC File Offset: 0x0000CBBC
		public void OnEnable()
		{
			if (!this.m_IsEnabled)
			{
				AnimationUtility.onCurveWasModified = (AnimationUtility.OnCurveWasModified)Delegate.Combine(AnimationUtility.onCurveWasModified, new AnimationUtility.OnCurveWasModified(this.OnCurveWasModified));
				this.m_IsEnabled = true;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000E7F3 File Offset: 0x0000CBF3
		public void OnDisable()
		{
			if (this.m_IsEnabled)
			{
				AnimationUtility.onCurveWasModified = (AnimationUtility.OnCurveWasModified)Delegate.Remove(AnimationUtility.onCurveWasModified, new AnimationUtility.OnCurveWasModified(this.OnCurveWasModified));
				this.m_IsEnabled = false;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000E82C File Offset: 0x0000CC2C
		private void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType modification)
		{
			AnimationClipCurveInfo animationClipCurveInfo;
			if (modification == null)
			{
				this.m_ClipCache.Remove(clip);
			}
			else if (this.m_ClipCache.TryGetValue(clip, out animationClipCurveInfo))
			{
				animationClipCurveInfo.dirty = true;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000E874 File Offset: 0x0000CC74
		public AnimationClipCurveInfo GetCurveInfo(AnimationClip clip)
		{
			AnimationClipCurveInfo result;
			if (clip == null)
			{
				result = null;
			}
			else
			{
				AnimationClipCurveInfo animationClipCurveInfo;
				if (!this.m_ClipCache.TryGetValue(clip, out animationClipCurveInfo))
				{
					animationClipCurveInfo = new AnimationClipCurveInfo();
					animationClipCurveInfo.dirty = true;
					this.m_ClipCache[clip] = animationClipCurveInfo;
				}
				if (animationClipCurveInfo.dirty)
				{
					animationClipCurveInfo.Update(clip);
				}
				result = animationClipCurveInfo;
			}
			return result;
		}

		// Token: 0x0400015A RID: 346
		private static AnimationClipCurveCache s_Instance;

		// Token: 0x0400015B RID: 347
		private Dictionary<AnimationClip, AnimationClipCurveInfo> m_ClipCache = new Dictionary<AnimationClip, AnimationClipCurveInfo>();

		// Token: 0x0400015C RID: 348
		private bool m_IsEnabled = false;
	}
}

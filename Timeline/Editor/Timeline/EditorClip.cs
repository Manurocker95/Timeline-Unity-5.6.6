using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000041 RID: 65
	internal class EditorClip : ScriptableObject
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000147B9 File Offset: 0x00012BB9
		private EditorClip()
		{
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000147CC File Offset: 0x00012BCC
		public double duration
		{
			get
			{
				return this.m_Clip.duration;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000239 RID: 569 RVA: 0x000147EC File Offset: 0x00012BEC
		public TrackAsset track
		{
			get
			{
				return this.m_Clip.parentTrack;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0001480C File Offset: 0x00012C0C
		public AnimationClip animClip
		{
			get
			{
				return this.m_Clip.curves;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0001482C File Offset: 0x00012C2C
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00014847 File Offset: 0x00012C47
		public Object asset
		{
			get
			{
				return this.m_Instance;
			}
			set
			{
				this.m_Instance = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00014854 File Offset: 0x00012C54
		public List<string> exposedParameters
		{
			get
			{
				return this.m_Clip.exposedParameters;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00014874 File Offset: 0x00012C74
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0001488F File Offset: 0x00012C8F
		public TimelineAsset timeline
		{
			get
			{
				return this.m_Timeline;
			}
			set
			{
				this.m_Timeline = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0001489C File Offset: 0x00012C9C
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000148B7 File Offset: 0x00012CB7
		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
			set
			{
				this.m_Clip = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000148C4 File Offset: 0x00012CC4
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000148DF File Offset: 0x00012CDF
		public PlayableDirector director
		{
			get
			{
				return this.m_Director;
			}
			set
			{
				this.m_Director = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000148EC File Offset: 0x00012CEC
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00014907 File Offset: 0x00012D07
		public int lastHash
		{
			get
			{
				return this.m_Hash;
			}
			set
			{
				this.m_Hash = value;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00014914 File Offset: 0x00012D14
		public static EditorClip CreateEditorClip(TimelineAsset timeline, TimelineClip clip)
		{
			if (clip == null)
			{
				throw new NullReferenceException("parameter clip cannot be null");
			}
			EditorClip editorClip = ScriptableObject.CreateInstance<EditorClip>();
			editorClip.timeline = timeline;
			editorClip.clip = clip;
			return editorClip;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00014950 File Offset: 0x00012D50
		public override int GetHashCode()
		{
			return this.m_Clip.Hash();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00014970 File Offset: 0x00012D70
		public void SetDirty()
		{
			EditorUtility.SetDirty(this.m_Timeline);
			EditorUtility.SetDirty(this);
			TimelineWindow windowDontShow = EditorWindow.GetWindowDontShow<TimelineWindow>();
			if (windowDontShow != null)
			{
				windowDontShow.Repaint();
			}
		}

		// Token: 0x040001CA RID: 458
		private TimelineAsset m_Timeline;

		// Token: 0x040001CB RID: 459
		[SerializeField]
		private TimelineClip m_Clip;

		// Token: 0x040001CC RID: 460
		private Object m_Instance;

		// Token: 0x040001CD RID: 461
		private PlayableDirector m_Director;

		// Token: 0x040001CE RID: 462
		private int m_Hash = -1;
	}
}

using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    // Token: 0x02000024 RID: 36
    [NotKeyable]
    [Serializable]
    internal class AnimationPlayableAsset : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        // Token: 0x1700006A RID: 106
        // (get) Token: 0x0600012A RID: 298 RVA: 0x00006408 File Offset: 0x00004608
        // (set) Token: 0x0600012B RID: 299 RVA: 0x00006423 File Offset: 0x00004623
        public Vector3 position
        {
            get
            {
                return this.m_Position;
            }
            set
            {
                this.m_Position = value;
                if (this.m_AnimationOffsetPlayable != null && this.m_AnimationOffsetPlayable.IsValid())
                {
                    this.m_AnimationOffsetPlayable.position = this.position;
                }
            }
        }

        // Token: 0x1700006B RID: 107
        // (get) Token: 0x0600012C RID: 300 RVA: 0x0000645C File Offset: 0x0000465C
        // (set) Token: 0x0600012D RID: 301 RVA: 0x00006477 File Offset: 0x00004677
        public Quaternion rotation
        {
            get
            {
                return this.m_Rotation;
            }
            set
            {
                this.m_Rotation = value;
                if (this.m_AnimationOffsetPlayable != null && this.m_AnimationOffsetPlayable.IsValid())
                {
                    this.m_AnimationOffsetPlayable.rotation = value;
                }
            }
        }

        // Token: 0x1700006C RID: 108
        // (get) Token: 0x0600012E RID: 302 RVA: 0x000064A8 File Offset: 0x000046A8
        // (set) Token: 0x0600012F RID: 303 RVA: 0x000064C3 File Offset: 0x000046C3
        public bool useTrackMatchFields
        {
            get
            {
                return this.m_UseTrackMatchFields;
            }
            set
            {
                this.m_UseTrackMatchFields = value;
            }
        }

        // Token: 0x1700006D RID: 109
        // (get) Token: 0x06000130 RID: 304 RVA: 0x000064D0 File Offset: 0x000046D0
        // (set) Token: 0x06000131 RID: 305 RVA: 0x000064EB File Offset: 0x000046EB
        public MatchTargetFields matchTargetFields
        {
            get
            {
                return this.m_MatchTargetFields;
            }
            set
            {
                this.m_MatchTargetFields = value;
            }
        }

        // Token: 0x1700006E RID: 110
        // (get) Token: 0x06000132 RID: 306 RVA: 0x000064F8 File Offset: 0x000046F8
        private bool removeStartOffset
        {
            get
            {
                return this.m_Clip != null &&
                       (this.m_Clip.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
            }
        }

        // Token: 0x1700006F RID: 111
        // (get) Token: 0x06000133 RID: 307 RVA: 0x00006534 File Offset: 0x00004734
        // (set) Token: 0x06000134 RID: 308 RVA: 0x0000654F File Offset: 0x0000474F
        public AnimationClip clip
        {
            get
            {
                return this.m_Clip;
            }
            set
            {
                if (value != null)
                {
                    base.name = "AnimationPlayableAsset of " + value.name;
                }
                this.m_Clip = value;
            }
        }

        // Token: 0x17000070 RID: 112
        // (get) Token: 0x06000135 RID: 309 RVA: 0x0000657C File Offset: 0x0000477C
        public override double duration
        {
            get
            {
                double result;
                if (this.clip == null)
                {
                    result = double.MaxValue;
                }
                else
                {
                    double num = (double)this.clip.length;
                    if (this.clip.frameRate > 0f)
                    {
                        double num2 = (double)Mathf.Round(this.clip.length * this.clip.frameRate);
                        num = num2 / (double)this.clip.frameRate;
                    }
                    result = num;
                }
                return result;
            }
        }

        // Token: 0x17000071 RID: 113
        // (get) Token: 0x06000136 RID: 310 RVA: 0x00006604 File Offset: 0x00004804
        public override PlayableBinding[] outputs
        {
            get
            {
                if (this.m_Outputs == null)
                {
                    PlayableBinding playableBinding = default(PlayableBinding);
                    playableBinding.streamType = 0;
                    this.m_Outputs = new PlayableBinding[]
                    {
                        playableBinding
                    };
                }
                return this.m_Outputs;
            }
        }

        // Token: 0x06000137 RID: 311 RVA: 0x00006658 File Offset: 0x00004858
        public override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
        {
            PlayableHandle playableHandle = AnimationPlayableGraphExtensions.CreateAnimationClipPlayable(graph, this.m_Clip);
            this.m_AnimationClipPlayable = playableHandle.GetObject<AnimationClipPlayable>();
            this.m_AnimationClipPlayable.removeStartOffset = this.removeStartOffset;
            PlayableHandle result = playableHandle;
            if (this.applyRootMotion)
            {
                PlayableHandle playableHandle2 = AnimationPlayableGraphExtensions.CreateAnimationOffsetPlayable(graph, this.m_Position, this.m_Rotation, 1);
                this.m_AnimationOffsetPlayable = playableHandle2.GetObject<AnimationOffsetPlayable>();
                graph.Connect(playableHandle, 0, playableHandle2, 0);
                result = playableHandle2;
            }
            this.LiveLink();
            return result;
        }

        // Token: 0x17000072 RID: 114
        // (get) Token: 0x06000138 RID: 312 RVA: 0x000066E0 File Offset: 0x000048E0
        private bool applyRootMotion
        {
            get
            {
                return this.m_Position != Vector3.zero ||
                       this.m_Rotation != Quaternion.identity ||
                       HasRootMotion(this.m_Clip);
            }
        }

        private static bool HasRootMotion(AnimationClip clip)
        {
            if (clip == null)
                return false;

#if UNITY_EDITOR
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

            for (int i = 0; i < bindings.Length; i++)
            {
                string propertyName = bindings[i].propertyName;

                if (propertyName.Contains("RootT") ||
                    propertyName.Contains("RootQ") ||
                    propertyName.Contains("MotionT") ||
                    propertyName.Contains("MotionQ"))
                {
                    return true;
                }
            }
#endif

            return false;
        }

        // Token: 0x06000139 RID: 313 RVA: 0x00006744 File Offset: 0x00004944
        public void LiveLink()
        {
            if (this.m_AnimationOffsetPlayable != null && this.m_AnimationOffsetPlayable.IsValid())
            {
                this.m_AnimationOffsetPlayable.position = this.position;
                this.m_AnimationOffsetPlayable.rotation = this.rotation;
            }
        }

        // Token: 0x17000073 RID: 115
        // (get) Token: 0x0600013A RID: 314 RVA: 0x00006794 File Offset: 0x00004994
        public ClipCaps clipCaps
        {
            get
            {
                ClipCaps clipCaps = ClipCaps.All;
                if (this.m_Clip == null || !this.m_Clip.isLooping)
                {
                    clipCaps &= ~ClipCaps.Looping;
                }
                return clipCaps;
            }
        }

        // Token: 0x0600013B RID: 315 RVA: 0x000067D3 File Offset: 0x000049D3
        public void ResetOffsets()
        {
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
        }

        // Token: 0x0600013C RID: 316 RVA: 0x000067EC File Offset: 0x000049EC
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            driver.AddFromClip(this.m_Clip);
        }

        // Token: 0x040000A4 RID: 164
        [SerializeField]
        private AnimationClip m_Clip;

        // Token: 0x040000A5 RID: 165
        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        // Token: 0x040000A6 RID: 166
        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        // Token: 0x040000A7 RID: 167
        private PlayableBinding[] m_Outputs = null;

        // Token: 0x040000A8 RID: 168
        [SerializeField]
        private bool m_UseTrackMatchFields = false;

        // Token: 0x040000A9 RID: 169
        [SerializeField]
        private MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;

        // Token: 0x040000AA RID: 170
        private AnimationClipPlayable m_AnimationClipPlayable = null;

        // Token: 0x040000AB RID: 171
        private AnimationOffsetPlayable m_AnimationOffsetPlayable = null;
    }
}

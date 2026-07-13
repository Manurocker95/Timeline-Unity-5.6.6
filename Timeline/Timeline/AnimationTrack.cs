using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    // Token: 0x02000027 RID: 39
    [TrackClipType(typeof(AnimationClip))]
    [TrackMediaType(TimelineAsset.MediaType.Animation)]
    [Serializable]
    internal class AnimationTrack : TrackAsset
    {
        // Token: 0x17000074 RID: 116
        // (get) Token: 0x06000141 RID: 321 RVA: 0x000068BC File Offset: 0x00004ABC
        // (set) Token: 0x06000142 RID: 322 RVA: 0x000068D7 File Offset: 0x00004AD7
        public Vector3 position
        {
            get
            {
                return this.m_Position;
            }
            set
            {
                this.m_Position = value;
            }
        }

        // Token: 0x17000075 RID: 117
        // (get) Token: 0x06000143 RID: 323 RVA: 0x000068E4 File Offset: 0x00004AE4
        // (set) Token: 0x06000144 RID: 324 RVA: 0x000068FF File Offset: 0x00004AFF
        public Quaternion rotation
        {
            get
            {
                return this.m_Rotation;
            }
            set
            {
                this.m_Rotation = value;
            }
        }

        // Token: 0x17000076 RID: 118
        // (get) Token: 0x06000145 RID: 325 RVA: 0x0000690C File Offset: 0x00004B0C
        // (set) Token: 0x06000146 RID: 326 RVA: 0x00006927 File Offset: 0x00004B27
        public bool applyOffsets
        {
            get
            {
                return this.m_ApplyOffsets;
            }
            set
            {
                this.m_ApplyOffsets = value;
            }
        }

        // Token: 0x17000077 RID: 119
        // (get) Token: 0x06000147 RID: 327 RVA: 0x00006934 File Offset: 0x00004B34
        // (set) Token: 0x06000148 RID: 328 RVA: 0x0000694F File Offset: 0x00004B4F
        public MatchTargetFields matchTargetFields
        {
            get
            {
                return this.m_MatchTargetFields;
            }
            set
            {
                this.m_MatchTargetFields = (value & MatchTargetFieldConstants.All);
            }
        }

        // Token: 0x06000149 RID: 329 RVA: 0x00006960 File Offset: 0x00004B60
        internal void UpdateClipOffsets()
        {
            if (this.m_ClipOffset != null && this.m_ClipOffset.IsValid())
            {
                this.m_ClipOffset.position = this.m_Position;
                this.m_ClipOffset.rotation = this.m_Rotation;
            }
        }

        // Token: 0x0600014A RID: 330 RVA: 0x000069B0 File Offset: 0x00004BB0
        internal override void OnCreateClipFromAsset(Object asset, TimelineClip clip)
        {
            AnimationClip animationClip = asset as AnimationClip;
            if (animationClip != null)
            {
                if (animationClip.legacy)
                {
                    throw new InvalidOperationException("Legacy Animation Clips are not supported");
                }
                AnimationPlayableAsset animationPlayableAsset = ScriptableObject.CreateInstance<AnimationPlayableAsset>();
                animationPlayableAsset.clip = animationClip;
                clip.displayName = animationClip.name;
                if (animationClip.frameRate > 0f)
                {
                    double num = (double)Mathf.Round(animationClip.length * animationClip.frameRate);
                    clip.duration = num / (double)animationClip.frameRate;
                }
                else
                {
                    clip.duration = (double)animationClip.length;
                }
                TimelineClip.ClipExtrapolation clipExtrapolation = TimelineClip.ClipExtrapolation.None;
                if (!base.isSubTrack)
                {
                    clipExtrapolation = TimelineClip.ClipExtrapolation.Hold;
                }
                clip.asset = animationPlayableAsset;
                clip.underlyingAsset = animationClip;
                clip.preExtrapolationMode = clipExtrapolation;
                clip.postExtrapolationMode = clipExtrapolation;
            }
        }

        // Token: 0x0600014B RID: 331 RVA: 0x00006A78 File Offset: 0x00004C78
        internal PlayableHandle CompileTrackPlayable(PlayableGraph graph, TrackAsset track, GameObject go, IntervalTree tree)
        {
            PlayableHandle playableHandle = AnimationPlayableGraphExtensions.CreateAnimationMixerPlayable(graph, track.clips.Length);
            for (int i = 0; i < track.clips.Length; i++)
            {
                TimelineClip timelineClip = track.clips[i];
                PlayableAsset playableAsset = timelineClip.asset as PlayableAsset;
                if (!(playableAsset == null))
                {
                    PlayableHandle playableHandle2 = playableAsset.CreatePlayable(graph, go);
                    tree.Add(new RuntimeClip(timelineClip, playableHandle2, playableHandle)
                    {
                        inclusiveDuration = track.HasClipInclusiveDuration(i)
                    });
                    graph.Connect(playableHandle2, 0, playableHandle, i);
                    playableHandle.SetInputWeight(i, 0f);
                }
            }
            return this.ApplyTrackOffset(graph, playableHandle);
        }

        // Token: 0x0600014C RID: 332 RVA: 0x00006B30 File Offset: 0x00004D30
        internal override PlayableHandle OnCreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree tree)
        {
            if (base.isSubTrack)
            {
                throw new InvalidOperationException("Nested animation tracks should never be asked to create a graph directly");
            }
            List<AnimationTrack> list = new List<AnimationTrack>();
            if (this.compilableIsolated)
            {
                list.Add(this);
            }
            for (int i = 0; i < base.subTracks.Count; i++)
            {
                AnimationTrack animationTrack = base.subTracks[i] as AnimationTrack;
                if (animationTrack != null && animationTrack.compilable)
                {
                    list.Add(animationTrack);
                }
            }
            PlayableHandle playableHandle = AnimationPlayableGraphExtensions.CreateAnimationMotionXToDeltaPlayable(graph);
            PlayableHandle playableHandle2 = this.CreateGroupMixer(graph, go, list.Count);
            graph.Connect(playableHandle2, 0, playableHandle, 0);
            playableHandle.SetInputWeight(0, 1f);
            for (int j = 0; j < list.Count; j++)
            {
                PlayableHandle playableHandle3 = (!list[j].inClipMode) ? list[j].CreateInfiniteTrackPlayable(graph, go, tree) : this.CompileTrackPlayable(graph, list[j], go, tree);
                tree.Add(list[j]);
                graph.Connect(playableHandle3, 0, playableHandle2, j);
                playableHandle2.SetInputWeight(j, (float)((!list[j].inClipMode) ? 1 : 0));
            }
            return playableHandle;
        }

        // Token: 0x0600014D RID: 333 RVA: 0x00006C90 File Offset: 0x00004E90
        public PlayableHandle CreateGroupMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return AnimationPlayableGraphExtensions.CreateAnimationLayerMixerPlayable(graph, inputCount);
        }

        // Token: 0x0600014E RID: 334 RVA: 0x00006CAC File Offset: 0x00004EAC
        private PlayableHandle CreateInfiniteTrackPlayable(PlayableGraph graph, GameObject go, IntervalTree tree)
        {
            PlayableHandle result;
            if (base.animClip == null)
            {
                result = PlayableHandle.Null;
            }
            else
            {
                if (this.m_FakeAnimClip == null || this.m_AnimationPlayableAsset == null)
                {
                    this.m_AnimationPlayableAsset = ScriptableObject.CreateInstance<AnimationPlayableAsset>();
                    this.m_FakeAnimClip = new TimelineClip(null)
                    {
                        asset = this.m_AnimationPlayableAsset,
                        displayName = "Animation Clip",
                        timeScale = 1.0,
                        start = 0.0,
                        postExtrapolationMode = TimelineClip.ClipExtrapolation.Hold,
                        preExtrapolationMode = TimelineClip.ClipExtrapolation.Hold
                    };
                    this.m_FakeAnimClip.SetPostExtrapolationTime(TimelineClip.kMaxTimeValue);
                }
                this.m_AnimationPlayableAsset.clip = base.animClip;
                this.m_AnimationPlayableAsset.position = this.m_OpenClipOffsetPosition;
                this.m_AnimationPlayableAsset.rotation = this.m_OpenClipOffsetRotation;
                this.m_FakeAnimClip.start = 0.0;
                this.m_FakeAnimClip.SetPreExtrapolationTime(0.0);
                this.m_FakeAnimClip.duration = (double)base.animClip.length;
                PlayableHandle playableHandle = AnimationPlayableGraphExtensions.CreateAnimationMixerPlayable(graph, 1);
                PlayableHandle playableHandle2 = this.m_AnimationPlayableAsset.CreatePlayable(graph, go);
                tree.Add(new RuntimeClip(this.m_FakeAnimClip, playableHandle2, playableHandle));
                graph.Connect(playableHandle2, 0, playableHandle, 0);
                playableHandle.SetInputWeight(0, 1f);
                result = this.ApplyTrackOffset(graph, playableHandle);
            }
            return result;
        }

        // Token: 0x0600014F RID: 335 RVA: 0x00006E24 File Offset: 0x00005024
        private PlayableHandle ApplyTrackOffset(PlayableGraph graph, PlayableHandle root)
        {
            this.m_ClipOffset = null;
            PlayableHandle result;
            if (!this.m_ApplyOffsets)
            {
                result = root;
            }
            else
            {
                PlayableHandle playableHandle = AnimationPlayableGraphExtensions.CreateAnimationOffsetPlayable(graph, this.m_Position, this.m_Rotation, 1);
                this.m_ClipOffset = playableHandle.GetObject<AnimationOffsetPlayable>();
                graph.Connect(root, 0, playableHandle, 0);
                result = playableHandle;
            }
            return result;
        }

        // Token: 0x06000150 RID: 336 RVA: 0x00006E80 File Offset: 0x00005080
        internal override void GetEvaluationTime(out double outStart, out double outDuration)
        {
            if (this.inClipMode)
            {
                base.GetEvaluationTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0.0;
                outDuration = TimelineClip.kMaxTimeValue;
            }
        }

        // Token: 0x06000151 RID: 337 RVA: 0x00006EB4 File Offset: 0x000050B4
        internal override void GetSequenceTime(out double outStart, out double outDuration)
        {
            if (this.inClipMode)
            {
                base.GetSequenceTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0.0;
                outDuration = 0.0;
                if (base.animClip != null)
                {
                    outDuration = (double)base.animClip.length;
                }
            }
        }

        // Token: 0x17000078 RID: 120
        // (get) Token: 0x06000152 RID: 338 RVA: 0x00006F14 File Offset: 0x00005114
        private bool compilableIsolated
        {
            get
            {
                return !base.muted && (this.m_Clips.Count > 0 || (base.animClip != null && base.animClip.length > 0));
            }
        }

        // Token: 0x17000079 RID: 121
        // (get) Token: 0x06000153 RID: 339 RVA: 0x00006F70 File Offset: 0x00005170
        internal override bool compilable
        {
            get
            {
                bool result;
                if (!this.compilableIsolated)
                {
                    result = base.subTracks.Any((TrackAsset x) => x.compilable);
                }
                else
                {
                    result = true;
                }
                return result;
            }
        }

        // Token: 0x1700007A RID: 122
        // (get) Token: 0x06000154 RID: 340 RVA: 0x00006FBC File Offset: 0x000051BC
        public override PlayableBinding[] outputs
        {
            get
            {
                PlayableBinding[] array = new PlayableBinding[1];
                int num = 0;
                PlayableBinding playableBinding = default(PlayableBinding);
                playableBinding.sourceObject = this;
                playableBinding.streamName = base.name;
                playableBinding.streamType = 0;
                array[num] = playableBinding;
                return array;
            }
        }

        // Token: 0x1700007B RID: 123
        // (get) Token: 0x06000155 RID: 341 RVA: 0x0000700C File Offset: 0x0000520C
        public bool inClipMode
        {
            get
            {
                return base.clips != null && base.clips.Length != 0;
            }
        }

        // Token: 0x1700007C RID: 124
        // (get) Token: 0x06000156 RID: 342 RVA: 0x00007040 File Offset: 0x00005240
        // (set) Token: 0x06000157 RID: 343 RVA: 0x0000705B File Offset: 0x0000525B
        public Vector3 openClipOffsetPosition
        {
            get
            {
                return this.m_OpenClipOffsetPosition;
            }
            set
            {
                this.m_OpenClipOffsetPosition = value;
            }
        }

        // Token: 0x1700007D RID: 125
        // (get) Token: 0x06000158 RID: 344 RVA: 0x00007068 File Offset: 0x00005268
        // (set) Token: 0x06000159 RID: 345 RVA: 0x00007083 File Offset: 0x00005283
        public Quaternion openClipOffsetRotation
        {
            get
            {
                return this.m_OpenClipOffsetRotation;
            }
            set
            {
                this.m_OpenClipOffsetRotation = value;
            }
        }

        // Token: 0x1700007E RID: 126
        // (get) Token: 0x0600015A RID: 346 RVA: 0x00007090 File Offset: 0x00005290
        // (set) Token: 0x0600015B RID: 347 RVA: 0x000070AB File Offset: 0x000052AB
        internal double openClipTimeOffset
        {
            get
            {
                return this.m_OpenClipTimeOffset;
            }
            set
            {
                this.m_OpenClipTimeOffset = value;
            }
        }

        // Token: 0x1700007F RID: 127
        // (get) Token: 0x0600015C RID: 348 RVA: 0x000070B8 File Offset: 0x000052B8
        // (set) Token: 0x0600015D RID: 349 RVA: 0x000070D3 File Offset: 0x000052D3
        public TimelineClip.ClipExtrapolation openClipPreExtrapolation
        {
            get
            {
                return this.m_OpenClipPreExtrapolation;
            }
            set
            {
                this.m_OpenClipPreExtrapolation = value;
            }
        }

        // Token: 0x17000080 RID: 128
        // (get) Token: 0x0600015E RID: 350 RVA: 0x000070E0 File Offset: 0x000052E0
        // (set) Token: 0x0600015F RID: 351 RVA: 0x000070FB File Offset: 0x000052FB
        public TimelineClip.ClipExtrapolation openClipPostExtrapolation
        {
            get
            {
                return this.m_OpenClipPostExtrapolation;
            }
            set
            {
                this.m_OpenClipPostExtrapolation = value;
            }
        }

        // Token: 0x06000160 RID: 352 RVA: 0x00007105 File Offset: 0x00005305
        [ContextMenu("Reset Offsets")]
        private void ResetOffsets()
        {
            this.m_Position = Vector3.zero;
            this.m_Rotation = Quaternion.identity;
            this.UpdateClipOffsets();
        }

        // Token: 0x040000B7 RID: 183
        protected AnimationPlayableAsset m_AnimationPlayableAsset;

        // Token: 0x040000B8 RID: 184
        protected TimelineClip m_FakeAnimClip;

        // Token: 0x040000B9 RID: 185
        [SerializeField]
        private TimelineClip.ClipExtrapolation m_OpenClipPreExtrapolation = TimelineClip.ClipExtrapolation.None;

        // Token: 0x040000BA RID: 186
        [SerializeField]
        private TimelineClip.ClipExtrapolation m_OpenClipPostExtrapolation = TimelineClip.ClipExtrapolation.None;

        // Token: 0x040000BB RID: 187
        [SerializeField]
        private Vector3 m_OpenClipOffsetPosition = Vector3.zero;

        // Token: 0x040000BC RID: 188
        [SerializeField]
        private Quaternion m_OpenClipOffsetRotation = Quaternion.identity;

        // Token: 0x040000BD RID: 189
        [SerializeField]
        private double m_OpenClipTimeOffset = 0.0;

        // Token: 0x040000BE RID: 190
        [SerializeField]
        private MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;

        // Token: 0x040000BF RID: 191
        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        // Token: 0x040000C0 RID: 192
        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        // Token: 0x040000C1 RID: 193
        [SerializeField]
        private bool m_ApplyOffsets;

        // Token: 0x040000C2 RID: 194
        private AnimationOffsetPlayable m_ClipOffset;
    }
}

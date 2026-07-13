using System;

namespace UnityEngine.Playables
{
    /// <summary>
    ///   <para>This structure contains the frame information a Playable receives in Playable.PrepareFrame.</para>
    /// </summary>
    // Token: 0x020004B1 RID: 1201
    public struct FrameData
    {
        /// <summary>
        ///   <para>The current frame identifier.</para>
        /// </summary>
        // Token: 0x17000CED RID: 3309
        // (get) Token: 0x060038E6 RID: 14566 RVA: 0x00058B3C File Offset: 0x00056D3C
        public ulong frameId
        {
            get
            {
                return this.m_FrameID;
            }
        }

        /// <summary>
        ///   <para>Time difference between this frame and the preceding frame.</para>
        /// </summary>
        // Token: 0x17000CEE RID: 3310
        // (get) Token: 0x060038E7 RID: 14567 RVA: 0x00058B58 File Offset: 0x00056D58
        public float deltaTime
        {
            get
            {
                return (float)this.m_DeltaTime;
            }
        }

        /// <summary>
        ///   <para>The weight of the current Playable.</para>
        /// </summary>
        // Token: 0x17000CEF RID: 3311
        // (get) Token: 0x060038E8 RID: 14568 RVA: 0x00058B74 File Offset: 0x00056D74
        public float weight
        {
            get
            {
                return this.m_Weight;
            }
        }

        /// <summary>
        ///   <para>The accumulated weight of the Playable during the PlayableGraph traversal.</para>
        /// </summary>
        // Token: 0x17000CF0 RID: 3312
        // (get) Token: 0x060038E9 RID: 14569 RVA: 0x00058B90 File Offset: 0x00056D90
        public float effectiveWeight
        {
            get
            {
                return this.m_EffectiveWeight;
            }
        }

        /// <summary>
        ///   <para>The accumulated speed of the Playable during the PlayableGraph traversal.</para>
        /// </summary>
        // Token: 0x17000CF1 RID: 3313
        // (get) Token: 0x060038EA RID: 14570 RVA: 0x00058BAC File Offset: 0x00056DAC
        public float effectiveSpeed
        {
            get
            {
                return this.m_EffectiveSpeed;
            }
        }

        /// <summary>
        ///   <para>Indicates the type of evaluation that caused PlayableGraph.PrepareFrame to be called.</para>
        /// </summary>
        // Token: 0x17000CF2 RID: 3314
        // (get) Token: 0x060038EB RID: 14571 RVA: 0x00058BC8 File Offset: 0x00056DC8
        public FrameData.EvaluationType evaluationType
        {
            get
            {
                return ((this.m_Flags & FrameData.Flags.Evaluate) == (FrameData.Flags)0) ? FrameData.EvaluationType.Playback : FrameData.EvaluationType.Evaluate;
            }
        }

        /// <summary>
        ///   <para>Indicates that the local time was explicitly set.</para>
        /// </summary>
        // Token: 0x17000CF3 RID: 3315
        // (get) Token: 0x060038EC RID: 14572 RVA: 0x00058BF4 File Offset: 0x00056DF4
        public bool seekOccurred
        {
            get
            {
                return (this.m_Flags & FrameData.Flags.SeekOccured) != (FrameData.Flags)0;
            }
        }

        // Token: 0x04001115 RID: 4373
        internal ulong m_FrameID;

        // Token: 0x04001116 RID: 4374
        internal double m_DeltaTime;

        // Token: 0x04001117 RID: 4375
        internal float m_Weight;

        // Token: 0x04001118 RID: 4376
        internal float m_EffectiveWeight;

        // Token: 0x04001119 RID: 4377
        internal float m_EffectiveSpeed;

        // Token: 0x0400111A RID: 4378
        internal FrameData.Flags m_Flags;

        // Token: 0x020004B2 RID: 1202
        [Flags]
        internal enum Flags
        {
            // Token: 0x0400111C RID: 4380
            Evaluate = 1,
            // Token: 0x0400111D RID: 4381
            SeekOccured = 2
        }

        /// <summary>
        ///   <para>Describes the cause for the evaluation of a PlayableGraph.</para>
        /// </summary>
        // Token: 0x020004B3 RID: 1203
        public enum EvaluationType
        {
            /// <summary>
            ///   <para>Indicates the graph was updated due to a call to PlayableGraph.Evaluate.</para>
            /// </summary>
            // Token: 0x0400111F RID: 4383
            Evaluate,
            /// <summary>
            ///   <para>Indicates the graph was called by the runtime during normal playback due to PlayableGraph.Play being called.</para>
            /// </summary>
            // Token: 0x04001120 RID: 4384
            Playback
        }
    }
}

using System;

namespace UnityEngine.Playables
{
    /// <summary>
    ///   <para>Script output for the Graph. ScriptPlayable can be used to write custom Playable that implement their own PrepareFrame callback.</para>
    /// </summary>
    // Token: 0x020000F0 RID: 240

    public struct ScriptPlayableOutput
    {
        /// <summary>
        ///   <para>Used to compare against ScriptPlayableOutput instances to check their validity.</para>
        /// </summary>
        // Token: 0x170003B4 RID: 948
        // (get) Token: 0x0600113F RID: 4415 RVA: 0x000171D0 File Offset: 0x000153D0
        public static ScriptPlayableOutput Null
        {
            get
            {
                return new ScriptPlayableOutput
                {
                    m_Output = new PlayableOutput
                    {
                        m_Version = 69
                    }
                };
            }
        }

        // Token: 0x170003B5 RID: 949
        // (get) Token: 0x06001140 RID: 4416 RVA: 0x00017208 File Offset: 0x00015408
        // (set) Token: 0x06001141 RID: 4417 RVA: 0x00017228 File Offset: 0x00015428
        internal Object referenceObject
        {
            get
            {
                return PlayableOutput.GetInternalReferenceObject(ref this.m_Output);
            }
            set
            {
                PlayableOutput.SetInternalReferenceObject(ref this.m_Output, value);
            }
        }

        /// <summary>
        ///   <para>Used to pass custom data to ScriptPlayable.ProcessFrame.</para>
        /// </summary>
        // Token: 0x170003B6 RID: 950
        // (get) Token: 0x06001142 RID: 4418 RVA: 0x00017238 File Offset: 0x00015438
        // (set) Token: 0x06001143 RID: 4419 RVA: 0x00017258 File Offset: 0x00015458
        public Object userData
        {
            get
            {
                return PlayableOutput.GetInternalUserData(ref this.m_Output);
            }
            set
            {
                PlayableOutput.SetInternalUserData(ref this.m_Output, value);
            }
        }

        /// <summary>
        ///   <para>Returns true if the PlayableOutput is properly constructed by the PlayableGraph and has not been destroyed.</para>
        /// </summary>
        // Token: 0x06001144 RID: 4420 RVA: 0x00017268 File Offset: 0x00015468
        public bool IsValid()
        {
            return PlayableOutput.IsValidInternal(ref this.m_Output);
        }

        /// <summary>
        ///   <para>The Playable that is bound to the output.</para>
        /// </summary>
        // Token: 0x170003B7 RID: 951
        // (get) Token: 0x06001145 RID: 4421 RVA: 0x00017288 File Offset: 0x00015488
        // (set) Token: 0x06001146 RID: 4422 RVA: 0x000172A8 File Offset: 0x000154A8
        public PlayableHandle sourcePlayable
        {
            get
            {
                return PlayableOutput.InternalGetSourcePlayable(ref this.m_Output);
            }
            set
            {
                PlayableOutput.InternalSetSourcePlayable(ref this.m_Output, ref value);
            }
        }

        // Token: 0x170003B8 RID: 952
        // (get) Token: 0x06001147 RID: 4423 RVA: 0x000172B8 File Offset: 0x000154B8
        // (set) Token: 0x06001148 RID: 4424 RVA: 0x000172D8 File Offset: 0x000154D8
        public int sourceInputPort
        {
            get
            {
                return PlayableOutput.InternalGetSourceInputPort(ref this.m_Output);
            }
            set
            {
                PlayableOutput.InternalSetSourceInputPort(ref this.m_Output, value);
            }
        }

        // Token: 0x0400023A RID: 570
        internal PlayableOutput m_Output;
    }
}

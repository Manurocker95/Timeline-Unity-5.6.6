using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Audio output for the PlayableGraph.  Defines how a Playable is connected to an AudioMixerGroup.</para>
	/// </summary>
	// Token: 0x0200021B RID: 539
	
	public struct AudioPlayableOutput
	{
		/// <summary>
		///   <para>Used to compare against AudioPlayableOutput instances to check their validity.</para>
		/// </summary>
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x00028A40 File Offset: 0x00026C40
		public static AudioPlayableOutput Null
		{
			get
			{
				return new AudioPlayableOutput
				{
					m_Output = new PlayableOutput
					{
						m_Version = 69
					}
				};
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x00028A78 File Offset: 0x00026C78
		// (set) Token: 0x06002343 RID: 9027 RVA: 0x00028A98 File Offset: 0x00026C98
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
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x00028AA8 File Offset: 0x00026CA8
		// (set) Token: 0x06002345 RID: 9029 RVA: 0x00028AC8 File Offset: 0x00026CC8
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
		///   <para>Returns true if the PlayableOutput has been properly constructed by the PlayableGraph and has not been destroyed.</para>
		/// </summary>
		// Token: 0x06002346 RID: 9030 RVA: 0x00028AD8 File Offset: 0x00026CD8
		public bool IsValid()
		{
			return PlayableOutput.IsValidInternal(ref this.m_Output);
		}

		/// <summary>
		///   <para>The AudioMixerGroup that is bound to this output.</para>
		/// </summary>
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x00028AF8 File Offset: 0x00026CF8
		// (set) Token: 0x06002348 RID: 9032 RVA: 0x00028B18 File Offset: 0x00026D18
		public AudioMixerGroup target
		{
			get
			{
				return AudioPlayableOutput.InternalGetTarget(ref this.m_Output);
			}
			set
			{
				AudioPlayableOutput.InternalSetTarget(ref this.m_Output, value);
			}
		}

		// Token: 0x06002349 RID: 9033
		
		[MethodImpl(4096)]
		private static extern AudioMixerGroup InternalGetTarget(ref PlayableOutput output);

		// Token: 0x0600234A RID: 9034
		
		[MethodImpl(4096)]
		private static extern void InternalSetTarget(ref PlayableOutput output, AudioMixerGroup target);

		/// <summary>
		///   <para>The Playable that is bound to the output.</para>
		/// </summary>
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x00028B28 File Offset: 0x00026D28
		// (set) Token: 0x0600234C RID: 9036 RVA: 0x00028B48 File Offset: 0x00026D48
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

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600234D RID: 9037 RVA: 0x00028B58 File Offset: 0x00026D58
		// (set) Token: 0x0600234E RID: 9038 RVA: 0x00028B78 File Offset: 0x00026D78
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

		// Token: 0x0400063F RID: 1599
		internal PlayableOutput m_Output;
	}
}

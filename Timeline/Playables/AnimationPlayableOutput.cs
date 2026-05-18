using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Animation output for the PlayableGraph.  Defines how a Playable is connected to an Animator.</para>
	/// </summary>
	// Token: 0x02000266 RID: 614
	public struct AnimationPlayableOutput
	{
		/// <summary>
		///   <para>Used to compare against AnimationPlayableOutput instances to check their validity.</para>
		/// </summary>
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x0002C2E8 File Offset: 0x0002A4E8
		public static AnimationPlayableOutput Null
		{
			get
			{
				return new AnimationPlayableOutput
				{
					m_Output = new PlayableOutput
					{
						m_Version = 69
					}
				};
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x0002C320 File Offset: 0x0002A520
		// (set) Token: 0x06002701 RID: 9985 RVA: 0x0002C340 File Offset: 0x0002A540
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
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002702 RID: 9986 RVA: 0x0002C350 File Offset: 0x0002A550
		// (set) Token: 0x06002703 RID: 9987 RVA: 0x0002C370 File Offset: 0x0002A570
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
		// Token: 0x06002704 RID: 9988 RVA: 0x0002C380 File Offset: 0x0002A580
		public bool IsValid()
		{
			return PlayableOutput.IsValidInternal(ref this.m_Output);
		}

		/// <summary>
		///   <para>The Animator component that is bound to this output.</para>
		/// </summary>
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
		// (set) Token: 0x06002706 RID: 9990 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
		public Animator target
		{
			get
			{
				return AnimationPlayableOutput.InternalGetTarget(ref this.m_Output);
			}
			set
			{
				AnimationPlayableOutput.InternalSetTarget(ref this.m_Output, value);
			}
		}

		/// <summary>
		///   <para>The blend weight of the sourcePlayable to the animator.</para>
		/// </summary>
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x0002C3D0 File Offset: 0x0002A5D0
		// (set) Token: 0x06002708 RID: 9992 RVA: 0x0002C3F0 File Offset: 0x0002A5F0
		public float weight
		{
			get
			{
				return PlayableOutput.InternalGetWeight(ref this.m_Output);
			}
			set
			{
				PlayableOutput.InternalSetWeight(ref this.m_Output, value);
			}
		}

		// Token: 0x06002709 RID: 9993
		
		[MethodImpl(4096)]
		private static extern Animator InternalGetTarget(ref PlayableOutput output);

		// Token: 0x0600270A RID: 9994
		
		[MethodImpl(4096)]
		private static extern void InternalSetTarget(ref PlayableOutput output, Animator target);

		/// <summary>
		///   <para>The Playable that is bound to the output.</para>
		/// </summary>
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x0002C400 File Offset: 0x0002A600
		// (set) Token: 0x0600270C RID: 9996 RVA: 0x0002C420 File Offset: 0x0002A620
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

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x0002C430 File Offset: 0x0002A630
		// (set) Token: 0x0600270E RID: 9998 RVA: 0x0002C450 File Offset: 0x0002A650
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

		// Token: 0x04000762 RID: 1890
		internal PlayableOutput m_Output;
	}
}

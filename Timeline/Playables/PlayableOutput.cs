using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
    // Token: 0x020000ED RID: 237
    //[UsedByNativeCode]
    internal struct PlayableOutput
	{
        // Token: 0x06001117 RID: 4375
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern bool IsValidInternal(ref PlayableOutput output);

        // Token: 0x06001118 RID: 4376
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern Object GetInternalReferenceObject(ref PlayableOutput output);

        // Token: 0x06001119 RID: 4377
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern void SetInternalReferenceObject(ref PlayableOutput output, Object target);

        // Token: 0x0600111A RID: 4378
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern Object GetInternalUserData(ref PlayableOutput output);

        // Token: 0x0600111B RID: 4379
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern void SetInternalUserData(ref PlayableOutput output, Object target);

		// Token: 0x0600111C RID: 4380 RVA: 0x00016CE4 File Offset: 0x00014EE4
		internal static PlayableHandle InternalGetSourcePlayable(ref PlayableOutput output)
		{
			PlayableHandle result;
			PlayableOutput.INTERNAL_CALL_InternalGetSourcePlayable(ref output, out result);
			return result;
		}

        // Token: 0x0600111D RID: 4381
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalGetSourcePlayable(ref PlayableOutput output, out PlayableHandle value);

		// Token: 0x0600111E RID: 4382 RVA: 0x00016D04 File Offset: 0x00014F04
		internal static void InternalSetSourcePlayable(ref PlayableOutput output, ref PlayableHandle target)
		{
			PlayableOutput.INTERNAL_CALL_InternalSetSourcePlayable(ref output, ref target);
		}

        // Token: 0x0600111F RID: 4383
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalSetSourcePlayable(ref PlayableOutput output, ref PlayableHandle target);

        // Token: 0x06001120 RID: 4384
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern int InternalGetSourceInputPort(ref PlayableOutput output);

        // Token: 0x06001121 RID: 4385
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern void InternalSetSourceInputPort(ref PlayableOutput output, int port);

        // Token: 0x06001122 RID: 4386
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern void InternalSetWeight(ref PlayableOutput output, float weight);

        // Token: 0x06001123 RID: 4387
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		internal static extern float InternalGetWeight(ref PlayableOutput output);

		// Token: 0x04000237 RID: 567
		internal IntPtr m_Handle;

		// Token: 0x04000238 RID: 568
		internal int m_Version;
	}
}

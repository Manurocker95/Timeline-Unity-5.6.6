using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Playables are customizable runtime objects that can be connected together in a tree to create complex behaviours.</para>
	/// </summary>
	// Token: 0x020000E9 RID: 233
	
	public class Playable : IPlayable
	{
		// Token: 0x060010DD RID: 4317 RVA: 0x0001690C File Offset: 0x00014B0C
		public static implicit operator PlayableHandle(Playable b)
		{
			return b.handle;
		}

		/// <summary>
		///   <para>Returns true if the Playable is valid. A playable can be invalid if it was disposed. This is different from a Null playable.</para>
		/// </summary>
		// Token: 0x060010DE RID: 4318 RVA: 0x00016928 File Offset: 0x00014B28
		public bool IsValid()
		{
			return this.handle.IsValid();
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00016948 File Offset: 0x00014B48
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x00016964 File Offset: 0x00014B64
		public PlayableHandle playableHandle
		{
			get
			{
				return this.handle;
			}
			set
			{
				this.handle = value;
			}
		}

		/// <summary>
		///   <para>Returns the PlayableHandle for this playable.</para>
		/// </summary>
		// Token: 0x0400022F RID: 559
		public PlayableHandle handle;
	}
}

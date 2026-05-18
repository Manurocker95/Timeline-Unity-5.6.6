using System;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Specifies the type of PlayableAsset or ScriptPlayables that this track can create clips representing.</para>
	/// </summary>
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class TrackClipTypeAttribute : Attribute
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00004561 File Offset: 0x00002761
		public TrackClipTypeAttribute(Type clipClass)
		{
			this.inspectedType = clipClass;
		}

		// Token: 0x0400003B RID: 59
		public Type inspectedType;
	}
}

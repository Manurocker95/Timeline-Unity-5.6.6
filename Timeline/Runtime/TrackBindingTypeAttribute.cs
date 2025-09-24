using System;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Specifies the type of object that should be bound to a TrackAsset.</para>
	/// </summary>
	// Token: 0x02000012 RID: 18
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class TrackBindingTypeAttribute : Attribute
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00004589 File Offset: 0x00002789
		public TrackBindingTypeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x0400003D RID: 61
		public Type type;
	}
}

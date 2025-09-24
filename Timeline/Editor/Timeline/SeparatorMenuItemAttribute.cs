using System;

namespace UnityEditor.Timeline
{
	// Token: 0x02000034 RID: 52
	[AttributeUsage(AttributeTargets.All)]
	internal class SeparatorMenuItemAttribute : Attribute
	{
		// Token: 0x060001DC RID: 476 RVA: 0x000115B7 File Offset: 0x0000F9B7
		public SeparatorMenuItemAttribute(SeparatorMenuItemPosition position)
		{
			this.position = position;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000115C7 File Offset: 0x0000F9C7
		public SeparatorMenuItemAttribute()
		{
			this.position = SeparatorMenuItemPosition.None;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000115D8 File Offset: 0x0000F9D8
		public bool before
		{
			get
			{
				return (this.position & SeparatorMenuItemPosition.Before) == SeparatorMenuItemPosition.Before;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000115F8 File Offset: 0x0000F9F8
		public bool after
		{
			get
			{
				return (this.position & SeparatorMenuItemPosition.After) == SeparatorMenuItemPosition.After;
			}
		}

		// Token: 0x04000194 RID: 404
		public SeparatorMenuItemPosition position;
	}
}

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000078 RID: 120
	internal abstract class Manipulator
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x0000A0BE File Offset: 0x000084BE
		protected Manipulator()
		{
			Manipulator.showEventConsumer = false;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000A0D0 File Offset: 0x000084D0
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000A0E9 File Offset: 0x000084E9
		public static bool showEventConsumer { get; set; }

		// Token: 0x060003D9 RID: 985
		public abstract void Init(IControl parent);

		// Token: 0x060003DA RID: 986 RVA: 0x0000A0F4 File Offset: 0x000084F4
		public bool ConsumeEvent()
		{
			if (Manipulator.showEventConsumer)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Event ",
					Event.current.type,
					" consumed by ",
					base.GetType().Name
				}));
			}
			return true;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000A154 File Offset: 0x00008554
		public bool IgnoreEvent()
		{
			return false;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000A16C File Offset: 0x0000856C
		public static List<IBounds> GetElementsInRectangle(QuadTree<IBounds> qtree, Vector2 startPoint, Vector2 endPoint)
		{
			Rect r = default(Rect);
			r.min = new Vector2(Math.Min(startPoint.x, endPoint.x), Math.Min(startPoint.y, endPoint.y));
			r.max = new Vector2(Math.Max(startPoint.x, endPoint.x), Math.Max(startPoint.y, endPoint.y));
			return Manipulator.GetElementsInRectangle(qtree, r);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000A1F4 File Offset: 0x000085F4
		public static List<IBounds> GetElementsInRectangle(QuadTree<IBounds> qtree, Rect r)
		{
			return qtree.ContainedBy(r);
		}
	}
}

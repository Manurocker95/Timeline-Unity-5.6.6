using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000096 RID: 150
	internal sealed class Clipboard
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x0002924D File Offset: 0x0002764D
		public static void AddData(Object data)
		{
			Clipboard.AddDataInternal(data);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00029258 File Offset: 0x00027658
		public static void AddDataCollection(IEnumerable<Object> data)
		{
			foreach (Object data2 in data)
			{
				Clipboard.AddDataInternal(data2);
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000292B0 File Offset: 0x000276B0
		public static IEnumerable<T> GetData<T>() where T : class
		{
			IEnumerable<T> result;
			try
			{
				result = (from x in Clipboard.m_Data
				where typeof(T).IsAssignableFrom(x.targetObject.GetType())
				select x into y
				select y.targetObject as T).ToList<T>();
			}
			catch (NullReferenceException)
			{
				result = Enumerable.Empty<T>();
			}
			return result;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00029310 File Offset: 0x00027710
		public static void Clear()
		{
			Clipboard.m_Data.Clear();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00029320 File Offset: 0x00027720
		private static void AddDataInternal(Object data)
		{
			SerializedObject item = new SerializedObject(data);
			Clipboard.m_Data.Add(item);
		}

		// Token: 0x04000379 RID: 889
		private static readonly int kListInitialSize = 10;

		// Token: 0x0400037A RID: 890
		private static List<SerializedObject> m_Data = new List<SerializedObject>(Clipboard.kListInitialSize);
	}
}

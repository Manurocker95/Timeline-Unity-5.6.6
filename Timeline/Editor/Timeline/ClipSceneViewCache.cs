using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Timeline
{
	// Token: 0x02000050 RID: 80
	internal static class ClipSceneViewCache
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x00018E00 File Offset: 0x00017200
		private static void BuildCache()
		{
			ClipSceneViewCache.m_Dictionary = new Dictionary<Type, Type>();
			List<Type> list = (from x in EditorAssemblies.loadedTypes
			where typeof(IClipSceneView).IsAssignableFrom(x) && x.GetCustomAttributes(typeof(ClipSceneViewAttribute), true).Any<object>()
			select x).ToList<Type>();
			for (int i = 0; i < list.Count<Type>(); i++)
			{
				Type type = list[i];
				ClipSceneViewAttribute clipSceneViewAttribute = (ClipSceneViewAttribute)type.GetCustomAttributes(typeof(ClipSceneViewAttribute), true)[0];
				Type typeInspected = clipSceneViewAttribute.typeInspected;
				ClipSceneViewCache.m_Dictionary[typeInspected] = type;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00018E94 File Offset: 0x00017294
		public static IClipSceneView GetViewForType(Type type)
		{
			if (ClipSceneViewCache.m_Dictionary == null)
			{
				ClipSceneViewCache.BuildCache();
			}
			Type type2 = null;
			IClipSceneView result;
			if (!ClipSceneViewCache.m_Dictionary.TryGetValue(type, out type2))
			{
				result = null;
			}
			else
			{
				result = (IClipSceneView)Activator.CreateInstance(type2);
			}
			return result;
		}

		// Token: 0x0400021C RID: 540
		private static Dictionary<Type, Type> m_Dictionary = null;
	}
}

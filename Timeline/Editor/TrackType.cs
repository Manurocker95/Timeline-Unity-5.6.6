using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000019 RID: 25
	internal class TrackType
	{
		// Token: 0x0600013C RID: 316 RVA: 0x0000BDA3 File Offset: 0x0000A1A3
		public TrackType(Type trackType, TimelineAsset.MediaType mt)
		{
			this.m_TrackType = trackType;
			this.m_MediaType = mt;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000BDBC File Offset: 0x0000A1BC
		public TrackType(Type trackType)
		{
			this.m_TrackType = trackType;
			this.m_MediaType = 0;
			object[] customAttributes = trackType.GetCustomAttributes(typeof(TrackMediaType), true);
			if (customAttributes.Length > 0)
			{
				this.m_MediaType = ((TrackMediaType)customAttributes[0]).m_MediaType;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000BE0C File Offset: 0x0000A20C
		public Type innerType
		{
			get
			{
				return this.m_TrackType;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000BE28 File Offset: 0x0000A228
		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				TrackType trackType = obj as TrackType;
				result = (trackType != null && this.m_TrackType == trackType.m_TrackType);
			}
			return result;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000BE70 File Offset: 0x0000A270
		public bool Equals(TrackType p)
		{
			return p != null && this.m_TrackType == p.m_TrackType;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000BEA4 File Offset: 0x0000A2A4
		public override int GetHashCode()
		{
			return this.m_TrackType.GetHashCode() ^ this.m_MediaType.GetHashCode();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000BED8 File Offset: 0x0000A2D8
		public bool requiresGameObjectBinding
		{
			get
			{
				bool result;
				if (this.m_MediaType == null)
				{
					result = true;
				}
				else if (!this.m_TrackType.IsDefined(typeof(TrackBindingTypeAttribute), true))
				{
					result = false;
				}
				else
				{
					TrackBindingTypeAttribute trackBindingTypeAttribute = Attribute.GetCustomAttribute(this.m_TrackType, typeof(TrackBindingTypeAttribute), true) as TrackBindingTypeAttribute;
					result = (typeof(GameObject).IsAssignableFrom(trackBindingTypeAttribute.type) || typeof(Component).IsAssignableFrom(trackBindingTypeAttribute.type));
				}
				return result;
			}
		}

		// Token: 0x0400013D RID: 317
		public Type m_TrackType;

		// Token: 0x0400013E RID: 318
		public TimelineAsset.MediaType m_MediaType;
	}
}

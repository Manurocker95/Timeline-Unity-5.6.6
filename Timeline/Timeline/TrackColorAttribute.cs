using System;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Attribute used to specify the color of the track and its clips inside the Timeline Editor.</para>
	/// </summary>
	// Token: 0x02000028 RID: 40
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TrackColorAttribute : Attribute
	{
		/// <summary>
		///   <para>Specify the track color using [0-1] R,G,B values.</para>
		/// </summary>
		/// <param name="r">Red value	[0-1].</param>
		/// <param name="g">Green value [0-1].</param>
		/// <param name="b">Blue value [0-1].</param>
		// Token: 0x06000162 RID: 354 RVA: 0x0000713E File Offset: 0x0000533E
		public TrackColorAttribute(float r, float g, float b)
		{
			this.m_Color = new Color(r, g, b);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00007158 File Offset: 0x00005358
		public Color color
		{
			get
			{
				return this.m_Color;
			}
		}

		// Token: 0x040000C4 RID: 196
		private Color m_Color;
	}
}

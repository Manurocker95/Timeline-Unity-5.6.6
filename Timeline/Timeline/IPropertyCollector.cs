using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000039 RID: 57
	public interface IPropertyCollector
	{
		/// <summary>
		///   <para>Sets the active game object for subsequence property modifications.</para>
		/// </summary>
		/// <param name="gameObject"></param>
		// Token: 0x060001B3 RID: 435
		void PushActiveGameObject(GameObject gameObject);

		/// <summary>
		///   <para>Removes the active GameObject from the modification stack, restoring the previous value.</para>
		/// </summary>
		// Token: 0x060001B4 RID: 436
		void PopActiveGameObject();

		/// <summary>
		///   <para>Add property modifications modified by an animation clip.</para>
		/// </summary>
		/// <param name="clip"></param>
		/// <param name="obj"></param>
		// Token: 0x060001B5 RID: 437
		void AddFromClip(AnimationClip clip);

		// Token: 0x060001B6 RID: 438
		void AddFromName<T>(string name) where T : Component;

		/// <summary>
		///   <para>Add property modifications using the serialized property name.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="obj"></param>
		// Token: 0x060001B7 RID: 439
		void AddFromName(string name);

		/// <summary>
		///   <para>Add property modifications modified by an animation clip.</para>
		/// </summary>
		/// <param name="clip"></param>
		/// <param name="obj"></param>
		// Token: 0x060001B8 RID: 440
		void AddFromClip(GameObject obj, AnimationClip clip);

		// Token: 0x060001B9 RID: 441
		void AddFromName<T>(GameObject obj, string name) where T : Component;

		/// <summary>
		///   <para>Add property modifications using the serialized property name.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="obj"></param>
		// Token: 0x060001BA RID: 442
		void AddFromName(GameObject obj, string name);

		// Token: 0x060001BB RID: 443
		void AddFromComponent(GameObject obj, Component component);

		/// <summary>
		///   <para>Add property modifications for a ScriptableObject.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="clip"></param>
		// Token: 0x060001BC RID: 444
		void AddObjectProperties(Object obj, AnimationClip clip);
	}
}

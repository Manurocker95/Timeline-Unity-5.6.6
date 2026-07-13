using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
    /// <summary>
    ///   <para>Extends PlayableGraph for Script Playables.</para>
    /// </summary>
    // Token: 0x020000EF RID: 239
    public static class ScriptPlayableGraphExtensions
    {
        /// <summary>
        ///   <para>Get the number of ScriptOutputs in the graph.</para>
        /// </summary>
        /// <param name="graph">The PlayableGraph object.</param>
        // Token: 0x06001130 RID: 4400 RVA: 0x00016DC4 File Offset: 0x00014FC4
        public static int GetScriptOutputCount(this PlayableGraph graph)
        {
            return InternalScriptOutputCount(ref graph);
        }

        private static int InternalScriptOutputCount(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.GetScriptOutputCount(graph);
        }

        /// <summary>
        ///   <para>Get a ScriptOutput at the given index.</para>
        /// </summary>
        /// <param name="graph">The PlayableGraph object.</param>
        /// <param name="index">The index of the ScriptPlayableOutput.</param>
        // Token: 0x06001132 RID: 4402 RVA: 0x00016DE0 File Offset: 0x00014FE0
        public static ScriptPlayableOutput GetScriptOutput(this PlayableGraph graph, int index)
        {
            ScriptPlayableOutput scriptPlayableOutput = default(ScriptPlayableOutput);
            ScriptPlayableOutput result;
            if (!ScriptPlayableGraphExtensions.InternalGetScriptOutput(ref graph, index, out scriptPlayableOutput.m_Output))
            {
                result = ScriptPlayableOutput.Null;
            }
            else
            {
                result = scriptPlayableOutput;
            }
            return result;
        }

        // Token: 0x06001133 RID: 4403

        private static bool InternalGetScriptOutput(
            ref PlayableGraph graph,
            int index,
            out PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetScriptOutput(
                graph,
                index,
                out output);
        }

        // Token: 0x06001134 RID: 4404 RVA: 0x00016E20 File Offset: 0x00015020
        public static PlayableHandle CreateScriptPlayable<T>(this PlayableGraph graph) where T : class, IScriptPlayable, IPlayable, new()
        {
            PlayableHandle playableHandle = graph.CreatePlayable();
            PlayableHandle result;
            if (!playableHandle.IsValid())
            {
                result = PlayableHandle.Null;
            }
            else
            {
                T t = (T)((object)null);
                bool flag = typeof(ScriptableObject).IsAssignableFrom(typeof(T));
                if (flag)
                {
                    t = (ScriptableObject.CreateInstance(typeof(T)) as T);
                }
                else
                {
                    t = Activator.CreateInstance<T>();
                }
                if (t == null)
                {
                    playableHandle.Destroy();
                    Debug.LogError("Could not create a ScriptPlayable of Type " + typeof(T).ToString());
                    result = PlayableHandle.Null;
                }
                else
                {
                    ScriptPlayableGraphExtensions.SetScriptInstance(ref playableHandle, t);
                    IPlayable playable = t;
                    playable.playableHandle = playableHandle;
                    result = playableHandle;
                }
            }
            return result;
        }

        // Token: 0x06001135 RID: 4405 RVA: 0x00016F00 File Offset: 0x00015100
        public static PlayableHandle CreateScriptMixerPlayable<T>(this PlayableGraph graph, int inputCount) where T : class, IScriptPlayable, IPlayable, new()
        {
            PlayableHandle result = graph.CreateScriptPlayable<T>();
            if (result.IsValid())
            {
                result.inputCount = inputCount;
            }
            return result;
        }

        // Token: 0x06001136 RID: 4406 RVA: 0x00016F34 File Offset: 0x00015134
        public static PlayableHandle CloneScriptPlayable(this PlayableGraph graph, IScriptPlayable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source parameter cannot be null");
            }
            ScriptableObject scriptableObject = source as ScriptableObject;
            Object @object = source as Object;
            PlayableHandle result;
            if (scriptableObject != null)
            {
                result = ScriptPlayableGraphExtensions.InternalCloneScriptableObjectPlayable(graph, scriptableObject, scriptableObject.GetType());
            }
            else if (@object != null)
            {
                result = ScriptPlayableGraphExtensions.InternalCloneEngineObjectPlayable(graph, @object);
            }
            else
            {
                result = ScriptPlayableGraphExtensions.InternalCloneObjectPlayable(graph, source);
            }
            return result;
        }

        // Token: 0x06001137 RID: 4407 RVA: 0x00016FA8 File Offset: 0x000151A8
        internal static PlayableHandle InternalCloneScriptableObjectPlayable(PlayableGraph graph, ScriptableObject source, Type type)
        {
            PlayableHandle playableHandle = graph.CreatePlayable();
            PlayableHandle result;
            if (!playableHandle.IsValid())
            {
                result = PlayableHandle.Null;
            }
            else
            {
                ScriptableObject scriptableObject = Object.Instantiate<ScriptableObject>(source);
                if (scriptableObject == null)
                {
                    Debug.LogError("Could not clone a ScriptPlayable of Type " + type.ToString());
                    playableHandle.Destroy();
                    result = PlayableHandle.Null;
                }
                else
                {
                    ScriptPlayableGraphExtensions.SetScriptInstance(ref playableHandle, scriptableObject);
                    IPlayable playable = (IPlayable)scriptableObject;
                    playable.playableHandle = playableHandle;
                    scriptableObject.hideFlags |= HideFlags.DontSave;
                    result = playableHandle;
                }
            }
            return result;
        }

        // Token: 0x06001138 RID: 4408 RVA: 0x00017040 File Offset: 0x00015240
        internal static PlayableHandle InternalCloneEngineObjectPlayable(PlayableGraph graph, Object source)
        {
            PlayableHandle playableHandle = graph.CreatePlayable();
            PlayableHandle result;
            if (!playableHandle.IsValid())
            {
                result = PlayableHandle.Null;
            }
            else
            {
                Object @object = Object.Instantiate(source);
                if (@object == null)
                {
                    Debug.LogError("Could not clone a ScriptPlayable of Type " + source.GetType().ToString());
                    playableHandle.Destroy();
                    result = PlayableHandle.Null;
                }
                else
                {
                    ScriptPlayableGraphExtensions.SetScriptInstance(ref playableHandle, @object);
                    IPlayable playable = (IPlayable)@object;
                    playable.playableHandle = playableHandle;
                    @object.hideFlags |= HideFlags.DontSave;
                    result = playableHandle;
                }
            }
            return result;
        }

        // Token: 0x06001139 RID: 4409 RVA: 0x000170DC File Offset: 0x000152DC
        internal static PlayableHandle InternalCloneObjectPlayable(PlayableGraph graph, object source)
        {
            PlayableHandle playableHandle = graph.CreatePlayable();
            PlayableHandle result;
            if (!playableHandle.IsValid())
            {
                result = PlayableHandle.Null;
            }
            else
            {
                ICloneable cloneable = source as ICloneable;
                if (cloneable == null)
                {
                    Debug.LogError("Could not clone a ScriptPlayable of Type " + source.GetType().ToString() + " as it does not implement ICloneable");
                    playableHandle.Destroy();
                    result = PlayableHandle.Null;
                }
                else
                {
                    object obj = cloneable.Clone();
                    if (obj == null)
                    {
                        Debug.LogError("Could not clone a ScriptPlayable of Type " + source.GetType().ToString());
                        playableHandle.Destroy();
                        result = PlayableHandle.Null;
                    }
                    else
                    {
                        ScriptPlayableGraphExtensions.SetScriptInstance(ref playableHandle, obj);
                        IPlayable playable = (IPlayable)obj;
                        playable.playableHandle = playableHandle;
                        result = playableHandle;
                    }
                }
            }
            return result;
        }

        // Token: 0x0600113A RID: 4410 RVA: 0x000171A4 File Offset: 0x000153A4
        private static void SetScriptInstance(
            ref PlayableHandle handle,
            object instance)
        {
            LegacyPlayableRuntime.SetScriptInstance(handle, instance);

            if (instance != null)
                LegacyPlayableRuntime.SetPlayableType(
                    handle,
                    instance.GetType());
        }

        // Token: 0x0600113C RID: 4412 RVA: 0x000171B0 File Offset: 0x000153B0
        private static object InternalCreateScriptPlayable(
            ref PlayableGraph graph,
            ref PlayableHandle handle,
            Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            handle = graph.CreatePlayable();
            if (!handle.IsValid())
                return null;

            object instance;

            if (typeof(ScriptableObject).IsAssignableFrom(type))
                instance = ScriptableObject.CreateInstance(type);
            else
                instance = Activator.CreateInstance(type);

            if (instance == null)
            {
                graph.DestroyPlayable(handle);
                handle = PlayableHandle.Null;
                return null;
            }

            SetScriptInstance(ref handle, instance);
            return instance;
        }

        // Token: 0x0600113E RID: 4414

        private static void InternalCopyObject(Object source, Object dest)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (dest == null)
                throw new ArgumentNullException("dest");

            Debug.LogWarning(
                "InternalCopyObject is not required by the managed Legacy Timeline runtime. " +
                "Use Object.Instantiate when a clone is needed.");
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
    /// <summary>
    ///   <para>The PlayableGraph is used to manage PlayableHandle creation, destruction and connections.</para>
    /// </summary>
    // Token: 0x020000EC RID: 236

    public struct PlayableGraph
    {
        /// <summary>
        ///   <para>Returns true if the PlayableGraph has been properly constructed using PlayableGraph.CreateGraph and is not deleted.</para>
        /// </summary>
        // Token: 0x060010E6 RID: 4326 RVA: 0x000169D4 File Offset: 0x00014BD4
        public bool IsValid()
        {
            return LegacyPlayableRuntime.IsGraphValid(this);
        }

        private static bool IsValidInternal(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.IsGraphValid(graph);
        }

        /// <summary>
        ///   <para>Creates a PlayableGraph.</para>
        /// </summary>
        /// <returns>
        ///   <para>The created graph.</para>
        /// </returns>
        // Token: 0x060010E8 RID: 4328 RVA: 0x000169F0 File Offset: 0x00014BF0
        public static PlayableGraph Create()
        {
            return LegacyPlayableRuntime.CreateGraph();
        }

        public static PlayableGraph CreateGraph()
        {
            return Create();
        }

        internal static void InternalCreate(ref PlayableGraph graph)
        {
            graph = Create();
        }

        /// <summary>
        ///   <para>Indicates that a graph has completed its operations.</para>
        /// </summary>
        // Token: 0x170003AF RID: 943
        // (get) Token: 0x060010EA RID: 4330 RVA: 0x00016A18 File Offset: 0x00014C18
        public bool isDone
        {
            get
            {
                return PlayableGraph.InternalIsDone(ref this);
            }
        }

        // Token: 0x060010EB RID: 4331

        internal static bool InternalIsDone(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.IsDone(graph);
        }

        /// <summary>
        ///   <para>Property Table used to resolve ExposedReferences.</para>
        /// </summary>
        // Token: 0x170003B0 RID: 944
        // (get) Token: 0x060010EC RID: 4332 RVA: 0x00016A34 File Offset: 0x00014C34
        // (set) Token: 0x060010ED RID: 4333 RVA: 0x00016A50 File Offset: 0x00014C50
        public IExposedPropertyTable resolver
        {
            get
            {
                return PlayableGraph.InternalGetResolver(ref this);
            }
            set
            {
                PlayableGraph.InternalSetResolver(ref this, value);
            }
        }

        // Token: 0x060010EE RID: 4334

        internal static IExposedPropertyTable InternalGetResolver(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.GetResolver(graph);
        }

        // Token: 0x060010EF RID: 4335

        internal static void InternalSetResolver(
            ref PlayableGraph graph,
            IExposedPropertyTable resolver)
        {
            LegacyPlayableRuntime.SetResolver(graph, resolver);
        }

        /// <summary>
        ///   <para>Plays the graph.</para>
        /// </summary>
        // Token: 0x060010F0 RID: 4336 RVA: 0x00016A5C File Offset: 0x00014C5C
        public void Play()
        {
            PlayableGraph.InternalPlay(ref this);
        }

        // Token: 0x060010F1 RID: 4337

        internal static void InternalPlay(ref PlayableGraph graph)
        {
            LegacyPlayableRuntime.Play(graph);
        }

        /// <summary>
        ///   <para>Stops the graph, if it is playing.</para>
        /// </summary>
        // Token: 0x060010F2 RID: 4338 RVA: 0x00016A68 File Offset: 0x00014C68
        public void Stop()
        {
            PlayableGraph.InternalStop(ref this);
        }

        // Token: 0x060010F3 RID: 4339

        internal static void InternalStop(ref PlayableGraph graph)
        {
            LegacyPlayableRuntime.Stop(graph);
        }

        /// <summary>
        ///   <para>Returns the number of PlayableHandle owned by the Graph.</para>
        /// </summary>
        // Token: 0x170003B1 RID: 945
        // (get) Token: 0x060010F4 RID: 4340 RVA: 0x00016A74 File Offset: 0x00014C74
        public int playableCount
        {
            get
            {
                return PlayableGraph.InternalPlayableCount(ref this);
            }
        }

        // Token: 0x060010F5 RID: 4341

        internal static int InternalPlayableCount(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.GetPlayableCount(graph);
        }

        /// <summary>
        ///   <para>Creates a ScriptPlayableOutput in the [PlayableGraph]].</para>
        /// </summary>
        /// <param name="name">The name of the output.</param>
        // Token: 0x060010F6 RID: 4342 RVA: 0x00016A90 File Offset: 0x00014C90
        public ScriptPlayableOutput CreateScriptOutput(string name)
        {
            ScriptPlayableOutput scriptPlayableOutput = default(ScriptPlayableOutput);
            ScriptPlayableOutput result;
            if (!PlayableGraph.InternalCreateScriptOutput(ref this, name, out scriptPlayableOutput.m_Output))
            {
                result = ScriptPlayableOutput.Null;
            }
            else
            {
                result = scriptPlayableOutput;
            }
            return result;
        }

        // Token: 0x060010F7 RID: 4343

        private static bool InternalCreateScriptOutput(
            ref PlayableGraph graph,
            string name,
            out PlayableOutput output)
        {
            return LegacyPlayableRuntime.CreateOutput(graph, name, out output);
        }

        /// <summary>
        ///   <para>This method allows you to create custom Playable instances.</para>
        /// </summary>
        /// <returns>
        ///   <para>The created Playable.</para>
        /// </returns>
        // Token: 0x060010F8 RID: 4344 RVA: 0x00016ACC File Offset: 0x00014CCC
        public PlayableHandle CreatePlayable()
        {
            return LegacyPlayableRuntime.CreatePlayable(this);
        }

        // Token: 0x060010F9 RID: 4345 RVA: 0x00016B00 File Offset: 0x00014D00
        [ExcludeFromDocs]
        public PlayableHandle CreateGenericMixerPlayable()
        {
            int inputCount = 0;
            return this.CreateGenericMixerPlayable(inputCount);
        }

        /// <summary>
        ///   <para>Creates a generic ScriptPlayable mixer.</para>
        /// </summary>
        /// <param name="inputCount">The number of input.</param>
        /// <returns>
        ///   <para>The created Playable.</para>
        /// </returns>
        // Token: 0x060010FA RID: 4346 RVA: 0x00016B20 File Offset: 0x00014D20
        public PlayableHandle CreateGenericMixerPlayable([DefaultValue("0")] int inputCount)
        {
            PlayableHandle @null = PlayableHandle.Null;
            PlayableHandle result;
            if (!PlayableGraph.InternalCreatePlayable(ref this, ref @null))
            {
                result = PlayableHandle.Null;
            }
            else
            {
                @null.inputCount = inputCount;
                result = @null;
            }
            return result;
        }

        // Token: 0x060010FB RID: 4347 RVA: 0x00016B5C File Offset: 0x00014D5C
        private static bool InternalCreatePlayable(
            ref PlayableGraph graph,
            ref PlayableHandle handle)
        {
            handle = LegacyPlayableRuntime.CreatePlayable(graph);
            return handle.IsValid();
        }

        /// <summary>
        ///   <para>Destroys the graph.</para>
        /// </summary>
        // Token: 0x060010FD RID: 4349 RVA: 0x00016B78 File Offset: 0x00014D78
        public void Destroy()
        {
            LegacyPlayableRuntime.DestroyGraph(ref this);
        }

        private static void DestroyInternal(ref PlayableGraph graph)
        {
            LegacyPlayableRuntime.DestroyGraph(ref graph);
        }

        /// <summary>
        ///   <para>Connects two Playable instances, either by referencing the Playable instances themselves or by their PlayableHandles.</para>
        /// </summary>
        /// <param name="source">The source playable or its handle.</param>
        /// <param name="sourceOutputPort">The port used in the source playable.</param>
        /// <param name="destination">The destination playable or its handle.</param>
        /// <param name="destinationInputPort">The port used in the destination playable.</param>
        /// <returns>
        ///   <para>Returns true if connection is successful.</para>
        /// </returns>
        // Token: 0x060010FF RID: 4351 RVA: 0x00016B84 File Offset: 0x00014D84
        public bool Connect(PlayableHandle source, int sourceOutputPort, PlayableHandle destination, int destinationInputPort)
        {
            return PlayableGraph.ConnectInternal(ref this, source, sourceOutputPort, destination, destinationInputPort);
        }

        /// <summary>
        ///   <para>Connects two Playable instances, either by referencing the Playable instances themselves or by their PlayableHandles.</para>
        /// </summary>
        /// <param name="source">The source playable or its handle.</param>
        /// <param name="sourceOutputPort">The port used in the source playable.</param>
        /// <param name="destination">The destination playable or its handle.</param>
        /// <param name="destinationInputPort">The port used in the destination playable.</param>
        /// <returns>
        ///   <para>Returns true if connection is successful.</para>
        /// </returns>
        // Token: 0x06001100 RID: 4352 RVA: 0x00016BA4 File Offset: 0x00014DA4
        public bool Connect(Playable source, int sourceOutputPort, Playable destination, int destinationInputPort)
        {
            return PlayableGraph.ConnectInternal(ref this, source.handle, sourceOutputPort, destination.handle, destinationInputPort);
        }

        // Token: 0x06001101 RID: 4353 RVA: 0x00016BD0 File Offset: 0x00014DD0
        private static bool ConnectInternal(
            ref PlayableGraph graph,
            PlayableHandle source,
            int sourceOutputPort,
            PlayableHandle destination,
            int destinationInputPort)
        {
            return LegacyPlayableRuntime.Connect(
                graph,
                source,
                sourceOutputPort,
                destination,
                destinationInputPort);
        }

        /// <summary>
        ///   <para>Disconnects PlayableHandle.  The connections determine the topology of the PlayableGraph and how its is evaluated.</para>
        /// </summary>
        /// <param name="playable">The source playabe or its handle.</param>
        /// <param name="inputPort">The port used in the source playable.</param>
        // Token: 0x06001103 RID: 4355 RVA: 0x00016BF4 File Offset: 0x00014DF4
        public void Disconnect(Playable playable, int inputPort)
        {
            PlayableHandle handle = playable.handle;
            PlayableGraph.DisconnectInternal(ref this, ref handle, inputPort);
        }

        /// <summary>
        ///   <para>Disconnects PlayableHandle.  The connections determine the topology of the PlayableGraph and how its is evaluated.</para>
        /// </summary>
        /// <param name="playable">The source playabe or its handle.</param>
        /// <param name="inputPort">The port used in the source playable.</param>
        // Token: 0x06001104 RID: 4356 RVA: 0x00016C14 File Offset: 0x00014E14
        public void Disconnect(PlayableHandle playable, int inputPort)
        {
            PlayableGraph.DisconnectInternal(ref this, ref playable, inputPort);
        }

        // Token: 0x06001105 RID: 4357 RVA: 0x00016C20 File Offset: 0x00014E20
        private static void DisconnectInternal(
            ref PlayableGraph graph,
            ref PlayableHandle playable,
            int inputPort)
        {
            LegacyPlayableRuntime.Disconnect(graph, playable, inputPort);
        }

        /// <summary>
        ///   <para>Destroys the Playable associated with this PlayableHandle.</para>
        /// </summary>
        /// <param name="playable">The playable to destroy.</param>
        // Token: 0x06001107 RID: 4359 RVA: 0x00016C2C File Offset: 0x00014E2C
        public void DestroyPlayable(PlayableHandle playable)
        {
            LegacyPlayableRuntime.DestroyPlayable(this, ref playable);
        }

        private static void InternalDestroyPlayable(
            ref PlayableGraph graph,
            ref PlayableHandle playable)
        {
            LegacyPlayableRuntime.DestroyPlayable(graph, ref playable);
        }

        /// <summary>
        ///   <para>Destroys the PlayableOutput.</para>
        /// </summary>
        /// <param name="output">The output to destroy.</param>
        // Token: 0x0600110A RID: 4362 RVA: 0x00016C44 File Offset: 0x00014E44
        public void DestroyOutput(ScriptPlayableOutput output)
        {
            PlayableGraph.InternalDestroyOutput(ref this, ref output.m_Output);
        }

        // Token: 0x0600110B RID: 4363

        internal static void InternalDestroyOutput(
            ref PlayableGraph graph,
            ref PlayableOutput output)
        {
            LegacyPlayableRuntime.DestroyOutput(graph, ref output);
        }

        /// <summary>
        ///   <para>Recursively destroys the given Playable and all children connected to its inputs.</para>
        /// </summary>
        /// <param name="playable">The playable to destroy.</param>
        // Token: 0x0600110C RID: 4364 RVA: 0x00016C54 File Offset: 0x00014E54
        public void DestroySubgraph(PlayableHandle playable)
        {
            PlayableGraph.InternalDestroySubgraph(ref this, playable);
        }

        // Token: 0x0600110D RID: 4365 RVA: 0x00016C60 File Offset: 0x00014E60
        private static void InternalDestroySubgraph(
            ref PlayableGraph graph,
            PlayableHandle playable)
        {
            LegacyPlayableRuntime.DestroySubgraph(graph, playable);
        }

        // Token: 0x0600110F RID: 4367 RVA: 0x00016C6C File Offset: 0x00014E6C
        [ExcludeFromDocs]
        public void Evaluate()
        {
            float deltaTime = 0f;
            this.Evaluate(deltaTime);
        }

        /// <summary>
        ///   <para>Evaluates all the PlayableOutputs in the graph, and updates all the connected Playables in the graph.</para>
        /// </summary>
        /// <param name="deltaTime">The time in seconds by which to advance each Playable in the graph.</param>
        // Token: 0x06001110 RID: 4368 RVA: 0x00016C88 File Offset: 0x00014E88
        public void Evaluate([DefaultValue("0")] float deltaTime)
        {
            PlayableGraph.InternalEvaluate(ref this, deltaTime);
        }

        // Token: 0x06001111 RID: 4369

        internal static void InternalEvaluate(
            ref PlayableGraph graph,
            float deltaTime)
        {
            LegacyPlayableRuntime.Evaluate(graph, deltaTime);
        }

        /// <summary>
        ///   <para>Returns the number of PlayableHandle owned by the Graph that have no connected outputs.</para>
        /// </summary>
        // Token: 0x170003B2 RID: 946
        // (get) Token: 0x06001112 RID: 4370 RVA: 0x00016C94 File Offset: 0x00014E94
        public int rootPlayableCount
        {
            get
            {
                return PlayableGraph.InternalRootPlayableCount(ref this);
            }
        }

        // Token: 0x06001113 RID: 4371

        internal static int InternalRootPlayableCount(ref PlayableGraph graph)
        {
            return LegacyPlayableRuntime.GetRootPlayableCount(graph);
        }

        /// <summary>
        ///   <para>Returns the PlayableHandle with no output connections at the given index.</para>
        /// </summary>
        /// <param name="index">The index of the root PlayableHandle.</param>
        // Token: 0x06001114 RID: 4372 RVA: 0x00016CB0 File Offset: 0x00014EB0
        public PlayableHandle GetRootPlayable(int index)
        {
            PlayableHandle @null = PlayableHandle.Null;
            PlayableGraph.InternalGetRootPlayable(index, ref this, ref @null);
            return @null;
        }

        // Token: 0x06001115 RID: 4373 RVA: 0x00016CD8 File Offset: 0x00014ED8
        internal static void InternalGetRootPlayable(
            int index,
            ref PlayableGraph graph,
            ref PlayableHandle handle)
        {
            handle = LegacyPlayableRuntime.GetRootPlayable(graph, index);
        }

        // Token: 0x04000235 RID: 565
        internal IntPtr m_Handle;

        // Token: 0x04000236 RID: 566
        internal int m_Version;
    }

    internal static class LegacyPlayableRuntime
    {
        private struct ConnectionKey : IEquatable<ConnectionKey>
        {
            internal long DestinationId;
            internal int InputPort;

            public bool Equals(ConnectionKey other)
            {
                return DestinationId == other.DestinationId &&
                       InputPort == other.InputPort;
            }

            public override bool Equals(object obj)
            {
                return obj is ConnectionKey && Equals((ConnectionKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (DestinationId.GetHashCode() * 397) ^ InputPort;
                }
            }
        }

        private sealed class ConnectionState
        {
            internal long SourceId;
            internal int SourceOutputPort;
        }

        private sealed class GraphState
        {
            internal int Version;
            internal bool IsPlaying;
            internal bool IsDone;
            internal double Time;
            internal IExposedPropertyTable Resolver;

            internal readonly HashSet<long> Playables = new HashSet<long>();
            internal readonly HashSet<long> Outputs = new HashSet<long>();

            internal readonly Dictionary<ConnectionKey, ConnectionState> Connections =
                new Dictionary<ConnectionKey, ConnectionState>();
        }

        private sealed class PlayableState
        {
            internal int Version;
            internal long GraphId;
            internal int InputCount;
            internal int OutputCount;
            internal PlayState PlayState = PlayState.Paused;
            internal double Speed = 1.0;
            internal double Time;
            internal double Duration;
            internal bool IsDone;
            internal bool PropagateSetTime = true;
            internal object ScriptInstance;
            internal Type PlayableType;

            internal AnimationClip AnimationClip;
            internal bool AnimationApplyFootIK;
            internal bool AnimationRemoveStartOffset;
            internal bool AnimationMixerNormalizeWeights;
            internal bool AnimationLayerMixer;
            internal bool AnimationMotionXToDelta;
            internal RuntimeAnimatorController AnimatorController;
            internal Vector3 AnimationOffsetPosition;
            internal Quaternion AnimationOffsetRotation = Quaternion.identity;

            internal readonly Dictionary<int, float> AnimatorFloats =
                new Dictionary<int, float>();
            internal readonly Dictionary<int, bool> AnimatorBools =
                new Dictionary<int, bool>();
            internal readonly Dictionary<int, int> AnimatorIntegers =
                new Dictionary<int, int>();
            internal readonly HashSet<int> AnimatorTriggers =
                new HashSet<int>();
            internal readonly Dictionary<int, float> AnimatorLayerWeights =
                new Dictionary<int, float>();
            internal readonly Dictionary<int, string> AnimatorResolvedHashes =
                new Dictionary<int, string>();

            internal int AnimatorStateHash;
            internal int AnimatorStateLayer = -1;
            internal float AnimatorStateTime = float.NegativeInfinity;
            internal bool AnimatorTransitioning;

            internal readonly Dictionary<int, float> InputWeights =
                new Dictionary<int, float>();
        }

        private enum OutputKind
        {
            Script,
            Animation,
            Audio
        }

        private sealed class OutputState
        {
            internal int Version;
            internal long GraphId;
            internal string Name;
            internal OutputKind Kind;
            internal UnityEngine.Object ReferenceObject;
            internal UnityEngine.Object UserData;
            internal PlayableHandle SourcePlayable;
            internal int SourceInputPort;
            internal float Weight = 1f;
        }

        private static readonly Dictionary<long, GraphState> s_Graphs =
            new Dictionary<long, GraphState>();

        private static readonly Dictionary<long, PlayableState> s_Playables =
            new Dictionary<long, PlayableState>();

        private static readonly Dictionary<long, OutputState> s_Outputs =
            new Dictionary<long, OutputState>();

        private static long s_NextGraphId = 1;
        private static long s_NextPlayableId = 1;
        private static long s_NextOutputId = 1;
        private static int s_NextVersion = 1;

        internal static PlayableGraph CreateGraph()
        {
            long id = s_NextGraphId++;
            int version = s_NextVersion++;

            GraphState state = new GraphState();
            state.Version = version;
            s_Graphs.Add(id, state);

            PlayableGraph graph = default(PlayableGraph);
            graph.m_Handle = new IntPtr(id);
            graph.m_Version = version;
            return graph;
        }

        internal static bool IsGraphValid(PlayableGraph graph)
        {
            GraphState state;
            return TryGetGraph(graph, out state);
        }

        internal static bool IsDone(PlayableGraph graph)
        {
            GraphState state;
            return TryGetGraph(graph, out state) && state.IsDone;
        }

        internal static IExposedPropertyTable GetResolver(PlayableGraph graph)
        {
            GraphState state;
            return TryGetGraph(graph, out state) ? state.Resolver : null;
        }

        internal static void SetResolver(
            PlayableGraph graph,
            IExposedPropertyTable resolver)
        {
            GraphState state = GetGraphOrThrow(graph);
            state.Resolver = resolver;
        }

        internal static void Play(PlayableGraph graph)
        {
            GraphState state = GetGraphOrThrow(graph);
            state.IsPlaying = true;
            state.IsDone = false;

            long[] playableIds = new long[state.Playables.Count];
            state.Playables.CopyTo(playableIds);

            for (int i = 0; i < playableIds.Length; ++i)
            {
                PlayableState playableState;
                if (!s_Playables.TryGetValue(
                    playableIds[i],
                    out playableState))
                {
                    continue;
                }

                if (!playableState.IsDone)
                    playableState.PlayState = PlayState.Playing;
            }
        }

        internal static void Stop(PlayableGraph graph)
        {
            GraphState state = GetGraphOrThrow(graph);
            state.IsPlaying = false;

            long[] playableIds = new long[state.Playables.Count];
            state.Playables.CopyTo(playableIds);

            for (int i = 0; i < playableIds.Length; ++i)
            {
                PlayableState playableState;
                if (s_Playables.TryGetValue(
                    playableIds[i],
                    out playableState))
                {
                    playableState.PlayState = PlayState.Paused;
                }
            }
        }

        internal static int GetPlayableCount(PlayableGraph graph)
        {
            GraphState state;
            return TryGetGraph(graph, out state) ? state.Playables.Count : 0;
        }

        internal static void Evaluate(PlayableGraph graph, float deltaTime)
        {
            GraphState state = GetGraphOrThrow(graph);

            if (deltaTime < 0f)
                throw new ArgumentOutOfRangeException(
                    "deltaTime",
                    "PlayableGraph cannot be evaluated with a negative delta time.");

            state.Time += deltaTime;
            bool hasPlayable = false;
            bool allDone = true;

            foreach (long playableId in state.Playables)
            {
                PlayableState playableState;
                if (!s_Playables.TryGetValue(playableId, out playableState))
                    continue;

                hasPlayable = true;

                if (playableState.PlayState == PlayState.Playing &&
                    !playableState.IsDone)
                {
                    playableState.Time += deltaTime * playableState.Speed;

                    if (playableState.Duration >= 0.0 &&
                        playableState.Time >= playableState.Duration)
                    {
                        playableState.Time = playableState.Duration;
                        playableState.IsDone = true;
                        playableState.PlayState = PlayState.Paused;
                    }
                }

                if (!playableState.IsDone)
                    allDone = false;
            }

            state.IsDone = hasPlayable && allDone;

            EvaluateAnimationOutputs(state);
        }

        private struct AnimationSampleCandidate
        {
            internal bool IsValid;
            internal long PlayableId;
            internal AnimationClip Clip;
            internal double Time;
            internal float Weight;
            internal Vector3 PositionOffset;
            internal Quaternion RotationOffset;
        }

        private static void EvaluateAnimationOutputs(
            GraphState graphState)
        {
            if (graphState.Outputs.Count == 0)
                return;

            long[] outputIds = new long[graphState.Outputs.Count];
            graphState.Outputs.CopyTo(outputIds);
            Array.Sort(outputIds);

            for (int i = 0; i < outputIds.Length; ++i)
            {
                OutputState outputState;
                if (!s_Outputs.TryGetValue(
                    outputIds[i],
                    out outputState))
                {
                    continue;
                }

                if (outputState.Kind != OutputKind.Animation)
                    continue;

                if (outputState.Weight <= 0f)
                    continue;

                Animator animator =
                    outputState.ReferenceObject as Animator;

                if (animator == null)
                    continue;

                if (!outputState.SourcePlayable.IsValid())
                    continue;

                AnimationSampleCandidate candidate =
                    default(AnimationSampleCandidate);

                HashSet<long> visited = new HashSet<long>();

                FindBestAnimationSample(
                    graphState,
                    outputState.SourcePlayable.m_Handle.ToInt64(),
                    outputState.Weight,
                    Vector3.zero,
                    Quaternion.identity,
                    visited,
                    ref candidate);

                if (!candidate.IsValid || candidate.Clip == null)
                    continue;

                float sampleTime = ResolveAnimationSampleTime(
                    candidate.Clip,
                    candidate.Time);

                candidate.Clip.SampleAnimation(
                    animator.gameObject,
                    sampleTime);

                if (candidate.PositionOffset != Vector3.zero)
                {
                    animator.transform.localPosition +=
                        candidate.PositionOffset;
                }

                if (candidate.RotationOffset != Quaternion.identity)
                {
                    animator.transform.localRotation =
                        candidate.RotationOffset *
                        animator.transform.localRotation;
                }
            }
        }

        private static void FindBestAnimationSample(
            GraphState graphState,
            long playableId,
            float inheritedWeight,
            Vector3 inheritedPositionOffset,
            Quaternion inheritedRotationOffset,
            HashSet<long> visited,
            ref AnimationSampleCandidate best)
        {
            if (playableId == 0 || inheritedWeight <= 0f)
                return;

            if (!visited.Add(playableId))
                return;

            PlayableState playableState;
            if (!s_Playables.TryGetValue(
                playableId,
                out playableState))
            {
                return;
            }

            Vector3 positionOffset = inheritedPositionOffset;
            Quaternion rotationOffset = inheritedRotationOffset;

            if (playableState.PlayableType ==
                typeof(AnimationOffsetPlayable))
            {
                positionOffset +=
                    playableState.AnimationOffsetPosition;

                rotationOffset =
                    playableState.AnimationOffsetRotation *
                    rotationOffset;
            }

            if (playableState.AnimationClip != null)
            {
                if (!best.IsValid ||
                    inheritedWeight > best.Weight)
                {
                    best.IsValid = true;
                    best.PlayableId = playableId;
                    best.Clip = playableState.AnimationClip;
                    best.Time = playableState.Time;
                    best.Weight = inheritedWeight;
                    best.PositionOffset = positionOffset;
                    best.RotationOffset = rotationOffset;
                }

                return;
            }

            List<ConnectionKey> inputs =
                new List<ConnectionKey>();

            foreach (
                KeyValuePair<ConnectionKey, ConnectionState> pair
                in graphState.Connections)
            {
                if (pair.Key.DestinationId == playableId)
                    inputs.Add(pair.Key);
            }

            inputs.Sort(
                delegate (ConnectionKey left, ConnectionKey right)
                {
                    return left.InputPort.CompareTo(right.InputPort);
                });

            if (inputs.Count == 0)
                return;

            float totalWeight = 0f;

            if (playableState.AnimationMixerNormalizeWeights)
            {
                for (int i = 0; i < inputs.Count; ++i)
                {
                    float inputWeight;
                    if (!playableState.InputWeights.TryGetValue(
                        inputs[i].InputPort,
                        out inputWeight))
                    {
                        inputWeight = 1f;
                    }

                    if (inputWeight > 0f)
                        totalWeight += inputWeight;
                }
            }

            for (int i = 0; i < inputs.Count; ++i)
            {
                ConnectionKey key = inputs[i];
                ConnectionState connection;

                if (!graphState.Connections.TryGetValue(
                    key,
                    out connection))
                {
                    continue;
                }

                float inputWeight;
                if (!playableState.InputWeights.TryGetValue(
                    key.InputPort,
                    out inputWeight))
                {
                    inputWeight = 1f;
                }

                if (playableState.AnimationMixerNormalizeWeights &&
                    totalWeight > 0f)
                {
                    inputWeight /= totalWeight;
                }

                FindBestAnimationSample(
                    graphState,
                    connection.SourceId,
                    inheritedWeight * inputWeight,
                    positionOffset,
                    rotationOffset,
                    visited,
                    ref best);
            }
        }

        private static float ResolveAnimationSampleTime(
            AnimationClip clip,
            double playableTime)
        {
            if (clip == null || clip.length <= 0f)
                return 0f;

            float time = (float)playableTime;

            if (clip.isLooping)
                return Mathf.Repeat(time, clip.length);

            return Mathf.Clamp(time, 0f, clip.length);
        }

        internal static void DestroyGraph(ref PlayableGraph graph)
        {
            long graphId = graph.m_Handle.ToInt64();
            GraphState graphState;

            if (TryGetGraph(graph, out graphState))
            {
                long[] playableIds = new long[graphState.Playables.Count];
                graphState.Playables.CopyTo(playableIds);

                for (int i = 0; i < playableIds.Length; ++i)
                    s_Playables.Remove(playableIds[i]);

                long[] outputIds = new long[graphState.Outputs.Count];
                graphState.Outputs.CopyTo(outputIds);

                for (int i = 0; i < outputIds.Length; ++i)
                    s_Outputs.Remove(outputIds[i]);

                s_Graphs.Remove(graphId);
            }

            graph.m_Handle = IntPtr.Zero;
            graph.m_Version = 0;
        }

        internal static PlayableHandle CreatePlayable(PlayableGraph graph)
        {
            GraphState graphState;
            if (!TryGetGraph(graph, out graphState))
                return PlayableHandle.Null;

            long graphId = graph.m_Handle.ToInt64();
            long playableId = s_NextPlayableId++;
            int version = s_NextVersion++;

            PlayableState playableState = new PlayableState();
            playableState.GraphId = graphId;
            playableState.Version = version;

            s_Playables.Add(playableId, playableState);
            graphState.Playables.Add(playableId);

            PlayableHandle handle = default(PlayableHandle);
            handle.m_Handle = new IntPtr(playableId);
            handle.m_Version = version;
            return handle;
        }

        internal static bool IsPlayableValid(PlayableHandle playable)
        {
            PlayableState state;
            return TryGetPlayable(playable, out state);
        }

        internal static bool Connect(
            PlayableGraph graph,
            PlayableHandle source,
            int sourceOutputPort,
            PlayableHandle destination,
            int destinationInputPort)
        {
            if (sourceOutputPort < 0)
                throw new ArgumentOutOfRangeException("sourceOutputPort");

            if (destinationInputPort < 0)
                throw new ArgumentOutOfRangeException("destinationInputPort");

            GraphState graphState = GetGraphOrThrow(graph);
            PlayableState sourceState = GetPlayableOrThrow(source);
            PlayableState destinationState = GetPlayableOrThrow(destination);
            long graphId = graph.m_Handle.ToInt64();

            if (sourceState.GraphId != graphId ||
                destinationState.GraphId != graphId)
            {
                throw new InvalidOperationException(
                    "Both PlayableHandles must belong to this PlayableGraph.");
            }

            ConnectionKey key = new ConnectionKey();
            key.DestinationId = destination.m_Handle.ToInt64();
            key.InputPort = destinationInputPort;

            ConnectionState connection = new ConnectionState();
            connection.SourceId = source.m_Handle.ToInt64();
            connection.SourceOutputPort = sourceOutputPort;

            graphState.Connections[key] = connection;

            if (destinationState.InputCount <= destinationInputPort)
                destinationState.InputCount = destinationInputPort + 1;

            if (sourceState.OutputCount <= sourceOutputPort)
                sourceState.OutputCount = sourceOutputPort + 1;

            return true;
        }

        internal static void Disconnect(
            PlayableGraph graph,
            PlayableHandle destination,
            int inputPort)
        {
            if (inputPort < 0)
                throw new ArgumentOutOfRangeException("inputPort");

            GraphState graphState = GetGraphOrThrow(graph);
            PlayableState destinationState = GetPlayableOrThrow(destination);

            if (destinationState.GraphId != graph.m_Handle.ToInt64())
            {
                throw new InvalidOperationException(
                    "The PlayableHandle does not belong to this PlayableGraph.");
            }

            ConnectionKey key = new ConnectionKey();
            key.DestinationId = destination.m_Handle.ToInt64();
            key.InputPort = inputPort;
            graphState.Connections.Remove(key);
        }

        internal static void DestroyPlayable(
            PlayableGraph graph,
            ref PlayableHandle playable)
        {
            GraphState graphState = GetGraphOrThrow(graph);
            PlayableState playableState;

            if (!TryGetPlayable(playable, out playableState))
                return;

            long graphId = graph.m_Handle.ToInt64();
            long playableId = playable.m_Handle.ToInt64();

            if (playableState.GraphId != graphId)
            {
                throw new InvalidOperationException(
                    "The PlayableHandle does not belong to this PlayableGraph.");
            }

            RemoveConnectionsForPlayable(graphState, playableId);
            graphState.Playables.Remove(playableId);
            s_Playables.Remove(playableId);

            playable.m_Handle = IntPtr.Zero;
            playable.m_Version = 0;
        }

        internal static void DestroySubgraph(
            PlayableGraph graph,
            PlayableHandle root)
        {
            GraphState graphState = GetGraphOrThrow(graph);
            PlayableState rootState = GetPlayableOrThrow(root);

            if (rootState.GraphId != graph.m_Handle.ToInt64())
                throw new InvalidOperationException(
                    "The PlayableHandle does not belong to this PlayableGraph.");

            HashSet<long> collected = new HashSet<long>();
            CollectInputChildren(
                graphState,
                root.m_Handle.ToInt64(),
                collected);

            long[] ids = new long[collected.Count];
            collected.CopyTo(ids);

            for (int i = 0; i < ids.Length; ++i)
            {
                long id = ids[i];
                RemoveConnectionsForPlayable(graphState, id);
                graphState.Playables.Remove(id);
                s_Playables.Remove(id);
            }
        }

        internal static bool CreateOutput(
            PlayableGraph graph,
            string name,
            out PlayableOutput output)
        {
            output = default(PlayableOutput);

            GraphState graphState;
            if (!TryGetGraph(graph, out graphState))
                return false;

            long id = s_NextOutputId++;
            int version = s_NextVersion++;

            OutputState state = new OutputState();
            state.GraphId = graph.m_Handle.ToInt64();
            state.Version = version;
            state.Name = name ?? string.Empty;
            state.Kind = OutputKind.Script;
            state.SourcePlayable = PlayableHandle.Null;
            state.SourceInputPort = 0;
            state.Weight = 1f;

            s_Outputs.Add(id, state);
            graphState.Outputs.Add(id);

            output.m_Handle = new IntPtr(id);
            output.m_Version = version;
            return true;
        }

        internal static void DestroyOutput(
            PlayableGraph graph,
            ref PlayableOutput output)
        {
            GraphState graphState = GetGraphOrThrow(graph);
            long outputId = output.m_Handle.ToInt64();
            OutputState state;

            if (outputId != 0 &&
                s_Outputs.TryGetValue(outputId, out state) &&
                state.Version == output.m_Version)
            {
                if (state.GraphId != graph.m_Handle.ToInt64())
                    throw new InvalidOperationException(
                        "The PlayableOutput does not belong to this PlayableGraph.");

                graphState.Outputs.Remove(outputId);
                s_Outputs.Remove(outputId);
            }

            output.m_Handle = IntPtr.Zero;
            output.m_Version = 0;
        }

        internal static bool IsOutputValid(PlayableOutput output)
        {
            OutputState state;
            return TryGetOutput(output, out state);
        }

        internal static UnityEngine.Object GetOutputReferenceObject(
            PlayableOutput output)
        {
            return GetOutputOrThrow(output).ReferenceObject;
        }

        internal static void SetOutputReferenceObject(
            PlayableOutput output,
            UnityEngine.Object target)
        {
            GetOutputOrThrow(output).ReferenceObject = target;
        }

        internal static UnityEngine.Object GetOutputUserData(
            PlayableOutput output)
        {
            return GetOutputOrThrow(output).UserData;
        }

        internal static void SetOutputUserData(
            PlayableOutput output,
            UnityEngine.Object target)
        {
            GetOutputOrThrow(output).UserData = target;
        }

        internal static PlayableHandle GetOutputSourcePlayable(
            PlayableOutput output)
        {
            return GetOutputOrThrow(output).SourcePlayable;
        }

        internal static void SetOutputSourcePlayable(
            PlayableOutput output,
            PlayableHandle playable)
        {
            OutputState outputState = GetOutputOrThrow(output);

            if (playable.IsValid())
            {
                PlayableState playableState = GetPlayableOrThrow(playable);
                if (playableState.GraphId != outputState.GraphId)
                {
                    throw new InvalidOperationException(
                        "The source PlayableHandle and PlayableOutput must belong to the same PlayableGraph.");
                }
            }

            outputState.SourcePlayable = playable;
        }

        internal static int GetOutputSourceInputPort(
            PlayableOutput output)
        {
            return GetOutputOrThrow(output).SourceInputPort;
        }

        internal static void SetOutputSourceInputPort(
            PlayableOutput output,
            int port)
        {
            if (port < 0)
                throw new ArgumentOutOfRangeException("port");

            GetOutputOrThrow(output).SourceInputPort = port;
        }

        internal static float GetOutputWeight(PlayableOutput output)
        {
            return GetOutputOrThrow(output).Weight;
        }

        internal static void SetOutputWeight(
            PlayableOutput output,
            float weight)
        {
            GetOutputOrThrow(output).Weight = weight;
        }

        internal static int GetScriptOutputCount(PlayableGraph graph)
        {
            return GetOutputCount(graph, OutputKind.Script);
        }

        internal static bool GetScriptOutput(
            PlayableGraph graph,
            int index,
            out PlayableOutput output)
        {
            return GetOutput(
                graph,
                index,
                OutputKind.Script,
                out output);
        }

        internal static bool CreateAnimationOutput(
            PlayableGraph graph,
            string name,
            out PlayableOutput output)
        {
            return CreateTypedOutput(
                graph,
                name,
                OutputKind.Animation,
                out output);
        }

        internal static int GetAnimationOutputCount(
            PlayableGraph graph)
        {
            return GetOutputCount(graph, OutputKind.Animation);
        }

        internal static bool GetAnimationOutput(
            PlayableGraph graph,
            int index,
            out PlayableOutput output)
        {
            return GetOutput(
                graph,
                index,
                OutputKind.Animation,
                out output);
        }

        private static bool CreateTypedOutput(
            PlayableGraph graph,
            string name,
            OutputKind kind,
            out PlayableOutput output)
        {
            output = default(PlayableOutput);

            GraphState graphState;
            if (!TryGetGraph(graph, out graphState))
                return false;

            long id = s_NextOutputId++;
            int version = s_NextVersion++;

            OutputState state = new OutputState();
            state.GraphId = graph.m_Handle.ToInt64();
            state.Version = version;
            state.Name = name ?? string.Empty;
            state.Kind = kind;
            state.SourcePlayable = PlayableHandle.Null;
            state.SourceInputPort = 0;
            state.Weight = 1f;

            s_Outputs.Add(id, state);
            graphState.Outputs.Add(id);

            output.m_Handle = new IntPtr(id);
            output.m_Version = version;
            return true;
        }

        private static int GetOutputCount(
            PlayableGraph graph,
            OutputKind kind)
        {
            GraphState graphState = GetGraphOrThrow(graph);
            int count = 0;

            foreach (long outputId in graphState.Outputs)
            {
                OutputState state;

                if (s_Outputs.TryGetValue(outputId, out state) &&
                    state.Kind == kind)
                {
                    ++count;
                }
            }

            return count;
        }

        private static bool GetOutput(
            PlayableGraph graph,
            int index,
            OutputKind kind,
            out PlayableOutput output)
        {
            output = default(PlayableOutput);

            GraphState graphState;
            if (!TryGetGraph(graph, out graphState))
                return false;

            List<long> outputIds = new List<long>();

            foreach (long outputId in graphState.Outputs)
            {
                OutputState candidate;

                if (s_Outputs.TryGetValue(outputId, out candidate) &&
                    candidate.Kind == kind)
                {
                    outputIds.Add(outputId);
                }
            }

            outputIds.Sort();

            if (index < 0 || index >= outputIds.Count)
                return false;

            long id = outputIds[index];
            OutputState state = s_Outputs[id];

            output.m_Handle = new IntPtr(id);
            output.m_Version = state.Version;
            return true;
        }

        internal static AnimationClip GetAnimationClip(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationClip;
        }

        internal static void SetAnimationClip(
            PlayableHandle playable,
            AnimationClip clip)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimationClip = clip;

            if (clip != null)
                state.Duration = clip.length;
        }

        internal static bool GetAnimationApplyFootIK(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationApplyFootIK;
        }

        internal static void SetAnimationApplyFootIK(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimationApplyFootIK = value;
        }

        internal static bool GetAnimationRemoveStartOffset(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationRemoveStartOffset;
        }

        internal static void SetAnimationRemoveStartOffset(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimationRemoveStartOffset = value;
        }

        internal static void SetAnimationMixerNormalizeWeights(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimationMixerNormalizeWeights = value;
        }

        internal static bool GetAnimationMixerNormalizeWeights(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationMixerNormalizeWeights;
        }

        internal static void SetAnimationLayerMixer(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimationLayerMixer = value;
        }

        internal static void SetAnimationMotionXToDelta(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimationMotionXToDelta = value;
        }

        internal static void SetAnimatorController(
            PlayableHandle playable,
            RuntimeAnimatorController controller)
        {
            GetPlayableOrThrow(playable).AnimatorController = controller;
        }

        internal static RuntimeAnimatorController GetAnimatorController(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimatorController;
        }

        internal static void SetAnimationOffset(
            PlayableHandle playable,
            Vector3 position,
            Quaternion rotation)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimationOffsetPosition = position;
            state.AnimationOffsetRotation = rotation;
        }

        internal static Vector3 GetAnimationOffsetPosition(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationOffsetPosition;
        }

        internal static Quaternion GetAnimationOffsetRotation(
            PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).AnimationOffsetRotation;
        }

        internal static void SetAnimationOffsetPosition(
            PlayableHandle playable,
            Vector3 position)
        {
            GetPlayableOrThrow(playable).AnimationOffsetPosition = position;
        }

        internal static void SetAnimationOffsetRotation(
            PlayableHandle playable,
            Quaternion rotation)
        {
            GetPlayableOrThrow(playable).AnimationOffsetRotation = rotation;
        }

        internal static float GetAnimatorFloat(
            PlayableHandle playable,
            int id)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            float value;
            return state.AnimatorFloats.TryGetValue(id, out value)
                ? value
                : 0f;
        }

        internal static void SetAnimatorFloat(
            PlayableHandle playable,
            int id,
            float value)
        {
            GetPlayableOrThrow(playable).AnimatorFloats[id] = value;
        }

        internal static bool GetAnimatorBool(
            PlayableHandle playable,
            int id)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            bool value;
            return state.AnimatorBools.TryGetValue(id, out value) && value;
        }

        internal static void SetAnimatorBool(
            PlayableHandle playable,
            int id,
            bool value)
        {
            GetPlayableOrThrow(playable).AnimatorBools[id] = value;
        }

        internal static int GetAnimatorInteger(
            PlayableHandle playable,
            int id)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            int value;
            return state.AnimatorIntegers.TryGetValue(id, out value)
                ? value
                : 0;
        }

        internal static void SetAnimatorInteger(
            PlayableHandle playable,
            int id,
            int value)
        {
            GetPlayableOrThrow(playable).AnimatorIntegers[id] = value;
        }

        internal static void SetAnimatorTrigger(
            PlayableHandle playable,
            int id)
        {
            GetPlayableOrThrow(playable).AnimatorTriggers.Add(id);
        }

        internal static void ResetAnimatorTrigger(
            PlayableHandle playable,
            int id)
        {
            GetPlayableOrThrow(playable).AnimatorTriggers.Remove(id);
        }

        internal static bool IsAnimatorParameterControlledByCurve(
            PlayableHandle playable,
            int id)
        {
            return false;
        }

        internal static int GetAnimatorLayerCount(
            PlayableHandle playable)
        {
            RuntimeAnimatorController controller =
                GetPlayableOrThrow(playable).AnimatorController;

            return controller != null ? 1 : 0;
        }

        internal static string GetAnimatorLayerName(
            PlayableHandle playable,
            int layerIndex)
        {
            if (layerIndex != 0 || GetAnimatorLayerCount(playable) == 0)
                throw new ArgumentOutOfRangeException("layerIndex");

            return "Base Layer";
        }

        internal static int GetAnimatorLayerIndex(
            PlayableHandle playable,
            string layerName)
        {
            if (String.Equals(layerName, "Base Layer"))
                return 0;

            return -1;
        }

        internal static float GetAnimatorLayerWeight(
            PlayableHandle playable,
            int layerIndex)
        {
            if (layerIndex < 0)
                throw new ArgumentOutOfRangeException("layerIndex");

            PlayableState state = GetPlayableOrThrow(playable);
            float weight;

            if (state.AnimatorLayerWeights.TryGetValue(
                layerIndex,
                out weight))
            {
                return weight;
            }

            return layerIndex == 0 ? 1f : 0f;
        }

        internal static void SetAnimatorLayerWeight(
            PlayableHandle playable,
            int layerIndex,
            float weight)
        {
            if (layerIndex < 0)
                throw new ArgumentOutOfRangeException("layerIndex");

            GetPlayableOrThrow(playable).AnimatorLayerWeights[layerIndex] =
                weight;
        }

        internal static string ResolveAnimatorHash(
            PlayableHandle playable,
            int hash)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            string value;

            return state.AnimatorResolvedHashes.TryGetValue(hash, out value)
                ? value
                : hash.ToString();
        }

        internal static bool IsAnimatorInTransition(
            PlayableHandle playable,
            int layerIndex)
        {
            return GetPlayableOrThrow(playable).AnimatorTransitioning;
        }

        internal static int GetAnimatorParameterCount(
            PlayableHandle playable)
        {
            return GetAnimatorParameters(playable).Length;
        }

        internal static AnimatorControllerParameter[] GetAnimatorParameters(
            PlayableHandle playable)
        {
            RuntimeAnimatorController controller =
                GetPlayableOrThrow(playable).AnimatorController;

            return new AnimatorControllerParameter[0];
        }

        internal static void AnimatorCrossFadeInFixedTime(
            PlayableHandle playable,
            int stateNameHash,
            float transitionDuration,
            int layer,
            float fixedTime)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimatorStateHash = stateNameHash;
            state.AnimatorStateLayer = layer;
            state.AnimatorStateTime = fixedTime;
            state.AnimatorTransitioning = transitionDuration > 0f;
        }

        internal static void AnimatorCrossFade(
            PlayableHandle playable,
            int stateNameHash,
            float transitionDuration,
            int layer,
            float normalizedTime)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimatorStateHash = stateNameHash;
            state.AnimatorStateLayer = layer;
            state.AnimatorStateTime = normalizedTime;
            state.AnimatorTransitioning = transitionDuration > 0f;
        }

        internal static void AnimatorPlayInFixedTime(
            PlayableHandle playable,
            int stateNameHash,
            int layer,
            float fixedTime)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimatorStateHash = stateNameHash;
            state.AnimatorStateLayer = layer;
            state.AnimatorStateTime = fixedTime;
            state.AnimatorTransitioning = false;
        }

        internal static void AnimatorPlay(
            PlayableHandle playable,
            int stateNameHash,
            int layer,
            float normalizedTime)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.AnimatorStateHash = stateNameHash;
            state.AnimatorStateLayer = layer;
            state.AnimatorStateTime = normalizedTime;
            state.AnimatorTransitioning = false;
        }

        internal static bool AnimatorHasState(
            PlayableHandle playable,
            int layerIndex,
            int stateID)
        {
            PlayableState state = GetPlayableOrThrow(playable);

            return state.AnimatorStateHash == stateID &&
                   (state.AnimatorStateLayer == layerIndex ||
                    state.AnimatorStateLayer == -1);
        }

        internal static int GetRootPlayableCount(PlayableGraph graph)
        {
            return GetRootPlayables(graph).Count;
        }

        internal static PlayableHandle GetRootPlayable(
            PlayableGraph graph,
            int index)
        {
            List<long> roots = GetRootPlayables(graph);

            if (index < 0 || index >= roots.Count)
                throw new ArgumentOutOfRangeException("index");

            long id = roots[index];
            PlayableState state = s_Playables[id];

            PlayableHandle handle = default(PlayableHandle);
            handle.m_Handle = new IntPtr(id);
            handle.m_Version = state.Version;
            return handle;
        }

        private static List<long> GetRootPlayables(PlayableGraph graph)
        {
            GraphState graphState = GetGraphOrThrow(graph);
            HashSet<long> sourcesWithOutput = new HashSet<long>();

            foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                     in graphState.Connections)
            {
                sourcesWithOutput.Add(pair.Value.SourceId);
            }

            List<long> roots = new List<long>();
            foreach (long playableId in graphState.Playables)
            {
                if (!sourcesWithOutput.Contains(playableId))
                    roots.Add(playableId);
            }

            roots.Sort();
            return roots;
        }

        private static void CollectInputChildren(
            GraphState graphState,
            long playableId,
            HashSet<long> collected)
        {
            if (!collected.Add(playableId))
                return;

            foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                     in graphState.Connections)
            {
                if (pair.Key.DestinationId == playableId)
                {
                    CollectInputChildren(
                        graphState,
                        pair.Value.SourceId,
                        collected);
                }
            }
        }

        private static void RemoveConnectionsForPlayable(
            GraphState graphState,
            long playableId)
        {
            List<ConnectionKey> keys = new List<ConnectionKey>();

            foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                     in graphState.Connections)
            {
                if (pair.Key.DestinationId == playableId ||
                    pair.Value.SourceId == playableId)
                {
                    keys.Add(pair.Key);
                }
            }

            for (int i = 0; i < keys.Count; ++i)
                graphState.Connections.Remove(keys[i]);
        }

        internal static object GetScriptInstance(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).ScriptInstance;
        }

        internal static void SetScriptInstance(
            PlayableHandle playable,
            object scriptInstance)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            state.ScriptInstance = scriptInstance;
            if (scriptInstance != null)
                state.PlayableType = scriptInstance.GetType();
        }

        internal static Type GetPlayableType(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).PlayableType;
        }

        internal static void SetPlayableType(
            PlayableHandle playable,
            Type playableType)
        {
            GetPlayableOrThrow(playable).PlayableType = playableType;
        }

        internal static PlayableGraph GetGraph(PlayableHandle playable)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            GraphState graphState;

            if (!s_Graphs.TryGetValue(state.GraphId, out graphState))
                return default(PlayableGraph);

            PlayableGraph graph = default(PlayableGraph);
            graph.m_Handle = new IntPtr(state.GraphId);
            graph.m_Version = graphState.Version;
            return graph;
        }

        internal static int GetInputCount(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).InputCount;
        }

        internal static void SetInputCount(PlayableHandle playable, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            PlayableState state = GetPlayableOrThrow(playable);
            state.InputCount = count;

            List<int> remove = new List<int>();
            foreach (int index in state.InputWeights.Keys)
            {
                if (index >= count)
                    remove.Add(index);
            }
            for (int i = 0; i < remove.Count; ++i)
                state.InputWeights.Remove(remove[i]);
        }

        internal static int GetOutputCount(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).OutputCount;
        }

        internal static void SetOutputCount(PlayableHandle playable, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            GetPlayableOrThrow(playable).OutputCount = count;
        }

        internal static PlayState GetPlayState(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).PlayState;
        }

        internal static void SetPlayState(
            PlayableHandle playable,
            PlayState playState)
        {
            GetPlayableOrThrow(playable).PlayState = playState;
        }

        internal static double GetSpeed(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).Speed;
        }

        internal static void SetSpeed(PlayableHandle playable, double speed)
        {
            if (Double.IsNaN(speed) || Double.IsInfinity(speed))
                throw new ArgumentOutOfRangeException("speed");
            GetPlayableOrThrow(playable).Speed = speed;
        }

        internal static double GetTime(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).Time;
        }

        internal static void SetTime(PlayableHandle playable, double time)
        {
            if (Double.IsNaN(time) || Double.IsInfinity(time))
                throw new ArgumentOutOfRangeException("time");

            PlayableState state = GetPlayableOrThrow(playable);
            state.Time = time;
            state.IsDone = state.Duration >= 0.0 && time >= state.Duration;

            if (state.PropagateSetTime)
            {
                PlayableGraph graph = GetGraph(playable);
                GraphState graphState = GetGraphOrThrow(graph);
                long destinationId = playable.m_Handle.ToInt64();

                foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                         in graphState.Connections)
                {
                    if (pair.Key.DestinationId != destinationId)
                        continue;

                    PlayableState inputState;
                    if (s_Playables.TryGetValue(pair.Value.SourceId, out inputState))
                        inputState.Time = time;
                }
            }
        }

        internal static bool GetDone(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).IsDone;
        }

        internal static void SetDone(PlayableHandle playable, bool isDone)
        {
            GetPlayableOrThrow(playable).IsDone = isDone;
        }

        internal static double GetDuration(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).Duration;
        }

        internal static void SetDuration(
            PlayableHandle playable,
            double duration)
        {
            if (Double.IsNaN(duration) || duration < 0.0)
                throw new ArgumentOutOfRangeException("duration");

            PlayableState state = GetPlayableOrThrow(playable);
            state.Duration = duration;
            state.IsDone = state.Time >= duration;
        }

        internal static bool GetPropagateSetTime(PlayableHandle playable)
        {
            return GetPlayableOrThrow(playable).PropagateSetTime;
        }

        internal static void SetPropagateSetTime(
            PlayableHandle playable,
            bool value)
        {
            GetPlayableOrThrow(playable).PropagateSetTime = value;
        }

        internal static bool CanChangeInputs(PlayableHandle playable)
        {
            return IsPlayableValid(playable);
        }

        internal static bool CanSetWeights(PlayableHandle playable)
        {
            return IsPlayableValid(playable);
        }

        internal static bool CanDestroy(PlayableHandle playable)
        {
            return IsPlayableValid(playable);
        }

        internal static PlayableHandle GetInput(
            PlayableHandle playable,
            int index)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            if (index < 0 || index >= state.InputCount)
                throw new IndexOutOfRangeException("Invalid input index.");

            PlayableGraph graph = GetGraph(playable);
            GraphState graphState = GetGraphOrThrow(graph);
            ConnectionKey key = new ConnectionKey();
            key.DestinationId = playable.m_Handle.ToInt64();
            key.InputPort = index;

            ConnectionState connection;
            if (!graphState.Connections.TryGetValue(key, out connection))
                return PlayableHandle.Null;

            return CreateHandle(connection.SourceId);
        }

        internal static PlayableHandle GetOutput(
            PlayableHandle playable,
            int index)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            if (index < 0 || index >= state.OutputCount)
                throw new IndexOutOfRangeException("Invalid output index.");

            PlayableGraph graph = GetGraph(playable);
            GraphState graphState = GetGraphOrThrow(graph);
            long sourceId = playable.m_Handle.ToInt64();
            int current = 0;

            foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                     in graphState.Connections)
            {
                if (pair.Value.SourceId != sourceId)
                    continue;

                if (current == index)
                    return CreateHandle(pair.Key.DestinationId);

                ++current;
            }

            return PlayableHandle.Null;
        }

        internal static void SetInputWeight(
            PlayableHandle playable,
            int index,
            float weight)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            if (index < 0 || index >= state.InputCount)
                throw new IndexOutOfRangeException("Invalid input index.");
            state.InputWeights[index] = weight;
        }

        internal static float GetInputWeight(
            PlayableHandle playable,
            int index)
        {
            PlayableState state = GetPlayableOrThrow(playable);
            if (index < 0 || index >= state.InputCount)
                throw new IndexOutOfRangeException("Invalid input index.");

            float weight;
            return state.InputWeights.TryGetValue(index, out weight)
                ? weight
                : 0f;
        }

        internal static void SetInputWeight(
            PlayableHandle playable,
            PlayableHandle input,
            float weight)
        {
            PlayableState destinationState = GetPlayableOrThrow(playable);
            GetPlayableOrThrow(input);

            PlayableGraph graph = GetGraph(playable);
            GraphState graphState = GetGraphOrThrow(graph);
            long destinationId = playable.m_Handle.ToInt64();
            long inputId = input.m_Handle.ToInt64();

            foreach (KeyValuePair<ConnectionKey, ConnectionState> pair
                     in graphState.Connections)
            {
                if (pair.Key.DestinationId == destinationId &&
                    pair.Value.SourceId == inputId)
                {
                    destinationState.InputWeights[pair.Key.InputPort] = weight;
                    return;
                }
            }

            throw new InvalidOperationException(
                "The supplied PlayableHandle is not connected as an input.");
        }

        private static PlayableHandle CreateHandle(long playableId)
        {
            PlayableState state;
            if (!s_Playables.TryGetValue(playableId, out state))
                return PlayableHandle.Null;

            PlayableHandle handle = default(PlayableHandle);
            handle.m_Handle = new IntPtr(playableId);
            handle.m_Version = state.Version;
            return handle;
        }

        private static bool TryGetGraph(
            PlayableGraph graph,
            out GraphState state)
        {
            long id = graph.m_Handle.ToInt64();
            var ss = s_Graphs.TryGetValue(id, out state);
            return id != 0 &&
                   ss &&
                   state.Version == graph.m_Version;
        }

        private static GraphState GetGraphOrThrow(PlayableGraph graph)
        {
            GraphState state;

            if (!TryGetGraph(graph, out state))
                throw new InvalidOperationException(
                    "The PlayableGraph is invalid or has been destroyed.");

            return state;
        }

        private static bool TryGetOutput(
            PlayableOutput output,
            out OutputState state)
        {
            long id = output.m_Handle.ToInt64();
            var ss = s_Outputs.TryGetValue(id, out state);
            return id != 0 &&
                   ss &&
                   state.Version == output.m_Version;
        }

        private static OutputState GetOutputOrThrow(
            PlayableOutput output)
        {
            OutputState state;

            if (!TryGetOutput(output, out state))
            {
                throw new InvalidOperationException(
                    "The PlayableOutput is invalid or has been destroyed.");
            }

            return state;
        }

        private static bool TryGetPlayable(
            PlayableHandle playable,
            out PlayableState state)
        {
            long id = playable.m_Handle.ToInt64();
            var ss = s_Playables.TryGetValue(id, out state);

            return id != 0 &&
                   ss &&
                   state.Version == playable.m_Version;
        }

        private static PlayableState GetPlayableOrThrow(
            PlayableHandle playable)
        {
            PlayableState state;

            if (!TryGetPlayable(playable, out state))
                throw new InvalidOperationException(
                    "The PlayableHandle is invalid or has been destroyed.");

            return state;
        }
    }

}

namespace UnityEngine.Playables
{
    /// <summary>
    /// Callback contract for managed ScriptPlayable instances.
    /// </summary>
    public interface IScriptPlayable
    {
        /// <summary>
        /// Called when the owning PlayableGraph starts playing.
        /// </summary>
        void OnGraphStart();

        /// <summary>
        /// Called when the owning PlayableGraph stops.
        /// </summary>
        void OnGraphStop();

        /// <summary>
        /// Called before processing the current graph frame.
        /// </summary>
        void PrepareFrame(FrameData info);

        /// <summary>
        /// Called during output processing for the current graph frame.
        /// </summary>
        void ProcessFrame(FrameData info, object playerData);

        /// <summary>
        /// Called whenever the PlayState changes.
        /// </summary>
        void OnPlayStateChanged(FrameData info, PlayState newState);
    }
}

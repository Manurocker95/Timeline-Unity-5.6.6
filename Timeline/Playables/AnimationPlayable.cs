namespace UnityEngine.Playables
{
    /// <summary>
    /// Base class for managed animation playables.
    /// </summary>
    public class AnimationPlayable : Playable
    {
        public new AnimationPlayable GetInput(int inputPort)
        {
            if (!handle.IsValid())
                return null;

            PlayableHandle input = handle.GetInput(inputPort);
            return input.IsValid()
                ? input.GetObject<AnimationPlayable>()
                : null;
        }
    }
}

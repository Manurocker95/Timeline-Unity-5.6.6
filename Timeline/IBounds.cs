using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Timeline
{
    public interface IBounds
    {
        Rect boundingRect { get; }
    }
}
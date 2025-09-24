using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualPhenix
{
    public interface IExposedPropertyTable2
    {
        void SetReferenceValue(PropertyName id, Object value);

        Object GetReferenceValue(PropertyName id, out bool idValid);

        void ClearReferenceValue(PropertyName id);
    }
}
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerIdleData
    {
       [field: SerializeField] public List<playerCamra> BackwardsCameraRecenteringData { get; private set; }
    }
}

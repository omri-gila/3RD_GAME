using System;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerWalkData
    {

        [field: SerializeField][field: Range(0f, 1f)] public float SpeedModifier { get; private set; } =  0.225f;
      [field: SerializeField] public List<playerCamra> BackwardsCameraRecenteringData { get; private set; }


    }
}

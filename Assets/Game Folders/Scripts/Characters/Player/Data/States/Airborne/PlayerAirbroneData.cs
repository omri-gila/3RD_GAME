using System;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerAirbroneData
    {
        [field : SerializeField] public PlayerJumpData JumpData {  get; private set; }
        [field: SerializeField] public PlayerFallData FallData { get; private set; }


    }
}

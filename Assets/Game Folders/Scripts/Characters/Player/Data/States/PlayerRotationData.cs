using System;
using UnityEditor;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]


    public class PlayerRotationData
    {

        [field : SerializeField] public Vector3 TargetRotationReachTime {  get; private set; }



    }
}

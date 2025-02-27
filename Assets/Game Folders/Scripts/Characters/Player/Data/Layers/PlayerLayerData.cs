using System;
using UnityEngine;

namespace MovementSystem
{

    [Serializable]
    public class PlayerLayerData
    {
        [field : SerializeField] public LayerMask GroundLayer {  get; private set; }

       public bool ContinsLayer( LayerMask layerMask ,int layer)
        {
            return (1 << layer & layerMask) != 0;
        }

        public bool IsGruondLayer(int layer)
        {
            return ContinsLayer(GroundLayer, layer);
        }

       
    }
}

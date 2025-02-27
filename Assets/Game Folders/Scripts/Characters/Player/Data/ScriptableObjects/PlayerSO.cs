using UnityEngine;

namespace MovementSystem
{

    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Plater")]
    public class PlayerSO : ScriptableObject
    {

        [field :SerializeField ]public PlayerGroundedData GroundedData { get; private set; }
        [field: SerializeField] public PlayerAirbroneData AirbroneData { get; private set; }



    }
}

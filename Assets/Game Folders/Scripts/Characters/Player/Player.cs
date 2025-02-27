using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    [RequireComponent(typeof(PlayerInput))]

    public class Player : MonoBehaviour
    {

        [field : Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }


        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }



        public Rigidbody Rigidbody { get; private set; }


        private PlayerMovementStateMachine mStateMachine;


        public PlayerInput Input { get; private set; }

        public Transform MainCamreaTransfrom { get; private set; }


        private void Awake()
        {

            Rigidbody = GetComponent<Rigidbody>();

            Input = GetComponent<PlayerInput>();

    


            mStateMachine = new PlayerMovementStateMachine(this);

            MainCamreaTransfrom = Camera.main.transform;


        }

        private void OnValidate()
        {
            ColliderUtility.Initialize(gameObject);

            ColliderUtility.CalculateCapsuleColliderDimensions();
        }

        private void Start()
        {

            mStateMachine.Changestate(mStateMachine.IdlingState);


        }

        public void OnTriggerEnter(Collider collider)
        {
            mStateMachine.OnTriggerEnter(collider);
        }

        private void Update()
        {

            mStateMachine.HandleInput();


            mStateMachine.Update();

        }


        private void FixedUpdate()
        {


            mStateMachine.PhysicsUpdate();

        }

    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    [RequireComponent(typeof(PlayerInput))]

    [RequireComponent(typeof(PlayerCapsuleColliderUtility))]

    public class Player : MonoBehaviour
    {

        [field : Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }


        [field: Header("Camera")]
        [field: SerializeField] public PlayerCameraRecenteringUtility CameraRecenteringUtility { get; private set; }

        [field: Header("Collisions")]
        public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field: Header("Animations")]
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

        public Rigidbody Rigidbody { get; private set; }


        private PlayerMovementStateMachine mStateMachine;

        public Animator Animator { get; private set; }
        public PlayerInput Input { get; private set; }

        public Transform MainCamreaTransfrom { get; private set; }


        private void Awake()
        {
            CameraRecenteringUtility.Initialize();
            AnimationData.Initialize();

            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            Input = GetComponent<PlayerInput>();
            ColliderUtility = GetComponent<PlayerCapsuleColliderUtility>();

            MainCamreaTransfrom = Camera.main.transform;

            mStateMachine = new PlayerMovementStateMachine(this);
        }

        private void Start()
        {
            mStateMachine.Changestate(mStateMachine.IdlingState);
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

        private void OnTriggerEnter(Collider collider)
        {
            mStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            mStateMachine.OnTriggerExit(collider);
        }

        public void OnMovementStateAnimationEnterEvent()
        {
            mStateMachine.OnAnimitionEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            mStateMachine.OnAnimitionExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            mStateMachine.OnAnimitionTransitionEvent();
        }
    }
}

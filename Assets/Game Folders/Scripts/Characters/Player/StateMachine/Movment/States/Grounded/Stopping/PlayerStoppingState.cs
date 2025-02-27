using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            //  SetBaseCameraRecenteringData();

            base.Enter();

            // StartAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            //StopAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

          RotateTowardsTargetRotation();
         
           if (!IsMovingHorizontally())
            {
                return;
            }

           DecelerateHorizontally();
        }
      

        public override void OnAnimitionTransitionEvent()
        {
            stateMachine.Changestate(stateMachine.IdlingState);
        }
        #endregion

        #region Input Methods
   

        protected override void OnMovmentCenceld(InputAction.CallbackContext context)
        {


        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion


        #region Reusable Methods

        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;

        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;

        }

        #endregion
    }
}

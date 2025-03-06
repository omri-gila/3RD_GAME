using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region Input Metods

    
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
         
        }

        #endregion



        public override void Enter()
        {
            base.Enter();

           StartAnimation(stateMachine.Player.AnimationData.LandingParameterHash);

            DisableCameraRecentering();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
        }

    }
}

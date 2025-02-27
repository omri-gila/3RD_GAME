using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerWlkingState : PlayerMovingState
    {
        //  public PlayerMovementStateMachine playerMovementStateMachine;

        public PlayerWlkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            // this.playerMovementStateMachine = playerMovementStateMachine;

        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.WeakForce;

            stateMachine.ReusableData.MovementSpeedModifier = movmentData.WalkData.SpeedModifier;
        }

        #endregion


        #region Input Methods


        protected override void OnMovmentCenceld(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.LightStoppingState);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.Changestate(stateMachine.RunningState);
        }
        #endregion



    }
}

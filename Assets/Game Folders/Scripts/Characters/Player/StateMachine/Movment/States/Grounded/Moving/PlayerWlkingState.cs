using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerWlkingState : PlayerMovingState
    {

        public PlayerWlkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movmentData.WalkData.SpeedModifier;

            stateMachine.ReusableData.BackwardsCameraRecenteringData = movmentData.WalkData.BackwardsCameraRecenteringData;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

            SetBaseCameraRecenteringData();
        }

        #endregion


        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
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

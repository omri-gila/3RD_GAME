using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerRuunigState : PlayerMovingState
    {
        private float startTime;
        private PlayerSprintData PlayerSprintData;
        public PlayerRuunigState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            PlayerSprintData = movmentData.SprintData;
        }
        #region Istate
        public override void Enter()
        {
            base.Enter();
             stateMachine.ReusableData.MovementSpeedModifier = movmentData.RunData.SpeedModifier;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.MediumForce;

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if (!stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }
            if(Time.time <startTime + PlayerSprintData.RunToWalkTime)
            {
                 return ;
            }
            StopRunnig();
        }


        #endregion


        #region Main Methods

        private void StopRunnig()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.IdlingState);
                return;
            }

            stateMachine.Changestate(stateMachine.WalkingState);
        }


        #endregion

        #region Input Methods
        protected override void OnMovmentCenceld(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.MediumStoppingState);

        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.Changestate(stateMachine.WalkingState);
        }


        #endregion

    }
}


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
        }
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movmentData.RunData.SpeedModifier;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.MediumForce;

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (!stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            if (Time.time < startTime + movmentData.SprintData.RunToWalkTime)
            {
                return;
            }

            StopRunning();
        }

        private void StopRunning()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.IdlingState);

                return;
            }

            stateMachine.Changestate(stateMachine.WalkingState);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.Changestate(stateMachine.WalkingState);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.MediumStoppingState);

            base.OnMovementCanceled(context);
        }
    }
}


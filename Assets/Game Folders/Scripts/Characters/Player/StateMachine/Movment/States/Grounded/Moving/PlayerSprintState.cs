using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerSprintState : PlayerMovingState
    {
        private PlayerSprintData playerSprintData;

        private bool KeepSprinting;
        private float startTime;
        private bool shouldResetSprintState;

        public PlayerSprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movmentData.SprintData.SpeedModifier;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.SprintParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;

            startTime = Time.time;

            shouldResetSprintState = true;

            if (!stateMachine.ReusableData.ShouldSprint)
            {
                KeepSprinting = false;
            }
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.SprintParameterHash);

            if (shouldResetSprintState)
            {
                KeepSprinting = false;

                stateMachine.ReusableData.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (KeepSprinting)
            {
                return;
            }

            if (Time.time < startTime + movmentData.SprintData.SprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        private void StopSprinting()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.IdlingState);

                return;
            }

            stateMachine.Changestate(stateMachine.RunningState);
        }

        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }

        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            KeepSprinting = true;

            stateMachine.ReusableData.ShouldSprint = true;
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.HardStoppingState);

            base.OnMovementCanceled(context);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;

            base.OnJumpStarted(context);
        }

        protected override void OnFall()
        {
            shouldResetSprintState = false;

            base.OnFall();
        }
    }
}

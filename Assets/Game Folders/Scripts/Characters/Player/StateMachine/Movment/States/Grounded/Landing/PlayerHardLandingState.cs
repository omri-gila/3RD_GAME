using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.HardLandParameterHash);

            stateMachine.Player.Input.PlayerActions.Movement.Disable();

            ResetVeloctiy();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.HardLandParameterHash);

            stateMachine.Player.Input.PlayerActions.Movement.Enable();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVeloctiy();
        }

        public override void OnAnimitionExitEvent()
        {
            stateMachine.Player.Input.PlayerActions.Movement.Enable();

        }

        public override void OnAnimitionTransitionEvent()
        {
            stateMachine.Changestate(stateMachine.IdlingState);
        }

       

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
 

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }

        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            stateMachine.Changestate(stateMachine.RunningState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
    }
}

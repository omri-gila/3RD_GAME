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
        #region ISatet MWthod
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.Input.PlayerActions.Movement.Disable();    

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVeloctiy();

        }

        public override void Exit()
        {
            base.Exit();

            stateMachine.Player.Input.PlayerActions.Movement.Enable();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsMovingHorizontally()) { return; }
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
        #endregion



        #region Reusable Methods


        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.started += OnMovmentStarted;

        }

   

        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovmentStarted;

        }


        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk) { return; }
            stateMachine.Changestate(stateMachine.RunningState);
        }



        #endregion

        #region Input Methods

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }

        private void OnMovmentStarted(InputAction.CallbackContext context)
        {
            OnMove();   
        }

        #endregion
    }
}

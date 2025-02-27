using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerRollingState : PlayerLandingState
    {
        public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movmentData.RollData.SpeedModifier;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.RollParameterHash);

            stateMachine.ReusableData.ShouldSprint = false;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.RollParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimitionTransitionEvent()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.MediumStoppingState);

                return;
            }

            OnMove();
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
    }
}

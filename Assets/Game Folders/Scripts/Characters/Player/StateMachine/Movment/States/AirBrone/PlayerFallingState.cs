using System;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {

        private Vector3 playerPositionOnEnter;

        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            playerPositionOnEnter = stateMachine.Player.transform.position;

            ResetVeloctiy();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVeloctiy();

            if (playerVerticalVelocity.y >= -airbroneData.FallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(0f, -airbroneData.FallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

            if (fallDistance < airbroneData.FallData.MinimumDistanceToBeConsideredHardFall)
            {
                stateMachine.Changestate(stateMachine.LightLandingState);

                return;
            }

            if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.HardLandingState);

                return;
            }

            stateMachine.Changestate(stateMachine.RollingState);

        }
    }
}

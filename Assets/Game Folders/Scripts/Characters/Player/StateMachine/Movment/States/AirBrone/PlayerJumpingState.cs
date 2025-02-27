using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private bool sholdKeepRotating;
        private PlayerJumpData jumpData;

        private bool canStatrFalling;

        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.MovementDecelerationForce = airbroneData.JumpData.DecelerationForce;

            stateMachine.ReusableData.RotationData = airbroneData.JumpData.RotationData;

            sholdKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStatrFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if (!canStatrFalling && IsMovingUp(0f))
            {
                canStatrFalling = true;
            }

            if (!canStatrFalling || IsMovingUp(0f))
            {
                return;
            }

            stateMachine.Changestate(stateMachine.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (sholdKeepRotating)
            {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }

        private void Jump()
        {
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = stateMachine.Player.transform.forward;

            if (sholdKeepRotating)
            {
                UpdateTargetRotation(GetMovmentInputDirection());

                jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            jumpForce = GetJumpForceOnSlope(jumpForce);

            ResetVeloctiy();

            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, airbroneData.JumpData.JumpToGroundRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = airbroneData.JumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = airbroneData.JumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            return jumpForce;
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
    }
}


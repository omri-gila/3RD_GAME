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
            jumpData = airbroneData.JumpData;
        }
        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0.5f;

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

            if (!canStatrFalling && IsMovingUp()) 
            {
                canStatrFalling = true;

            }


            if (!canStatrFalling || GetPlayerVerticalVeloctiy().y > 0)
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
        #endregion

        #region Main Methods 

        private void Jump()
        {
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = stateMachine.Player.transform.forward;

            jumpForce.x = jumpDirection.x;
            jumpForce.z = jumpDirection.z;

            if (sholdKeepRotating)
            {
                UpdateTargetRotation(GetMovmentInputDirection());

                jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);


            }

            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;

            Ray dwonwardsRayFromCapsule = new Ray(capsuleColliderCenterInWorldSpace,Vector3.down);
            if (Physics.Raycast(dwonwardsRayFromCapsule, out RaycastHit hit, airbroneData.JumpData.JumpToGroundRayDistance,
                stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -dwonwardsRayFromCapsule.direction);

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


            ResetVeloctiy();

            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        #endregion
        #region Reusable Methods

        protected override void ResetSprintState()
        {


        }

        #endregion
    }
}


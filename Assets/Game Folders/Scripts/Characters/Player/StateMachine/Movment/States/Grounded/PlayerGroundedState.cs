using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerGroundedState : PlayerMoventState
    {

        private SlopeData slopeData;
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            slopeData = stateMachine.Player.ColliderUtility.SlopeData;
        }

        #region IState Methods

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        public override void Enter()
        {
            base.Enter();


            UpdateShouldSprintState();

        }




        #endregion


        #region Main Methods


        private void UpdateShouldSprintState()
        {
            if (!stateMachine.ReusableData.ShouldSprint)
            {
                return;
            }

            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }
            stateMachine.ReusableData.ShouldSprint = false;
        }


        private void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit,
                slopeData.FloatRayDistance,stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore))
            {


                float groundAngle = Vector3.Angle(hit.normal , - downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0) { return; }

                float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y 
                    * stateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }


                float amountTolift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVeloctiy().y ;

                Vector3 liftForce = new Vector3 ( 0f, amountTolift, 0f); 

                stateMachine.Player.Rigidbody.AddForce (liftForce , ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle) 
        { 
            float slopeSpeedModifier = movmentData.SlopeSpeedAngles.Evaluate(angle);

            stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
            return slopeSpeedModifier;

        }
        //{
        //    float slopeSpeedModifier = groundedData.SlopeSpeedAngles.Evaluate(angle);

        //    if (stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
        //    {
        //        stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

        //        UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        //    }

        //    return slopeSpeedModifier;
        //}
    
        #endregion

        #region Reusable Mwthods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovmentCenceld;

            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;


        }
   


        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovmentCenceld;
            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;





        }

        protected virtual void OnMove()
        {

            if (stateMachine.ReusableData.ShouldSprint) 
            {

                stateMachine.Changestate(stateMachine.SprintingState);
                return;


            }

            if ( stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.Changestate(stateMachine.WalkingState);
                return;
            }

            stateMachine.Changestate(stateMachine.RunningState);

        }

        #endregion

        #region Input Methods


       
        protected virtual void OnMovmentCenceld(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.IdlingState);

        }


        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.JumpingState);

        }

        #endregion
    }
}

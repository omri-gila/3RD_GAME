using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private float startTime;

        private int consecutiveDashesUsed;

        public bool shouldKeepRotating;

        private PlayerDashData dashData;

        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData = movmentData.DashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();


            stateMachine.ReusableData.MovementOnSlopesSpeedModifier = dashData.SpeedModifier;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;


            stateMachine.ReusableData.RotationData = dashData.RotationData;

            Dash();

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            UpdateConsecutiveDashes();

            startTime = Time.time;

        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!shouldKeepRotating)
            {
                return;
            }

; RotateTowardsTargetRotation();
        }

        public override void OnAnimitionTransitionEvent()
        {


            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.HardStoppingState);
                return;
            }


            stateMachine.Changestate(stateMachine.SprintingState);

        }

        #endregion

        #region Main Methods
        private void Dash()
        {

            Vector3 dashDirection = stateMachine.Player.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);


            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovmentInputDirection());

                dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);


            }



            stateMachine.Player.Rigidbody.linearVelocity = dashDirection * GetMovementSpeed(false);

        }



        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if (consecutiveDashesUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash,
                   dashData.DashLimitReachedCooldown);

            }


        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
        }
        #endregion


        #region Reusable Methods 

        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovmentPerformed;

        }



        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovmentPerformed;

        }


        #endregion

        #region Input Methods

        protected override void OnMovmentCenceld(InputAction.CallbackContext context)
        {

        }

        //protected override void OnDashStarted(InputAction.CallbackContext context)
        //{

        //}

        private void OnMovmentPerformed(InputAction.CallbackContext context)
        {

            shouldKeepRotating = true;
        }


        #endregion
    }
}

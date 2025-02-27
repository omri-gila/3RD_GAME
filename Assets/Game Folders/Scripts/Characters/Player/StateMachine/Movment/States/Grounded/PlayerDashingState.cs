using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
   public class PlayerDashingState : PlayerGroundedState
    {
        private float startTime;

    private int consecutiveDashesUsed;

    private bool shouldKeepRotating;

    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {

            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.IdlingState);
                return;
            }


            stateMachine.ReusableData.MovementSpeedModifier = movmentData.DashData.SpeedModifier;

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);

        stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;

        stateMachine.ReusableData.RotationData = movmentData.DashData.RotationData;

        Dash();

        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

        UpdateConsecutiveDashes();

        startTime = Time.time;

   
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);

        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!shouldKeepRotating)
        {
            return;
        }

        RotateTowardsTargetRotation();
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

    protected override void AddInputActionsCallBacks()
    {
        base.AddInputActionsCallBacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;

    }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
           base.OnMovementPerformed(context);

            shouldKeepRotating = true;

        }

        protected override void RemoveInputActionsCallBacks()
    {
        base.RemoveInputActionsCallBacks();

        stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

   

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

        if (consecutiveDashesUsed == movmentData.DashData.ConsecutiveDashesLimitAmount)
        {
            consecutiveDashesUsed = 0;

            stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash, movmentData.DashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime + movmentData.DashData.TimeToBeConsideredConsecutive;
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }
}
}

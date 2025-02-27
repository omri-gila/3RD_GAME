using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerGroundedState : PlayerMoventState
    //{

    //    private SlopeData slopeData;
    //    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    //    {
    //        slopeData = stateMachine.Player.ColliderUtility.SlopeData;
    //    }

    //    #region IState Methods

    //    public override void PhysicsUpdate()
    //    {
    //        base.PhysicsUpdate();

    //        Float();
    //    }

    //    public override void Enter()
    //    {
    //        base.Enter();



    //        StartAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);


    //        UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);

    //        UpdateShouldSprintState();

    //    }

    //    public override void Exit()
    //    {
    //        base.Exit();

    //       StopAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);

    //    }



    //    #endregion


    //    #region Main Methods


    //    private void UpdateShouldSprintState()
    //    {
    //        if (!stateMachine.ReusableData.ShouldSprint)
    //        {
    //            return;
    //        }

    //        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
    //        {
    //            return;
    //        }
    //        stateMachine.ReusableData.ShouldSprint = false;
    //    }


    //    private void Float()
    //    {
    //        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;

    //        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

    //        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit,
    //            slopeData.FloatRayDistance,stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore))
    //        {


    //            float groundAngle = Vector3.Angle(hit.normal , - downwardsRayFromCapsuleCenter.direction);

    //            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

    //            if (slopeSpeedModifier == 0) { return; }

    //            float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y 
    //                * stateMachine.Player.transform.localScale.y - hit.distance;

    //            if (distanceToFloatingPoint == 0f)
    //            {
    //                return;
    //            }


    //            float amountTolift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVeloctiy().y ;

    //            Vector3 liftForce = new Vector3 ( 0f, amountTolift, 0f); 

    //            stateMachine.Player.Rigidbody.AddForce (liftForce , ForceMode.VelocityChange);
    //        }
    //    }

    //    private float SetSlopeSpeedModifierOnAngle(float angle) 
    //    { 
    //        float slopeSpeedModifier = movmentData.SlopeSpeedAngles.Evaluate(angle);

    //        stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
    //        return slopeSpeedModifier;

    //    }



    //    private bool IsThereGruondUnderath()
    //    {


    //        BoxCollider groundCheckCollider = stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckCollider;

    //        Vector3 groundColliedrInWorldSpace = groundCheckCollider.bounds.center;
    //        Collider[] overlappedGroindcolliders = Physics.OverlapBox(groundColliedrInWorldSpace,
    //          stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckColliderVerticalExtents, groundCheckCollider.transform.rotation,
    //          stateMachine.Player.LayerData.GroundLayer,
    //            QueryTriggerInteraction.Ignore);
    //        return overlappedGroindcolliders.Length > 0;

    //    }


    //    #endregion

    //    #region Reusable Mwthods
    //    protected override void AddInputActionsCallBacks()
    //    {
    //        base.AddInputActionsCallBacks();

    //        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovmentCenceld;

    //        stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

    //        stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;


    //    }



    //    protected override void RemoveInputActionsCallBacks()
    //    {
    //        base.RemoveInputActionsCallBacks();
    //        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovmentCenceld;
    //        stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
    //        stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;





    //    }

    //    protected virtual void OnMove()
    //    {

    //        if (stateMachine.ReusableData.ShouldSprint) 
    //        {

    //            stateMachine.Changestate(stateMachine.SprintingState);
    //            return;


    //        }

    //        if ( stateMachine.ReusableData.ShouldWalk)
    //        {
    //            stateMachine.Changestate(stateMachine.WalkingState);
    //            return;
    //        }

    //        stateMachine.Changestate(stateMachine.RunningState);

    //    }

    //    protected override void OnContactWithGroundExited(Collider collider)
    //    {
    //        base.OnContactWithGroundExited(collider);


    //        if (IsThereGruondUnderath())
    //        {
    //            return;

    //        }


    //        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;
    //        Ray dwonwardRayCapsuleBottom = new Ray (capsuleColliderCenterInWorldSpace - 
    //            stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtens,Vector3.down);

    //        if(!Physics.Raycast(dwonwardRayCapsuleBottom,out _, movmentData.GruondToFallRayDistance,
    //            stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore))
    //        {
    //            OnFall();
    //        }


    //        OnFall();
    //    }


    //    protected virtual void OnFall()
    //    {
    //        stateMachine.Changestate(stateMachine.FallingState);
    //    }


    //    #endregion

    //    #region Input Methods



    //    protected virtual void OnMovmentCenceld(InputAction.CallbackContext context)
    //    {
    //        stateMachine.Changestate(stateMachine.IdlingState);

    //    }


    //    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    //    {
    //        stateMachine.Changestate(stateMachine.DashingState);
    //    }

    //    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    //    {
    //        stateMachine.Changestate(stateMachine.JumpingState);

    //    }

    //    #endregion
    //}

    {
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);

            UpdateShouldSprintState();

            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.GroundedParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

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

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, stateMachine.Player.ColliderUtility.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    return;
                }

                float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * stateMachine.Player.ColliderUtility.SlopeData.StepReachForce - GetPlayerVerticalVeloctiy().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = movmentData.SlopeSpeedAngles.Evaluate(angle);

            if (stateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

                UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
            }

            return slopeSpeedModifier;
        }

        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;

            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.JumpingState);
        }

        protected virtual void OnMove()
        {
            if (stateMachine.ReusableData.ShouldSprint)
            {
                stateMachine.Changestate(stateMachine.SprintingState);

                return;
            }

            if (stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.Changestate(stateMachine.WalkingState);

                return;
            }

            stateMachine.Changestate(stateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.CapsuleCollider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtens, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movmentData.GruondToFallRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        private bool IsThereGroundUnderneath()
        {
            PlayerTriggerColliderData triggerColliderData = stateMachine.Player.ColliderUtility.TriggerColliderData;

            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }

        protected virtual void OnFall()
        {
            stateMachine.Changestate(stateMachine.FallingState);
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            UpdateTargetRotation(GetMovmentInputDirection());
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData _rollData;
        public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            _rollData = movmentData.RollData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = _rollData.SpeedModifier;
            stateMachine.ReusableData.ShouldSprint = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(stateMachine.ReusableData.MovementInput != Vector2.zero) { return; }
            RotateTowardsTargetRotation();


        }

        public override void OnAnimitionTransitionEvent()
        {
           if(stateMachine.ReusableData.MovementInput == Vector2.zero) { stateMachine.Changestate(stateMachine.MediumStoppingState); return; }
            OnMove();

        }

        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }

        #endregion

    }
}

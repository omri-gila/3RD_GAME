using UnityEngine;

namespace MovementSystem
{

    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }


        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StationaryForce;



            ResetVeloctiy();
        }

        public override void Update()
        {
            base.Update();
            if(stateMachine.ReusableData.MovementInput == Vector2.zero) { return; }

            OnMove();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsMovingHorizontally()) { return; }
            ResetVeloctiy();
        }

        public override void OnAnimitionTransitionEvent()
        {


            stateMachine.Changestate(stateMachine.IdlingState);
        }

        #endregion  
    }
}

using System;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {

        private PlayerFallData fallData;
        private Vector3 playerPositionOnEnter;

        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            fallData = airbroneData.FallData;
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            playerPositionOnEnter = stateMachine.Player.transform.position;

            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            ResetVerticalVeloctiy();   
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVeloctiy();
        }



        #endregion

        #region Reusable Methods

        protected override void ResetSprintState()
        {


        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance =playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

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


        #endregion


        #region Main Methods

        private void LimitVerticalVeloctiy()
        {
            Vector3 playerVerticalVelocty = GetPlayerVerticalVeloctiy();

            if(playerVerticalVelocty.y >= -fallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitVeloctiy = new Vector3(0f , -fallData.FallSpeedLimit -playerVerticalVelocty.y, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitVeloctiy,ForceMode.VelocityChange);
        }

        #endregion

    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

             stateMachine.ReusableData.MovementSpeedModifier = 1f;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StationaryForce;

            ResetVeloctiy(); 
        }


        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                return;
            }
            OnMove();

        }


        #endregion
    }
}

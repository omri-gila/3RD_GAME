using UnityEngine;

namespace MovementSystem
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Metohds
        public override void Enter()
        {
            base.Enter();

          stateMachine.ReusableData.MovementDecelerationForce = movmentData.StopData.LightDecelerationForce;
            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.WeakForce;

        }

        #endregion
    }
}

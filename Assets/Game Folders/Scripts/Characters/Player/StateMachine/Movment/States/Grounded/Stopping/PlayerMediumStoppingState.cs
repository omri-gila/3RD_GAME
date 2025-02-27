using UnityEngine;

namespace MovementSystem
{
    public class PlayerMediumStoppingState : PlayerLightStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Metohds
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = movmentData.StopData.MediumDecelerationForce;
            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.MediumForce;

        }

        #endregion

    }
}

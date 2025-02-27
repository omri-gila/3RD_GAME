using UnityEngine;

namespace MovementSystem
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }

        #region IState Metohds
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = movmentData.StopData.HardDecelerationForce;
            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;

        }

        #endregion


        #region Reusable Methods

        protected override void OnMove()
        {

            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;

            }

            stateMachine.Changestate(stateMachine.RunningState);
        }

        #endregion
    }
}

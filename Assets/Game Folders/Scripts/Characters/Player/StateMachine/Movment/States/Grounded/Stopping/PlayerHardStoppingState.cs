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

            StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = movmentData.StopData.HardDecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);
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

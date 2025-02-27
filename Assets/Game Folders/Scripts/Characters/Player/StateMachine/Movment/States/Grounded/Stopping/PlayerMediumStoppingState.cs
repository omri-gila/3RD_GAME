using UnityEngine;

namespace MovementSystem
{
    public class PlayerMediumStoppingState : PlayerLightStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = movmentData.StopData.MediumDecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);
        }
    }

}


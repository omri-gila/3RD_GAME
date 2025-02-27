using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerSprintState : PlayerMovingState
    {
        private PlayerSprintData playerSprintData;

        private bool KeepSprinting;
        private float startTime;
        private bool shouldResetSprintState;

        public PlayerSprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            playerSprintData = movmentData.SprintData;

        }
        #region IState Methods 


        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementSpeedModifier = playerSprintData.SpeedModifier;
            stateMachine.ReusableData.CurrentJumpForce = airbroneData.JumpData.StrongForce;
            shouldResetSprintState = true;


            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            if (shouldResetSprintState) 
            {
                KeepSprinting = false;
                stateMachine.ReusableData.ShouldSprint = false;

            }

        }

        public override void Update()
        {
            base.Update();

            if (KeepSprinting) 
            {
                
                return;
            }
            if (startTime < Time.time + playerSprintData.SprintToRunTime) 
            { 
                return ;

            }

            StopSprinting();
        }






        #endregion

        #region Main Methods 


        private void StopSprinting()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.Changestate(stateMachine.IdlingState);
                return;
            }

            stateMachine.Changestate(stateMachine.RunningState);


        }



        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintFormed;
        }

       

        protected override void RemoveInputActionsCallBacks()
        {
            
            base.RemoveInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintFormed;

        }



        #endregion

        #region Input Methods 

        protected override void OnMovmentCenceld(InputAction.CallbackContext context)
        {
            stateMachine.Changestate(stateMachine.HardStoppingState);
        }


        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;
            base.OnJumpStarted(context);



        }


        private void OnSprintFormed(InputAction.CallbackContext context)
        {
            KeepSprinting = true;

            stateMachine.ReusableData.ShouldSprint = true;
        }

        #endregion
    }
}

using System;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerMovementStateMachine : stateMachine
    {
        public Player Player { get; }

       public PlayerStateReusableData ReusableData { get; }


        public PlayerIdlingState IdlingState { get; }

        public PlayerDashingState DashingState { get; }

        public PlayerWlkingState WalkingState { get; }

        public PlayerRuunigState RunningState { get; }

        public PlayerSprintState SprintingState { get; }

        public PlayerHardStoppingState HardStoppingState { get; }
        public PlayerMediumStoppingState MediumStoppingState { get; }
        public PlayerLightStoppingState LightStoppingState { get; }

        public PlayerJumpingState JumpingState { get; }

        public PlayerMovementStateMachine(Player player)
        {
            Player = player;


           ReusableData = new PlayerStateReusableData();

            IdlingState = new PlayerIdlingState(this);

            DashingState = new PlayerDashingState(this);

            WalkingState = new PlayerWlkingState(this);

            RunningState = new PlayerRuunigState(this);

            SprintingState = new PlayerSprintState(this);

            HardStoppingState = new PlayerHardStoppingState(this);

            MediumStoppingState = new PlayerMediumStoppingState(this);

            LightStoppingState = new PlayerLightStoppingState(this);    

            JumpingState = new PlayerJumpingState(this);    



        }


    }
}
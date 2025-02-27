
using Unity.VisualScripting;
using UnityEngine;

namespace MovementSystem
{
    public abstract class stateMachine
    {

        protected IState currentState;


        public void Changestate(IState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }
        public void Update()
        {
            currentState?.Update();

        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }

        public void OnAnimitionEnterEvent()
        {
            currentState?.OnAnimitionEnterEvent();
        }

        public void OnAnimitionExitEvent()
        {
            currentState?.OnAnimitionExitEvent();
        }

        public void OnAnimitionTransitionEvent()
        {
            currentState?.OnAnimitionTransitionEvent();
        }

       public void OnTriggerEnter(Collider collider)
       {
           currentState?.OnTriggerEnter(collider);
       }
        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
        }







    }
}

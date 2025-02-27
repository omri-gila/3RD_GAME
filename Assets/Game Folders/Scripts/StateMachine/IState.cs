using Unity.VisualScripting;
using UnityEngine;

namespace MovementSystem
{
    public interface IState
    {

        public void Enter();
        public void Exit();

        public void HandleInput();

        public void Update();


        public void PhysicsUpdate();

        public void OnAnimitionEnterEvent();

        public void OnAnimitionExitEvent();

        public void OnAnimitionTransitionEvent();

        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);





    }
}

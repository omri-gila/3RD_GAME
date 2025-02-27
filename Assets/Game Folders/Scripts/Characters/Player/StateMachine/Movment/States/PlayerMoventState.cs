using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerMoventState : IState
    {

        protected PlayerMovementStateMachine stateMachine;



        protected PlayerGroundedData movmentData;

        protected PlayerAirbroneData airbroneData;

        
  
        public PlayerMoventState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;

            movmentData= stateMachine.Player.Data.GroundedData;
            airbroneData =stateMachine.Player.Data.AirbroneData;

            InitializeData();
        }

        private void InitializeData()
        {
            SetBaseRotationData();
            SetBaseCameraRecenteringData();
        }



        #region IState Methods


        protected void SetBaseCameraRecenteringData()
        {
            stateMachine.ReusableData.SidewaysCameraRecenteringData = movmentData.SidewaysCameraRecenteringData;


            stateMachine.ReusableData.BackwardsCameraRecenteringData = movmentData.BackwardsCameraRecenteringData;
        }
        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(stateMachine.ReusableData.MovementInput);
        }
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionsCallBacks();
        }



        public virtual void Exit()
        {

            RemoveInputActionsCallBacks();
        }



        public virtual void HandleInput()
        {
            ReadMovmentInput();
        }

        public virtual void Update()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }


        public virtual void OnAnimitionEnterEvent()
        {

        }

        public virtual void OnAnimitionExitEvent()
        {

        }

        public virtual void OnAnimitionTransitionEvent()
        {
           
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGruondLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return; 
            }

        }

       

        public void OnTriggerExit(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGruondLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);
                return ;
            }

        }

     

        #endregion

        #region Main Methods

        private void ReadMovmentInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero ||  stateMachine.ReusableData.MovementSpeedModifier == 0)
            {
                return; 
            }

            Vector3 movementDirection = GetMovmentInputDirection();

            float targetRotationYangle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYangle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currpentPlayerHorizontalVeloctiy = GetPlayerHorizontalVeloctiy();

            stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed -
                currpentPlayerHorizontalVeloctiy, ForceMode.VelocityChange);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);
            return directionAngle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
             stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;
             stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        private float AddCameraRotationToAngle(float directionAngle)
        {
            directionAngle += stateMachine.Player.MainCamreaTransfrom.eulerAngles.y;

            if (directionAngle > 360f)
            {
                directionAngle -= 360f;
            }

            return directionAngle;
        }

        private static float GetDirectionAngle(Vector3 angle)
        {
            float directionAngle = MathF.Atan2(angle.x, angle.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        #endregion

        #region Reusable Methods

        protected void StartAnimation(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, false);
        }


        protected Vector3 GetMovmentInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }

        protected float GetMovementSpeed(bool sholdConsiderSlopes = true)
        {
            float movmentSpeed = movmentData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

            if (sholdConsiderSlopes) { movmentSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier; }


            return movmentSpeed;

        }

    
        protected Vector3 GetPlayerHorizontalVeloctiy()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.linearVelocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }


        protected Vector3 GetPlayerVerticalVeloctiy() 
        { 
            return new Vector3(0f , stateMachine.Player.Rigidbody.linearVelocity.y,0f);


        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle ==  stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                return; 
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(
                currentYAngle,
                 stateMachine.ReusableData.CurrentTargetRotation.y,
                ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,
                 stateMachine.ReusableData.TimeToReachTargetRotation.y -  stateMachine.ReusableData.DampedTargetRotationPassedTime.y
            );

             stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
            stateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            RotateTowardsTargetRotation();

            if (directionAngle !=  stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }


        protected void ResetVeloctiy()
        {
            stateMachine.Player.Rigidbody.linearVelocity = Vector3.zero;
        }

        protected void ResetVerticalVeloctiy()
        {
            Vector3 playerHorizontalVeloctiy = GetPlayerHorizontalVeloctiy();

            stateMachine.Player.Rigidbody.angularVelocity = playerHorizontalVeloctiy;
        }

        protected virtual void AddInputActionsCallBacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
            stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;



        }



        protected virtual void RemoveInputActionsCallBacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;

        }


        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVeloctiy();

            stateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVeloctiy();

            stateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * stateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVeloctiy();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVeloctiy().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVeloctiy().y < -minimumVelocity;
        }

        protected void SetBaseRotationData()
        {
            stateMachine.ReusableData.RotationData = movmentData.BaseRotationData;
            stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }

   
        protected void SetCameraRecenteringState(float cameraVerticalAngle, List<playerCamra> cameraRecenteringData)
        {
            foreach (playerCamra recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                {
                    continue;
                }

                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);

                return;
            }

            DisableCameraRecentering(); ;
        }


        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            if (movementInput == Vector2.up)
            {
                DisableCameraRecentering();

                return;
            }

            float cameraVerticalAngle = stateMachine.Player.MainCamreaTransfrom.eulerAngles.x;

            if (cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            }

            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if (movementInput == Vector2.down)
            {
              SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.BackwardsCameraRecenteringData);

                return;
            }

          SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.SidewaysCameraRecenteringData);
        }

   

        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movementSpeed = GetMovementSpeed();

            if (movementSpeed == 0f)
            {
                movementSpeed = movmentData.BaseSpeed;
            }

            stateMachine.Player.CameraRecenteringUtility.EnableRecentering(waitTime, recenteringTime, movmentData.BaseSpeed, movementSpeed);
        }

        protected void DisableCameraRecentering()
        {
            stateMachine.Player.CameraRecenteringUtility.DisableRecentering();
        }


        protected virtual void OnContactWithGround(Collider collider)
        {


        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {


        }

        #endregion

        #region Input Methods

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {

             stateMachine.ReusableData.ShouldWalk = ! stateMachine.ReusableData.ShouldWalk;
        }



        //protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        //{
        //    float movementSpeed = GetMovementSpeed();

        //    if (movementSpeed == 0f)
        //    {
        //        movementSpeed = movmentData.BaseSpeed;
        //    }

        //  stateMachine.Player.CameraRecenteringUtility.EnableRecentering(waitTime, recenteringTime, movmentData.BaseSpeed, movementSpeed);
        //}

        //protected void DisableCameraRecentering()
        //{
        //    stateMachine.Player.CameraRecenteringUtility.DisableRecentering();
        //}


        #endregion


    }
}

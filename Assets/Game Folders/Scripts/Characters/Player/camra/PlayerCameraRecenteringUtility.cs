using System;
using Unity.Cinemachine;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerCameraRecenteringUtility
    {
        [field: SerializeField] public CinemachineCamera VirtualCamera { get; private set; }
        [field: SerializeField] public float DefaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float DefaultHorizontalRecenteringSpeed { get; private set; } = 1f;


        private bool isRecentering;
        private float recenterSpeed;

        public void Initialize()
        {
            if (VirtualCamera == null)
            {
                Debug.LogError("Virtual Camera is not assigned.");
            }
        }

        public void EnableRecentering(float waitTime = -1f, float recenteringSpeed = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
        {
            if (VirtualCamera == null) return;

            if (waitTime == -1f)
                waitTime = DefaultHorizontalWaitTime;

            if (recenteringSpeed == -1f)
                recenteringSpeed = DefaultHorizontalRecenteringSpeed;

            recenterSpeed = recenteringSpeed * (baseMovementSpeed / movementSpeed);
            isRecentering = true;

            VirtualCamera.StartCoroutine(RecenteringCoroutine(waitTime));
        }

        private System.Collections.IEnumerator RecenteringCoroutine(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            while (isRecentering)
            {
                // Get the current rotation
                Vector3 currentRotation = VirtualCamera.transform.localEulerAngles;

                // Normalize the X rotation
                float currentRotationX = currentRotation.x;
                if (currentRotationX > 180f)
                    currentRotationX -= 360f;

                // Lerp towards zero
                float newRotationX = Mathf.Lerp(currentRotationX, 0, Time.deltaTime * recenterSpeed);

                if (Mathf.Abs(newRotationX) < 0.01f)
                {
                    newRotationX = 0;
                    isRecentering = false;
                }

                // Update the camera rotation
                VirtualCamera.transform.localEulerAngles = new Vector3(
                    newRotationX,
                    currentRotation.y,
                    currentRotation.z
                );

                yield return null;
            }
        }

        public void DisableRecentering()
        {
            isRecentering = false;
        }
    }
}
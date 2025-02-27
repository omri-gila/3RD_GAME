using Unity.Cinemachine;
using UnityEngine;

namespace MovementSystem
{
    public class CameraCloseUpController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private float minDistance = 0.5f; // מרחק מינימלי לפני התקרבות
        [SerializeField] private float maxZoom = 30f; // FOV מקסימלי
        [SerializeField] private float minZoom = 60f; // FOV מינימלי (זום אאוט)
        [SerializeField] private LayerMask wallLayer; // Layer של קירות
        [SerializeField] private Transform playerHead; // Transform של ראש השחקן

        private float originalFOV;
        private bool isInCloseUpMode = false;

        private void Start()
        {
            originalFOV = virtualCamera.Lens.FieldOfView;
        }

        private void Update()
        {
            CheckWallProximity();
        }

        private void CheckWallProximity()
        {
            // בדיקת מרחק מקיר
            Ray ray = new Ray(playerHead.position, virtualCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, minDistance, wallLayer))
            {
                // קרוב מדי לקיר - מעבר למצב Close Up
                EnterCloseUpMode(hit.distance);
            }
            else
            {
                // לא קרוב לקיר - חזרה למצב רגיל
                ExitCloseUpMode();
            }
        }

        private void EnterCloseUpMode(float distanceToWall)
        {
            if (!isInCloseUpMode)
            {
                isInCloseUpMode = true;

                // שינוי FOV באופן הדרגתי
                virtualCamera.Lens.FieldOfView = Mathf.Lerp(maxZoom, minZoom, 1 - distanceToWall / minDistance);

                // יכול להוסיף אנימציה או התאמות נוספות כאן
            }
        }

        private void ExitCloseUpMode()
        {
            if (isInCloseUpMode)
            {
                isInCloseUpMode = false;

                // החזרה ל-FOV המקורי באופן הדרגתי
                virtualCamera.Lens.FieldOfView = Mathf.Lerp(virtualCamera.Lens.FieldOfView, originalFOV, Time.deltaTime * 5f);
            }
        }
    }
}
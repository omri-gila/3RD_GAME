using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class CameraCursor : MonoBehaviour
    {
        [SerializeField] private InputActionReference cameraToggleInputAction;
        [SerializeField] private bool startHidden;
        [SerializeField] private CinemachineInputAxisController inputProvider;
        [SerializeField] private bool disableCameraLookOnCursorVisible;
        [SerializeField] private bool disableCameraZoomOnCursorVisible;

        private void Awake()
        {
            if (cameraToggleInputAction != null)
            {
                cameraToggleInputAction.action.started += OnCameraCursorToggled;
            }

            if (startHidden)
            {
                ToggleCursor();
            }
        }

        private void OnEnable()
        {
            if (cameraToggleInputAction != null)
            {
                cameraToggleInputAction.asset.Enable();
            }
        }

        private void OnDisable()
        {
            if (cameraToggleInputAction != null)
            {
                cameraToggleInputAction.asset.Disable();
            }
        }

        private void OnCameraCursorToggled(InputAction.CallbackContext context)
        {
            ToggleCursor();
        }

        private void ToggleCursor()
        {
            Cursor.visible = !Cursor.visible;

            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;

                if (inputProvider != null)
                {
                    inputProvider.enabled = true;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;

                if (inputProvider != null)
                {
                    inputProvider.enabled = false;

                    if (disableCameraLookOnCursorVisible)
                    {
                        // מצא את הדרך הנכונה לנטרל קלט במערכת החדשה של Cinemachine
                        // ייתכן שתצטרך להשתמש בשיטה שונה בהתאם לגרסה
                    }

                    if (disableCameraZoomOnCursorVisible)
                    {
                        // מצא את הדרך הנכונה לנטרל זום במערכת החדשה של Cinemachine
                    }
                }
            }
        }

        // הוסף את זה אם אתה רוצה לוודא שהעכבר ננעל בהתחלה
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
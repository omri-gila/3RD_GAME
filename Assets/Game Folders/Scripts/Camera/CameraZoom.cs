using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public CinemachineFollowZoom followZoom; 
    public float zoomSpeed = 1.0f; 
    public float fovSpeed = 10.0f;
    public float minFovRange = 10f, maxFovRange = 80f; 
    public float minWidth = 1f, maxWidth = 5f; 

    public InputAction zoomAction; 

    private void OnEnable()
    {
        zoomAction.Enable();
    }

    private void OnDisable()
    {
        zoomAction.Disable(); 
    }

    void Update()
    {
        float scroll = zoomAction.ReadValue<float>(); 

        if (followZoom != null)
        {
            followZoom.Width += scroll * zoomSpeed;
            followZoom.Width = Mathf.Clamp(followZoom.Width, minWidth, maxWidth);

          
            Vector2 newFovRange = followZoom.FovRange;
            newFovRange.x = Mathf.Clamp(newFovRange.x + (scroll * fovSpeed), minFovRange, maxFovRange);
            newFovRange.y = Mathf.Clamp(newFovRange.y + (scroll * fovSpeed), minFovRange, maxFovRange);
            followZoom.FovRange = newFovRange;
        }
    }
}

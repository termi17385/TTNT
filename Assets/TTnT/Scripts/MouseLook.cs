using UnityEngine;

/// <summary>
/// Allows for the rotation of the camera to be controlled by the mouse
/// </summary>
public class MouseLook : MonoBehaviour
{
    /// <summary>
    /// Enumeration for the two axes of rotational movement of the camera
    /// </summary>
    public enum RotationalAxis
    {
        MouseX,
        MouseY
    }
    /// <summary>
    /// Header for rotation variables
    /// </summary>
    [Header("Rotation Variables")]
    // Sets axis as MouseX by default
    public RotationalAxis axis = RotationalAxis.MouseX;
    /// <summary>
    /// A range for the sensitivity of the camera rotation
    /// </summary>
    [Range(0, 500)] public float sensitivity = 300;
    /// <summary>
    /// The minimum and maximum values for y rotation
    /// </summary>
    public float minY = -60, maxY = 60;
    /// <summary>
    /// The value for y rotation
    /// </summary>
    private float _rotY;
    private void Start()
    {
        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Makes the cursor invisible
        Cursor.visible = false;
        // If the object this script is attached to has a camera component
        if (GetComponent<Camera>())
        {
            // Set axis to mouseY
            axis = RotationalAxis.MouseY;
        }
    }
    private void Update()
    {
        // If axis is MouseX
        if (axis == RotationalAxis.MouseX)
        {
            // Change y angle rotation based on the Mouse X settings in Unity Project Settings
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0);
        }
        // If axis is MouseY
        else
        {
            // Change rotY based on the Mouse Y settings in Unity Project Settings and clamp it based on the min and max Y values
            _rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            _rotY = Mathf.Clamp(_rotY, minY, maxY);
            // Change X angle rotation based on rotY
            transform.localEulerAngles = new Vector3(-_rotY, 0, 0);
        }
    }
}
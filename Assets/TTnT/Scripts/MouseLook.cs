using UnityEngine;
public class MouseLook : MonoBehaviour
{
	[SerializeField] private Transform head;
	[SerializeField, Range(0, 500)] public float sensitivity = 300;
	[SerializeField] private float minY = -60, maxY = 60;
	private float rotY;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		// handles the characters movement
		var mouseX = Input.GetAxis("Mouse X") * sensitivity;
		transform.Rotate(0, mouseX * Time.deltaTime, 0);

		// handles the camera's movement
		rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
		rotY = Mathf.Clamp(rotY, minY, maxY);

		// the head of the player (camera /or actual head)
		head.localRotation = Quaternion.Euler(rotY, 0, 0);
	}
}
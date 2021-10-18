using UnityEngine;
public class MouseLook : MonoBehaviour
{
	[SerializeField] private Transform head;
	[SerializeField, Range(0, 500)] public float sensitivity = 300;
	[SerializeField] private float minY = -60, maxY = 60;
	private float rotY;
	private bool isUsingMenu = false;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		if (!isUsingMenu)
		{
			// handles the characters movement
			var mouseX = Input.GetAxis("Mouse X") * sensitivity;
			transform.Rotate(0, mouseX * Time.deltaTime, 0);

			// handles the camera's movement
			rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
			rotY = Mathf.Clamp(rotY, minY, maxY);

			// the head of the player (camera /or actual head)
			head.localRotation = Quaternion.Euler(rotY, 0, 0);

			// Unlocks the mouse and stops camera rotation so you can use menus
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				isUsingMenu = true;
			}
		}
		else
		{
			// Locks cursor again so you can use mouse to look around
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				isUsingMenu = false;
			}
		}
	}
}
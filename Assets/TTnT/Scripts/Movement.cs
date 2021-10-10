using UnityEngine;

/// <summary>
/// Responsible for controlling character movement using the input settings from the Unity Project Settings
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    /// <summary>
    /// The character controller attached to the player character's
    /// </summary>
    private CharacterController playerChar;
    /// <summary>
    /// The speed of character movement
    /// </summary>
    public float speed = 10f;
    /// <summary>
    /// Speed of the jump
    /// </summary>
    public float jumpSpeed = 15f;
    /// <summary>
    /// The force of gravity applied to the character
    /// </summary>
    public float gravity = 1f;
    /// <summary>
    /// Pause Menu canvas
    /// </summary>
    //public GameObject menuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        // Sets playerChar to the CharacterController attached to the player
        playerChar = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        // Sets move based on the player character's rotation and input
        Vector3 move = Quaternion.Euler(0, playerChar.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move *= speed;
        if (playerChar.isGrounded && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("JUMP CHECK");
            move.y = jumpSpeed;
        }
        move.y -= gravity * Time.deltaTime;
        // Updates the player character's position based on move
        playerChar.Move(move * Time.deltaTime);
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuCanvas.activeSelf)
            {
                Time.timeScale = 1;
                menuCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                menuCanvas.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }*/
    }
}
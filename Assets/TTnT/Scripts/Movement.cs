using System;

using UnityEngine;

/// <summary>/// Responsible for controlling character movement using the input settings from the Unity Project Settings/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    /// <summary> The character controller attached to the player character's </summary>
    private CharacterController playerChar;
    /// <summary> The speed of character movement </summary>
    [SerializeField] private float speed = 10f;
    [SerializeField] private float vSpeed = 0;
    /// <summary> Speed of the jump </summary>
    [SerializeField] private float jumpSpeed = 15f;
    /// <summary> The force of gravity applied to the character </summary>
    private float gravity = 9.8f;
    [SerializeField] private float gravityModifier = 1f;

    private bool grounded;
    private bool resetGravity;

    void Start()
    {
        // Sets playerChar to the CharacterController attached to the player
        playerChar = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();
        
        //// Sets move based on the player character's rotation and input
        //Vector3 move = Quaternion.Euler(0, playerChar.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //move *= speed;
        //if (playerChar.isGrounded && Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("JUMP CHECK");
        //    move.y = jumpSpeed;
        //}
        //move.y -= gravity * Time.deltaTime;
        //// Updates the player character's position based on move
        //playerChar.Move(move * Time.deltaTime);
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

    /// <summary> Handles the movement
    /// and jumping of the player </summary>
    private void PlayerMovement()
    {
        // these handle the input axis's
        var h = Input.GetAxis("Horizontal") * speed;
        var v = Input.GetAxis("Vertical") * speed;

        // handles the movement and rotation
        Vector3 vel = Quaternion.Euler(0, playerChar.transform.eulerAngles.y, 0) * new Vector3(h, 0, v);
        if(grounded) // checking if grounded
        {
            //todo set a 1 time velocity check thing 
            if(resetGravity) // making sure gravity is only reset once
            {
                vSpeed = 0; // stop the character moving down when grounded
                resetGravity = false; 
            }
            if(Input.GetKeyDown(KeyCode.Space)) // checking if the player has jumped
            {
                Debug.Log("jump pressed");
                vSpeed = jumpSpeed; // sets the upwards velocity
                //el.y = vSpeed;
                //layerChar.Move(vel * Time.deltaTime);
            }
            //Debug.Log("Controller" + playerChar.isGrounded);
            //Debug.Log("Collider" + grounded);
        }
        // if the player has left the ground
        if(!grounded)
        {
            // activates gravity
            vSpeed -= gravity * gravityModifier * Time.deltaTime;
            resetGravity = true; // primes the reset
        }
        vel.y = vSpeed;
        playerChar.Move(vel * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider _other) { if(_other.gameObject.CompareTag("Ground")) grounded = true; }
    private void OnTriggerExit(Collider _other) { if(_other.gameObject.CompareTag("Ground")) grounded = false; }
}
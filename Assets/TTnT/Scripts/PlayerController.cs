using System;
using UnityEngine;

namespace TTnT.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
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

        private void OnTriggerEnter(Collider _other) { if(!_other.gameObject.CompareTag("Player")) grounded = true; }
        private void OnTriggerExit(Collider _other) { if(!_other.gameObject.CompareTag("Player")) grounded = false; }
    }
}

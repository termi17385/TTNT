using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaileyPlayerController : MonoBehaviour
{
    public float defaultSpeed = 5f;
    public float rotationSpeed = 100.0f;
    public float jumpSpeed;
    public Animator anim;

    private float currentSpeed;
    private float sprintSpeed;

    bool shiftPressed;
    bool turning;
    bool turnblock;
    bool mouseClicked;
    bool ctrlPressed;

    float crouchVal;



    // Start is called before the first frame update
    void Start()
    {
        sprintSpeed = defaultSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        turning = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        turnblock = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W);
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        mouseClicked = Input.GetMouseButtonDown(0);
        ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        {

            anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
            anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

            float translation = Input.GetAxis("Vertical") * currentSpeed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

            // Make it move 10 meters per second instead of 10 meters per frame...
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;

            // Move translation along the object's z-axis
            transform.Translate(0, 0, translation);

            // Rotate around our y-axis
            transform.Rotate(0, rotation, 0);

            if (shiftPressed)
            {
                Sprinting();
            }
            else NotSprinting();

            if (turning)
            {
                Turning();
            }
            else NotTurning();

            if (mouseClicked)
            {
                print("fire");
                Shoot();
            }
            else NoShoot();

            if (ctrlPressed)
            {
                Crouch();
            }
            else NoCrouch();

        }

        void Sprinting()
        {
            currentSpeed = sprintSpeed;
            anim.SetBool("Sprinting", true);
        }

        void NotSprinting()
        {
            currentSpeed = defaultSpeed;
            anim.SetBool("Sprinting", false);
        }

        void Turning()
        {
            if (turnblock)
            {
                anim.SetBool("Turning", false);
            }
            else anim.SetBool("Turning", true);
        }
        void NotTurning()
        {
            anim.SetBool("Turning", false);
        }
        void Shoot()
        {
            anim.SetBool("Shoot", true);
        }
        void NoShoot()
        {
            anim.SetBool("Shoot", false);
        }
        void Crouch()
        {
            while (crouchVal > (int)(0)) 
            {
                crouchVal -= (int)(0.1) * Time.deltaTime;
            }
            anim.SetBool("Crouched", true);
            anim.SetFloat("Crouch", crouchVal);
        }

        void NoCrouch()
        {
            if (crouchVal == (int)(0))
            {
                crouchVal += (int)(0.1) * Time.deltaTime;
            }
            anim.SetBool("Crouched", false);
            anim.SetFloat("Crouch", crouchVal);
        }
    }
}

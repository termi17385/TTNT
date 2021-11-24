using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float sprintSpeed = 7.5f;
    public float currentSpeed = 5f;
    public float speed = 5f;
    public float rotationSpeed = 100f;
    float defaultWeight = 0f;
    float newWeight;
    float pistolWeight;
    float rifleWeight;

    private Animator anim;
    public RuntimeAnimatorController assaultrifle;
    public RuntimeAnimatorController handgun;
    bool fire;
    bool reload;
    bool pistol;
    bool rifle;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        pistol = Input.GetKey(KeyCode.Alpha1);
        rifle = Input.GetKey(KeyCode.Alpha2);
        fire = Input.GetKeyDown(KeyCode.Mouse0);
        reload = Input.GetKeyDown(KeyCode.R);

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


        if (fire)
        {
            Firing();
        }

        if (reload)
        {
            Reloading();
        }
        if (pistol && anim.runtimeAnimatorController == assaultrifle)
        {
            UsingPistol();
            anim.runtimeAnimatorController = handgun as RuntimeAnimatorController;
        }

        if (rifle && anim.runtimeAnimatorController == handgun)
        {
            UsingRifle();
            anim.runtimeAnimatorController = assaultrifle as RuntimeAnimatorController;
        }
    }

    void Firing()
    {
        anim.SetTrigger("Firing");
    }
    void Reloading ()
    {
        anim.SetTrigger("Reloading");
    }
    void UsingPistol()
    {
        anim.SetBool("UsingPistol", true);
        anim.SetBool("UsingRifle", false);


        anim.runtimeAnimatorController = handgun as RuntimeAnimatorController;

    }
    void UsingRifle()
    {
        anim.SetBool("UsingRifle", true);
        anim.SetBool("UsingPistol", false);

        anim.runtimeAnimatorController = assaultrifle as RuntimeAnimatorController;
    }
}

using Mirror;
using TTnT.Player.Weapons;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    private float nextFire;
    private float fireRate;
    
    [Header("Additional Variables")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private WeaponManger weaponManger;
    [SerializeField] private BaseWeapon currentWeapon;
    [SerializeField] private RuntimeAnimatorController[] animControllers;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private AudioSource gunSound;
    //[SerializeField] private ParticleSystem muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = weaponManger.EquipedWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer)
        {
            if(currentWeapon.SwapFireMode() && Time.time > nextFire) Shoot();
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeapon.gameObject.SetActive(false);
                weaponManger.SwapWeapon(0);
                currentWeapon = weaponManger.EquipedWeapon();
                
                currentWeapon.gameObject.SetActive(true);
                playerAnim.runtimeAnimatorController = animControllers[0];
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeapon.gameObject.SetActive(false);
                weaponManger.SwapWeapon(1);
                currentWeapon = weaponManger.EquipedWeapon();
                
                currentWeapon.gameObject.SetActive(true);
                playerAnim.runtimeAnimatorController = animControllers[1];
            }    
        }
    }
    
    public void Shoot()
    {
        // if mouse is pressed and the time between shots is higher then next fire
        {
            Debug.Log("Shot");
            // set the next fire
            nextFire = Time.time + currentWeapon.fireRate;
            // sets the ray to the center of the screen/camera viewport
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            //playerManager.CmdStartParticles(); 
            
            CmdOnShoot();
            // shoot the target
            if(Physics.Raycast(rayOrigin, transform.forward, out hit, currentWeapon.weaponRange, ignorePlayer))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    var remotePlayerID = hit.collider.GetComponentInParent<NetworkIdentity>().netId;
                    CmdHitTarget(remotePlayerID);
                    Debug.Log($"Player {hit.collider.gameObject.name} {remotePlayerID} hit!");
                }
            }
        }
    }
    
    [Command]
    public void CmdHitTarget(uint _id)
    {
        var getTarget = CustomNetworkManager.FindPlayer(_id).GetComponent<NetworkCharacter>();
        getTarget.RpcDamage(currentWeapon.damage);
    }


    [Command]
    private void CmdOnShoot()
    {
        RpcEffect();
    }
		
    [ClientRpc]
    public void RpcEffect()
    {
        //muzzleFlash.Play();
        gunSound = currentWeapon.GetComponent<AudioSource>();
        gunSound.clip = currentWeapon.shootEffect;
        gunSound.Play();
    }
    
}

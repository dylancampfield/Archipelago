using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private Animator zoomAnimation;
    private Camera mainCamera;
    private GameObject crosshair;
    private WeaponManager weaponManager;
    private float nextTimeToFire; //Amount of time between firing (pressing and releasing fire button)
    private bool isZoomed;
    private bool isAiming;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowStartPosition;

    public float fireRate = 15f; //For assault rifle, how fast the gun can shoot
    public float damage = 20f; //Base damage

    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        zoomAnimation = transform.Find("Look").transform.Find("FPCamera").GetComponent<Animator>();

        crosshair = GameObject.FindWithTag("Crosshair");

        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShootWeapon();
        ZoomInAndOut();
    }

    void ShootWeapon()
    {
        if (weaponManager.GetCurrentWeapon().fireType == WeaponFireType.AUTO) //For assault rifle
        {
            //If holding fire button (Left Mouse Button)
            //AND if Time elapsed is greater than time it takes to start firing again
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weaponManager.GetCurrentWeapon().ShootAnimation();

                FireBullet();
            }
        }
        else //Regular weapon with singular fire rate (revolver, shotgun, etc.)
        {
            if (Input.GetMouseButtonDown(0)) //Fire button pressed only once
            {
                if (weaponManager.GetCurrentWeapon().tag == "Axe") //Handles axe attack
                {
                    weaponManager.GetCurrentWeapon().ShootAnimation();
                }

                if (weaponManager.GetCurrentWeapon().bulletType == WeaponBulletType.BULLET) //Handles revolver & shotgun
                {
                    weaponManager.GetCurrentWeapon().ShootAnimation();

                    FireBullet();
                }
                else //Handles bow & arrow
                {
                    if (isAiming)
                    {
                        weaponManager.GetCurrentWeapon().ShootAnimation();

                        if (weaponManager.GetCurrentWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            //Create and shoot arrow
                            LaunchArrow(true);
                        }
                    }
                }
            }
        }
    }

    void ZoomInAndOut()
    {
        //Weapons that aim with the zoom animation
        if (weaponManager.GetCurrentWeapon().weaponAim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomAnimation.Play("ZoomIn");

                //While zooming, turn off crosshair to use iron sights
                crosshair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomAnimation.Play("ZoomOut");

                //Reactivate crosshair when not zooming
                crosshair.SetActive(true);
            }
        }
        //Handling bow and arrow aim; has built-in aim animation
        if (weaponManager.GetCurrentWeapon().weaponAim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentWeapon().AimAnimation(true); //Play aiming animation
                isAiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentWeapon().AimAnimation(false); //Stop playing animation
                isAiming = false;
            }
        }
    }

    void LaunchArrow(bool launchArrow)
    {
        if (launchArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);

            arrow.transform.position = arrowStartPosition.position;

            arrow.GetComponent<BowArrow>().Launch(mainCamera);
        }
    }

    void FireBullet()
    {
        RaycastHit hit;

        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            if(hit.transform.tag == "Enemy") //If the tag of the object hit is "Enemy"
            {
                hit.transform.GetComponent<HealthScript>().DealDamage(damage); //Apply damage 
            }
        }
    }
}

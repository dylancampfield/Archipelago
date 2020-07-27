using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim {
    NONE, SELF_AIM, AIM
}

public enum WeaponFireType {
    SINGLE, AUTO
}

public enum WeaponBulletType {
    NONE, BULLET, ARROW
}

public class WeaponHandler : MonoBehaviour
{

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private AudioSource shoot, reload;
    private Animator anim;

    public WeaponAim weaponAim;
    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject pointOfAttack; //Determine if weapon collided with enemy
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootAnimation() {
        anim.SetTrigger("Shoot");
    }

    public void AimAnimation(bool canAim) {
        anim.SetBool("Aim", canAim);
    }

    void ActivateMuzzleFlash() {
        muzzleFlash.SetActive(true);
    }

    void DeactivateMuzzleFlash() {
        muzzleFlash.SetActive(false);
    }

    void PlayGunshot() {
        shoot.Play();
    }

    void PlayReload() {
        reload.Play();
    }

    void ActivateAttackPoint() {
        pointOfAttack.SetActive(true);
    }

    void DeactivateAttackPoint() {
        if(pointOfAttack.activeInHierarchy) {
            pointOfAttack.SetActive(false);
        }
    }
}

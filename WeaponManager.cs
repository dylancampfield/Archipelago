using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private WeaponHandler[] weapons;

    private int currentWeaponIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponIndex = 0; 
        weapons[currentWeaponIndex].gameObject.SetActive(true); //Equip default weapon
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectWeapon(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectWeapon(4);
        }
    }

    void SelectWeapon(int weaponIndex)
    {
        if (currentWeaponIndex == weaponIndex) //Don't repeat animation if same weapon selected
            return;

        weapons[currentWeaponIndex].gameObject.SetActive(false); //Holster current weapon
        weapons[weaponIndex].gameObject.SetActive(true); //Equip selected weapon

        currentWeaponIndex = weaponIndex; //Update current index
    }

    public WeaponHandler GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}

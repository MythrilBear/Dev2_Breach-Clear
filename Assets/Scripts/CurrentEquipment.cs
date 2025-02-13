using NUnit.Framework;
using UnityEngine;

public class CurrentEquipment : MonoBehaviour
{
    private GameObject primaryWeapon;
    public GameObject rifle;
    public GameObject submachinegun;
    public GameObject shotgun;
    public GameObject secondaryWeapon;
    public GameObject grenade;
    public GameObject knife;
    public GameObject specialEquipment;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance.equipmentLoadout == 0)
        {
            primaryWeapon = rifle;
        }
        else if (GameManager.instance.equipmentLoadout == 1)
        {
             primaryWeapon = submachinegun;
        }
        else
        {
            primaryWeapon = shotgun;
        }
        primaryWeapon.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (primaryWeapon.activeSelf)
            {
                primaryWeapon.SetActive(false);
                specialEquipment.SetActive(true);
                GameManager.instance.updateAmmoCount(0);
            }
            else if (specialEquipment.activeSelf)
            {
                specialEquipment.SetActive(false);
                grenade.SetActive(true);
                GameManager.instance.updateAmmoCount(0);
            }
            else if (grenade.activeSelf)
            {
                grenade.SetActive(false);
                secondaryWeapon.SetActive(true);
                
            }
            else if (secondaryWeapon.activeSelf)
            {
                secondaryWeapon.SetActive(false);
                primaryWeapon.SetActive(true);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (primaryWeapon.activeSelf)
            {
                primaryWeapon.SetActive(false);
                secondaryWeapon.SetActive(true);
            }
            else if (secondaryWeapon.activeSelf)
            {
                secondaryWeapon.SetActive(false);
                grenade.SetActive(true);
            }
            else if (grenade.activeSelf)
            {
                grenade.SetActive(false);
                specialEquipment.SetActive(true);
            }
            else if (specialEquipment.activeSelf)
            {
                specialEquipment.SetActive(false);
                primaryWeapon.SetActive(true);
            }

        }
        
    }
}

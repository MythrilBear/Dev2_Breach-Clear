using NUnit.Framework;
using UnityEngine;

public class CurrentEquipment : MonoBehaviour
{
    private GameObject primaryWeapon;
    public GameObject rifle;
    public GameObject submachinegun;
    public GameObject shotgun;
    public GameObject secondaryWeapon;
    public GameObject heldGrenade;
    public GameObject tossedGrenade;
    public GameObject knife;
    public GameObject specialEquipment;
    public GameObject doorbreaker;
    public GameObject claymore;
    public GameObject riotShield;

    void Awake()
    {
        int savedLoadout = PlayerPrefs.GetInt("SelectedLoadout", 0); 
        AssignLoadOut(savedLoadout);
    }
    public void AssignLoadOut(int loadoutSelection)
    {
       
        if (loadoutSelection == 0)
        {
            primaryWeapon = rifle;
            specialEquipment = claymore;
        }
        else if (loadoutSelection == 1)
        {
            primaryWeapon = submachinegun;
            specialEquipment = doorbreaker;
        }
        else if (loadoutSelection == 2)
        {
            primaryWeapon = shotgun;
            specialEquipment = riotShield;
        }
        if (primaryWeapon != null) primaryWeapon.SetActive(true);
        //if (secondaryWeapon != null) secondaryWeapon.SetActive(true);

    }

    //Update is called once per frame
    void Update()
    {
        if (primaryWeapon == null) return;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !GameManager.instance.isReloading && !GameManager.instance.isAiming)
        {
            if (primaryWeapon.activeSelf)
            {
                primaryWeapon.SetActive(false);
                knife.SetActive(true);

                GameManager.instance.ammoDisplayText.SetActive(false);
                GameManager.instance.ammoCountDisplay.SetActive(false);
                GameManager.instance.ammoDisplaySlash.SetActive(false);
                GameManager.instance.ammoReserveDisplay.SetActive(false);
            }
            else if (knife.activeSelf)
            {
                knife.SetActive(false);
                //specialEquipment.SetActive(true);
                heldGrenade.SetActive(true);
                //GameManager.instance.updateAmmoCount(-1);

                GameManager.instance.ammoDisplayText.SetActive(true);
                GameManager.instance.ammoCountDisplay.SetActive(true);
                
            }
            //else if (specialEquipment.activeSelf)
            //{
            //    specialEquipment.SetActive(false);
            //    heldGrenade.SetActive(true);
            //    //tossedGrenade.SetActive(true);
            //    //Destroy(tossedGrenade);
            //    GameManager.instance.updateAmmoCount(0);
            //}
            else if (heldGrenade.activeSelf)
            {
                heldGrenade.SetActive(false);
                //tossedGrenade.SetActive(false);
                secondaryWeapon.SetActive(true);
                GameManager.instance.ammoDisplaySlash.SetActive(true);
                GameManager.instance.ammoReserveDisplay.SetActive(true);

            }
            else if (secondaryWeapon.activeSelf)
            {
                secondaryWeapon.SetActive(false);
                primaryWeapon.SetActive(true);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !GameManager.instance.isReloading && !GameManager.instance.isAiming)
        {
            if (primaryWeapon.activeSelf)
            {
                primaryWeapon.SetActive(false);
                secondaryWeapon.SetActive(true);
            }
            else if (secondaryWeapon.activeSelf)
            {
                secondaryWeapon.SetActive(false);
                heldGrenade.SetActive(true);
                //tossedGrenade.SetActive(true);
                //Destroy(tossedGrenade);
                GameManager.instance.ammoDisplaySlash.SetActive(false);
                GameManager.instance.ammoReserveDisplay.SetActive(false);
            }
            else if (heldGrenade.activeSelf)
            {
                heldGrenade.SetActive(false);
                knife.SetActive(true);
                GameManager.instance.ammoCountDisplay.SetActive(false);
                GameManager.instance.ammoDisplayText.SetActive(false);
                //tossedGrenade.SetActive(false);
                //specialEquipment.SetActive(true);
                //GameManager.instance.updateAmmoCount(-1);
            }
            //else if (specialEquipment.activeSelf)
            //{
            //    specialEquipment.SetActive(false);
            //    knife.SetActive(true);
            //    GameManager.instance.updateAmmoCount(-1);
            //}
            else if (knife.activeSelf)
            {
                knife.SetActive(false);
                primaryWeapon.SetActive(true);

                GameManager.instance.ammoDisplayText.SetActive(true);
                GameManager.instance.ammoDisplaySlash.SetActive(true);
                GameManager.instance.ammoReserveDisplay.SetActive(true);
                GameManager.instance.ammoCountDisplay.SetActive(true);
                
            }

        }

    }

}

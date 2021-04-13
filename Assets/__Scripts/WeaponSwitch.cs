using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the gun container has children (gun is in inventory)
        //If the "1" key is pressed
        //If the gun is currently not equipped
        if (transform.childCount > 0 && Input.GetKeyDown(KeyCode.Alpha1) && PickUpController.equipped == false)
        {
            //Select the gun
            SelectWeapon();
            
            //The gun is selected
            PickUpController.equipped = true;
        }

        //If the gun container has children (gun is in inventory)
        //If the "1" key is pressed
        //If the gun is currently equipped
        else if (transform.childCount > 0 && Input.GetKeyDown(KeyCode.Alpha1) && PickUpController.equipped == true)
        {
            //Deselect the gun
            DeselectWeapon();

            //The gun is not selected
            PickUpController.equipped = false;
        }
    }

    /// <summary>
    /// Selects the gun
    /// </summary>
    void SelectWeapon()
    {
        //Creating and assigning a variable to reference the hand gun transform
        Transform handGun = gameObject.transform.GetChild(0);

        //Set the hand gun game object to active 
        handGun.gameObject.SetActive(true);
    }

    /// <summary>
    /// Deselects the gun
    /// </summary>
    void DeselectWeapon()
    {
        //Creating and assigning a variable to reference the hand gun transform
        Transform handGun = gameObject.transform.GetChild(0);

        //Set the hand gun game object to unactive 
        handGun.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSet
    {
        public List<GameObject> objectsToEnable;
        public List<GameObject> objectsToDisable;
    }

    public List<WeaponSet> weapons; // List of weapon sets

    private int currentWeaponIndex = 0; // Index of the current weapon

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that only the first weapon is initially active
        SetActiveWeapon(currentWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Switch weapons using scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            ChangeWeapon(scroll > 0 ? 1 : -1);
        }

        // Switch weapons using number keys
        for (int i = 1; i <= weapons.Count; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                ChangeWeapon(i - 1);
            }
        }
    }

    // Switches to the next or previous weapon based on the direction parameter
    void ChangeWeapon(int direction)
    {
        SetActiveWeapon(currentWeaponIndex + direction);
    }

    // Sets the active weapon based on the given index
    void SetActiveWeapon(int index)
    {
        // Ensure the index is within valid range
        index = Mathf.Clamp(index, 0, weapons.Count - 1);

        // Disable the current weapon
        SetWeaponObjects(weapons[currentWeaponIndex].objectsToEnable, false);
        SetWeaponObjects(weapons[currentWeaponIndex].objectsToDisable, true);

        // Enable the new weapon
        SetWeaponObjects(weapons[index].objectsToEnable, true);
        SetWeaponObjects(weapons[index].objectsToDisable, false);

        // Update the current weapon index
        currentWeaponIndex = index;
    }

    // Helper method to enable/disable objects in a list
    void SetWeaponObjects(List<GameObject> objects, bool setActive)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(setActive);
        }
    }
}

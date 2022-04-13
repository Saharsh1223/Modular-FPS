//Very (very) basic weapon selection system which can be expandable!

using System;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    [HideInInspector] public bool canSwitchWeapons;

    private void Start()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[0].SetActive(true);
    }

    private void Update()
    {
        if (canSwitchWeapons)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                foreach (GameObject weapon in weapons)
                {
                    weapon.SetActive(false);
                }
                weapons[0].SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (GameObject weapon in weapons)
                {
                    weapon.SetActive(false);
                }
                weapons[1].SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                foreach (GameObject weapon in weapons)
                {
                    weapon.SetActive(false);
                }
                weapons[2].SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                foreach (GameObject weapon in weapons)
                {
                    weapon.SetActive(false);
                }
                weapons[3].SetActive(true);
            }
        }
    }
}

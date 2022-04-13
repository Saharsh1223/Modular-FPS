//Simple and easy weapon shooting implementation

using System;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform scopedTransform;
    [SerializeField] private Transform normalTransform;
    [Space]
    [SerializeField] private Transform shootPoint;
    [Space]
    [SerializeField] private Camera cam;
    [Space]
    [SerializeField] private GameObject crossHair;
    [SerializeField] private Animator animator;
    [Space] 
    [SerializeField] private TMP_Text ammoText;

    [Header("Settings")]
    [SerializeField] private int magSize = 10;
    [SerializeField] private int amountLeft = 0;
    [SerializeField] private float reloadTime = 1f;
    [Space]
    [Range(0f, 10f)] [SerializeField] private float lerpTime = 10f;
    [Space]
    [SerializeField] private float scopedFOV = 42.5f;
    [SerializeField] private float normalFOV = 65f;
    [Space]
    [SerializeField] private float scopedSwaySpeed = 4f;
    [SerializeField] private float normalSwaySpeed = 8f;
    [Space]
    [SerializeField] private bool automatic;

    [Header("Shooting")]
    [SerializeField] private float shootForce = 3f;
    [SerializeField] private float fireRate = 15f;
    private float nextTimeToFire = 0f;

    [Header("Camera Shake")]
    [SerializeField] private float intensity = 0.6f;
    [SerializeField] private float roughness = 0.8f;

    [Header("KeyCodes")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode scopeKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    [Header("Script References")]
    [SerializeField] private WeaponSelector selector;
    [SerializeField] private CameraShaker camShaker;
    [SerializeField] private WeaponSway sway;

    [Header("Debug")] 
    [SerializeField] private bool isScoped;
    [SerializeField] private bool isShooting;
    [SerializeField] private bool isReloading;

    private RaycastHit hit;

    private void Start()
    {
        amountLeft = magSize;
    }

    private void Update()
    {
        if (isReloading || isShooting)
        {
            selector.canSwitchWeapons = false;
        }
        else
        {
            selector.canSwitchWeapons = true;
        }
        
        HandleScoping();

        bool shootNonAuto = Input.GetKeyDown(shootKey);
        bool shootAuto = Input.GetKey(shootKey);
        bool reload = Input.GetKeyDown(reloadKey);

        if (automatic)
        {
            if (shootAuto)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }
        }
        /*else
        {
            if (shootNonAuto)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }
        }*/
        
        if (automatic)
        {
            if (shootAuto && !isReloading && Time.time >= nextTimeToFire)
            {
                StartShootingAnimationAuto();
                Shoot();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
            else
            {
                StopShootingAnimationAuto();
            }
        }
        else
        {
            if (shootNonAuto && !isReloading)
            {
                Shoot();
                StartShootingAnimationNonAuto();
                Invoke("StopShootingAnimationNonAuto", 0.09f);
            }
            else
            {
            }
        }

        if (isScoped && !isReloading)
        {
            sway.speed = scopedSwaySpeed;
        }
        else
        {
            sway.speed = normalSwaySpeed;
        }

        if (!isReloading && amountLeft != 0 && amountLeft != magSize && reload)
        {
            Reload();
        }
        else if (!isReloading && amountLeft == 0)
        {
            Reload();
        }

        SetAmmoText();
    }

    void Shoot()
    {
        if (Physics.Raycast(shootPoint.position, cam.transform.forward, out hit, 500f))
        {
            if (hit.rigidbody != null)
            {
                Vector3 forceDir = shootPoint.position - hit.point;
                hit.rigidbody.AddForce(-forceDir * shootForce);
                //Debug.Log("Adding force");
            }
        }

        amountLeft--;
        camShaker.ShakeOnce(intensity, roughness, 0.1f, 0.1f);
    }

    void Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);
        Invoke("StopReload", reloadTime);
    }

    void StopReload()
    {
        animator.SetBool("isReloading", false);
        amountLeft = magSize;
        isReloading = false;
    }

    void HandleScoping()
    {
        isScoped = Input.GetKey(scopeKey);

        if (isScoped && !isReloading)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, scopedTransform.position, Time.deltaTime * 12f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, scopedFOV, Time.deltaTime * lerpTime);
            
            //crossHair.SetActive(false);
            Image img = crossHair.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(img.color.a, 0f, Time.deltaTime * lerpTime));
        }
        else if (!isScoped && !isReloading)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, normalTransform.position, Time.deltaTime * 12f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * lerpTime);
            
            //crossHair.SetActive(true);
            Image img = crossHair.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(img.color.a, 1f, Time.deltaTime * lerpTime));
        }
        else if (isScoped && isReloading)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, normalTransform.position, Time.deltaTime * 12f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * lerpTime);
            
            //crossHair.SetActive(true);
            Image img = crossHair.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(img.color.a, 1f, Time.deltaTime * lerpTime));
        }
    }

    void StartShootingAnimationAuto()
    {
        animator.SetBool("isShooting", true);
    }

    void StopShootingAnimationAuto()
    {
        animator.SetBool("isShooting", false);
        animator.gameObject.transform.position = new Vector3(animator.gameObject.transform.position.x, animator.gameObject.transform.position.y, normalTransform.position.z);
    }

    void StartShootingAnimationNonAuto()
    {
        animator.SetBool("isShooting", true);
    }

    void StopShootingAnimationNonAuto()
    {
        animator.SetBool("isShooting", false);
        //animator.gameObject.transform.position = new Vector3(animator.gameObject.transform.position.x, animator.gameObject.transform.position.y, normalTransform.position.z);
    }

    void SetAmmoText()
    {
        if (!isReloading)
        {
            ammoText.text = magSize + "/" + amountLeft;
        }
        else
        {
            ammoText.text = "RELOADING";
        }
    }
}

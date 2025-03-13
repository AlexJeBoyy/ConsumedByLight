using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public BaseController controller;
    public Transform cameraTransform;
    public Recoil recoil;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        controller = transform.root.GetComponent<BaseController>();

    }

    private void Update()
    {
        recoil.ResetRecoil(gunData);
    }

    public void TryReloading()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunData.gunName + " is reloading");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + " is reloaded");
    }

    public void TryShoot()
    {
        if (isReloading == true || currentAmmo <= 0)
        {
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }

    }

    private void HandleShoot()
    {
        currentAmmo--;
        Debug.Log("shoot");
        Shoot();

        recoil.ApplyRecoil(gunData);
    }

    public abstract void Shoot();
}


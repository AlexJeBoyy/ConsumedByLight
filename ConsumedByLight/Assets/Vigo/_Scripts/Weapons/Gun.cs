using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public BaseController controller;
    public Transform GunMuzzle;
    public Transform cameraTransform;
    //public Recoil recoil;

    WeaponAnim weaponanim;
    private CinemachineImpulseSource recoilShakeImpulseSource;

    [SerializeField] private TextMeshProUGUI AmmoCount;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        weaponanim = GetComponent<WeaponAnim>();

        controller = transform.root.GetComponent<BaseController>();
        recoilShakeImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {

        if (!isReloading)
        {
            AmmoCount.text = currentAmmo.ToString() + " / " + gunData.magazineSize.ToString();
        }
        else
        {
            AmmoCount.text = "Reloading...";
        }

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
        weaponanim.isRecoiling = true;
        Shoot();
        recoilShakeImpulseSource.GenerateImpulse();

    }

    public abstract void Shoot();
}


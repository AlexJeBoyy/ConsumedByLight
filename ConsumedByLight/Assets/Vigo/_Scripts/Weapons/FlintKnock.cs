using System.Collections;
using UnityEngine;

public class FlintKnock : Gun
{
    public AudioSource ShootSound;
    
    public override void Shoot()
    {
        ShootSound.Play();
        RaycastHit hit;
        Vector3 target = Vector3.zero;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, gunData.shootingRange))
        {
            if (hit.collider.gameObject.layer == 6)
            {
                if (hit.collider.gameObject.GetComponent<EnemyHealth>() != null)
                {
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(hit.point);
                }
            }
            target = hit.point;
        }
        else
        {
            target = cameraTransform.position + cameraTransform.forward * gunData.shootingRange;
        }
        StartCoroutine(BulletFire(target));
    }

    private IEnumerator BulletFire(Vector3 target)
    {
        GameObject bulletTrial = Instantiate(gunData.bulletTrialPrefab, GunMuzzle.position, Quaternion.identity);

        while (bulletTrial != null && Vector3.Distance(bulletTrial.transform.position, target) > 0.1f)
        {
            bulletTrial.transform.position = Vector3.MoveTowards(bulletTrial.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }

        Destroy(bulletTrial);
    }
}

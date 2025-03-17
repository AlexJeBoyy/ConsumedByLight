using UnityEngine;

public class FlintKnock : Gun
{
    public override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, gunData.shootingRange))
        {
            Debug.Log(hit.collider.name);
        }
    }
}

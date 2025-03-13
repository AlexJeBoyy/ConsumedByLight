using UnityEngine;

public class Recoil : MonoBehaviour
{
    //THIS DOESNT DO ANYTHING AND WILL BE IMPLEMENTED WHEN ALEX IS DONE WITH THE PLAYERMOVEMENT THIS IS FOR THE CAMERA RECOIL

    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 currentRecoil = Vector3.zero;
    public void ApplyRecoil(GunData gunData)
    {
        float recoilX = Random.Range(-gunData.maxRecoil.x, gunData.maxRecoil.x) * gunData.recoilAmount;
        float recoilY = Random.Range(-gunData.maxRecoil.y, gunData.maxRecoil.y) * gunData.recoilAmount;

        targetRecoil += new Vector3(recoilX, recoilY, 0);

        currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.recoilSpeed);
    }

    public void ResetRecoil(GunData gunData)
    {
        currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
        targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
    }
}

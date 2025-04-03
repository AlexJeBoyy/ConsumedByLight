using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    Vector3 pos;

    public
        void OnHover()
    {
        pos = transform.position;
        pos.x += Input.GetAxis("Horizontal");
        pos.x = Mathf.Clamp(pos.x, -5f, -5f);
        transform.position = pos;
    }
}

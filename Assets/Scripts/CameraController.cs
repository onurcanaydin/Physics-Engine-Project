using UnityEngine;
using Vector3 = cyclone.Vector3;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    void LateUpdate()
    {
        Vector3 newCamPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = newCamPos.CycloneToUnity();
    }
}
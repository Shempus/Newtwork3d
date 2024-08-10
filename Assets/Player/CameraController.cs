using UnityEngine;
using Unity.Netcode;


public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;
    public bool cameraActive;

    public override void OnNetworkSpawn()
    { 
        cameraHolder.SetActive(IsOwner);
        cameraActive = IsOwner;
        base.OnNetworkSpawn();
    }

    public void Update()
    {
        cameraHolder.transform.position = transform.position + offset;
    }
}

using UnityEngine;
using Unity.Netcode;


public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;
 

    public override void OnNetworkSpawn()
    { 
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }



    public void Update()
    {
		// check scene
        cameraHolder.transform.position = transform.position + offset;
    }
}

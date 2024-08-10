using UnityEngine;
using Unity.Netcode;

/*
 * This handles setting the cameras active per player and keeping the camea holder facing the same way
 */
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

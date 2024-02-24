using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
//using Mirror;


public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;
    public bool cameraActive;

    public override void OnNetworkSpawn()
    { // This is basically a Start method
        cameraHolder.SetActive(IsOwner);
        cameraActive = IsOwner;
        base.OnNetworkSpawn(); // Not sure if this is needed though, but good to have it.
    }

    public void Update()
    {
        cameraHolder.transform.position = transform.position + offset;
    }
}

using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private Rigidbody rbPlayer;

    [SerializeField]
    private float speed = 30f;
    
    [SerializeField]
    Transform orientation;

    bool canJump = true;
    

    private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;

        if (context.performed && canJump)
        {
            canJump = false;
            jumpServerRPC(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    public void Move(InputAction.CallbackContext context) 
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            moveServerRPC(context.ReadValue<Vector3>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    [ServerRpc]
    void jumpServerRPC(Vector3 force, ForceMode mode)
    {
        rbPlayer.AddForce(force, mode);
    }

    [ServerRpc]
    void moveServerRPC(Vector3 vect)
    {

        rbPlayer.AddForce(orientation.rotation * vect * speed, ForceMode.Force);
    }
}

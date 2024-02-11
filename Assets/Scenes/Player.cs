using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private Rigidbody rbPlayer;

    [SerializeField]
    private float speed = 0.5f;


    private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;

        if (context.performed)
        {
            rbPlayer.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    public void Move(InputAction.CallbackContext context) 
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            //rbPlayer.MovePosition(context.ReadValue<Vector2>());
            //rbPlayer.AddForce(context.ReadValue<Vector2>() * 5f, ForceMode.Acceleration);

            Vector3  vect = context.ReadValue<Vector3>();

            //Vector3 moveDirection = new Vector3(vect.x, 0, vect.z).normalized;
            //transform.Translate(speed * Time.deltaTime * vect);
            rbPlayer.AddForce(vect * 5f, ForceMode.Impulse);
        }
    }
}

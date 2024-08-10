
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    private Rigidbody rbPlayer;

    [SerializeField]
    private float speed = 30f;
    
    [SerializeField]
    Transform orientation;

    [SerializeField]
    float jumpHeight = 0.5f;

    bool canJump = true;
    [SerializeField]
    float sensitivityX = 8.0f;

    [SerializeField]
    float sensitivityY = 0.5f;

    [SerializeField]
    float xClamp = 85.0f;

    float mouseX, mouseY;
    float xRotation = 0.0f;


	public int playerNumber;


	private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
		transform.name = System.Guid.NewGuid().ToString();
		Debug.Log("Player " + transform.name + " Connected");
		
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;

        if (context.performed && canJump)
        {
            canJump = false;
            JumpServerRPC(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void Move(InputAction.CallbackContext context) 
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            MoveServerRPC(context.ReadValue<Vector2>());
        }
    }

    public void Look(InputAction.CallbackContext context)
	{
		rbPlayer.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        if (!IsOwner) return;
        if (context.performed)
        {
            Vector2 lookValue = context.ReadValue<Vector2>();

            mouseX = lookValue.x * sensitivityX;
            mouseY = lookValue.y * sensitivityY;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

            Vector3 target = transform.eulerAngles;
            target.x = xRotation;
            orientation.eulerAngles = target;

            // using physics smooths out the animation - but need angular drag on the rigid body to stop it spinning.
            RotateServerRPC(Vector3.up * mouseX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

		if (!IsOwner) return; 

		if (other.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        } else
		{
			rbPlayer.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

		}

		if (other.gameObject.CompareTag("Finish"))
		{
			SomeoneWonServerRPC(transform.name);
		}
	}

    [ServerRpc]
    void JumpServerRPC(Vector3 force, ForceMode mode)
    {
        rbPlayer.AddForce(force, mode);
    }

    [ServerRpc]
    void MoveServerRPC(Vector2 vect)
    {
        Vector3 horizontalVelocity = (transform.right * vect.x + transform.forward * vect.y) * speed;
        rbPlayer.velocity = horizontalVelocity;
    }

    [ServerRpc]
    void RotateServerRPC(Vector3 tourque)
    {
        rbPlayer.AddTorque(tourque);
    }

	[ServerRpc]
	void SomeoneWonServerRPC(string id)
	{
		Debug.Log("Player " + id + " won");
		NetworkManager.SceneManager.LoadScene("Win", LoadSceneMode.Single);
	}
}

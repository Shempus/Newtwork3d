
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


/*
 *
 * Handles the input for the player
 * 
 */

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



	/*
	 * Initialises the player, assigns a unique 'name'
	 */
	private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
		transform.name = System.Guid.NewGuid().ToString();
		Debug.Log("Player " + transform.name + " Connected");
		
    }

	/*
	 * Actual movement happens on the server, this checks if we are the 'owner' i.e. this is inut happened
	 * on the computer that owns this instance of the player, if so and we are on the grounfd (canJump) calls the server to perform
	 */
	public void Jump(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;

        if (context.performed && canJump)
        {
            canJump = false;
            JumpServerRPC(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

	/*
	 *  As per jump, calls the server if this is the correct instance
	 */
    public void Move(InputAction.CallbackContext context) 
    {
        if (!IsOwner) return;

        if (context.performed)
        {
            MoveServerRPC(context.ReadValue<Vector2>());
        }
    }

	/*
	 * The mpuse handling for the camera and player are separate here, this needs more extensive testing but if the player 
	 * was more complex that a capsule I beleive the other players would need to see the rotation as well so would have to be done on the
	 * server
	 */
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

	/*
	 *  Using physics based control, which may have been a misake but I was thinking of puzzle games like portal/portal 2 where we would have traps
	 *  and possibly coop physics based puzzles
	 *  
	 *  This gave a few problems like weird rotation when hitting walls
	 *  
	 *  This function handles the fact we are on the ground so can jump
	 *  
	 *  Have hit a wall so don't rotate weirdly - not perfect
	 *  
	 *  Hit the finish sphere
	 *  
	 *  Finish has to call the server to handle showing new scenes over the network
	 */
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
		GameManager.winnerName = id;
		NetworkManager.SceneManager.LoadScene("Win", LoadSceneMode.Single);
	}
}

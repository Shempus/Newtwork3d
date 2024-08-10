using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class EndStats : NetworkBehaviour
{
	/*
	 * Used to update the end screen with the winner
	 */

    // Start is called before the first frame update
    void Start()
    {
		Debug.Log("Winner "+ GameManager.winnerName);
		DisplayWinnerServerRPC();
	}

	void DisplayWinner(string winner)
	{
		var text = GetComponent<TMP_Text>();
		text.text = winner+ " WINS";
	}


	// The server has the definitive winner name, so it needs to be called, but then call back to each client to update their screens
	[ServerRpc]
	void DisplayWinnerServerRPC()
	{
		DisplayWinner(GameManager.winnerName);
		DisplayWinnerClientRPC(GameManager.winnerName);
	}

	[ClientRpc]
	void DisplayWinnerClientRPC(string winner)
	{
		DisplayWinner(winner);
	}

}

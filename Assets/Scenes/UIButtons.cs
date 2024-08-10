
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ServerBtn;
    [SerializeField] private Button ClientBtn;


    private void Awake()
    {
        HostBtn.onClick.AddListener(() => { 
			NetworkManager.Singleton.StartHost();
			HideButtons();
		});
        ServerBtn.onClick.AddListener(() => { 
			NetworkManager.Singleton.StartServer();
			HideButtons();
		});
        ClientBtn.onClick.AddListener(() => { 
			NetworkManager.Singleton.StartClient();
			HideButtons();
		});
    }

	private void HideButtons()
	{
		HostBtn.gameObject.SetActive(false);
		ServerBtn.gameObject.SetActive(false);
		ClientBtn.gameObject.SetActive(false);
	}

}

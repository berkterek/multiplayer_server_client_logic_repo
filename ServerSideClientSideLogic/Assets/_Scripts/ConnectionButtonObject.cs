using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionButtonObject : MonoBehaviour
{
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;

    void OnEnable()
    {
        _hostButton.onClick.AddListener(HandleOnHostButtonClicked);
        _clientButton.onClick.AddListener(HandleOnClientButtonClicked);
    }

    void OnDisable()
    {
        _hostButton.onClick.RemoveListener(HandleOnHostButtonClicked);
        _clientButton.onClick.RemoveListener(HandleOnClientButtonClicked);
    }

    void HandleOnClientButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
    }

    void HandleOnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
        this.gameObject.SetActive(false);
    }
}

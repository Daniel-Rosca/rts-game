using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField addessInput = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable(){
        CustomNetworkManager.ClientOnConnected += HandleClientConnected;
        CustomNetworkManager.ClientOnConnected += HandleClientsDisconnected;
    }
    private void OnDisable(){
        CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
        CustomNetworkManager.ClientOnConnected -= HandleClientsDisconnected;  
    }
    
    public void Join(){
        string address = addessInput.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected(){
        joinButton.interactable = true;
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientsDisconnected(){
        joinButton.interactable = true;


    }
    
}

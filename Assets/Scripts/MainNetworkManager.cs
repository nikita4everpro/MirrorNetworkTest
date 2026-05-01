using Zenject;
using Mirror;
using UnityEngine;

// Основной NetworkManager
public class MainNetworkManager : NetworkManager
{
    [Inject] private MirrorMessagesWrapper _mirrorMessagesWrapper;  // Прослойка для сообщений Mirror

    [Header("UI")]
    [SerializeField] private ServerUIWindow _serverUIWindow;        // UI окно сервера
    [SerializeField] private ClientUIWindow _clientUIWindow;        // UI окно клиента

    public override void OnStartServer()
    {
        Debug.Log("Server started!");
        base.OnStartServer();
        _mirrorMessagesWrapper.ServerInit();
        _serverUIWindow.Show();
    }

    public override void OnStopServer()
    {
        Debug.Log("Server stopped!");
        base.OnStopServer();
        _serverUIWindow.Hide();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        _mirrorMessagesWrapper.OnClientDisconnectFromServer(conn);
    }

    public override void OnStartClient()
    {
        Debug.Log("Client started!");
        base.OnStartClient();
        _clientUIWindow.Show();
    }

    public override void OnStopClient()
    {
        Debug.Log("Client stopped!");
        base.OnStopClient();
        _clientUIWindow.Hide();
    }
}

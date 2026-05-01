using Zenject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// UI окно сервера
public class ServerUIWindow : BaseUIWindow
{
    [Inject] private MirrorMessagesWrapper _mirrorMessagesWrapper;      // Прослойка для сообщений Mirror

    [SerializeField] private TMP_InputField _helloMessageInputField;    // Поле ввода текста сообщения HelloMessage для отправки клиентам
    [SerializeField] private Button _helloMessageSendButton;            // Кнопка отправки сообщения HelloMessage клиентам

    protected override void Init()
    {
        _helloMessageSendButton.onClick.AddListener(OnHelloMessageSendButtonClick);
    }

    protected override void Deinit()
    {
        _helloMessageSendButton.onClick.RemoveListener(OnHelloMessageSendButtonClick);
    }

    // При нажатии кнопки отправки сообщения HelloMessage клиентам
    private void OnHelloMessageSendButtonClick()
    {
        // Отправить сообщение HelloMessage с текстом из _helloMessageInputField всем подписанным клиентам
        HelloMessage msg = new HelloMessage { Text = _helloMessageInputField.text };
        _mirrorMessagesWrapper.SendToSubscribers(msg);  // Вместо NetworkServer.SendToAll(msg);
        Debug.Log($"Server sent HelloMessage to all subscribing clients: {msg.Text}");
    }
}

using Zenject;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UI окно клиента
public class ClientUIWindow : BaseUIWindow
{
    [Inject] private MirrorMessagesWrapper _mirrorMessagesWrapper;  // Прослойка для сообщений Mirror

    [SerializeField] private Button _helloMessageSubscribeButton;   // Кнопка подписки на сообщения HelloMessage от сервера
    [SerializeField] private Button _helloMessageUnsubscribeButton; // Кнопка отписки от сообщений HelloMessage сервера
    [SerializeField] private GameObject _messagesChatPanel;         // Панель с сообщениями HelloMessage от сервера
    [SerializeField] private TMP_Text _messagesChatText;            // Текст на панели с сообщениями HelloMessage от сервера

    protected override void Init()
    {
        _helloMessageSubscribeButton.onClick.AddListener(OnHelloMessageSubscribeButtonClick);
        _helloMessageUnsubscribeButton.onClick.AddListener(OnHelloMessageUnsubscribeButtonClick);
        _helloMessageSubscribeButton.gameObject.SetActive(true);
        _helloMessageUnsubscribeButton.gameObject.SetActive(false);
    }

    protected override void Deinit()
    {
        _helloMessageSubscribeButton.onClick.RemoveListener(OnHelloMessageSubscribeButtonClick);
        _helloMessageUnsubscribeButton.onClick.RemoveListener(OnHelloMessageUnsubscribeButtonClick);
    }

    public override void Show()
    {
        base.Show();
        _messagesChatPanel.SetActive(false);
    }

    // При нажатии кнопки подписки
    private void OnHelloMessageSubscribeButtonClick()
    {
        // Подписаться на сообщения HelloMessage от сервера
        _mirrorMessagesWrapper.Subscribe<HelloMessage>(OnHelloMessageReceived); // Вместо NetworkClient.RegisterHandler<HelloMessage>(OnHelloMessageReceived);

        // Переключаем кнопки и включаем панель с сообщениями HelloMessage от сервера
        _helloMessageSubscribeButton.gameObject.SetActive(false);
        _helloMessageUnsubscribeButton.gameObject.SetActive(true);
        _messagesChatText.text = string.Empty;
        _messagesChatPanel.SetActive(true);
    }

    // При нажатии кнопки отписки
    private void OnHelloMessageUnsubscribeButtonClick()
    {
        // Отписаться от сообщений HelloMessage сервера
        _mirrorMessagesWrapper.Unsubscribe<HelloMessage>(); // Вместо NetworkClient.UnregisterHandler<HelloMessage>();

        // Переключаем кнопки и выключаем панель с сообщениями HelloMessage от сервера
        _helloMessageSubscribeButton.gameObject.SetActive(true);
        _helloMessageUnsubscribeButton.gameObject.SetActive(false);
        _messagesChatPanel.SetActive(false);
    }

    // При получении HelloMessage от сервера
    private void OnHelloMessageReceived(HelloMessage msg)
    {
        // Выводим текст HelloMessage в лог и в панель с сообщениями
        string text = msg.Text;
        Debug.Log($"Client received HelloMessage from server: {text}");
        _messagesChatText.text += text + "\n";
    }
}

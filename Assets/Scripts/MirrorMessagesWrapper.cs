using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

// Прослойка для сообщений Mirror
public class MirrorMessagesWrapper
{
    private Dictionary<string, HashSet<NetworkConnectionToClient>> _subscribers = new(); // Словарь, в котором содержится информация о том, на какие сообщения какие клиенты пописаны

    // Инициализируем прослойку для сервера
    public void ServerInit()
    {
        // Регистрируем сообщения подписки и отписки клиента на сообщения
        NetworkServer.RegisterHandler<SubscribeMessage>(OnSubscribeMessageReceived);
        NetworkServer.RegisterHandler<UnsubscribeMessage>(OnUnsubscribeMessageReceived);
    }

    // При получении сервером сообщения подписки от клиента
    private void OnSubscribeMessageReceived(NetworkConnectionToClient conn, SubscribeMessage msg)
    {
        Debug.Log($"Server recieved SubscribeMessage \"{msg.MessageType}\" by {conn.connectionId}");

        // Добавляем клиента в список подписчиков на сообщения типа msg.MessageType
        if (_subscribers.ContainsKey(msg.MessageType) == false)
        {
            _subscribers[msg.MessageType] = new HashSet<NetworkConnectionToClient>();
        }
        _subscribers[msg.MessageType].Add(conn);

        // Отправляем сразу приветственное сообщение, если подписались на HelloMessage
        if (msg.MessageType == typeof(HelloMessage).Name)
        {
            conn.Send(new HelloMessage { Text = "Hello Client!" });
        }
    }

    // При получении сервером  сообщения отписки от клиента
    private void OnUnsubscribeMessageReceived(NetworkConnectionToClient conn, UnsubscribeMessage msg)
    {
        Debug.Log($"Server recieved UnsubscribeMessage \"{msg.MessageType}\" by {conn.connectionId}");

        // Удаляем клиента из списка подписчиков на сообщения типа msg.MessageType
        if (_subscribers.ContainsKey(msg.MessageType) == true)
        {
            _subscribers[msg.MessageType].Remove(conn);
        }
    }

    // При отсоединении клиента от сервера
    public void OnClientDisconnectFromServer(NetworkConnectionToClient conn)
    {
        // Отписываем клиента от всех сообщений
        foreach (HashSet<NetworkConnectionToClient> clients in _subscribers.Values)
        {
            clients.Remove(conn);
        }
    }

    // Отправить от сервера сообщение типа T всем клиентам, подписанным на данный тип сообщения
    public void SendToSubscribers<T>(T message) where T : struct, NetworkMessage
    {
        string typeName = typeof(T).Name;
        if (_subscribers.ContainsKey(typeName) == true)
        {
            HashSet<NetworkConnectionToClient> clients = _subscribers[typeName];
            foreach (var conn in clients)
            {
                conn.Send(message);
            }
        }
    }

    // Клиент хочет подписаться на сообщения типа T от сервера
    public void Subscribe<T>(Action<T> handler) where T : struct, NetworkMessage
    {
        string typeName = typeof(T).Name;
        NetworkClient.RegisterHandler<T>(handler);

        // Сообщаем серверу, что мы хотим это получать
        NetworkClient.Send(new SubscribeMessage { MessageType = typeName });
    }

    // Клиент хочет отписаться от сообщения типа T от сервера
    public void Unsubscribe<T>() where T : struct, NetworkMessage
    {
        string typeName = typeof(T).Name;

        NetworkClient.UnregisterHandler<T>();

        // Сообщаем серверу, что мы хотим это получать
        NetworkClient.Send(new UnsubscribeMessage { MessageType = typeName });
    }
}

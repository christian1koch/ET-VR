using UnityEngine;
using SocketIOClient;
using System;
using System.Collections.Generic;

public class MagicDebugSender : MonoBehaviour
{
    // localhost für PC, später IP für VR
    public string serverUrl = "http://localhost:3000"; 
    
    public SocketIOUnity socket;

    void Start()
    {
        var uri = new Uri(serverUrl);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.OnConnected += (sender, e) => {
            Debug.Log("✅ Unity: Verbunden mit Debug-Server!");
            SendToWeb("System", "Unity ist verbunden!");
        };

        socket.Connect();
        Application.logMessageReceived += HandleUnityLog;
    }

    void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        SendToWeb(type.ToString(), logString);
    }

    public void SendToWeb(string type, string message)
    {
        if (socket == null || !socket.Connected) return;

        var data = new { 
            type = type, 
            message = message 
        };

        socket.EmitAsync("unity-log", data);
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleUnityLog;
        if (socket != null) socket.Disconnect();
    }
    
    // HIER WAR VORHER UPDATE - DAS HABEN WIR GELÖSCHT, WEIL WIR ES NICHT MEHR BRAUCHEN
}
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class UDPListener : MonoBehaviour
{
    private Thread udpThread;
    private UdpClient udpClient;
    public int listenPort = 5005;
    private bool isRunning = true;
    
    public float stress_level_100b { get; private set; }
    public float stress_level_15b { get; private set; }
    public float hr { get; private set; }
    

    public static UDPListener instance; // Singleton instance

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        udpThread = new Thread(new ThreadStart(ReceiveData));
        udpThread.IsBackground = true;
        udpThread.Start();
    }

    void ReceiveData()
    {
        udpClient = new UdpClient(listenPort);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
        
        while (isRunning)
        {
            try
            {
                byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedText = Encoding.UTF8.GetString(receivedBytes).Trim();
                
                string[] values = receivedText.Split(',');
                if (values.Length == 3)
                {
                    stress_level_100b = float.Parse(values[0], CultureInfo.InvariantCulture);
                    stress_level_15b = float.Parse(values[1], CultureInfo.InvariantCulture);
                    hr = float.Parse(values[2], CultureInfo.InvariantCulture);
                    
                    // Debug.Log($"Stress Level 100B: {stress_level_100b:.2f}, Stress Level 15B: {stress_level_15b:.2f}, HR: {hr:.2f}");
                }
                else
                {
                    Debug.LogError("Données reçues incorrectes: " + receivedText);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Erreur UDP: " + e.Message);
            }
        }
    }
    
    void OnApplicationQuit()
    {
        isRunning = false;
        udpClient?.Close();
        udpThread?.Abort();
    }

    public float getstress_level_100b()
    {
        return stress_level_100b;
    }
    public float getstress_level_15b()
    {
        return stress_level_15b;
    }
    public float gethr()
    {
        return hr;
    }
}

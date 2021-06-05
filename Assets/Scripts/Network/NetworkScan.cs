using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Text;
public class NetworkScan : MonoBehaviour
{
    //source this
    static Thread shit = new Thread(fuck);
    // Start is called before the first frame update

    public static void LobbySearch(){
        shit.Start();
    }

    static void fuck()
    {
        string ipBase = "192.168.1";

        for (int i = 0; i < 255; i++)
        {
            string ip = ipBase + i.ToString();
            var p = new System.Net.NetworkInformation.Ping();
            p.PingCompleted += new PingCompletedEventHandler(pComplete);
            try
            {
                p.SendAsync(ip, 100, ip);
            }
            catch (Exception e)
            {

            }
        }
        Debug.Log("Found all devices. Checking if they are Primordial Lobbies...");
        
    }

    static void pComplete(object sender, PingCompletedEventArgs e)
    {
        string ip = (string)e.UserState;

        if (e.Reply != null && e.Reply.Status == IPStatus.Success)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                Debug.Log(ip + " " + hostEntry.HostName);
            }
            catch (Exception ee)
            {
                Debug.Log(ee);
            }
        }
    }
}

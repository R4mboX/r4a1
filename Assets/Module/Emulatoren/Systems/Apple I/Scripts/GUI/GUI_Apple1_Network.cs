using TMPro;
using UnityEngine;

public class GUI_Apple1_Network : MonoBehaviour
{
    [Header("Connections")]
    public GUI_Apple1 GUI;

    [Header("UI Elements")]
    public GameObject PNL_Host;
    public GameObject PNL_Connect;
    public GameObject PNL_Status;
    public GameObject PNL_Connection;
    public TMP_InputField IN_HPort;
    public TMP_InputField IN_CIP;
    public TMP_InputField IN_CPort;
    public GameObject BTN_Host;
    public GameObject BTN_Connect;
    public GameObject BTN_Stop;
    public TMP_Text TXT_Status1;
    public TMP_Text TXT_Status2;
    public TMP_Text TXT_Status3;

    [Header("Status")]
    public string Status = "None";

    public void Tick()
    {
        TXT_Status1.text = GUI.SYS.NET.TCP.Status1;
        TXT_Status2.text = GUI.SYS.NET.TCP.Status2;
        TXT_Status3.text = GUI.SYS.NET.TCP.Status3;

        if (Status != "None" && GUI.SYS.NET.TCP.Status1 == "No Connection")
            ResetStatus();
    }

    public void HandleButtons(string A_Button)
    {
        switch (A_Button) 
        {
            case "Start":
                Status = "Host";
                GUI.SYS.NET.TCP.Status1 = "Starting Host";
                GUI.SYS.NET.TCP.Port = int.Parse(IN_CPort.text);
                GUI.SYS.NET.TCP.StartHost();
                GUI.SYS.NET.ActiveConnection = true;
                GUI.SYS.NET.MEM_RemoteReady = 0x01;
                break;

            case "Connect":
                Status = "Client";
                GUI.SYS.NET.TCP.Status1 = "Connecting to Host";
                GUI.SYS.NET.TCP.Connect(IN_CIP.text, int.Parse(IN_CPort.text));
                GUI.SYS.NET.ActiveConnection = true;
                GUI.SYS.NET.MEM_RemoteReady = 0x01;
                break;

            case "Stop":
                GUI.SYS.NET.TCP.Disconnect();
                ResetStatus();
                break;
        }

        EnableElements();
    }

    private void ResetStatus()
    {
        Status = "None";
        GUI.SYS.NET.ActiveConnection = false;
        EnableElements();
    }

    private void EnableElements()
    {
        switch(Status)
        {
            case "None":
                PNL_Host.SetActive(true);
                PNL_Connect.SetActive(true);
                BTN_Host.SetActive(true);
                BTN_Connect.SetActive(true);
                BTN_Stop.SetActive(false);
                break;

            case "Host":
                PNL_Host.SetActive(true);
                PNL_Connect.SetActive(false);
                BTN_Host.SetActive(false);
                BTN_Connect.SetActive(false);
                BTN_Stop.SetActive(true);
                break;

            case "Client":
                PNL_Host.SetActive(false);
                PNL_Connect.SetActive(true);
                BTN_Host.SetActive(false);
                BTN_Connect.SetActive(false);
                BTN_Stop.SetActive(true);
                break;
        }
    }
}

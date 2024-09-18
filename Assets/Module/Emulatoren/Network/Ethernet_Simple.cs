using UnityEngine;

public class Ethernet_Simple : MonoBehaviour
{
    public byte MEM_Received = 0x00;
    public byte MEM_RemoteReady = 0x00;

    public ushort ADR_Received = 0xD000;
    public ushort ADR_Sent = 0xD001;
    public ushort ADR_RemoteReady = 0xD002;

    public bool ActiveConnection = false;

    [Header("Verknüpfungen")]
    public Bus_Generic Bus;
    public SimpleTCP TCP;

    [Header("Konfiguration")]
    public bool CFG_Debug = false;

    public void Tick()
    {
        if (ActiveConnection == false)
            return;

        CheckTCP();

        // Reading Received Packet
        if (Bus.Address == ADR_Received)
        {
            if (Bus.Read == true)
            {
                Bus.Data = MEM_Received;
                // Tell Remote machine you're ready to receive more
                byte[] T_Packet = new byte[2] { 0x02, 0x00 };
                TCP.SendData(T_Packet);
                Bus.Address = 0xDFFF; // Set Address to Nirvana

                if (CFG_Debug == true)
                    Debug.Log("[ETHERNET] Reading 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
            else
            {
                MEM_Received = Bus.Data;
            }
        }

        // Writing Packet to send
        if (Bus.Address == ADR_Sent)
        {
            if (Bus.Read == false)
            {
                byte[] T_Packet = new byte[2] { 0x01, Bus.Data };
                TCP.SendData(T_Packet);
                Bus.Address = 0xDFFF; // Set Address to Nirvana
                MEM_RemoteReady = 0x00;

                if (CFG_Debug == true)
                    Debug.Log("[ETHERNER] Writing 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
            else
            {
                Bus.Data = 0x00;
            }
        }

        // RemoteReady gets checked
        if (Bus.Address == ADR_RemoteReady)
        {
            if (Bus.Read)
            {
                Bus.Data = MEM_RemoteReady;
                if (CFG_Debug == true)
                    Debug.Log("[ETHERNET] Reading 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
        }
    }

    public void CheckTCP()
    {
        if (TCP.ReceivedData[0] != 0x00)
        {
            if (TCP.ReceivedData[0] == 0x01) // Data
                MEM_Received = TCP.ReceivedData[1];

            if (TCP.ReceivedData[0] == 0x02) // Remote Machine is ready to Receive
                MEM_RemoteReady = 0x01;

            TCP.ReceivedData = new byte[2] { 0x00, 0x00 };
        }
    }
}

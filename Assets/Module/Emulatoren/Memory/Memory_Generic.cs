using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Memory_Generic : MonoBehaviour
{
    [HideInInspector]
    public byte[] Memory;

    [Header("Verknüpfungen")]
    public Bus_Generic Bus;

    [Header("Konfiguration")]
    public int ADR_Start = 0x0000;
    public int ADR_End = 0x9FFF;
    public List<ByteVector> ADR_Ignored = new List<ByteVector>();
    public bool CFG_Debug = false;   

    public void Init()
    {
        Memory = new byte[ADR_End - ADR_Start + 1];
    }

    public void Tick()
    {
        foreach (ByteVector T_Igno in ADR_Ignored)
            if (Bus.Address >= T_Igno.b1 || Bus.Address <= T_Igno.b2)
                return;

        if (Bus.Address >= ADR_Start && Bus.Address <= ADR_End)
        {

            int offset = Bus.Address - ADR_Start;
            if (Bus.Read)
            {
                Bus.Data = Memory[offset];
                if (CFG_Debug == true)
                    Debug.Log("[RAM] Reading 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
            else
            {
                Memory[offset] = Bus.Data;
                if (CFG_Debug == true)
                    Debug.Log("[RAM] Writing 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
        }
    }

    public void Dump(string A_Path, ushort A_Start, ushort A_End)
    {
        StringBuilder T_HexDump = new StringBuilder();
        bool T_LastEmpty = true;
        bool T_NewLine = false;

        int T_Count = 1; // 32 = New Line

        for (int i = A_Start; i <= A_End; i++)
        {
            if (Memory[i] == 0x00)
            {
                T_LastEmpty = true;
                continue;
            }

            if (T_LastEmpty == true)
            {
                T_HexDump.AppendLine();
                T_HexDump.Append(i.ToString("X4") + ": ");
                T_LastEmpty = false;
                T_NewLine = false;
            }

            if (T_NewLine == true)
            {
                T_HexDump.AppendLine();
                T_HexDump.Append(": ");
                T_NewLine = false;
            }               
          
            T_HexDump.Append(Memory[i].ToString("X2") + " ");
      
            if (T_Count >= 31)
            {
                T_Count = 0;
                T_NewLine = true;
            }

            T_Count++;
        }

        File.WriteAllText(A_Path, T_HexDump.ToString());
    }
}
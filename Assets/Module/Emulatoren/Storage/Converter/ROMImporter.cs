using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

public class ROMImporter : MonoBehaviour
{
    public ushort Entrypoint = 0x0000;
    public byte[] Memory = new byte[0x9FFF];
    public List<string> AutoText = new List<string>();

    public void Import(string A_FilePath)
    {
        string[] T_Lines = File.ReadAllLines(A_FilePath);
        Entrypoint = 0xFFFF;
        Memory = new byte[0x9FFF];
        ushort CurrentAddress = 0x0000;
        AutoText = new List<string>();

        foreach (string T_Line in T_Lines)
        {
            if (string.IsNullOrWhiteSpace(T_Line))
                continue;
                
            if (T_Line.Contains(":"))
            {
                string[] T_Parts = T_Line.Split(':');
                if (T_Parts.Length == 2)
                {
                    string addressPart = T_Parts[0].Trim(); // Left Side
                    if (!string.IsNullOrWhiteSpace(addressPart))
                    {
                        if (ushort.TryParse(addressPart, System.Globalization.NumberStyles.HexNumber, null, out ushort address))
                            CurrentAddress = address;
                        else
                            Debug.LogWarning("[Importer] Invalid Address: " + addressPart);
                    }

                    string hexPart = T_Parts[1].Trim(); // Right Side
                    if (!string.IsNullOrWhiteSpace(hexPart))
                    {
                        WriteHexToMemory(hexPart, ref CurrentAddress);
                    }
                }
            }
            else
            {
                if (T_Line.StartsWith("!"))
                {
                    AutoText.Add(T_Line.Substring(1));
                    continue;
                }
                    
                if (T_Line.EndsWith("R")) // EntryPoint
                {
                    string T_EntryPointString = T_Line.TrimEnd('R');
                    if (ushort.TryParse(T_EntryPointString, System.Globalization.NumberStyles.HexNumber, null, out ushort entryPoint))
                        Entrypoint = entryPoint;
                    else
                        Debug.LogWarning("[Importer] Invalid Entrypoint: " + T_EntryPointString);
                }
                else // Address
                {
                    if (ushort.TryParse(T_Line, System.Globalization.NumberStyles.HexNumber, null, out ushort address))
                        CurrentAddress = address;
                    else
                        Debug.LogWarning("[Importer] Invalid Address: " + T_Line);
                }
            }
        }
    }

    private void WriteHexToMemory(string hexData, ref ushort address)
    {
        string[] hexBytes = hexData.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string hexByte in hexBytes)
        {
            if (byte.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out byte value))
            {
                if (address < Memory.Length)
                {
                    Memory[address] = value;
                    address++;
                }
                else
                {
                    Debug.LogWarning("[Importer] Address out of bounds: " + address);
                    break;
                }
            }
            else
            {
                Debug.LogWarning($"[Importer] Invalid Hex Byte: {hexByte}");
            }
        }
    }
}

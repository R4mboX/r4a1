using UnityEngine;

[System.Serializable]
public class Cartridge_Generic
{
    [Header("Data")]
    public string C_Title = "Untitled";
    public byte[] C_Data = new byte[4096];
    public ushort C_EntryPoint = 0x0000;
    public bool WriteProtected = false;
    public bool AutoSave = true;
}

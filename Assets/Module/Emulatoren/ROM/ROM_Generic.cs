using UnityEngine;

public class ROM_Generic : MonoBehaviour
{
    private byte[] Memory;

    [Header("Verknüpfungen")]
    public Bus_Generic Bus;
    public TextAsset ROM;

    [Header("Konfiguration")]
    public int ADR_Start = 0xE000;
    public int ADR_End = 0xEFFF;
    public bool CFG_WriteProtection = true;
    public bool CFG_DebugAll = false;

    public void Init()
    {
        Memory = new byte[ADR_End - ADR_Start + 1];

        byte[] T_ROM = ROM.bytes;
        for (int i = 0; i < T_ROM.Length; i++)
            Memory[i] = T_ROM[i];
    }

    public void Tick()
    {
        if (Bus.Address >= ADR_Start && Bus.Address <= ADR_End)
        {
            int offset = Bus.Address - ADR_Start;
            if (Bus.Read)
            {
                Bus.Data = Memory[offset];
                if (CFG_DebugAll == true)
                    Debug.Log("[ROM] Reading 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
            else
            {
                if (CFG_WriteProtection == false)
                {
                    Memory[offset] = Bus.Data;
                    if (CFG_DebugAll == true)
                        Debug.Log("[ROM] Writing 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
                }
                else
                {
                    Debug.Log("[ROM] Writing NOT ALLOWED 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
                }
            }     
        }
    }
}

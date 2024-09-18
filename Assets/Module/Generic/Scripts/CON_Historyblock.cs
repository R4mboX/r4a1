using Unity.VisualScripting;

[System.Serializable]
public class CON_Historyblock
{
    public ushort PC;
    public byte OPC;

    public CON_Historyblock()
    {
        PC = 0x0000;
        OPC = 0x00;
    }
}

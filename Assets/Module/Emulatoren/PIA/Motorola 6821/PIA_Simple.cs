using UnityEngine;

public class PIA_Simple : MonoBehaviour
{
    public Bus_Generic Bus;
    public Display_SimpleText Display;
    public Keyboard_ASCII Keyboard; // Used by GUI, etc, To mute Keyboard
    public bool Waiting = false;

    // Simplified Version

    public byte KBD = 0x00;     // 0xD010
    public byte KBDCR = 0x00;   // 0xD011
    public byte DSP = 0x00;     // 0xD012
    public byte DSPCR = 0x00;   // 0xD013

    public void Tick()
    {
        // Is Display ready?
        if (Display.PleaseWait == false && Waiting == true)
        {
            DSP = SetFlag(Bus.Data, 7, false);
            Waiting = false;
        }
            
        switch(Bus.Address)
        {
            case 0xD010: // KBD
                if (Bus.Read == true)
                {
                    Bus.Data = KBD;
                    KBDCR = SetFlag(KBDCR, 7, false);
                }
                else
                {
                    if (ReadFlag(KBDCR, 2) == true)
                        KBD = Bus.Data;
                    else
                        Debug.Log("PIA: Write Command on KBD in Config-Mode: " + Bus.Data.ToString("X2"));
                }
                break;

            case 0xD011: // KBDCR
                if (Bus.Read == true)
                {
                    Bus.Data = KBDCR;
                }
                else
                {
                    KBDCR = Bus.Data;
                    //KBDCR = SetFlag(Bus.Data, 7, false);
                }
                break;

            case 0xD012: // DSP
                if (Bus.Read == true)
                {
                    Bus.Data = DSP;
                }
                else
                {
                    if (ReadFlag(DSPCR, 2) == true)
                    {
                        DSP = SetFlag(Bus.Data, 7, true); // CPU will wait
                        Waiting = true;
                        Display.PrepareCharacter(SetFlag(Bus.Data, 7, false));
                    }
                    else
                    {
                        //Debug.Log("PIA: Write Command on DSP in Config-Mode: " + Bus.Data.ToString("X2"));
                    }
                }
                break;

            case 0xD013:
                if (Bus.Read == true)
                {
                    Bus.Data = DSPCR;
                }
                else
                {
                    DSPCR = SetFlag(Bus.Data, 7, false);
                }
                break;
        }
    }

    public bool ReadFlag(byte A_Byte, int A_Bit)
    {
        return (A_Byte & (1 << A_Bit)) != 0;
    }

    public byte SetFlag(byte A_Byte, int A_Bit, bool A_State)
    {
        if (A_State)
            A_Byte |= (byte)(1 << A_Bit);
        else
            A_Byte &= (byte)~(1 << A_Bit);

        return A_Byte;
    }
}

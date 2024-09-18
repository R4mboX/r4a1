using UnityEngine;

public class PIA_6821 : MonoBehaviour
{
    // This was a rough implementation of the PIA and was later replaced by a way more simple version
    // It might not work correctly and needs Cleanup.

    // Ablauf durch WOZ-Mon:
    // 1. WRITE 0xD012, 0x7F    0111 1111   DSP-Filter wird gesetzt.
    // 2. WRITE 0xD011, 0xA7    1010 0111
    // 3. WRITE 0xD013, 0xA7    1010 0111
    // 4. READ  0xD012, (0x7F)
    // 5. READ  0xD012, (0x7F)
    // 6. WRITE 0xD012, 0xDC    1101 1100
    // 7. READ  0xD012, (0xDC)
    // 8. READ  0xD012, (0xDC)
    // 7 und 8 wiederholen sich endlos.

    // TODO
    // Scheiß Filter werden noch nicht berücksichtigt
    // Display-Part nicht fertig
    // Nicht alle Tick-Szenarien implementiert
    // Display-Script muss BUS prüfen, Daten entnehmen falls Bit7 aktiv ist und auf inaktiv setzen wenn sie entnommen wurden, und dann am Schluss noch RDA auf High setzen, weil es dann wieder Ready ist.
    // Scheiß drauf, das ist ein Schlachtfeld. Chars werden jetzt manuell zum Display durchgeschleust und B7 auf False gesetzt.

    [Header("Verknüpfungen")]
    public Bus_Generic Bus;
    public Display_SimpleText Display;

    [Header("Register")]
    public byte Reg_KBD = 0x00;     // 0xD010   KBD     Keyboard Data
    public byte Reg_KBDCR = 0x00;   // 0xD011   KBDCR   Keyboard Controller Register    [CRA][IRQA1, IRQA2, CA2Control (3 Bit), DDR Access, CA1Control (2 Bit)]
    public byte Reg_DSP = 0x00;     // 0xD012   DSP     Display Data                    Bit7 ist Data Available. CPU sendet erst Daten wenn das False ist.
    public byte Reg_DSPCR = 0x00;   // 0xD013   DSPCR   Display Control Register        [CRB][IRQB1, IRQB2, CB2Control (3 Bit), DDR Access, CB1Control (2 Bit)]

    public byte Bus_KBD = 0x00;     // Verbindung PIA <-> Keyboard
    public byte Bus_DSP = 0x00;     // Verbindung PIA <-> Display
    public bool RDA = false;        // CB1 Verbindung PIA <-> Display, Display aktiviert RDA wenn es bereit ist Daten zu empfangen
    public bool KBDStrobe = false;  // CA1 Verbindung PIA <-> Keyboard, Wird von Keyboard aktiviert wenn von Bus_KBD Daten ins Reg_KBD geladen werden sollen.
    private bool RDA_OLD = false;   // Memory um Low-High Transitions festzustellen und Interrupt auszulösen.
    private bool KBDStrobe_OLD = false; // Memory um Low-High Transitions festzustellen und Interrupt auszulösen.
    public bool CB2 = false;        // Verbindung PIA --> Display (OUTPUT only)


    [Header("Konfiguration")]
    public int ADR_Start = 0xD010;
    public int ADR_End = 0xD013;
    public byte KBDFilter = 0x00;
    public byte DSPFilter = 0x00;

    public void Update()
    {
        if (ReadFlag(Reg_KBDCR, 0) == true)
            HandleKBDInterrupts();

        //if (ReadFlag(Reg_DSPCR, 0) == true)
            //HandleDSPInterrupts(); // Deaktiviert weils Scheiße ist.
    }

    private void HandleKBDInterrupts()
    {
        if (ReadFlag(Reg_KBDCR, 1) == true && KBDStrobe == true && KBDStrobe_OLD == false)
        {
            // Interrupt
            Reg_KBD = Bus_KBD;
            // Irgendeine Scheiß Flag setzen!
            Reg_KBDCR = SetFlag(Reg_KBDCR, 7, true);
            Reg_KBDCR = SetFlag(Reg_KBDCR, 6, true);
            // Reset-Signal abfangen und an CPU weiterleiten...
        }
        if (ReadFlag(Reg_KBDCR, 0) == true && KBDStrobe == false && KBDStrobe_OLD == true)
        {
            // Interrupt
            Reg_KBD = Bus_KBD;
            // Irgendeine Scheiß Flag setzen!
            Reg_KBDCR = SetFlag(Reg_KBDCR, 7, true);
            Reg_KBDCR = SetFlag(Reg_KBDCR, 6, true);
            // Reset-Signal abfangen und an CPU weiterleiten...
        }
        KBDStrobe_OLD = KBDStrobe;
    }

    private void HandleDSPInterrupts()
    {
        if (ReadFlag(Reg_DSPCR, 1) == true && RDA == true && RDA_OLD == false)
        {
            // B3-Flag muss nun auch High sein
            Reg_DSPCR = SetFlag(Reg_DSPCR, 3, true); // Die Flag wird später deaktiviert wenn irgendwas ins Bus_DSP geladen wird.
            Bus_DSP = Reg_DSP; // Daten in BUS ablegen.
            Reg_DSP = SetFlag(Reg_DSP, 7, false); // RDA auf Low damit die CPU nachschaufeln kann
        }
        if (ReadFlag(Reg_DSPCR, 0) == true && RDA == false && RDA_OLD == true)
        {
            // B3-Flag muss nun auch High sein
            Reg_DSPCR = SetFlag(Reg_DSPCR, 3, true); // Die Flag wird später deaktiviert wenn irgendwas ins Bus_DSP geladen wird.
            Bus_DSP = Reg_DSP; // Daten in BUS ablegen.
            Reg_DSP = SetFlag(Reg_DSP, 7, false); // RDA auf Low damit die CPU nachschaufeln kann
        }
        RDA_OLD = RDA;
    }

    public void Tick()
    {
        if (Bus.Address >= ADR_Start && Bus.Address <= ADR_End)
        {
            switch(Bus.Address)
            {
                case 0xD010:
                    if(ReadFlag(Reg_KBDCR, 2) == true)
                    {
                        // KBD_Bus
                    }
                    else
                    {
                        // Normaler Bus
                        if (Bus.Read == true)
                        {
                            // ? Daten senden von KBD an BUS ?
                        }
                        else
                        {
                            // KBD-Filter setzen
                            KBDFilter = Bus.Data;
                        }
                    }
                    break;

                case 0xD011: // KBDCR
                    if (Bus.Read == false)
                    {
                        Reg_KBDCR = Bus.Data;
                    }
                    break;

                case 0xD012: // DSP
                    if (ReadFlag(Reg_DSPCR, 2) == true)
                    {
                        // DSP_Bus
                        if (Bus.Read == true)
                        {
                            // Will wohl das Bit7 (DA) prüfen
                            Bus.Data = Reg_DSP;
                            Debug.Log("PIA legt DSP-Register auf Bus ab.");
                            if (ReadFlag(Reg_DSP, 7) == true)
                                Debug.Log("... und Data Available ist true");
                            else
                                Debug.Log("... und Data Available ist false, also freie fahrt!");
                        }
                        else
                        {
                            // CPU schreibt Daten ins DSP Register und setzt Bit7 (DA) auf High.
                            Reg_DSP = (byte)((Bus.Data & 0x7F) | 0x80);
                            // Durchschleusen wegen zu viel Probleme
                            Display.PrepareCharacter(SetFlag(Reg_DSP, 7, false));
                            Reg_DSP = SetFlag(Reg_DSP, 7, false);
                        }
                    }
                    else
                    {
                        // Normaler Bus
                        if (Bus.Read == true)
                        {
                            // ? Daten senden von DSP an BUS ?
                        }
                        else
                        {
                            // DSP-Filter setzen
                            DSPFilter = Bus.Data;
                        }
                    }
                    break;

                case 0xD013: // DSPCR
                    if (Bus.Read == false)
                    {
                        Reg_DSPCR = Bus.Data;
                    }
                    break;
            }


            if (Bus.Read == true)
            {
                Debug.Log("PIA READ 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
            if (Bus.Read == false)
            {
                Debug.Log("PIA WRITE 0x" + Bus.Address.ToString("X2") + ", 0x" + Bus.Data.ToString("X2"));
            }
        }
    }

    public bool ReadFlag(byte A_Byte,int A_Bit)
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

    /*
    void Update()
    {
        GetCache();

        if (GetBit(Mem_KBDCR, 2) == true)
            ProcessKeyboard();
        else
            ConfigurationMode();

        if (GetBit(Mem_DSPCR, 2) == true)
            ProcessVideo();
        else
            ConfigurationMode();

        //UpdateFlags();
    }

    public void GetCache()
    {
        //Mem_KBD = Memory.ReadByte(0xD010);
        Mem_KBD = Memory.memory[0xD010];
        Mem_KBDCR = Memory.ReadByte(0xD011);
        Mem_DSP = Memory.ReadByte(0xD012);
        Mem_DSPCR = Memory.ReadByte(0xD013);
    }

    public void ConfigurationMode()
    {
        if (Mem_DSP == 0x7F)
        {
            Debug.Log("PIA hat 0x7F auf DSP erhalten."); // Wird nicht erfasst wegen Timing...
            Memory.WriteByte(0xD012, 0x00);
        }
    }

    public void UpdateFlags()
    {
        // Keyboard
        Mem_KBDCR = SetBit(Mem_KBDCR, 1, false); // Kein Strobe Signal
        // Mem_KBDCR = SetBit(Mem_KBDCR, 7, false); // Kein NewKeyFlag
        if (Memory.ReadByte(0xD011) != Mem_KBDCR)
            Memory.WriteByte(0xD011, Mem_KBDCR);

        // Display
        Mem_DSPCR = SetBit(Mem_DSPCR, 1, true); // Ready Data Accept
        if (Memory.ReadByte(0xD013) != Mem_DSPCR)
            Memory.WriteByte(0xD013, Mem_DSPCR);
    }

    public void ProcessKeyboard()
    {
        if (Keyboard.KeyPressed == true)
        {
            Mem_KBD = Keyboard.KeyValue;
            Mem_KBDCR = SetBit(Mem_KBDCR, 7, true);
            Memory.WriteByte(0xD010, Mem_KBD);
            Keyboard.KeyPressed = false;
        }
        else
        {
            Mem_KBDCR = SetBit(Mem_KBDCR, 7, false);
        }
    }

    public void ProcessVideo()
    {
        byte T_DPChar = (byte)(Mem_DSP & 0x7F);

        if (GetBit(Mem_DSP, 7) == true) // Ist Data Available?
        {
            Display.RenderCharacter(T_DPChar);
            Memory.WriteByte(0xD012, (byte)(Mem_DSP & 0x7F)); // Data Nicht mehr Available.
        }
    }

    public void ModifyKeyboardRegister(int A_Bit, bool A_Status)
    {
        byte T_Register = Memory.ReadByte(0xD011);

        if (A_Status == true)
            Memory.WriteByte(0xD011, T_Register |= (byte)(1 << A_Bit));
        else
            Memory.WriteByte(0xD011, T_Register &= (byte)~(1 << A_Bit));
    }

    public void ModifyDisplayRegister(int A_Bit, bool A_Status)
    {
        byte T_Register = Memory.ReadByte(0xD013);

        if (A_Status == true)
            Memory.WriteByte(0xD013, T_Register |= (byte)(1 << A_Bit));
        else
            Memory.WriteByte(0xD013, T_Register &= (byte)~(1 << A_Bit));
    }

    public bool GetBit(byte A_Byte, int A_Bit)
    {
        return (A_Byte & (1 << A_Bit)) != 0;
    }

    public byte SetBit(byte A_Byte, int A_Bit, bool A_Set)
    {
        if (A_Set)
            return (byte)(A_Byte | (1 << A_Bit));
        else
            return (byte)(A_Byte & ~(1 << A_Bit));
    }
    */

}

using System.Collections.Generic;
using UnityEngine;

public class CPU_MOS6502 : MonoBehaviour
{
    [Header("Verknüpfungen")]
    public Bus_Generic Bus; // [ushort]Address, [byte]Data, [bool]Read

    [Header("Register")]
    public ushort PC;
    public byte A;
    public byte X;
    public byte Y;
    public byte Status; // Flags: [7]Negative, [6]Overflow, [5]Ignored, [4]Break, [3]Decimal Mode, [2]Interrupt Disable, [1]Zero, [0]Carry
    public byte SP;

    [Header("Status")]
    public int CPUStep = -3; // Reset = -3
    public byte OPC = 0x00;
    public int CycleCollector = 0x00;
    public List<CON_Historyblock> History = new List<CON_Historyblock>();

    [Header("Bugfixes")]
    public bool FIX_JmpInd = false;

    [Header("Hilfviariablen")]
    byte H_Byte = 0x00;
    byte H_LByte = 0x00;
    byte H_HByte = 0x00;
    sbyte H_SByte = 0x00;
    ushort H_UShort = 0x0000;

    public void Tick()
    {
        switch (CPUStep)
        {
            case < -3: // Errorhandling
                CPUStep = -3;
                break;

            case > 100: // Errorhandling
                CPUStep = -3;
                break;

            case -3: // Reset Step 1
                A = 0x00;
                X = 0x00;
                Y = 0x00;
                Status = 0x00;
                SP = 0xFF;
                Status = 0x00;

                SetFlag(2, true);
                Bus.Address = 0xFFFC;
                Bus.Read = true;
                CPUStep++;
                break;

            case -2: // Reset Step 2
                A = Bus.Data;
                Bus.Address = 0xFFFD;
                CPUStep++;
                break;

            case -1: // Reset Step 3
                PC = (ushort)(Bus.Data << 8 | A);
                CPUStep++;
                break;

            case 0: // Ask for next OPCode
                Bus.Address = PC;
                Bus.Read = true;
                CPUStep++;
                break;

            case 1: // Set OPC and Operate()
                OPC = Bus.Data;

                CON_Historyblock HBlock = new CON_Historyblock();
                HBlock.PC = PC;
                HBlock.OPC = OPC;
                History.Add(HBlock);

                if (History.Count > 1000)
                    History.RemoveAt(0);

                PC++;

                CPUStep++;
                Operate();
                break;

            case >= 2:
                Operate();
                break;  
        }
    }

    private void Operate()
    {
        switch (OPC)
        {
            case 0x00: // [BRK impl]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = (ushort)(0x0100 + SP--);
                        Bus.Data = (byte)((PC + 1) >> 8);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (ushort)(0x0100 + SP--);
                        Bus.Data = (byte)(PC + 1);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Address = (ushort)(0x0100 + SP--);
                        Bus.Data = (byte)(Status | 0x10);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Address = 0xFFFE;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        H_LByte = Bus.Data;
                        Bus.Address = 0xFFFF;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 7:
                        H_HByte = Bus.Data;
                        PC = (ushort)(H_LByte | (H_HByte << 8));
                        SetFlag(2, true);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x01: // [ORA X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x05: // [ORA zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x06: // [ASL zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Data = ASL(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x08: // [PHP implied]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = (ushort)(0x0100 + SP--);
                        Bus.Data = (byte)(Status | 0x30);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 3:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x09: // [ORA #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x0A: // [ASL A]
                switch (CPUStep)
                {
                    case 2:
                        A = ASL(A);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x0D: // [ORA abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x0E: // [ASL abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Data = ASL(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x10: // [BPL rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        if (!ReadFlag(7))
                        {
                            H_SByte = (sbyte)Bus.Data;
                            ushort H_UShort = PC;
                            PC = (ushort)(PC + H_SByte);
                            if ((H_UShort & 0xFF00) != (PC & 0xFF00))
                                CycleCollector -= 2;
                            else
                                CycleCollector -= 1;
                        }
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x11: // [ORA ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        H_UShort = Bus.Address;
                        Bus.Address += Y;
                        Bus.Read = true;
                        if ((H_UShort & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector -= 1;
                        CPUStep++;
                        break;

                    case 6:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x15: // [ORA zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x16: // [ASL zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Data = ASL(Bus.Data);
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x18: // [CLC impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(0, false);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x19: // [ORA abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        H_UShort = (ushort)((H_HByte << 8) | H_LByte);
                        Bus.Address = (ushort)(H_UShort + Y);
                        Bus.Read = true;
                        if ((H_UShort & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        CPUStep++;
                        break;

                    case 5:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x1D: // [ORA abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)((H_HByte << 8) | H_LByte);
                        H_UShort = Bus.Address;
                        Bus.Address += X;
                        Bus.Read = true;
                        if ((H_UShort & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        CPUStep++;
                        break;

                    case 5:
                        ORA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x1E: // [ASL abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Data = ASL(Bus.Data);
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x20: // [JSR abs]
                switch (CPUStep)
                {
                    case 2: // Request Low Byte
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3: // Read Low Byte and Push Stack #1
                        H_LByte = Bus.Data;
                        Bus.Address = (ushort)(0x0100 | SP--);
                        Bus.Data = (byte)((PC >> 8) & 0xFF);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 4: // Push Stack #2
                        Bus.Address = (ushort)(0x0100 | SP--);
                        Bus.Data = (byte)(PC & 0xFF);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5: // Request High Byte
                        Bus.Address = PC;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6: // Read MSB and set PC
                        H_HByte = Bus.Data;
                        PC = (ushort)(H_LByte | (H_HByte << 8));
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x21: // [AND X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        AND(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x24: // [BIT zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        BIT(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x25: // [AND zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        AND(Bus.Data);
                        CycleCollector++;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x26: // [ROL zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Data = ROL(H_Byte);
                        Bus.Read = false;
                        CycleCollector--;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x28: // [PLP impl]
                switch (CPUStep)
                {
                    case 2:
                        SP++;
                        Bus.Address = (ushort)(0x0100 + SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Status = Bus.Data;
                        SetFlag(4, false);
                        SetFlag(5, true);
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x29: // [AND #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        AND(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x2A: // [ROL A]
                switch (CPUStep)
                {
                    case 2:
                        A = ROL(A);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x2C: // [BIT abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        BIT(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x2D: // [AND abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        AND(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x2E: // [ROL abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Data = ROL(H_Byte);
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x30: // [BMI rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_SByte = (sbyte)Bus.Data;
                        if (ReadFlag(7))
                        {
                            CycleCollector--;

                            ushort newPC = (ushort)(PC + H_SByte);
                            if ((PC & 0xFF00) != (newPC & 0xFF00))
                            {
                                CycleCollector--;
                            }
                            PC = newPC;
                        }
                        CycleCollector++;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x31: // [AND ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += Y;
                        if ((H_LByte + Y) > 0xFF)
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        AND(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x35: // [AND zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        AND(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x36: // [ROL zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Data = ROL(H_Byte);
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x38: // [SEC impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(0, true);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x39: // [AND abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += Y;
                        if ((H_LByte + Y) > 0xFF)
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        AND(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x3D: // [AND abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        if ((H_LByte + X) > 0xFF)
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        AND(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x3E: // [ROL abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Data = ROL(H_Byte);
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x40: // [RTI impl]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = (ushort)(0x0100 + ++SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_Byte = Bus.Data;
                        Status = (byte)((Status & 0x30) | (H_Byte & 0xCF));
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Address = (ushort)(0x0100 + ++SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_LByte = Bus.Data;
                        Bus.Address = (ushort)(0x0100 + ++SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        H_HByte = Bus.Data;
                        PC = (ushort)(H_LByte | (H_HByte << 8));
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x41: // [EOR X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x45: // [EOR zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x46: // [LSR zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        LSR(ref H_Byte);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x48: // [PHA impl]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = (ushort)(0x0100 + SP);
                        Bus.Data = A;
                        Bus.Read = false;
                        SP--;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x49: // [EOR #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x4A: // [LSR A]
                switch (CPUStep)
                {
                    case 2:
                        LSR(ref A);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x4C: // [JMP abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        PC = (ushort)(H_LByte | (H_HByte << 8));
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x4D: // [EOR abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x4E: // [LSR abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        LSR(ref H_Byte);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x50: // [BVC rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        if (!ReadFlag(6))
                        {
                            sbyte offset = (sbyte)Bus.Data;
                            ushort oldPC = PC;
                            PC = (ushort)(PC + offset);

                            if ((oldPC & 0xFF00) != (PC & 0xFF00))
                                CycleCollector -= 2;
                            else
                                CycleCollector--;
                        }
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x51: // [EOR ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x55: // [EOR zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x56:
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        LSR(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x58: // [CLI impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(2, false);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x59: // [EOR abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x5D: // [EOR abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        EOR(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x5E: // [LSR abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        LSR(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector -= 2;
                        break;
                }
                break;

            case 0x60: // [RTS impl]
                switch (CPUStep)
                {
                    case 2:
                        SP++;
                        Bus.Address = (ushort)(0x0100 | SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        SP++;
                        Bus.Address = (ushort)(0x0100 | SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        CPUStep++;
                        break;

                    case 5:
                        PC = (ushort)((H_HByte << 8) | H_LByte);
                        PC++;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x61: // [ADC X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x65: // [ADC zpg]
                switch(CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x66: // [ROR zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        ROR(ref H_Byte);
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x68: // [PLA impl]
                switch (CPUStep)
                {
                    case 2:
                        SP++;
                        Bus.Address = (ushort)(0x0100 | SP);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        A = Bus.Data;
                        SetFlag(1, A == 0);
                        SetFlag(7, (A & 0x80) != 0);
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x69: // [ADC #]
                switch(CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_Byte = Bus.Data;
                        ADC(H_Byte);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x6A: // [ROR A]
                switch (CPUStep)
                {
                    case 2:
                        ROR(ref A);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x6C: // [JMP ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_LByte = Bus.Data;
                        if (FIX_JmpInd == true)
                        {
                            Bus.Address++;
                        }
                        else
                        {
                            if ((Bus.Address & 0x00FF) == 0x00FF)
                                Bus.Address -= 0xFF;
                            else
                                Bus.Address++;
                        }
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        H_HByte = Bus.Data;
                        PC = (ushort)(H_LByte | (H_HByte << 8));
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x6D: // [ADC abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x6E: // [ROR abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        ROR(ref H_Byte);
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x70: // [BVS rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        if (ReadFlag(6))
                        {
                            sbyte offset = (sbyte)Bus.Data;
                            ushort oldPC = PC;
                            PC = (ushort)(PC + offset);

                            CycleCollector--;

                            if ((oldPC & 0xFF00) != (PC & 0xFF00))
                                CycleCollector--;
                            
                        }
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x71: // [ADC ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x75: // [ADC zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x76: // [ROR zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        ROR(ref H_Byte);
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x78: // [SEI Impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(2, true);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x79: // [ADC abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x7D: // [ADC abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        ADC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0x7E: // [ROR abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        ROR(ref H_Byte);
                        Bus.Data = H_Byte;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x81:
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x84: // [STY zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = false;
                        Bus.Data = Y;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x85: // [STA zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x86: // [STX zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Data = X;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x88: // [DEY impl]
                switch (CPUStep)
                {
                    case 2:
                        Y--;
                        SetFlag(1, Y == 0);
                        SetFlag(7, (Y & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x8A: // [TXA impl]
                switch (CPUStep)
                {
                    case 2:
                        A = X;
                        SetFlag(1, A == 0);
                        SetFlag(7, (A & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x8C: // [STY abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = false;
                        Bus.Data = Y;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x8D: // [STA abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x8E: // [STX abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Data = X;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x90: // [BCC rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        byte offset = Bus.Data;
                        if (!ReadFlag(0))
                        {
                            ushort oldPC = PC;

                            if ((offset & 0x80) != 0)
                                PC = (ushort)(PC - ((~offset + 1) & 0xFF));
                            else
                                PC = (ushort)(PC + offset);

                            CycleCollector--;

                            if ((oldPC & 0xFF00) != (PC & 0xFF00))
                                CycleCollector--;
                        }
                        CycleCollector++;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x91: // [STA ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += Y;
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x94: // [STY zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = false;
                        Bus.Data = Y;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x95: // [STA zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x96: // [STX zpg, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + Y);
                        Bus.Data = X;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x98: // [TYA impl]
                switch (CPUStep)
                {
                    case 2:
                        A = Y;
                        SetFlag(1, A == 0);
                        SetFlag(7, (A & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;
               
            case 0x99: // [STA abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += Y;
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0x9A: // [TXS impl]
                switch (CPUStep)
                {
                    case 2:
                        SP = X;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0x9D: // [STA abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Data = A;
                        Bus.Read = false;
                        CPUStep = 0;
                        CycleCollector--;
                        break;
                }
                break;

            case 0xA0: // [LDY #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        LDY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xA1: // [LDA X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        LDA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xA2: // [LDX #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        LDX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xA4: // [LDY zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xA5: // [LDA zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xA6: // [LDX zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xA8: // [TAY impl]
                switch (CPUStep)
                {
                    case 2:
                        Y = A;
                        SetFlag(1, Y == 0);
                        SetFlag(7, (Y & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xA9: // [LDA #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        LDA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xAA: // [TAX impl]
                switch (CPUStep)
                {
                    case 2:
                        X = A;
                        SetFlag(1, X == 0);
                        SetFlag(7, (X & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xAC: // [LDY abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        LDY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xAD: // [LDA abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        LDA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xAE: // [LDX abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        LDX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xB0: // [BCS rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        if (ReadFlag(0))
                        {
                            H_SByte = (sbyte)Bus.Data;
                            ushort oldPC = PC;
                            ushort newPC = (ushort)(PC + H_SByte);

                            CycleCollector--;

                            if ((oldPC & 0xFF00) != (newPC & 0xFF00))
                                CycleCollector--;

                            PC = newPC;
                        }
                        CycleCollector++;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xB1: // [LDA ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        LDA(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xB4: // [LDY zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDY(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xB5: // [LDA zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xB6: // [LDX zpg, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + Y);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        LDX(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xB8: // [CLV impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(6, false);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xB9: // [LDA abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        LDA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xBA: // [TSX impl]
                switch (CPUStep)
                {
                    case 2:
                        X = SP;
                        SetFlag(1, X == 0);
                        SetFlag(7, (X & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xBC: // [LDY abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        LDY(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xBD: // [LDA abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        LDA(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xBE: // [LDX abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        LDX(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xC0: // [CPY #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        CPY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xC1: // [CMP X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xC4: // [CPY zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        CPY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xC5: // [CMP zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xC6: // [DEC zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        DEC(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xC8: // [INY impl]
                switch (CPUStep)
                {
                    case 2:
                        Y += 1;
                        SetFlag(1, Y == 0);
                        SetFlag(7, (Y & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xC9: // [CMP #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xCA: // [DEX impl]
                switch (CPUStep)
                {
                    case 2:
                        X--;
                        SetFlag(1, X == 0);
                        SetFlag(7, (X & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xCC: // [CPY abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CPY(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xCD: // [CMP abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xCE: // [DEC abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        DEC(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xD0: // [BNE rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_Byte = Bus.Data;
                        if (!ReadFlag(1))
                        {
                            ushort oldPC = PC;
                            H_UShort = (ushort)(PC + (sbyte)H_Byte);

                            CycleCollector--;

                            if ((oldPC & 0xFF00) != (H_UShort & 0xFF00))
                                CycleCollector--;
                            PC = H_UShort;
                        }
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xD1: // [CMP ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xD5: // [CMP zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xD6: // [DEC zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        DEC(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xD8: // [CLD impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(3, false);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xD9: // [CMP abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CMP(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xDD: // [CMP abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        CMP(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xDE: // [DEC abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_Byte = Bus.Data;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        DEC(ref H_Byte);
                        Bus.Data = H_Byte;
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 7:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xE0: // [CPX #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        CPX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xE1: // [SBC X, ind]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        SBC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xE4: // [CPX zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        CPX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xE5: // [SBC zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        SBC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xE6: // [INC zpg]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Data = INC(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xE8: // [INX impl]
                switch (CPUStep)
                {
                    case 2:
                        X++;
                        SetFlag(1, X == 0);
                        SetFlag(7, (X & 0x80) != 0);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xE9: // [SBC #]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_Byte = Bus.Data;
                        SBC(H_Byte);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xEA: // [NOP impl]
                switch (CPUStep)
                {
                    case 2:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xEC: // [CPX abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CPX(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xED: // [SBC abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        SBC(Bus.Data);
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xEE: // [INC abs]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Data = INC(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xF0: // [BEQ rel]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        if (ReadFlag(1)) // Wenn das Zero-Flag gesetzt ist (Z=1)
                        {
                            H_SByte = (sbyte)Bus.Data;
                            ushort oldPC = PC;
                            ushort newPC = (ushort)(PC + H_SByte);

                            CycleCollector--;

                            if ((oldPC & 0xFF00) != (newPC & 0xFF00))
                                CycleCollector--;

                            PC = newPC;
                        }
                        CPUStep = 0;
                        CycleCollector++;
                        break;
                }
                break;

            case 0xF1: // [SBC ind, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = Bus.Data;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_LByte = Bus.Data;
                        Bus.Address = (byte)(Bus.Address + 1);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 6:
                        CycleCollector++;
                        SBC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xF5: // [SBC zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        SBC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xF6: // [INC zpg, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        Bus.Address = (byte)(Bus.Data + X);
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        Bus.Data = INC(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 5:
                        CPUStep++;
                        break;

                    case 6:
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xF8: // [SED impl]
                switch (CPUStep)
                {
                    case 2:
                        SetFlag(3, true);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xF9: // [SBC abs, Y]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += Y;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        SBC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xFD: // [SBC abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        ushort oldAddress = Bus.Address;
                        Bus.Address += X;
                        if ((oldAddress & 0xFF00) != (Bus.Address & 0xFF00))
                            CycleCollector--;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        CycleCollector++;
                        SBC(Bus.Data);
                        CPUStep = 0;
                        break;
                }
                break;

            case 0xFE: // [INC abs, X]
                switch (CPUStep)
                {
                    case 2:
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 3:
                        H_LByte = Bus.Data;
                        Bus.Address = PC++;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 4:
                        H_HByte = Bus.Data;
                        Bus.Address = (ushort)(H_LByte | (H_HByte << 8));
                        Bus.Address += X;
                        Bus.Read = true;
                        CPUStep++;
                        break;

                    case 5:
                        Bus.Data = INC(Bus.Data);
                        Bus.Read = false;
                        CPUStep++;
                        break;

                    case 6:
                        CPUStep++;
                        break;

                    case 7:
                        CPUStep = 0;
                        break;
                }
                break;

            default:
                Debug.LogError("Unhandled OPCode: 0x" + OPC.ToString("X2"));
                Application.Quit();
                break;
        }
    }

    // Instructions
    private void ADC(byte A_Value)
    {
        // Its messed up because Dec Mode drove me crazy.
        // However, Binary Mode fully works, DEC mode works in >75% of cases.
        // I know I calculate OverflowFlag 2 Times, I dont care, I just dont want to touch that part of the code anymore.
        // If you still want to use Decimal Mode, why don't u just try it out?
        ushort T_Result;
        byte originalA = A;

        // Binary Operation
        T_Result = (ushort)(A + A_Value + (ReadFlag(0) ? 1 : 0));
        bool OverflowFlag = (((A ^ T_Result) & 0x80) != 0) && (((A ^ A_Value) & 0x80) == 0);
        SetFlag(6, OverflowFlag);

        if (ReadFlag(3))
        {
            // Decimal-Mode
            int T_LowerNibble = (A & 0x0F) + (A_Value & 0x0F) + (ReadFlag(0) ? 1 : 0);
            if (T_LowerNibble > 9)
                T_LowerNibble += 6;

            int T_UpperNibble = (A & 0xF0) + (A_Value & 0xF0) + (T_LowerNibble > 0x0F ? 0x10 : 0);
            T_Result = (ushort)(T_UpperNibble + (T_LowerNibble & 0x0F));

            SetFlag(7, (T_Result & 0x80) != 0);

            if (T_Result > 0x99)
            {
                T_Result += 0x60;
                SetFlag(0, true); // C-Flag
            }
            else
            {
                SetFlag(0, false);
            }

            A = (byte)T_Result;
        }
        else
        {
            // Binary-Mode
            SetFlag(0, T_Result > 0xFF);
            A = (byte)T_Result;
            SetFlag(7, (A & 0x80) != 0);
            bool OA = (originalA & 0x80) != 0;
            bool OM = (A_Value & 0x80) != 0;
            if (OA == OM)
            {
                bool OR = (A & 0x80) != 0;
                if (OA == OR)
                    SetFlag(6, false);
                else
                    SetFlag(6, true);
            }
            else
            {
                SetFlag(6, false);
            }
        }
        SetFlag(1, A == 0);
    }

    private void AND(byte value)
    {
        A = (byte)(A & value);
        SetFlag(1, A == 0);
        SetFlag(7, (A & 0x80) != 0);
    }

    private byte ASL(byte value)
    {
        SetFlag(0, (value & 0x80) != 0);
        value <<= 1;
        SetFlag(1, value == 0);
        SetFlag(7, (value & 0x80) != 0);
        return value;
    }

    private void BIT(byte operand)
    {
        SetFlag(1, (A & operand) == 0);
        SetFlag(7, (operand & 0x80) != 0);
        SetFlag(6, (operand & 0x40) != 0);
    }

    private void CMP(byte M)
    {
        ushort result = (ushort)(A - M);
        SetFlag(0, A >= M);
        SetFlag(1, (result & 0xFF) == 0);
        SetFlag(7, (result & 0x80) != 0);
    }

    private void CPX(byte M)
    {
        ushort result = (ushort)(X - M);
        SetFlag(0, X >= M);
        SetFlag(1, (result & 0xFF) == 0);
        SetFlag(7, (result & 0x80) != 0);
    }

    private void CPY(byte M)
    {
        ushort result = (ushort)(Y - M);
        SetFlag(0, Y >= M);
        SetFlag(1, (result & 0xFF) == 0);
        SetFlag(7, (result & 0x80) != 0);
    }

    private void DEC(ref byte memory)
    {
        memory--;
        SetFlag(1, memory == 0);
        SetFlag(7, (memory & 0x80) != 0);
    }

    private void EOR(byte value)
    {
        A ^= value;
        SetFlag(1, A == 0);
        SetFlag(7, (A & 0x80) != 0);
    }

    private byte INC(byte value)
    {
        value++;
        SetFlag(1, value == 0);
        SetFlag(7, (value & 0x80) != 0);
        return value;
    }

    private void LDA(byte value)
    {
        A = value;
        SetFlag(1, A == 0);
        SetFlag(7, (A & 0x80) != 0);
    }

    private void LDX(byte value)
    {
        X = value;
        SetFlag(1, X == 0);
        SetFlag(7, (X & 0x80) != 0);
    }

    private void LDY(byte value)
    {
        Y = value;
        SetFlag(1, Y == 0);
        SetFlag(7, (Y & 0x80) != 0);
    }

    private void LSR(ref byte value)
    {
        SetFlag(0, (value & 0x01) != 0);
        value >>= 1;
        SetFlag(1, value == 0);
        SetFlag(7, false);
    }

    private void ORA(byte A_Value)
    {
        A |= A_Value;
        SetFlag(1, A == 0);
        SetFlag(7, (A & 0x80) != 0);
    }

    private byte ROL(byte value)
    {
        bool carryIn = ReadFlag(0);
        bool carryOut = (value & 0x80) != 0;
        byte result = (byte)((value << 1) | (carryIn ? 1 : 0));

        SetFlag(0, carryOut);
        SetFlag(1, result == 0);
        SetFlag(7, (result & 0x80) != 0);

        return result;
    }

    private void ROR(ref byte value)
    {
        bool carryIn = ReadFlag(0);
        bool carryOut = (value & 0x01) != 0;

        value >>= 1;
        if (carryIn)
            value |= 0x80;

        SetFlag(0, carryOut);
        SetFlag(1, value == 0);
        SetFlag(7, (value & 0x80) != 0);
    }

    private void SBC(byte M_Value)
    {
        ADC((byte)(M_Value ^0xFF));
    }

    // Tools
    public bool ReadFlag(int A_Bit)
    {
        return (Status & (1 << A_Bit)) != 0;
    }

    public void SetFlag(int A_Bit, bool A_State)
    {
        if (A_State)
            Status |= (byte)(1 << A_Bit);
        else
            Status &= (byte)~(1 << A_Bit);
    }
}
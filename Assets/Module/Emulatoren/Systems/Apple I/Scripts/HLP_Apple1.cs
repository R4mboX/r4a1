using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLP_Apple1 : MonoBehaviour
{
    // Currently not getting used. But can be used to get Descriptions for OPCodes.

    public string GetOPCDesc(byte A_OPC)
    {
        string T_Response = "Missing";

        switch(A_OPC)
        {
            case 0x00:
                T_Response = "BRK impl";
                break;

            case 0x01:
                T_Response = "ORA ind, Y";
                break;

            case 0x02:
                T_Response = "Illegal";
                break;

            case 0x03:
                T_Response = "Illegal";
                break;

            case 0x04:
                T_Response = "Illegal";
                break;

            case 0x05:
                T_Response = "ORA zpg";
                break;

            case 0x06:
                T_Response = "ASL zpg";
                break;

            case 0x07:
                T_Response = "Illegal";
                break;

            case 0x08:
                T_Response = "PHP impl";
                break;

            case 0x09:
                T_Response = "ORA #";
                break;

            case 0x0A:
                T_Response = "ASL A";
                break;

            case 0x0B:
                T_Response = "Illegal";
                break;

            case 0x0C:
                T_Response = "Illegal";
                break;

            case 0x0D:
                T_Response = "ORA abs";
                break;

            case 0x0E:
                T_Response = "ASL abs";
                break;

            case 0x0F:
                T_Response = "Illegal";
                break;

            case 0x10:
                T_Response = "BPL rel";
                break;

            case 0x11:
                T_Response = "ORA ind, Y";
                break;

            case 0x12:
                T_Response = "Illegal";
                break;

            case 0x13:
                T_Response = "Illegal";
                break;

            case 0x14:
                T_Response = "Illegal";
                break;

            case 0x15:
                T_Response = "ORA zpg, X";
                break;

            case 0x16:
                T_Response = "ASL zpg, X";
                break;

            case 0x17:
                T_Response = "Illegal";
                break;

            case 0x18:
                T_Response = "CLC impl";
                break;

            case 0x19:
                T_Response = "ORA abs, Y";
                break;

            case 0x1A:
                T_Response = "Illegal";
                break;

            case 0x1B:
                T_Response = "Illegal";
                break;

            case 0x1C:
                T_Response = "Illegal";
                break;

            case 0x1D:
                T_Response = "ORA abs, X";
                break;

            case 0x1E:
                T_Response = "ASL abs, X";
                break;

            case 0x1F:
                T_Response = "Illegal";
                break;

            case 0x20:
                T_Response = "JSR abs";
                break;

            case 0x21:
                T_Response = "AND X, ind";
                break;

            case 0x22:
                T_Response = "Illegal";
                break;

            case 0x23:
                T_Response = "Illegal";
                break;

            case 0x24:
                T_Response = "BIT zpg";
                break;

            case 0x25:
                T_Response = "AND zpg";
                break;

            case 0x26:
                T_Response = "ROL zpg";
                break;

            case 0x27:
                T_Response = "Illegal";
                break;

            case 0x28:
                T_Response = "PLP impl";
                break;

            case 0x29:
                T_Response = "AND #";
                break;

            case 0x2A:
                T_Response = "ROL A";
                break;

            case 0x2B:
                T_Response = "Illegal";
                break;

            case 0x2C:
                T_Response = "BIT abs";
                break;

            case 0x2D:
                T_Response = "AND abs";
                break;

            case 0x2E:
                T_Response = "ROL abs";
                break;

            case 0x2F:
                T_Response = "Illegal";
                break;

            case 0x30:
                T_Response = "BMI rel";
                break;

            case 0x31:
                T_Response = "AND ind, Y";
                break;

            case 0x32:
                T_Response = "Illegal";
                break;

            case 0x33:
                T_Response = "Illegal";
                break;

            case 0x34:
                T_Response = "Illegal";
                break;

            case 0x35:
                T_Response = "AND zpg, X";
                break;

            case 0x36:
                T_Response = "ROL zpg, X";
                break;

            case 0x37:
                T_Response = "Illegal";
                break;

            case 0x38:
                T_Response = "SEC impl";
                break;

            case 0x39:
                T_Response = "AND abs, Y";
                break;

            case 0x3A:
                T_Response = "Illegal";
                break;

            case 0x3B:
                T_Response = "Illegal";
                break;

            case 0x3C:
                T_Response = "Illegal";
                break;

            case 0x3D:
                T_Response = "AND abs, X";
                break;

            case 0x3E:
                T_Response = "ROL abs, X";
                break;

            case 0x3F:
                T_Response = "Illegal";
                break;

            case 0x40:
                T_Response = "RTI impl";
                break;

            case 0x41:
                T_Response = "EOR X, ind";
                break;

            case 0x42:
                T_Response = "Illegal";
                break;

            case 0x43:
                T_Response = "Illegal";
                break;

            case 0x44:
                T_Response = "Illegal";
                break;

            case 0x45:
                T_Response = "EOR zpg";
                break;

            case 0x46:
                T_Response = "LSR zpg";
                break;

            case 0x47:
                T_Response = "Illegal";
                break;

            case 0x48:
                T_Response = "PHA impl";
                break;

            case 0x49:
                T_Response = "EOR #";
                break;

            case 0x4A:
                T_Response = "LSR A";
                break;

            case 0x4B:
                T_Response = "Illegal";
                break;

            case 0x4C:
                T_Response = "JMP abs";
                break;

            case 0x4D:
                T_Response = "EOR abs";
                break;

            case 0x4E:
                T_Response = "LSR abs";
                break;

            case 0x4F:
                T_Response = "Illegal";
                break;

            case 0x50:
                T_Response = "BVC rel";
                break;

            case 0x51:
                T_Response = "EOR ind, Y";
                break;

            case 0x52:
                T_Response = "Illegal";
                break;

            case 0x53:
                T_Response = "Illegal";
                break;

            case 0x54:
                T_Response = "Illegal";
                break;

            case 0x55:
                T_Response = "EOR zpg, X";
                break;

            case 0x56:
                T_Response = "LSR zpg, X";
                break;

            case 0x57:
                T_Response = "Illegal";
                break;

            case 0x58:
                T_Response = "CLI impl";
                break;

            case 0x59:
                T_Response = "EOR abs, Y";
                break;

            case 0x5A:
                T_Response = "Illegal";
                break;

            case 0x5B:
                T_Response = "Illegal";
                break;

            case 0x5C:
                T_Response = "Illegal";
                break;

            case 0x5D:
                T_Response = "EOR abs, X";
                break;

            case 0x5E:
                T_Response = "LSR abs, X";
                break;

            case 0x5F:
                T_Response = "Illegal";
                break;

            case 0x60:
                T_Response = "RTS impl";
                break;

            case 0x61:
                T_Response = "ADC X, ind";
                break;

            case 0x62:
                T_Response = "Illegal";
                break;

            case 0x63:
                T_Response = "Illegal";
                break;

            case 0x64:
                T_Response = "Illegal";
                break;

            case 0x65:
                T_Response = "ADC zpg";
                break;

            case 0x66:
                T_Response = "ROR zpg";
                break;

            case 0x67:
                T_Response = "Illegal";
                break;

            case 0x68:
                T_Response = "PLA impl";
                break;

            case 0x69:
                T_Response = "ADC #";
                break;

            case 0x6A:
                T_Response = "ROR A";
                break;

            case 0x6B:
                T_Response = "Illegal";
                break;

            case 0x6C:
                T_Response = "JMP ind";
                break;

            case 0x6D:
                T_Response = "ABC abs";
                break;

            case 0x6E:
                T_Response = "ROR abs";
                break;

            case 0x6F:
                T_Response = "Illegal";
                break;

            case 0x70:
                T_Response = "BVS rel";
                break;

            case 0x71:
                T_Response = "ADC ind, Y";
                break;

            case 0x72:
                T_Response = "Illegal";
                break;

            case 0x73:
                T_Response = "Illegal";
                break;

            case 0x74:
                T_Response = "Illegal";
                break;

            case 0x75:
                T_Response = "ADC zpg, X";
                break;

            case 0x76:
                T_Response = "ROR zpg, X";
                break;

            case 0x77:
                T_Response = "Illegal";
                break;

            case 0x78:
                T_Response = "SEI impl";
                break;

            case 0x79:
                T_Response = "ADC abs, Y";
                break;

            case 0x7A:
                T_Response = "Illegal";
                break;

            case 0x7B:
                T_Response = "Illegal";
                break;

            case 0x7C:
                T_Response = "Illegal";
                break;

            case 0x7D:
                T_Response = "ABC abs, X";
                break;

            case 0x7E:
                T_Response = "ROR abs, X";
                break;

            case 0x7F:
                T_Response = "Illegal";
                break;

            case 0x80:
                T_Response = "Illegal";
                break;

            case 0x81:
                T_Response = "STA X, ind";
                break;

            case 0x82:
                T_Response = "Illegal";
                break;

            case 0x83:
                T_Response = "Illegal";
                break;

            case 0x84:
                T_Response = "STY zpg";
                break;

            case 0x85:
                T_Response = "STA zpg";
                break;

            case 0x86:
                T_Response = "STX zpg";
                break;

            case 0x87:
                T_Response = "Illegal";
                break;

            case 0x88:
                T_Response = "DEY impl";
                break;

            case 0x89:
                T_Response = "Illegal";
                break;

            case 0x8A:
                T_Response = "TXA impl";
                break;

            case 0x8B:
                T_Response = "Illegal";
                break;

            case 0x8C:
                T_Response = "STY abs";
                break;

            case 0x8D:
                T_Response = "STA abs";
                break;

            case 0x8E:
                T_Response = "STX abs";
                break;

            case 0x8F:
                T_Response = "Illegal";
                break;

            case 0x90:
                T_Response = "BCC rel";
                break;

            case 0x91:
                T_Response = "STA ind, Y";
                break;

            case 0x92:
                T_Response = "Illegal";
                break;

            case 0x93:
                T_Response = "Illegal";
                break;

            case 0x94:
                T_Response = "STY zpg, X";
                break;

            case 0x95:
                T_Response = "STA zpg, X";
                break;

            case 0x96:
                T_Response = "STX zpg, Y";
                break;

            case 0x97:
                T_Response = "Illegal";
                break;

            case 0x98:
                T_Response = "TYA impl";
                break;

            case 0x99:
                T_Response = "STA abs, Y";
                break;

            case 0x9A:
                T_Response = "TXS impl";
                break;

            case 0x9B:
                T_Response = "Illegal";
                break;

            case 0x9C:
                T_Response = "Illegal";
                break;

            case 0x9D:
                T_Response = "STA abs, X";
                break;

            case 0x9E:
                T_Response = "Illegal";
                break;

            case 0x9F:
                T_Response = "Illegal";
                break;

            case 0xA0:
                T_Response = "LDY #";
                break;

            case 0xA1:
                T_Response = "LDA X, ind";
                break;

            case 0xA2:
                T_Response = "LDA X";
                break;

            case 0xA3:
                T_Response = "Illegal";
                break;

            case 0xA4:
                T_Response = "LDY zpg";
                break;

            case 0xA5:
                T_Response = "LDA zpg";
                break;

            case 0xA6:
                T_Response = "LDX zpg";
                break;

            case 0xA7:
                T_Response = "Illegal";
                break;

            case 0xA8:
                T_Response = "TAY impl";
                break;

            case 0xA9:
                T_Response = "LDA #";
                break;

            case 0xAA:
                T_Response = "TAX impl";
                break;

            case 0xAB:
                T_Response = "Illegal";
                break;

            case 0xAC:
                T_Response = "LDY abs";
                break;

            case 0xAD:
                T_Response = "LDA abs";
                break;

            case 0xAE:
                T_Response = "LDX abs";
                break;

            case 0xAF:
                T_Response = "Illegal";
                break;

            case 0xB0:
                T_Response = "BCS rel";
                break;

            case 0xB1:
                T_Response = "LDA ind, Y";
                break;

            case 0xB2:
                T_Response = "Illegal";
                break;

            case 0xB3:
                T_Response = "Illegal";
                break;

            case 0xB4:
                T_Response = "LDY zpg, X";
                break;

            case 0xB5:
                T_Response = "LDA zpg, X";
                break;

            case 0xB6:
                T_Response = "LDX zpg, Y";
                break;

            case 0xB7:
                T_Response = "Illegal";
                break;

            case 0xB8:
                T_Response = "CLV impl";
                break;

            case 0xB9:
                T_Response = "LDA abs, Y";
                break;

            case 0xBA:
                T_Response = "TSX impl";
                break;

            case 0xBB:
                T_Response = "Illegal";
                break;

            case 0xBC:
                T_Response = "LDY abs, X";
                break;

            case 0xBD:
                T_Response = "LDA abs, X";
                break;

            case 0xBE:
                T_Response = "LDX abs, Y";
                break;

            case 0xBF:
                T_Response = "Illegal";
                break;

            case 0xC0:
                T_Response = "CPY #";
                break;

            case 0xC1:
                T_Response = "CMP X, ind";
                break;

            case 0xC2:
                T_Response = "Illegal";
                break;

            case 0xC3:
                T_Response = "Illegal";
                break;

            case 0xC4:
                T_Response = "CPY zpg";
                break;

            case 0xC5:
                T_Response = "CMP zpg";
                break;

            case 0xC6:
                T_Response = "DEC zpg";
                break;

            case 0xC7:
                T_Response = "Illegal";
                break;

            case 0xC8:
                T_Response = "INY impl";
                break;

            case 0xC9:
                T_Response = "CMP #";
                break;

            case 0xCA:
                T_Response = "DEX impl";
                break;

            case 0xCB:
                T_Response = "Illegal";
                break;

            case 0xCC:
                T_Response = "CPY abs";
                break;

            case 0xCD:
                T_Response = "CMP abs";
                break;

            case 0xCE:
                T_Response = "DEC abs";
                break;

            case 0xCF:
                T_Response = "Illegal";
                break;

            case 0xD0:
                T_Response = "BNE rel";
                break;

            case 0xD1:
                T_Response = "CMP ind, Y";
                break;

            case 0xD2:
                T_Response = "Illegal";
                break;

            case 0xD3:
                T_Response = "Illegal";
                break;

            case 0xD4:
                T_Response = "Illegal";
                break;

            case 0xD5:
                T_Response = "CMP zpg, X";
                break;

            case 0xD6:
                T_Response = "DEC zpg, X";
                break;

            case 0xD7:
                T_Response = "Illegal";
                break;

            case 0xD8:
                T_Response = "CLD impl";
                break;

            case 0xD9:
                T_Response = "CMP abs, Y";
                break;

            case 0xDA:
                T_Response = "Illegal";
                break;

            case 0xDB:
                T_Response = "Illegal";
                break;

            case 0xDC:
                T_Response = "Illegal";
                break;

            case 0xDD:
                T_Response = "CMP abs, X";
                break;

            case 0xDE:
                T_Response = "DEC abs, X";
                break;

            case 0xDF:
                T_Response = "Illegal";
                break;

            case 0xE0:
                T_Response = "CPX #";
                break;

            case 0xE1:
                T_Response = "SEC X, ind";
                break;

            case 0xE2:
                T_Response = "Illegal";
                break;

            case 0xE3:
                T_Response = "Illegal";
                break;

            case 0xE4:
                T_Response = "CPX zpg";
                break;

            case 0xE5:
                T_Response = "SBC zpg";
                break;

            case 0xE6:
                T_Response = "INC zpg";
                break;

            case 0xE7:
                T_Response = "Illegal";
                break;

            case 0xE8:
                T_Response = "INX impl";
                break;

            case 0xE9:
                T_Response = "SBC #";
                break;

            case 0xEA:
                T_Response = "NOP impl";
                break;

            case 0xEB:
                T_Response = "Illegal";
                break;

            case 0xEC:
                T_Response = "CPX abs";
                break;

            case 0xED:
                T_Response = "SBC abs";
                break;

            case 0xEE:
                T_Response = "INC abs";
                break;

            case 0xEF:
                T_Response = "Illegal";
                break;

            case 0xF0:
                T_Response = "BEQ rel";
                break;

            case 0xF1:
                T_Response = "SBC ind, Y";
                break;

            case 0xF2:
                T_Response = "Illegal";
                break;

            case 0xF3:
                T_Response = "Illegal";
                break;

            case 0xF4:
                T_Response = "Illegal";
                break;

            case 0xF5:
                T_Response = "SBC zpg, X";
                break;

            case 0xF6:
                T_Response = "INC zpg, X";
                break;

            case 0xF7:
                T_Response = "Illegal";
                break;

            case 0xF8:
                T_Response = "SED impl";
                break;

            case 0xF9:
                T_Response = "SBC abs, Y";
                break;

            case 0xFA:
                T_Response = "Illegal";
                break;

            case 0xFB:
                T_Response = "Illegal";
                break;

            case 0xFC:
                T_Response = "Illegal";
                break;

            case 0xFD:
                T_Response = "SBC abs, X";
                break;

            case 0xFE:
                T_Response = "INC abs, X";
                break;

            case 0xFF:
                T_Response = "Illegal";
                break;
        }

        return T_Response;
    }
}

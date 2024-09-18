using System;
using UnityEngine;

// Thats an easy implementation. Final one shouldnt have access to PIA Functions

public class Keyboard_ASCII : MonoBehaviour
{
    [Header("Connections")]
    public PIA_Simple PIA;

    [Header("Settings")]
    public bool CFG_CapitalOnly = true;
    public bool KBDActive = true;
    public string Layout = "Original";
    public bool DebugKBD = false;

    [Header("Status")]
    public bool Shift = false;
    public bool Caps = false;
    public bool CTRL = false;
    public bool ALTGR = false;

    void Update()
    {
        if (KBDActive == false)
            return;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            Shift = true;

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            Shift = false;

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            CTRL = true;

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            CTRL = false;

        if (Input.GetKeyDown(KeyCode.CapsLock))
            Caps = true;

        if (Input.GetKeyUp(KeyCode.CapsLock))
            Caps = false;

        if (Input.GetKeyDown(KeyCode.RightAlt))
            ALTGR = true;

        if (Input.GetKeyUp(KeyCode.RightAlt))
            ALTGR = false;

        switch (Layout)
        {
            case "Original":
                Listen();
                break;

            case "US":
                ListenUS();
                break;

            case "UK":
                ListenUK();
                break;

            case "DE":
                ListenDE();
                break;

            default:
                Listen();
                break;
        }   
    }

    private void ListenDE()
    {
        if (ALTGR == true)
            CTRL = false;

        byte T_Value = 0xFF;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                switch (kcode)
                {
                    /*
                    case KeyCode.Backslash: // ^ Unfortunately its the same Keycode for <
                        if (Shift == false && Caps == false)
                            T_Value = 0x5E; // ^
                        break;
                    */

                    case KeyCode.Alpha0:
                        if (Shift == false && Caps == false)
                            T_Value = 0x30; // 0
                        if (Shift == true || Caps == true)
                            T_Value = 0x3D; // =
                        if (ALTGR == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.Alpha1:
                        if (Shift == false && Caps == false)
                            T_Value = 0x31; // 1
                        if (Shift == true || Caps == true)
                            T_Value = 0x21; // !
                        break;

                    case KeyCode.Alpha2:
                        if (Shift == false && Caps == false)
                            T_Value = 0x32; // 2
                        if (Shift == true || Caps == true)
                            T_Value = 0x22; // "
                        break;

                    case KeyCode.Alpha3:
                        if (Shift == false && Caps == false)
                            T_Value = 0x33; // 3
                        break;

                    case KeyCode.Alpha4:
                        if (Shift == false && Caps == false)
                            T_Value = 0x34; // 4
                        if (Shift == true || Caps == true)
                            T_Value = 0x24; // $
                        break;

                    case KeyCode.Alpha5:
                        if (Shift == false && Caps == false)
                            T_Value = 0x35; // 5
                        if (Shift == true || Caps == true)
                            T_Value = 0x25; // %
                        break;

                    case KeyCode.Alpha6:
                        if (Shift == false && Caps == false)
                            T_Value = 0x36; // 6
                        if (Shift == true || Caps == true)
                            T_Value = 0x26; // &
                        break;

                    case KeyCode.Alpha7:
                        if (Shift == false && Caps == false)
                            T_Value = 0x37; // 7
                        if (Shift == true || Caps == true)
                            T_Value = 0x2F; // /
                        if (ALTGR == true)
                            T_Value = 0x7B; // {
                        break;

                    case KeyCode.Alpha8:
                        if (Shift == false && Caps == false)
                            T_Value = 0x38; // 8
                        if (Shift == true || Caps == true)
                            T_Value = 0x28; // (
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5B; // [
                        if (ALTGR == true)
                            T_Value = 0x5B; // [
                        break;

                    case KeyCode.Alpha9:
                        if (Shift == false && Caps == false)
                            T_Value = 0x39; // 9
                        if (Shift == true || Caps == true)
                            T_Value = 0x29; // )
                        if (ALTGR == true)
                            T_Value = 0x5D; // ]
                        break;

                    case KeyCode.LeftBracket: // ß
                        if (Shift == true || Caps == true)
                            T_Value = 0x3F; // ?
                        if (ALTGR == true)
                            T_Value = 0x5C; // \
                        break;

                    case KeyCode.RightBracket: // ´
                        if (Shift == true || Caps == true)
                            T_Value = 0x60; // `
                        break;

                    case KeyCode.A:
                        if (Shift == false && Caps == false)
                            T_Value = 0x61; // a
                        if (Shift == true || Caps == true)
                            T_Value = 0x41; // A
                        if (CTRL == true && Shift == false)
                            T_Value = 0x01; // SOH
                        break;

                    case KeyCode.B:
                        if (Shift == false && Caps == false)
                            T_Value = 0x62; // b
                        if (Shift == true || Caps == true)
                            T_Value = 0x42; // B
                        if (CTRL == true && Shift == false)
                            T_Value = 0x02; // STX
                        break;

                    case KeyCode.C:
                        if (Shift == false && Caps == false)
                            T_Value = 0x63; // c
                        if (Shift == true || Caps == true)
                            T_Value = 0x43; // C
                        if (CTRL == true && Shift == false)
                            T_Value = 0x03; // ETX
                        break;

                    case KeyCode.D:
                        if (Shift == false && Caps == false)
                            T_Value = 0x64; // d
                        if (Shift == true || Caps == true)
                            T_Value = 0x44; // D
                        if (CTRL == true && Shift == false)
                            T_Value = 0x04; // EOT
                        break;

                    case KeyCode.E:
                        if (Shift == false && Caps == false)
                            T_Value = 0x65; // e
                        if (Shift == true || Caps == true)
                            T_Value = 0x45; // E
                        if (CTRL == true && Shift == false)
                            T_Value = 0x05; // ENQ
                        break;

                    case KeyCode.F:
                        if (Shift == false && Caps == false)
                            T_Value = 0x66; // f
                        if (Shift == true || Caps == true)
                            T_Value = 0x46; // F
                        if (CTRL == true && Shift == false)
                            T_Value = 0x06; // ACK
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.G:
                        if (Shift == false && Caps == false)
                            T_Value = 0x67; // g
                        if (Shift == true || Caps == true)
                            T_Value = 0x47; // G
                        if (CTRL == true && Shift == false)
                            T_Value = 0x07; // BEL
                        break;

                    case KeyCode.H:
                        if (Shift == false && Caps == false)
                            T_Value = 0x68; // h
                        if (Shift == true || Caps == true)
                            T_Value = 0x48; // H
                        if (CTRL == true && Shift == false)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.I:
                        if (Shift == false && Caps == false)
                            T_Value = 0x69; // i
                        if (Shift == true || Caps == true)
                            T_Value = 0x49; // I
                        if (CTRL == true && Shift == false)
                            T_Value = 0x09; // HT
                        break;

                    case KeyCode.J:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6A; // j
                        if (Shift == true || Caps == true)
                            T_Value = 0x4A; // J
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0A; // LF
                        break;

                    case KeyCode.K:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6B; // k
                        if (Shift == true || Caps == true)
                            T_Value = 0x4B; // K
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0B; // VT
                        break;

                    case KeyCode.L:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6C; // l
                        if (Shift == true || Caps == true)
                            T_Value = 0x4C; // L
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0C; // FF
                        break;

                    case KeyCode.M:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6D; // m
                        if (Shift == true || Caps == true)
                            T_Value = 0x4D; // M
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.N:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6E; // n
                        if (Shift == true || Caps == true)
                            T_Value = 0x4E; // N
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0E; // SO
                        if (CTRL == true && Shift == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.O:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6F; // o
                        if (Shift == true || Caps == true)
                            T_Value = 0x4F; // O
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0F; // SI
                        break;

                    case KeyCode.P:
                        if (Shift == false && Caps == false)
                            T_Value = 0x70; // p
                        if (Shift == true || Caps == true)
                            T_Value = 0x50; // P
                        if (CTRL == true && Shift == false)
                            T_Value = 0x10; // DLE
                        if (CTRL == true && Shift == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Q:
                        if (Shift == false && Caps == false)
                            T_Value = 0x71; // q
                        if (Shift == true || Caps == true)
                            T_Value = 0x51; // Q
                        if (ALTGR == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.R:
                        if (Shift == false && Caps == false)
                            T_Value = 0x72; // r
                        if (Shift == true || Caps == true)
                            T_Value = 0x52; // R
                        if (CTRL == true && Shift == false)
                            T_Value = 0x12; // DC2
                        break;

                    case KeyCode.S:
                        if (Shift == false && Caps == false)
                            T_Value = 0x73; // s
                        if (Shift == true || Caps == true)
                            T_Value = 0x53; // S
                        if (CTRL == true && Shift == false)
                            T_Value = 0x13; // DC3
                        break;

                    case KeyCode.T:
                        if (Shift == false && Caps == false)
                            T_Value = 0x74; // t
                        if (Shift == true || Caps == true)
                            T_Value = 0x54; // T
                        if (CTRL == true && Shift == false)
                            T_Value = 0x14; // DC4]
                        break;

                    case KeyCode.U:
                        if (Shift == false && Caps == false)
                            T_Value = 0x75; // u
                        if (Shift == true || Caps == true)
                            T_Value = 0x55; // U
                        if (CTRL == true && Shift == false)
                            T_Value = 0x15; // NAK
                        break;

                    case KeyCode.V:
                        if (Shift == false && Caps == false)
                            T_Value = 0x76; // v
                        if (Shift == true || Caps == true)
                            T_Value = 0x56; // V
                        if (CTRL == true && Shift == false)
                            T_Value = 0x16; // SYN
                        break;

                    case KeyCode.W:
                        if (Shift == false && Caps == false)
                            T_Value = 0x77; // w
                        if (Shift == true || Caps == true)
                            T_Value = 0x57; // W
                        if (CTRL == true && Shift == false)
                            T_Value = 0x17; // ETB
                        break;

                    case KeyCode.X:
                        if (Shift == false && Caps == false)
                            T_Value = 0x78; // x
                        if (Shift == true || Caps == true)
                            T_Value = 0x58; // X
                        if (CTRL == true && Shift == false)
                            T_Value = 0x18; // CAN
                        break;

                    case KeyCode.Y:
                        if (Shift == false && Caps == false)
                            T_Value = 0x79; // y
                        if (Shift == true || Caps == true)
                            T_Value = 0x59; // Y
                        if (CTRL == true && Shift == false)
                            T_Value = 0x19; // EM
                        break;

                    case KeyCode.Z:
                        if (Shift == false && Caps == false)
                            T_Value = 0x7A; // z
                        if (Shift == true || Caps == true)
                            T_Value = 0x5A; // Z
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1A; // SUB
                        break;

                    
                    case KeyCode.Equals:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2B; // +
                        if (Shift == true || Caps == true)
                            T_Value = 0x2A; // *
                        if (ALTGR == true)
                            T_Value = 0x7E; // ~
                        break;

                    case KeyCode.Slash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x23; // #
                        break;

                    case KeyCode.Backslash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3C; // <
                        if (Shift == true || Caps == true)
                            T_Value = 0x3E; // >
                        if (ALTGR == true)
                            T_Value = 0x7C; // |
                        break;

                    case KeyCode.Comma:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2C; // ,
                        if (Shift == true || Caps == true)
                            T_Value = 0x3B; // ;
                        break;

                    case KeyCode.Period:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2E; // .
                        if (Shift == true || Caps == true)
                            T_Value = 0x3A; // :
                        break;

                    case KeyCode.Minus: // -
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        if (Shift == true || Caps == true)
                            T_Value = 0x5F; // _
                        break;

                    case KeyCode.Space:
                        if (Shift == false && Caps == false)
                            T_Value = 0x20; // SP
                        break;

                    case KeyCode.Return:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Backspace: // Bonus
                        if (Shift == false && Caps == false)
                            T_Value = 0x5F; // DEL // 7F??? originally
                        if (Shift == true || Caps == true)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.Escape:
                        if (Shift == false && Caps == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.KeypadPlus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2B; // +
                        break;

                    case KeyCode.KeypadMultiply:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2A; // *
                        break;

                    case KeyCode.KeypadMinus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        break;

                    case KeyCode.KeypadDivide:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // /
                        break;

                    case KeyCode.KeypadEnter:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                        break;

                    default:
                        break;
                }
                if (DebugKBD == true)
                    Debug.Log("[Keyboard] KeyCode: " + kcode);
            }
        }
        byte O_Value = T_Value;

        // Only Capital Letters
        if (T_Value >= 0x61 && T_Value <= 0x7A && CFG_CapitalOnly == true)
            T_Value -= 0x20;

        T_Value = (byte)(T_Value | (1 << 7));

        if (O_Value != 0xFF)
        {
            //Debug.Log("KeyCode down: " + kcode + "; Value sent: 0x" + T_Value.ToString("X2"));
            PIA.KBD = T_Value;
            PIA.KBDCR = SetFlag(PIA.KBDCR, 7, true);
        }
    }

    private void ListenUS()
    {
        byte T_Value = 0xFF;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                switch (kcode)
                {
                    case KeyCode.BackQuote: // Unfortunately its the same as '
                        if (Shift == false && Caps == false)
                            T_Value = 0x60; // `
                        break;

                    case KeyCode.Alpha1:
                        if (Shift == false && Caps == false)
                            T_Value = 0x31; // 1
                        if (Shift == true || Caps == true)
                            T_Value = 0x21; // !
                        break;

                    case KeyCode.Alpha2:
                        if (Shift == false && Caps == false)
                            T_Value = 0x32; // 2
                        if (Shift == true || Caps == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Alpha3:
                        if (Shift == false && Caps == false)
                            T_Value = 0x33; // 3
                        if (Shift == true || Caps == true)
                            T_Value = 0x23; // #
                        break;

                    case KeyCode.Alpha4:
                        if (Shift == false && Caps == false)
                            T_Value = 0x34; // 4
                        if (Shift == true || Caps == true)
                            T_Value = 0x24; // $
                        break;

                    case KeyCode.Alpha5:
                        if (Shift == false && Caps == false)
                            T_Value = 0x35; // 5
                        if (Shift == true || Caps == true)
                            T_Value = 0x25; // %
                        break;

                    case KeyCode.Alpha6:
                        if (Shift == false && Caps == false)
                            T_Value = 0x36; // 6
                        if (Shift == true || Caps == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.Alpha7:
                        if (Shift == false && Caps == false)
                            T_Value = 0x37; // 7
                        if (Shift == true || Caps == true)
                            T_Value = 0x26; // &
                        break;

                    case KeyCode.Alpha8:
                        if (Shift == false && Caps == false)
                            T_Value = 0x38; // 8
                        if (Shift == true || Caps == true)
                            T_Value = 0x2A; // *
                        break;

                    case KeyCode.Alpha9:
                        if (Shift == false && Caps == false)
                            T_Value = 0x39; // 9
                        if (Shift == true || Caps == true)
                            T_Value = 0x28; // (
                        break;

                    case KeyCode.Alpha0:
                        if (Shift == false && Caps == false)
                            T_Value = 0x30; // 0
                        if (Shift == true || Caps == true)
                            T_Value = 0x29; // )
                        if (CTRL == true)
                            T_Value = 0x00; // NUL
                        break;

                    case KeyCode.Minus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        if (Shift == true || Caps == true)
                            T_Value = 0x5F; // _
                        break;

                    case KeyCode.Equals:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3D; // =
                        if (Shift == true || Caps == true)
                            T_Value = 0x2B; // +
                        break;

                    case KeyCode.A:
                        if (Shift == false && Caps == false)
                            T_Value = 0x61; // a
                        if (Shift == true || Caps == true)
                            T_Value = 0x41; // A
                        if (CTRL == true && Shift == false)
                            T_Value = 0x01; // SOH
                        break;

                    case KeyCode.B:
                        if (Shift == false && Caps == false)
                            T_Value = 0x62; // b
                        if (Shift == true || Caps == true)
                            T_Value = 0x42; // B
                        if (CTRL == true && Shift == false)
                            T_Value = 0x02; // STX
                        break;

                    case KeyCode.C:
                        if (Shift == false && Caps == false)
                            T_Value = 0x63; // c
                        if (Shift == true || Caps == true)
                            T_Value = 0x43; // C
                        if (CTRL == true && Shift == false)
                            T_Value = 0x03; // ETX
                        break;

                    case KeyCode.D:
                        if (Shift == false && Caps == false)
                            T_Value = 0x64; // d
                        if (Shift == true || Caps == true)
                            T_Value = 0x44; // D
                        if (CTRL == true && Shift == false)
                            T_Value = 0x04; // EOT
                        break;

                    case KeyCode.E:
                        if (Shift == false && Caps == false)
                            T_Value = 0x65; // e
                        if (Shift == true || Caps == true)
                            T_Value = 0x45; // E
                        if (CTRL == true && Shift == false)
                            T_Value = 0x05; // ENQ
                        break;

                    case KeyCode.F:
                        if (Shift == false && Caps == false)
                            T_Value = 0x66; // f
                        if (Shift == true || Caps == true)
                            T_Value = 0x46; // F
                        if (CTRL == true && Shift == false)
                            T_Value = 0x06; // ACK
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.G:
                        if (Shift == false && Caps == false)
                            T_Value = 0x67; // g
                        if (Shift == true || Caps == true)
                            T_Value = 0x47; // G
                        if (CTRL == true && Shift == false)
                            T_Value = 0x07; // BEL
                        break;

                    case KeyCode.H:
                        if (Shift == false && Caps == false)
                            T_Value = 0x68; // h
                        if (Shift == true || Caps == true)
                            T_Value = 0x48; // H
                        if (CTRL == true && Shift == false)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.I:
                        if (Shift == false && Caps == false)
                            T_Value = 0x69; // i
                        if (Shift == true || Caps == true)
                            T_Value = 0x49; // I
                        if (CTRL == true && Shift == false)
                            T_Value = 0x09; // HT
                        break;

                    case KeyCode.J:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6A; // j
                        if (Shift == true || Caps == true)
                            T_Value = 0x4A; // J
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0A; // LF
                        break;

                    case KeyCode.K:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6B; // k
                        if (Shift == true || Caps == true)
                            T_Value = 0x4B; // K
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0B; // VT
                        break;

                    case KeyCode.L:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6C; // l
                        if (Shift == true || Caps == true)
                            T_Value = 0x4C; // L
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0C; // FF
                        break;

                    case KeyCode.M:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6D; // m
                        if (Shift == true || Caps == true)
                            T_Value = 0x4D; // M
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.N:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6E; // n
                        if (Shift == true || Caps == true)
                            T_Value = 0x4E; // N
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0E; // SO
                        if (CTRL == true && Shift == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.O:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6F; // o
                        if (Shift == true || Caps == true)
                            T_Value = 0x4F; // O
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0F; // SI
                        break;

                    case KeyCode.P:
                        if (Shift == false && Caps == false)
                            T_Value = 0x70; // p
                        if (Shift == true || Caps == true)
                            T_Value = 0x50; // P
                        if (CTRL == true && Shift == false)
                            T_Value = 0x10; // DLE
                        if (CTRL == true && Shift == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Q:
                        if (Shift == false && Caps == false)
                            T_Value = 0x71; // q
                        if (Shift == true || Caps == true)
                            T_Value = 0x51; // Q
                        if (CTRL == true && Shift == false)
                            T_Value = 0x11; // DC1
                        break;

                    case KeyCode.R:
                        if (Shift == false && Caps == false)
                            T_Value = 0x72; // r
                        if (Shift == true || Caps == true)
                            T_Value = 0x52; // R
                        if (CTRL == true && Shift == false)
                            T_Value = 0x12; // DC2
                        break;

                    case KeyCode.S:
                        if (Shift == false && Caps == false)
                            T_Value = 0x73; // s
                        if (Shift == true || Caps == true)
                            T_Value = 0x53; // S
                        if (CTRL == true && Shift == false)
                            T_Value = 0x13; // DC3
                        break;

                    case KeyCode.T:
                        if (Shift == false && Caps == false)
                            T_Value = 0x74; // t
                        if (Shift == true || Caps == true)
                            T_Value = 0x54; // T
                        if (CTRL == true && Shift == false)
                            T_Value = 0x14; // DC4]
                        break;

                    case KeyCode.U:
                        if (Shift == false && Caps == false)
                            T_Value = 0x75; // u
                        if (Shift == true || Caps == true)
                            T_Value = 0x55; // U
                        if (CTRL == true && Shift == false)
                            T_Value = 0x15; // NAK
                        break;

                    case KeyCode.V:
                        if (Shift == false && Caps == false)
                            T_Value = 0x76; // v
                        if (Shift == true || Caps == true)
                            T_Value = 0x56; // V
                        if (CTRL == true && Shift == false)
                            T_Value = 0x16; // SYN
                        break;

                    case KeyCode.W:
                        if (Shift == false && Caps == false)
                            T_Value = 0x77; // w
                        if (Shift == true || Caps == true)
                            T_Value = 0x57; // W
                        if (CTRL == true && Shift == false)
                            T_Value = 0x17; // ETB
                        break;

                    case KeyCode.X:
                        if (Shift == false && Caps == false)
                            T_Value = 0x78; // x
                        if (Shift == true || Caps == true)
                            T_Value = 0x58; // X
                        if (CTRL == true && Shift == false)
                            T_Value = 0x18; // CAN
                        break;

                    case KeyCode.Y:
                        if (Shift == false && Caps == false)
                            T_Value = 0x79; // y
                        if (Shift == true || Caps == true)
                            T_Value = 0x59; // Y
                        if (CTRL == true && Shift == false)
                            T_Value = 0x19; // EM
                        break;

                    case KeyCode.Z:
                        if (Shift == false && Caps == false)
                            T_Value = 0x7A; // z
                        if (Shift == true || Caps == true)
                            T_Value = 0x5A; // Z
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1A; // SUB
                        break;

                    case KeyCode.LeftBracket:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5B; // [
                        if (Shift == true || Caps == true)
                            T_Value = 0x7B; // {
                        break;

                    case KeyCode.RightBracket:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5D; // ]
                        if (Shift == true || Caps == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.Semicolon:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3B; // ;
                        if (Shift == true || Caps == true)
                            T_Value = 0x3A; // :
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.Quote:
                        if (Shift == true || Caps == true)
                            T_Value = 0x22; // "
                        break;

                    case KeyCode.Backslash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5C; // \
                        if (Shift == true || Caps == true)
                            T_Value = 0x7C; // |
                        break;

                    case KeyCode.Space:
                        if (Shift == false && Caps == false)
                            T_Value = 0x20; // SP
                        break;

                    case KeyCode.Return:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Backspace: // Bonus
                        if (Shift == false && Caps == false)
                            T_Value = 0x5F; // DEL // 7F??? originally
                        if (Shift == true || Caps == true)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.Escape:
                        if (Shift == false && Caps == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.Period:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2E; // .
                        if (Shift == true || Caps == true)
                            T_Value = 0x3E; // >
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1E; // RS
                        break;

                    case KeyCode.Comma:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2C; // ,
                        if (Shift == true || Caps == true)
                            T_Value = 0x3C; // <
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1C; // FS
                        break;

                    case KeyCode.Slash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // SLASH
                        if (Shift == true || Caps == true)
                            T_Value = 0x3F; // ?
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5C; // BACKSLASH
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7C; // |
                        break;

                    case KeyCode.KeypadPlus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2B; // +
                        break;

                    case KeyCode.KeypadMultiply:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2A; // *
                        break;

                    case KeyCode.KeypadMinus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        break;

                    case KeyCode.KeypadDivide:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // /
                        break;

                    case KeyCode.KeypadEnter:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                        break;

                    default:
                        break;
                }
                if (DebugKBD == true)
                    Debug.Log("[Keyboard] KeyCode: " + kcode);
            }
        }

        byte O_Value = T_Value;

        // Only Capital Letters
        if (T_Value >= 0x61 && T_Value <= 0x7A && CFG_CapitalOnly == true)
            T_Value -= 0x20;

        T_Value = (byte)(T_Value | (1 << 7));

        if (O_Value != 0xFF)
        {
            //Debug.Log("KeyCode down: " + kcode + "; Value sent: 0x" + T_Value.ToString("X2"));
            PIA.KBD = T_Value;
            PIA.KBDCR = SetFlag(PIA.KBDCR, 7, true);
        }
    }

    private void ListenUK()
    {
        byte T_Value = 0xFF;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                switch (kcode)
                {
                    /*
                    case KeyCode.BackQuote: // Unfortunately its the same as '
                        if (Shift == false && Caps == false)
                            T_Value = 0x60; // `
                        break;
                    */

                    case KeyCode.Alpha1:
                        if (Shift == false && Caps == false)
                            T_Value = 0x31; // 1
                        if (Shift == true || Caps == true)
                            T_Value = 0x21; // !
                        break;

                    case KeyCode.Alpha2:
                        if (Shift == false && Caps == false)
                            T_Value = 0x32; // 2
                        if (Shift == true || Caps == true)
                            T_Value = 0x22; // "
                        break;

                    case KeyCode.Alpha3:
                        if (Shift == false && Caps == false)
                            T_Value = 0x33; // 3
                        break;

                    case KeyCode.Alpha4:
                        if (Shift == false && Caps == false)
                            T_Value = 0x34; // 4
                        if (Shift == true || Caps == true)
                            T_Value = 0x24; // $
                        break;

                    case KeyCode.Alpha5:
                        if (Shift == false && Caps == false)
                            T_Value = 0x35; // 5
                        if (Shift == true || Caps == true)
                            T_Value = 0x25; // %
                        break;

                    case KeyCode.Alpha6:
                        if (Shift == false && Caps == false)
                            T_Value = 0x36; // 6
                        if (Shift == true || Caps == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.Alpha7:
                        if (Shift == false && Caps == false)
                            T_Value = 0x37; // 7
                        if (Shift == true || Caps == true)
                            T_Value = 0x26; // &
                        break;

                    case KeyCode.Alpha8:
                        if (Shift == false && Caps == false)
                            T_Value = 0x38; // 8
                        if (Shift == true || Caps == true)
                            T_Value = 0x2A; // *
                        break;

                    case KeyCode.Alpha9:
                        if (Shift == false && Caps == false)
                            T_Value = 0x39; // 9
                        if (Shift == true || Caps == true)
                            T_Value = 0x28; // (
                        break;

                    case KeyCode.Alpha0:
                        if (Shift == false && Caps == false)
                            T_Value = 0x30; // 0
                        if (Shift == true || Caps == true)
                            T_Value = 0x29; // )
                        if (CTRL == true)
                            T_Value = 0x00; // NUL
                        break;

                    case KeyCode.Minus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        if (Shift == true || Caps == true)
                            T_Value = 0x5F; // _
                        break;

                    case KeyCode.Equals:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3D; // =
                        if (Shift == true || Caps == true)
                            T_Value = 0x2B; // +
                        break;

                    case KeyCode.A:
                        if (Shift == false && Caps == false)
                            T_Value = 0x61; // a
                        if (Shift == true || Caps == true)
                            T_Value = 0x41; // A
                        if (CTRL == true && Shift == false)
                            T_Value = 0x01; // SOH
                        break;

                    case KeyCode.B:
                        if (Shift == false && Caps == false)
                            T_Value = 0x62; // b
                        if (Shift == true || Caps == true)
                            T_Value = 0x42; // B
                        if (CTRL == true && Shift == false)
                            T_Value = 0x02; // STX
                        break;

                    case KeyCode.C:
                        if (Shift == false && Caps == false)
                            T_Value = 0x63; // c
                        if (Shift == true || Caps == true)
                            T_Value = 0x43; // C
                        if (CTRL == true && Shift == false)
                            T_Value = 0x03; // ETX
                        break;

                    case KeyCode.D:
                        if (Shift == false && Caps == false)
                            T_Value = 0x64; // d
                        if (Shift == true || Caps == true)
                            T_Value = 0x44; // D
                        if (CTRL == true && Shift == false)
                            T_Value = 0x04; // EOT
                        break;

                    case KeyCode.E:
                        if (Shift == false && Caps == false)
                            T_Value = 0x65; // e
                        if (Shift == true || Caps == true)
                            T_Value = 0x45; // E
                        if (CTRL == true && Shift == false)
                            T_Value = 0x05; // ENQ
                        break;

                    case KeyCode.F:
                        if (Shift == false && Caps == false)
                            T_Value = 0x66; // f
                        if (Shift == true || Caps == true)
                            T_Value = 0x46; // F
                        if (CTRL == true && Shift == false)
                            T_Value = 0x06; // ACK
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.G:
                        if (Shift == false && Caps == false)
                            T_Value = 0x67; // g
                        if (Shift == true || Caps == true)
                            T_Value = 0x47; // G
                        if (CTRL == true && Shift == false)
                            T_Value = 0x07; // BEL
                        break;

                    case KeyCode.H:
                        if (Shift == false && Caps == false)
                            T_Value = 0x68; // h
                        if (Shift == true || Caps == true)
                            T_Value = 0x48; // H
                        if (CTRL == true && Shift == false)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.I:
                        if (Shift == false && Caps == false)
                            T_Value = 0x69; // i
                        if (Shift == true || Caps == true)
                            T_Value = 0x49; // I
                        if (CTRL == true && Shift == false)
                            T_Value = 0x09; // HT
                        break;

                    case KeyCode.J:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6A; // j
                        if (Shift == true || Caps == true)
                            T_Value = 0x4A; // J
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0A; // LF
                        break;

                    case KeyCode.K:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6B; // k
                        if (Shift == true || Caps == true)
                            T_Value = 0x4B; // K
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0B; // VT
                        break;

                    case KeyCode.L:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6C; // l
                        if (Shift == true || Caps == true)
                            T_Value = 0x4C; // L
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0C; // FF
                        break;

                    case KeyCode.M:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6D; // m
                        if (Shift == true || Caps == true)
                            T_Value = 0x4D; // M
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.N:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6E; // n
                        if (Shift == true || Caps == true)
                            T_Value = 0x4E; // N
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0E; // SO
                        if (CTRL == true && Shift == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.O:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6F; // o
                        if (Shift == true || Caps == true)
                            T_Value = 0x4F; // O
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0F; // SI
                        break;

                    case KeyCode.P:
                        if (Shift == false && Caps == false)
                            T_Value = 0x70; // p
                        if (Shift == true || Caps == true)
                            T_Value = 0x50; // P
                        if (CTRL == true && Shift == false)
                            T_Value = 0x10; // DLE
                        if (CTRL == true && Shift == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Q:
                        if (Shift == false && Caps == false)
                            T_Value = 0x71; // q
                        if (Shift == true || Caps == true)
                            T_Value = 0x51; // Q
                        if (CTRL == true && Shift == false)
                            T_Value = 0x11; // DC1
                        break;

                    case KeyCode.R:
                        if (Shift == false && Caps == false)
                            T_Value = 0x72; // r
                        if (Shift == true || Caps == true)
                            T_Value = 0x52; // R
                        if (CTRL == true && Shift == false)
                            T_Value = 0x12; // DC2
                        break;

                    case KeyCode.S:
                        if (Shift == false && Caps == false)
                            T_Value = 0x73; // s
                        if (Shift == true || Caps == true)
                            T_Value = 0x53; // S
                        if (CTRL == true && Shift == false)
                            T_Value = 0x13; // DC3
                        break;

                    case KeyCode.T:
                        if (Shift == false && Caps == false)
                            T_Value = 0x74; // t
                        if (Shift == true || Caps == true)
                            T_Value = 0x54; // T
                        if (CTRL == true && Shift == false)
                            T_Value = 0x14; // DC4]
                        break;

                    case KeyCode.U:
                        if (Shift == false && Caps == false)
                            T_Value = 0x75; // u
                        if (Shift == true || Caps == true)
                            T_Value = 0x55; // U
                        if (CTRL == true && Shift == false)
                            T_Value = 0x15; // NAK
                        break;

                    case KeyCode.V:
                        if (Shift == false && Caps == false)
                            T_Value = 0x76; // v
                        if (Shift == true || Caps == true)
                            T_Value = 0x56; // V
                        if (CTRL == true && Shift == false)
                            T_Value = 0x16; // SYN
                        break;

                    case KeyCode.W:
                        if (Shift == false && Caps == false)
                            T_Value = 0x77; // w
                        if (Shift == true || Caps == true)
                            T_Value = 0x57; // W
                        if (CTRL == true && Shift == false)
                            T_Value = 0x17; // ETB
                        break;

                    case KeyCode.X:
                        if (Shift == false && Caps == false)
                            T_Value = 0x78; // x
                        if (Shift == true || Caps == true)
                            T_Value = 0x58; // X
                        if (CTRL == true && Shift == false)
                            T_Value = 0x18; // CAN
                        break;

                    case KeyCode.Y:
                        if (Shift == false && Caps == false)
                            T_Value = 0x79; // y
                        if (Shift == true || Caps == true)
                            T_Value = 0x59; // Y
                        if (CTRL == true && Shift == false)
                            T_Value = 0x19; // EM
                        break;

                    case KeyCode.Z:
                        if (Shift == false && Caps == false)
                            T_Value = 0x7A; // z
                        if (Shift == true || Caps == true)
                            T_Value = 0x5A; // Z
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1A; // SUB
                        break;

                    case KeyCode.LeftBracket:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5B; // [
                        if (Shift == true || Caps == true)
                            T_Value = 0x7B; // {
                        break;

                    case KeyCode.RightBracket:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5D; // ]
                        if (Shift == true || Caps == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.Semicolon:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3B; // ;
                        if (Shift == true || Caps == true)
                            T_Value = 0x3A; // :
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.BackQuote:
                        if (Shift == false && Caps == false)
                            T_Value = 0x60; // ` Normally: '
                        if (Shift == true || Caps == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Quote:
                        if (Shift == false && Caps == false)
                            T_Value = 0x23; // #
                        if (Shift == true || Caps == true)
                            T_Value = 0x7E; // ~
                        break;

                    case KeyCode.Backslash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x5C; // \
                        if (Shift == true || Caps == true)
                            T_Value = 0x7C; // |
                        break;

                    case KeyCode.Space:
                        if (Shift == false && Caps == false)
                            T_Value = 0x20; // SP
                        break;

                    case KeyCode.Return:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Backspace: // Bonus
                        if (Shift == false && Caps == false)
                            T_Value = 0x5F; // DEL // 7F??? originally
                        if (Shift == true || Caps == true)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.Escape:
                        if (Shift == false && Caps == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.Period:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2E; // .
                        if (Shift == true || Caps == true)
                            T_Value = 0x3E; // >
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1E; // RS
                        break;

                    case KeyCode.Comma:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2C; // ,
                        if (Shift == true || Caps == true)
                            T_Value = 0x3C; // <
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1C; // FS
                        break;

                    case KeyCode.Slash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // SLASH
                        if (Shift == true || Caps == true)
                            T_Value = 0x3F; // ?
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5C; // BACKSLASH
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7C; // |
                        break;
                                       
                    case KeyCode.KeypadPlus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2B; // +
                        break;

                    case KeyCode.KeypadMultiply:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2A; // *
                        break;

                    case KeyCode.KeypadMinus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        break;

                    case KeyCode.KeypadDivide:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // /
                        break;

                    case KeyCode.KeypadEnter:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                        break;

                    default:
                        break;
                }
                if (DebugKBD == true)
                    Debug.Log("[Keyboard] KeyCode: " + kcode);
            }
        }

        byte O_Value = T_Value;

        // Only Capital Letters
        if (T_Value >= 0x61 && T_Value <= 0x7A && CFG_CapitalOnly == true)
            T_Value -= 0x20;

        T_Value = (byte)(T_Value | (1 << 7));

        if (O_Value != 0xFF)
        {
            //Debug.Log("KeyCode down: " + kcode + "; Value sent: 0x" + T_Value.ToString("X2"));
            PIA.KBD = T_Value;
            PIA.KBDCR = SetFlag(PIA.KBDCR, 7, true);
        }
    }

    private void Listen()
    {
        byte T_Value = 0xFF;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                switch (kcode)
                {
                    case KeyCode.Alpha0:
                        if (Shift == false && Caps == false)
                            T_Value = 0x30; // 0
                        if (Shift == true || Caps == true)
                            T_Value = 0x5E; // ^
                        if (CTRL == true)
                            T_Value = 0x00; // NUL
                        break;

                    case KeyCode.Alpha1:
                        if (Shift == false && Caps == false)
                            T_Value = 0x31; // 1
                        if (Shift == true || Caps == true)
                            T_Value = 0x21; // !
                        break;

                    case KeyCode.Alpha2:
                        if (Shift == false && Caps == false)
                            T_Value = 0x32; // 2
                        if (Shift == true || Caps == true)
                            T_Value = 0x22; // "
                        break;

                    case KeyCode.Alpha3:
                        if (Shift == false && Caps == false)
                            T_Value = 0x33; // 3
                        if (Shift == true || Caps == true)
                            T_Value = 0x23; // #
                        break;

                    case KeyCode.Alpha4:
                        if (Shift == false && Caps == false)
                            T_Value = 0x34; // 4
                        if (Shift == true || Caps == true)
                            T_Value = 0x24; // $
                        break;

                    case KeyCode.Alpha5:
                        if (Shift == false && Caps == false)
                            T_Value = 0x35; // 5
                        if (Shift == true || Caps == true)
                            T_Value = 0x25; // %
                        break;

                    case KeyCode.Alpha6:
                        if (Shift == false && Caps == false)
                            T_Value = 0x36; // 6
                        if (Shift == true || Caps == true)
                            T_Value = 0x26; // &
                        break;

                    case KeyCode.Alpha7:
                        if (Shift == false && Caps == false)
                            T_Value = 0x37; // 7
                        if (Shift == true || Caps == true)
                            T_Value = 0x27; // '
                        if (CTRL == true && Shift == true)
                            T_Value = 0x60; // `
                        break;

                    case KeyCode.Alpha8:
                        if (Shift == false && Caps == false)
                            T_Value = 0x38; // 8
                        if (Shift == true || Caps == true)
                            T_Value = 0x28; // (
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5B; // [
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7B; // {
                        break;

                    case KeyCode.Alpha9:
                        if (Shift == false && Caps == false)
                            T_Value = 0x39; // 9
                        if (Shift == true || Caps == true)
                            T_Value = 0x29; // )
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5D; // ]
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.A:
                        if (Shift == false && Caps == false)
                            T_Value = 0x61; // a
                        if (Shift == true || Caps == true)
                            T_Value = 0x41; // A
                        if (CTRL == true && Shift == false)
                            T_Value = 0x01; // SOH
                        break;

                    case KeyCode.B:
                        if (Shift == false && Caps == false)
                            T_Value = 0x62; // b
                        if (Shift == true || Caps == true)
                            T_Value = 0x42; // B
                        if (CTRL == true && Shift == false)
                            T_Value = 0x02; // STX
                        break;

                    case KeyCode.C:
                        if (Shift == false && Caps == false)
                            T_Value = 0x63; // c
                        if (Shift == true || Caps == true)
                            T_Value = 0x43; // C
                        if (CTRL == true && Shift == false)
                            T_Value = 0x03; // ETX
                        break;

                    case KeyCode.D:
                        if (Shift == false && Caps == false)
                            T_Value = 0x64; // d
                        if (Shift == true || Caps == true)
                            T_Value = 0x44; // D
                        if (CTRL == true && Shift == false)
                            T_Value = 0x04; // EOT
                        break;

                    case KeyCode.E:
                        if (Shift == false && Caps == false)
                            T_Value = 0x65; // e
                        if (Shift == true || Caps == true)
                            T_Value = 0x45; // E
                        if (CTRL == true && Shift == false)
                            T_Value = 0x05; // ENQ
                        break;

                    case KeyCode.F:
                        if (Shift == false && Caps == false)
                            T_Value = 0x66; // f
                        if (Shift == true || Caps == true)
                            T_Value = 0x46; // F
                        if (CTRL == true && Shift == false)
                            T_Value = 0x06; // ACK
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7D; // }
                        break;

                    case KeyCode.G:
                        if (Shift == false && Caps == false)
                            T_Value = 0x67; // g
                        if (Shift == true || Caps == true)
                            T_Value = 0x47; // G
                        if (CTRL == true && Shift == false)
                            T_Value = 0x07; // BEL
                        break;

                    case KeyCode.H:
                        if (Shift == false && Caps == false)
                            T_Value = 0x68; // h
                        if (Shift == true || Caps == true)
                            T_Value = 0x48; // H
                        if (CTRL == true && Shift == false)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.I:
                        if (Shift == false && Caps == false)
                            T_Value = 0x69; // i
                        if (Shift == true || Caps == true)
                            T_Value = 0x49; // I
                        if (CTRL == true && Shift == false)
                            T_Value = 0x09; // HT
                        break;

                    case KeyCode.J:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6A; // j
                        if (Shift == true || Caps == true)
                            T_Value = 0x4A; // J
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0A; // LF
                        break;

                    case KeyCode.K:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6B; // k
                        if (Shift == true || Caps == true)
                            T_Value = 0x4B; // K
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0B; // VT
                        break;

                    case KeyCode.L:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6C; // l
                        if (Shift == true || Caps == true)
                            T_Value = 0x4C; // L
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0C; // FF
                        break;

                    case KeyCode.M:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6D; // m
                        if (Shift == true || Caps == true)
                            T_Value = 0x4D; // M
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.N:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6E; // n
                        if (Shift == true || Caps == true)
                            T_Value = 0x4E; // N
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0E; // SO
                        if (CTRL == true && Shift == true)
                            T_Value = 0x5E; // ^
                        break;

                    case KeyCode.O:
                        if (Shift == false && Caps == false)
                            T_Value = 0x6F; // o
                        if (Shift == true || Caps == true)
                            T_Value = 0x4F; // O
                        if (CTRL == true && Shift == false)
                            T_Value = 0x0F; // SI
                        break;

                    case KeyCode.P:
                        if (Shift == false && Caps == false)
                            T_Value = 0x70; // p
                        if (Shift == true || Caps == true)
                            T_Value = 0x50; // P
                        if (CTRL == true && Shift == false)
                            T_Value = 0x10; // DLE
                        if (CTRL == true && Shift == true)
                            T_Value = 0x40; // @
                        break;

                    case KeyCode.Q:
                        if (Shift == false && Caps == false)
                            T_Value = 0x71; // q
                        if (Shift == true || Caps == true)
                            T_Value = 0x51; // Q
                        if (CTRL == true && Shift == false)
                            T_Value = 0x11; // DC1
                        break;

                    case KeyCode.R:
                        if (Shift == false && Caps == false)
                            T_Value = 0x72; // r
                        if (Shift == true || Caps == true)
                            T_Value = 0x52; // R
                        if (CTRL == true && Shift == false)
                            T_Value = 0x12; // DC2
                        break;

                    case KeyCode.S:
                        if (Shift == false && Caps == false)
                            T_Value = 0x73; // s
                        if (Shift == true || Caps == true)
                            T_Value = 0x53; // S
                        if (CTRL == true && Shift == false)
                            T_Value = 0x13; // DC3
                        break;

                    case KeyCode.T:
                        if (Shift == false && Caps == false)
                            T_Value = 0x74; // t
                        if (Shift == true || Caps == true)
                            T_Value = 0x54; // T
                        if (CTRL == true && Shift == false)
                            T_Value = 0x14; // DC4]
                        break;

                    case KeyCode.U:
                        if (Shift == false && Caps == false)
                            T_Value = 0x75; // u
                        if (Shift == true || Caps == true)
                            T_Value = 0x55; // U
                        if (CTRL == true && Shift == false)
                            T_Value = 0x15; // NAK
                        break;

                    case KeyCode.V:
                        if (Shift == false && Caps == false)
                            T_Value = 0x76; // v
                        if (Shift == true || Caps == true)
                            T_Value = 0x56; // V
                        if (CTRL == true && Shift == false)
                            T_Value = 0x16; // SYN
                        break;

                    case KeyCode.W:
                        if (Shift == false && Caps == false)
                            T_Value = 0x77; // w
                        if (Shift == true || Caps == true)
                            T_Value = 0x57; // W
                        if (CTRL == true && Shift == false)
                            T_Value = 0x17; // ETB
                        break;

                    case KeyCode.X:
                        if (Shift == false && Caps == false)
                            T_Value = 0x78; // x
                        if (Shift == true || Caps == true)
                            T_Value = 0x58; // X
                        if (CTRL == true && Shift == false)
                            T_Value = 0x18; // CAN
                        break;

                    case KeyCode.Y:
                        if (Shift == false && Caps == false)
                            T_Value = 0x79; // y
                        if (Shift == true || Caps == true)
                            T_Value = 0x59; // Y
                        if (CTRL == true && Shift == false)
                            T_Value = 0x19; // EM
                        break;

                    case KeyCode.Z:
                        if (Shift == false && Caps == false)
                            T_Value = 0x7A; // z
                        if (Shift == true || Caps == true)
                            T_Value = 0x5A; // Z
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1A; // SUB
                        break;

                    case KeyCode.Space:
                        if (Shift == false && Caps == false)
                            T_Value = 0x20; // SP
                        break;

                    case KeyCode.Return:
                        if (Shift == false && Caps == false)
                            T_Value = 0x0D; // CR
                        break;

                    case KeyCode.Backspace: // Bonus
                        if (Shift == false && Caps == false)
                            T_Value = 0x5F; // DEL // 7F??? originally
                        if (Shift == true || Caps == true)
                            T_Value = 0x08; // BS
                        break;

                    case KeyCode.Escape:
                        if (Shift == false && Caps == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.Period:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2E; // .
                        if (Shift == true || Caps == true)
                            T_Value = 0x3E; // >
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1E; // RS
                        break;

                    case KeyCode.Comma:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2C; // ,
                        if (Shift == true || Caps == true)
                            T_Value = 0x3C; // <
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1C; // FS
                        break;

                    case KeyCode.Semicolon:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3B; // ;
                        if (Shift == true || Caps == true)
                            T_Value = 0x2B; // +
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1B; // ESC
                        break;

                    case KeyCode.Quote:
                        if (Shift == false && Caps == false)
                            T_Value = 0x3A; // :
                        if (Shift == true || Caps == true)
                            T_Value = 0x2A; // *
                        if (CTRL == true && Shift == false)
                            T_Value = 0x1A; // SUB
                        break;

                    case KeyCode.Minus:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2D; // -
                        if (Shift == true || Caps == true)
                            T_Value = 0x2A; // *
                        if (CTRL == true && Shift == false)
                            T_Value = 0x3D; // =
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7E; // ~
                        break;

                    case KeyCode.Slash:
                        if (Shift == false && Caps == false)
                            T_Value = 0x2F; // SLASH
                        if (Shift == true || Caps == true)
                            T_Value = 0x3F; // ?
                        if (CTRL == true && Shift == false)
                            T_Value = 0x5C; // BACKSLASH
                        if (CTRL == true && Shift == true)
                            T_Value = 0x7C; // |

                        break;

                    case KeyCode.Mouse0:
                    case KeyCode.Mouse1:
                    case KeyCode.Mouse2:
                        break;

                    default:
                        break;
                }
                if (DebugKBD == true)
                    Debug.Log("[Keyboard] KeyCode: " + kcode);
            }
        }

        byte O_Value = T_Value;

        // Only Capital Letters
        if (T_Value >= 0x61 && T_Value <= 0x7A && CFG_CapitalOnly == true)
            T_Value -= 0x20;

        T_Value = (byte)(T_Value | (1 << 7));

        if (O_Value != 0xFF)
        {
            //Debug.Log("KeyCode down: " + kcode + "; Value sent: 0x" + T_Value.ToString("X2"));
            PIA.KBD = T_Value;
            PIA.KBDCR = SetFlag(PIA.KBDCR, 7, true);
        }
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

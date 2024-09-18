using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Keyboard_ASCII_Auto : MonoBehaviour
{
    [Header("Connections")]
    public PIA_Simple PIA;
    

    [Header("Storage")]
    public List<string> AutoText = new List<string>();
    public bool ProcessText = false;
    
    [Header("Settings")]
    public float WaitTime = 0.5f;
    public float WaitTimeReturn = 1f;
    private bool AutoToggle = false;
    private bool NewLineToggle = false;

    public void Update()
    {
        if (ProcessText == true && AutoToggle == false)
        {
            AutoToggle = true;
            StartCoroutine(KeyProcess());
        }
    }

    public void ImportFile(string A_Path)
    {
        string[] T_Lines = File.ReadAllLines(A_Path);
        AutoText = new List<string>(T_Lines);
    }

    public IEnumerator KeyProcess()
    {
        if (NewLineToggle == true)
        {
            NewLineToggle = false;
            yield return new WaitForSeconds(WaitTimeReturn);
        }
        else
        {
            yield return new WaitForSeconds(WaitTime);
        }

        if (AutoText.Count == 0)
        {
            ProcessText = false;
            AutoToggle = false;
        }
        else
        {
            if (AutoText[0] == "")
            {
                AutoText.RemoveAt(0);
                ProcessKey("RET");
                NewLineToggle = true;
            }
            else if (AutoText[0] != "")
            {
                string T_Key = AutoText[0].Substring(0, 1);
                AutoText[0] = AutoText[0].Substring(1);
                ProcessKey(T_Key);
            }
        }

        AutoToggle = false;
    }

    public void ProcessKey(string A_Key)
    {
        byte T_Value = 0xFF;

        switch (A_Key)
        {
            case "BACKSPACE":
                T_Value = 0x5F;
                break;

            case "DEL":
                T_Value = 0x7F;
                break;

            case "NUL":
                T_Value = 0x00;
                break;

            case "SOH":
                T_Value = 0x01;
                break;

            case "STX":
                T_Value = 0x02;
                break;

            case "ETX":
                T_Value = 0x03;
                break;

            case "EOT":
                T_Value = 0x04;
                break;

            case "ENQ":
                T_Value = 0x05;
                break;

            case "ACK":
                T_Value = 0x06;
                break;

            case "BEL":
                T_Value = 0x07;
                break;

            case "BS":
                T_Value = 0x08;
                break;

            case "HT":
                T_Value = 0x09;
                break;

            case "LF":
                T_Value = 0x0A;
                break;

            case "VT":
                T_Value = 0x0B;
                break;

            case "FF":
                T_Value = 0x0C;
                break;

            case "CR":
                T_Value = 0x0D;
                break;

            case "SO":
                T_Value = 0x0E;
                break;

            case "SI":
                T_Value = 0x0F;
                break;

            case "DLE":
                T_Value = 0x10;
                break;

            case "DC1":
                T_Value = 0x11;
                break;

            case "DC2":
                T_Value = 0x12;
                break;

            case "DC3":
                T_Value = 0x13;
                break;

            case "DC4":
                T_Value = 0x14;
                break;

            case "NAK":
                T_Value = 0x15;
                break;

            case "SYN":
                T_Value = 0x16;
                break;

            case "ETB":
                T_Value = 0x17;
                break;

            case "CAN":
                T_Value = 0x18;
                break;

            case "EM":
                T_Value = 0x19;
                break;

            case "SUB":
                T_Value = 0x1A;
                break;

            case "ESC":
                T_Value = 0x1B;
                break;

            case "FS":
                T_Value = 0x1C;
                break;

            case "GS":
                T_Value = 0x1D;
                break;

            case "RS":
                T_Value = 0x1E;
                break;

            case "US":
                T_Value = 0x1F;
                break;

            case "RET":
                T_Value = 0x0D; // CR
                break;

            case " ":
                T_Value = 0x20; // SPACE
                break;

            case "!":
                T_Value = 0x21; // !
                break;

            case "\"":
                T_Value = 0x22; // "
                break;

            case "#":
                T_Value = 0x23; // #
                break;

            case "$":
                T_Value = 0x24; // $
                break;

            case "%":
                T_Value = 0x25; // %
                break;

            case "&":
                T_Value = 0x26; // &
                break;

            case "'":
                T_Value = 0x27; // '
                break;

            case "(":
                T_Value = 0x28; // (
                break;

            case ")":
                T_Value = 0x29; // )
                break;

            case "*":
                T_Value = 0x2A; // *
                break;

            case "+":
                T_Value = 0x2B; // +
                break;

            case ",":
                T_Value = 0x2C; // ,
                break;

            case "-":
                T_Value = 0x2D; // -
                break;

            case ".":
                T_Value = 0x2E; // .
                break;

            case "/":
                T_Value = 0x2F; // /
                break;

            case "0":
                T_Value = 0x30; // 0
                break;

            case "1":
                T_Value = 0x31; // 1
                break;

            case "2":
                T_Value = 0x32; // 2
                break;

            case "3":
                T_Value = 0x33; // 3
                break;

            case "4":
                T_Value = 0x34; // 4
                break;

            case "5":
                T_Value = 0x35; // 5
                break;

            case "6":
                T_Value = 0x36; // 6
                break;

            case "7":
                T_Value = 0x37; // 7
                break;

            case "8":
                T_Value = 0x38; // 8
                break;

            case "9":
                T_Value = 0x39; // 9
                break;

            case ":":
                T_Value = 0x3A; // :
                break;

            case ";":
                T_Value = 0x3B; // ;
                break;

            case "<":
                T_Value = 0x3C; // <
                break;

            case "=":
                T_Value = 0x3D; // =
                break;

            case ">":
                T_Value = 0x3E; // >
                break;

            case "?":
                T_Value = 0x3F; // ?
                break;

            case "@":
                T_Value = 0x40; // @
                break;

            case "A":
                T_Value = 0x41; // A
                break;

            case "B":
                T_Value = 0x42; // B
                break;

            case "C":
                T_Value = 0x43; // C
                break;

            case "D":
                T_Value = 0x44; // D
                break;

            case "E":
                T_Value = 0x45; // E
                break;

            case "F":
                T_Value = 0x46; // F
                break;

            case "G":
                T_Value = 0x47; // G
                break;

            case "H":
                T_Value = 0x48; // H
                break;

            case "I":
                T_Value = 0x49; // I
                break;

            case "J":
                T_Value = 0x4A; // J
                break;

            case "K":
                T_Value = 0x4B; // K
                break;

            case "L":
                T_Value = 0x4C; // L
                break;

            case "M":
                T_Value = 0x4D; // M
                break;

            case "N":
                T_Value = 0x4E; // n
                break;

            case "O":
                T_Value = 0x4F; // O
                break;

            case "P":
                T_Value = 0x50; // P
                break;

            case "Q":
                T_Value = 0x51; // Q
                break;

            case "R":
                T_Value = 0x52; // R
                break;

            case "S":
                T_Value = 0x53; // S
                break;

            case "T":
                T_Value = 0x54; // T
                break;

            case "U":
                T_Value = 0x55; // U
                break;

            case "V":
                T_Value = 0x56; // V
                break;

            case "W":
                T_Value = 0x57; // W
                break;

            case "X":
                T_Value = 0x58; // X
                break;

            case "Y":
                T_Value = 0x59; // Y
                break;

            case "Z":
                T_Value = 0x5A; // Z
                break;

            case "[":
                T_Value = 0x5B; // [
                break;

            case "\\":
                T_Value = 0x5C; // \
                break;

            case "]":
                T_Value = 0x5D; // ]
                break;

            case "^":
                T_Value = 0x5E; // ^
                break;

            case "_":
                T_Value = 0x5F; // _
                break;

            case "`":
                T_Value = 0x60; // `
                break;

            case "{":
                T_Value = 0x7B; // {
                break;

            case "|":
                T_Value = 0x7C; // |
                break;

            case "}":
                T_Value = 0x7D; // }
                break;

            case "~":
                T_Value = 0x7E; // ~
                break;
        }

        if (T_Value == 0xFF)
            Debug.Log("[AUTOKEY] Warning! Unhandled Char: " + A_Key);
        T_Value = (byte)(T_Value | (1 << 7));
        PIA.KBD = T_Value;
        PIA.KBDCR = SetFlag(PIA.KBDCR, 7, true);
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

using UnityEngine;
using UnityEngine.UI;

public class GUI_Apple1_OSK : MonoBehaviour
{
    [Header("Connections")]
    public Keyboard_ASCII_Auto Keyboard;
    public Image IMG_Shift;
    public Image IMG_CTRL;

    [Header("Status")]
    public bool ShiftKey = false;
    public bool CTRLKey = false;

    private Color COL_Inactive = new Color(0, 1, 0, 0f);
    private Color COL_Active = new Color(0, 1, 0, 0.5f);

    public void Key(string A_Key)
    {
        switch(A_Key)
        {
            case "SHIFT":
                if (ShiftKey == false)
                {
                    ShiftKey = true;
                    IMG_Shift.color = COL_Active;
                }
                else
                {
                    ShiftKey = false;
                    IMG_Shift.color = COL_Inactive;
                }
                break;

            case "CTRL":
                if (CTRLKey == false)
                {
                    CTRLKey = true;
                    IMG_CTRL.color = COL_Active;
                }
                else
                {
                    CTRLKey = false;
                    IMG_CTRL.color = COL_Inactive;
                }
                break;

            case "":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("");
                break;

            case "DEL":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("BACKSPACE");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("DEL");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("US");
                break;

            case "1":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("1");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("!");
                break;

            case "2":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("2");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("\"");
                break;

            case "3":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("3");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("#");
                break;

            case "4":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("4");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("$");
                break;

            case "5":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("5");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("%");
                break;

            case "6":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("6");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("&");
                break;

            case "7":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("7");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("`");
                break;

            case "8":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("8");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("(");
                break;

            case "9":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("9");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey(")");
                break;

            case "0":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("0");
                break;

            case "RS":
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("~");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("RS");
                break;

            case "MINUS":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("-");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("=");
                break;

            case "ESC":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("[");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("(");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("ESC");
                break;

            case "Q":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("Q");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("DC1");
                break;

            case "W":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("W");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("ETB");
                break;

            case "E":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("E");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("ENQ");
                break;

            case "R":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("R");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("DC2");
                break;

            case "T":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("T");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("DC4");
                break;

            case "Y":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("Y");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("EM");
                break;

            case "U":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("U");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("NAK");
                break;

            case "I":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("I");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("HT");
                break;

            case "O":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("O");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("SI");
                break;

            case "P":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("P");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("DLE");
                break;

            case "NUL":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("@");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey("`");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("NUL");
                break;

            case "GS":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("]");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey(")");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("GS");
                break;

            case "A":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("A");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("SOH");
                break;

            case "S":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("S");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("DC3");
                break;

            case "D":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("D");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("EOT");
                break;

            case "F":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("F");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("ACK");
                break;

            case "G":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("G");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("BEL");
                break;

            case "H":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("H");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("BS");
                break;

            case "J":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("J");
                break;

            case "K":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("K");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("VT");
                break;

            case "L":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("L");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("FF");
                break;

            case "SEMI":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey(";");
                if (ShiftKey == true || CTRLKey == true)
                    Keyboard.ProcessKey("+");
                break;

            case "DP":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey(":");
                if (ShiftKey == true || CTRLKey == true)
                    Keyboard.ProcessKey("*");
                break;

            case "BS":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("\\");
                if (ShiftKey == true && CTRLKey == false)
                    Keyboard.ProcessKey(":");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("FS");
                break;

            case "Z":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("Z");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("SUB");
                break;

            case "X":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("X");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("CAN");
                break;

            case "C":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("C");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("ETX");
                break;

            case "V":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("V");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("SYN");
                break;

            case "B":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("B");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("STX");
                break;

            case "N":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("N");
                if (ShiftKey == false && CTRLKey == true)
                    Keyboard.ProcessKey("SO");
                break;

            case "M":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("M");
                break;

            case "COMMA":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey(",");
                if (ShiftKey == true || CTRLKey == true)
                    Keyboard.ProcessKey("<");
                break;

            case "PERIOD":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey(".");
                if (ShiftKey == true || CTRLKey == true)
                    Keyboard.ProcessKey(">");
                break;

            case "SLASH":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("/");
                if (ShiftKey == true || CTRLKey == true)
                    Keyboard.ProcessKey("?");
                break;

            case "LF":
                if (ShiftKey == false && CTRLKey == false)
                    Keyboard.ProcessKey("LF");
                break;

            case "SPACE":
                Keyboard.ProcessKey(" ");
                break;

            case "CR":
                Keyboard.ProcessKey("CR");
                break;
        }
    }
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUI_Apple1_Diagnostics : MonoBehaviour
{
    [Header("Connections")]
    public SYS_Apple1 SYS;

    [Header("CPU")]
    public TMP_Text TXT_PC;
    public TMP_Text TXT_SP;
    public TMP_Text TXT_A;
    public TMP_Text TXT_X;
    public TMP_Text TXT_Y;
    public TMP_Text TXT_OPC;
    public TMP_Text TXT_Step;
    public Image IMG_FlagN;
    public Image IMG_FlagV;
    public Image IMG_FlagX;
    public Image IMG_FlagB;
    public Image IMG_FlagD;
    public Image IMG_FlagI;
    public Image IMG_FlagZ;
    public Image IMG_FlagC;

    [Header("Map")]
    public string M_Category = "Stack";
    public int M_Page = 0;

    public TMP_Text M_Title;

    public TMP_Text A19;
    public TMP_Text A18;
    public TMP_Text A17;
    public TMP_Text A16;
    public TMP_Text A15;
    public TMP_Text A14;
    public TMP_Text A13;
    public TMP_Text A12;
    public TMP_Text A11;
    public TMP_Text A10;
    public TMP_Text A09;
    public TMP_Text A08;
    public TMP_Text A07;
    public TMP_Text A06;
    public TMP_Text A05;
    public TMP_Text A04;
    public TMP_Text A03;
    public TMP_Text A02;
    public TMP_Text A01;
    public TMP_Text A00;

    public TMP_Text V19;
    public TMP_Text V18;
    public TMP_Text V17;
    public TMP_Text V16;
    public TMP_Text V15;
    public TMP_Text V14;
    public TMP_Text V13;
    public TMP_Text V12;
    public TMP_Text V11;
    public TMP_Text V10;
    public TMP_Text V09;
    public TMP_Text V08;
    public TMP_Text V07;
    public TMP_Text V06;
    public TMP_Text V05;
    public TMP_Text V04;
    public TMP_Text V03;
    public TMP_Text V02;
    public TMP_Text V01;
    public TMP_Text V00;

    public Button BTN_P20;
    public Button BTN_P200;
    public Button BTN_M20;
    public Button BTN_M200;

    public void Init()
    {
        M_Category = "Stack";
        M_Page = 12;
        CheckPage();
    }

    public void Tick()
    {
        UpdateCPU();
        UpdateMap();
    }

    private void UpdateCPU()
    {
        TXT_PC.text = SYS.CPU.PC.ToString("X2");
        TXT_SP.text = SYS.CPU.SP.ToString("X2");
        TXT_A.text = SYS.CPU.A.ToString("X2");
        TXT_X.text = SYS.CPU.X.ToString("X2");
        TXT_Y.text = SYS.CPU.Y.ToString("X2");
        TXT_OPC.text = SYS.CPU.OPC.ToString("X2");
        TXT_Step.text = SYS.CPU.CPUStep.ToString();

        if (SYS.CPU.ReadFlag(7) == true)
            IMG_FlagN.color = Color.green;
        else
            IMG_FlagN.color = Color.grey;

        if (SYS.CPU.ReadFlag(6) == true)
            IMG_FlagV.color = Color.green;
        else
            IMG_FlagV.color = Color.grey;

        if (SYS.CPU.ReadFlag(5) == true)
            IMG_FlagX.color = Color.green;
        else
            IMG_FlagX.color = Color.grey;

        if (SYS.CPU.ReadFlag(4) == true)
            IMG_FlagB.color = Color.green;
        else
            IMG_FlagB.color = Color.grey;

        if (SYS.CPU.ReadFlag(3) == true)
            IMG_FlagD.color = Color.green;
        else
            IMG_FlagD.color = Color.grey;

        if (SYS.CPU.ReadFlag(2) == true)
            IMG_FlagI.color = Color.green;
        else
            IMG_FlagI.color = Color.grey;

        if (SYS.CPU.ReadFlag(1) == true)
            IMG_FlagZ.color = Color.green;
        else
            IMG_FlagZ.color = Color.grey;

        if (SYS.CPU.ReadFlag(0) == true)
            IMG_FlagC.color = Color.green;
        else
            IMG_FlagC.color = Color.grey;
    }

    public void CPUControls(int A_Option)
    {
        switch (A_Option)
        {
            case 0:
                SYS.Run = true;
                break;

            case 1:
                SYS.Run = false;
                break;

            case 2:
                SYS.SendMessage("Heartbeat");
                break;

            case 3:
                for (int i = 0; i < 10; i++)
                    SYS.SendMessage("Heartbeat");
                break;

            case 4:
                SYS.Reset();
                break;
        }
    }

    public void MapControls(string A_Option)
    {
        switch (A_Option)
        {
            case "Stack":
                M_Category = "Stack";
                M_Page = 13;
                M_Title.text = "STACK";
                break;

            case "Memory":
                M_Category = "Memory";
                M_Page = 0;
                M_Title.text = "MEMORY";
                break;

            case "OPC":
                M_Category = "OPC";
                M_Page = 49;
                M_Title.text = "OPCODE HISTORY";
                break;

            case "P20":
                M_Page += 1;
                break;

            case "P200":
                M_Page += 10;
                break;

            case "M20":
                M_Page -= 1;
                break;

            case "M200":
                M_Page -= 10;
                break;
        }

        CheckPage();
        UpdateMap();
    }

    private void CheckPage()
    {
        switch(M_Category)
        {
            case "Stack":
                if (M_Page > 12)
                    M_Page = 12;

                if (M_Page == 12)
                {
                    BTN_P20.gameObject.SetActive(false);
                    BTN_P200.gameObject.SetActive(false);
                }
                else
                {
                    BTN_P20.gameObject.SetActive(true);
                    BTN_P200.gameObject.SetActive(true);
                }
                break;

            case "Memory":
                if (M_Page > SYS.RAM.ADR_End / 20)
                    M_Page = SYS.RAM.ADR_End / 20;

                if (M_Page == (int)(SYS.RAM.ADR_End / 20))
                {
                    BTN_P20.gameObject.SetActive(false);
                    BTN_P200.gameObject.SetActive(false);
                }
                else
                {
                    BTN_P20.gameObject.SetActive(true);
                    BTN_P200.gameObject.SetActive(true);
                }
                break;

            case "OPC":
                if (M_Page > 49)
                    M_Page = 49;

                if (M_Page == 49)
                {
                    BTN_P20.gameObject.SetActive(false);
                    BTN_P200.gameObject.SetActive(false);
                }
                else
                {
                    BTN_P20.gameObject.SetActive(true);
                    BTN_P200.gameObject.SetActive(true);
                }
                break;

        }

        if (M_Page < 0)
            M_Page = 0;

        if (M_Page == 0)
        {
            BTN_M20.gameObject.SetActive(false);
            BTN_M200.gameObject.SetActive(false);
        }
        else
        {
            BTN_M20.gameObject.SetActive(true);
            BTN_M200.gameObject.SetActive(true);
        }
    }

    private void UpdateMap()
    {
        ushort T_Offset = 0x0000;
        ushort T_Max = 0xFFFF;

        switch(M_Category)
        {
            case "Stack":
                T_Offset = 0x0100;
                T_Max = 0x01FF;
                break;

            case "Memory":
                T_Offset = 0x0000;
                T_Max = (ushort)SYS.RAM.ADR_End;
                break;

            case "OPC":
                T_Offset = 0;
                T_Max = (ushort) SYS.CPU.History.Count;
                break;
        }

        if (M_Category == "Stack" || M_Category == "Memory")
        {
            if (T_Offset + (M_Page * 20) + 19 > T_Max)
            {
                A19.text = "";
                V19.text = "";
            }
            else
            {
                A19.text = "0x" + (T_Offset + (M_Page * 20) + 19).ToString("X4");
                V19.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 19)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 18 > T_Max)
            {
                A18.text = "";
                V18.text = "";
            }
            else
            {
                A18.text = "0x" + (T_Offset + (M_Page * 20) + 18).ToString("X4");
                V18.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 18)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 17 > T_Max)
            {
                A17.text = "";
                V17.text = "";
            }
            else
            {
                A17.text = "0x" + (T_Offset + (M_Page * 20) + 17).ToString("X4");
                V17.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 17)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 16 > T_Max)
            {
                A16.text = "";
                V16.text = "";
            }
            else
            {
                A16.text = "0x" + (T_Offset + (M_Page * 20) + 16).ToString("X4");
                V16.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 16)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 15 > T_Max)
            {
                A15.text = "";
                V15.text = "";
            }
            else
            {
                A15.text = "0x" + (T_Offset + (M_Page * 20) + 15).ToString("X4");
                V15.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 15)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 14 > T_Max)
            {
                A14.text = "";
                V14.text = "";
            }
            else
            {
                A14.text = "0x" + (T_Offset + (M_Page * 20) + 14).ToString("X4");
                V14.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 14)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 13 > T_Max)
            {
                A13.text = "";
                V13.text = "";
            }
            else
            {
                A13.text = "0x" + (T_Offset + (M_Page * 20) + 13).ToString("X4");
                V13.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 13)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 12 > T_Max)
            {
                A12.text = "";
                V12.text = "";
            }
            else
            {
                A12.text = "0x" + (T_Offset + (M_Page * 20) + 12).ToString("X4");
                V12.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 12)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 11 > T_Max)
            {
                A11.text = "";
                V11.text = "";
            }
            else
            {
                A11.text = "0x" + (T_Offset + (M_Page * 20) + 11).ToString("X4");
                V11.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 11)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 10 > T_Max)
            {
                A10.text = "";
                V10.text = "";
            }
            else
            {
                A10.text = "0x" + (T_Offset + (M_Page * 20) + 10).ToString("X4");
                V10.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 10)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 09 > T_Max)
            {
                A09.text = "";
                V09.text = "";
            }
            else
            {
                A09.text = "0x" + (T_Offset + (M_Page * 20) + 09).ToString("X4");
                V09.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 09)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 08 > T_Max)
            {
                A08.text = "";
                V08.text = "";
            }
            else
            {
                A08.text = "0x" + (T_Offset + (M_Page * 20) + 08).ToString("X4");
                V08.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 08)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 07 > T_Max)
            {
                A07.text = "";
                V07.text = "";
            }
            else
            {
                A07.text = "0x" + (T_Offset + (M_Page * 20) + 07).ToString("X4");
                V07.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 07)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 06 > T_Max)
            {
                A06.text = "";
                V06.text = "";
            }
            else
            {
                A06.text = "0x" + (T_Offset + (M_Page * 20) + 06).ToString("X4");
                V06.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 06)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 05 > T_Max)
            {
                A05.text = "";
                V05.text = "";
            }
            else
            {
                A05.text = "0x" + (T_Offset + (M_Page * 20) + 05).ToString("X4");
                V05.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 05)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 04 > T_Max)
            {
                A04.text = "";
                V04.text = "";
            }
            else
            {
                A04.text = "0x" + (T_Offset + (M_Page * 20) + 04).ToString("X4");
                V04.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 04)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 03 > T_Max)
            {
                A03.text = "";
                V03.text = "";
            }
            else
            {
                A03.text = "0x" + (T_Offset + (M_Page * 20) + 03).ToString("X4");
                V03.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 03)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 02 > T_Max)
            {
                A02.text = "";
                V02.text = "";
            }
            else
            {
                A02.text = "0x" + (T_Offset + (M_Page * 20) + 02).ToString("X4");
                V02.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 02)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 01 > T_Max)
            {
                A01.text = "";
                V01.text = "";
            }
            else
            {
                A01.text = "0x" + (T_Offset + (M_Page * 20) + 01).ToString("X4");
                V01.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 01)].ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 00 > T_Max)
            {
                A00.text = "";
                V00.text = "";
            }
            else
            {
                A00.text = "0x" + (T_Offset + (M_Page * 20) + 00).ToString("X4");
                V00.text = "0x" + SYS.RAM.Memory[(T_Offset + (M_Page * 20) + 00)].ToString("X2");
            }
        }
        else
        {
            if (T_Offset + (M_Page * 20) + 19 >= T_Max)
            {
                A19.text = "";
                V19.text = "";
            }
            else
            {
                A19.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 19)].PC.ToString("X4");
                V19.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 19)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 18 >= T_Max)
            {
                A18.text = "";
                V18.text = "";
            }
            else
            {
                A18.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 18)].PC.ToString("X4");
                V18.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 18)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 17 >= T_Max)
            {
                A17.text = "";
                V17.text = "";
            }
            else
            {
                A17.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 17)].PC.ToString("X4");
                V17.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 17)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 16 >= T_Max)
            {
                A16.text = "";
                V16.text = "";
            }
            else
            {
                A16.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 16)].PC.ToString("X4");
                V16.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 16)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 15 >= T_Max)
            {
                A15.text = "";
                V15.text = "";
            }
            else
            {
                A15.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 15)].PC.ToString("X4");
                V15.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 15)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 14 >= T_Max)
            {
                A14.text = "";
                V14.text = "";
            }
            else
            {
                A14.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 14)].PC.ToString("X4");
                V14.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 14)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 13 >= T_Max)
            {
                A13.text = "";
                V13.text = "";
            }
            else
            {
                A13.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 13)].PC.ToString("X4");
                V13.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 13)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 12 >= T_Max)
            {
                A12.text = "";
                V12.text = "";
            }
            else
            {
                A12.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 12)].PC.ToString("X4");
                V12.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 12)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 11 >= T_Max)
            {
                A11.text = "";
                V11.text = "";
            }
            else
            {
                A11.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 11)].PC.ToString("X4");
                V11.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 11)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 10 >= T_Max)
            {
                A10.text = "";
                V10.text = "";
            }
            else
            {
                A10.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 10)].PC.ToString("X4");
                V10.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 10)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 09 >= T_Max)
            {
                A09.text = "";
                V09.text = "";
            }
            else
            {
                A09.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 09)].PC.ToString("X4");
                V09.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 09)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 08 >= T_Max)
            {
                A08.text = "";
                V08.text = "";
            }
            else
            {
                A08.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 08)].PC.ToString("X4");
                V08.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 08)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 07 >= T_Max)
            {
                A07.text = "";
                V07.text = "";
            }
            else
            {
                A07.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 07)].PC.ToString("X4");
                V07.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 07)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 06 >= T_Max)
            {
                A06.text = "";
                V06.text = "";
            }
            else
            {
                A06.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 06)].PC.ToString("X4");
                V06.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 06)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 05 >= T_Max)
            {
                A05.text = "";
                V05.text = "";
            }
            else
            {
                A05.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 05)].PC.ToString("X4");
                V05.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 05)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 04 >= T_Max)
            {
                A04.text = "";
                V04.text = "";
            }
            else
            {
                A04.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 04)].PC.ToString("X4");
                V04.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 04)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 03 >= T_Max)
            {
                A03.text = "";
                V03.text = "";
            }
            else
            {
                A03.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 03)].PC.ToString("X4");
                V03.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 03)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 02 >= T_Max)
            {
                A02.text = "";
                V02.text = "";
            }
            else
            {
                A02.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 02)].PC.ToString("X4");
                V02.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 02)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 01 >= T_Max)
            {
                A01.text = "";
                V01.text = "";
            }
            else
            {
                A01.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 01)].PC.ToString("X4");
                V01.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 01)].OPC.ToString("X2");
            }

            if (T_Offset + (M_Page * 20) + 00 >= T_Max)
            {
                A00.text = "";
                V00.text = "";
            }
            else
            {
                A00.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 00)].PC.ToString("X4");
                V00.text = "0x" + SYS.CPU.History[(T_Offset + (M_Page * 20) + 00)].OPC.ToString("X2");
            }
        }
    }
        
}

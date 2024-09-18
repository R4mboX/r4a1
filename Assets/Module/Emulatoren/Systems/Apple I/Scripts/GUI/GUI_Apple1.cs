using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_Apple1 : MonoBehaviour
{
    [Header("Connections")]
    public SYS_Apple1 SYS;
    public List<GameObject> Pages = new List<GameObject>();
    public GameObject PG_Fullscreen;
    public GUI_Apple1_Diagnostics Diagnostics;
    public GUI_Apple1_Settings Settings;
    public GUI_Apple1_CartSelection CartSelection;
    public GUI_Apple1_CartEditor CartEditor;
    public CartridgeDrive_Generic CartDrive;
    public GUI_Apple1_RomSelection ROMSelection;
    public GUI_Apple1_Network Network;
    public GUI_Apple1_Guide Guide;
    public int LastOpened = 0;

    [Header("Card Preview")]
    public TMP_Text TXT_CardLabel;

    [Header("Menu")]
    public TMP_Text TXT_Info;

    public void Tick()
    {
        Hotkeys();

        if (LastOpened == 2)
            Diagnostics.Tick();

        if (LastOpened == 8)
            Network.Tick();
    }

    public void Hotkeys()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OpenPage(0);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            OpenPage(2);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (Pages[1].activeSelf == true)
                OpenPage(0);
            else
                OpenPage(1);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            OpenPage(6);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            OpenPage(7);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            OpenPage(9);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            OpenPage(3);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            OpenPage(8);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            OpenPage(4);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            SYS.AutoKeyboard.ImportFile(Application.dataPath + "/Files/Apple I/AutoText.txt");
            SYS.AutoKeyboard.ProcessText = true;
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SYS.RAM.Dump(Application.dataPath + "/Files/Apple I/HEXDump.txt", 0x0000, 0x9FFF);
        }  

        if (Input.GetKeyDown(KeyCode.F12))
        {
            Application.Quit();
        }

        /*
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SYS.Reset();
        }
        */
    }

    public void OpenPage(int A_PageID)
    {
        foreach (GameObject T_Page in Pages)
            T_Page.SetActive(false);

        SYS.PIA.Keyboard.KBDActive = false;

        Pages[A_PageID].SetActive(true);

        if (A_PageID == 0)
            SYS.PIA.Keyboard.KBDActive = true;

        if (A_PageID == 7)
            SYS.PIA.Keyboard.KBDActive = true;

        if (A_PageID == 1)
        {
            PG_Fullscreen.SetActive(true);
            SYS.PIA.Keyboard.KBDActive = true;
        }
        else
        {
            PG_Fullscreen.SetActive(false);
        }

        if (A_PageID == 2)
            SYS.PIA.Keyboard.KBDActive = true;

        if (A_PageID == 3)
        {
            Settings.Init();
        }

        if (A_PageID == 4)
        {
            if (LastOpened == 4)
            {
                OpenPage(0);
                return;
            }
            else
                CartSelection.Init();
        }

        if (A_PageID == 5)
        {
            CartEditor.Init();
        }

        if (A_PageID == 6)
            ROMSelection.Init();

        if (A_PageID == 9)
        {
            Guide.Init();
        }

        TXT_Info.text = "";
        LastOpened = A_PageID;
    }

    public void CTRL_Menu(string A_Option)
    {
        if (A_Option == "Exit")
            Application.Quit();
    }

    public void Init()
    {
        TXT_Info.text = "";
        Diagnostics.Init();

        OpenPage(0);
        UpdateCart();
    }

    public void UpdateCart()
    {
        if (CartDrive.Cartridge != null)
            TXT_CardLabel.text = CartDrive.Cartridge.C_Title;
        else
            TXT_CardLabel.text = "None";
    }

    public void ShowInfo(string A_Info)
    {
        switch (A_Info)
        {
            case "Terminal":
                TXT_Info.text = "TERMINAL (F1)";
                break;

            case "Diagnostics":
                TXT_Info.text = "DIAGNOSTICS (F2)";
                break;

            case "Fullscreen":
                TXT_Info.text = "FULLSCREEN (F3)";
                break;

            case "Roms":
                TXT_Info.text = "ROMS (F4)";
                break;

            case "OSK":
                TXT_Info.text = "KEYBOARD (F5)";
                break;

            case "Guide":
                TXT_Info.text = "GUIDE (F6)";
                break;

            case "Settings":
                TXT_Info.text = "SETTINGS (F7)";
                break;

            case "Network":
                TXT_Info.text = "NETWORK (F8)";
                break;

            case "Carts":
                TXT_Info.text = "CARTRIDGES (F9)";
                break;    

            case "Exit":
                TXT_Info.text = "EXIT (F12)";
                break;
        
            case "Clear":
                TXT_Info.text = "";
                break;
        }
    }
}

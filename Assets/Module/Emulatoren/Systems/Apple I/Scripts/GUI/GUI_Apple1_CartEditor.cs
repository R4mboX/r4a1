using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Apple1_CartEditor : MonoBehaviour
{
    [Header("Storage")]
    public Cartridge_Generic Cart;
    public bool EditMode = false;
    public string OldName = "";

    [Header("Connections")]
    public GUI_Apple1 GUI;
    public CartridgeDrive_Generic CDrive;

    [Header("GUI Elements")]
    public TMP_Text TXT_Label;
    public TMP_InputField IN_Title;
    public TMP_InputField IN_EntryPoint;
    public Image BTN_WriteProtection_On;
    public Image BTN_WriteProtection_Off;
    public Image BTN_AutoSave_On;
    public Image BTN_AutoSave_Off;
    public TMP_Text TXT_Info;
    public GameObject BTN_Standard;

    public void Init()
    {
        TXT_Info.text = "";
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        TXT_Label.text = Cart.C_Title;
        IN_Title.text = Cart.C_Title;
        IN_EntryPoint.text = Cart.C_EntryPoint.ToString("X4");
        if (Cart.WriteProtected == true)
        {
            BTN_WriteProtection_On.color = Color.green;
            BTN_WriteProtection_Off.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            BTN_WriteProtection_On.color = new Color(0.2f, 0.2f, 0.2f);
            BTN_WriteProtection_Off.color = Color.green;
        }
        if (Cart.AutoSave == true)
        {
            BTN_AutoSave_On.color = Color.green;
            BTN_AutoSave_Off.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            BTN_AutoSave_On.color = new Color(0.2f, 0.2f, 0.2f);
            BTN_AutoSave_Off.color = Color.green;
        }
        if (Cart.C_Title == GUI.SYS.Settings.Cart_Standard)
            BTN_Standard.SetActive(false);
        else
            BTN_Standard.SetActive(true);
    }

    public void Hover(string A_Option)
    {
        switch(A_Option)
        {
            case "Title":
                TXT_Info.text = "This will also be the Filename";
                break;

            case "Entry":
                TXT_Info.text = "For Autorun. Between 0x0000 and 0x1FFF. Will be mapped to 0xA...";
                break;

            case "WriteProtection":
                TXT_Info.text = "Cart is write-protected if ON";
                break;

            case "AutoSave":
                TXT_Info.text = "If Active will save Cart every 5 min and on Cart Change / Exit";
                break;

            case "Import":
                TXT_Info.text = "Import Data from Files/Apple I/import.bytes | Needs to be pure Bytes";
                break;

            case "Export":
                TXT_Info.text = "Export Data to Files/Apple I/export.bytes | Needs to be pure Bytes";
                break;

            case "Format":
                TXT_Info.text = "Delete all Content";
                break;

            case "Standard":
                TXT_Info.text = "Your standard Cart gets automatically inserted on Startup";
                break;

            case "Off":
                TXT_Info.text = "";
                break;
        }
    }

    public void Controls(string A_Option)
    {
        switch(A_Option)
        {
            case "UpdateLabel":
                TXT_Label.text = IN_Title.text;
                Cart.C_Title = IN_Title.text;
                break;

            case "UpdateEntry":
                Cart.C_EntryPoint = Convert.ToUInt16(IN_EntryPoint.text, 16);
                break;

            case "WPOn":
                Cart.WriteProtected = true;
                Cart.AutoSave = true;
                UpdateGUI();
                break;

            case "WPOff":
                Cart.WriteProtected = false;
                UpdateGUI();
                break;

            case "ASOn":
                Cart.AutoSave = true;
                Cart.WriteProtected = false;
                UpdateGUI();
                break;

            case "ASOff":
                Cart.AutoSave = false;
                UpdateGUI();
                break;

            case "Import":
                string T_Path = Application.dataPath + "/Files/Apple I/import.bytes";
                try
                {
                    Cart.C_Data = File.ReadAllBytes(T_Path);
                    TXT_Info.text = "Import: OK";
                }
                catch (Exception ex)
                {
                    TXT_Info.text = "Import failed: " + ex;
                }
                break;

            case "Export":
                string T_Path_X = Application.dataPath + "/Files/Apple I/export.bytes";
                try
                {
                    File.WriteAllBytes(T_Path_X, Cart.C_Data);
                    TXT_Info.text = "Export: OK";
                }
                catch (Exception ex)
                {
                    TXT_Info.text = "Export failed: " + ex;
                }
                break;

            case "Format":
                Cart.C_Data = new byte[4096];
                TXT_Info.text = "Format: OK";
                break;

            case "Save":
                Cart.C_Title = IN_Title.text;
                Cart.C_EntryPoint = Convert.ToUInt16(IN_EntryPoint.text, 16);
                CDrive.SaveCart(Cart);
                if (CDrive.Cartridge != null)
                    if (Cart.C_Title == CDrive.Cartridge.C_Title)
                        CDrive.Cartridge = Cart;
                GUI.OpenPage(4);

                if (EditMode == true)
                {
                    if (Cart.C_Title != OldName)
                        CDrive.DeleteCart(OldName);
                }
                break;

            case "Standard":
                GUI.SYS.Settings.Cart_Standard = Cart.C_Title;
                GUI.SYS.SaveSettings();
                UpdateGUI();
                break;

            case "Cancel":
                GUI.OpenPage(4);
                break;
        }
    }
}

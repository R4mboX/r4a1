using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Apple1_RomSelection : MonoBehaviour
{
    [Header("Connections")]
    public GUI_Apple1 GUI;
    public ROMImporter Importer;
    public Keyboard_ASCII_Auto Keyboard;

    [Header ("UI Elements")]
    public List<Image> FileBGs = new List<Image>();
    public List<TMP_Text> FileTitles = new List<TMP_Text>();
    public TMP_Text TXT_Page;
    public GameObject BTN_Prev;
    public GameObject BTN_Next;
    public TMP_Text TXT_Info;

    private string[] FileList = new string[0];
    private int ActiveElement = 0;
    private int Page = 0;

    private Color COL_Inactive = new Color(0, 0.33f, 0);
    private Color COL_Hovered = new Color(0.03f, 0.66f, 0);
    private Color COL_Active = new Color(0.07f, 1f, 0);

    public void Init()
    {
        GetList();
        ManageButtons();
        Hover(99);
    }

    private void GetList()
    {
        string T_Path = Application.dataPath + "/Files/Apple I/Roms/";
        FileList = Directory.GetFiles(T_Path);
        List<string> T_FilteredList = new List<string>();
        for (int i = 0; i < FileList.Length; i++)
            if (Path.GetExtension(FileList[i]) != ".meta")
                T_FilteredList.Add(Path.GetFileName(FileList[i]));
        FileList = T_FilteredList.ToArray();
    }

    private void ManageButtons()
    {
        foreach (Image T_IMG in FileBGs)
            T_IMG.gameObject.SetActive(false);

        for (int i = 0; i < (FileList.Count() - (Page * 20)); i++)
        {
            FileBGs[i].gameObject.SetActive(true);
            FileTitles[i].text = FileList[i + (Page * 20)];
        }

        if (Page == 0)
            BTN_Prev.SetActive(false);
        else
            BTN_Prev.SetActive(true);

        if (((Page + 1) * 20) < (FileList.Count()))
            BTN_Next.SetActive(true);
        else
            BTN_Next.SetActive(false);
    }

    public void Click(int A_Selection)
    {
        ActiveElement = A_Selection;
        Hover(A_Selection);
    }

    public void ClickBTN(string A_Option)
    {
        switch(A_Option)
        {
            case "Run":
                Importer.Import(Application.dataPath + "/Files/Apple I/Roms/" + FileList[ActiveElement + (Page * 20)]);
                GUI.SYS.RAM.Memory = Importer.Memory;
                GUI.SYS.PIA.Display.ClearScreen();
                GUI.OpenPage(0);
                if (Importer.Entrypoint != 0xFFFF)
                {
                    GUI.SYS.CPU.PC = Importer.Entrypoint;
                    GUI.SYS.CPU.CPUStep = 0;
                    TXT_Info.text = "EntryPoint at: " + Importer.Entrypoint.ToString("X4");
                }
                else
                {
                    TXT_Info.text = "No EntryPoint found.";
                }

                if (Importer.AutoText.Count != 0)
                {
                    Keyboard.AutoText = Importer.AutoText;
                    Keyboard.ProcessText = true;
                }
                    
                break;

            case "Load":
                Importer.Import(Application.dataPath + "/Files/Apple I/Roms/" + FileList[ActiveElement + (Page * 20)]);
                GUI.SYS.RAM.Memory = Importer.Memory;

                GUI.SYS.PIA.Display.ClearScreen();
                GUI.OpenPage(0);
                if (Importer.Entrypoint != 0xFFFF)
                    TXT_Info.text = "EntryPoint at: " + Importer.Entrypoint.ToString("X4");
                else
                    TXT_Info.text = "No EntryPoint found.";
                break;

            case "PrevPage":
                Page--;
                ManageButtons();
                break;

            case "NextPage":
                Page++;
                ManageButtons();
                break;
        }
    }

    public void Hover(int A_Selection)
    {
        foreach (Image T_IMG in FileBGs)
            T_IMG.color = COL_Inactive;

        if (A_Selection != 99)
            FileBGs[A_Selection].color = COL_Hovered;
        FileBGs[ActiveElement].color = COL_Active;
    }
}

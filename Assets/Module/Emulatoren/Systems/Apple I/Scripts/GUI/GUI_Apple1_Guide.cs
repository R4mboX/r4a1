using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Apple1_Guide : MonoBehaviour
{
    [Header("Pages")]
    public List<GameObject> P_Emulator;
    public List<GameObject> P_Hardware;
    public List<GameObject> P_Wozmon;
    public List<GameObject> P_Credits;

    [Header("Buttons")]
    public List<Image> BTN_Categories = new List<Image>();
    public List<Image> BTN_Controls = new List<Image>();
    public TMP_Text TXT_Page;
    public GameObject BTN_Prev;
    public GameObject BTN_Next;

    private string Category = "Emulator";
    private int Page = 0;
    private int PageMax = 0;

    private Color CLR_Active = new Color(0, 0, 0, 0);
    private Color CLR_Hover = new Color(0, 0, 0, 0.25f);
    private Color CLR_Inactive = new Color(0, 0, 0, 0.5f);


    public void Init()
    {
        ChangeCategory("Emulator");
    }

    public void UpdatePage()
    {
        UpdateControls();

        foreach (GameObject T_Page in P_Emulator)
            T_Page.SetActive(false);

        foreach (GameObject T_Page in P_Hardware)
            T_Page.SetActive(false);

        foreach (GameObject T_Page in P_Wozmon)
            T_Page.SetActive(false);

        foreach (GameObject T_Page in P_Credits)
            T_Page.SetActive(false);

        switch (Category)
        {
            case "Emulator":
                P_Emulator[Page].SetActive(true);
                break;

            case "Hardware":
                P_Hardware[Page].SetActive(true);
                break;

            case "Wozmon":
                P_Wozmon[Page].SetActive(true);
                break;

            case "Credits":
                P_Credits[Page].SetActive(true);
                break;
        }

        UpdateControls();
    }

    public void UpdateControls()
    {
        if (Page == 0)
            BTN_Prev.SetActive(false);
        else
            BTN_Prev.SetActive(true);

        if (Page < PageMax)
            BTN_Next.SetActive(true);
        else
            BTN_Next.SetActive(false);

        TXT_Page.text = "PAGE " + Page;
    }

    public void ChangeCategory(string A_Category)
    {
        Category = A_Category;
        Page = 0;
        
        switch (A_Category)
        {
            case "Emulator":
                PageMax = P_Emulator.Count - 1;
                break;

            case "Hardware":
                PageMax = P_Hardware.Count - 1;
                break;

            case "Wozmon":
                PageMax = P_Wozmon.Count - 1;
                break;

            case "Credits":
                PageMax = P_Credits.Count - 1;
                break;
        }

        Hover_Categories("None");
        UpdatePage();
    }

    public void Hover_Categories(string A_Category)
    {
        foreach (Image T_IMG in BTN_Categories)
            T_IMG.color = CLR_Inactive;

        switch (A_Category)
        {
            case "Emulator":
                BTN_Categories[0].color = CLR_Hover;
                break;

            case "Hardware":
                BTN_Categories[1].color = CLR_Hover;
                break;

            case "Wozmon":
                BTN_Categories[2].color = CLR_Hover;
                break;

            case "Credits":
                BTN_Categories[3].color = CLR_Hover;
                break;

            case "None":
                break;
        }

        switch (Category)
        {
            case "Emulator":
                BTN_Categories[0].color = CLR_Active;
                break;

            case "Hardware":
                BTN_Categories[1].color = CLR_Active;
                break;

            case "Wozmon":
                BTN_Categories[2].color = CLR_Active;
                break;

            case "Credits":
                BTN_Categories[3].color = CLR_Active;
                break;
        }
    }

    public void Hover_Controls(string A_Button)
    {
        foreach (Image T_IMG in BTN_Controls)
            T_IMG.color = CLR_Inactive;

        switch(A_Button)
        {
            case "Prev":
                BTN_Controls[0].color = CLR_Hover;
                break;

            case "Next":
                BTN_Controls[1].color = CLR_Hover;
                break;

            case "None":
                break;
        }
    }

    public void Click_Controls(string A_Button)
    {
        switch (A_Button)
        {
            case "Prev":
                Page--;
                UpdatePage();
                break;

            case "Next":
                Page++;
                UpdatePage();
                break;
        }
    }
}

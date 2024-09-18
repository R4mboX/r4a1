using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Apple1_CartSelection : MonoBehaviour
{
    [Header("Connections")]
    public List<GameObject> Carts = new List<GameObject>();
    public List<TMP_Text> CartLabels = new List<TMP_Text>();
    public GUI_Apple1 GUI;
    public CartridgeDrive_Generic CartDrive;
    public TMP_Text TXT_Info;
    private string[] CartFolder;
    private int Selected = 0;
    private int Page = 0;
    private bool ConfirmDelete = false;
    public GameObject ButtonNP;
    public GameObject ButtonPP;
    public GameObject ButtonShare;
    

    public void Init()
    {
        GetList();
        Selected = 0;
        Page = 0;
        DrawSlots();
        Hover(99);
        MarkSelected();
        ConfirmDelete = false;
    }

    public void Hover(int A_Cart)
    {
        foreach(GameObject T_Cart in Carts)
            T_Cart.GetComponent<Image>().color = Color.black;

        if (A_Cart == 99)
        {
            MarkSelected();
            return;
        }
           
        Carts[A_Cart].GetComponent<Image>().color = new Color(0f, 0.5f, 0f);
        MarkSelected();
    }

    public void MarkSelected()
    {
        Carts[Selected].GetComponent<Image>().color = Color.green;
    }

    public void GetList()
    {
        string T_Path = Application.dataPath + "/Files/Apple I/Carts/";
        CartFolder = Directory.GetFiles(T_Path, "*.cart");
        for (int i = 0; i < CartFolder.Length; i++)
        {
            CartFolder[i] = Path.GetFileNameWithoutExtension(CartFolder[i]);
        }
    }

    public void DrawSlots()
    {
        foreach (GameObject T_Cart in Carts)
            T_Cart.SetActive(false);

        if (CartFolder.Count() > 0 + (Page * 12))
        {
            Carts[0].SetActive(true);
            CartLabels[0].text = CartFolder[0 + (Page * 12)];
        }

        if (CartFolder.Count() > 1 + (Page * 12))
        {
            Carts[1].SetActive(true);
            CartLabels[1].text = CartFolder[1 + (Page * 12)];
        }

        if (CartFolder.Count() > 2 + (Page * 12))
        {
            Carts[2].SetActive(true);
            CartLabels[2].text = CartFolder[2 + (Page * 12)];
        }

        if (CartFolder.Count() > 3 + (Page * 12))
        {
            Carts[3].SetActive(true);
            CartLabels[3].text = CartFolder[3 + (Page * 12)];
        }

        if (CartFolder.Count() > 4 + (Page * 12))
        {
            Carts[4].SetActive(true);
            CartLabels[4].text = CartFolder[4 + (Page * 12)];
        }

        if (CartFolder.Count() > 5 + (Page * 12))
        {
            Carts[5].SetActive(true);
            CartLabels[5].text = CartFolder[5 + (Page * 12)];
        }

        if (CartFolder.Count() > 6 + (Page * 12))
        {
            Carts[6].SetActive(true);
            CartLabels[6].text = CartFolder[6 + (Page * 12)];
        }

        if (CartFolder.Count() > 7 + (Page * 12))
        {
            Carts[7].SetActive(true);
            CartLabels[7].text = CartFolder[7 + (Page * 12)];
        }

        if (CartFolder.Count() > 8 + (Page * 12))
        {
            Carts[8].SetActive(true);
            CartLabels[8].text = CartFolder[8 + (Page * 12)];
        }

        if (CartFolder.Count() > 9 + (Page * 12))
        {
            Carts[9].SetActive(true);
            CartLabels[9].text = CartFolder[9 + (Page * 12)];
        }

        if (CartFolder.Count() > 10 + (Page * 12))
        {
            Carts[10].SetActive(true);
            CartLabels[10].text = CartFolder[10 + (Page * 12)];
        }

        if (CartFolder.Count() > 11 + (Page * 12))
        {
            Carts[11].SetActive(true);
            CartLabels[11].text = CartFolder[11 + (Page * 12)];
        }

        if (CartFolder.Count() > ((Page + 1) * 12))
            ButtonNP.SetActive(true);
        else
            ButtonNP.SetActive(false);

        if (Page <= 0)
            ButtonPP.SetActive(false);
        else
            ButtonPP.SetActive(true);

        // Not Implemented, can be used to share Carts.
        ButtonShare.SetActive(false);
    }

    public void Select(int A_Cart)
    {
        Selected = A_Cart;
        Hover(99);
        MarkSelected();
    }

    public void Controls(string A_Option)
    {
        switch(A_Option)
        {
            case "Load":
                CartDrive.SaveCart();
                CartDrive.LoadCart(CartFolder[Selected + (Page * 0)]);
                GUI.OpenPage(0);
                GUI.UpdateCart();
                break;

            case "Autorun":
                CartDrive.SaveCart();
                CartDrive.LoadCart(CartFolder[Selected + (Page * 0)]);
                GUI.OpenPage(0);
                GUI.UpdateCart();
                GUI.SYS.CPU.PC = (ushort)(CartDrive.Cartridge.C_EntryPoint + CartDrive.ADR_Start);
                break;

            case "Edit":
                GUI.CartEditor.Cart = CartDrive.ServeCart(CartFolder[Selected + (Page * 0)]);
                GUI.CartEditor.EditMode = true;
                GUI.CartEditor.OldName = GUI.CartEditor.Cart.C_Title;
                GUI.OpenPage(5);
                break;

            case "Share":
                break;

            case "Create":
                GUI.CartEditor.Cart = new Cartridge_Generic();
                GUI.OpenPage(5);
                break;

            case "Delete":
                if (ConfirmDelete == false)
                {
                    ConfirmDelete = true;
                    TXT_Info.text = "Click again to Confirm";
                }
                else
                {
                    CartDrive.DeleteCart(CartFolder[Selected + (Page * 0)]);
                    Init();
                    GUI.UpdateCart();
                }
                break;

            case "Next":
                if (CartFolder.Count() > ((Page + 1) * 12))
                    Page++;
                DrawSlots();
                break;

            case "Prev":
                if (Page > 0)
                    Page--;
                DrawSlots();
                break;
        }
    }
}

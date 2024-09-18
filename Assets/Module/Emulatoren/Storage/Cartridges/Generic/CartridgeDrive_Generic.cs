using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CartridgeDrive_Generic : MonoBehaviour
{
    [Header("Connections")]
    public Bus_Generic BUS;

    [Header("Settings")]
    public ushort ADR_Start = 0xA000;
    public ushort ADR_End = 0xBFFF;
    private bool AutoSaveToggle = false;
    public bool CFG_DebugAll = false;

    [Header("Storage")]
    public Cartridge_Generic Cartridge;

    public void Init(string A_StandardCart)
    {
        if (A_StandardCart == "")
            return;

        LoadCart(A_StandardCart);
    }

    public void Tick()
    {
        if (BUS.Address >= ADR_Start && BUS.Address <= ADR_End && Cartridge != null)
        {
            int offset = BUS.Address - ADR_Start;
            if (BUS.Read)
            {
                BUS.Data = Cartridge.C_Data[offset];
                if (CFG_DebugAll == true)
                    Debug.Log("[CART] Reading 0x" + BUS.Address.ToString("X2") + ", 0x" + BUS.Data.ToString("X2"));
            }
            else
            {
                if (Cartridge.WriteProtected == false)
                {
                    Cartridge.C_Data[offset] = BUS.Data;
                    if (CFG_DebugAll == true)
                        Debug.Log("[CART] Writing 0x" + BUS.Address.ToString("X2") + ", 0x" + BUS.Data.ToString("X2"));
                }
                else
                {
                    Debug.Log("[CART] Writing NOT ALLOWED 0x" + BUS.Address.ToString("X2") + ", 0x" + BUS.Data.ToString("X2"));
                }
            }
        }
    }

    public void Update()
    {
        if (AutoSaveToggle == false)
        {
            AutoSaveToggle = true;
            StartCoroutine(AutoSaveRoutine());
        }
    }

    public IEnumerator AutoSaveRoutine()
    {
        yield return new WaitForSeconds(300);
        if (Cartridge != null)
            if (Cartridge.AutoSave == true)
                SaveCart();
        AutoSaveToggle = false;
    }

    public void LoadCart(string A_Name)
    {
        string path = Application.dataPath + "/Files/Apple I/Carts/" + A_Name + ".cart";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Cartridge = (Cartridge_Generic)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("[CART] File not found at " + path);
        }
    }

    public Cartridge_Generic ServeCart(string A_Name)
    {
        string path = Application.dataPath + "/Files/Apple I/Carts/" + A_Name + ".cart";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Cartridge_Generic T_Cartridge = (Cartridge_Generic)formatter.Deserialize(file);
            file.Close();
            //Debug.Log("[CART] loaded successfully from: " + path);
            return T_Cartridge;
        }
        else
        {
            return null;
        }
    }

    public void SaveCart()
    {
        if (Cartridge != null && Cartridge.WriteProtected == false)
        {
            string path = Application.dataPath + "/Files/Apple I/Carts/" + Cartridge.C_Title + ".cart";
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, Cartridge);
            file.Close();
            Debug.Log("[CART] Cartridge saved: " + path);
        }   
    }

    public void SaveCart(Cartridge_Generic A_Cart)
    {
        string path = Application.dataPath + "/Files/Apple I/Carts/" + A_Cart.C_Title + ".cart";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file, A_Cart);
        file.Close();
        Debug.Log("[CART] Cartridge saved: " + path);
    }

    public void CreateNewCart()
    {
        Cartridge = new Cartridge_Generic();
    }

    public void DeleteCart(string A_Name)
    {
        string path = Application.dataPath + "/Files/Apple I/Carts/" + A_Name + ".cart";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("[CART] Cartridge deleted: " + path);
        }

        if (Cartridge != null)
            if (A_Name == Cartridge.C_Title)
                Eject();
    }

    public void Eject()
    {
        Cartridge = null;
    }
}

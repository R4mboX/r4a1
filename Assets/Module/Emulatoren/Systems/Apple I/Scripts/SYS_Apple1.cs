using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SYS_Apple1 : MonoBehaviour
{
    [Header("Connections")]
    public Bus_Generic Bus;
    public CPU_MOS6502 CPU;
    public Memory_Generic RAM;
    public CartridgeDrive_Generic CART;
    public Ethernet_Simple NET;
    public PIA_Simple PIA;
    public ROM_Generic ROM1;
    public ROM_Generic ROM2;
    public GUI_Apple1 GUI;

    public ROMImporter Importer;
    public Keyboard_ASCII_Auto AutoKeyboard;

    [Header("Memory")]
    public ushort[] M_RAM = new ushort[]    { 0x0000, 0x9FFF };
    public ushort[] M_CART = new ushort[]   { 0xA000, 0xBFFF };
    public ushort[] M_ACI = new ushort[]    { 0xC000, 0xCFFF }; // [C000]Hardware; [C100] ROM; Not implemented.
    public ushort[] M_PIA = new ushort[]    { 0xD000, 0xDFFF }; // Ignored | [0xD010] - [0xD012]
    public ushort[] M_ROM1 = new ushort[]   { 0xE000, 0xEFFF }; // Apple Integer Basic; [0xE000] Entry Point
    public ushort[] M_ROM2 = new ushort[]   { 0xFF00, 0xFFFF }; // WOZ Monitor; [0xFF00] - [0xFFFF] used.

    [Header("Settings")]
    public bool Run = false;
    public SET_Apple1 Settings = new SET_Apple1();

    private int CyclesPerFrame;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Run)
        {
            int AddCycles = CPU.CycleCollector;
            CPU.CycleCollector = 0;

            for (int i = AddCycles; i < CyclesPerFrame; i++)
            {
                Heartbeat();
            }
        }

        GUI.Tick();
    }

    private void Init()
    {
        LoadSettings();
        SetSpeed();
        SetLayout();

        RAM.ADR_Start = M_RAM[0];
        RAM.ADR_End = M_RAM[1];
        RAM.Init();

        CART.ADR_Start = M_CART[0];
        CART.ADR_End = M_CART[1];

        ROM1.ADR_Start = M_ROM1[0];
        ROM1.ADR_End = M_ROM1[1];
        ROM1.Init();

        ROM2.ADR_Start = M_ROM2[0];
        ROM2.ADR_End = M_ROM2[1];
        ROM2.Init();

        CART.Init(Settings.Cart_Standard);
        GUI.Init();
    }

    public void SetSpeed()
    {
        Application.targetFrameRate = Settings.Framerate;
        CyclesPerFrame = Mathf.RoundToInt((Settings.Frequency * Settings.Multiplier) / Settings.Framerate);
    }

    private void Heartbeat()
    {
        CPU.Tick();
        RAM.Tick();
        CART.Tick();
        ROM1.Tick();
        ROM2.Tick();
        NET.Tick();
        PIA.Tick();
    }

    public void Reset()
    {
        Init();
        CPU.CPUStep = -3;
    }

    private void LoadSettings()
    {
        string path = Application.dataPath + "/Files/Apple I/Settings.r4";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Settings = (SET_Apple1)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        string path = Application.dataPath + "/Files/Apple I/Settings.r4";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file, Settings);
        file.Close();
    }

    public void SetLayout()
    {
        PIA.Keyboard.Layout = Settings.Layout;
    }
}
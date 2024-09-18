using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Windows.WebCam;

public class TEST_MOS6502 : MonoBehaviour
{
    // This Script was used to run JSON Tests against the CPU.
    // It might not work anymore out of the box and needs Cleanup.

    [Header("Connections")]
    public CPU_MOS6502 CPU;
    public Memory_Generic Memory;

    [Header("Settings")]
    public string Path_Tests = Application.dataPath + "/Module/Emulatoren/CPU/MOS6502/Tests/";
    public ushort RAMStart = 0x0000;
    public ushort RAMEnd = 0xBFFF;
    public string OPCode = "00";
    public int Iterations = 0; // 0 = ALL
    public bool Debugging = false;
    public bool IgnoreDecimalMode = true;
    public bool Run;

    [Header("Cycles")]
    public bool TestCycles = true;
    public bool OnlyCountCycles = true;
    public bool DebugCycles = false;
    public bool AcceptCycleOffset = true;

    private struct Cycle
    {
        public ushort Address;
        public byte Data;
        public bool Read;

        public Cycle(ushort A_Address, byte A_Data, bool A_Read)
        {
            Address = A_Address;
            Data = A_Data;
            Read = A_Read;
        }
    }

    private void Update()
    {
        if (Run == true)
        {
            Run = false;
            TextAsset jsonFile = new TextAsset(File.ReadAllText(Path_Tests + OPCode + ".json"));
            jsonFile.name = OPCode;
            RunJSON(jsonFile);
        }
    }

    private void Heartbeat()
    {
        CPU.Tick();
        Memory.Tick();
    }

    public void RunJSON(TextAsset JSONFile)
    {
        var root = SimpleJSON.JSON.Parse(JSONFile.text);

        int Test_OK = 0;
        int Test_Failure = 0;
        List<Cycle> Cycles = new List<Cycle>();

        // Prepare RAM
        Memory.ADR_End = 0xFFFF;
        Memory.Memory = new byte[Memory.ADR_End - Memory.ADR_Start + 1];
        Memory.ADR_Ignored = new List<ByteVector>();

        if (Iterations == 0)
            Iterations = root.Count;

        for (int i = 0; i < Iterations; i++) // root.Count
        {
            // Load initial Values
            CPU.PC = (ushort)root[i]["initial"]["pc"].AsInt;
            CPU.SP = (byte)root[i]["initial"]["s"].AsInt;
            CPU.A = (byte)root[i]["initial"]["a"].AsInt;
            CPU.X = (byte)root[i]["initial"]["x"].AsInt;
            CPU.Y = (byte)root[i]["initial"]["y"].AsInt;
            CPU.Status = (byte)root[i]["initial"]["p"].AsInt;

            // Load RAM
            for (int j = 0; j < root[i]["initial"]["ram"].Count; j++)
            {
                Memory.Memory[root[i]["initial"]["ram"][j][0]] = (byte)root[i]["initial"]["ram"][j][1].AsInt;
            }

            // Execute
            CPU.OPC = (byte)Convert.ToInt32(JSONFile.name, 16);
            CPU.CPUStep = 0;

            CPU.CycleCollector = 0;
            Cycles = new List<Cycle>();

            Heartbeat(); // OPCode laden
            Cycles.Add(new Cycle(CPU.Bus.Address, CPU.Bus.Data, CPU.Bus.Read));
            Heartbeat(); // OPCode laden
            Cycles.Add(new Cycle(CPU.Bus.Address, CPU.Bus.Data, CPU.Bus.Read));

            while (CPU.CPUStep >= 2 && CPU.CPUStep <= 10)
            {
                Heartbeat();
                Cycles.Add(new Cycle(CPU.Bus.Address, CPU.Bus.Data, CPU.Bus.Read));
            }

            // Inspect
            bool T_Failure = false;
            if (CPU.PC != (ushort)root[i]["final"]["pc"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("Failed: PC, Got: 0x" + CPU.PC.ToString("X2") + ", Expected: 0x" + root[i]["final"]["pc"].AsInt.ToString("X2"));
                T_Failure = true;
            }
                
            if (CPU.SP != (byte)root[i]["final"]["s"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("SP failed");
                T_Failure = true;
            }

            if (CPU.A != (byte)root[i]["final"]["a"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("Failed: A, Got: 0x" + CPU.A.ToString("X2") + ", Expected: 0x" + root[i]["final"]["a"].AsInt.ToString("X2"));
                T_Failure = true;
            }

            if (CPU.X != (byte)root[i]["final"]["x"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("X failed");
                T_Failure = true;
            }

            if (CPU.Y != (byte)root[i]["final"]["y"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("Y failed");
                T_Failure = true;
            }

            if (CPU.Status != (byte)root[i]["final"]["p"].AsInt)
            {
                if (Debugging == true)
                    Debug.Log("Failed: Status Register, Got: 0x" + CPU.Status.ToString("X2") + ", Expected: 0x" + root[i]["final"]["p"].AsInt.ToString("X2"));
                T_Failure = true;
            }

            // Inspect Memory
            for (int j = 0; j < root[i]["final"]["ram"].Count; j++)
            {
                ushort T_Location = (ushort)root[i]["final"]["ram"][j][0].AsInt;
                byte T_Exp = (byte)root[i]["final"]["ram"][j][1].AsInt;
                byte T_Real = Memory.Memory[root[i]["final"]["ram"][j][0].AsInt];

                if (T_Exp != T_Real)
                {
                    if (Debugging == true)
                        Debug.Log("Failed: Memory[0x" + T_Location.ToString("X2") + "]. Got: 0x" + T_Real.ToString("X2") + ", Expected: 0x" + T_Exp.ToString("X2"));
                    T_Failure = true;
                }
            }

            // Cycle Test
            if (TestCycles == true)
            {
                int T_CycleCount = Cycles.Count;
                if (AcceptCycleOffset == true)
                    T_CycleCount -= (int)CPU.CycleCollector;

                if (T_CycleCount != root[i]["cycles"].Count)
                {
                    if (Debugging == true)
                        Debug.Log("Failed: Wrong amount of Cycles. Got: " + T_CycleCount + ", Expected: " + root[i]["cycles"].Count);
                    T_Failure = true;
                }
            }
            
            // Ignore Decimal Mode
            if (T_Failure == true && CPU.ReadFlag(3) == true && IgnoreDecimalMode == true)
                T_Failure = false;

            // Result
            if (T_Failure == true)
            {
                Test_Failure++;
                Debug.Log(root[i]);
            }  
            else
                Test_OK++;
        }
        if (Test_Failure > 0)
            Debug.LogError("OK: " + Test_OK + ", Failed: " + Test_Failure);
        else
            Debug.Log("0x" + OPCode + " | OK: " + Test_OK + ", Failed: " + Test_Failure);
    }

    public bool ReadFlag(byte A_Byte, int A_Bit)
    {
        return (A_Byte & (1 << A_Bit)) != 0;
    }
}
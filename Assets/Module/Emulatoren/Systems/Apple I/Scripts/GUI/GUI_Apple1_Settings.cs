using TMPro;
using UnityEngine;

public class GUI_Apple1_Settings : MonoBehaviour
{
    [Header("Connections")]
    public SYS_Apple1 SYS;

    [Header("UI Elements")]
    public TMP_InputField IN_Profile;
    public TMP_InputField IN_Frequency;
    public TMP_InputField IN_Multiplier;
    public TMP_InputField IN_FrameRate;
    public TMP_Dropdown DD_Layout;

    public void Init()
    {
        GetSettings();
    }

    private void GetSettings()
    {
        IN_Profile.text = SYS.Settings.Profile;
        IN_Frequency.text = SYS.Settings.Frequency.ToString();
        IN_Multiplier.text = SYS.Settings.Multiplier.ToString();
        IN_FrameRate.text = SYS.Settings.Framerate.ToString();
    }

    public void SaveSettings()
    {
        SYS.Settings.Profile = IN_Profile.text;
        SYS.Settings.Frequency = int.Parse(IN_Frequency.text);
        if (SYS.Settings.Frequency < 100)
            SYS.Settings.Frequency = 100;
        SYS.Settings.Multiplier = int.Parse(IN_Multiplier.text);
        SYS.Settings.Framerate = int.Parse(IN_FrameRate.text);
        
        switch (DD_Layout.value)
        {
            case 0:
                SYS.Settings.Layout = "US";
                break;

            case 1:
                SYS.Settings.Layout = "UK";
                break;

            case 2:
                SYS.Settings.Layout = "DE";
                break;

            case 3:
                SYS.Settings.Layout = "Original";
                break;
        }

        SYS.SaveSettings();
        SYS.SetLayout();
        SYS.SetSpeed();
        SYS.GUI.OpenPage(0);
    }

    public void Default()
    {
        SYS.Settings = new SET_Apple1();
        SYS.SaveSettings();
        GetSettings();
    }
}

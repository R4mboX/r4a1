using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Display_SimpleText : MonoBehaviour
{
    // Simplified Version

    [Header("Verbindungen")]
    public TextAsset CharMapROM;
    public List<Image> DisplayImages = new List<Image>();

    [Header("Status")]
    private byte[] ScreenBuffer = new byte[40 * 24];
    public int CursorX = 0;
    public int CursorY = 0;
    public bool Delay = false;
    public bool PleaseWait = false;
    private Texture2D DisplayTexture; 
    private byte[] CharMap;
    private bool CursorBlinkstate = false;

    public void PrepareCharacter(byte A_Char)
    {
        PleaseWait = true;

        // Only Capital Letters
        if (A_Char >= 0x61 && A_Char <= 0x7A) 
            A_Char &= 0x5F;
            
        // New Line
        if (A_Char == 0x0D)
        {
            Unblink();
            CursorX = 0;
            CursorY++;
        }

        // Range supported by CharMap -> Draw
        if (A_Char >= 0x20 && A_Char <= 0x5F)
        {
            ScreenBuffer[CursorY * 40 + CursorX] = A_Char;
            RenderCharacter(CursorX, CursorY, CharMap, A_Char);
            CursorX++;
        }

        // End of Line
        if (CursorX == 40)
        {
            Unblink();
            CursorX = 0;
            CursorY++;
        }

        // End of Screen
        if (CursorY == 24)
        {
            ScrollScreen();
            CursorY--;
        }

        UpdateDisplayImage();
        if (Delay == true)
            StartCoroutine(Waiter());
        else
            PleaseWait = false;
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.1f);
        PleaseWait = false;
    }

    private void ScrollScreen()
    {
        // Move everything up
        System.Array.Copy(ScreenBuffer, 40, ScreenBuffer, 0, (24 - 1) * 40);

        // Create new empty Line
        System.Array.Clear(ScreenBuffer, (24 - 1) * 40, 40);

        // Render what's changed
        for (int y = 0; y < 24 - 1; y++)
        {
            for (int x = 0; x < 40; x++)
            {
                RenderCharacter(x, y, CharMap, ScreenBuffer[y * 40 + x]);
            }
        }

        // Render last line
        for (int x = 0; x < 40; x++)
        {
            RenderCharacter(x, 24 - 1, CharMap, 0x20); // Leerzeichen
        }

        UpdateDisplayImage();
    }

    private void RenderCharacter(int x, int y, byte[] charmap, byte data)
    {
        int invertedY = (24 - 1) - y;
        for (int row = 0; row < 8; row++)
        {
            byte lineData = charmap[data * 8 + (7 - row)];
            lineData = ReverseBits(lineData);
            for (int col = 0; col < 8; col++)
            {
                bool pixelOn = (lineData & (1 << (7 - col))) != 0;
                DisplayTexture.SetPixel(x * 8 + col, invertedY * 8 + row, pixelOn ? Color.green : Color.black);
            }
        }
        DisplayTexture.Apply();
    }

    private void UpdateDisplayImage()
    {
        Sprite sprite = Sprite.Create(DisplayTexture, new Rect(0, 0, DisplayTexture.width, DisplayTexture.height), new Vector2(1f, 1));
        foreach (Image T_IMG in DisplayImages)
            T_IMG.sprite = sprite;
    }

    public void ClearScreen()
    {
        for (int i = 0; i < ScreenBuffer.Length; i++)
        {
            ScreenBuffer[i] = 0x20;
        }
        Color32[] blackPixels = new Color32[DisplayTexture.width * DisplayTexture.height];
        for (int i = 0; i < blackPixels.Length; i++)
        {
            blackPixels[i] = new Color32(0, 0, 0, 255);
        }
        DisplayTexture.SetPixels32(blackPixels);
        DisplayTexture.Apply();
        CursorX = 0;
        CursorY = 0;
        UpdateDisplayImage();
    }

    private void Start()
    {
        CharMap = CharMapROM.bytes;
        DisplayTexture = new Texture2D(40 * 8, 24 * 8, TextureFormat.RGBA32, false);
        DisplayTexture.filterMode = FilterMode.Point;
        DisplayTexture.Apply();
        ClearScreen();
        StartCoroutine(Blinky());
    }

    private byte ReverseBits(byte value)
    {
        byte result = 0;
        for (int i = 0; i < 8; i++)
        {
            result = (byte)((result << 1) | (value & 1));
            value >>= 1;
        }
        return result;
    }

    private IEnumerator Blinky()
    {
        while (true)
        {
            CursorBlinkstate = !CursorBlinkstate;

            if (CursorBlinkstate)
            {
                // Draw @
                RenderCharacter(CursorX, CursorY, CharMap, 0x40);
            }
            else
            {
                // Remove @
                RenderCharacter(CursorX, CursorY, CharMap, ScreenBuffer[CursorY * 40 + CursorX]);
            }

            UpdateDisplayImage();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Unblink()
    {
        CursorBlinkstate = false;
        RenderCharacter(CursorX, CursorY, CharMap, 0x20);
    }
}

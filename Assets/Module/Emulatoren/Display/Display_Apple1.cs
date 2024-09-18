using UnityEngine;
using UnityEngine.UI;

public class Display_Apple1 : MonoBehaviour
{
    [Header("Verbindungen")]
    public PIA_Simple PIA;

    public TextAsset charmapROM;
    public Image DisplayImage;  // Verwende Image für das UI-Element
    private Texture2D displayTexture;
    private byte[] charmap;
    private byte[] screenBuffer;
    private int nCols = 40;
    private int nRows = 24;
    private int nCharWidth = 8;
    private int nCharHeight = 8;
    private int nCursorX = 0;
    private int nCursorY = 0;

    public void Start()
    {
        charmap = charmapROM.bytes;
        screenBuffer = new byte[nCols * nRows];
        displayTexture = new Texture2D(nCols * nCharWidth, nRows * nCharHeight, TextureFormat.RGBA32, false);
        displayTexture.filterMode = FilterMode.Point; // Setze den Filtermodus auf Point, um scharfe Kanten zu erhalten
        ClearScreen();  // ClearScreen aufrufen, bevor die Textur angewendet wird
        displayTexture.Apply();

        // Initialisiere das DisplayImage mit einem Sprite, das von der Texture2D erstellt wurde
        UpdateDisplayImage();
    }

    private void Update()
    {
        
    }

    public bool ReadFlag(byte A_Byte, int A_Bit)
    {
        return (A_Byte & (1 << A_Bit)) != 0;
    }

    public byte SetFlag(byte A_Byte, int A_Bit, bool A_State)
    {
        if (A_State)
            A_Byte |= (byte)(1 << A_Bit);
        else
            A_Byte &= (byte)~(1 << A_Bit);

        return A_Byte;
    }

    public void ClearScreen()
    {
        // Bildschirm löschen und auf Schwarz setzen
        for (int i = 0; i < screenBuffer.Length; i++)
        {
            screenBuffer[i] = 0x20; // Leerzeichen
        }
        Color32[] blackPixels = new Color32[displayTexture.width * displayTexture.height];
        for (int i = 0; i < blackPixels.Length; i++)
        {
            blackPixels[i] = new Color32(0, 0, 0, 255); // Setze alle Pixel auf Schwarz
        }
        displayTexture.SetPixels32(blackPixels);
        displayTexture.Apply();
        nCursorX = 0;
        nCursorY = 0;
        Debug.Log("DISPLAY CLEAR");
        UpdateDisplayImage();  // Aktualisiere das Sprite nach dem Löschen
    }

    public void RenderCharacter(byte data)
    {
        if (data >= 0x61 && data <= 0x7A)
            data &= 0x5F; // Kleinbuchstaben in Großbuchstaben umwandeln

        switch (data)
        {
            case 0x0D: // Carriage Return
                nCursorX = 0;
                nCursorY++;
                break;
            default:
                if (data >= 0x20 && data <= 0x5F)
                {
                    screenBuffer[nCursorY * nCols + nCursorX] = data;
                    RenderCharacter(nCursorX, nCursorY, charmap, data);
                    nCursorX++;
                }
                break; 
        }

        // Überprüfen, ob der Cursor am Ende der Zeile ist
        if (nCursorX == nCols)
        {
            nCursorX = 0;
            nCursorY++;
        }

        // Überprüfen, ob der Cursor am Ende des Bildschirms ist (Scrolling)
        if (nCursorY == nRows)
        {
            ScrollScreen();
            nCursorY--;
        }

        UpdateDisplayImage();  // Aktualisiere das Sprite nach jeder Zeichnung
    }

    private void RenderCharacter(int x, int y, byte[] charmap, byte data)
    {
        for (int row = 0; row < nCharHeight; row++)
        {
            byte lineData = charmap[data * nCharHeight + row];
            for (int col = 0; col < nCharWidth; col++)
            {
                bool pixelOn = (lineData & (1 << (7 - col))) != 0;
                displayTexture.SetPixel(x * nCharWidth + col, y * nCharHeight + row, pixelOn ? Color.green : Color.black);
            }
        }
        displayTexture.Apply();
    }

    private void ScrollScreen()
    {
        for (int y = 0; y < nRows - 1; y++)
        {
            for (int x = 0; x < nCols; x++)
            {
                screenBuffer[y * nCols + x] = screenBuffer[(y + 1) * nCols + x];
                RenderCharacter(x, y, charmap, screenBuffer[y * nCols + x]);
            }
        }

        // Letzte Zeile löschen
        for (int x = 0; x < nCols; x++)
        {
            screenBuffer[(nRows - 1) * nCols + x] = 0x20; // Leerzeichen
            RenderCharacter(x, nRows - 1, charmap, 0x20);
        }

        UpdateDisplayImage();  // Aktualisiere das Sprite nach dem Scrollen
    }

    private void UpdateDisplayImage()
    {
        // Wandle die Textur in ein Sprite um und weise es dem Image zu
        Sprite sprite = Sprite.Create(displayTexture, new Rect(0, 0, displayTexture.width, displayTexture.height), new Vector2(0.5f, 0.5f));
        DisplayImage.sprite = sprite;
    }
}

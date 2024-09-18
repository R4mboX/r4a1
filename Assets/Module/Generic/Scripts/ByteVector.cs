using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ByteVector
{
    public byte b1 = 0x00;
    public byte b2 = 0x00;

    public ByteVector()
    {
        b1 = 0x00;
        b2 = 0x00;
    }

    public ByteVector(byte tb1, byte tb2)
    {
        b1 = tb1;
        b2 = tb2;
    }
}

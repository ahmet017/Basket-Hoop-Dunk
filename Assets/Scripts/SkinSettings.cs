using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSettings : ScriptableObject
{
    public List<BallSkin> ballSkins = new List<BallSkin>();
    public List<LineSkin> lineSkins = new List<LineSkin>();
    public bool unlockAllSkins;
}

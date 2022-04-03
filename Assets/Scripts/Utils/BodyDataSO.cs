using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyDataSO", menuName = "BodyDataSO", order = 1)]
public class BodyDataSO : ScriptableObject
{
    public string Name;
    public Sprite Front1;
    public Sprite Front2;
    public Sprite Front3;
    public Sprite Back1;
    public Sprite Back2;
    public Sprite Back3;
    public Sprite Rot1;
    public Sprite Rot2;
    public Sprite Rot3;
    public Sprite Dead;
    public Color PieceTint;
    public Vector3 EyeOffset;
}

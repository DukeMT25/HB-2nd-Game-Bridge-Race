using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/BrickColor", order = 0)]
public class BrickColor : ScriptableObject
{
    public List<Material> Mats;

    public Material GetMaterial(BrickType brickType)
    {
        if ((int)brickType >= 5)
        {
            return Mats[5];
        }
        return Mats[(int)brickType];
    }
}

public enum BrickType
{
    Green, Red, Purple, Yellow, White, Gray
}
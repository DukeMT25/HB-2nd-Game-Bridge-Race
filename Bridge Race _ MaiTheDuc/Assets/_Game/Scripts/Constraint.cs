using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint : MonoBehaviour
{
    public const string LAYER_WALL = "Wall";

    public static int idleName = Animator.StringToHash("Idle");
    public static int runName = Animator.StringToHash("Running");
    public static int danceName = Animator.StringToHash("Dance");
}

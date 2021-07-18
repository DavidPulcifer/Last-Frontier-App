using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Hacking Puzzle", menuName = "Puzzles/Hacking Puzzle", order = 1)]
public class HackingPuzzle : ScriptableObject
{
    public string deviceName;
    [Range(1, 10)] public int difficulty;
    public HackingPuzzleType type;
    public HackingMethod[] weakAgainst;
    public HackingMethod[] strongAgainst;
    [TextArea] public string description;
}

public enum HackingPuzzleType
{
    Terminal,
    Lockbox,
    Safe,
}

public enum HackingMethod
{
    Keylogger,
    Install_Rootkit,
    SQL_Injection,
    Brute_Force,
    None,
}

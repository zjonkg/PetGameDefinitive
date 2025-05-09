using UnityEditor.VersionControl;
using UnityEngine;

[System.Serializable]
public class PlayerGameData
{
    public string id;
    public string id_minigames;
    public int score;
    public int moneyGained;
}

public class ResponseGameData
{
    public string message;
}
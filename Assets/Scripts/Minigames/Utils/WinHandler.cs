using UnityEngine;
using ShyLaura.Database;
using System.Data; // Asegúrate de tener esto si usas IDataReader

public class WinHandler : MonoBehaviour
{
    private ScoreMatch scoreDb;

    void Start()
    {
        scoreDb = new ScoreMatch();

        InsertScore("1", "1", 2500, 120);
        ReadAllScores();

        scoreDb.close();
    }

    private void InsertScore(string playerId, string matchId, int score, int time)
    {
        scoreDb.insertData(playerId, matchId, score, time);
        Debug.Log("Score inserted successfully.");
    }

    private void ReadAllScores()
    {
        var reader = scoreDb.getAllData();
        while (reader.Read())
        {
            Debug.Log("Player: " + reader["player_id"] + " | Score: " + reader["score"]);
        }
        reader.Close(); // Siempre cerrar el reader después de usarlo
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; // Asegúrate de usar esto si vas a mostrar el tiempo en pantalla
using ShyLaura.Database;
using UnityEngine.Networking;

public class MemoryGameManagerUI : MinigamesBase
{
    public static MemoryGameManagerUI Instance { get; private set; }
    public GameObject winPanel;
    [SerializeField] private CardGroup cardGroup;
    [SerializeField] private List<CardSingleUI> cardSingleUIList = new List<CardSingleUI>();
    [SerializeField] private TextMeshProUGUI timerText; 

    private float timeRemaining = 0f;
    private bool timerRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cardGroup.OnCardMatch += CardGroup_OnCardMatch;
    }

    private bool lowTimeWarningTriggered = false;

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";

            // Activar animación si quedan 10 segundos o menos, y no se ha activado antes
            if (timerRunning && timeRemaining <= 10f && !lowTimeWarningTriggered)
            {
                lowTimeWarningTriggered = true;

                // Escalado con rebote
                timerText.transform.DOScale(1.3f, 0.3f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);

                // Cambiar color a rojo con loop
                timerText.DOColor(Color.red, 0.3f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
    }


    private void OnEnable()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.1f);
        SetTimerByDifficulty(CardGridUI.CurrentDifficulty);
        StartCoroutine(StartCountdownTimer());
    }
    private void SetTimerByDifficulty(DifficultyEnum difficulty)
    {
        switch (difficulty)
        {
            case DifficultyEnum.Easy:
                timeRemaining = 30f;
                break;
            case DifficultyEnum.Normal:
                timeRemaining = 45f;
                break;
            case DifficultyEnum.Hard:
                timeRemaining = 60f;
                break;
        }
    }

    public void Subscribe(CardSingleUI cardSingleUI)
    {
        if (cardSingleUIList == null)
        {
            cardSingleUIList = new List<CardSingleUI>();
        }

        if (!cardSingleUIList.Contains(cardSingleUI))
        {
            cardSingleUIList.Add(cardSingleUI);
        }
    }

    private void CardGroup_OnCardMatch(object sender, System.EventArgs e)
    {
        if (cardSingleUIList.All(x => x.GetObjectMatch()))
        {
            StartCoroutine(OnCompleteGame());
        }
    }

    private IEnumerator OnCompleteGame()
    {
        timerRunning = false;
        StopTimerAnimation();
        yield return new WaitForSeconds(0.75f);

        Debug.Log("Has ganado");

        int userId = PlayerPrefs.GetInt("user_id", 0); // ID del usuario
        int matchid = System.Guid.NewGuid().GetHashCode(); // ID único por partida
        int scoreMatch = CalculateScore(); // Implementa tu lógica
        int coinMatch = Mathf.FloorToInt(timeRemaining); // O lo que represente las monedas


        PostGameScore(userId, matchid, scoreMatch, coinMatch);

        using (var scoreDb = new ScoreMatch())
        {
            string playerId = PlayerPrefs.GetString("player_id", "default_id");
            string matchId = System.Guid.NewGuid().ToString(); // ID único por partida
            int score = CalculateScore(); // Implementa tu lógica
            int coinGained = Mathf.FloorToInt(timeRemaining); // O lo que represente las monedas

            scoreDb.insertData(playerId, matchId, score, coinGained);
            Debug.Log("Datos guardados en la DB.");
        }

        winPanel.SetActive(true);
    }


    private int CalculateScore()
    {
        // Lógica personalizada, aquí solo un ejemplo simple
        int baseScore = 1000;
        int bonus = Mathf.FloorToInt(timeRemaining * 10);
        return baseScore + bonus;
    }


    public void Restart()
    {
        cardSingleUIList.Clear();
        timeRemaining = 60f;
        timerRunning = false;
        lowTimeWarningTriggered = false;

        // Reiniciar animaciones DOTween
        timerText.transform.DOKill();
        timerText.transform.localScale = Vector3.one;
        timerText.DOKill();
        timerText.color = Color.white;

        StartCoroutine(StartCountdownTimer());
    }

    private void StopTimerAnimation()
    {
        timerText.transform.DOKill(); // Detener animaciones de escala
        timerText.DOKill();           // Detener animaciones de color
        timerText.transform.localScale = Vector3.one;
        timerText.color = Color.white;
    }



    private IEnumerator StartCountdownTimer()
    {
        yield return new WaitForSeconds(3f); // Espera inicial de 3 segundos
        timerRunning = true;

        while (timeRemaining > 0 && timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        if (timeRemaining <= 0)
        {
            timerRunning = false;
            timerText.text = "00:00";
            Debug.Log("Tiempo agotado");
            StopTimerAnimation();
            // Aquí puedes poner lógica para cuando se acabe el tiempo (perder, reiniciar, etc.)
        }
    }

    public class GameScoreData
    {
        public int id_user;
        public int id_minigame;
        public int score;
        public int money_earned;
    }

    private IEnumerator PostGameScore(int userId, int minigameId, int score, int moneyEarned)
    {
        string url = "http://127.0.0.1:8000/play"; // Reemplaza con tu URL

        // Crear el objeto de datos
        GameScoreData scoreData = new GameScoreData
        {
            id_user = userId,
            id_minigame = minigameId,
            score = score,
            money_earned = moneyEarned
        };

        // Convertir a JSON
        string jsonData = JsonUtility.ToJson(scoreData);
        Debug.Log("Enviando JSON: " + jsonData);

        // Configurar la solicitud
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar respuesta
        yield return request.SendWebRequest();

        // Manejar la respuesta
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);
            // Aquí puedes procesar la respuesta si es necesario
        }
        else
        {
            Debug.LogError("Error al enviar datos: " + request.error);
            Debug.LogError("Respuesta del servidor: " + request.downloadHandler.text);
        }
    }
}

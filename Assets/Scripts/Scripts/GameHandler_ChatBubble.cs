using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler_ChatBubble : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        // Puedes dejar esto si quieres una burbuja inicial estática, o quitarlo
        // ChatBubble.Create(playerTransform, new Vector3(0, 1.7f, 2.5f), ChatBubble.IconType.Neutral, "Here is some text!");

        // ❌ Ya NO se imprimen mensajes aleatorios periódicos
        // FunctionPeriodic.Create(() => {
        //     string message = GetRandomMessage();
        //     ChatBubble.IconType[] iconArray = {
        //         ChatBubble.IconType.Happy,
        //         ChatBubble.IconType.Neutral,
        //         ChatBubble.IconType.Angry
        //     };
        //     ChatBubble.IconType icon = iconArray[Random.Range(0, iconArray.Length)];
        //     ChatBubble.Create(playerTransform, new Vector3(0, 1.7f, 2.5f), icon, message);
        // }, 5.5f);
    }

    public void ShowChatMessage(string message)
    {
        ChatBubble.IconType[] iconArray = {
            ChatBubble.IconType.Happy,
            ChatBubble.IconType.Neutral,
            ChatBubble.IconType.Angry
        };

        ChatBubble.IconType icon = iconArray[Random.Range(0, iconArray.Length)];

        ChatBubble.Create(playerTransform, new Vector3(0, 1.7f, 2.5f), icon, message);
    }
}

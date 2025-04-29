using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler_ChatBubble : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;

    private void Start()
    {
         ChatBubble.Create(playerTransform, new Vector3(0, 1.7f, 2.5f), ChatBubble.IconType.Neutral, "Here is some text!");


        FunctionPeriodic.Create(() => {
            string message = GetRandomMessage();


            ChatBubble.IconType[] iconArray =
                new ChatBubble.IconType[] { ChatBubble.IconType.Happy, ChatBubble.IconType.Neutral, ChatBubble.IconType.Angry };
            ChatBubble.IconType icon = iconArray[Random.Range(0, iconArray.Length)];

 
            ChatBubble.Create(playerTransform, new Vector3(0, 1.7f, 2.5f), icon, message);
        }, 5.5f);  
    }


    private string GetRandomMessage()
    {
        string[] messageArray = new string[] {
           "Laura, cada vez que sonríes, el mundo se detiene.",
            "César tiene la risa más contagiosa que he escuchado.",
            "Pol, tu forma de ser me hace creer en la bondad.",
            "Estar con Laura es como un sueño del que no quiero despertar.",
            "César, contigo los días grises parecen verano.",
            "Pol, tus palabras me acarician el alma.",
            "Laura, sin ti el café no sabe igual.",
            "César, eres caos, pero del que vale la pena.",
            "Pol, tu voz podría calmar cualquier tormenta.",
            "Laura, si fueras más falsa, vendrías con etiqueta.",
            "César, tu ego no cabe ni en este universo.",
            "Pol, ni con GPS encuentras una buena decisión.",
            "Laura, tu toxicidad es más fuerte que el café de oficina.",
            "César, deja de hablar de ti... el mundo ya sufre bastante.",
            "Pol, eres tan útil como una puerta giratoria en un submarino.",
            "Laura, contigo hasta el silencio suena molesto.",
            "César, lo tuyo no es personalidad... es advertencia.",
            "Pol, si las malas ideas fueran oro, serías millonario."
        };

        return messageArray[Random.Range(0, messageArray.Length)];
    }
}

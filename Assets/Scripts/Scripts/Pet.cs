using UnityEngine;

public class Pet : MonoBehaviour
{
    [Header("Estadísticas del Pet")]
    [SerializeField] private string petName;
    [SerializeField, Range(0, 100)] private int hunger = 100;
    [SerializeField, Range(0, 100)] private int grooming = 100;
    [SerializeField, Range(0, 100)] private int happiness = 100;
    [SerializeField] private int nuts = 0;

    // Propiedades públicas (Getters y Setters)
    public string Name
    {
        get => petName;
        set => petName = value;
    }

    public int Hunger
    {
        get => hunger;
        set => hunger = Mathf.Clamp(value, 0, 100);
    }

    public int Grooming
    {
        get => grooming;
        set => grooming = Mathf.Clamp(value, 0, 100);
    }

    public int Happiness
    {
        get => happiness;
        set => happiness = Mathf.Clamp(value, 0, 100);
    }

    public int Nuts
    {
        get => nuts;
        set => nuts = Mathf.Max(0, value);
    }

    // Puedes añadir métodos para modificar los valores
    public void Feed(int amount)
    {
        Hunger += amount;
        Debug.Log($"Se ha alimentado a {petName}, nueva hambre: {Hunger}");
    }

    public void Groom(int amount)
    {
        Grooming += amount;
        Debug.Log($"Se ha aseado a {petName}, nuevo grooming: {Grooming}");
    }

    public void Play(int amount)
    {
        Happiness += amount;
        Debug.Log($"Se ha jugado con {petName}, nueva felicidad: {Happiness}");
    }

    public void GiveNuts(int amount)
    {
        Nuts += amount;
        Debug.Log($"Se dieron {amount} nueces a {petName}. Total: {Nuts}");
    }

    public void SpendNuts(int amount)
    {
        if (Nuts >= amount)
        {
            Nuts -= amount;
            Debug.Log($"{petName} ha gastado {amount} nueces. Restantes: {Nuts}");
        }
        else
        {
            Debug.Log($"{petName} no tiene suficientes nueces.");
        }
    }
}

using UnityEngine;
using System.Collections;

public class iddleStmt : IPetStatement
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private PetManager petManager;
    private MonoBehaviour mono;

    private Coroutine animCoroutine;

    public iddleStmt() { }

    public iddleStmt(GameObject gameplayUI, Animator animator, PetManager petManager, MonoBehaviour mono)
    {
        this.animator = animator;
        this.gameplayUI = gameplayUI;
        this.petManager = petManager;
        this.mono = mono;
    }

    public void EnterState()
    {
        gameplayUI.SetActive(true);
        Debug.Log("La mascota está en modo Idle");

        // Animación inicial
        animator.Play("idle0");

        // Iniciar la corrutina para reproducir otra animación periódicamente
        animCoroutine = mono.StartCoroutine(AnimacionCadaNueveSegundos());
    }

    public void ExitState()
    {
        Debug.Log("Saliendo de Idle");

        // Detener la corrutina cuando se sale del estado
        if (animCoroutine != null)
        {
            mono.StopCoroutine(animCoroutine);
            animCoroutine = null;
        }
    }

    public void UpdateState()
    {
        // Aquí normalmente no necesitas nada para este comportamiento
    }

    private IEnumerator AnimacionCadaNueveSegundos()
    {
        while (true)
        {
            yield return new WaitForSeconds(9f);

            // Reproduce una animación secundaria
            animator.Play("iddle");

            // Opcional: vuelve a la animación idle0 después de un rato (si es corta la variación)
            yield return new WaitForSeconds(2f);
            animator.Play("idle0");
        }
    }
}

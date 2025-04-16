using UnityEngine;

public class Monster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShouldDie(collision);
    }

    private void ShouldDie(Collision2D collision)
    {
        bool isBird = collision.gameObject.GetComponent<Bird>();

        if (isBird)
        {
            Destroy(gameObject);
        }

        bool isCrushed = collision.contacts[0].normal.y < -0.5f;

        if (isCrushed)
        {
            Destroy(gameObject);
        }
    }
}

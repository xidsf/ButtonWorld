using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    [SerializeField] private Bomb bomb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player is in range");
            bomb.FireBomb();
        }
    }
}

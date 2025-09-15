using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    Collider2D goalColli;

    private void Start()
    {
        goalColli = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if(player != null)
            {
                player.OnCollisionGoal(goalColli);
                GameManager.Instance.ClearStage();
            }
        }
    }
}

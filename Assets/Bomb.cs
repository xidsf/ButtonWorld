using UnityEngine;

public class Bomb : MonoBehaviour
{
    GameObject gm;

    LayerMask LayerMask;
    float radius = 3f;

    private void Awake()
    {
        LayerMask = LayerMask.GetMask("Player", "Enemy");
    }

    public void FireBomb()
    {
        Debug.Log("bomb is fired");
        Invoke("Explode", 2f);
    }

    private void Explode()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask);
        foreach (Collider2D collider in hit)
        {
            Debug.Log(collider.name);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

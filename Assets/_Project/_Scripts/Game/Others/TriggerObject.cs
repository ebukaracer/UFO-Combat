using UnityEngine;

/// <summary>
/// Added to pickup objects which are marked as IsTrigger.
/// </summary>
internal class TriggerObject : MonoBehaviour
{
     [SerializeField] private Rigidbody2D rb2D;

    private void OnEnable()
    {
        rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    // Disables physics interaction when on collision with ground.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Ground))
            rb2D.bodyType = RigidbodyType2D.Static;
    }
}

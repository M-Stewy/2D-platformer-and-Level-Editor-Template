using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// currently unused, but should be easy to utilize
/// </summary>
public class AbilityPickUp : MonoBehaviour
{
    public UnityEvent PlayerTouch;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerTouch.Invoke();
            Destroy(gameObject);
        }
    }
}

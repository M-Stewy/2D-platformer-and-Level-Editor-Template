using UnityEngine;

public class RespawnPlayerScript : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Should respawn here");
            collision.gameObject.GetComponent<Player>().RespawnPlayer();
        }
    }
}

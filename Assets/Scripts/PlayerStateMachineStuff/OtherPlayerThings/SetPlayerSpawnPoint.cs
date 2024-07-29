using UnityEngine;

public class SetPlayerSpawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Should be Setting respawnPoint to " + transform.parent.transform);
            other.GetComponent<Player>().SetRespawnPoint(transform.parent.transform);
        }
    }
}

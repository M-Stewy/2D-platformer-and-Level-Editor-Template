using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
     //   if (collision.transform.CompareTag("enemy"))
     //   {
     //        collision.transform.GetComponentInParent<WHERE YOU PUT THE NAME OF THE SCRIPT YOU MADE>().ReceiveDamage();
     //   }

        Destroy(gameObject);
    }
}

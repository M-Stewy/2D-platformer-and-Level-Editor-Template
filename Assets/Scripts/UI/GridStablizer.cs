using UnityEngine;

public class GridStablizer : MonoBehaviour
{
    float xPos;
    float yPos;
    [SerializeField] Transform p;
    private void Update()
    {
        xPos = p.position.x;
        yPos = p.position.y;
        transform.position = new Vector3(Mathf.CeilToInt(xPos), Mathf.CeilToInt(yPos));
    }
}

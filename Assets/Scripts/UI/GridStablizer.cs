using UnityEngine;
/// <summary>
/// Ensures that the visible grid in the editor stays allinged
///     *Needs work, might replace with a shader or something later
/// </summary>
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

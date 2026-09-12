using UnityEngine;

public class PortalManger : MonoBehaviour
{
    [SerializeField] Transform connectedTo;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && connectedTo != null)
        {
            collision.transform.position = connectedTo.position;
            connectedTo.parent.gameObject.SetActive(true);
            Vector3 roomPos = connectedTo.parent.position;
            Camera.main.transform.position = new Vector3(roomPos.x, roomPos.y, Camera.main.transform.position.z);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}

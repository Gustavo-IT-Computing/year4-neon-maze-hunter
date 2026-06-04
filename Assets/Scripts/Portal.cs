using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPoint;

    private static bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTeleporting) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other));
        }
    }

    System.Collections.IEnumerator Teleport(Collider2D player)
    {
        isTeleporting = true;

        Vector3 offset = (exitPoint.right != Vector3.zero ? exitPoint.right : Vector3.up) * 0.8f;

        player.transform.position = exitPoint.position + offset;

        yield return new WaitForSeconds(0.3f);

        isTeleporting = false;
    }
}
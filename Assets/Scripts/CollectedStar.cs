using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StarCollector : MonoBehaviour
{
    [SerializeField] private StarManager starManager;
    [SerializeField] private float moveSpeed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // UI'daki sýradaki yýldýz pozisyonu
            Vector3 targetPos = starManager.StarsGameobjects[starManager.collectedCount].transform.position;
            
            // Coroutine ile hareket et
            StartCoroutine(MoveToUI(targetPos));
        }
    }

    private IEnumerator MoveToUI(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Manager'a bildir ve objeyi yok et
        starManager.CollectStar();
        Destroy(gameObject);
    }
}

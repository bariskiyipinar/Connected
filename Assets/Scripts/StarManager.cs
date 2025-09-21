using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    [Header("UI veya GameObject Y�ld�zlar�")]
    public GameObject[] StarsGameobjects;

    [Header("Finish Objesi")]
    public GameObject finishObject;

    public int collectedCount = 0;

    public void CollectStar()
    {
        collectedCount++;

        if (collectedCount <= StarsGameobjects.Length)
        {
            StarsGameobjects[collectedCount - 1].SetActive(true);
        }
        else
        {
            Debug.Log("Daha fazla y�ld�z yok!");
        }

     
        if (collectedCount >= StarsGameobjects.Length && finishObject != null)
        {
            finishObject.SetActive(true); // Finish�i aktif et
            Debug.Log("T�m y�ld�zlar topland�! Finish aktif oldu.");
        }
    }
}

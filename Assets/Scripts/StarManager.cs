using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    [Header("UI veya GameObject Yýldýzlarý")]
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
            Debug.Log("Daha fazla yýldýz yok!");
        }

     
        if (collectedCount >= StarsGameobjects.Length && finishObject != null)
        {
            finishObject.SetActive(true); // Finish’i aktif et
            Debug.Log("Tüm yýldýzlar toplandý! Finish aktif oldu.");
        }
    }
}

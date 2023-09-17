using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForClick());
    }

    IEnumerator WaitForClick()
    {
        while (Input.GetButtonDown("Fire1") == false)
        {
            yield return null;
        }
        Debug.Log("Clicked");
    }
}

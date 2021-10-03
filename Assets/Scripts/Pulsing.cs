using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoPulsing());
    }

    private IEnumerator DoPulsing()
    {
        while (true)
        {
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.4f);

            yield return new WaitForSeconds(0.5f);

            transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
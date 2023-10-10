using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour
{
    [SerializeField] private GameObject lantarnObject;

    private void OnTriggerEnter(Collider other)
    {
        lantarnObject.GetComponent<Light>().canLight = true;
        lantarnObject.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.5f);

    }
}

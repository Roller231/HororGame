using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    [SerializeField] private GameObject light;
    [SerializeField] private AudioSource lightSource;

    public bool on_of;
    public bool canLight;


    public void FlipFLopLight()
    {
        if (!canLight) return;
        if (!on_of)
        {
            light.gameObject.SetActive(false);
            lightSource.Play();
            on_of = true;

        }
        else
        {
            light.gameObject.SetActive(true);
            lightSource.Play();
            on_of = false;
        }
    }
}

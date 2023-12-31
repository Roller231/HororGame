﻿using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UtilScripts : MonoBehaviour
{

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static void SetDateTime(string key, DateTime value)
    {
        string convertedToString = value.ToString(format: "u", CultureInfo.InvariantCulture);
        PlayerPrefs.SetString(key, convertedToString);
    }

    public static DateTime GetDateTime(string key, DateTime value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string stored = PlayerPrefs.GetString(key);
            DateTime result = DateTime.ParseExact(s: stored, format: "u", CultureInfo.InvariantCulture);
            return result;
        }
        else
        {
            return value;
        }
        
    }
    

    public static void DestroyObjectWithComponent3D()
    {
        MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
        SphereCollider[] sphereRenders = FindObjectsOfType<SphereCollider>();


        // Óäàëÿåì êàæäûé îáúåêò ñ êîìïîíåíòîì MeshRenderer
        foreach (MeshRenderer renderer in meshRenderers)
        {
            Destroy(renderer.gameObject);
        }

        foreach (SphereCollider renderer in sphereRenders)
        {
            Destroy(renderer.gameObject);
        }
    }


    public static void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public static void OpenSceneVoid(int index)
    {
        SceneManager.LoadScene(index);
    }



    public static void PlaySounds(string nameAudio, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + nameAudio);
        AudioSource source = GameObject.Find("Sounds").GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public static void PlaySoundsWithAUS(string nameAudio, string nameSource, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + nameAudio);
        AudioSource source = GameObject.Find(nameSource).GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public void PlaySoundsButton(string nameAudio)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + nameAudio);
        AudioSource source = GameObject.Find("Sounds").GetComponent<AudioSource>();

        source.clip = clip;
        source.Play();
    }

}
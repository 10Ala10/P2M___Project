using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image image;
    [SerializeField] private Sprite _default, pressed;
    [SerializeField] private AudioClip compressClip, uncompressClip;
    [SerializeField] private AudioSource source;
    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressed;
        source.PlayOneShot(compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = _default;
        source.PlayOneShot(uncompressClip);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level_Selection");
    }




}

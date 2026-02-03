using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject winVfx;
    [SerializeField] private GameObject loseVfx;
    private void Awake()
    {
        gameManager.ResultEvent.AddListener(GetResult);
    }

    private void GetResult(bool arg0)
    {
        winVfx.gameObject.SetActive(arg0);
        loseVfx.gameObject.SetActive(!arg0);
    }
}

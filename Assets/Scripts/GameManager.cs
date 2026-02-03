using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winSprite;
    [SerializeField] private GameObject loseSprite;

    [Range(0,1)]
    [SerializeField] private float winProbability;
    
    [Header("Results")]
    [HideInInspector] public UnityEvent<bool> ResultEvent;
    public UnityEvent WinEvent;
    public UnityEvent LoseEvent;
    
    private bool gameResult = false;
    private void Start()
    {
        gameResult = Random.value > winProbability;
        var _result = gameResult ? winSprite : loseSprite;
        _result.SetActive(true);
    }

    public void GameResult()
    {
        ResultEvent?.Invoke(gameResult);
        
        var _event =  gameResult ? WinEvent : LoseEvent;
        _event?.Invoke();
    }
}

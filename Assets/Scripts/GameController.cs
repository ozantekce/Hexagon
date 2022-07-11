using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController Instance { get => instance; set => instance = value; }
    
    public GameStatus GameStatus { get => _gameStatus; set => _gameStatus = value; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private GameStatus _gameStatus;



}

public enum GameStatus
{
    menu,
    waitForMove,
    moving,
    finish,

}

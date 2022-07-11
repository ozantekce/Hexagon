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

    private void Start()
    {
        CreateNewLevel();
    }



    private int numberOfObstacles = 12;
    private void CreateNewLevel()
    {

        Obstacle.ResetStaticValues();
        GameObject obstacle;

        for (int i = 0; i < numberOfObstacles; i++)
        {
            obstacle = GameObject.Instantiate(Resources.Load("Obstacle") as GameObject);

            Debug.Log(Obstacle.LastObstacle.transform.position.z);
        }

        GameObject finish = GameObject.Instantiate(Resources.Load("Finish") as GameObject);
        finish.transform.position = new Vector3(0,0,Obstacle.LastObstacle.transform.position.z+30f);
        finish.transform.parent = Hexagon.Instance.transform;


    }

}






public enum GameStatus
{
    menu,
    waitForMove,
    moving,
    finish,

}

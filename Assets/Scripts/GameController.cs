using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController Instance { get => instance; set => instance = value; }
    public GameStatus CurrentStatus { get => _currentStatus; }

    private GameObject mainMenuScreen;
    private GameObject gameplay;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private GameStatus _currentStatus;

    private void Start()
    {
        mainMenuScreen = GameObject.Find("MainMenuScreen") as GameObject;
        Button playButton = mainMenuScreen.GetComponentInChildren<Button>();
        playButton.onClick.AddListener(PlayButton);

        gameplay = GameObject.Find("Gameplay");
        gameplay.SetActive(false);
        CreateNewLevel();

        gameplay.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(PauseButton);
    }


    private void Update()
    {
        


    }


    private void PlayButton()
    {
        ChangeGameStatus(GameStatus.waitToPlay);
    }

    private void PauseButton()
    {
        if (CurrentStatus == GameStatus.playing)
            ChangeGameStatus(GameStatus.paused);
        else if(CurrentStatus == GameStatus.paused)
            ChangeGameStatus(GameStatus.playing);
    }

    private int numberOfObstacles = 12;
    private int numberOfWindows = 10;
    private void CreateNewLevel()
    {

        Obstacle.ResetStaticValues();
        GameObject obstacle;

        for (int i = 0; i < numberOfObstacles; i++)
        {
            obstacle = GameObject.Instantiate(Resources.Load("Obstacle") as GameObject);

        }

        GameObject finish = GameObject.Instantiate(Resources.Load("Finish") as GameObject);
        finish.transform.position = new Vector3(0,0,Obstacle.LastObstacle.transform.position.z+30f);
        finish.transform.parent = Hexagon.Instance.transform;

        GameObject window;
        for (int i = 0; i < numberOfWindows; i++)
        {
            window = GameObject.Instantiate(Resources.Load("Window") as GameObject);
        }


    }





    public void ChangeGameStatus(GameStatus nextStatus)
    {

        if(_currentStatus == GameStatus.menu)
        {
           if(nextStatus == GameStatus.waitToPlay)
            {
                mainMenuScreen.SetActive(false);
                gameplay.SetActive(true);
                _currentStatus = GameStatus.waitToPlay;
            }
        }
        else if(_currentStatus == GameStatus.waitToPlay)
        {
            if(nextStatus == GameStatus.playing)
            {
                _currentStatus=GameStatus.playing;
            }
        }
        else if(_currentStatus == GameStatus.playing)
        {
            if(nextStatus == GameStatus.paused)
            {
                _currentStatus = GameStatus.paused;
                Time.timeScale = 0;
            }
            else if(nextStatus == GameStatus.died)
            {
                _currentStatus = GameStatus.died;
            }
            else if(nextStatus == GameStatus.levelUp)
            {
                _currentStatus = GameStatus.levelUp;
            }
        }
        else if(_currentStatus == GameStatus.paused)
        {

            if(nextStatus == GameStatus.playing)
            {
                _currentStatus = GameStatus.playing;
                Time.timeScale = 1;
            }
            else if(nextStatus == GameStatus.menu){

                _currentStatus = GameStatus.menu;
                gameplay.SetActive(false);
                mainMenuScreen.SetActive(true);
                Time.timeScale = 1;

            }

        }
        else if(_currentStatus == GameStatus.died)
        {
            if (nextStatus == GameStatus.menu)
            {
                _currentStatus = GameStatus.menu;
                gameplay.SetActive(false);
                mainMenuScreen.SetActive(true);
                //FLASH EFFECT

                Hexagon.Instance.transform.rotation = Quaternion.identity;

                Ball.Instance.transform.position = new Vector3(0, -5.5f, 5);
                Ball.Instance.transform.localScale = Vector3.one;
                

                for (int i = 1; i < Hexagon.Instance.transform.childCount; i++)
                {
                    Destroy(Hexagon.Instance.transform.GetChild(i).gameObject);
                }
                CreateNewLevel();
                Ball.Instance.MeshRenderer.enabled = true;
                Ball.Instance.Collider.enabled = true;
                Ball.Instance.ChangeMaterial(Ball.Instance.GetRandomMaterial());
                
            }
        }
        else if(_currentStatus == GameStatus.levelUp)
        {
            if (nextStatus == GameStatus.waitToPlay) 
            {

                _currentStatus = GameStatus.waitToPlay;
                //FLASH EFFECT

                Hexagon.Instance.transform.rotation = Quaternion.identity;

                Ball.Instance.transform.position = new Vector3(0, -5.5f, 5);
                Ball.Instance.transform.localScale = Vector3.one;


                for (int i = 1; i < Hexagon.Instance.transform.childCount; i++)
                {
                    Destroy(Hexagon.Instance.transform.GetChild(i).gameObject);
                }
                CreateNewLevel();
                Ball.Instance.MeshRenderer.enabled = true;
                Ball.Instance.Collider.enabled = true;
                Ball.Instance.ChangeMaterial(Ball.Instance.GetRandomMaterial());
            }

        }

    }




}






public enum GameStatus
{
    menu,
    waitToPlay,
    playing,
    paused,
    died,
    levelUp,

}

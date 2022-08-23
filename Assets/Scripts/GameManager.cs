using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace WilhelmAndFriends {

    public class GameManager : MonoBehaviour
    {
/*
        private static GameManager _instance;

        public static GameManager Instance {

            get
            {
                if(_instance == null)
                {
                    Debug.Log("GameManager Instance is null!");


                }

               return _instance;
            }

         }



        public delegate int CheckpointDelegate();

        public static event CheckpointDelegate CheckPointEvent;


        public delegate void CameraDelegate(GameObject player);

        public static event CameraDelegate CameraToPositionEvent;



        public delegate void PlayerSetupDelegate(GameObject player);

        public static event PlayerSetupDelegate GetPlayerInstanceEvent;


       //make an event that sends the newly created player as parameter to
       // the listeners of the event
       //so to the camera manager.




        [SerializeField] Transform[] CheckPoints;
        private int currentCheckPoint;
        // SoundManager soundManager;
        [SerializeField] GameObject wilhelm;
        GameObject player;
        private int playerLives;
        public bool isDead;
        public int health;
        // Wilhelm wilhelmScript;
        Vector2 checkPointPosition;
        public bool isGameOver;
        public bool hasEatenObject;
        // Start is called before the first frame update'
        private void Awake()
        {
            //wilhelm = GameObject.Find("Wilhelm");
            _instance = this;

                

            //wilhelmScript = wilhelm.GetComponent<Wilhelm>();
            //wilhelmScript.enabled = true;

        }
        void Start()
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            //soundManager.Play("Music");

      


        }

        private void OnEnable()
        {
            Init();
            Wilhelm.OnDeathEvent += TakePlayerToCheckPoint;
       
        }



        // Update is called once per frame
        void Update()
        {

            IsPlayerDead();
          

        }


        private void IsPlayerDead()
        {
     
            if (isDead == true) {
                //Debug.Log("is player dead ." + isDead);
                Wilhelm.OnDeathEvent -= TakePlayerToCheckPoint;
                isDead = false;
            }
            if(isDead == false)
            {
                Wilhelm.OnDeathEvent += TakePlayerToCheckPoint;

            }

        }


        public void CheckPointUpdate()
        {
            if(CheckPointEvent !=null)
            {

                currentCheckPoint = CheckPointEvent();
                //Debug.Log("CurrentCheckPoint is:" + currentCheckPoint);

                //Debug.Log(" current check" + currentCheckPoint);


            }

        }







        private void Init()
        {
            currentCheckPoint = 1;
            hasEatenObject = false;
        }



        private void InstantiatePlayer()
        {
            checkPointPosition = CheckPoints[currentCheckPoint-1].transform.position;

            if (!player) { 
            player = Instantiate(wilhelm, checkPointPosition, wilhelm.transform.rotation);

            }


            if (player != null)
            {
                //CameraToPositionEvent(player);
                //Event Invokes an event.
                //ResetPlayer(player);

            }


        }

        public void ResetPlayer()
        {

            //UI lives needs updating
            //PLayer health to full
            //position to first checkpoint of the level
            //Current checkpoint need to be set back to 1;
            currentCheckPoint = 1;
            UImanager.Instance.UpdateLives2(health);


            //TakePlayerToCheckPoint();
            //do reseting
            // also call TakePlayerToCheckPoint;
        }


        //When playerHealth == 0
        //Listen the player GameOver event.
        // if it is fired: 
        //isGameOver = true



        private Vector3 TakePlayerToCheckPoint()
        {
     
            //if (wilhelmScript.Health == 1)
            //{
            //    checkPointPosition = CheckPoints[0].position;

            //}

            //if(wilhelmScript.Health > 1){
               

            //}

            checkPointPosition = CheckPoints[currentCheckPoint - 1].transform.position;


            //Debug.Log("Take player to checkPoint: " + checkPointPosition);
            return checkPointPosition;
        }


    

        public void StateOfHealth(int playerHealth)
        {

            health = playerHealth;

        }

  

*/

    }
}
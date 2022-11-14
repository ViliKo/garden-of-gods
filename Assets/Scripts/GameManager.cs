using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace GOG {


    public class GameManager : MonoBehaviour
    {

        // public variables here
        GameObject player;
        public GameObject globalLight;
        GameObject environment;
        new GameObject camera;

        // Death logic here
        public int respawn;
        
       // signleton pattern here
        public static GameManager Instance { get; private set; }

        // !!!!!!!!!!!!!! if is enemy dont put health system here !!!!!!!!!!!!!!!
        // Add health system to a player of 100 units

        // :::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // MOVE THIS LINE TO ENEMY SCRIPTS RATHER THAN DOING IT HERE
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public UnitHealth playerHealth = new UnitHealth(100, 100);


        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;


            InstantiateEverything();

            // save game objects if it needs editing
            environment = GameObject.Find("environment");
            camera = GameObject.Find("cm-camera");
            player = GameObject.Find("player");
        } // function

        private void Update()
        {
            if (playerHealth.Health == 50)
            {
            }
        }


        private void InstantiateEverything()
        {
            // Make the light come alive only after starting the game
           Instantiate(globalLight, transform.position, Quaternion.identity);
        }

    } //class
} // namespace
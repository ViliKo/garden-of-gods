using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOG {
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] HealthBar _healthBar;
        Controller2D controller;
        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player Behaviour has started");
            controller = GetComponent<Controller2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // In this update you choose the actions that decrease or increase health
            if (GameManager.Instance.playerHealth.Health == 100)
            {
                PlayerTakeDamage(50);
                Debug.Log(GameManager.Instance.playerHealth.Health);
            }

            if (controller.collisions.death == true)
            {
                animator.SetBool("is-dead", true);
                // Put this couroutin to a better place in the end
                StartCoroutine(DeathEvent());
            }
        }

        IEnumerator DeathEvent()
        {

            
            //Wait for 4 seconds
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("start-screen");

        }

        // Method on how the player takes damage
        private void PlayerTakeDamage(int dmg) {
            // In game manager there is an object instatiated that has a health system
            // /Systems/UnitHealth.cs
            GameManager.Instance.playerHealth.DamageUnit(dmg);
            _healthBar.SetHealth(GameManager.Instance.playerHealth.Health);   
        }


        // Method on how the player heals damage
        private void PlayerHeal(int heal)
        {
            // In game manager there is an object instatiated that has a health system
            // /Systems/UnitHealth.cs
            GameManager.Instance.playerHealth.HealUnit(heal);
            _healthBar.SetHealth(GameManager.Instance.playerHealth.Health);
        }
    }

}
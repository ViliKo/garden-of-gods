using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Grab the Unity engine Ui Namespace
using UnityEngine.UI;

namespace GOG { 

    public class HealthBar : MonoBehaviour
    {

        // Make a new empty gameobject of slider
        Slider _healthSlider;

        private void Start()
        {
            // Get the component of health slider to the empty game object
            _healthSlider = GetComponent<Slider>();
        }


        // Set its max value and value
        public void SetMaxHealth(int maxHealth)
        {
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = maxHealth;
        }


        // Set health bar length by value
        public void SetHealth(int health)
        {
            _healthSlider.value = health;
        }
    }

    

}
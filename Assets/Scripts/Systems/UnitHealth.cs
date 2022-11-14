using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOG {

    // This class was made because the game needs an health system
    // That can be easily given to all the enemies and bosses of the game
    public class UnitHealth
    {
        // Fields
        int _currentHealth;
        int _currentMaxHealth;

        // Properties
        public int Health 
        {
            get
            {
                return _currentHealth;
            }
            set 
            {
                _currentHealth = value;
            }
        } // property

        public int MaxHealth
        {
            get
            {
                return _currentMaxHealth;
            }
            set
            {
                _currentMaxHealth = value;
            }
        } // property

        // Constructor
        public UnitHealth(int health, int maxHealth) {
            _currentHealth = health;
            _currentMaxHealth = maxHealth;
        } // constructor

        // Methods
        public bool DamageUnit(int damageAmount)
        {
            // if current health is less than zero dont take damage
            if (_currentHealth <= 0) { return false; }
            _currentHealth -= damageAmount;

            return true;
        }

        public bool HealUnit(int healAmount)
        {
            // if current health is more or equal to max health dont heal
            if (_currentHealth >= _currentMaxHealth) { return false; }
            _currentHealth += healAmount;

            // If current health is bigger than max health then set current health to max health
            if (_currentHealth > _currentMaxHealth)
                _currentHealth = _currentMaxHealth;

            return true;
        } // method
    } // class 
}// namespace
﻿using System.Collections;
using System.Collections.Generic;
using HauntedCity.GameMechanics.BattleSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class IndicatorsView : MonoBehaviour
    {
        //private PlayerBattleController _battleController;

        private PlayerBattleBehavior _player;

        [Inject]
        public void InitializeDependencies(PlayerBattleBehavior player)
        {
            _player = player;
        }
        
        private Transform _energy;
        private Transform _health;

        private Transform _energyBar;
        private Transform _healthBar;

        // Use this for initialization
        void Start()
        {
            //_player.BattleController = GameObject.FindWithTag("Player").GetComponent<PlayerBattleBehavior>().BattleController;

            _player.BattleController.OnEnergyChanged += UpdateEnergy;
            _player.BattleController.OnDamage += UpdateHealth;
            _player.BattleController.OnReset += ResetHandle;

            _health = transform.Find("HealthPanel");
            _energy = transform.Find("EnergyPanel");

            _healthBar = _health.Find("HealthBar");
            _energyBar = _energy.Find("EnergyBar");
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnDestroy()
        {
            _player.BattleController.OnEnergyChanged -= UpdateEnergy;
            _player.BattleController.OnDamage -= UpdateHealth;
            _player.BattleController.OnReset -= ResetHandle;
        }

        public void UpdateEnergy(int delta)
        {
            var currentEnergy = _player.BattleController.BattleStats.CurrentEnergy;
            var maxEnergy = _player.BattleController.BattleStats.MaxEnergy;
            _energy.GetComponentInChildren<Text>().text = currentEnergy + "/" + maxEnergy;

            var energyPercent = (float) currentEnergy / maxEnergy;
            _energyBar.GetComponent<Image>().fillAmount = energyPercent;
        }

        public void UpdateHealth(int delta)
        {
            var currentHealth = _player.BattleController.BattleStats.CurrentHealth;
            var maxHealth = _player.BattleController.BattleStats.MaxHealth;
            _health.GetComponentInChildren<Text>().text = currentHealth + "/" + maxHealth;

            var healthPercent = (float) currentHealth / maxHealth;
            _healthBar.GetComponent<Image>().fillAmount = healthPercent;
        }

        public void ResetHandle()
        {
            UpdateEnergy(0);
            UpdateHealth(0);
        }
    }
}
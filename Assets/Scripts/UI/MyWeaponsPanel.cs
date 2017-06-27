﻿using System.Collections.Generic;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI
{
    public class MyWeaponsPanel:MonoBehaviour
    {
        public GameObject WeaponCardPrefab;
        public Transform CardContainer;
        
        
        [Inject] private WeaponLoader _weaponLoader;
        private readonly Dictionary<string, WeaponCard> _weaponCards = new Dictionary<string, WeaponCard>();
        private List<string> _playerWeapons;
        
        private void Start()
        {
            _playerWeapons = GameController.GameStats.AllowableWeapons;
            if (_weaponLoader == null)
            {
                _weaponLoader = new WeaponLoader();
            }
            UpdateView();
        }
      
        public void UpdateView()
        {            
            foreach (var weapon in _playerWeapons)
            {
                if (_weaponCards.ContainsKey(weapon))
                {
                    _weaponCards[weapon].UpdateView();
                }
                else
                {
                    DrawWeaponCard(_weaponLoader.LoadWeapon(weapon));
                }
            } 
        }
        

        
        
        private void DrawWeaponCard(Weapon weapon)
        {
            var weaponCard = Instantiate(WeaponCardPrefab);
            weaponCard.transform.SetParent( CardContainer, false);
            
            var weaponCardComponent =weaponCard.GetComponent<WeaponCard>();
            weaponCardComponent.UpdateView(weapon);
            _weaponCards.Add(weapon.Id, weaponCardComponent);

        }
    }
}
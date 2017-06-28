﻿using GameSparks.Api.Responses;
using HauntedCity.Networking;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class LoginPanel : Panel
    {
        public Panel MainMenu;

        public GameObject ErrorField;
        private InputField _login;
        private InputField _password;
        
        private ScreenManager _screenManager;
        private StorageService _storageService;
        
        [Inject]
        public void InitializeDependencies(ScreenManager screenManager, StorageService storageService)
        {
            _screenManager = screenManager;
            _storageService = storageService;
        }
        
        private void Start()
        {
            _login = transform.Find("LoginForm/Login").GetComponent<InputField>();
            _password = transform.Find("LoginForm/Password").GetComponent<InputField>();
        }

        public void Login()
        {
            AuthService.Instance.Login(
                _login.text,
                _password.text
            );
        }

        private void OnEnable()
        {
            AuthService.Instance.OnLogin += OnLogin;
        }
        
        private void OnDisable()
        {
            AuthService.Instance.OnLogin -= OnLogin;
        }

        protected override void OnShow()
        {
            ErrorField.SetActive(false);
        }

        public void OnLogin(AuthenticationResponse response)
        {
            if (!response.HasErrors)
            {
                (new LeaderboardService()).GetLeaderboard();

                ShowInstead(MainMenu);
                _storageService.LoadPlayer();
            }
            else
            {
                ErrorField.SetActive(true);
            }
        }
    }
}
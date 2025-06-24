﻿using DG.Tweening;
using SimpleSolitaire.Model.Config;
using SimpleSolitaire.Model.Enum;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SimpleSolitaire.Controller
{
    public abstract class GameManager : MonoBehaviour
    {
        [Header("Ads Components:")]
        public GameObject AdsBtn;
        [SerializeField]
        private GameObject _adsLayer;
        [SerializeField]
        private GameObject _watchButton;
        [SerializeField]
        private Text _adsInfoText;
        [SerializeField]
        private Text _adsDidNotLoadText;
        [SerializeField]
        private Text _adsClosedTooEarlyText;

        public string NoAdsInfoText = "DO YOU WANNA TO DEACTIVATE ALL ADS FOR THIS GAME SESSION? JUST WATCH LAST REWARD VIDEO AND INSTALL APP. THEN ADS WON'T DISTURB YOU AGAIN!";
        public string GetUndoAdsInfoText = "DO YOU WANNA TO GET FREE UNDO COUNTS? JUST WATCH REWARD VIDEO AND INSTALL APP. THEN UNDO WILL ADDED TO YOUR GAME SESSION!";

        [Header("Components:")]
        [SerializeField]
        protected CardLogic _cardLogic;
        // [SerializeField]
        // private AdsManager _adsManagerComponent;
        [SerializeField]
        private CongratulationManager _congratManagerComponent;
        [SerializeField]
        private UndoPerformer _undoPerformComponent;
        [SerializeField]
        private AutoCompleteManager _autoCompleteComponent;
        [SerializeField]
        protected StatisticsController _statisticsComponent;
        [SerializeField]
        private HowToPlayManager _howToPlayComponent;
        [SerializeField]
        private OrientationManager _orientationManager; 
        
        [Header("Layers:")]
        [SerializeField]
        protected GameObject _gameLayer;
        [SerializeField]
        protected GameObject _cardLayer;
        [SerializeField]
        private GameObject _winLayer;
        [SerializeField]
        private GameObject _settingLayer;
        [SerializeField]
        private GameObject _statisticLayer;
        [SerializeField]
        private GameObject _exitLayer;
        [SerializeField]
        private GameObject _continueLayer;
        [SerializeField]
        private GameObject _howToPlayLayer;

        [Header("Labels:")]
        [SerializeField]
        private Text _timeLabel;
        [SerializeField]
        private Text _scoreLabel;
        [SerializeField]
        private Text _stepsLabel;
        [SerializeField]
        private Text _timeWinLabel;
        [SerializeField]
        private Text _scoreWinLabel;
        [SerializeField]
        private Text _stepsWinLabel;

        [Header("Switchers:")]
        [SerializeField]
        private SwitchSpriteComponent _soundSwitcher;
        [SerializeField]
        private SwitchSpriteComponent _bgmSwitcher, _hapticSwitcher;


        [SerializeField]
        private SwitchSpriteComponent _autoCompleteSwitcher;
        [SerializeField]
        private SwitchSpriteComponent _orientationSwitcher;
        [SerializeField]
        private SwitchSpriteComponent _highlightDraggableSwitcher;
        [SerializeField]
        private TextSwitchSpriteComponent _screenOrientationSwitcher;
        
        [Header("Settings:")]
        public bool UseLoadLastGameOption;

        public int TimeCount => _timeCount;
        public int StepCount => _stepCount;
        public int ScoreCount => _scoreCount;

        private readonly string _appearTrigger = "Appear";
        private readonly string _disappearTrigger = "Disappear";
        private readonly string _bestScoreKey = "WinBestScore";

        private int _timeCount;
        private int _stepCount;
        private int _scoreCount;
        private Coroutine _timeCoroutine;
        private AudioController _audioController;
        public AudioSource _bgmController;

        private RewardAdsType _currentAdsType = RewardAdsType.None;

        private bool _highlightDraggableEnable;
        private bool _soundEnable
        {
            get => Data.sfx;
            set => Data.sfx = value;
        }

        private bool _bgmEnable
        {
            get => Data.music;
            set => Data.music = value;
        }


        private bool _hapticEnable
        {
            get => Data.haptic;
            set => Data.haptic = value;
        }



        private bool _autoCompleteEnable;

        protected float _windowAnimationTime = 0.0f;
        
        private bool _isShouldOpenSettings = false;
        
        private void Awake()
        {
            InitializeGame();
        }

        /// <summary>
        /// Initialize game structure.
        /// </summary>
        protected virtual void InitializeGame()
        {
            Application.targetFrameRate = 300;

            _undoPerformComponent.Initialize();

            // _soundEnable = true;
            _autoCompleteEnable = true;

            // _adsManagerComponent.RewardAction += OnRewardActionState;
            _cardLogic.SubscribeEvents();
            _audioController = AudioController.Instance;

            UpdateSFX();
            UpdateBGM();
            UpdateHaptic();
        }

        private void Start()
        {
            InitGameState();
            InitOrientationStateSwitch();
        }

        /// <summary>
        /// Appear window with animation.
        /// </summary>
        protected void AppearWindow(GameObject window)
        {
            if (window == null)
            {
                return;
            }

            var rect = window.GetComponentInChildren<OrientationLayerScaler>().GetComponent<RectTransform>();
            if (rect)
            {
                rect.pivot = new Vector2(0.5f, 0f);
                rect.transform.DOKill();

                rect.transform.DOScale(0f, 0.0f);
                rect.transform.DOScale(1f, 0.35f);
            }

            if (_audioController != null)
            {
                _audioController.Play(AudioController.AudioType.WindowOpen);
            }

        }

        /// <summary>
        /// Disappear window with animation.
        /// </summary>
        protected void DisappearWindow(GameObject window, Action onDisappear)
        {
            if (window == null)
            {
                return;
            }

            var rect = window.GetComponentInChildren<OrientationLayerScaler>().GetComponent<RectTransform>();
            if (rect)
            {
                rect.pivot = new Vector2(0.5f, 0f);

                rect.transform.DOKill();
                rect.transform.DOScale(1f, 0.0f);
                rect.transform.DOScale(0f, 0.35f);

            }

            if (_audioController != null)
            {
                _audioController.Play(AudioController.AudioType.WindowClose);
            }

            StartCoroutine(InvokeAction(onDisappear, _windowAnimationTime));
        }
        
        /// <summary>
        /// Show how to play window with animation.
        /// </summary>
        public void ShowHowToPlayLayer()
        {
            _cardLayer.SetActive(true);
            _howToPlayLayer.SetActive(true);
            AppearWindow(_howToPlayLayer);
        }

        /// <summary>
        /// Hide how to play layer with animation.
        /// </summary>
        public void HideHowToPlayLayer()
        {
            DisappearWindow(_howToPlayLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _howToPlayLayer.SetActive(false);
            }
        }

        /// <summary>
        /// Init new game state or show continue game panel.
        /// </summary>
        private void InitGameState()
        {
            InitSettingBtns();

            if (UseLoadLastGameOption && _howToPlayComponent.IsHasKey() && _undoPerformComponent.IsHasGame())
            {
                _cardLayer.SetActive(false);
                _continueLayer.SetActive(true);
                AppearWindow(_continueLayer);
            }
            else
            {
                _cardLogic.InitCardLogic();
                _cardLogic.Shuffle(false);
                InitMenuView(false);
            }
        }

        /// <summary>
        /// Change position of bottom panel. Used for ads banner.
        /// </summary>
        /// <param name="offset"></param>
        public void OnNoAdsRewardedUser()
        {
            OnClickAdsCloseBtn();
            AdsBtn.SetActive(false);
        }

        private void OnDestroy()
        {
            // _adsManagerComponent.RewardAction -= OnRewardActionState;

            _cardLogic.UnsubscribeEvents();
        }

        /// <summary>
        /// Initialize first state of UI elements. And first timer state.
        /// </summary>
        private void InitMenuView(bool isLoadGame)
        {
            _timeCount = isLoadGame ? _undoPerformComponent.StatesData.Time : 0;
            SetTimeLabel(_timeCount);
            _stepCount = isLoadGame ? _undoPerformComponent.StatesData.Steps : 0;
            _stepsLabel.text = _stepCount.ToString();
            _scoreCount = isLoadGame ? _undoPerformComponent.StatesData.Score : 0;
            _scoreLabel.text = _scoreCount.ToString();
            StopGameTimer();
        }

        /// <summary>
        /// Deactivate <see cref="AdsBtn"/> button if we show <see cref="AdsManager.ShowRewardBasedVideo"/> this ads material.
        /// </summary>
        private void InitSettingBtns()
        {
            // if (_adsManagerComponent.IsHasKeyNoAds())
            // {
                AdsBtn.SetActive(false);
            // }
        }

        /// <summary>
        /// Update <see cref="_timeLabel"/> view text.
        /// </summary>
        /// <param name="seconds"></param>
        private void SetTimeLabel(int seconds)
        {
            int sec = seconds % 60;
            int min = (seconds % 3600) / 60;
            _timeLabel.text = string.Format("{0,2}:{1,2}", min.ToString().PadLeft(2, '0'), sec.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Win game action.
        /// </summary>
        public void HasWinGame()
        {
            _cardLayer.SetActive(false);
            _winLayer.SetActive(true);

            StopGameTimer();
            _congratManagerComponent.CongratulationTextFill();
            var score = _scoreCount + Public.SCORE_NUMBER / _timeCount;
            _timeWinLabel.text = "YOUR TIME: " + _timeLabel.text;
            _scoreWinLabel.text = "YOUR SCORE: " + score;
            _stepsWinLabel.text = "YOUR MOVES: " + _stepCount;

            if (_audioController != null)
            {
                _audioController.Play(AudioController.AudioType.Win);
            }

            SetBestValuesToPrefs(score);

            AppearWindow(_winLayer);

            _statisticsComponent.IncreasePlayedGamesTime(_timeCount);
            _statisticsComponent.GetAverageGameTime();
            _statisticsComponent.IncreaseScoreAmount(score);
            _statisticsComponent.IncreaseWonGamesAmount();
            _statisticsComponent.SetBestWinTime(_timeCount);
            _statisticsComponent.SetBestWinMoves(_stepCount);

            // _adsManagerComponent.ShowInterstitial();
        }

        /// <summary>
        /// Save to prefs best score if it need :)
        /// </summary>
        /// <param name="score">Score value</param>
        private void SetBestValuesToPrefs(int score)
        {
            if (!PlayerPrefs.HasKey(_bestScoreKey))
            {
                PlayerPrefs.SetInt(_bestScoreKey, score);
            }
            else
            {
                if (score > PlayerPrefs.GetInt(_bestScoreKey))
                {
                    PlayerPrefs.SetInt(_bestScoreKey, score);
                }
            }
        }

        /// <summary>
        /// Click on new game button.
        /// </summary>
        public void OnClickWinNewGame()
        {
            DisappearWindow(_winLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _winLayer.SetActive(false);
                _cardLayer.SetActive(!_statisticLayer.activeInHierarchy && !_winLayer.activeInHierarchy);
            }

            _cardLogic.Shuffle(false);
            _undoPerformComponent.ResetUndoStates();
            _statisticsComponent.IncreasePlayedGamesAmount();
        }

        /// <summary>
        /// Click on play button in bottom setting layer.
        /// </summary>
        public void OnClickPlayBtn()
        {
            _cardLayer.SetActive(false);
            AppearGameLayer();
        }

        protected void AppearGameLayer()
        {
            _gameLayer.SetActive(true);
            InitCardLogic();
            AppearWindow(_gameLayer);
        }

        protected abstract void InitCardLogic();

        #region Continue Layer
        /// <summary>
        /// Click on play button in bottom setting layer.
        /// </summary>
        private void LoadGame()
        {
            InitSettingBtns();

            _cardLogic.InitCardLogic();

            _undoPerformComponent.LoadGame();

            _cardLogic.OnNewGameStart();

            InitMenuView(true);
        }

        /// <summary>
        /// Start new game.
        /// </summary>
        public void OnClickContinueNoBtn()
        {
            DisappearWindow(_continueLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                //Uncomment if you wanna clear last game when User click No button on Continue Layer.
                //_undoPerformComponent.DeleteLastGame();
                _cardLogic.InitCardLogic();
                _cardLogic.Shuffle(false);
                _continueLayer.SetActive(false);
                _cardLayer.SetActive(true);
            }
        }

        /// <summary>
        /// Continue last game.
        /// </summary>
        public void OnClickContinueYesBtn()
        {
            DisappearWindow(_continueLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                LoadGame();
                _continueLayer.SetActive(false);
                _cardLayer.SetActive(true);
            }
        }
        #endregion

        #region Exit Layer
        /// <summary>
        /// Click on Exit button.
        /// </summary>
        public void OnClickExitBtn()
        {
            _cardLayer.SetActive(false);
            _exitLayer.SetActive(true);
            AppearWindow(_exitLayer);
        }

        /// <summary>
        /// Close <see cref="_adsLayer"/>.
        /// </summary>
        public void OnClickExitNoBtn()
        {
            DisappearWindow(_exitLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _exitLayer.SetActive(false);
                _cardLayer.SetActive(true);
            }
        }

        /// <summary>
        /// Quit application. Exit game.
        /// </summary>
        public void OnClickExitYesBtn()
        {
            DisappearWindow(_exitLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
#if UNITY_EDITOR
                _cardLogic.SaveGameState(isTempState: true);
                EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
            }
        }

        #endregion

        #region Ads Layer

        /// <summary>
        /// Click on NoAds button.
        /// </summary>
        public void OnClickGetUndoAdsBtn()
        {
            _currentAdsType = RewardAdsType.GetUndo;
            ShowAdsLayer();
        }

        /// <summary>
        /// Click on NoAds button.
        /// </summary>
        public void OnClickNoAdsBtn()
        {
            _currentAdsType = RewardAdsType.NoAds;
            ShowAdsLayer();
        }

        /// <summary>
        /// Appearing ads layer with information about ads type.
        /// </summary>
        private void ShowAdsLayer()
        {
            UpdateAdsInfoText(_currentAdsType);

            _cardLayer.SetActive(false);
            _adsLayer.SetActive(true);
            _adsInfoText.enabled = true;
            _adsDidNotLoadText.enabled = false;
            _adsClosedTooEarlyText.enabled = false;
            _watchButton.SetActive(true);
            AppearWindow(_adsLayer);
        }

        /// <summary>
        /// Close <see cref="_adsLayer"/>.
        /// </summary>
        public void OnClickAdsCloseBtn()
        {
            DisappearWindow(_adsLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _adsLayer.SetActive(false);
                _cardLayer.SetActive(true);
            }
        }

        /// <summary>
        /// Close <see cref="_adsLayer"/>.
        /// </summary>
        public void OnWatchAdsBtnClick()
        {
            switch (_currentAdsType)
            {
                case RewardAdsType.GetUndo:
                    // _adsManagerComponent.ShowGetUndoAction();
                    break;
                case RewardAdsType.NoAds:
                    // _adsManagerComponent.NoAdsAction();
                    break;
            }
        }

        /// <summary>
        /// Call result of watched reward video.
        /// </summary>
        public void OnRewardActionState(RewardAdsState state, RewardAdsType type)
        {
            DisappearWindow(_adsLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                bool infoText = false;
                bool closedText = false;
                bool notLoadedText = false;
                switch (state)
                {
                    case RewardAdsState.TOO_EARLY_CLOSE:
                        closedText = true;
                        break;
                    case RewardAdsState.DID_NOT_LOADED:
                        notLoadedText = true;
                        break;
                }
                _adsLayer.SetActive(true);
                _adsInfoText.enabled = infoText;
                _adsDidNotLoadText.enabled = notLoadedText;
                _adsClosedTooEarlyText.enabled = closedText;
                _watchButton.SetActive(false);
                _cardLayer.SetActive(false);
                AppearWindow(_adsLayer);
            }
        }

        public void UpdateAdsInfoText(RewardAdsType type)
        {
            switch (type)
            {
                case RewardAdsType.NoAds:
                    _adsInfoText.text = NoAdsInfoText;
                    break;
                case RewardAdsType.GetUndo:
                    _adsInfoText.text = GetUndoAdsInfoText;
                    break;
            }
        }
        #endregion

        #region Rule Layer
        /// <summary>
        /// Click on rule button.
        /// </summary>
        public void OnClickSettingLayerRuleBtn()
        {
            _isShouldOpenSettings = true;
            _howToPlayComponent.SetFirstPage();
            StartCoroutine(InvokeAction(delegate { OnClickSettingLayerCloseBtn(true); Invoke(nameof(OnHowToPlayAppearing), _windowAnimationTime); }, 0f));
        }

        /// <summary>
        /// Close <see cref="_ruleLayer"/>.
        /// </summary>
        public void OnClickHowToPlayBackBtn()
        {
            DisappearWindow(_howToPlayLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _howToPlayLayer.SetActive(false);

                // if (_isShouldOpenSettings)
                // {
                //     OnClickSettingBtn();
                // }
            }
        }

        /// <summary>
        /// Show <see cref="_ruleLayer"/>.
        /// </summary>
        private void OnHowToPlayAppearing()
        {
            _howToPlayLayer.SetActive(true);
            AppearWindow(_howToPlayLayer);
        }
        #endregion

        #region Settings Layer
        /// <summary>
        /// Click on settings button.
        /// </summary>
        public void OnClickSettingBtn()
        {
            // _cardLayer.SetActive(false);
            _settingLayer.SetActive(true);
            AppearWindow(_settingLayer);
        }

        /// <summary>
        /// Close <see cref="_settingLayer"/>.
        /// </summary>
        public void OnClickSettingLayerCloseBtn(bool skipAnim = false)
        {
            if (!skipAnim)
                DisappearWindow(_settingLayer, OnWindowDisappeared);
            else
            {
                OnWindowDisappeared();
            }

            void OnWindowDisappeared()
            {
                _settingLayer.SetActive(false);
                _cardLayer.SetActive(!_statisticLayer.activeInHierarchy && !_howToPlayLayer.activeInHierarchy);
            }
        }
        #endregion

        #region Statistics Layer
        /// <summary>
        /// Click on statistics button.
        /// </summary>
        public void OnClickStatisticBtn()
        {
            StartCoroutine(InvokeAction(delegate { OnClickSettingLayerCloseBtn(true); Invoke(nameof(OnStatisticAppearing), _windowAnimationTime); }, 0f));
        }

        /// <summary>
        /// Call animation which appear statistics popup.
        /// </summary>
        private void OnStatisticAppearing()
        {
            _statisticLayer.SetActive(true);
            AppearWindow(_statisticLayer);
        }

        /// <summary>
        /// Close <see cref="_statisticLayer"/>.
        /// </summary>
        public void OnClickStatisticLayerCloseBtn()
        {
            DisappearWindow(_statisticLayer, OnStatisticsLayerClosed);
        }

        protected virtual void OnStatisticsLayerClosed()
        {
            _statisticLayer.SetActive(false);
            // OnClickSettingBtn();
        }
        #endregion

        #region Game Layer
        /// <summary>
        /// Click on random button.
        /// </summary>
        public void OnClickModalRandom()
        {
            DisappearWindow(_gameLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _cardLogic.OnNewGameStart();
                _statisticsComponent.IncreasePlayedGamesAmount();
                _gameLayer.SetActive(false);
                _cardLayer.SetActive(true);
                _cardLogic.Shuffle(false);
                _undoPerformComponent.ResetUndoStates();
            }
        }

        /// <summary>
        /// Click on replay button.
        /// </summary>
        public void OnClickModalReplay()
        {
            DisappearWindow(_gameLayer, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _cardLogic.OnNewGameStart();
                _statisticsComponent.IncreasePlayedGamesAmount();
                _gameLayer.SetActive(false);
                _cardLayer.SetActive(true);
                _cardLogic.Shuffle(true);
                _undoPerformComponent.ResetUndoStates();
            }
        }

        /// <summary>
        /// Close <see cref="_gameLayer"/>.
        /// </summary>
        public void OnClickModalClose()
        {
            DisappearWindow(_gameLayer, OnModalLayerDisappeared);
        }

        protected virtual void OnModalLayerDisappeared()
        {
            _gameLayer.SetActive(false);
            _cardLayer.SetActive(true);
        }
        #endregion

        /// <summary>
        /// Call action via time.
        /// </summary>
        /// <param name="action">Delegate.</param>
        /// <param name="time">Time for invoke.</param>
        /// <returns></returns>
        protected IEnumerator InvokeAction(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action?.Invoke();
        }

        /// <summary>
        /// Increase <see cref="_stepCount"/> value and start timer <see cref="GameTimer"/> if count == 1.
        /// </summary>
        public void CardMove()
        {
            _stepCount++;
            _statisticsComponent.IncreaseMovesAmount();

            _stepsLabel.text = _stepCount.ToString();
            if (_stepCount >= 1 && _timeCoroutine == null)
            {
                _timeCoroutine = StartCoroutine(GameTimer());
            }
        }

        /// <summary>
        /// Reset all view and states.
        /// </summary>
        public void RestoreInitialState()
        {
            InitMenuView(false);
        }

        /// <summary>
        /// Update score value <see cref="_scoreCount"/> and view text <see cref="_scoreLabel"/> on UI. 
        /// </summary>
        /// <param name="value">Score</param>
        public void AddScoreValue(int value)
        {
            _scoreCount += value;

            _scoreLabel.DOKill();
            _scoreLabel.transform.DOScale(1.25f, 0.2f).SetEase(Ease.InOutSine).onComplete = (() =>
            {
                _scoreLabel.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine).SetDelay(0.2f);
            });

            if (_scoreCount < 0)
            {
                _scoreCount = 0;
            }
            _scoreLabel.text = _scoreCount.ToString();
        }

        /// <summary>
        /// Click on sound switch button.
        /// </summary>
        public void OnClickSoundSwitch()
        {
            _soundEnable = !_soundEnable;
            UpdateSFX();
        }

        public void OnClickBGMSwitch()
        {
            _bgmEnable = !_bgmEnable;
            UpdateBGM();
        }


        public void OnClickHapticSwitch()
        {
            _hapticEnable = !_hapticEnable;
            UpdateHaptic();
        }

        void UpdateHaptic()
        {
            _hapticSwitcher.UpdateSwitchImg(_hapticEnable);
        }

        void UpdateBGM()
        {
            _bgmSwitcher.UpdateSwitchImg(_bgmEnable);

            if (_bgmController != null)
            {
                _bgmController.mute = (!_bgmEnable);
            }
        }
        void UpdateSFX()
        {
            _soundSwitcher.UpdateSwitchImg(_soundEnable);

            if (_audioController != null)
            {
                _audioController.SetMute(!_soundEnable);
            }
        }
        /// <summary>
        /// Click on hand orientation switch button.
        /// </summary>
        public void OnClickOrientationSwitch()
        {
            _orientationManager.SetHandOrientation(_orientationManager.HandOrientation == HandOrientation.RIGHT ? HandOrientation.LEFT : HandOrientation.RIGHT);
            _orientationManager.SetOrientation();
            _orientationSwitcher.UpdateSwitchImg(_orientationManager.HandOrientation != HandOrientation.LEFT);

        }

        /// <summary>
        /// Click on auto complete off/on switch button.
        /// </summary>
        public void OnClickAutoCompleteEnablingSwitch()
        {
            _autoCompleteEnable = !_autoCompleteEnable;
            _autoCompleteComponent.SetEnableAutoCompleteFeature(_autoCompleteEnable);
            _autoCompleteSwitcher.UpdateSwitchImg(_autoCompleteEnable);
        }

        /// <summary>
        /// Click on highlight draggable cards state turn off/on switch button.
        /// </summary>
        public void OnClickHighlightDraggableSwitch()
        {
            _cardLogic.HighlightDraggable = !_cardLogic.HighlightDraggable;
            for (int i = 0; i < _cardLogic.AllDeckArray.Length; i++)
            {
                _cardLogic.AllDeckArray[i].UpdateBackgroundColor();
            }
            _highlightDraggableSwitcher.UpdateSwitchImg(_cardLogic.HighlightDraggable);
        }

        /// <summary>
        /// Click on orientation state change switch button.
        /// </summary>
        public void OnClickOrientationStateSwitch()
        {
            var currentOrientationType = _orientationManager.OrientationType;
            var nextOrientationType = currentOrientationType.Next();
            _orientationManager.SwitchOrientationType(nextOrientationType);
            _screenOrientationSwitcher.UpdateSwitchImg(nextOrientationType);
        }
        
        public void InitOrientationStateSwitch()
        {
            var currentOrientationType = _orientationManager.OrientationType;
            _screenOrientationSwitcher.UpdateSwitchImg(currentOrientationType);
        }

        /// <summary>
        /// Start game timer.
        /// </summary>
        private IEnumerator GameTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                _timeCount++;
                if (_timeCount % 30 == 0)
                {
                    AddScoreValue(Public.SCORE_OVER_THIRTY_SECONDS_DECREASE);
                }
                SetTimeLabel(_timeCount);
            }
        }

        /// <summary>
        /// Stop game timer.
        /// </summary>
        private void StopGameTimer()
        {
            if (_timeCoroutine != null)
            {
                StopCoroutine(_timeCoroutine);
                _timeCoroutine = null;
            }
        }

        public void OnApplicationFocus(bool state)
        {
            if (!_cardLogic.IsGameStarted)
            {
                Debug.LogWarning($"Game does not started.");
                return;
            }

            if (!state)
            {
                _cardLogic.SaveGameState(isTempState: true);
            }
            else
            {
                _undoPerformComponent.Undo(removeOnlyState: true);
            }
        }

        public void OnApplicationPause(bool state)
        {
            if (!_cardLogic.IsGameStarted)
            {
                Debug.LogWarning($"Game does not started.");
                return;
            }

            if (state)
            {
                _cardLogic.SaveGameState(isTempState: true);
            }
            else
            {
                _undoPerformComponent.Undo(removeOnlyState: true);
            }
        }
    }
}
using System.Collections.Generic;
using EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SimpleSolitaire.Controller
{
    [System.Serializable]
    public class PreviewScene
    {
        public Sprite Icon;
        public string Name;
        [Scene] public string Scene;
    }

    public class PreviewScenesManager : MonoBehaviour
    {
        [Scene] public string PreviewScene;
        [Space] public List<PreviewScene> Scenes;
        public PreviewSceneItem SceneButton;
        public Transform ScenesContainer;
        public Canvas PreviewSceneBtnsCanvas;
        public EventSystem EventSystem;

        [SerializeField] private int _numberOfTouchesToEnterDevScene = 4;
        private string _activeScene;
        private List<PreviewSceneItem> _items = new List<PreviewSceneItem>();

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (Transform child in ScenesContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < Scenes.Count; i++)
            {
                var scene = Scenes[i];

                PreviewSceneItem sceneBtn = Instantiate(SceneButton, ScenesContainer);
                if (sceneBtn != null)
                {
                    sceneBtn.Btn.onClick.AddListener(() => LoadSceneAdditively(scene.Scene));
                    sceneBtn.Btn.image.sprite = scene.Icon;
                    sceneBtn.Txt.text = scene.Name;
                    sceneBtn.gameObject.SetActive(true);
                    _items.Add(sceneBtn);
                }
            }

            ShowAnimation();
            
            PreviewSceneBtnsCanvas.enabled = false;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void ShowAnimation()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var animationName = (i + 1) % 2 == 0 ? "Left" : "Right";
                _items[i].Anim.SetTrigger(animationName);
            }
        }

        /// <summary>
        /// Load scene above preview scene.
        /// </summary>
        /// <param name="scene"></param>
        private void LoadSceneAdditively(string scene)
        {
            _activeScene = scene;
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (_activeScene != PreviewScene)
            {
                PreviewSceneBtnsCanvas.enabled = true;
                EventSystem.enabled = false;
            }
        }

        private void Update()
        {
            CheckTouches();
        }

        /// <summary>
        /// Open preview scene.
        /// </summary>
        private void CheckTouches()
        {
            if (_activeScene != PreviewScene && Input.touchCount >= _numberOfTouchesToEnterDevScene)
            {
                OpenPreviewScene();
            }
        }

        /// <summary>
        /// Open preview scene
        /// </summary>
        private void OpenPreviewScene()
        {
            CardLogic cardLogic = FindObjectOfType<CardLogic>();
            
            if(cardLogic != null)
            {
                cardLogic.SaveGameState(isTempState: true);
            }
            
            if (_activeScene != null)
            {
                SceneManager.UnloadSceneAsync(_activeScene);
            }

            _activeScene = PreviewScene;
            PreviewSceneBtnsCanvas.enabled = false;
            EventSystem.enabled = true;
            ShowAnimation();
        }
        
        public void ForceOpenPreviewScene()
        {
            if (_activeScene != PreviewScene)
            {
                OpenPreviewScene();
            }
        }
    }
}
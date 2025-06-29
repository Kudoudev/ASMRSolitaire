﻿using UnityEngine;

namespace SimpleSolitaire.Controller
{
    public class PyramidGameManager : GameManager
    {
        [SerializeField] private GameObject _layoutsSettings;
        [SerializeField] private RectTransform _layoutsContent;
        [SerializeField] private PyramidLayoutContainer _layoutsContainer;

        [SerializeField] private VisualiseElement _layout;
        private PyramidCardLogic _pyramidCardLogic => _cardLogic as PyramidCardLogic;
        
        protected override void InitializeGame()
        {
            base.InitializeGame();

            _layoutsContainer.GetLayoutsSettings();
            GenerateLayoutsPreviews();
        }

        private void GenerateLayoutsPreviews()
        {
            for (int i = 0; i < _layoutsContainer.Layouts.Count; i++)
            {
                PyramidLayoutData layoutInfo = _layoutsContainer.Layouts[i];
                VisualiseElement layoutVisual = Instantiate(_layout, _layoutsContent);

                if (_layoutsContainer.ActiveLayouts.Contains(layoutInfo.LayoutId))
                {
                    layoutVisual.ActivateCheckmark();
                }
                else
                {
                    layoutVisual.DeactivateCheckmark();
                }

                layoutVisual.VisualImage.sprite = layoutInfo.Preview;

                layoutVisual.Btn.onClick.AddListener(() => OnLayoutClicked(layoutVisual, layoutInfo));
                layoutVisual.gameObject.SetActive(true);
            }
        }

        private void OnLayoutClicked(VisualiseElement layoutVisual, PyramidLayoutData layoutInfo)
        {
            var alreadyActive = _layoutsContainer.IsActiveLayout(layoutInfo.LayoutId);
            if (alreadyActive)
            {
                if (_layoutsContainer.HasOneOrLessLayout())
                {
                    return;
                }

                layoutVisual.DeactivateCheckmark();
                _layoutsContainer.RemoveLayout(layoutInfo.LayoutId);
            }
            else
            {
                layoutVisual.ActivateCheckmark();
                _layoutsContainer.AddLayout(layoutInfo.LayoutId);
            }

            _layoutsContainer.SaveLayouts();
        }

        protected override void InitCardLogic()
        {
            _pyramidCardLogic.InitCurrentLayout();
        }

        public void OnClickLayoutSettingBtn()
        {
            StartCoroutine(InvokeAction(delegate
            {
                OnClickModalClose();
                Invoke(nameof(OnLayoutSettingsAppearing), _windowAnimationTime);
            }, 0f));
        }

        protected override void OnModalLayerDisappeared()
        {
            _gameLayer.SetActive(false);
            _cardLayer.SetActive(!_gameLayer.activeInHierarchy && !_layoutsSettings.activeInHierarchy);
        }

        /// <summary>
        /// Call animation which appear layouts settings popup.
        /// </summary>
        private void OnLayoutSettingsAppearing()
        {
            _layoutsSettings.SetActive(true);
            AppearWindow(_layoutsSettings);
        }

        public void OnClickLayoutsSettingsLayerCloseBtn()
        {
            DisappearWindow(_layoutsSettings, OnWindowDisappeared);

            void OnWindowDisappeared()
            {
                _layoutsSettings.SetActive(false);
                AppearGameLayer();
            }
        }
    }
}
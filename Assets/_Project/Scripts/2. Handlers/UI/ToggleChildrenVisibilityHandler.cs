using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.General.UI.Animations.Config;
using GoodVillageGames.Game.Interfaces;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Handlers.UI
{
    public class ToggleChildrenVisibilityHandler : MonoBehaviour
    {
        [SerializeField] private List<AnimationTransitionConfig> _animationTransitionConfigs = new();

        private Dictionary<AnimationTransitionID,List<GameObject>> _componentsToToggle = new();


        void OnEnable()
        {
            EventsManager.Instance.OnPlayingAnimationEventTriggered += ToggleChildren;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnPlayingAnimationEventTriggered -= ToggleChildren;
        }

        void Start()
        {
            FillTransitionComponentsToToggle();
        }

        void FillTransitionComponentsToToggle()
        {
            for (int i = 0; i < _animationTransitionConfigs.Count; i++)
            {
                var key = _animationTransitionConfigs[i].CompletedAnimation;
                var value = _animationTransitionConfigs[i].ObjectToToggle;

                if (key == AnimationTransitionID.NONE)
                {
                    Debug.LogError($"Transition at {i} is NONE!");
                    continue;
                }

                if (_componentsToToggle.ContainsKey(key))
                {
                    Debug.LogError($"Duplicate key: {key}");
                    continue;
                }

                _componentsToToggle.Add(key, value);
            }
        }

        void ToggleChildren(AnimationTransitionID animationTransitionID)
        {
            if (_componentsToToggle.ContainsKey(animationTransitionID))
            {
                IList list = _componentsToToggle[animationTransitionID];
                for (int i = 0; i < list.Count; i++)
                {
                    bool toggleValue = _animationTransitionConfigs[i].ChildrenValue;
                    GameObject child = (GameObject)list[i];
                    child.SetActive(toggleValue);
                }
            }
        }
    }
}

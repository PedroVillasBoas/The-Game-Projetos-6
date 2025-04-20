using TMPro;
using UnityEngine;

namespace GoodVillageGames.Game.General.UI
{
    public class PopupUpdater : MonoBehaviour 
    { 
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeRarity;
        [SerializeField] private TextMeshProUGUI upgradeDescription;

        public void SetPopupInfo(string name, string rarity, string description)
        {
            upgradeName.text = name;
            upgradeRarity.text = rarity;
            upgradeDescription.text = description;
        }
    }
}
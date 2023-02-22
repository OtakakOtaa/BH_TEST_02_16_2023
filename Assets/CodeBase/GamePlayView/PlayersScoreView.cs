using System.Linq;
using CodeBase.Client_Server;
using TMPro;
using UnityEngine;

namespace CodeBase.GamePlayView
{
    public class PlayersScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreListField;
        [SerializeField] private TMP_Text _winText;
        private Client[] _activePlayers;

        public void DetectAllPlayer()
        { 
            _activePlayers = FindObjectsOfType<Client>().ToArray();
        }
        
        public void UpdatePlayerScoreView()
        {
            if(_activePlayers is null) return;

            string scoreListText = "";
            foreach (var player in _activePlayers)
                scoreListText += $"{player.NickName} : score hit - {player.HitCount} \n";

            _scoreListField.text = scoreListText;
        }

        public void ShowWinPanel(string winnerPlayer)
            => _winText.text = winnerPlayer + " - Win";

        public void DiscardWinPanel()
            => _winText.text = "";

        public void DiscardPlayersScore()
        {
            if(_activePlayers is null) return;

            string scoreListText = "";
            foreach (var player in _activePlayers)
                scoreListText += $"{player.NickName} : score hit - {0} \n";

            _scoreListField.text = scoreListText;
        }
    }
}
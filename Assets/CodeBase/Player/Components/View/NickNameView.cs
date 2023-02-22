using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CodeBase.Player.Components.View
{
    public class NickNameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameField;

        private List<NickNameView> _otherNickNames = new ();
        private Transform _camera;

        public void Init()
        {
            _camera = Camera.main.transform;
            _otherNickNames = FindObjectsOfType<NickNameView>().ToList();
        } 
        
        public void UpdateState()
            => RotateAllPanels();

        public void SetName(string name)
            => _nickNameField.text = name;
        
        private void RotateAllPanels()
        {
            foreach (var panel in _otherNickNames)
                panel.transform.LookAt(_camera.transform);
        }
    }
}
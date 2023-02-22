using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.ConnectionToServer
{
    public class ConfirmWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nickNameInputField;
        [SerializeField] private TMP_InputField _connectionAddressInputField;

        [SerializeField] private Button _confirmClientButton;
        [SerializeField] private Button _startServerButton;

        [SerializeField] private TMP_Text _notificationTextFiled;
        [SerializeField] private TMP_InputField _localHostTextFiled;

        private const float NotificationTime = 7f;
        private readonly ColorBlock _exceptionColor = new () { highlightedColor = Color.red, normalColor = Color.red };
        private const string IncorrectAddressText = "session with this address not found!";
        private const string BusyNameText = "this name is already occupied!";
        private ColorBlock _cashedColor;
        private string _localhost;

        public event Action<string, string> StartConnectionToServer;
        public event Action<string, string> StartServer;

        public void Start()
        {
            DetectLocalHostId();
            CashedOriginalColorScheme();
            SubscribeActionsToButtons();

            _connectionAddressInputField.text = _localhost;
        }

        public void Update()
        {
            UpdateLocalHostField();
        }

        public void ThrowBusyNameWarning()
        {
            StopAllCoroutines();
            _notificationTextFiled.text = BusyNameText;
            ChangeColorFieldPalette(_nickNameInputField, _exceptionColor);
            StartCoroutine(ReturnOriginState());
        } 
        
        public void ThrowIncorrectAddressWarning()
        {
            StopAllCoroutines();
            _notificationTextFiled.text = IncorrectAddressText;
            ChangeColorFieldPalette(_connectionAddressInputField, _exceptionColor);
            StartCoroutine(ReturnOriginState());
        }

        public void Lock()
        {
            _confirmClientButton.interactable = false;
            _startServerButton.interactable = false;
        }
        
        public void UnLock()
        {
            _confirmClientButton.interactable = true;
            _startServerButton.interactable = true;
        }

        private void CashedOriginalColorScheme()
            => _cashedColor = _nickNameInputField.colors;

        private void SubscribeActionsToButtons()
        {
            _confirmClientButton.onClick.AddListener(() => StartConnectionToServer?.Invoke(
                _nickNameInputField.text,
                _connectionAddressInputField.text
            ));
            _startServerButton.onClick.AddListener(() => StartServer?.Invoke(
                _nickNameInputField.text, 
                _connectionAddressInputField.text
            ));
        }

        private IEnumerator ReturnOriginState()
        {
            yield return new WaitForSeconds(NotificationTime);
            _notificationTextFiled.text = string.Empty;
            ChangeColorFieldPalette(_nickNameInputField, _cashedColor);
            ChangeColorFieldPalette(_connectionAddressInputField, _cashedColor);
        }

        private void ChangeColorFieldPalette(TMP_InputField field, ColorBlock colorBlock)
        {
            var origin = field.colors;
            colorBlock.normalColor = colorBlock.normalColor;
            colorBlock.highlightedColor = colorBlock.highlightedColor;
            field.colors = origin;
        }

        private void DetectLocalHostId()
        {
            string host = System.Net.Dns.GetHostName();
            _localhost = System.Net.Dns.GetHostByName(host).AddressList[0].ToString();
        }

        private void UpdateLocalHostField()
            => _localHostTextFiled.text = $"localhost: {_localhost}";
    }
}
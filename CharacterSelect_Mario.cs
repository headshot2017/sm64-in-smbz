using UnityEngine.Events;
using UnityEngine.UI;

namespace SMBZG.CharacterSelect
{
    public class CharacterSetting_Mario : CharacterSetting
    {
        protected override void Start()
        {
            base.Start();
            ModSettings.Player playerSettings = ModSettings.GetPlayerSettings(this.PlayerIndex);
            this.Toggle_SM64.isOn = playerSettings.Mario_SM64_IsEnabled.Value;
            this.Toggle_SM64.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleSM64));
        }

        private void ToggleSM64(bool isOn)
        {
            ModSettings.Player playerSettings = ModSettings.GetPlayerSettings(this.PlayerIndex);
            playerSettings.Mario_SM64_IsEnabled.Value = isOn;
        }

        public Toggle Toggle_SM64;
    }
}
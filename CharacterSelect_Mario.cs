using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;
using HarmonyLib;

namespace SMBZG.CharacterSelect
{
    public class CharacterSetting_Mario : CharacterSetting
    {
        public Toggle Toggle_SM64;

        // zethros why
        [HarmonyPatch(typeof(CharacterSetting), "Setup", new Type[] { typeof(UI_Participant) })]
        private static class SetupPatch
        {
            private static void Postfix(CharacterSetting __instance, UI_Participant participantUI)
            {
                if (__instance.GetType() != typeof(CharacterSetting_Mario))
                    return;

                CharacterSetting_Mario ins = (CharacterSetting_Mario)__instance;
                ins.Toggle_SM64.onValueChanged.AddListener(ins.ToggleSM64);

                Navigation navigation = ins.Toggle_UseAlternateColor.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnDown = ins.Toggle_SM64;
                ins.Toggle_UseAlternateColor.navigation = navigation;
                navigation = ins.Toggle_SM64.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = ins.Toggle_UseAlternateColor;
                ins.Toggle_SM64.navigation = navigation;

                int ParticipantIndex = (int)typeof(UI_Participant).GetField("ParticipantIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(participantUI);
                ModSettings.Player playerSettings = ModSettings.GetPlayerSettings(ParticipantIndex);
                ins.Toggle_SM64.isOn = playerSettings.Mario_SM64_IsEnabled.Value;
            }
        }

        private void ToggleSM64(bool isOn)
        {
            UI_Participant ParticipantUI = (UI_Participant)GetType().GetField("ParticipantUI", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
            int ParticipantIndex = (int)typeof(UI_Participant).GetField("ParticipantIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ParticipantUI);

            ModSettings.Player playerSettings = ModSettings.GetPlayerSettings(ParticipantIndex);
            playerSettings.Mario_SM64_IsEnabled.Value = isOn;
            playerSettings.playerCategory.SaveToFile();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SMBZG.CharacterSelect
{
    // Token: 0x020000DB RID: 219
    public class CharacterSetting_SM64 : CharacterSetting
    {
        // Token: 0x060010CC RID: 4300 RVA: 0x00068728 File Offset: 0x00066928
        protected override void Start()
        {
            base.Start();
            SaveData.GameSaveData_Type.PlayerSettings playerSettings = SaveData.Data.GetPlayerSettings(this.PlayerIndex);
            this.Toggle_SM64.isOn = false;
            this.Toggle_SM64.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleSM64));
        }

        private void ToggleSM64(bool isOn)
        {
            
        }

        public Toggle Toggle_SM64;
    }
}
using MelonLoader;
using LibSM64;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using SMBZG.CharacterSelect;

[assembly: MelonInfo(typeof(SMBZ_64.Core), "SMBZ_64", "1.0.0", "Headshotnoby/headshot2017", null)]
[assembly: MelonGame("Jonathan Miller aka Zethros", "SMBZ-G")]

namespace SMBZ_64
{
    public class Core : MelonMod
    {
        static List<SM64Mario> _marios = new List<SM64Mario>();
        static List<SM64DynamicTerrain> _surfaceObjects = new List<SM64DynamicTerrain>();

        public static CharacterData_SO Mario64Data = null;
        public bool showError;

        public override void OnInitializeMelon()
        {
            byte[] rom;
            try
            {
                rom = File.ReadAllBytes("sm64.us.z64");
            }
            catch (FileNotFoundException)
            {
                LoggerInstance.Msg("Super Mario 64 US ROM 'sm64.us.z64' not found");
                showError = true;
                return;
            }
            Interop.GlobalInit(rom);
        }

        public override void OnGUI()
        {
            if (showError)
            {
                GUI.BeginGroup(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 120, 320, 240));
                GUI.Box(new Rect(0, 0, 320, 240), "SMBZ_64");
                GUI.Label(new Rect(32, 32, 320-64, 240-64), "Super Mario 64 US ROM not found.\nPlease supply a ROM with the filename 'sm64.us.z64' in the folder where the EXE is located");

                if (GUI.Button(new Rect(20, 240-64-8, 320-40, 64), "OK"))
                    showError = false;

                GUI.EndGroup();
            }
        }

        public override void OnLateInitializeMelon()
        {
            if (!Interop.isGlobalInit) return;

            // Create a clone of Mario's character data
            GameObject Mario64Prefab = GameObject.Instantiate(BattleCache.ins.CharacterData_Mario.Prefab_BattleGameObject);
            Mario64Prefab.name = "Mario64";
            Mario64Prefab.SetActive(false);
            MarioControl smbzMarioOld = Mario64Prefab.GetComponent<MarioControl>();
            Mario64Control smbzMario = Mario64Prefab.AddComponent<Mario64Control>();
            smbzMarioOld.Comp_SpriteRenderer.enabled = false;
            smbzMario.Comp_Hurtbox = smbzMarioOld.Comp_Hurtbox;
            smbzMario.enabled = true;
            GameObject.Destroy(smbzMarioOld);
            GameObject.DontDestroyOnLoad(Mario64Prefab);


            // Add "SM64 Mario" to Mario's special settings

            // Clone the original object
            GameObject settings = GameObject.Instantiate(BattleCache.ins.CharacterData_Mario.Prefab_SpecialCharacterSettingsUI);
            GameObject.DontDestroyOnLoad(settings);
            settings.name = "UI_CharacterSettings_Mario";
            CharacterSetting oldCharSetting = settings.GetComponent<CharacterSetting>();
            CharacterSetting_Mario charSetting = settings.AddComponent<CharacterSetting_Mario>();

            // Copy all fields from oldCharSetting to the new one
            Type type = typeof(CharacterSetting);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                field.SetValue(charSetting, field.GetValue(oldCharSetting));
            }
            GameObject.Destroy(oldCharSetting);

            BattleCache.ins.CharacterData_Mario.Prefab_SpecialCharacterSettingsUI = settings;

            // Add the "SM64 Mario" checkbox
            GameObject verticalList = settings.transform.GetChild(1).gameObject;
            GameObject UseSM64 = GameObject.Instantiate(verticalList.transform.GetChild(0).gameObject); // Clone the "Alternate Color" checkbox
            GameObject.DontDestroyOnLoad(UseSM64);
            UseSM64.name = "UseSM64";
            UseSM64.transform.SetParent(verticalList.transform, false);

            GameObject LabelObj = UseSM64.transform.GetChild(0).gameObject;
            TMPro.TextMeshProUGUI Label = LabelObj.GetComponent<TMPro.TextMeshProUGUI>();
            Label.text = "SM64 Mario";

            GameObject toggleObj = UseSM64.transform.GetChild(1).gameObject;
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            toggleObj.name = "UseSM64 Toggle";
            charSetting.Toggle_SM64 = toggle;

            Mario64Data = ScriptableObject.CreateInstance<CharacterData_SO>();
            Mario64Data.Prefab_BattleGameObject = Mario64Prefab;
            Mario64Data.Prefab_SpecialCharacterSettingsUI = settings;
            Mario64Data.Character = BattleCache.CharacterEnum.Mario;
            Mario64Data.name = "[CharacterData] Mario64";
            Mario64Data.DittoHue = BattleCache.ins.CharacterData_Mario.DittoHue;
            Mario64Data.DittoSaturation = BattleCache.ins.CharacterData_Mario.DittoSaturation;
            Mario64Data.DittoContrast = BattleCache.ins.CharacterData_Mario.DittoContrast;
            smbzMario.GetType().BaseType.GetField("CharacterData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).SetValue(smbzMario, Mario64Data);

            Application.logMessageReceived += LogHandler;
        }

        private void LogHandler(string message, string stacktrace, LogType type)
        {
            //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            LoggerInstance.Msg($"{message}\n{stacktrace}");
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            _surfaceObjects.Clear();
            _marios.Clear();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"{buildIndex} {sceneName}");

            switch (buildIndex)
            {
                case 3:
                    BattleTerrain();
                    break;

                case 6:
                    SetupCharSelect();
                    break;
            }
        }

        public override void OnUpdate()
        {
            foreach (var o in _surfaceObjects)
                o.contextUpdate();

            foreach (var o in _marios)
            {
                if (!o.smbzChar || !o.smbzChar.CharacterGO)
                    continue;
                //if (o.smbzChar.CharacterGO.Comp_SpriteRenderer.enabled)
                    //o.smbzChar.CharacterGO.Comp_SpriteRenderer.enabled = false;

                o.contextUpdate();

                BaseCharacter.MovementRushStateENUM movRush =
                    (BaseCharacter.MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o.smbzChar.CharacterGO);
                bool canRecover =
                    !o.smbzChar.CharacterGO.IsHurt &&
                    !o.smbzChar.CharacterGO.IsPursuing;
                bool overrideSM64 =
                    !canRecover ||
                    (bool)typeof(BaseCharacter).GetField("IsBursting", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o.smbzChar.CharacterGO) ||
                    (bool)typeof(BaseCharacter).GetField("IsRushing", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o.smbzChar.CharacterGO) ||
                    movRush != BaseCharacter.MovementRushStateENUM.Inactive;
                o.SetCanRecover(canRecover);

                if (overrideSM64)
                {
                    Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
                    o.SetPosition(o.smbzChar.CharacterGO.transform.position, new Vector3(0, -0.9f, -1));
                    o.SetVelocity(new Vector3(0, o.smbzChar.CharacterGO.GetVelocity().y/3, 0));
                }
                else
                    o.smbzChar.CharacterGO.transform.position = o.transform.position + new Vector3(0, 0.8f, 0);
            }
        }

        public override void OnFixedUpdate()
        {
            foreach (var o in _surfaceObjects)
            {
                BattleBackgroundManager BBManager = (BattleBackgroundManager)typeof(BattleController).GetField("BackgroundManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                float GroundPositionY = (float)BBManager.GetType().GetField("GroundPositionY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BBManager);

                Vector3 pos = o.position;
                pos.y = GroundPositionY;
                o.SetPosition(pos);

                o.contextFixedUpdate();
            }

            foreach (var o in _marios)
            {
                if (!o.smbzChar || !o.smbzChar.CharacterGO)
                    continue;

                if (!o.smbzChar.CharacterGO.IsDead)
                    o.SetHealth(0x800);

                Type baseCharType = typeof(BaseCharacter);
                FieldInfo frozenField = baseCharType.GetField("IsFrozen", BindingFlags.NonPublic | BindingFlags.Instance);

                if (!(bool)frozenField.GetValue(o.smbzChar.CharacterGO))
                    o.contextFixedUpdate();
            }
        }

        public override void OnDeinitializeMelon()
        {
            Interop.GlobalTerminate();
        }

        public void RefreshStaticTerrain()
        {
            LoggerInstance.Msg("refreshstaticterrain");
            Interop.StaticSurfacesLoad(Utils.GetAllStaticSurfaces());
        }

        public void RegisterMario(SM64Mario mario)
        {
            if (!_marios.Contains(mario))
                _marios.Add(mario);
        }

        public void UnregisterMario(SM64Mario mario)
        {
            if (_marios.Contains(mario))
                _marios.Remove(mario);
        }

        public void RegisterSurfaceObject(SM64DynamicTerrain surfaceObject)
        {
            if (!_surfaceObjects.Contains(surfaceObject))
                _surfaceObjects.Add(surfaceObject);
        }

        public void UnregisterSurfaceObject(SM64DynamicTerrain surfaceObject)
        {
            if (_surfaceObjects.Contains(surfaceObject))
                _surfaceObjects.Remove(surfaceObject);
        }


        public void BattleTerrain()
        {
            BattleBackgroundManager BBManager = (BattleBackgroundManager)typeof(BattleController).GetField("BackgroundManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
            float GroundPositionY = (float)BBManager.GetType().GetField("GroundPositionY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BBManager);

            for (int i = -10; i <= 10; i++)
            {
                Vector3 P = new Vector3(128 * i, GroundPositionY, -1);
                GameObject surfaceObj = new GameObject("SM64_SURFACE");
                surfaceObj.transform.position = P;
                MeshCollider surfaceMesh = surfaceObj.AddComponent<MeshCollider>();
                Mesh mesh = new Mesh();
                mesh.name = "SM64_SURFACE_MESH";
                mesh.SetVertices(
                    new Vector3[]
                    {
                            new Vector3(-128,0,-128), new Vector3(128,0,+128), new Vector3(128,0,-128),
                            new Vector3(128,0,+128), new Vector3(-128,0,-128), new Vector3(-128,0,+128),
                    }
                );
                mesh.SetTriangles(new int[] { 0, 1, 2, 3, 4, 5 }, 0);
                surfaceMesh.sharedMesh = mesh;
                surfaceObj.AddComponent<SM64DynamicTerrain>();
            }

            LoggerInstance.Msg($"ground pos: {GroundPositionY}");
        }

        void SetupCharSelect()
        {
            
        }


        public static void OnMarioChangeAction(SM64Mario o)
        {
            SM64Constants.Action action = (SM64Constants.Action)o.marioState.action;
            uint actionArg = o.marioState.actionArg;

            Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
            character.OnChangeSM64Action(action, actionArg);
        }

        public static void OnMarioAdvanceAnimFrame(SM64Mario o)
        {
            SM64Constants.MarioAnimID animID = (SM64Constants.MarioAnimID)o.marioState.animID;
            short animFrame = o.marioState.animFrame;

            Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
            character.OnMarioAdvanceAnimFrame(animID, animFrame);
        }

        [HarmonyPatch(typeof(CharacterControl), "InstantiateCharacterObject", new Type[] { typeof(Vector3), typeof(Quaternion) })]
        private static class CharObjPatch
        {
            private static bool Prefix(CharacterControl __instance)
            {
                ModSettings.Player player = ModSettings.GetPlayerSettings(__instance.PlayerDataReference.PlayerIndex);
                if (__instance.PlayerDataReference.InitialCharacterData == BattleCache.ins.CharacterData_Mario && player.Mario_SM64_IsEnabled.Value)
                {
                    __instance.PlayerDataReference.CurrentCharacterData = Mario64Data;
                }

                return true;
            }
        }
    }
}
using MelonLoader;
using LibSM64;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using SMBZG.CharacterSelect;
using SMBZG;
using System.Security.Cryptography;
using System.Text;

[assembly: MelonInfo(typeof(SMBZ_64.Core), "SMBZ_64", "1.0.0", "Headshotnoby/headshot2017", null)]
[assembly: MelonGame("Jonathan Miller aka Zethros", "SMBZ-G")]

namespace SMBZ_64
{
    public class Core : MelonMod
    {
        static List<SM64Mario> _marios = new List<SM64Mario>();
        static List<SM64DynamicTerrain> _surfaceObjects = new List<SM64DynamicTerrain>();

        public static CustomCharacter Mario64cc = null;
        public bool showError;
        public string errorMsg;

        public override void OnInitializeMelon()
        {
            byte[] rom;

            try
            {
                rom = File.ReadAllBytes("sm64.z64");
            }
            catch (FileNotFoundException)
            {
                errorMsg = "Super Mario 64 US ROM not found.\nPlease supply a ROM with the filename 'sm64.z64' in the folder where the EXE is located";
                showError = true;
                LoggerInstance.Msg(errorMsg);
                return;
            }

            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                byte[] hash = cryptoProvider.ComputeHash(rom);
                StringBuilder result = new StringBuilder(4 * 2);

                for (int i = 0; i < 4; i++)
                    result.Append(hash[i].ToString("x2"));

                string hashStr = result.ToString();

                if (hashStr != "9bef1128")
                {
                    errorMsg = $"Super Mario 64 US ROM 'sm64.z64' SHA-1 mismatch\nExpected: 9bef1128\nYour copy: {hashStr}\n\nPlease supply the correct ROM.";
                    showError = true;
                    LoggerInstance.Msg(errorMsg);
                    return;
                }
            }

            Interop.GlobalInit(rom);
        }

        public override void OnGUI()
        {
            if (showError)
            {
                GUI.BeginGroup(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 120, 320, 240));
                GUI.Box(new Rect(0, 0, 320, 240), "SMBZ_64");
                GUI.Label(new Rect(32, 32, 320-64, 240-64), errorMsg);

                if (GUI.Button(new Rect(20, 240-64-8, 320-40, 64), "OK"))
                    showError = false;

                GUI.EndGroup();
            }
        }

        void LoadMario64(CustomCharacter cc)
        {
            if (cc.internalName != "Mario64") return;
            Mario64cc = cc;

            GameObject Prefab = cc.characterData.Prefab_BattleGameObject;
            CustomBaseCharacter old = Prefab.GetComponent<CustomBaseCharacter>();
            Mario64Control control = Prefab.AddComponent<Mario64Control>();
            control.Comp_Hurtbox = old.Comp_Hurtbox;
            control.Comp_Hurtbox.transform.localScale = new Vector3(1, 1.75f, 1);
            control.Comp_SpriteRenderer = Prefab.transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
            control.Comp_SpriteRenderer.enabled = false;
            control.GetType().BaseType.GetField("CharacterData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).SetValue(control, cc.characterData);
            GameObject.Destroy(old);
        }

        public override void OnLateInitializeMelon()
        {
            if (!Interop.isGlobalInit) return;

            CharLoader.Core.afterCharacterLoad += LoadMario64;
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
                case 4:
                    BattleTerrain();
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
                    !o.smbzChar.CharacterGO.IsInBlockStun &&
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
    }
}
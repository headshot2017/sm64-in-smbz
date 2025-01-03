using MelonLoader;
using LibSM64;
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
                return;
            }
            Interop.GlobalInit(rom);
        }

        public override void OnLateInitializeMelon()
        {
            // Add "SM64 Mario" to Mario's special settings

            // Clone the original object
            GameObject o = GameObject.Instantiate(BattleCache.ins.CharacterData_Mario.Prefab_SpecialCharacterSettingsUI);
            GameObject.DontDestroyOnLoad(o);
            o.name = "UI_CharacterSettings_Mario";
            CharacterSetting oldCharSetting = o.GetComponent<CharacterSetting>();
            CharacterSetting_Mario charSetting = o.AddComponent<CharacterSetting_Mario>();

            // Copy all fields from oldCharSetting to the new one
            Type type = typeof(CharacterSetting);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                field.SetValue(charSetting, field.GetValue(oldCharSetting));
            }
            GameObject.Destroy(oldCharSetting);

            BattleCache.ins.CharacterData_Mario.Prefab_SpecialCharacterSettingsUI = o;

            // Add the "SM64 Mario" checkbox
            GameObject verticalList = o.transform.GetChild(1).gameObject;
            GameObject UseSM64 = GameObject.Instantiate(verticalList.transform.GetChild(0).gameObject); // Clone the "Alternate Color" checkbox
            GameObject.DontDestroyOnLoad(UseSM64);
            UseSM64.name = "UseSM64";
            UseSM64.transform.parent = verticalList.transform;

            GameObject LabelObj = UseSM64.transform.GetChild(0).gameObject;
            TMPro.TextMeshProUGUI Label = LabelObj.GetComponent<TMPro.TextMeshProUGUI>();
            Label.text = "SM64 Mario";

            GameObject toggleObj = UseSM64.transform.GetChild(1).gameObject;
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            toggleObj.name = "UseSM64 Toggle";
            charSetting.Toggle_SM64 = toggle;
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            _surfaceObjects.Clear();
            _marios.Clear();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"{buildIndex} {sceneName}");

            switch (buildIndex)
            {
                case 3:
                    BattleStart();
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
                if (!o.smbzChar.CharacterGO)
                    continue;
                //if (o.smbzChar.CharacterGO.Comp_SpriteRenderer.enabled)
                    //o.smbzChar.CharacterGO.Comp_SpriteRenderer.enabled = false;

                o.contextUpdate();

                if (o.smbzChar.CharacterGO.IsHurt || o.smbzChar.CharacterGO.IsPursuing)
                {
                    Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
                    o.SetPosition(o.smbzChar.CharacterGO.transform.position + new Vector3(0, -0.9f, -1));
                    o.SetVelocity(new Vector3(0, o.smbzChar.CharacterGO.GetVelocity().y/3, 0));
                }
                else
                    o.smbzChar.CharacterGO.transform.position = o.transform.position + new Vector3(0, 0.8f, 0);
            }
        }

        public override void OnFixedUpdate()
        {
            foreach (var o in _surfaceObjects)
                o.contextFixedUpdate();

            foreach (var o in _marios)
            {
                if (!o.smbzChar.CharacterGO)
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


        void BattleStart()
        {
            for (int i = -10; i <= 10; i++)
            {
                Vector3 P = new Vector3(128 * i, 0, -1);
                GameObject surfaceObj = new GameObject("SM64_SURFACE");
                surfaceObj.transform.position = P;
                MeshCollider surfaceMesh = surfaceObj.AddComponent<MeshCollider>();
                surfaceObj.AddComponent<SM64StaticTerrain>();
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
            }
            RefreshStaticTerrain();

            for (int i = 1; i <= 2; i++)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag($"Team{i}");
                CharacterControl smbzControl = objs[0].GetComponent<CharacterControl>();
                ModSettings.Player player = ModSettings.GetPlayerSettings((TeamUtility.IO.PlayerID)(i - 1));
                if (smbzControl.CharacterGO is not MarioControl || !player.Mario_SM64_IsEnabled.Value)
                    continue;

                Type type = typeof(Mario64Control);
                FieldInfo field = type.GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance);

                MarioControl smbzMarioOld = objs[1].GetComponent<MarioControl>();
                Mario64Control smbzMario = objs[1].AddComponent<Mario64Control>();
                smbzControl.CharacterGO = smbzMario;
                smbzMarioOld.Comp_SpriteRenderer.enabled = false;
                smbzMario.Comp_Hurtbox = smbzMarioOld.Comp_Hurtbox;
                smbzMario.Comp_InterplayerCollider = smbzMarioOld.Comp_InterplayerCollider;
                field.SetValue(smbzMario, smbzControl);
                GameObject.Destroy(smbzMarioOld);
                smbzMario.enabled = true;

                GameObject marioObj = new GameObject("SM64_MARIO");
                marioObj.transform.position = new Vector3(smbzMario.transform.position.x, smbzMario.transform.position.y, -1);
                SM64InputSMBZG input = marioObj.AddComponent<SM64InputSMBZG>();
                SM64Mario mario = marioObj.AddComponent<SM64Mario>();
                if (mario.spawned)
                {
                    input.c = smbzControl;
                    smbzMario.sm64 = mario;
                    smbzMario.sm64input = input;

                    Material[] mat = GameObject.FindObjectsOfType<Material>();
                    Material m = Material.Instantiate<Material>(mat[1]);
                    Shader[] sh = Resources.FindObjectsOfTypeAll<Shader>();
                    foreach (Shader s in sh)
                    {
                        if (s.name != "Legacy Shaders/VertexLit")
                            continue;

                        m.shader = s;
                        m.SetColor("_Emission", new Color(0.4f, 0.4f, 0.4f, 1));
                        break;

                    }

                    mario.smbzChar = smbzControl;
                    mario.changeActionCallback = OnMarioChangeAction;
                    mario.advanceAnimFrameCallback = OnMarioAdvanceAnimFrame;
                    mario.SetMaterial(m);
                    mario.SetFaceAngle((float)Math.PI / 2 * (i == 1 ? -1 : 1));
                    RegisterMario(mario);
                }
                else
                    LoggerInstance.Msg($"Failed to spawn Mario {i}");
            }
        }

        void SetupCharSelect()
        {
            
        }


        static void OnMarioChangeAction(SM64Mario o)
        {
            SM64Constants.Action action = (SM64Constants.Action)o.marioState.action;
            uint actionArg = o.marioState.actionArg;

            Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
            character.OnChangeSM64Action(action, actionArg);
        }

        static void OnMarioAdvanceAnimFrame(SM64Mario o)
        {
            SM64Constants.MarioAnimID animID = (SM64Constants.MarioAnimID)o.marioState.animID;
            short animFrame = o.marioState.animFrame;

            Mario64Control character = (Mario64Control)o.smbzChar.CharacterGO;
            character.OnMarioAdvanceAnimFrame(animID, animFrame);
        }
    }
}
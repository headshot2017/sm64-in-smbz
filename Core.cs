using MelonLoader;
using LibSM64;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using System.Net.NetworkInformation;

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

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            _surfaceObjects.Clear();
            _marios.Clear();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg($"{buildIndex} {sceneName}");

            if (buildIndex != 3)
                return;

            for (int i = -10; i <= 10; i++)
            {
                Vector3 P = new Vector3(128*i, 0, -1);
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
                RefreshStaticTerrain();
            }

            for (int i = 1; i <= 2; i++)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag($"Team{i}");

                MarioControl smbzMario = objs[1].GetComponent<MarioControl>();
                if (!smbzMario) // Only replace Mario with SM64 Mario
                    continue;

                GameObject marioObj = new GameObject("SM64_MARIO");
                marioObj.transform.position = new Vector3(smbzMario.transform.position.x, smbzMario.transform.position.y, -1);
                SM64InputSMBZG input = marioObj.AddComponent<SM64InputSMBZG>();
                SM64Mario mario = marioObj.AddComponent<SM64Mario>();
                if (mario.spawned)
                {
                    input.c = objs[0].GetComponent<CharacterControl>();

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

                    smbzMario.Comp_SpriteRenderer.enabled = false;
                    //smbzMario.enabled = false;
                    mario.smbzChar = smbzMario;
                    mario.changeActionCallback = OnMarioChangeAction;
                    mario.SetMaterial(m);
                    RegisterMario(mario);
                }
                else
                    LoggerInstance.Msg($"Failed to spawn Mario {i}");
            }
        }

        public override void OnUpdate()
        {
            foreach (var o in _surfaceObjects)
                o.contextUpdate();

            foreach (var o in _marios)
            {
                if (!o.smbzChar.IsDead)
                    o.SetHealth(0x800);

                o.contextUpdate();

                if (o.smbzChar.IsHurt)
                    o.SetPosition(o.smbzChar.transform.position, new Vector3(0, -0.8f, -1));
                else
                    o.smbzChar.transform.position = o.transform.position + new Vector3(0, 0.8f, 0);
            }
        }

        public override void OnFixedUpdate()
        {
            foreach (var o in _surfaceObjects)
                o.contextFixedUpdate();

            foreach (var o in _marios)
            {
                Type marioControlType = typeof(MarioControl);
                FieldInfo frozenField = marioControlType.GetField("IsFrozen", BindingFlags.NonPublic | BindingFlags.Instance);

                if (!(bool)frozenField.GetValue(o.smbzChar))
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


        static void OnMarioChangeAction(SM64Mario o)
        {
            uint action = o.marioState.action;
            uint actionArg = o.marioState.actionArg;
            
            Melon<Core>.Logger.Msg($"{o} {o.smbzChar} {action} {actionArg}");
        }

        [HarmonyPatch(typeof(BaseCharacter), "Hurt", new Type[] { typeof(TakeDamageRequest) } )]
        static class HurtPatch
        {
            private static void Prefix(BaseCharacter __instance)
            {

            }

            private static void Postfix(BaseCharacter __instance, TakeDamageRequest request)
            {
                foreach (var o in _marios)
                {
                    if (o.smbzChar != __instance)
                        continue;

                    if (o.smbzChar.IsHurt)
                        o.TakeDamage((uint)(request.damage * 3), 0, o.transform.position + new Vector3(request.launch.x * -1, 0, -1));
                    if (o.smbzChar.IsDead)
                        o.Kill();
                }
            }
        }
    }
}
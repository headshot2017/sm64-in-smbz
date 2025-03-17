using MelonLoader;

public static class ModSettings
{
    public class Player
    {
        public Player(int PlayerID)
        {
            playerCategory = MelonPreferences.CreateCategory($"Player{PlayerID}Preferences");
            playerCategory.SetFilePath("UserData/SMBZ_64.cfg");
            Mario_SM64_IsEnabled = playerCategory.CreateEntry<bool>("Mario_SM64_IsEnabled", false);
        }

        public MelonPreferences_Category playerCategory;
        public MelonPreferences_Entry<bool> Mario_SM64_IsEnabled;
    }

    public static Player GetPlayerSettings(int id)
    {
        if (!PlayerSettings.ContainsKey(id))
            PlayerSettings.Add(id, new Player(id));
        return PlayerSettings[id];
    }

    static Dictionary<int, Player> PlayerSettings = new Dictionary<int, Player>();
}

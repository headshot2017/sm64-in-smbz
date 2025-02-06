using TeamUtility.IO;
using MelonLoader;

public static class ModSettings
{
    public class Player
    {
        public Player(PlayerID PlayerID)
        {
            playerCategory = MelonPreferences.CreateCategory($"Player{PlayerID}Preferences");
            playerCategory.SetFilePath("UserData/SMBZ_64.cfg");
            Mario_SM64_IsEnabled = playerCategory.CreateEntry<bool>("Mario_SM64_IsEnabled", false);
        }

        public MelonPreferences_Category playerCategory;
        public MelonPreferences_Entry<bool> Mario_SM64_IsEnabled;
    }

    public static Player GetPlayerSettings(PlayerID id)
    {
        return (id == PlayerID.One) ? Player1Settings : (id == PlayerID.Two) ? Player2Settings : null;
    }

    static Player Player1Settings = new Player(PlayerID.One);
    static Player Player2Settings = new Player(PlayerID.Two);
}
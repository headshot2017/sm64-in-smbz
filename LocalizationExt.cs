using static LocalizationResource;

public class LocalizationExt
{
    public enum ResourceKeyExtENUM
    {
        PunchCombo,
        DiveAttack,
        AirKick,
        AirDive,
        Breakdance,
        SlideKick,
        Twirl,
        GroundTwirl,
        GroundPound,
        PunchSpin,
        SuperUppercut,
        WhileMoving,
        WhenMovingFastPunch,
        WhenMovingFastKick,
    }

    public class LanguageKeyPairExt
    {
        public ResourceKeyExtENUM Key;

        public string EnglishValue;

        public string SpanishValue;

        public LanguageKeyPairExt()
        {
        }

        public LanguageKeyPairExt(ResourceKeyExtENUM key, string englishValue)
        {
            Key = key;
            EnglishValue = englishValue;
        }

        public LanguageKeyPairExt(ResourceKeyExtENUM key, string englishValue, string spanishValue)
        {
            Key = key;
            EnglishValue = englishValue;
            SpanishValue = spanishValue;
        }

        public string GetValue(LanguageENUM lang)
        {
            if (lang != 0 && lang == LanguageENUM.Spanish)
            {
                return SpanishValue;
            }

            return EnglishValue;
        }
    }

    public static List<LanguageKeyPairExt> Resource = new List<LanguageKeyPairExt>
    {
        new LanguageKeyPairExt(ResourceKeyExtENUM.PunchCombo, "Punch Combo", "Combo de puñetazos"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.DiveAttack, "Dive Attack", "Ataque en picado"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.AirKick, "Aerial Kick", "Patada aérea"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.AirDive, "Aerial Dive", "Picado aéreo"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.Breakdance, "Breakdance", "Breakdance"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.SlideKick, "Slide Kick", "Patada deslizante"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.Twirl, "Twirl", "Ataque giratorio"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.GroundTwirl, "Quick Twirl", "Ataque giratorio rápido"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.GroundPound, "Ground Pound", "Ground Pound"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.PunchSpin, "Punch Spin", "Puñetazo giratorio"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.SuperUppercut, "Super Uppercut", "Super Uppercut"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.WhileMoving, "While moving", "Al moverse"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.WhenMovingFastPunch, "When moving fast, Mario will dive instead of punching", "Al moverse rápido, Mario hará esto en vez de un puñetazo"),
        new LanguageKeyPairExt(ResourceKeyExtENUM.WhenMovingFastKick, "When moving fast, Mario will dive instead of kicking", "Al moverse rápido, Mario hará esto en vez de una patada"),
        
    };

    public static string Fetch(ResourceKeyExtENUM resourceKey)
    {
        return Fetch(resourceKey, SaveData.Data.CurrentLanguageSetting);
    }

    public static string Fetch(ResourceKeyExtENUM resourceKey, LanguageENUM currentLanguageSetting)
    {
        LanguageKeyPairExt languageKeyPair = Resource.Find((LanguageKeyPairExt m) => m.Key == resourceKey);
        if (languageKeyPair == null)
        {
            return string.Empty;
        }

        return languageKeyPair.GetValue(currentLanguageSetting);
    }
}
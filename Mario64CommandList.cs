public static class CommandListDataExt
{
    public static CommandListModel Mario64()
    {
        return new CommandListModel
        {
            CharacterName = "SM64 Mario",
            RecordList =
            {
                new CommandListRecordModel
                {
                    Title = "Punch Combo",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack,
                        CommandImageDisplayEnum.Attack,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Dive Attack",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    AdditionalInfo = "When moving fast, Mario will dive instead of punching",
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack
                    }
                },
                /*
                new CommandListRecordModel
                {
                    Title = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.HookPunch),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.Attack
                    }
                },
                */
                new CommandListRecordModel
                {
                    Title = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.Uppercut),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Up,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Air Kick",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Air Dive",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    AdditionalInfo = "When moving fast, Mario will dive instead of kicking",
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.SmackDown),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Breakdance",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    FeatureImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.GuardBreaker
                    },
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Defend,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Slide Kick",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    AdditionalInfo = "While moving",
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Defend,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Twirl",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.ZTrigger,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Ground Pound",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.ZTrigger,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = "Punch Spin",
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    FeatureImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.CriticalStrike
                    },
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.ZTrigger,
                        CommandImageDisplayEnum.Attack
                    }
                },
            }
        };
    }
}
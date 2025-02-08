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
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.PunchCombo),
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
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.DiveAttack),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    AdditionalInfo = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.WhenMovingFastPunch),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.Breakdance),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    FeatureImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.GuardBreaker
                    },
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.SlideKick),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    AdditionalInfo = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.WhileMoving),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Down,
                        CommandImageDisplayEnum.Attack
                    }
                },
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
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.AirKick),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.AirDive),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    AdditionalInfo = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.WhenMovingFastKick),
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
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.GroundTwirl),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileOnGround),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.ZTrigger,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.PunchSpin),
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
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.Twirl),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
                    CommandImageList = new List<CommandImageDisplayEnum>
                    {
                        CommandImageDisplayEnum.ZTrigger,
                        CommandImageDisplayEnum.Attack
                    }
                },
                new CommandListRecordModel
                {
                    Title = LocalizationExt.Fetch(LocalizationExt.ResourceKeyExtENUM.GroundPound),
                    Subtitle = LocalizationResource.Fetch(LocalizationResource.ResourceKeyENUM.WhileInAir),
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
using UnityEngine;

public class ConfigManager : GlobalReference<ConfigManager>
{
    public ToolConfig toolConfig;
    public CardConfig cardConfig;
    public ResearchConfig researchConfig;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

}

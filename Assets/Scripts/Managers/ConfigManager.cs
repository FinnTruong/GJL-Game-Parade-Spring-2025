using UnityEngine;

public class ConfigManager : GlobalReference<ConfigManager>
{
    public CardConfig cardConfig;
    public CropConfig cropConfig;
    public ResearchConfig researchConfig;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

}

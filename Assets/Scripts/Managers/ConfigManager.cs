using UnityEngine;

public class ConfigManager : GlobalReference<ConfigManager>
{
    public CardConfig cardConfig;
    public CropConfig cropConfig;
    public ResearchConfig researchConfig;
    public UserLevelConfig userLevelConfig;
    public CardAppearanceConfig cardAppearanceConfig;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

}

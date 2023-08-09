using Vintagestory.API.Common;

[assembly: ModInfo("Vanilla Crate Compatibility")]

namespace VanillaCrateCompatibility;

public class Core : ModSystem
{
    public const string Namespace = nameof(VanillaCrateCompatibility);

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}
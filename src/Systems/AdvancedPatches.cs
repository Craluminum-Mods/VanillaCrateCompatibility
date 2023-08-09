using Vintagestory.API.Common;
using System.Collections.Generic;
using System.Linq;
using static VanillaCrateCompatibility.Core;

namespace VanillaCrateCompatibility;

public class AdvancedPatches : ModSystem
{
    public override void AssetsFinalize(ICoreAPI api)
    {
        List<string> woodTypes = api.GetTypesFromWorldProperties("worldproperties/block/wood.json", extraTypesAtStart: "aged").ConvertAll(x => "wood-" + x);
        List<string> notCrateWoodTypes = woodTypes.Skip(13).ToList();

        if (notCrateWoodTypes?.Count == 0)
        {
            api.Logger.Debug(Namespace + "." + ToString() + ": No new wood types detected");
            return;
        }

        foreach (Block block in api.World.Blocks)
        {
            if (block.Code != new AssetLocation("crate"))
            {
                continue;
            }

            List<string> types = block.Attributes["types"].AsObject<List<string>>().Concat(notCrateWoodTypes).ToList();

            block.ChangeAttribute(types, "types");
            block.AddToCreativeInventory(api.World, notCrateWoodTypes);
        }
    }

}
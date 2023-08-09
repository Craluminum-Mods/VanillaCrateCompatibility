using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;
using System;
using System.Linq;
using static VanillaCrateCompatibility.Core;

namespace VanillaCrateCompatibility;

public class JsonPatches : ModSystem
{
    public override double ExecuteOrder() => 0.05;

    public override void AssetsLoaded(ICoreAPI api)
    {
        int TickCount = Environment.TickCount;
        List<JsonPatch> patches = CreatePatches(api);

        if (patches?.Count == 0)
        {
            api.Logger.Debug(Namespace + "." + ToString() + ": No new wood types detected");
            return;
        }

        api.ApplyJsonPatches(patches);
        api.Logger.Debug(Namespace + ": {0} took {1} ms", ToString(), Environment.TickCount - TickCount);
    }

    private List<JsonPatch> CreatePatches(ICoreAPI api)
    {
        List<JsonPatch> patches = new();

        List<string> woodTypes = api.GetTypesFromWorldProperties("worldproperties/block/wood.json", extraTypesAtStart: "aged");
        List<string> notCrateWoodTypes = woodTypes.Skip(13).ToList();

        if (notCrateWoodTypes == null)
        {
            return patches;
        }

        AssetLocation file = new("blocktypes/wood/woodtyped/crate.json");

        int TickCount1 = Environment.TickCount;

        foreach (string type in notCrateWoodTypes)
        {
            try
            {
                patches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = new { @base = $"game:block/wood/planks/{type}1", rotation = 90 }.Parse(),
                    Path = $"/textures/wood-{type}-inside",
                    File = file
                });
                patches.Add(new JsonPatch()
                {
                    Op = EnumJsonPatchOp.Add,
                    Value = new { @base = $"game:block/wood/planks/{type}1", rotation = 90 }.Parse(),
                    Path = $"/textures/wood-{type}-sides",
                    File = file
                });
            }
            catch (Exception e)
            {
                api.Logger.Error(Namespace + ": Failed to patch file {0}: {1}", file, e);
            }
        }

        api.Logger.Debug(Namespace + ": {0} took {1} ms", nameof(CreatePatches) + " for crate textures", Environment.TickCount - TickCount1);

        return patches;
    }
}
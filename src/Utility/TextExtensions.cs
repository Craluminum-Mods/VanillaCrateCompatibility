using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;

namespace VanillaCrateCompatibility;

public static class TextExtensions
{
    public static JsonObject Parse(this object obj)
    {
        return new(JToken.FromObject(obj));
    }
}
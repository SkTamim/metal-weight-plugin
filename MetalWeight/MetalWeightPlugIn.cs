using Rhino.PlugIns;

namespace MetalWeight
{
    public class MetalWeightPlugIn : PlugIn
    {
        public static MetalWeightPlugIn Instance { get; private set; }

        public MetalWeightPlugIn()
        {
            Instance = this;
        }
    }
}
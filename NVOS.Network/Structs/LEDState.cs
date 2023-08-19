using NVOS.Network.gRPC;

namespace NVOS.Network.Structs
{
    public class LEDState
    {
        public bool PoweredOn;
        public float Brightness;
        public LEDMode Mode;

        public LEDState(bool poweredOn, float brightness, LEDMode mode)
        {
            PoweredOn = poweredOn;
            Brightness = brightness;
            Mode = mode;
        }
    }
}

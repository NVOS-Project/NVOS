namespace NVOS.Network.Structs
{
    public class GpsLocation
    {
        public double Latitude;
        public double Longitude;

        public GpsLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}

namespace NVOS.Network.Structs
{
    public class GpsReport
    {
        public bool HasFix;
        public double Latitude;
        public double Longitude;
        public float Altitude;
        public float SpeedOverGround;
        public float Heading;
        public uint SatelliteCount;
        public float VerticalAccuracy;
        public float HorizontalAccuracy;

        public GpsReport(bool hasFix, double latitude, double longitude, float altitude, float speedOverGround, float heading, uint satelliteCount, float verticalAccuracy, float horizontalAccuracy)
        {
            HasFix = hasFix;
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            SpeedOverGround = speedOverGround;
            Heading = heading;
            SatelliteCount = satelliteCount;
            VerticalAccuracy = verticalAccuracy;
            HorizontalAccuracy = horizontalAccuracy;
        }
    }
}

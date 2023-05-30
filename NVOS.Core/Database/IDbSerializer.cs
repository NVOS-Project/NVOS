namespace NVOS.Core.Database
{
    public interface IDbSerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] obj);
    }
}

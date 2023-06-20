namespace NVOS.Core.Database.Serialization
{
    public interface IDbValueSerializer
    {
        string Serialize(object obj);
        object Deserialize(string obj);
    }
}

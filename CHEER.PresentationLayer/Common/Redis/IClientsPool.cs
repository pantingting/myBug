
namespace Redis
{
  public interface IClientsPool<T>
  {
    IRedisClient<T> GetClient();
  }
}

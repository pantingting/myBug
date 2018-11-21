
using System;

namespace Redis.Executors
{
  internal interface IExecutor : IDisposable
  {
    RedisClientBase Client { get; }

    void Init(RedisClientBase client);
    void Init(IExecutor previous);

    T Execute<T>(Invocation<T> invocation);
  }
}

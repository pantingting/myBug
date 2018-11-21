﻿
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Redis.Serialization
{
  public class ObjectSerializer : ObjectSerializer<object>
  {
  }

  public class ObjectSerializer<T> : SerializerBase<T>
  {
    private BinaryFormatter _formatter;
    private MemoryStream _mem;

    protected override void OnInit()
    {
      _formatter = new BinaryFormatter();
      _mem = new MemoryStream(Settings.SerializationBufferSize);
    }


    public override T Read(Stream src, int length)
    {
      using (var limiter = new LimitingStream(src, length))
        return (T)_formatter.Deserialize(limiter);
    }


    public override int GetBytesNeeded(T obj)
    {
      _mem.SetLength(0);
      _formatter.Serialize(_mem, obj);

      _mem.Seek(0, SeekOrigin.Begin);
      return (int)_mem.Length;
    }

    public override void Write(T obj, Stream dest, int bytesNeeded)
    {
      _mem.CopyTo(dest);
    }
  }
}

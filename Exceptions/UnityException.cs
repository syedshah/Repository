using System;
using System.Runtime.Serialization;

namespace Exceptions
{
  public class UnityException : Exception
  {
    public UnityException()
    {
    }

    public UnityException(string message)
      : base(message)
    {
    }

    public UnityException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected UnityException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}


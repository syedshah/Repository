namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class UnityUpdateItemException : UnityException
  {
    public UnityUpdateItemException()
    {
    }

    public UnityUpdateItemException(string message)
      : base(message)
    {
    }

    public UnityUpdateItemException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected UnityUpdateItemException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

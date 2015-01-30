namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class UnableToCheckOutDocumentException : UnityException
  {
    public UnableToCheckOutDocumentException()
    {
    }

    public UnableToCheckOutDocumentException(string message)
      : base(message)
    {
    }

    public UnableToCheckOutDocumentException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected UnableToCheckOutDocumentException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

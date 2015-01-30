namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class DocumentAlreadyRejectedException : UnityException
  {
    public DocumentAlreadyRejectedException()
    {
    }

    public DocumentAlreadyRejectedException(string message)
      : base(message)
    {
    }

    public DocumentAlreadyRejectedException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocumentAlreadyRejectedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

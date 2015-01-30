namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class DocumentCurrentlyCheckedOutException : UnityException
  {
    public DocumentCurrentlyCheckedOutException()
    {
    }

    public DocumentCurrentlyCheckedOutException(string message)
      : base(message)
    {
    }

    public DocumentCurrentlyCheckedOutException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocumentCurrentlyCheckedOutException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

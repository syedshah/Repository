namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class UnableToRetrieveDocumentException : UnityException
  {
    public UnableToRetrieveDocumentException()
    {
    }

    public UnableToRetrieveDocumentException(string message)
      : base(message)
    {
    }

    public UnableToRetrieveDocumentException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected UnableToRetrieveDocumentException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

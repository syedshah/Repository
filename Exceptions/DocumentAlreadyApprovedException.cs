namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class DocumentAlreadyApprovedException : UnityException
  {
    public DocumentAlreadyApprovedException()
    {
    }

    public DocumentAlreadyApprovedException(string message)
      : base(message)
    {
    }

    public DocumentAlreadyApprovedException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocumentAlreadyApprovedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

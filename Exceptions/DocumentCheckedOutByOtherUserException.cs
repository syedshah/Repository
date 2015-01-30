namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class DocumentCheckedOutByOtherUserException : UnityException
  {
    public DocumentCheckedOutByOtherUserException()
    {
    }

    public DocumentCheckedOutByOtherUserException(string message)
      : base(message)
    {
    }

    public DocumentCheckedOutByOtherUserException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected DocumentCheckedOutByOtherUserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

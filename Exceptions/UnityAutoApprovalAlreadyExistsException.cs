namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class UnityAutoApprovalAlreadyExistsException : UnityException
  {
    public UnityAutoApprovalAlreadyExistsException()
    {
    }

    public UnityAutoApprovalAlreadyExistsException(string message)
      : base(message)
    {
    }

    public UnityAutoApprovalAlreadyExistsException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected UnityAutoApprovalAlreadyExistsException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

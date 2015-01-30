namespace Exceptions
{
  using System;
  using System.Runtime.Serialization;

  public class BasketContainsDifferentManCoException : UnityException
  {
    public BasketContainsDifferentManCoException()
    {
    }

    public BasketContainsDifferentManCoException(string message)
      : base(message)
    {
    }

    public BasketContainsDifferentManCoException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected BasketContainsDifferentManCoException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

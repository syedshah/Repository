namespace Repository
{
  using System;
  using System.Runtime.Serialization;

  [Serializable]
  public class RepositoryException : Exception
  {
    public RepositoryException()
    {
    }

    public RepositoryException(string message)
      : base(message)
    {
    }

    public RepositoryException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected RepositoryException(
        SerializationInfo info,
        StreamingContext context)
      : base(info, context)
    {
    }
  }
}

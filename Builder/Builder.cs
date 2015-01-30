using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
  public class Builder<T> where T : new()
  {
    protected static T Instance;

    protected Builder()
    {
      new T();
    }

    public Builder<T> With(Action<T> action)
    {
      action(Instance);
      return this;
    }

    public static implicit operator T(Builder<T> builder)
    {
      return builder.This();
    }

    public T This()
    {
      return Instance;
    }
  }
}

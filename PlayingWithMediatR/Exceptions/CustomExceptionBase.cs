using System;
using Serilog;

namespace PlayingWithMediatR.Exceptions
{
  public abstract class CustomExceptionBase : Exception
  {
    public bool IsAlreadyWritten { get; set; }


    public CustomExceptionBase() : base() { }

    public CustomExceptionBase(string message) : base(message) { }

    public CustomExceptionBase(string message, Exception exception) : base(message, exception) { }


    public virtual void LogErrorIfSo(string message, object[] objParams = null, bool forceToLog = false)
    {
      if (IsAlreadyWritten && !forceToLog) return;

      Log.Error(this, message, objParams);

      IsAlreadyWritten = true;
    }
  }
}

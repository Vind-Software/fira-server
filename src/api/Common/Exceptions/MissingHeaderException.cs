namespace FiraServer.api.Common.Exceptions;

public class MissingHeaderException : Exception {
    public MissingHeaderException(){}

    public MissingHeaderException(string Message) : base(Message){}

    public MissingHeaderException(string Message, Exception InnerException) : base(Message, InnerException){}
}
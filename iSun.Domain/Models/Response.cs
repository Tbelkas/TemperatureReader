namespace iSun.Domain.Models;

public interface IResponse
{
    public List<string>? errorMessages { get; set; }
    public bool isSuccess => errorMessages == null;
}
public record Response : IResponse
{
    public List<string>? errorMessages { get; set; }
    public bool isSuccess => errorMessages == null;

    public static Response Success()
    {
        return new Response();
    }

    public static Response Error(List<string> errors)
    {
        return new Response { errorMessages = errors };
    }

    public static Response Error(string error)
    {
        return new Response { errorMessages = new List<string> { error } };
    }
}

public record Response<T> : IResponse
{
    public T Data { get; set; }
    public List<string>? errorMessages { get; set; }
    public bool isSuccess => errorMessages == null;

    public static Response<T> Success(T data)
    {
        return new Response<T> { Data = data };
    }

    public static Response<T> Error(List<string> errors)
    {
        return new Response<T> { errorMessages = errors };
    }

    public static Response<T> Error(string error)
    {
        return new Response<T> { errorMessages = new List<string> { error } };
    }
}
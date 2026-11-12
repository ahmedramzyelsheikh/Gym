namespace GYM.BLL.Common
{
    // Every service returns a Result so controllers can check IsSuccess / ErrorMessage.
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string ErrorMessage { get; protected set; } = string.Empty;

        public static Result Success() => new() { IsSuccess = true };
        public static Result Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; } = default!;

        public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static new Result<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }
}

namespace ModularPatternTraining.Shared.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        public int StatusCode { get; private set; } = 200;
        public T? Data { get; private set; }

        private Result(bool isSuccess, T? data, string errorMessage , int status)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            StatusCode = status;
        }

        public static Result<T> Success(T data) => new Result<T>(true, data, string.Empty , 200);
        public static Result<T> Failure(string errorMessage , int status) => new Result<T>(false, default, errorMessage , status);
    }
}

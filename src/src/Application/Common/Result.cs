namespace Collection10Api.src.Application.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public object? Errors { get; set; }

    public static Result<T> Failure(object errors, string message = "Erro de validação")
        => new() { Success = false, Errors = errors, Message = message };

    public static Result<T> Ok(T data, string message = "Operação realizada com sucesso")
        => new() { Success = true, Data = data, Message = message };
}

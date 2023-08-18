using Playstudios.Common.Enumerations;

namespace Playstudios.Common.Models
{
    public class ManagerResult<T>
    {
        public static ManagerResult<T> FromSuccess(T value) => new ManagerResult<T> { DidSucceed = true, Value = value };

        public static ManagerResult<T> FromError(ErrorCodesEnum errorCode) => new ManagerResult<T> { DidSucceed = false, ErrorCode = errorCode };

        public T? Value { get; set; }

        public ErrorCodesEnum? ErrorCode { get; set; }

        public bool DidSucceed { get; set; }
    }
}

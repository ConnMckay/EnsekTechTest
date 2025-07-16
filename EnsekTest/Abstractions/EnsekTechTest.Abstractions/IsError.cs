namespace EnsekTechTest.Abstractions
{
    public sealed class IsError
    {
        public IsError(string code, string description = null)
        {
            
        }

        public static readonly IsError None = new(string.Empty);

        public static implicit operator OutcomeResult(IsError error) => OutcomeResult.Failure(error);
    }
}

namespace EnsekTechTest.Abstractions
{
    public sealed class IsError
    {
        public IsError(string code, string description = null)
        {
            Code = code;
            Description = description;
        }
        public string Code { get; set; }
        public string Description { get; set; }

        public static readonly IsError None = new(string.Empty);

        public static implicit operator OutcomeResult(IsError error) => OutcomeResult.Failure(error);
    }
}

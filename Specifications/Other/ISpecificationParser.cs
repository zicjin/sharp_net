namespace zic_dotnet.Specifications {

    public interface ISpecificationParser<TCriteria> {

        TCriteria Parse<T>(ISpecification<T> specification);
    }
}
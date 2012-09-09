namespace sharp_net.Specifications {

    public interface ISpecificationParser<TCriteria> {

        TCriteria Parse<T>(ISpecification<T> specification);
    }
}
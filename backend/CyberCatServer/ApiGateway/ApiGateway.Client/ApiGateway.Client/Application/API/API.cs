using ApiGateway.Client.Application.UseCases;

namespace ApiGateway.Client.Application.API
{
    public abstract class API
    {
        private readonly TinyIoCContainer _container;

        public API(TinyIoCContainer container)
        {
            _container = container;
        }

        public TUseCase GetUseCase<TUseCase>() where TUseCase : class, IUseCase
        {
            return _container.Resolve<TUseCase>();
        }
    }
}
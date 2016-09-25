namespace AmpedBiz.Common.Pipes
{
    public abstract class Filter<T> : IFilter<T>
    {
        private IFilter<T> _next;

        protected abstract T Process(T input);

        public T Execute(T input)
        {
            var result = Process(input);

            if (_next != null)
                result = _next.Execute(result);

            return result;
        }

        public void Register(IFilter<T> filter)
        {
            if (_next == null)
                _next = filter;

            else
                _next.Register(filter);
        }
    }
}

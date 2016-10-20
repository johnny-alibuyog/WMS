namespace AmpedBiz.Common.Pipes
{
    public abstract class Step<T> : IStep<T>
    {
        private IStep<T> _next;

        protected abstract T Process(T input);

        public T Execute(T input)
        {
            var result = Process(input);

            if (_next != null)
                result = _next.Execute(result);

            return result;
        }

        public void Register(IStep<T> step)
        {
            if (_next == null)
                _next = step;

            else
                _next.Register(step);
        }
    }
}

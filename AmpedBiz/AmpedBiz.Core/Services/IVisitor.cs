﻿namespace AmpedBiz.Core.Services
{
    public interface IVisitor { }

    public interface IVisitor<T> : IVisitor
    {
        void Visit(T target);
    }

    public interface IVisitor<T, R> : IVisitor
    {
        R Visit(T target);
    }
}

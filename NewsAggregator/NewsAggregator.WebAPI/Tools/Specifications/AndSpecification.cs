﻿using NewsAggregator.WebAPI.Tools.Interfaces;

namespace NewsAggregator.WebAPI.Tools.Specifications
{
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T article) => _left.IsSatisfiedBy(article) && _right.IsSatisfiedBy(article);
    }
}

﻿using System;
using System.Linq.Expressions;
using GuardExtensions;
using LambdaSqlBuilder;

// ReSharper disable CheckNamespace

namespace SqlSelectBuilder
{
    public class RestrictedSqlFilter<TEntity> : SqlFilterBase<TEntity>
    {
        internal RestrictedSqlFilter(ImmutableList<ISqlFilterItem> sqlFilterItems) : base(sqlFilterItems)
        {
        }

        private RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> AddFilter<TType>(
            ImmutableList<ISqlFilterItem> items, LambdaExpression field, SqlAlias<TEntity> alias)
        {
            Guard.IsNotNull(field);
            return new RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>>(items, BuildSqlFilterItem(field, alias));
        }

        private RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> AddFilter<TType>(
            ImmutableList<ISqlFilterItem> items, SqlField<TEntity> field)
        {
            Guard.IsNotNull(field);
            return new RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>>(items, BuildSqlFilterItem(field));
        }

        public static RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> From<TType>(
            Expression<Func<TEntity, TType>> field, SqlAlias<TEntity> alias = null)
        {
            Guard.IsNotNull(field);
            return new RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>>(
                ImmutableList<ISqlFilterItem>.Empty, BuildSqlFilterItem(field, alias));
        }

        public static RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> From<TType>(SqlField<TEntity> field)
        {
            Guard.IsNotNull(field);
            return new RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>>(
                ImmutableList<ISqlFilterItem>.Empty, BuildSqlFilterItem(field));
        }

        public RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> And<TType>(Expression<Func<TEntity, TType>> field, SqlAlias<TEntity> alias = null)
        {
            Guard.IsNotNull(field);
            return AddFilter<TType>(FilterItems.Add(SqlFilterItems.And), field, alias);
        }

        public RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> Or<TType>(Expression<Func<TEntity, TType>> field, SqlAlias<TEntity> alias = null)
        {
            Guard.IsNotNull(field);
            return AddFilter<TType>(FilterItems.Add(SqlFilterItems.Or), field, alias);
        }

        public RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> And<TType>(SqlField<TEntity> field)
        {
            Guard.IsNotNull(field);
            return AddFilter<TType>(FilterItems.Add(SqlFilterItems.And), field);
        }

        public RestrictedSqlFilterField<TEntity, TType, RestrictedSqlFilter<TEntity>> Or<TType>(SqlField<TEntity> field)
        {
            Guard.IsNotNull(field);
            return AddFilter<TType>(FilterItems.Add(SqlFilterItems.Or), field);
        }

        public RestrictedSqlFilter<TEntity> And(RestrictedSqlFilter<TEntity> filter)
        {
            Guard.IsNotNull(filter);
            var items = FilterItems
                .Add(SqlFilterItems.And)
                .AddRange(filter.FilterItems);
            return new RestrictedSqlFilter<TEntity>(items);
        }

        public RestrictedSqlFilter<TEntity> Or(RestrictedSqlFilter<TEntity> filter)
        {
            Guard.IsNotNull(filter);
            var items = FilterItems
                .Add(SqlFilterItems.Or)
                .AddRange(filter.FilterItems);
            return new RestrictedSqlFilter<TEntity>(items);
        }

        public RestrictedSqlFilter<TEntity> AndGroup(RestrictedSqlFilter<TEntity> filter)
        {
            Guard.IsNotNull(filter);
            var items = FilterItems
                .Add(SqlFilterItems.And)
                .Add(SqlFilterItems.Build("("))
                .AddRange(filter.FilterItems)
                .Add(SqlFilterItems.Build(")"));
            return new RestrictedSqlFilter<TEntity>(items);
        }

        public RestrictedSqlFilter<TEntity> OrGroup(RestrictedSqlFilter<TEntity> filter)
        {
            Guard.IsNotNull(filter);
            var items = FilterItems
                .Add(SqlFilterItems.Or)
                .Add(SqlFilterItems.Build("("))
                .AddRange(filter.FilterItems)
                .Add(SqlFilterItems.Build(")"));
            return new RestrictedSqlFilter<TEntity>(items);
        }

        public RestrictedSqlFilter<TEntity> WithoutAliases()
        {
            var filter = new RestrictedSqlFilter<TEntity>(FilterItems);
            filter.MustBeWithoutAliases = true;
            return filter;
        }

        public RestrictedSqlFilter<TEntity> WithAliases()
        {
            var filter = new RestrictedSqlFilter<TEntity>(FilterItems);
            filter.MustBeWithoutAliases = false;
            return filter;
        }
    }
}
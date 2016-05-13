﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Shareds.Promises
{
    public interface IPromise
    {
        IPromise Done(Action callback);
        IPromise Fail(Action callback);
        IPromise Always(Action callback);

        bool IsRejected { get; }
        bool IsResolved { get; }
        bool IsFulfilled { get; }
    }

    public interface IPromise<T> : IPromise
    {
        IPromise<T> Done(Action<T> callback);
        IPromise<T> Done(IEnumerable<Action<T>> callbacks);

        IPromise<T> Fail(Action<T> callback);
        IPromise<T> Fail(IEnumerable<Action<T>> callbacks);

        IPromise<T> Always(Action<T> callback);
        IPromise<T> Always(IEnumerable<Action<T>> callbacks);
    }
}

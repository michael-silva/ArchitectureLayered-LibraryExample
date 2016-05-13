/*
  modified from original source: https://bitbucket.org/mattkotsenas/c-promises/overview
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Domain.Shareds.Promises
{   

    public class Deferred : Deferred<object>
    {
        // generic object
    }

    public class Deferred<T> : IPromise<T>
    {
        private List<Callback> callbacks = new List<Callback>();
        protected bool _isResolved = false;
        protected bool _isRejected = false;
        private T _arg;

        public static IPromise When(IEnumerable<IPromise> promises)
        {
            var count = 0;
            var masterPromise = new Deferred();

            foreach (var p in promises)
            {
                count++;
                p.Fail(() =>
                {
                    masterPromise.Reject();
                });
                p.Done(() =>
                {
                    count--;
                    if (0 == count)
                    {
                        masterPromise.Resolve();
                    }
                });
            }

            return masterPromise;
        }

        public static IPromise When(object d)
        {
            var masterPromise = new Deferred();
            masterPromise.Resolve();
            return masterPromise;
        }

        public static IPromise When(Deferred d)
        {
            return d.Promise();
        }

        public static IPromise<T> When(Deferred<T> d)
        {
            return d.Promise();

        }

        public IPromise<T> Promise()
        {
            return this;
        }

        public IPromise Always(Action callback)
        {
            if (_isResolved || _isRejected)
                callback();
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Always, false));
            return this;
        }

        public IPromise<T> Always(Action<T> callback)
        {
            if (_isResolved || _isRejected)
                callback(_arg);
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Always, true));
            return this;
        }

        public IPromise<T> Always(IEnumerable<Action<T>> callbacks)
        {
            foreach (Action<T> callback in callbacks)
                this.Always(callback);
            return this;
        }

        public IPromise Done(Action callback)
        {
            if (_isResolved)
                callback();
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Success, false));
            return this;
        }

        public IPromise<T> Done(Action<T> callback)
        {
            if (_isResolved)
                callback(_arg);
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Success, true));
            return this;
        }

        public IPromise<T> Done(IEnumerable<Action<T>> callbacks)
        {
            foreach (Action<T> callback in callbacks)
                this.Done(callback);
            return this;
        }

        public IPromise Fail(Action callback)
        {
            if (_isRejected)
                callback();
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Fail, false));
            return this;
        }

        public IPromise<T> Fail(Action<T> callback)
        {
            if (_isRejected)
                callback(_arg);
            else
                callbacks.Add(new Callback(callback, Callback.Condition.Fail, true));
            return this;
        }

        public IPromise<T> Fail(IEnumerable<Action<T>> callbacks)
        {
            foreach (Action<T> callback in callbacks)
                this.Fail(callback);
            return this;
        }

        public bool IsRejected
        {
            get { return _isRejected; }
        }

        public bool IsResolved
        {
            get { return _isResolved; }
        }

        public bool IsFulfilled
        {
            get { return _isRejected || _isResolved; }
        }

        public IPromise Reject()
        {
            if (_isRejected || _isResolved) // ignore if already rejected or resolved
                return this;
            _isRejected = true;
            DequeueCallbacks(Callback.Condition.Fail);
            return this;
        }

        public Deferred<T> Reject(T arg)
        {
            if (_isRejected || _isResolved) // ignore if already rejected or resolved
                return this;
            _isRejected = true;
            this._arg = arg;
            DequeueCallbacks(Callback.Condition.Fail);
            return this;
        }

        public IPromise Resolve()
        {
            if (_isRejected || _isResolved) // ignore if already rejected or resolved
                return this;
            this._isResolved = true;
            DequeueCallbacks(Callback.Condition.Success);
            return this;
        }

        public Deferred<T> Resolve(T arg)
        {
            if (_isRejected || _isResolved) // ignore if already rejected or resolved
                return this;
            this._isResolved = true;
            this._arg = arg;
            DequeueCallbacks(Callback.Condition.Success);
            return this;
        }

        private void DequeueCallbacks(Callback.Condition cond)
        {
            foreach (Callback callback in callbacks)
            {
                if (callback.Cond == cond || callback.Cond == Callback.Condition.Always)
                {
                    if (callback.IsReturnValue)
                        callback.Del.DynamicInvoke(_arg);
                    else
                        callback.Del.DynamicInvoke();
                }
            }
            callbacks.Clear();
        }
    }

    class Callback
    {
        public enum Condition { Always, Success, Fail };

        public Callback(Delegate del, Condition cond, bool returnValue)
        {
            Del = del;
            Cond = cond;
            IsReturnValue = returnValue;
        }

        public bool IsReturnValue { get; private set; }
        public Delegate Del { get; private set; }
        public Condition Cond { get; private set; }

    }
}
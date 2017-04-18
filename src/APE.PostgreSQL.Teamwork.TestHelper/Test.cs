// <copyright file="Test.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using APE.CodeGeneration.Attributes;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using StructureMap.AutoMocking.Moq;

namespace APE.PostgreSQL.Teamwork.TestHelper
{
    /// <summary>
    /// Class that helps unit testing classes with Constructors that use DI.
    /// </summary>
    /// <typeparam name="T">The class which should be tested.</typeparam>
    [Disposable]
    public partial class Test<T>
        where T : class
    {
        private readonly List<Mock> knownMocks = new List<Mock>();

        private readonly Dictionary<string, bool> flags = new Dictionary<string, bool>();

        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        public Test(bool mockNow = false)
        {
            this.AutoMocker = new MoqAutoMocker<T>();
        }

        public T Target
        {
            get
            {
                return this.AutoMocker.ClassUnderTest;
            }
        }

        public MoqAutoMocker<T> AutoMocker { get; private set; }

        /* Mocking */

        public void Inject<TMock>(TMock instance)
        {
            this.AutoMocker.Inject(typeof(TMock), instance);
        }

        public void Raise<TMock>(Action<TMock> eventExpression, EventArgs args)
            where TMock : class
        {
            Mock.Get(this.AutoMocker.Get<TMock>()).Raise(eventExpression, args);
        }

        public ISetupGetter<TMock, TProperty> SetupGet<TMock, TProperty>(Expression<Func<TMock, TProperty>> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            return mock.SetupGet(exp);
        }

        public ISetup<TMock> Setup<TMock>(Expression<Action<TMock>> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            return mock.Setup(exp);
        }

        public ISetup<TMock, TResult> Setup<TMock, TResult>(Expression<Func<TMock, TResult>> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            return mock.Setup(exp);
        }

        public void ShouldNotHaveCalled<TMock>(Expression<Action<TMock>> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            mock.Verify(exp, Times.Never());
        }

        public void ShouldHaveCalled<TMock>(Expression<Action<TMock>> exp, int times)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            mock.Verify(exp, Times.Exactly(times));
        }

        public void ShouldNotHaveCalled<TMock, TResult>(Expression<Func<TMock, TResult>> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            mock.Verify(exp, Times.Never());
        }

        public void ShouldHaveCalled<TMock, TResult>(Expression<Func<TMock, TResult>> exp, int times)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            mock.Verify(exp, Times.Exactly(times));
        }

        public void VerifyAll()
        {
            this.knownMocks.ForEach(x => x.Verify());
        }

        public ISetup<TMock> SetupSet<TMock>(Action<TMock> exp)
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            return mock.SetupSet(exp);
        }

        public TMock Get<TMock>()
            where TMock : class
        {
            var mock = Mock.Get(this.AutoMocker.Get<TMock>());
            if (!this.knownMocks.Contains(mock))
            {
                this.knownMocks.Add(mock);
            }

            return mock.Object;
        }

        /* Flags */

        public void SetFlag(string name)
        {
            this.flags[name] = true;
        }

        public void ResetFlag(string name)
        {
            this.flags.Remove(name);
        }

        public bool HasFlag(string name)
        {
            return this.flags.ContainsKey(name);
        }

        public void ShouldHaveFlag(string name)
        {
            this.HasFlag(name).Should().BeTrue();
        }

        public void ShouldNotHaveFlag(string name)
        {
            this.HasFlag(name).Should().BeFalse();
        }

        /* Objects */

        public void Set(string name, object value)
        {
            this.objects[name] = value;
        }

        public TVal Get<TVal>(string name)
        {
            if (!this.objects.ContainsKey(name))
            {
                throw new InvalidOperationException(string.Format("'{0}' is not set", name));
            }

            var value = this.objects[name];
            if (value == null && default(TVal) == null)
            {
                return (TVal)value;
            }

            if (value is TVal)
            {
                return (TVal)value;
            }

            throw new InvalidOperationException(string.Format("Expected '{0}' to be a '{1}' but it's a '{2}'", name, typeof(TVal).Name, value.GetType().Name));
        }

        partial void Dispose(bool threadSpecificCleanup)
        {
            this.VerifyAll();

            if (threadSpecificCleanup)
            {
                var disposable = this.Target as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }
}

using System;

namespace ClinicHQ.Tests.Common
{
    public abstract class BaseTestCase<DerivedClassType, ExpectedResultType, TParams, TGiven> where DerivedClassType : BaseTestCase<DerivedClassType, ExpectedResultType, TParams, TGiven>
    {
        public string Description { get; private set; }
        public TParams TestInput { get; private set; }
        public TGiven Given { get; set; }
        public ExpectedResultType Expects { get; private set; }
        private Action<ExpectedResultType, ExpectedResultType> Assert { get; set; }
        private Action AssertVoid { get; set; }

        public Action<Exception> ExceptionAssert { get; private set; }


        public DerivedClassType WhenGiven(TGiven given)
        {
            Given = given;
            return this as DerivedClassType;
        }

        public DerivedClassType WhenCalledWith(TParams parameters)
        {
            TestInput = parameters;
            return this as DerivedClassType;
        }

        public DerivedClassType Expect(ExpectedResultType expectedResult)
        {
            Expects = expectedResult;
            return this as DerivedClassType;
        }

        public DerivedClassType DescriptionIs(string desc)
        {
            Description = desc;
            return this as DerivedClassType;
        }

        public DerivedClassType Asserts(Action<ExpectedResultType, ExpectedResultType> lambda)
        {
            Assert = lambda;
            return this as DerivedClassType;
        }

        public DerivedClassType Asserts(Action lambda)
        {
            AssertVoid = lambda;
            return this as DerivedClassType;
        }

        public DerivedClassType AssertsException(Action<Exception> lambda)
        {
            ExceptionAssert = lambda;
            return this as DerivedClassType;
        }

        public void DoAsserts(ExpectedResultType actual)
        {
            Assert?.Invoke(Expects, actual);
            AssertVoid?.Invoke();
        }

        public void DoAssertException(Exception e)
        {
            ExceptionAssert?.Invoke(e);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}

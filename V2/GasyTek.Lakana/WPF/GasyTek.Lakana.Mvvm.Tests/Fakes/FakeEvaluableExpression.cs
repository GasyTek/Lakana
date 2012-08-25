using System;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation.Fluent;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    /// <summary>
    /// An expression node that sleeps the working thread for a while to simulate asynchronous evaluation.
    /// </summary>
    class FakeEvaluableExpression : ExpressionNode
    {
        private readonly bool _evaluableValue;
        private readonly TimeSpan _sleepDuration;

        public FakeEvaluableExpression(bool evaluableValue, TimeSpan sleepDuration)
        {
            _evaluableValue = evaluableValue;
            _sleepDuration = sleepDuration;
        }

        public FakeEvaluableExpression(bool evaluableValue)
            : this(evaluableValue, TimeSpan.Zero)
        {
        }

        public FakeEvaluableExpression() 
            : this(true, TimeSpan.Zero)
        {
        }

        public override Task<bool> Evaluate()
        {
            var evaluableTask = new Task<bool>(() => _evaluableValue);
            if (_sleepDuration != TimeSpan.Zero) evaluableTask.Wait(_sleepDuration);
            return evaluableTask;
        }
    }
}

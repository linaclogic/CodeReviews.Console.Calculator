namespace CalculatorLibrary
{    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Power,
        Root
    }

    public class Calculator
    {
        public int UsageCounter { get; private set; } = 0;

        private List<Calculation> calculationHistory = [];
        public IReadOnlyCollection<Calculation> CalculationHistory
        {
            get { return calculationHistory.AsReadOnly(); }
        }

        public Calculation DoOperation(double num1, double num2, Operation op)
        {
            Calculation calculation = new(num1, num2, op);

            calculationHistory.Add(calculation);
            UsageCounter++;

            return calculation;
        }

        public int ClearHistory()
        {   
            int historyCount = CalculationHistory.Count;
            calculationHistory.Clear();
            return historyCount;
        }
    }
}

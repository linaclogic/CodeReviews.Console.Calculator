namespace CalculatorLibrary
{
    public class Calculation
    {
        private readonly double operand1;
        private readonly double operand2;

        private readonly Operation operation;
        private readonly string? operatorSymbol;

        public double Result { get; }

        internal Calculation(double num1, double num2, Operation op)
        {
            operand1 = num1;
            operand2 = num2;

            operation = op;

            switch (op)
            {
                case Operation.Add:
                    Result = num1 + num2;
                    operatorSymbol = "+";
                    break;

                case Operation.Subtract:
                    Result = num1 - num2;
                    operatorSymbol = "-";
                    break;

                case Operation.Multiply:
                    Result = num1 * num2;
                    operatorSymbol = "*";
                    break;

                case Operation.Divide:
                    Result = num1 / num2;
                    operatorSymbol = "/";
                    break;

                case Operation.Power:
                    Result = Math.Pow(num1, num2);
                    operatorSymbol = "^";
                    break;

                case Operation.Root:
                    Result = Math.Pow(num1, 1.0 / num2);
                    break;

                default:
                    throw new NotImplementedException($"Handling for value {op} of enum {typeof(Operation)} has not been implemented.");
            }
        }

        public override string ToString()
        {
            string returnValue;

            switch (operation)
            {
                case Operation.Root:
                    returnValue = $"{operand1} ^ (1 / {operand2}) = {Result}";
                    break;
                default:
                    returnValue = $"{operand1} {operatorSymbol} {operand2} = {Result}";
                    break;

            }

            return returnValue; 
        }

    }
}

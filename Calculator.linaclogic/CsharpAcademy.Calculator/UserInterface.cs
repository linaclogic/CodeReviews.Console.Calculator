using CalculatorLibrary;
using Spectre.Console;

namespace CalculatorProgram
{
    internal class UserInterface
    {
        private readonly Calculator calculator = new();
        private Calculation? previousCalculation = null;

        private enum MainMenuOption
        {
            Calculate,
            ShowHistory,
            UseHistory,
            ClearHistory,
            Exit
        }
        private enum CalcMenuOption
        {
            New,
            Reuse,
            Exit
        }

        private readonly string[] firstOperandNames = ["First summand", "Minuend", "First Factor", "Dividend", "Base", "Radicand"];
        private readonly string[] secondOperandNames = ["Second summand", "Subtrahend", "Second Factor", "Divisor", "Exponent", "Degree"];

        private readonly string[] calcMenuDisplayString = ["Yes", "Yes and reuse current result", "No"];

        internal void MainMenu()
        {
            bool runApp = true;

            while (runApp)
            {
                DisplayTitle();

                MainMenuOption menuSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOption>()
                    .Title("Choose an option:")
                    .AddChoices(Enum.GetValues<MainMenuOption>()));

                switch (menuSelection)
                {
                    case MainMenuOption.Calculate:
                        CalculatorMenu();
                        break;

                    case MainMenuOption.ShowHistory:
                        ShowHistory();
                        break;

                    case MainMenuOption.UseHistory:
                        bool historySelected = UseHistory();
                        if (historySelected)
                            CalculatorMenu();
                        break;

                    case MainMenuOption.ClearHistory:
                        ClearHistory();
                        break;

                    case MainMenuOption.Exit:
                        runApp = false;
                        break;

                    default:
                        throw new NotImplementedException($"Handling for value {menuSelection} of enum {typeof(MainMenuOption)} has not been implemented.");
                }
            }
        }

        private void CalculatorMenu()
        {

            bool runCalculator = true;

            while (runCalculator)
            {
                DisplayTitle();

                if (previousCalculation != null)
                    AnsiConsole.MarkupLine($"Using previous calculation as input: [bold]{previousCalculation}[/]\n");

                Operation selectedOperation = AnsiConsole.Prompt(
                    new SelectionPrompt<Operation>()
                    .Title("Select operation:")
                    .AddChoices(Enum.GetValues<Operation>()));

                (double num1, double num2) = PromptNumbers(selectedOperation);

                Calculation calculation = calculator.DoOperation(num1, num2, selectedOperation);
                AnsiConsole.MarkupLine($"\n[bold]{calculation}[/]\n");

                CalcMenuOption selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<CalcMenuOption>()
                    .Title("Another calculation?")
                    .AddChoices(Enum.GetValues<CalcMenuOption>())
                    .UseConverter<CalcMenuOption>(x => calcMenuDisplayString[(int)x]));
                
                switch (selectedOption)
                {
                    case CalcMenuOption.New:
                        previousCalculation = null;
                        break;

                    case CalcMenuOption.Reuse:
                        previousCalculation = calculation;
                        break;

                    case CalcMenuOption.Exit:
                        runCalculator = false;
                        break;
                }
            }
        }
        
        private void ShowHistory()
        {
            Table table = new();

            table.AddColumn("#");
            table.AddColumn("Operation");

            for (int i = 0; i < calculator.CalculationHistory.Count; i++)
                table.AddRow(i.ToString(), calculator.CalculationHistory.ElementAt(i).ToString());

            AnsiConsole.Write(table);

            WaitForUser();
        }

        private bool UseHistory()
        {
            if (calculator.CalculationHistory.Count > 0)
            {
                previousCalculation = AnsiConsole.Prompt<Calculation>(
                    new SelectionPrompt<Calculation>()
                    .Title("Choose one of the previous calculations:")
                    .AddChoices(calculator.CalculationHistory));

                return true;
            }
            else
            {
                AnsiConsole.WriteLine("No previous calculations available.");
                WaitForUser();

                return false;
            }
        }

        private void ClearHistory()
        {
            AnsiConsole.MarkupLine($"Histories deleted: {calculator.ClearHistory()}");
            WaitForUser();
        }

        private void DisplayTitle()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold green]Console Calculator in C#[/]");
            AnsiConsole.MarkupLine($"Times used: [yellow]{calculator.UsageCounter}[/]\n");
        }
        
        private (double, double) PromptNumbers (Operation operation)
        {
            double num1;

            if ((int)operation >= firstOperandNames.Length | (int)operation >= secondOperandNames.Length)
                throw new NotImplementedException($"Handling for value {operation} of enum {typeof(Operation)} has not been implemented.");

            // previousCalculation is set to null by default and by CalculatorMenu(), if the user chooses not to reuse the previous result
            if (previousCalculation == null)
            {
                num1 = AnsiConsole.Ask<double>($"Enter the {firstOperandNames[(int)operation].ToLower()}:");
            }
            else
            {
                AnsiConsole.WriteLine($"{firstOperandNames[(int)operation]}: {previousCalculation}");
                num1 = previousCalculation.Result;
            }

            double num2 = AnsiConsole.Ask<double>($"Enter the {secondOperandNames[(int)operation].ToLower()}:");

            return (num1, num2);
        }

        private static void WaitForUser()
        {
            AnsiConsole.MarkupLine("\nPress any key to Continue.");
            Console.ReadKey();
        }
    }
}

import java.util.Map;
import java.util.HashMap;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        // Allow user to input the program to test
        System.out.println("Enter your program (use ';' to separate assignments, e.g., x = 1; y = x + 2;): ");
        String input = scanner.nextLine();  // Read the entire input from the user

        String[] lines = input.split(";");  // Split the input into lines (assignments)

        // Process each assignment
        for (String line : lines) {
            if (line.trim().isEmpty()) continue;  // Skip empty lines
            try {
                // Parse the assignment (e.g., "x = 1")
                String[] assignment = line.split("=");
                String varName = assignment[0].trim();  // Extract the variable name
                String expression = assignment[1].trim();  // Extract the expression

                // Set up the input for the ExpParser
                ExpParser.str = expression;
                ExpParser.curIndex = 0;
                int value = ExpParser.exp();  // Evaluate the expression

                // Store the result in the variables map
                ExpParser.variables.put(varName, value);

            } catch (Exception e) {
                // If an error occurs, output "error" for this line
                System.out.println("error");
                return;  // Stop execution if any error occurs
            }
        }

        // After processing all assignments, print the variable values
        for (Map.Entry<String, Integer> entry : ExpParser.variables.entrySet()) {
            System.out.println(entry.getKey() + " = " + entry.getValue());
        }
    }
}

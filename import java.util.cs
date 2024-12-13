import java.util.HashMap;
import java.util.Map;

class ExpParser {
    static String str;
    static int curIndex;
    
    // Map to store variable names and their values
    static Map<String, Integer> variables = new HashMap<>();
    
    // Get the current token from the input string
    static char currentToken() {
        if (curIndex >= str.length()) {
            return '$';  // End of string
        } else {
            return str.charAt(curIndex);
        }
    }

    // Read the next token (move the index forward)
    static void readNextToken() {
        curIndex++;
    }

    // Method to throw a syntax error
    static void error() {
        throw new RuntimeException("syntax error");
    }

    // Parse an expression (Exp -> Term ExpPrime)
    static int exp() {
        int termResult = term();
        return expPrime(termResult);
    }

    // Handle the '+' or '-' operations (ExpPrime -> (+ | -) Term ExpPrime)
    static int expPrime(int left) {
        char token = currentToken();
        if (token == '+' || token == '-') {
            readNextToken();  // Consume the operator
            int termResult = term();  // Parse the next term
            if (token == '+') {
                return left + termResult;  // Perform addition
            } else {
                return left - termResult;  // Perform subtraction
            }
        }
        return left;  // If no operator, just return the left term's result
    }

    // Parse a term (Term -> Factor TermPrime)
    static int term() {
        int factorResult = factor();  // Parse the first factor
        return termPrime(factorResult);  // Handle multiplication or division
    }

    // Handle '*' or '/' operations (TermPrime -> (* | /) Factor TermPrime)
    static int termPrime(int left) {
        char token = currentToken();
        if (token == '*' || token == '/') {
            readNextToken();  // Consume the operator
            int factorResult = factor();  // Parse the next factor
            if (token == '*') {
                return left * factorResult;  // Perform multiplication
            } else {
                return left / factorResult;  // Perform division
            }
        }
        return left;  // If no operator, just return the left factor's result
    }

    // Parse a factor (Factor -> (Exp) | -Factor | +Factor | Literal | Identifier)
    static int factor() {
        char token = currentToken();
        if (token == '(') {
            readNextToken();
            int result = exp();  // Parse the expression inside parentheses
            if (currentToken() == ')') {
                readNextToken();  // Consume the closing parenthesis
                return result;
            } else {
                error();  // Syntax error if closing parenthesis is missing
            }
        } else if (token == '-') {
            readNextToken();  // Consume the minus sign
            return -factor();  // Parse the next factor and negate it
        } else if (token == '+') {
            readNextToken();  // Consume the plus sign
            return factor();  // Parse the next factor and return it as-is
        } else if (Character.isDigit(token)) {
            return literal();  // Parse a literal number
        } else if (Character.isLetter(token)) {
            return identifier();  // Parse an identifier (variable)
        }
        error();  // Syntax error if none of the valid cases
        return 0;  // Unreachable, just to avoid compilation error
    }

    // Parse a literal number (Literal -> NonZeroDigit Digit*)
    static int literal() {
        StringBuilder num = new StringBuilder();
        char token = currentToken();
        while (Character.isDigit(token)) {
            num.append(token);  // Append digits to form the number
            readNextToken();
            token = currentToken();
        }
        return Integer.parseInt(num.toString());  // Convert the number to an integer
    }

    // Parse an identifier (variable name)
    static int identifier() {
        StringBuilder identifier = new StringBuilder();
        char token = currentToken();
        if (Character.isLetter(token) || token == '_') {
            identifier.append(token);  // Append letters or underscore
            readNextToken();
            token = currentToken();
            // Append more letters, digits, or underscores to the identifier
            while (Character.isLetterOrDigit(token) || token == '_') {
                identifier.append(token);
                readNextToken();
                token = currentToken();
            }
        }
        String varName = identifier.toString();
        // Check if the variable is already initialized
        if (variables.containsKey(varName)) {
            return variables.get(varName);  // Return the value of the variable
        } else {
            error();  // Throw an error if the variable is not initialized
            return 0;  // Unreachable, just to avoid compilation error
        }
    }
}

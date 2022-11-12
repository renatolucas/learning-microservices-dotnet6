using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RestWithASPNET.Controllers;

[ApiController]
[Route("[controller]")]
//a primeira parte da URL (https://localhost:7264/Calculator/sum/2/3), "Calculator" faz o BIND com a classe (retirando a palavra Controller)
//a segunda parte (sum/2/3) chega até o endpoint sum que possui o verbo GET
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    //endpoint que receberá a requisição
    //a requisição será passada no PATH PARAM e o bind da requisição com o endpoint (método) será feito de forma sequencial
    // {firstNumber} BIND firstNumber | {secondNumber} BIND secondNumber
    [HttpGet(Name = "get/{operation}/{firstNumber}/{secondNumber}")]
    public IActionResult Calculator(string operation, string firstNumber, string secondNumber)
    {
        if(IsValidOperation(operation) && IsNumeric(firstNumber) && IsNumeric(secondNumber))
        { 
            switch(operation)
            {
                case "sum":
                    var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);
                    return Ok(sum.ToString());

                case "sub":
                    var sub = ConvertToDecimal(firstNumber) - ConvertToDecimal(secondNumber);
                    return Ok(sub.ToString());

                case "mul":
                    var mul = ConvertToDecimal(firstNumber) * ConvertToDecimal(secondNumber);
                    return Ok(mul.ToString());
                case "div":
                    if (ConvertToDecimal(secondNumber) != 0)
                    {
                        var div = ConvertToDecimal(firstNumber) / ConvertToDecimal(secondNumber);
                        return Ok(div.ToString());
                    }
                    else
                        return BadRequest("Cannot divide by 0");
         
            }                
        }
        //se algum dos valores não for numérico, retorna Bad Request.
        return BadRequest("Invalid Input");
    }

    private decimal ConvertToDecimal(string strNumber)
    {
        decimal decimalValue;
        if (decimal.TryParse(strNumber, out decimalValue))
        {
            return decimalValue;
        }
        return 0;

    }

    private bool IsNumeric(string strNumber)
    {
        double number;
        bool isNumber = double.TryParse(
            strNumber,
            System.Globalization.NumberStyles.Any,
            System.Globalization.NumberFormatInfo.InvariantInfo,
            out number);

        return isNumber;
    }

    private bool IsValidOperation(string operation)
    {
        return String.Equals("sum", operation) || String.Equals("sub", operation) || String.Equals("mul", operation) || String.Equals("div", operation);
    }
}

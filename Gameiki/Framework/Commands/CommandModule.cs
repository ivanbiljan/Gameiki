using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Gameiki.Extensions;
using Gameiki.Framework.Commands.ArgumentValidators;
using ReLogic.Utilities;

namespace Gameiki.Framework.Commands {
    public sealed class CommandModule {
        private static readonly Regex CommandRegex = new Regex(@"[\/-](\w+)(?:[=:\s+]([^-\/]+))?(?:\s+|$)");
        private static CommandModule _instance;
        
        private readonly IList<Command> _commands = new List<Command>();

        private readonly IDictionary<Type, Func<IArgument>> _validators = new Dictionary<Type, Func<IArgument>> {
            [typeof(string)] = () => new StringValidator(),
            [typeof(int)] = () => new Int32Validator()
        };
        
        private CommandModule() {
        }


        /// <summary>
        /// Gets the command manager instance.
        /// </summary>
        public static CommandModule Instance => _instance ?? (_instance = new CommandModule());

        /// <summary>
        /// Gets the commands.
        /// </summary>
        public IEnumerable<Command> Commands => _commands.AsEnumerable();
        
        /// <summary>
        ///     Parses arguments from the specified input string. Supports quotation marks.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parameters.</returns>
        private static IEnumerable<string> ParseArguments(string input)
        {
            var parameters = new List<string>();
            if (input == null)
            {
                return parameters;
            }

            var stringBuilder = new StringBuilder();
            var inQuotes = false;
            foreach (var currentCharacter in input)
            {
                switch (currentCharacter)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    {
                        if (inQuotes)
                        {
                            stringBuilder.Append(currentCharacter);
                        }
                        else
                        {
                            if (stringBuilder.Length == 0)
                            {
                                continue;
                            }

                            parameters.Add(stringBuilder.ToString());
                            stringBuilder.Clear();
                        }

                        break;
                    }

                    case '"':
                    {
                        inQuotes = !inQuotes;
                        if (inQuotes)
                        {
                            continue;
                        }

                        parameters.Add(stringBuilder.ToString());
                        stringBuilder.Clear();
                        break;
                    }

                    default:
                        stringBuilder.Append(currentCharacter);
                        break;
                }
            }

            if (stringBuilder.Length > 0)
            {
                parameters.Add(stringBuilder.ToString());
            }

            return parameters;
        }

        public void Register(object obj) {
            var commands = from method in obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                where method.GetAttribute<CommandAttribute>() != null
                select method;
            
            foreach (var commandInfo in commands) {
                var commandAttribute = commandInfo.GetAttribute<CommandAttribute>();
                Action<string> callback = input => {
                    var parameters = commandInfo.GetParameters();
                    var optionMatches = new Dictionary<string, string>();
                    foreach (Match match in CommandRegex.Matches(input)) {
                        optionMatches[match.Groups[1].Value] = match.Groups[2].Value;
                    }
                    
                    var strippedInput = CommandRegex.Replace(input, string.Empty).Trim();
                    var strippedArgs = ParseArguments(strippedInput).ToArray();
                    var coercedArgs = new object[parameters.Length];
                    for (var i = 0; i < parameters.Length; ++i) {
                        var parameter = parameters[i];
                        if (!_validators.TryGetValue(parameter.ParameterType, out var validator)) {
                            throw new Exception($"Missing argument validator for type '{parameter.ParameterType.Name}'");
                        }
                        
                        if (optionMatches.TryGetValue(parameter.Name, out var arg)) {
                            coercedArgs[i] = validator().Validate(arg);
                        }
                        else {
                            var positionalArgument = strippedArgs.ElementAtOrDefault(i);
                            if (positionalArgument != null) {
                                coercedArgs[i] = positionalArgument;
                            }
                            else {
                                if (!parameter.IsOptional()) {
                                    throw new Exception("Invalid method call.");
                                }
                                
                                coercedArgs[i] = parameter.GetCustomAttribute<DefaultAttribute>()?.Value ?? parameter.RawDefaultValue;
                            }
                        }
                        
                    }

                    commandInfo.Invoke(obj, coercedArgs);
                };

                _commands.Add(new Command(new[] {commandAttribute.Name}, commandAttribute.Description, commandAttribute.HelpText,
                    callback));
            }
        }

        public void Run(string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return;
            }

            var indexOfSpace = input.IndexOf(' ');
            indexOfSpace = indexOfSpace < 0 ? input.Length : indexOfSpace;

            var commandName = input.Substring(0, indexOfSpace);
            var command = _commands.FirstOrDefault(c => c.Aliases.Contains(commandName));
            command?.Callback.Invoke(input.Substring(indexOfSpace));
        }
    }
}
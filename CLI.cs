/*
    Copyright 2020 Tamas Bolner
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace ConController {
    /// <summary>
    /// ConController main class. Use the "Run" method.
    /// </summary>
    public class CLI {
        private class ParamCont {
            public ParameterInfo ParameterInfo { get; }
            public Parameter Param { get; }

            public ParamCont(Parameter param, ParameterInfo parameterInfo) {
                this.Param = param;
                this.ParameterInfo = parameterInfo;
            }
        }

        private class EntryPointCont {
            public MethodInfo MethodInfo { get; }
            public EntryPoint EntryPoint { get; }
            public Dictionary<string, ParamCont> Params { get; } =  new Dictionary<string, ParamCont>();

            public EntryPointCont(EntryPoint entryPoint, MethodInfo methodInfo) {
                this.EntryPoint = entryPoint;
                this.MethodInfo = methodInfo;
            }
        }

        private class ControllerCont {
            public Controller Controller { get; }
            public Dictionary<string, EntryPointCont> Entries { get; } = new Dictionary<string, EntryPointCont>();

            public ControllerCont(Controller controller) {
                this.Controller = controller;
            }
        }

        private class ParsedParameters {
            public bool ShowHelpScreen { get; } = false;
            public string Controller { get; }
            public string EntryPoint { get; }
            public Dictionary<string, string> Params { get; } = new Dictionary<string, string>();
            public List<string> Arguments { get; } = new List<string>();

            public ParsedParameters(string[] args) {
                if (args == null) {
                    ShowHelpScreen = true;
                    return;
                }

                foreach(string arg in args) {
                    this.parseParameterValues(arg);
                }

                if (Arguments.Count < 1) {
                    this.ShowHelpScreen = true;
                    return;
                }

                var parts = Arguments[0].Trim().Split('/');
                if (parts.Length < 2) {
                    throw new CommandParserException($"Invalid argument: {Arguments[0]}");
                }

                this.Controller = parts[0].Trim();
                this.EntryPoint = parts[1].Trim();
            }

            public void parseParameterValues(string line) {
                var match = Regex.Match(line.Trim(), "^[\\-]{0,2}([a-zA-Z0-9_]+)[\\=\\s]+[\"]{0,1}([^\"]+)[\"]{0,1}$");

                if (match.Groups.Count < 3) {
                    this.Arguments.Add(line);
                } else {
                    this.Params[match.Groups[1].Value] = match.Groups[2].Value.Trim();
                }
            }

            public object[] generateInvokeArray(List<ParameterInfo> pdefs) {
                var result = new object[pdefs.Count];

                for(int i = 0; i < pdefs.Count; i++) {
                    var pdef = pdefs[i];

                    if (Params.ContainsKey(pdef.Name)) {
                        try {
                            result[i] = Convert.ChangeType(Params[pdef.Name], pdef.ParameterType);
                        } catch (Exception) {
                            result[i] = null;
                        }
                    } else {
                        if (pdef.HasDefaultValue) {
                            result[i] = pdef.DefaultValue;
                        } else {
                            result[i] = null;
                        }
                    }
                }

                return result;
            }
        }

        private static Dictionary<string, ControllerCont> controllers = new Dictionary<string, ControllerCont>();

        /// <summary>
        /// Parse the controller classes and the input parameters.
        /// Pass execution to a controller.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static async Task<int> Run(string[] args) {
            CLI.ParseMetadata();
            var cmd = new ParsedParameters(args);

            if (cmd.ShowHelpScreen) {
                // TODO: Show help screen
                return 1;
            }

            if (!controllers.ContainsKey(cmd.Controller)) {
                throw new CommandParserException($"Controller '{cmd.Controller}' is unknown.");
            }

            var ctr = controllers[cmd.Controller];

            if (!ctr.Entries.ContainsKey(cmd.EntryPoint)) {
                throw new CommandParserException($"Controller '{cmd.Controller}' has no entry point '{cmd.EntryPoint}'.");
            }

            var entryPoint = ctr.Entries[cmd.EntryPoint];
            
            /*
                Invoke a method
            */
            if (!entryPoint.MethodInfo.IsStatic) {
                throw new CommandParserException($"The method for entry point {cmd.Controller}/{cmd.EntryPoint} "
                    + $"is not static. Method name: '{entryPoint.MethodInfo.Name}'");
            }

            foreach(var p in entryPoint.Params) {
                if (!p.Value.Param.Optional && !cmd.Params.ContainsKey(p.Value.Param.Name)) {
                    throw new CommandParserException($"Missing parameter '{p.Value.Param.Name}'.");
                }
            }

            object[] plist = null;

            if (entryPoint.Params.Count > 0) {
                plist = cmd.generateInvokeArray(entryPoint.Params.Values.Select(x => x.ParameterInfo).ToList());
            }

            try {
                if (entryPoint.MethodInfo.ReturnType.Name.StartsWith("Task")) {
                    await (Task)entryPoint.MethodInfo.Invoke(null, plist);
                } else {
                    entryPoint.MethodInfo.Invoke(null, plist);
                }
            } catch (TargetInvocationException ex) {
                if (ex.InnerException != null) {
                    throw(ex.InnerException);
                } else {
                    throw ex;
                }
            }

            return 0;
        }

        private static void ParseMetadata() {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) {
                throw new CommandParserException("The code was started from an unmanaged application.");
            }

            var classes = assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)));

            foreach (var classType in classes) {
                foreach(var item in classType.GetCustomAttributes()) {
                    if (item is Controller) {
                        var ctl = (Controller)item;

                        if (!controllers.ContainsKey(ctl.Name)) {
                            controllers[ctl.Name] = new ControllerCont(ctl);
                        }

                        foreach(var methodInfo in classType.GetMethods()) {
                            var trackParams = new Dictionary<string, Parameter>();
                            
                            foreach(var item2 in methodInfo.GetCustomAttributes()) {
                                if (item2 is Parameter) {
                                    var p = (Parameter)item2;
                                    if (trackParams.ContainsKey(p.Name)) {
                                        throw new CommandParserException($"In class '{classType.Name}' method '{methodInfo.Name}' "
                                            + $"there are 2 parameter annotations with the same name: '{p.Name}'.");
                                    }
                                    trackParams[p.Name] = p;
                                }
                            }

                            foreach(var item2 in methodInfo.GetCustomAttributes()) {
                                if (item2 is EntryPoint) {
                                    var ePoint = (EntryPoint)item2;
                                    var entryPointCont = new EntryPointCont(ePoint, methodInfo);
                                    controllers[ctl.Name].Entries[ePoint.Name] = entryPointCont;

                                    foreach(var paramInfo in methodInfo.GetParameters()) {
                                        if (!trackParams.ContainsKey(paramInfo.Name)) {
                                            throw new CommandParserException($"In class '{classType.Name}' method '{methodInfo.Name}' "
                                                + $"the parameter '{paramInfo.Name}' has no annotation.");
                                        }

                                        entryPointCont.Params[paramInfo.Name] = new ParamCont(trackParams[paramInfo.Name], paramInfo);
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}

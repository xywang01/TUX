using System;
using System.Collections.Generic;

namespace bmlTUX.Scripts.UI.Runtime {
    public class ParticipantVariableValuesValidator : InputValidator {
        
        public List<string> Errors { get; }
        public bool         Valid  { get; }
        public ParticipantVariableValuesValidator(ParticipantVariablePanel participantVariablePanel) {

            Valid = true;
            Errors = new List<string>();
            foreach (ParticipantVariableEntry participantVariableEntry in participantVariablePanel.Entries) {
                try {
                    participantVariableEntry.ConfirmValue();
                }
                catch (FormatException) {
                    Errors.Add($"Input for Variable {participantVariableEntry.Variable.Name} is incorrect format or type");
                    Valid = false;
                }
                catch (NoValueSelectedException) {
                    Errors.Add($"No value selected for {participantVariableEntry.Variable.Name}");
                    Valid = false;
                }
            }
        }

   
    }
}
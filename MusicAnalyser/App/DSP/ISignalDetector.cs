/*
 * Music Analyser - Script API - ISignalDetector
 * Author: Sean King
 */
using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    /// <summary>
    /// Defines the general behaviour and properties of a DSP detector script.
    /// </summary>
    public interface ISignalDetector
    {
        /// <summary>
        /// Denotes the priority of the script. Primary detector scripts provide fundemental funtionality such as pitch detection.
        /// Non-primary detection scripts can provide additional feature extraction but are not necessary.
        /// </summary>
        bool IsPrimary { get; }

        /// <summary>
        /// Data structure for defining script variables that are exposed to the app.
        /// Each setting has the format: FIELD_NAME, { Value, Type, Display Name, Min/Possible Values, Max }
        /// Supported types are int, double, enum - enum specifies a list of possible values separated with the '|' character, eg. 1|2|3
        /// </summary>
        Dictionary<string, string[]> Settings { get; set; }

        /// <summary>
        /// The input data provided to the script, generic to allow for any data type, the script decides this.
        /// </summary>
        object InputData { get; set; }

        /// <summary>
        /// List of properties passed down through the processing chain, are optional.
        /// </summary>
        Dictionary<string, object> InputArgs { get; set; }

        /// <summary>
        /// Output of script, script should assign this.
        /// </summary>
        object Output { get; set; }

        /// <summary>
        /// Any additional properties that should be passed further down the processing chain, are optional.
        /// </summary>
        Dictionary<string, object> OutputArgs { get; set; }

        /// <summary>
        /// Called when a script setting is changed in the app.
        /// </summary>
        void OnSettingsChange();

        /// <summary>
        /// Method invoked by the app to run the script, defines script functionality.
        /// </summary>
        void Detect();
    }
}

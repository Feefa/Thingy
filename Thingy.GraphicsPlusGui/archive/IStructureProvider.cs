using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public interface IStructureProvider
    {
        /// <summary>
        /// Replace any references to run-time settings values in the structure with
        /// the values of those settings drawn from the passed-in settings dictionary
        /// </summary>
        /// <param name="structure">The structure that the settings will be applied to</param>
        /// <param name="settings">The settings that are to be applied</param>
        /// <returns></returns>
        string ApplySettings(string structure, IDictionary<string, string> settings);
            
        /// <summary>
        /// Create an IJoint instance that represents the structure defined by the passed in
        /// structure string. No settings or sub-structures will be applied
        /// </summary>
        /// <param name="structure">The structure string</param>
        /// <returns>An IJoint instance represting structure defined bythe structure string</returns>
        IJoint Create(string structure);

        /// <summary>
        /// Create an IJoint instance that represents the structure defined by the passed in
        /// structure string. Settings in the passed-in settings dictionary will be applied
        /// to the structure string before the IJoint instance is built.
        /// </summary>
        /// <param name="structure">The structure string</param>
        /// <param name="settings">The settings that are to be applied</param>
        /// <returns>An IJoint instance represting structure defined bythe structure string</returns>
        IJoint Create(string structure, IDictionary<string, string> settings);

        /// <summary>
        /// Create an IJoint instance that represents the structure defined by the passed in
        /// structure string. Settings in the passed-in settings dictionary will be applied
        /// to the structure string before the IJoint instance is built. Structures defined in 
        /// the structures dictionary will be substituted for DynamicJoints in the structure
        /// string. Structures in the structure string are assumed to have had their settings
        /// resolved already. The settings dictionary will NOT be applied to elements of structures
        /// dictionary
        /// </summary>
        /// <param name="structure">The structure string</param>
        /// <param name="settings">The settings that are to be applied</param>
        /// <param name="structures">The structures to be substituted use for dynamic joints</param>
        /// <returns>An IJoint instance represting structure defined bythe structure string</returns>
        IJoint Create(string structure, IDictionary<string, string> settings, IDictionary<string, string> structures);
    }
}

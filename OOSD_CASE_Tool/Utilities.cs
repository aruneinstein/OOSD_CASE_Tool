using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Contains utility methods/variables used elsewhere in the project.
    /// </summary>
    public sealed class Utilities
    {
        /// <summary>
        /// Name of the Concrete Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string C_OBJ_MASTER_NAME = "C-Object";

        /// <summary>
        /// Name of the Abstract Data Type Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string ADT_OBJ_MASTER_NAME = "ADT-Object";

        /// <summary>
        /// Name of the State Machine Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string SM_OBJ_MASTER_NAME = "SM-Object";

        /// <summary>
        /// The name of the stencil that contains Object shapes.
        /// </summary>
        public const string OBJECT_STENCIL_NAME = "Object Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Flow Diagram shapes.
        /// </summary>
        public const string FLOW_STENCIL_NAME = "Flow Diagram Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Relation Editor shapes.
        /// </summary>
        public const string RELATION_STENCIL_NAME = "Relation Editor Stencil.vssx";

        /// <summary>
        /// Returns the path to the Stencils folder.
        /// </summary>
        /// <returns></returns>
        public static string getStencilPath()
        {
            //Get the assembly information, which has runtime info
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            //CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            string clickOnceLocation = System.IO.Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

            return clickOnceLocation += @"\Stencils\";
        }
    }
}

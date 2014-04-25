using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Contains utility constant/variable declarations.
    /// </summary>
    public sealed class CaseTypes
    {
        /// <summary>
        /// Name of the Object Editor Page.
        /// </summary>
        public const string OBJECT_PAGE = "Object Editor";

        /// <summary>
        /// Name of the Entity Relationship Editor Page.
        /// </summary>
        public const string RELATION_PAGE = "Relation Editor";

        /// <summary>
        /// Name of the Flow Diagrams Editor Page.
        /// </summary>
        public const string FLOW_PAGE = "Flow Editor";

        /// <summary>
        /// Page for displaying the generated object hierarchy diagram.
        /// </summary>
        public const string OBJECT_DIAGRAM_PAGE = "Generated Object Hierarchy";

        /// <summary>
        /// Page for displaying the generated data model with 'is-a' and 'has-a' relationship.
        /// </summary>
        public const string DATA_MODEL_DIAGRAM_PAGE = "Generated Data Model";

        /// <summary>
        /// Name of the Architecture Charts Page.
        /// </summary>
        public const string ARCHITECTURE_PAGE = "Architecture Chart";

        /// <summary>
        /// Name of the Concrete Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string C_OBJ_MASTER = "C-Object";

        /// <summary>
        /// Name of the Abstract Data Type Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string ADT_OBJ_MASTER = "ADT-Object";

        /// <summary>
        /// Name of the State Machine Object Master Shape as defined in the Object Stencil.
        /// </summary>
        public const string SM_OBJ_MASTER = "SM-Object";

        /// <summary>
        /// The name of the stencil that contains Object shapes.
        /// </summary>
        public const string OBJECT_STENCIL = "Object Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Flow Diagram shapes.
        /// </summary>
        public const string FLOW_STENCIL = "Flow Diagram Stencil.vssx";

        /// <summary>
        /// The name of the stencil that contains Relation Editor shapes.
        /// </summary>
        public const string RELATION_STENCIL = "Relation Editor Stencil.vssx";

        /// <summary>
        /// Index of the Shape Data Section in a Shapesheet.
        /// </summary>
        public const short SHAPE_DATA_SECTION = (short)Visio.VisSectionIndices.visSectionProp;

        /// <summary>
        /// Index of the Value Cell in the Data Section of a Shapesheet.
        /// </summary>
        public const short DS_VALUE_CELL = (short)Visio.VisCellIndices.visCustPropsValue;

        /// <summary>
        /// Index of the Label Cell in the Data Section of a Shapesheet.
        /// </summary>
        public const short DS_LABEL_CELL = (short)Visio.VisCellIndices.visCustPropsLabel;

        /// <summary>
        /// Returns the path to the Stencils folder including trailing '\'.
        /// </summary>
        /// <returns>Path to the Stencils folder including trailing '\'.</returns>
        public static string stencilPath()
        {
            // Get the assembly information, which has runtime info
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            // CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            string clickOnceLocation = System.IO.Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

            return clickOnceLocation += @"\Stencils\";
        }
    }
}

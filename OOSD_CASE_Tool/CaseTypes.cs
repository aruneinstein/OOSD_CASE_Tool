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

        #region Page Names

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
        public const string OBJECT_DIAGRAM_PAGE = "Object Hierarchy";

        /// <summary>
        /// Page for displaying the generated data model with 'is-a' and 'has-a' relationship.
        /// </summary>
        public const string DATA_MODEL_DIAGRAM_PAGE = "Data Model";

        /// <summary>
        /// Name of the Architecture Charts Page.
        /// </summary>
        public const string ARCHITECTURE_PAGE = "Architecture Chart";

        /// <summary>
        /// Name of the State Transition Table Page.
        /// </summary>
        public const string STATE_TABLE_PAGE = "State Transition Table";

        #endregion


        #region Stencil Master Names

        /// <summary>
        /// Name of the Dynamic Connector Shape as defined in the OOSD General Stencil.
        /// A Dynamic Connector is a line shape that connects and glues two shapes together.
        /// </summary>
        public const string OOSD_CONNECTOR = "Connector";

        /// <summary>
        /// Name of a Rectangular Shape as defined in the OOSD General Stencil.
        /// A Rectangle has connection points in which it can be connected to other shapes.
        /// </summary>
        public const string OOSD_RECTANGLE = "Rectangle";

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
        /// Name of the stencil master that represents an Is-A relationship between objects.
        /// </summary>
        public const string IS_A_STENCIL_MASTER = "Is-A Relation";

        /// <summary>
        /// Name of the connector for object hierarchy.
        /// </summary>
        public const string OBJECT_HIERARCHY_CONNECTOR = "Obj Connector";

        /// <summary>
        /// Name of the rectangle box for object hierarchy.
        /// </summary>
        public const string OBJECT_HIERARCHY_RECT = "Object Box";

        /// <summary>
        /// Name of the Transform-Center Master Shape as defined in the Flow Diagram Stencil.
        /// A transform center shape is the root of a Transform Center and is the root
        /// of all processes active in the Transform Center.
        /// </summary>
        public const string TRANSFORM_CENTER_MASTER = "Transform Center";

        /// <summary>
        /// Name of the Transform-Process Master Shape as defined in the Flow Diagram Stencil.
        /// A Transform Process is a process that operates on the data that comes into the
        /// Transform Center and is children of the Transform Center node.
        /// </summary>
        public const string TRANSFORM_PROCESS_MASTER = "Transform Process";

        /// <summary>
        /// Name of the Transform-Connector Master Shape as defined in the Flow Diagram stencil.
        /// A Transform Connector connects Transform Processes to each other and to the Transform Center,
        /// creating a tree-like structure.
        /// </summary>
        public const string TRANSFORM_CONNECTOR_MASTER = "Transform Connector";

        /// <summary>
        /// Name of the Transform Input Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string TRANSFORM_INPUT_MASTER = "Transform Input";

        /// <summary>
        /// Name of the Transform Output Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string TRANSFORM_OUTPUT_MASTER = "Transform Output";

        /// <summary>
        /// Name of the State Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string STATE_MASTER = "State";

        /// <summary>
        /// Name of the Start State Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string STATE_START_MASTER = "Start State";

        /// <summary>
        /// Name of the End State Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string STATE_END_MASTER = "End State";

        /// <summary>
        /// Name of the State Transition Master Shape as defined in the Flow Diagram Stencil.
        /// </summary>
        public const string STATE_TRANSITION_MASTER = "State Transition";

        #endregion


        #region Stencils

        /// <summary>
        /// The name of the stencil that contains general/miscellaneous shapes.
        /// </summary>
        public const string OOSD_GENERAL_STENCIL = "OOSD General Stencil.vssx";

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
            string path = getClickOnceLocation();
            return path += @"\Stencils\";
        }

        /// <summary>
        /// Returns the path of the code base.
        /// </summary>
        /// <returns> path of code base </returns>
        private static string getClickOnceLocation()
        {
            // Get the assembly information, which has runtime info
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            // CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            return System.IO.Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Class for working with the Flow Editor Subsystem.
    /// </summary>
    internal class FlowSystem
    {
        /// <summary>
        /// Instance of the Application that owns this AddIn.
        /// </summary>
        private Visio.Application app;

        /// <summary>
        /// X coordinate of where to draw a Shape on a Drawing Page.
        /// </summary>
        private double drawXPos = 0.0;

        /// <summary>
        /// Y coordinate of where to draw a Shape on a Drawing Page.
        /// </summary>
        private double drawYPos = 0.0;

        /// <summary>
        /// List of root nodes (Shape) for different Transform Center Diagrams.
        /// </summary>
        private List<Visio.Shape> transformCenters;

        /// <summary>
        /// Creates an instance of the FlowSystem to work with Flow Diagrams.
        /// </summary>
        public FlowSystem()
        {
            app = Globals.ThisAddIn.Application;
            transformCenters = new List<Visio.Shape>();
        }


        #region Convet State Transition Diagram to State Transition Table

        /// <summary>
        /// Converts a State Transition Diagram to a State Transition Table.
        /// </summary>
        /// <param name="selection">Selection of Shape(s) which contains Diagram to convert.</param>
        /// <param name="outputPage">Page to output the Chart.</param>
        public void stateDiagramToTable(Visio.Selection selection, Visio.Page outputPage)
        {
            // Start at any node (that is a State) of the selected shapes to build a State Machine
            // Note: currently can't build a SM starting at the End State node.
            Visio.Shape node = null;
            foreach (Visio.Shape s in selection)
            {
                string masterName = s.Master.Name;
                if (masterName == CaseTypes.STATE_START_MASTER ||
                    masterName == CaseTypes.STATE_MASTER)
                {
                    node = s;
                    break;
                }
            }

            // Builds the State Machine, which lists all the States (& their Transitions)
            List<State> stateMachine = buildStateMachine(node);

            // Creates and output the State Transition Table
            outputStateTransitionTable(stateMachine, outputPage);

            // Switches focus to resulting output
            app.ActiveWindow.Page = outputPage;
        }

        /// <summary>
        /// Creates and outputs a State Transition Table from the given State Machine.
        /// </summary>
        /// <param name="stateMachine">State Machine to convert to Transition Table.</param>
        /// <param name="outputPage">Page to display the Table.</param>
        private void outputStateTransitionTable(List<State> stateMachine, Visio.Page outputPage)
        {
            // drawing the table by drawing rectangles right next to each other
            // and grouping them into a table.

            // sets the drawing position to start at the left of the page
            setShapeDropPosition(outputPage);
            drawXPos = 1.0;
            double leftDrawEdge = drawXPos;

            double rectHeight = 0.5;
            double rectWidth = 1.5;

            // treat start state, end state (& states that are essentially end states:
            // they don't transition to another state) differently
            // Start the table with the start state & end state shouldn't be listed
            // as a starting state in the table.
            // Removes these states from the state machine to work on them separately.
            State startState = null;
            List<State> endStates = new List<State>();
            for (int i = stateMachine.Count - 1; i >= 0; --i)
            {
                State state = stateMachine[i];
                string type = state.Type;
                if (type == State.START_STATE)
                {
                    startState = state;
                    stateMachine.RemoveAt(i);
                } 
                else if (type == State.END_STATE || state.getNextStateCount() == 0)
                {
                    endStates.Add(state);
                    stateMachine.RemoveAt(i);
                }
            }

            // insert start state back into the beginning of the state machine, so
            // it gets processed first.
            if (startState != null)
            {
                stateMachine.Insert(0, startState);
            }

            // Table header
            double newXPos = drawXPos + rectWidth;
            double newYPos = drawYPos - rectHeight;
            Visio.Shape rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
            rect.Text = "State";

            drawXPos = newXPos + rectWidth;
            rect = outputPage.DrawRectangle(newXPos, drawYPos, drawXPos, newYPos);
            rect.Text = "Event";

            newXPos = drawXPos + rectWidth;
            rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
            rect.Text = "Operation";

            drawXPos = newXPos + rectWidth;
            rect = outputPage.DrawRectangle(newXPos, drawYPos, drawXPos, newYPos);
            rect.Text = "Next State";

            // Adjust the height of each start state rect depending on number of next states
            drawXPos = leftDrawEdge;
            drawYPos = newYPos;
            double eventXPos = drawXPos + rectWidth; // X coordinate of start of event column.
            double eventYPos = drawYPos; // Y coordinate of start of event column.
            foreach (State s in stateMachine)
            {
                int numNextState = s.getNextStateCount();
                // draw State column
                newXPos = drawXPos + rectWidth;
                newYPos = drawYPos - (rectHeight * numNextState);
                rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
                rect.Text = s.Name;

                // draw transition columns
                List<State.Transition> transitions = s.getTransitions();
                foreach (State.Transition t in transitions)
                {
                    // event column
                    drawXPos = eventXPos;
                    newXPos = drawXPos + rectWidth;
                    newYPos = drawYPos - rectHeight;
                    rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
                    rect.Text = t.Data;

                    // operation column
                    drawXPos = newXPos;
                    newXPos = drawXPos + rectWidth;
                    rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
                    rect.Text = t.Operation;

                    // next state column
                    drawXPos = newXPos;
                    newXPos = drawXPos + rectWidth;
                    rect = outputPage.DrawRectangle(drawXPos, drawYPos, newXPos, newYPos);
                    rect.Text = t.NextState.Name;

                    // if there are multiple transitions, move one row down for next transition
                    drawYPos = newYPos;
                }

                // reset the drawing position for the next state
                drawXPos = leftDrawEdge;
                drawYPos = newYPos;
            }

        }

        /// <summary>
        /// Builds a State Machine from a State Transition Diagram, starting at the given node.
        /// </summary>
        /// <param name="node">A starting node in the State Transition Diagram.</param>
        /// <returns>The State Machine, in the form of a list of States.</returns>
        private List<State> buildStateMachine(Visio.Shape node)
        {
            Visio.Shapes allShapesOnPage = node.ContainingPage.Shapes;

            List<State> stateMachine = new List<State>();

            // Lists the corresponding Shape for each State in the StateMachine list.
            List<Visio.Shape> stateShapes = new List<Visio.Shape>();

            // For each node, create a State for it (if it doesn't already exist)
            // & adds the State to the stateMachine (if it isn't already in the list).
            // Each node is connected to other nodes via a connector.
            bool notDone = true;
            int currentStateIndex = 0;
            while (notDone)
            {
                // if current state is not part of the state machine, add it
                string currentStateName = node.Text;
                State currentState = stateExists(stateMachine, currentStateName);
                if (currentState == null)
                {
                    currentState = new State(currentStateName, node.Master.Name);
                    stateMachine.Add(currentState);
                    stateShapes.Add(node);
                }

                // for the current state, get all transitions to its next state
                int[] transitions = (int[]) node.GluedShapes(
                    Visio.VisGluedShapesFlags.visGluedShapesOutgoing1D, "");

                // each transition leads to a next state
                foreach (int t in transitions)
                {
                    Visio.Shape connector = allShapesOnPage.get_ItemFromID(t);
                    int[] nextStateID = (int[]) connector.GluedShapes(
                        Visio.VisGluedShapesFlags.visGluedShapesOutgoing2D, "");

                    // there's only one shape connected to one end of a 1-D connector,
                    // but it could be a shape that's already been made into a State
                    Visio.Shape nextStateShape = allShapesOnPage.get_ItemFromID(nextStateID[0]);
                    string nextStateName = nextStateShape.Text;
                    State nextState = stateExists(stateMachine, nextStateName);
                    if (nextState == null)
                    {
                        nextState = new State(nextStateName, nextStateShape.Master.Name);
                        stateMachine.Add(nextState);
                        stateShapes.Add(nextStateShape);
                    }

                    // assumes that a Connector always has two data associated with it
                    // separated by a ','
                    string[] connectorData = connector.Text.Split(',');
                    currentState.addNextState(connectorData[0], connectorData[1], nextState);
                }
                
                // Needs to get states that lead to this state, else depending on the
                // starting state, won't be able to reach all states if we use just
                // next state transitions. Ex: starting at the End State, there are no
                // transitions that leads to another state & so won't be able to find
                // what other states are in the system.

                // for the current state, get all transitions that led to this state
                int[] backTransitions = (int[])node.GluedShapes(
                    Visio.VisGluedShapesFlags.visGluedShapesIncoming1D, "");

                // each backTransition points to a previous state
                foreach (int t in backTransitions)
                {
                    Visio.Shape connector = allShapesOnPage.get_ItemFromID(t);
                    int[] prevStateID = (int[])connector.GluedShapes(
                        Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, "");

                    // there's only one shape connected to one end of a 1-D connector,
                    // but it could be a shape that's already been made into a State
                    Visio.Shape prevStateShape = allShapesOnPage.get_ItemFromID(prevStateID[0]);
                    string prevStateName = prevStateShape.Text;
                    State prevState = stateExists(stateMachine, prevStateName);
                    if (prevState == null)
                    {
                        prevState = new State(prevStateName, prevStateShape.Master.Name);
                        stateMachine.Add(prevState);
                        stateShapes.Add(prevStateShape);
                    }
                }

                ++currentStateIndex;

                // done if there are no more States to check in the StateMachine.
                if (currentStateIndex >= stateMachine.Count)
                {
                    notDone = false;
                } else
                {
                    node = stateShapes[currentStateIndex];
                }
            }

            return stateMachine;
        }

        /// <summary>
        /// Returns the State if the state exists in the given state machine.
        /// </summary>
        /// <param name="stateMachine">State machine.</param>
        /// <param name="name">Name of state to find.</param>
        /// <returns>The State if state exists in state machine, else null</returns>
        private State stateExists(List<State> stateMachine, string name)
        {
            foreach (State s in stateMachine)
            {
                if (s.Name == name)
                {
                    return s;
                }
            }

            return null;
        }

        #endregion


        #region Convert Transform Center to Architecture Chart

        /// <summary>
        /// Converts a Flow Diagram to an Architecture Chart. By Default, retrieves
        /// shapes from the FLOW_PAGE and outputs to ARCHITECTURE_PAGE.
        /// </summary>
        /// <param name="selection">Selection of Shapes which contains Diagram to convert.</param>
        /// <param name="outputPage">Page to output the Chart.</param>
        public void convertToArchitectureChart(Visio.Selection selection, Visio.Page outputPage)
        {
            List<Visio.Shape> allShapes = new List<Visio.Shape>();
            // retrieve all shapes in the selection
            foreach (Visio.Shape s in selection)
            {
                allShapes.Add(s);
            }

            // grabs the root node of each Flow Diagram
            // i.e. a Transform-Center shape for a Transform Center Diagram,
            // a Transaction-Center shape for a Transaction Driven Diagram
            filterRootNodes(allShapes);

            Visio.Page inputPage = selection.ContainingPage;
            foreach (Visio.Shape s in transformCenters)
            {
                transformToArchChart(inputPage, outputPage, s);
            }

            // switches active window to display the architecture chart page
            app.ActiveWindow.Page = outputPage;
        }

        /// <summary>
        /// Converts a Transform-Center Diagram to an Architectural Chart.
        /// </summary>
        private void transformToArchChart(Visio.Page inputPage, Visio.Page outputPage, Visio.Shape root)
        {
            // Separates a Transform-Center into its different components
            // ie. a Process tree, an Input tree, and an Output tree. To get all these
            // components, start at the Root and retrieve all Shapes connected to Root.

            // Gets all shapes that are connected to the root shape through a connector
            // (such as through a 1-D Dynamic Connector)
            List<int> shapeIDs = new List<int>(
                (int[]) root.ConnectedShapes(Visio.VisConnectedShapesFlags.visConnectedShapesAllNodes, ""));

            // Gets all shapes that are glued to the root shape (as in, it is connected
            // directly to the root shape.
            shapeIDs.AddRange(
                (int[]) root.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesAll1D, ""));

            List<Visio.Shape> inputs = new List<Visio.Shape>();
            List<Visio.Shape> process = new List<Visio.Shape>();
            List<Visio.Shape> outputs = new List<Visio.Shape>();

            Visio.Shapes allShapes = inputPage.Shapes;

            foreach (int id in shapeIDs)
            {
                Visio.Shape toShape = allShapes.get_ItemFromID(id);

                if (toShape.Master.Name == CaseTypes.TRANSFORM_PROCESS_MASTER)
                {
                    process.Add(toShape);
                } else if (toShape.Master.Name == CaseTypes.TRANSFORM_INPUT_MASTER)
                {
                    inputs.Add(toShape);
                } else if (toShape.Master.Name == CaseTypes.TRANSFORM_OUTPUT_MASTER)
                {
                    outputs.Add(toShape);
                }
            }

            // Draws the Architecture Chart on the Architecture Chart Page.
            outputChart(outputPage, root, inputs, process, outputs);
        }

        /// <summary>
        /// Draws an Architecture Chart on the given outputPage from the nodes given.
        /// </summary>
        /// <param name="outputPage">Page to draw the chart.</param>
        /// <param name="root">Root node of the chart.</param>
        /// <param name="inputs">Inputs for the Input Node.</param>
        /// <param name="process">Processes for the Process Node.</param>
        /// <param name="outputs">Outputs for the Output Node.</param>
        private void outputChart(Visio.Page outputPage, Visio.Shape root, List<Visio.Shape> inputs,
            List<Visio.Shape> process, List<Visio.Shape> outputs)
        {
            // Space to leave open between shapes
            double yPad = .5, xPad = .1;

            // sets the first open position to start dropping shapes onto the page
            // into the field variables drawXPos, drawYPos.
            setShapeDropPosition(outputPage);

            // Get a Rectangle Master from the OOSD General Stencil to serve
            // as the container for each Node in the chart.
            Visio.Master nodeMaster = Utilities.getMasterFromStencil(app, CaseTypes.OOSD_GENERAL_STENCIL,
                CaseTypes.OOSD_RECTANGLE);

            // Root of the Architecture Chart
            Visio.Shape transformRoot = outputPage.Drop(nodeMaster, drawXPos, drawYPos);
            transformRoot.Text = root.Text;

            // Use the root node width and height as the basis for performing layout
            double nodeHeight = transformRoot.Cells["Height"].Result["inches"];
            double nodeWidth = transformRoot.Cells["Width"].Result["inches"];

            // Root of the Process subtree, set it directly under transform center root.
            drawYPos -= nodeHeight / 2 + yPad;
            Visio.Shape processRoot = outputPage.Drop(nodeMaster, drawXPos, drawYPos);
            processRoot.Text = @"Process";
            
            // Root of the Input subtree, set to left of Process Root
            Visio.Shape inputRoot = outputPage.Drop(nodeMaster, drawXPos - (nodeWidth * 3), drawYPos);
            inputRoot.Text = @"Input";

            // Root of the Output subtree, set to right of Process Root
            Visio.Shape outputRoot = outputPage.Drop(nodeMaster, drawXPos + (nodeWidth * 3), drawYPos);
            outputRoot.Text = @"Output";

            // Calculate starting position of the first Input child node based on
            // the number of all children (input, process, output) and the nodeWidth
            // Set the start y position as the next level down
            drawYPos -= nodeHeight / 2 + yPad;
            int childrenCount = inputs.Count + process.Count + outputs.Count;
            double totalWidth = childrenCount * (nodeWidth + xPad);
            drawXPos = drawXPos - (totalWidth / 2) + (nodeWidth / 2) + xPad;

            double pad = nodeWidth + xPad; // distance from one center of a shape to another center
            List<Visio.Shape> inputChildren = dropShapes(outputPage, nodeMaster, inputs, pad);
            List<Visio.Shape> processChildren = dropShapes(outputPage, nodeMaster, process, pad);
            List<Visio.Shape> outputChildren = dropShapes(outputPage, nodeMaster, outputs, pad);

            // Connects all shapes together to form tree
            glueRootToChildren(outputPage, transformRoot,
                    new List<Visio.Shape>() { inputRoot, processRoot, outputRoot });
            glueRootToChildren(outputPage, inputRoot, inputChildren);
            glueRootToChildren(outputPage, processRoot, processChildren);
            glueRootToChildren(outputPage, outputRoot, outputChildren);
        }

        /// <summary>
        /// Creates an instance of Master for every shape given in the shapes list
        /// and drops it onto the page. Uses & modifies the field variables drawXPos, drawYPos.
        /// </summary>
        /// <param name="page">The page to drop shapes on.</param>
        /// <param name="master">The master to derive instances of shapes.</param>
        /// <param name="shapes">The list of shapes to get properties from.</param>
        /// <param name="pad">Distance from one center of a shape to another center.</param>
        /// <returns>List of shapes dropped.</returns>
        private List<Visio.Shape> dropShapes(Visio.Page page, Visio.Master master, List<Visio.Shape> shapes, double pad)
        {
            List<Visio.Shape> shapesDropped = new List<Visio.Shape>();
            foreach (Visio.Shape s in shapes)
            {
                Visio.Shape dropped = page.Drop(master, drawXPos, drawYPos);
                dropped.Text = s.Text;

                drawXPos += pad;

                shapesDropped.Add(dropped);
            }

            return shapesDropped;
        }

        /// <summary>
        /// Creates a Dynamic Connector and, for each child node, connect it to
        /// the root node.
        /// </summary>
        /// <param name="page">Page on which to output the Shapes.</param>
        /// <param name="root">The root node of the output Architecture Chart.</param>
        /// <param name="children">The children nodes.</param>
        private void glueRootToChildren(Visio.Page page, Visio.Shape root, List<Visio.Shape> children)
        {
            // Since this is an Architecture Chart, all connection points of root
            // is on the bottom edge of its shape and all connection points of children
            // are on its top edge of its shape. Use the middle of its width for both.
            double xGluePoint = .5,
                   rootYGluePoint = 0.0,
                   childYGluePoint = 1.0;

            foreach (Visio.Shape child in children)
            {
                Utilities.glueShapesWithDynamicConnector(page, root, child, CaseTypes.OOSD_CONNECTOR,
                    xGluePoint, rootYGluePoint, xGluePoint, childYGluePoint);
            }
        }

        /// <summary>
        /// Retrieves only the root Shape for each Flow Diagram system from a 
        /// list of shapes.
        /// </summary>
        /// <param name="shapes">List of shapes to search and filter.</param>
        private void filterRootNodes(List<Visio.Shape> shapes)
        {
            foreach (Visio.Shape s in shapes)
            {
                if (s.Master.Name == CaseTypes.TRANSFORM_CENTER_MASTER)
                {
                    transformCenters.Add(s);
                }
            }
        }

        #endregion


        #region Flow System Utility Functions

        /// <summary>
        /// Sets the position to Drop a Shape into the drawX, drawY field variables.
        /// This is a position at the top center of the Page if there are no shapes in it.
        /// Else, it's the bottom center of the BoundingBox that surrounds all the 
        /// current shapes on the page.
        /// </summary>
        /// <param name="page"></param>
        private void setShapeDropPosition(Visio.Page page)
        {
            int shapeCount = page.Shapes.Count;
            if (shapeCount == 0)
            {
                double pageWidth = Utilities.getPageWidth(page);
                double pageHeight = Utilities.getPageHeight(page);
                drawXPos = pageWidth / 2;
                drawYPos = pageHeight;
            }
            else
            {
                BoundingBox box = Utilities.getBoundingBox(page);
                drawXPos = ((box.LowerRightX - box.UpperLeftX) / 2) + box.UpperLeftX;
                drawYPos = box.LowerRightY;
            }

            // Adjustment co-efficients to try to center the Point in the center top,
            // with a margin from the top.
            drawYPos -= 1;
        }

        #endregion
    }
}

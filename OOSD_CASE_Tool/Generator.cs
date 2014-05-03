using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Packaging;
using Visio = Microsoft.Office.Interop.Visio;
using System.Xml.Linq;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Class that handles generating from Objects/ Relationships to XML.
    /// </summary>
    internal class Generator
    {

        List<Visio.Shape> cObjects;
        List<Visio.Shape> adtObjects;
        List<Visio.Shape> smObjects;
        public string DirPath { get; private set; }

        /// <summary>
        /// Creates a Generator that can generate an XML file.
        /// </summary>
        public Generator(string dirPath)
        {
            cObjects = new List<Visio.Shape>();
            adtObjects = new List<Visio.Shape>();
            smObjects = new List<Visio.Shape>();
            DirPath = dirPath;
        }


        #region ER Diagram to Relationships Database

        /// <summary>
        /// converts an ER Diagram to a Relationship Database.
        /// </summary>
        /// <param name="page"></param>
        public void erToRelationshipsDB(Visio.Page page)
        {
            // XML Document to hold all Objects Data Dictionary.
            XElement doc = new XElement(
                new XElement("ER-DB"));
            XElement isARoot = new XElement(new XElement("Is-A-Relationships"));
            XElement multiRoot = new XElement(new XElement("Multiplicity-Relationships"));

            // output in form of: obj1_multiplicity-multiplicity_obj2
            // or obj1_is-a_obj2
            List<string> isA = new List<string>();
            List<string> multi = new List<string>();

            Visio.Shapes allShapes = page.Shapes;
            List<Visio.Shape> all1DShapes = getAll1DConnectors(page);

            // for each connector, get the type of connect (1:1, 1:M, ..., is-a)
            // and the begin & end shapes
            foreach (Visio.Shape connector in all1DShapes)
            {
                int beginShapeID = ((int[])connector.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesIncoming2D, ""))[0];
                int endShapeID = ((int[])connector.GluedShapes(Visio.VisGluedShapesFlags.visGluedShapesOutgoing2D, ""))[0];

                string relationship = allShapes.get_ItemFromID(beginShapeID).Text + " : ";

                string connectorType = connector.Master.Name;
                if (connectorType == CaseTypes.IS_A_MASTER)
                {
                    relationship += "is a" +  " : " + allShapes.get_ItemFromID(endShapeID).Text;
                    isA.Add(relationship);
                } 
                else
                {
                    string[] multiplicities = connector.Master.Name.Split(':');
                    relationship += multiplicities[0] + " :: " + multiplicities[1]
                        + " : " + allShapes.get_ItemFromID(endShapeID).Text;
                    multi.Add(relationship);
                }

            }

            // convert to XML element
            for (int i = 0; i < isA.Count; ++i)
            {
                isARoot.Add(new XElement("r" + i, isA[i]));
            }

            for (int i = 0; i < multi.Count; ++i)
            {
                multiRoot.Add(new XElement("r" + i, multi[i]));
            }

            doc.Add(isARoot);
            doc.Add(multiRoot);

            // Save to file
            doc.Save(this.DirPath);
        }

        /// <summary>
        /// Returns only 1D Connector Shapes.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<Visio.Shape> getAll1DConnectors(Visio.Page page)
        {
            List<Visio.Shape> all1DShapes = new List<Visio.Shape>();

            List<Visio.Shape> allShapes = Utilities.getAllShapesOnPage(page);
            foreach (Visio.Shape s in allShapes)
            {
                // OneD returns -1 if it's a 1D Shape
                if (s.OneD < 0)
                {
                    all1DShapes.Add(s);
                }
            }

            return all1DShapes;
        }

        #endregion


        #region Objects to Data Dictionary

        /// <summary>
        /// Outputs all Objects & its attributes from the page into an XML file.
        /// </summary>
        /// <param name="page"></param>
        public void objToDataDictionary(Visio.Page page)
        {

            // XML Document to hold all Objects Data Dictionary.
            XElement doc = new XElement(
                new XElement("Objects-DB"));

            // separates different types of objects so they can be written in order
            // and together in their own section.
            Visio.Shapes allShapes = page.Shapes;
            filterObjectTypes(allShapes);

            writeCObjToXML(doc);
            writeADTObjToXML(doc);
            writeSMObjToXML(doc);

            doc.Save(this.DirPath);

        }

        private void writeSMObjToXML(XElement doc)
        {
            foreach (Visio.Shape s in smObjects)
            {
                short numRows = s.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
                XElement root = new XElement("SM-Object");
                XElement operationRoot = null;
                XElement objectsRoot = null;
                string objectsList = "";

                for (short row = 0; row < numRows; ++row)
                {
                    string valCell = valCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_VALUE_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    string labelCell = labelCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_LABEL_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    // the first row gives the name of the Obj
                    if (row == 0)
                    {

                        root.SetAttributeValue("name", valCell);
                    }
                    else
                    {
                        string[] labelArr = labelCell.Split('_');

                        if (labelArr[0] == "obj")
                        {
                            objectsList += valCell;
                        }
                        else if (labelArr[2] == "")
                        {
                            operationRoot = new XElement("Operation");
                            operationRoot.SetAttributeValue("name", valCell);
                            root.Add(operationRoot);
                        } else if (labelArr[0] == "op")
                        {
                            operationRoot.Add(
                                new XElement(labelArr[2], valCell));
                        }
                    }
                }
                objectsRoot = new XElement("Objects", objectsList);

                root.Add(objectsRoot);
                root.Add(operationRoot);
                doc.Add(root);

            }
        }


        private void writeADTObjToXML(XElement doc)
        {
            foreach (Visio.Shape s in adtObjects)
            {
                short numRows = s.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
                XElement objRoot = new XElement("ADT-Object");
                XElement opRoot = null;
                XElement axiomRoot = null;

                for (short row = 0; row < numRows; ++row)
                {
                    string valCell = valCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_VALUE_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    string labelCell = labelCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_LABEL_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    // the first row gives the name of the Obj
                    if (row == 0)
                    {

                        objRoot.SetAttributeValue("name", valCell);
                    }
                    else
                    {
                        string[] labelArr = labelCell.Split('_');

                        if (labelArr.Length > 2 && labelArr[2] == "name")
                        {
                            opRoot = new XElement("Operation");
                            opRoot.SetAttributeValue("name", valCell);
                            objRoot.Add(opRoot);
                        }
                        else if (labelArr[0] == "op")
                        {
                            opRoot.Add(
                                new XElement(labelArr[2], valCell));
                        } else if (labelArr[0] == "axiom")
                        {
                            axiomRoot = new XElement("Axioms", valCell);
                        }
                    }


                }
                objRoot.Add(axiomRoot);
                doc.Add(objRoot);

            }
        }


        private void writeCObjToXML(XElement doc)
        {
            foreach (Visio.Shape s in cObjects)
            {
                short numRows = s.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
                XElement objRoot = new XElement("C-Object");
                XElement attrRoot = null;

                for (short row = 0; row < numRows; ++row)
                {
                    string valCell = valCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_VALUE_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    string labelCell = labelCell = s.get_CellsSRC(CaseTypes.SHAPE_DATA_SECTION, row, CaseTypes.DS_LABEL_CELL)
                            .get_ResultStr(Visio.VisUnitCodes.visUnitsString);
                    // the first row gives the name of the C-Obj
                    if (row == 0)
                    {

                        objRoot.SetAttributeValue("name", valCell);
                    } else
                    {
                        string[] labelArr = labelCell.Split('_');

                        if (labelArr[2] == "name")
                        {
                            attrRoot = new XElement("Attribute");
                            attrRoot.SetAttributeValue("name", valCell);
                            objRoot.Add(attrRoot);
                        }
                        else
                        {
                            attrRoot.Add(
                                new XElement(labelArr[2], valCell));
                        }
                    }

                    
                }

                doc.Add(objRoot);

            }
        }

        private void filterObjectTypes(Visio.Shapes allShapes)
        {
            foreach (Visio.Shape s in allShapes)
            {
                switch (s.Master.Name)
                {
                    case CaseTypes.C_OBJ_MASTER:
                        cObjects.Add(s);
                        break;
                    case CaseTypes.ADT_OBJ_MASTER:
                        adtObjects.Add(s);
                        break;
                    case CaseTypes.SM_OBJ_MASTER:
                        smObjects.Add(s);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

    }
}

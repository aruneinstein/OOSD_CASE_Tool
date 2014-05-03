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

        /// <summary>
        /// Creates a Generator that can generate an XML file.
        /// </summary>
        public Generator()
        {
            cObjects = new List<Visio.Shape>();
            adtObjects = new List<Visio.Shape>();
            smObjects = new List<Visio.Shape>();
        }

        /// <summary>
        /// Outputs all Objects & its attributes from the page into an XML file.
        /// </summary>
        /// <param name="page"></param>
        public void objToDataDictionary(Visio.Page page)
        {
            string dirPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Desktop) + @"\ObjectsDB.xml";

            // XML Document to hold all Objects Data Dictionary.
            XElement doc = new XElement(
                new XElement("Objects-DB"));

            // separates different types of objects so they can be written in order
            // and together in their own section.
            Visio.Shapes allShapes = page.Shapes;
            filterObjectTypes(allShapes);

            writeCObjToXML(doc);

            doc.Save(dirPath);
        }

        private void writeCObjToXML(XElement doc)
        {
            int objNum = 0;
            foreach (Visio.Shape s in cObjects)
            {
                short numRows = s.get_RowCount(CaseTypes.SHAPE_DATA_SECTION);
                XElement objRoot = new XElement("C-Object");
                XElement attrRoot = null;
                int attrNum = 0;
                string attrName = "";
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
                objNum++;

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
    }
}

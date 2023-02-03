using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsBetweenPoints
{
    public class SelectionUtils
    {
        public static XYZ GetPoint(ExternalCommandData commandData,
            string promptMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            XYZ points = null;
            XYZ pickedPoint = null;
            try
            {
                pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promptMessage);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
            {

            }

            points = pickedPoint;
            return points;
        }   
    }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining24
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        private object level;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //UIApplication uiapp = commandData.Application;
            //UIDocument uidoc = uiapp.ActiveUIDocument;
            //Document doc = uidoc.Document;
            var doc = commandData.Application.ActiveUIDocument.Document;
            var levels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();
            foreach (Level level in levels)
            {
                var ducts = new FilteredElementCollector(doc)
                  .OfClass(typeof(Duct))
                  .OfType<Duct>()
                  .Where(e => e.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString() == level.Name)
                  //.Cast<ViewPlan>()
                  .Count();

                TaskDialog.Show("Результат", $"Этаж - {level.Name} Количество воздуховодов - {ducts}");
            }
            return Result.Succeeded;
        }
    }

}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITR4._2
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string pipeInfo = string.Empty;

            var wall = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Pipes)
                .Cast<Pipe>()
                .ToList();

            foreach (Pipe pipe in pipes)
            {
                string pipeName = pipe.get_Parameter(BuiltInParameter.PIPE_NAME).AsString();
                pipeInfo += $"{pipeName}\t{pipe.Leigth}\t{pipe.Outside_diameter}\t{pipe.Inner_diameter}{Environment.NewLine}";
            }

            var saveDialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "All files (*.*)|*.*",
                FileName = "pipeInfo.csv",
                DefaultExt = ".csv"
            };

            string selectedFilePath = string.Empty;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = saveDialog.FileName;
            }

            if (string.IsNullOrEmpty(selectedFilePath))
                return Result.Cancelled;

            FileStyleUriParser.WriteAllText(selectedFilePath.pipeInfo);

            return IAsyncResult.Succeeded;
        }
    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace ElementsBetweenPoints
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> FamilyTypes { get; } = new List<FamilySymbol>(); //тип мебели
        public object Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; set; }
        public int AmountOfElements { get; set; }
        public FamilySymbol SelectedFamilyType { get; set; }
        public Level SelectedLevel { get; set; }
        public XYZ Point1 { get; }
        public XYZ Point2 { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            FamilyTypes = FamilySymbolUtils.GetFamilySymbols(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            AmountOfElements = 2;  // количество элементов

            Point1 = SelectionUtils.GetPoint(_commandData, "Выберите первую точку", ObjectSnapTypes.Endpoints);
            Point2 = SelectionUtils.GetPoint(_commandData, "Выберите вторую точку", ObjectSnapTypes.Endpoints);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            XYZ stepPoint = (Point2 - Point1) / (AmountOfElements - 1);     //шаг между элементами

            if (Point1 == null || Point2 == null || FamilyTypes == null || SelectedLevel == null || AmountOfElements < 2)
                return;

            for (int i = 0; i < AmountOfElements; i++)        //цикл с расстановкой элементов
            {
                XYZ point = Point1 + stepPoint * i;
                FamilySymbolUtils.CreateFamilyInstance(_commandData, SelectedFamilyType, point, SelectedLevel);  
            }

            RaiseCloseRequest();

        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}

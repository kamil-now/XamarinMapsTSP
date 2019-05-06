using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Abstractions;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private IEnumerable<ICrossoverAlgorithm> _crossoverAlgorithms;
        private IEnumerable<ISelectionAlgorithm> _selectionAlgorithms;

        public IBasicGeneticAlgorithmConfiguration Configuration { get; set; }

        public IList<string> CrossoverAlgorithms => _crossoverAlgorithms.Select(x => x.Name).ToList();
        public IList<string> SelectionAlgorithms => _selectionAlgorithms.Select(x => x.Name).ToList();
        public string SelectedCrossoverAlgorithm
        {
            get => Configuration.CrossoverAlgorithm.Name;
            set => Configuration.CrossoverAlgorithm = _crossoverAlgorithms.FirstOrDefault(x => x.Name == value);

        }
        public string SelectedSelectionAlgorithm
        {
            get => Configuration.SelectionAlgorithm.Name;
            set => Configuration.SelectionAlgorithm = _selectionAlgorithms.FirstOrDefault(x => x.Name == value);
        }


        public ConfigurationViewModel(IBasicGeneticAlgorithmConfiguration configuration
                                    , IEnumerable<ICrossoverAlgorithm> crossoverAlgorithms
                                    , IEnumerable<ISelectionAlgorithm> selectionAlgorithms)
        {
            Configuration = configuration;

            _crossoverAlgorithms = crossoverAlgorithms;
            _selectionAlgorithms = selectionAlgorithms;

        }
    }
}

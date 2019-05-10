using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Common.Extensions;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.GoogleMapsApi.Enums;
using XamarinTSP.TSP.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Utilities.Enums;

namespace XamarinTSP.UI.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private INavigator _navigator;
        private IBasicGeneticAlgorithmConfiguration _activeTSPConfiguration;
        private IDistanceMatrixRequestConfiguration _activeRouteConfiguration;

        private IEnumerable<ICrossoverAlgorithm> _crossoverAlgorithms;
        private IEnumerable<ISelectionAlgorithm> _selectionAlgorithms;
        private IEnumerable<IMutationAlgorithm> _mutationAlgorithms;
        private string _selectedCrossoverAlgorithm;
        private string _selectedSelectionAlgorithm;
        private string _selectedMutationAlgorithm;
        private ConfigurationType _configurationType;

        public ConfigurationType ConfigurationType
        {
            get => _configurationType;
            set
            {
                _configurationType = value;
                NotifyOfPropertyChange();
            }
        }
        public IBasicGeneticAlgorithmConfiguration TSPConfiguration { get; set; }

        public IDistanceMatrixRequestConfiguration RouteConfiguration { get; set; }
        public bool AvoidTolls
        {
            get => RouteConfiguration.Restriction.HasFlag(Restriction.AvoidTolls);
            set
            {
                if (value)
                {
                    RouteConfiguration.Restriction |= Restriction.AvoidTolls;
                }
                else
                {
                    RouteConfiguration.Restriction &= ~Restriction.AvoidTolls;
                }
            }
        }
        public bool AvoidFerries
        {
            get => RouteConfiguration.Restriction.HasFlag(Restriction.AvoidFerries);
            set
            {
                if (value)
                {
                    RouteConfiguration.Restriction |= Restriction.AvoidFerries;
                }
                else
                {
                    RouteConfiguration.Restriction &= ~Restriction.AvoidFerries;
                }
            }
        }
        public bool AvoidHighways
        {
            get => RouteConfiguration.Restriction.HasFlag(Restriction.AvoidHighways);
            set
            {
                if (value)
                {
                    RouteConfiguration.Restriction |= Restriction.AvoidHighways;
                }
                else
                {
                    RouteConfiguration.Restriction &= ~Restriction.AvoidHighways;
                }
            }
        }
        public IList<string> CrossoverAlgorithms => _crossoverAlgorithms.Select(x => x.Name).ToList();
        public IList<string> SelectionAlgorithms => _selectionAlgorithms.Select(x => x.Name).ToList();
        public IList<string> MutationAlgorithms => _mutationAlgorithms.Select(x => x.Name).ToList();
        public IList<string> TrafficModels { get; }

        private string _selectedTrafficModel;

        public string SelectedTrafficModel
        {
            get => _selectedTrafficModel;
            set
            {
                _selectedTrafficModel = value;
                TrafficModel val = TrafficModel.Undefined;
                Enum.TryParse(value, out TrafficModel myStatus);
                if (val != TrafficModel.Undefined)
                {
                    RouteConfiguration.TrafficModel = val;
                }
            }
        }

        public string SelectedCrossoverAlgorithm
        {
            get => _selectedCrossoverAlgorithm;
            set
            {
                _selectedCrossoverAlgorithm = value;
                var val = _crossoverAlgorithms.FirstOrDefault(x => x.Name == value);
                if (val != null)
                {
                    TSPConfiguration.CrossoverAlgorithm = val;
                }
            }
        }
        public string SelectedSelectionAlgorithm
        {
            get => _selectedSelectionAlgorithm;
            set
            {
                _selectedSelectionAlgorithm = value;
                var val = _selectionAlgorithms.FirstOrDefault(x => x.Name == value);
                if (val != null)
                {
                    TSPConfiguration.SelectionAlgorithm = val;
                }
            }
        }

        public string SelectedMutationAlgorithm
        {
            get => _selectedMutationAlgorithm;
            set
            {
                _selectedMutationAlgorithm = value;
                var val = _mutationAlgorithms.FirstOrDefault(x => x.Name == value);
                if (val != null)
                {
                    TSPConfiguration.MutationAlgorithm = val;
                }
            }
        }
        public ConfigurationViewModel(INavigator navigator
                                    , IBasicGeneticAlgorithmConfiguration activeTSPConfiguration
                                    , IDistanceMatrixRequestConfiguration activeRouteConfiguration
                                    , IEnumerable<ICrossoverAlgorithm> crossoverAlgorithms
                                    , IEnumerable<ISelectionAlgorithm> selectionAlgorithms
                                    , IEnumerable<IMutationAlgorithm> mutationAlgorithms)
        {
            _activeTSPConfiguration = activeTSPConfiguration;
            _activeRouteConfiguration = activeRouteConfiguration;

            TSPConfiguration = _activeTSPConfiguration.Copy();
            RouteConfiguration = _activeRouteConfiguration.Copy();
            _navigator = navigator;
            _crossoverAlgorithms = crossoverAlgorithms;
            _selectionAlgorithms = selectionAlgorithms;
            _mutationAlgorithms = mutationAlgorithms;
            
            TrafficModels = new List<string>(EnumExtensions.GetValues<TrafficModel>().Select(x => x.ToString()));

            _selectedMutationAlgorithm = activeTSPConfiguration.MutationAlgorithm.Name;
            _selectedSelectionAlgorithm = activeTSPConfiguration.SelectionAlgorithm.Name;
            _selectedCrossoverAlgorithm = activeTSPConfiguration.CrossoverAlgorithm.Name;
        }

        public ICommand OnAppearingCommand => new Command(() =>
        {
            ConfigurationType = ConfigurationType.TSP;
            TSPConfiguration = _activeTSPConfiguration.Copy();
            RouteConfiguration = _activeRouteConfiguration.Copy();
            NotifyOfPropertyChange();
        });
        public ICommand SetTSPConfigurationCommand => new Command(() => ConfigurationType = ConfigurationType.TSP);
        public ICommand SetRouteConfigurationCommand => new Command(() => ConfigurationType = ConfigurationType.Route);
        public ICommand ReturnCommand => new Command(async () => await _navigator.PopToRootAsync());
        public ICommand SaveCommand => new Command(async () =>
        {
            SaveTSPConfiguration();
            SaveRouteConfiguration();
            await Application.Current.MainPage.DisplayAlert("", "CONFIGURATION SAVED", "OK");
            ReturnCommand.Execute(null);
        });

        private void SaveTSPConfiguration()
        {
            _activeTSPConfiguration.CrossoverAlgorithm = TSPConfiguration.CrossoverAlgorithm;
            _activeTSPConfiguration.SelectionAlgorithm = TSPConfiguration.SelectionAlgorithm;
            _activeTSPConfiguration.MutationAlgorithm = TSPConfiguration.MutationAlgorithm;
            _activeTSPConfiguration.PopulationSize = TSPConfiguration.PopulationSize;
            _activeTSPConfiguration.TournamentSize = TSPConfiguration.TournamentSize;
            _activeTSPConfiguration.CrossoverChance = TSPConfiguration.CrossoverChance;
            _activeTSPConfiguration.MutationChance = TSPConfiguration.MutationChance;
            _activeTSPConfiguration.ElitismFactor = TSPConfiguration.ElitismFactor;
            _activeTSPConfiguration.Elitism = TSPConfiguration.Elitism;
            _activeTSPConfiguration.MutationBasedOnDiversity = TSPConfiguration.MutationBasedOnDiversity;
            _activeTSPConfiguration.TimeBasedFitness = TSPConfiguration.TimeBasedFitness;
            _activeTSPConfiguration.DistanceBasedFitness = TSPConfiguration.DistanceBasedFitness;
        }
        private void SaveRouteConfiguration()
        {
            _activeRouteConfiguration.TrafficModel = RouteConfiguration.TrafficModel;
            _activeRouteConfiguration.Restriction = RouteConfiguration.Restriction;
            _activeRouteConfiguration.ArrivalTime = RouteConfiguration.ArrivalTime;
            _activeRouteConfiguration.DepartureTime = RouteConfiguration.DepartureTime;
        }
    }
}

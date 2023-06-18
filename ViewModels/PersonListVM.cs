using ACS.Model;
using ACS.ViewModels.Base;
using System.Linq;

namespace ACS.ViewModels
{
    class PersonListVM : ViewModel
    {
        private Division? _SelectedDivision;

        public Division? SelectedDivision
        {
            get => _SelectedDivision;
            set
            {
                if (Set(ref _SelectedDivision, value))
                {
                    SelectedPerson = SelectedDivision?.Persons?.Select(p=>p).First();
                }
            }
        }

        private Person? _SelectedPerson;

        public Person? SelectedPerson
        {
            get => _SelectedPerson;
            set
            {
                Set(ref _SelectedPerson, value);
                if (SelectedPerson != null)
                    IsVisible = true;
            }
        }

        private bool _IsVisible;

        public bool IsVisible
        {
            get => _IsVisible;
            set => Set(ref _IsVisible, value);
        }

    }
}

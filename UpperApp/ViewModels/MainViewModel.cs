using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UpperApp.Commands;

namespace UpperApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _inputText ;
        private string _outputText;

        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                }
            }
        }

        public string OutputText
        {
            get => _outputText;
            set
            {
                if (_outputText != value)
                {
                    _outputText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConvertCommand { get; }

        public MainViewModel()
        {
            ConvertCommand = new RelayCommand(Convert, CanConvert);
        }

        private bool CanConvert(object? parameter)
        {
            return !string.IsNullOrEmpty(InputText) && InputText.Length >= 1 && InputText.Length <= 8;
        }

        private void Convert(object? parameter)
        {
            OutputText = InputText.ToUpper();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lock_Audit_Report_1._0.Models;
using Lock_Audit_Report_1._0.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Lock_Audit_Report_1._0.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly AppConfig _config;

        public ObservableCollection<OnlineEvent> Events { get; } = new();

        private string _roomNumber;
        public string RoomNumber
        {
            get => _roomNumber;
            set => SetProperty(ref _roomNumber, value);
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public IRelayCommand LoadCommand { get; }
        public IRelayCommand ExportPdfCommand { get; }

        public MainViewModel(AppConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            // Async command for loading events
            LoadCommand = new AsyncRelayCommand(LoadEventsAsync);

            // PDF export command
            ExportPdfCommand = new RelayCommand(ExportPdf);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(RoomNumber))
            {
                ErrorMessage = "Room number cannot be empty.";
                return false;
            }

            if (!Regex.IsMatch(RoomNumber, @"^[a-zA-Z0-9]+$"))
            {
                ErrorMessage = "Room number can only contain letters and numbers.";
                return false;
            }

            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                ErrorMessage = "Start and End dates must be selected.";
                return false;
            }

            if (EndDate < StartDate)
            {
                ErrorMessage = "End date cannot be earlier than start date.";
                return false;
            }

            if (EndDate > DateTime.Today)
            {
                ErrorMessage = "End date cannot be in the future.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private async Task LoadEventsAsync()
        {
            // 1️⃣ Room number validation
            if (string.IsNullOrWhiteSpace(RoomNumber))
            {
                ErrorMessage = "Room number cannot be empty.";
                return;
            }

            if (!Regex.IsMatch(RoomNumber, @"^[a-zA-Z0-9]+$"))
            {
                ErrorMessage = "Room number can only contain letters and numbers.";
                return;
            }

            // 2️⃣ Date validation
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                ErrorMessage = "Start and End dates must be selected.";
                return;
            }

            if (EndDate > DateTime.Today)
            {
                ErrorMessage = "End date cannot be in the future.";
                return;
            }

            if (EndDate < StartDate)
            {
                ErrorMessage = "End date cannot be earlier than start date.";
                return;
            }

            Events.Clear();

            try
            {
                // 3️⃣ Instantiate API client
                var apiClient = new ApiClient(_config);

                // 4️⃣ Fetch events
                var fetchedEvents = await apiClient.GetRoomEventsAsync(RoomNumber, StartDate.Value, EndDate.Value);

                if (fetchedEvents == null || fetchedEvents.Count == 0)
                {
                    ErrorMessage = "No events found for this room and date range.";
                    return;
                }

                foreach (var e in fetchedEvents)
                    Events.Add(e);

                // Clear previous errors
                ErrorMessage = string.Empty;
            }
            catch (HttpRequestException httpEx)
            {
                // Network or wrong IP/port
                ErrorMessage = $"Cannot connect to server: {httpEx.Message}";
            }
            catch (UnauthorizedAccessException authEx)
            {
                // Wrong credentials
                ErrorMessage = $"Authentication failed: {authEx.Message}";
            }
            catch (Exception ex)
            {
                // Other unexpected errors
                ErrorMessage = $"Unexpected error: {ex.Message}";
            }
        }

        private void ExportPdf()
        {
            if (Events.Count == 0)
            {
                ErrorMessage = "No events to export.";
                return;
            }

            try
            {
                PdfService.ExportEventsToPdf(Events);
                MessageBox.Show("PDF exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"PDF export failed: {ex.Message}";
            }
        }
    }
}
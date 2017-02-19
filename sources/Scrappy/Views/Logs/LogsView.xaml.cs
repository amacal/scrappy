using System;
using System.Collections.ObjectModel;
using Scrappy.Core;
using System.Collections.Generic;

namespace Scrappy.Views.Logs
{
    public partial class LogsView
    {
        private ObservableCollection<dynamic> items;

        public LogsView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Logger.Subscribe(OnGetAll, OnNewEntry);
        }

        private void OnGetAll(IEnumerable<dynamic> entries)
        {
            items = new ObservableCollection<dynamic>(entries);
            DataContext = items;
        }

        private void OnNewEntry(dynamic entry)
        {
            Action addNewEntry = () => items.Add(entry);
            Dispatcher.BeginInvoke(addNewEntry);
        }
    }
}
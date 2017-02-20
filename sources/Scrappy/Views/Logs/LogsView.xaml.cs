using System;
using System.Collections.ObjectModel;
using Scrappy.Core;
using System.Collections.Generic;
using Scrappy.Noom;

namespace Scrappy.Views.Logs
{
    public partial class LogsView : LoggerCallback, ICachable
    {
        private ObservableCollection<dynamic> items;

        public LogsView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Logger.Subscribe(this);
        }

        void LoggerCallback.OnAll(IEnumerable<dynamic> entries)
        {
            items = new ObservableCollection<dynamic>(entries);
            DataContext = items;
        }

        void LoggerCallback.OnNew(dynamic entry)
        {
            Dispatcher.BeginInvoke(new Action(() => items.Add(entry)));
        }

        void LoggerCallback.OnDisposed(dynamic entry)
        {
            Dispatcher.BeginInvoke(new Action(() => items.Remove(entry)));
        }
    }
}
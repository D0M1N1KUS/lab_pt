﻿using System;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;

namespace Lab1
{
    public class ContextMenuBuilder
    {
        private ContextMenu cm = new ContextMenu();

        public ContextMenuBuilder AddMenuItem(string header, RoutedEventHandler handler = null)
        {
            var mi = new MenuItem {Header = header};
            if (handler != null)
                mi.Click += handler;

            cm.Items.Add(mi);

            return this;
        }

        public ContextMenu Build()
        {
            return cm;
        }
    }
}
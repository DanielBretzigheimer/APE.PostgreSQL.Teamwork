// <copyright file="dialogwindow.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using APE.PostgreSQL.Teamwork.Model;
using APE.PostgreSQL.Teamwork.ViewModel;
using MaterialDesignThemes.Wpf;

namespace APE.PostgreSQL.Teamwork.GUI
{
    /// <summary>
    /// A <see cref="Window"/> which can display dialogs.
    /// </summary>
    public abstract class DialogWindow : Window
    {
        public DialogWindow()
        {
            this.UIDispatcher = Dispatcher.CurrentDispatcher;
            this.GotFocus += this.DialogWindowGotFocus;

            // set default dialog wrapper to this
            BaseViewModel.ShowDialog = this.ShowDialog;
            BaseViewModel.ShowDialogWithCloseHandler = this.ShowDialog;
            BaseViewModel.ShowExtendedDialog = this.ShowDialog;
            BaseViewModel.CloseView = () => { this.Close(); };
        }

        public abstract object DialogIdentifier { get; }

        /// <summary>
        /// Is used to open the other windows, if they are opened from another thread.
        /// </summary>
        protected Dispatcher UIDispatcher { get; set; }

        protected async Task<MaterialMessageBoxResult> ShowDialog(object view)
        {
            return await this.ShowDialog(view, null);
        }

        private void DialogWindowGotFocus(object sender, RoutedEventArgs e)
        {
            BaseViewModel.ShowDialog = this.ShowDialog;
            BaseViewModel.ShowDialogWithCloseHandler = this.ShowDialog;
        }

        private async Task<MaterialMessageBoxResult> ShowDialog(object view, Action<MaterialMessageBoxResult> closingEventHandler)
        {
            return await this.ShowDialog(
                view,
                new Func<MaterialMessageBoxResult, object>(
                (result) =>
                {
                    closingEventHandler?.Invoke(result);
                    return null;
                }));
        }

        private async Task<MaterialMessageBoxResult> ShowDialog(object view, Func<MaterialMessageBoxResult, object> closingEventHandler)
        {
            try
            {
                MaterialMessageBoxResult result = Model.MaterialMessageBoxResult.None;

                await this.UIDispatcher.Invoke(async () =>
                {
                    try
                    {
                        var retval = await DialogHost.Show(
                            view,
                            this.DialogIdentifier,
                            new DialogClosingEventHandler(
                                (sender, eventArgs) =>
                                {
                                    // if the settin view was closed return
                                    if (eventArgs.Parameter.GetType() != typeof(MaterialMessageBoxResult))
                                        return;

                                    var displayableView = closingEventHandler?.Invoke((MaterialMessageBoxResult)eventArgs.Parameter);
                                    if (displayableView != null)
                                    {
                                        eventArgs.Cancel();
                                        eventArgs.Session.UpdateContent(displayableView);
                                    }
                                }));

                        if (retval == null)
                            result = Model.MaterialMessageBoxResult.None;
                        else
                            result = retval.GetType() == typeof(MaterialMessageBoxResult) ? (MaterialMessageBoxResult)retval : Model.MaterialMessageBoxResult.None;
                    }
                    catch (InvalidOperationException)
                    {
                        // this can happen if the window is closed and the dialog can't be shown
                    }
                });

                return result;
            }
            catch (Exception e)
            {
                this.UIDispatcher.Invoke(() =>
                {
                    var n = new Greg.WPF.Utility.ExceptionMessageBox(e, "Unhandled Exception");
                    n.ShowDialog();
                });

                throw;
            }
        }
    }
}

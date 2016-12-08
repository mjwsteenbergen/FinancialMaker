using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FinancialMaker.Logic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinancialMaker.Steps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImportHTML : Page, IStep
    {
        private readonly MainPage _page;

        public ImportHTML(MainPage page)
        {
            _page = page;
            this.InitializeComponent();
        }

        public string StepName => "Step 1 : Import HTML";
        public Color StepColor => Color.FromArgb(255, 27, 161, 226);

        private async void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<Transaction> transactions = HTMLConverter.ConvertToObject((sender as TextBox).Text);
//                _page.SetPage();
                _page.EnableContinue(new ImportRules(_page, transactions));


            }
            catch (Exception e2)
            {
                _page.Error(e2.Message);
//                await (new MessageDialog(e2.Message)).ShowAsync();
            }
        }
    }
}

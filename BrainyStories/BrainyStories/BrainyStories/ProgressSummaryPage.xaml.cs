using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressSummaryPage : ContentPage
    {
        public ProgressSummaryPage()
        {
            InitializeComponent();
            // Set the binding context for this page to the User singleton instance, so that stats come from that User
            // BindingContext = Objects.User.Instance;
        }
    }
}
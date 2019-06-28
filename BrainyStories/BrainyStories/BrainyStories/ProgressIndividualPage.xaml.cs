using BrainyStories.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressIndividualPage : ContentPage
    {
        public ProgressIndividualPage()
        {
            InitializeComponent();
            // Set the binding context for this page to the User singleton instance, so that stats come from that User
            //BindingContext = User.Instance;
        }
    }
}
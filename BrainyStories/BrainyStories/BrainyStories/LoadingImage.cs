using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories
{
    public class LoadingImage : CachedImage
    {
        public LoadingImage()
        {
            //this is where we set the loading animation
            this.LoadingPlaceholder = "Loading_Animation.gif";
        }
    }
}
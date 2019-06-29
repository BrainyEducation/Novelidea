﻿using Xamarin.Forms;

namespace BrainyStories
{
    public class Stars : Label
    {
        public const int SMALL_STAR_SIZE = 30;
        public const int MEDIUM_STAR_SIZE = 50;
        public const int LARGE_STAR_SIZE = 80;

        //force the usage of the FontSize setter
        public new double FontSize { get; private set; }

        public bool Vibrates { get; set; }

        public void SetFontSize(double size)
        {
            base.FontSize = size * TextSizeMultiplier;
            this.FontSize = base.FontSize;
        }

        //used for the text embedded in a star
        public double TextSizeMultiplier { get; }

        //set a lot of default values
        public Stars(int column, int row, double textSizeMultiplier = 1)
        {
            TextSizeMultiplier = textSizeMultiplier;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            SetFontSize(SMALL_STAR_SIZE);
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            Grid.SetColumnSpan(this, 1);
            Grid.SetRow(this, row);
            Grid.SetColumn(this, column);
            VerticalOptions = LayoutOptions.CenterAndExpand;
        }

        /// <summary>
        /// Allow us to compare the text and star with the same Font Size constants,
        /// even though they are technically different fonts
        /// </summary>
        /// <param name="fontSize">The font size which we should compare to</param>
        /// <returns>true if they are equal, false otherwise</returns>
        public bool CompareFontSize(double fontSize)
        {
            return (fontSize * TextSizeMultiplier) == FontSize ? true : false;
        }
    }
}
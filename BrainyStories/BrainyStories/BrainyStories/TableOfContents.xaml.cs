﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainyStories
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TableOfContents : ContentPage
	{
        private List<Story> storyList;
        public TableOfContents ()
		{
			InitializeComponent();

            // TO-DO: 
            // make these into their own class for better code
            // add icons below title for quizes and home expierences
            /// add clickability

            StoryFactory storyFact = new StoryFactory();
            storyList = storyFact.generateStories();

            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            var storyIndex = 0;
            var rowNum = 0;
            for (int storyNum = 0; storyNum < storyList.Count; storyNum+=3 )
            {
                for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    var story = storyList[storyIndex];
                    storyIndex += 1;
                    var image = new Image { Source = story.Icon };
                    var label = new Label
                    {
                        Text = story.Name,
                        //VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    gridLayout.Children.Add(image, columnIndex, rowNum);
                    Grid.SetRowSpan(image, 2);
                    gridLayout.Children.Add(label, columnIndex, rowNum + 1);

                    gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
                    gridLayout.Children.Add(label, columnIndex, rowNum + 2);
                }
                rowNum += 3;
            }
        }
    }
}
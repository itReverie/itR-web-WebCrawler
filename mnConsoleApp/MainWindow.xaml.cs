using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mnBookmarkAnalyzer;
using mnEntityObjects;

namespace mnConsoleBookmarkAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Main Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        /// <summary>
        /// Creates the planets, links and keywords based on the bookmarks file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePlanetLinks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessingProgressBar();
                string bookmarkFile = ConfigurationManager.AppSettings["BookmarkFile"].ToString();
                LinkAnalyzer linkAnalyzer = new LinkAnalyzer();
                IPlanetsCollection planetCollection = linkAnalyzer.GetLinksFromFavoritesFile(bookmarkFile);
               
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: "+exception.Message);
            }
        }
        #endregion

        #region Methods
        public void ProcessingProgressBar()
        {
            Duration MaxTime = new Duration(TimeSpan.FromSeconds(10));
            DoubleAnimation doubleAnimation = new DoubleAnimation(100, MaxTime);
            progressBarLoadFile.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }
        #endregion

    }
}

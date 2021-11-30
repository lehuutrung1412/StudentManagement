using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for TimerControl.xaml
    /// </summary>
    public partial class ClockControl : UserControl
    {
        public ClockControl()
        {
            InitializeComponent();
            SetTimeNumber();
            DispatcherTimer timerDots = new DispatcherTimer();
            timerDots.Tick += new EventHandler(MethodAnimation);
            timerDots.Interval = new TimeSpan(0, 0, 1);
            timerDots.Start();
        }
        private void MethodAnimation(object sender, EventArgs e)
        {
            SetTimeNumber();
            ((Storyboard)FindResource("TickDots")).Begin();
        }

        private string ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch ((int)dayOfWeek)
            {
                case 1:
                    return "Thứ hai";
                case 2:
                    return "Thứ ba";
                case 3:
                    return "Thứ tư";
                case 4:
                    return "Thứ năm";
                case 5:
                    return "Thứ sáu";
                case 6:
                    return "Thứ bảy";
                default:
                    return "Chủ nhật";
            }
        }
        public void SetTimeNumber()
        {

            DateTime mainDateTime = DateTime.Now;
            Day.Text = ConvertDayOfWeek(mainDateTime.DayOfWeek);
            DayNumeric.Text = "Ngày " + mainDateTime.Date.ToString().Split(' ')[0];
            if ((mainDateTime.TimeOfDay.Hours.ToString().ToCharArray()).Length == 2)
            {
                Hours1.Text = mainDateTime.TimeOfDay.Hours.ToString().ToCharArray()[0].ToString();
                Hours2.Text = mainDateTime.TimeOfDay.Hours.ToString().ToCharArray()[1].ToString();
            }
            else
            {
                Hours1.Text = 0.ToString();
                Hours2.Text = mainDateTime.TimeOfDay.Hours.ToString().ToCharArray()[0].ToString();
            }
            // Minutes Minutes Minutes Minutes Minutes Minutes Minutes Minutes Minutes Minutes
            if ((mainDateTime.TimeOfDay.Minutes.ToString().ToCharArray()).Length == 2)
            {
                Minutes1.Text = mainDateTime.TimeOfDay.Minutes.ToString().ToCharArray()[0].ToString();
                Minutes2.Text = mainDateTime.TimeOfDay.Minutes.ToString().ToCharArray()[1].ToString();
            }
            else
            {
                Minutes1.Text = 0.ToString();
                Minutes2.Text = mainDateTime.TimeOfDay.Minutes.ToString().ToCharArray()[0].ToString();
            }
            // SECONDS SECONDS SECONDS SECONDS SECONDS SECONDS SECONDS SECONDS SECONDS SECONDS
            if ((mainDateTime.TimeOfDay.Seconds.ToString().ToCharArray()).Length == 2)
            {
                Seconds1.Text = mainDateTime.TimeOfDay.Seconds.ToString().ToCharArray()[0].ToString();
                Seconds2.Text = mainDateTime.TimeOfDay.Seconds.ToString().ToCharArray()[1].ToString();
            }
            else
            {
                Seconds1.Text = 0.ToString();
                Seconds2.Text = mainDateTime.TimeOfDay.Seconds.ToString().ToCharArray()[0].ToString();
            }
        }
    }
}

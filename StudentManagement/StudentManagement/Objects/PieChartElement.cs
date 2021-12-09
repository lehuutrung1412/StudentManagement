using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StudentManagement.Objects
{
    public class PieChartElement
    {
        private float _percent;
        private string _title;
        private Brush _colorBrush;

        public float Percentage
        {
            get => _percent;
            set => _percent = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public Brush ColorBrush
        {
            get => _colorBrush;
            set => _colorBrush = value;
        }
    }
}

using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class InfoItem : BaseViewModel
    {
        private Guid _id;
        private string _labelName;
        private int _type;
        private ObservableCollection<string> _itemSource;
        private object _value;
        private bool _isEnable;

        public Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string LabelName { get => _labelName; set { _labelName = value; OnPropertyChanged(); } }
        public int Type { get => _type; set { _type = value; OnPropertyChanged(); } }
        public ObservableCollection<string> ItemSource { get => _itemSource; set { _itemSource = value; OnPropertyChanged(); } }
        public object Value { get => _value; set { _value = value; OnPropertyChanged(); } }

        public bool IsEnable { get => _isEnable; set => _isEnable = value; }


        public InfoItem()
        {
            Id = Guid.NewGuid();
            IsEnable = true;
        }

        public InfoItem(Guid id, string labelName, int type, ObservableCollection<string> itemSource, object value, bool isEnable)
        {
            Id = id;
            LabelName = labelName;
            Type = type;
            ItemSource = itemSource;
            Value = value;
            IsEnable = isEnable;
        }
        public void CopyItem(InfoItem item)
        {
            Id = item.Id;
            LabelName = item.LabelName;
            Type = item.Type;
            ItemSource = item.ItemSource;
            Value = item.Value;
            IsEnable = item.IsEnable;
        }
        public InfoItem(InfoItem item)
        {
            Id = item.Id;
            LabelName = item.LabelName;
            Type = item.Type;
            ItemSource = item.ItemSource;
            Value = item.Value;
            IsEnable = item.IsEnable;
        }
    }
}

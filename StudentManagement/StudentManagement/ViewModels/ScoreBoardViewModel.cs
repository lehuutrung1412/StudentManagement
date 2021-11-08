using Microsoft.Win32;
using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
	public class ScoreBoardViewModel : BaseViewModel
	{
		private ObservableCollection<Score> _studentScoreItems;
		public ObservableCollection<Score> StudentScoreItems { get => _studentScoreItems; set => _studentScoreItems = value; }
		public ScoreBoardViewModel()
		{
			StudentScoreItems = new ObservableCollection<Score>();

			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 1 });
			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 2 });
			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 3 });
			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 4 });
			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 5 });
			StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 6 });

		}

	}

	public class Score
	{
		private string _idSubject;
		private string _subject;
		private string _credit;
		private string _faculty;
		private string _status;
		private int _id;

		public int ID
        {
			get => _id;
			set => _id = value;
        }

		public string IDSubject 
		{ 
			get => _idSubject; 
			set => _idSubject = value; 
		}

		public string Subject
		{
			get => _subject;
			set => _subject = value;
		}

		public string Credit
		{
			get => _credit;
			set => _credit = value;
		}

		public string Faculty
		{
			get => _faculty;
			set => _faculty = value;
		}

		public string Status
		{
			get => _status;
			set => _status = value;
		}

	}



}
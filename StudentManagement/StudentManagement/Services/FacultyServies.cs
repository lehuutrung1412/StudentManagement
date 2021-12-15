using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class FacultyServices
    {
        private static FacultyServices s_instance;

        public static FacultyServices Instance => s_instance ?? (s_instance = new FacultyServices());

        public FacultyServices() { }

        public DbSet<Faculty> LoadFacultyList()
        {
            return DataProvider.Instance.Database.Faculties;
        }

        /// <summary>
        /// Convert FacultyCard To Faculty
        /// </summary>
        /// <param name="facultyCard"></param>
        /// <returns>Faculty</returns>
        public Faculty ConvertFacultyCardToFaculty(FacultyCard facultyCard)
        {
            Faculty faculty = new Faculty()
            {
                Id = facultyCard.Id,
                DisplayName = facultyCard.DisplayName,
                Faculty_TrainingForm = new HashSet<Faculty_TrainingForm>()
            };

            return faculty;
        }

        /// <summary>
        /// Convert Faculty To Faculty Card
        /// </summary>
        /// <param name="faculty"></param>
        /// <returns>FacultyCard</returns>
        public FacultyCard ConvertFacultyToFacultyCard(Faculty faculty)
        {
            int numberOfStudentsOfFaculty = DataProvider.Instance.Database.Students.Where(student => student.IdFaculty == faculty.Id).Count();

            FacultyCard facultyCard = new FacultyCard(faculty.Id, faculty.DisplayName, new DateTime(2015, 12, 31), numberOfStudentsOfFaculty);

            return facultyCard;
        }

        /// <summary>
        /// Find Faculty By Faculty Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Faculty || null</returns>
        public Faculty FindFacultyByFacultyId(Guid id)
        {
            Faculty faculty = DataProvider.Instance.Database.Faculties.Where(facultyItem => facultyItem.Id == id).FirstOrDefault();

            return faculty;
        }

        /// <summary>
        /// Save Faculty To Database
        /// </summary>
        /// <param name="faculty"></param>
        public bool SaveFacultyToDatabase(Faculty faculty)
        {
            try
            {
                DataProvider.Instance.Database.Faculties.AddOrUpdate(faculty);
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save Faculty Card To Database
        /// </summary>
        /// <param name="facultyCard"></param>
        public bool SaveFacultyCardToDatabase(FacultyCard facultyCard)
        {
            Faculty faculty = ConvertFacultyCardToFaculty(facultyCard);

            return SaveFacultyToDatabase(faculty);
        }

        /// <summary>
        /// Remove Faculty From Database
        /// </summary>
        /// <param name="faculty"></param>
        public bool RemoveFacultyFromDatabase(Faculty faculty)
        {
            try
            {
                Faculty savedFaculty = FindFacultyByFacultyId(faculty.Id);

                // soft delete
                savedFaculty.IsDeleted = true;

                //DataProvider.Instance.Database.Faculties.Remove(savedFaculty);

                DataProvider.Instance.Database.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove FacultyCard From Database
        /// </summary>
        /// <param name="facultyCard"></param>
        public bool RemoveFacultyCardFromDatabase(FacultyCard facultyCard)
        {
            Faculty faculty = ConvertFacultyCardToFaculty(facultyCard);

            return RemoveFacultyFromDatabase(faculty);
        }

        public ObservableCollection<string> LoadListFaculty()
        {
            ObservableCollection<string> listFaculty = new ObservableCollection<string>();
            DataProvider.Instance.Database.Faculties.ToList().ForEach(faculty => listFaculty.Add(faculty.DisplayName));
            return listFaculty;
        }
    }
}

using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class Faculty_TrainingFormServices
    {
        private static Faculty_TrainingFormServices s_instance;

        public static Faculty_TrainingFormServices Instance => s_instance ?? (s_instance = new Faculty_TrainingFormServices());

        public Faculty_TrainingFormServices() { }

        public List<Faculty> LoadFacultyByTrainingForm(TrainingForm trainingForm)
        {
            return trainingForm.Faculty_TrainingForm.Select(faculty_TrainingFormItem => faculty_TrainingFormItem.Faculty).ToList();
        }

        public List<TrainingForm> LoadTrainingFormByFaculty(Faculty faculty)
        {
            try
            {
                if (faculty != null)
                {
                    return faculty.Faculty_TrainingForm.Select(faculty_TrainingFormItem => faculty_TrainingFormItem.TrainingForm).ToList();
                }
                else
                {
                    return new List<TrainingForm>();
                }
            }
            catch
            {
                return new List<TrainingForm>();
            }
        }
    }
}

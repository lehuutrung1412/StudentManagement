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
    public class TrainingFormServices
    {
        private static TrainingFormServices s_instance;

        public static TrainingFormServices Instance => s_instance ?? (s_instance = new TrainingFormServices());

        public TrainingFormServices() { }

        public DbSet<TrainingForm> LoadTrainingFormList()
        {
            return DataProvider.Instance.Database.TrainingForms;
        }

        public TrainingForm FindTrainingFormByDisplayName(string name)
        {
            TrainingForm trainingForm = DataProvider.Instance.Database.TrainingForms.Where(trainingFormItem => trainingFormItem.DisplayName == name).FirstOrDefault();
            return trainingForm;
        }

        /// <summary>
        /// Convert TrainingFormCard To TrainingForm
        /// </summary>
        /// <param name="trainingFormCard"></param>
        /// <returns>TrainingForm</returns>
        public TrainingForm ConvertTrainingFormCardToTrainingForm(TrainingFormCard trainingFormCard)
        {
            TrainingForm trainingForm = new TrainingForm()
            {
                Id = trainingFormCard.Id,
                DisplayName = trainingFormCard.DisplayName
            };

            return trainingForm;
        }

        /// <summary>
        /// Convert TrainingForm To TrainingForm Card
        /// </summary>
        /// <param name="trainingForm"></param>
        /// <returns>TrainingFormCard</returns>
        public TrainingFormCard ConvertTrainingFormToTrainingFormCard(TrainingForm trainingForm)
        {
            int numberOfStudentsOfTrainingForm = DataProvider.Instance.Database.Students.Where(student => student.IdTrainingForm == trainingForm.Id).Count();
            TrainingFormCard trainingFormCard = new TrainingFormCard(trainingForm.Id, trainingForm.DisplayName, trainingForm.Faculty_TrainingForm.Count, numberOfStudentsOfTrainingForm);

            return trainingFormCard;
        }

        /// <summary>
        /// Find TrainingForm By TrainingForm Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TrainingForm || null</returns>
        public TrainingForm FindTrainingFormByTrainingFormId(Guid id)
        {
            TrainingForm trainingForm = DataProvider.Instance.Database.TrainingForms.Where(trainingFormItem => trainingFormItem.Id == id).FirstOrDefault();

            return trainingForm;
        }

        /// <summary>
        /// Save TrainingForm To Database
        /// </summary>
        /// <param name="trainingForm"></param>
        public bool SaveTrainingFormToDatabase(TrainingForm trainingForm)
        {
            try
            {
                DataProvider.Instance.Database.TrainingForms.AddOrUpdate(trainingForm);
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save TrainingForm Card To Database
        /// </summary>
        /// <param name="trainingFormCard"></param>
        public bool SaveTrainingFormCardToDatabase(TrainingFormCard trainingFormCard)
        {

            try
            {
                TrainingForm trainingForm = ConvertTrainingFormCardToTrainingForm(trainingFormCard);

                SaveTrainingFormToDatabase(trainingForm);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove TrainingForm From Database
        /// </summary>
        /// <param name="traingingForm"></param>
        public bool RemoveTrainingFormFromDatabase(TrainingForm traingingForm)
        {
            try
            {
                TrainingForm savedTrainingForm = FindTrainingFormByTrainingFormId(traingingForm.Id);

                //DataProvider.Instance.Database.TrainingForms.Remove(savedTrainingForm);

                // soft delete
                savedTrainingForm.IsDeleted = true;

                DataProvider.Instance.Database.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove TrainingFormCard From Database
        /// </summary>
        /// <param name="traingingFormCard"></param>
        public bool RemoveTrainingFormCardFromDatabase(TrainingFormCard traingingFormCard)
        {
            TrainingForm traingingForm = ConvertTrainingFormCardToTrainingForm(traingingFormCard);

            return RemoveTrainingFormFromDatabase(traingingForm);
        }
        public ObservableCollection<string> LoadListTrainingForm()
        {
            ObservableCollection<string> listTrainingForm = new ObservableCollection<string>();
            DataProvider.Instance.Database.TrainingForms.Where(el => el.IsDeleted != true).ToList().ForEach(trainningForm => listTrainingForm.Add(trainningForm.DisplayName));
            return listTrainingForm;
        }
    }
}

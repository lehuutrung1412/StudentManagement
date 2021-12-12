using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class ScoreServices
    {
        private static ScoreServices s_instance;

        public static ScoreServices Instance => s_instance ?? (s_instance = new ScoreServices());

        public Func<StudentManagementEntities> db = () => DataProvider.Instance.Database;

        #region Convert

        public ComponentScore ConvertScoreInSettingToComponentScore(ComponentScoreInSetting score)
        {
            return new ComponentScore()
            {
                Id = score.Id,
                IdSubjectClass = score.IdSubjectClass,
                DisplayName = score.DisplayName,
                ContributePercent = score.Percent
            };
        }

        public ComponentScoreInSetting ConvertComponentScoreToScoreInSetting(ComponentScore score)
        {
            return new ComponentScoreInSetting()
            {
                Id = score.Id,
                DisplayName = score.DisplayName,
                IdSubjectClass = score.IdSubjectClass,
                Percent = score.ContributePercent
            };
        }

        public double? CalculateAverageScore(List<StudentDetailScore> scores)
        {
            return scores.Average(score => score.Score);
        }

        #endregion Convert

        #region Create

        public async Task<int> SaveComponentScoreDatabaseAsync(ComponentScoreInSetting score)
        {
            db().ComponentScores.AddOrUpdate(ConvertScoreInSettingToComponentScore(score));
            return await db().SaveChangesAsync();
        }

        #endregion

        #region Read

        public List<ComponentScore> LoadComponentScoreOfSubjectClass(Guid idSubjectClass)
        {
            return db().ComponentScores.Where(score => score.IdSubjectClass == idSubjectClass).ToList();
        }

        public List<StudentDetailScore> LoadScoreStudentInSubjectClass(Guid idSubjectClass)
        {
            return (from components in db().ComponentScores
                    join details in db().DetailScores
                    on components.Id equals details.IdComponentScore
                    select new StudentDetailScore()
                    {
                        Id = details.Id,
                        DisplayName = components.DisplayName,
                        IdComponentScore = components.Id,
                        IdStudent = details.IdStudent,
                        IdSubjectClass = components.IdSubjectClass,
                        Percent = components.ContributePercent,
                        Score = details.Score
                    }).ToList();
        }

        #endregion

        #region Delete

        public async Task DeleteComponentScoreAsync(ComponentScoreInSetting score)
        {
            var loadedScore = db().ComponentScores.FirstOrDefault(s => s.Id == score.Id);
            db().ComponentScores.Remove(loadedScore);
            await db().SaveChangesAsync();
        }

        public async Task DeleteComponentScoreBySubjectClassAsync(Guid idSubjectClass)
        {
            var scores = db().ComponentScores.Where(s => s.IdSubjectClass == idSubjectClass);
            db().ComponentScores.RemoveRange(scores);
            await db().SaveChangesAsync();
        }

        #endregion
    }
}

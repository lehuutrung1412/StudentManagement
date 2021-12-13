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

        public DetailScore ConvertStudentDetailScoreToDetailScore(StudentDetailScore score)
        {
            return new DetailScore()
            {
                Id = score.Id,
                IdComponentScore = score.IdComponentScore,
                IdStudent = score.IdStudent,
                Score = score.Score
            };
        }

        public double? CalculateAverageScore(List<StudentDetailScore> scores)
        {
            double? averageScore = 0;
            scores.ForEach(score => averageScore += score.Score * score.Percent / 100);
            return averageScore;
        }

        #endregion Convert

        #region Create

        public async Task<int> SaveComponentScoreDatabaseAsync(ComponentScoreInSetting score)
        {
            db().ComponentScores.AddOrUpdate(ConvertScoreInSettingToComponentScore(score));
            return await db().SaveChangesAsync();
        }

        public async Task<int> SaveStudentScoreDatabaseAsync(List<StudentDetailScore> scores)
        {
            foreach (var score in scores)
            {
                db().DetailScores.AddOrUpdate(ConvertStudentDetailScoreToDetailScore(score));
            }
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

        public List<StudentDetailScore> LoadScoreStudentById(Guid idSubjectClass, Guid idStudent)
        {
            return (from components in db().ComponentScores
                    join details in db().DetailScores
                    on components.Id equals details.IdComponentScore
                    where components.IdSubjectClass == idSubjectClass && details.IdStudent == idStudent
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

        public List<DetailScore> GetComponentScoreInDetailScoreById(Guid idComponentScore)
        {
            return db().DetailScores.Where(score => score.IdComponentScore == idComponentScore).ToList();
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

        public async Task DeleteStudentScoreByIdAsync(Guid idSubjectClass, Guid idStudent)
        {
            var scores = db().DetailScores.Where(s => s.IdStudent == idStudent && s.ComponentScore.IdSubjectClass == idSubjectClass);
            db().DetailScores.RemoveRange(scores);
            await db().SaveChangesAsync();
        }

        public async Task DeleteListComponentScoreAsync(List<ComponentScoreInSetting> scores)
        {
            foreach (var score in scores)
            {
                var studentScores = db().DetailScores.Where(ds => ds.IdComponentScore == score.Id);
                db().DetailScores.RemoveRange(studentScores);
                db().ComponentScores.RemoveRange(db().ComponentScores.Where(s => s.Id == score.Id));
            }
            await db().SaveChangesAsync();
        }

        #endregion
    }
}

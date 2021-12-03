using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class FileServices
    {
        private static FileServices s_instance;

        public static FileServices Instance => s_instance ?? (s_instance = new FileServices());

        public Func<StudentManagementEntities> db = () => DataProvider.Instance.Database;

        public FileServices() { }

        #region Convert

        public Document ConvertFileInfoToDocument(FileInfo file)
        {
            return new Document()
            {
                Id = (Guid)file.Id,
                DisplayName = file.Name,
                Content = file.Content,
                CreatedAt = file.UploadTime,
                IdFolder = file.FolderId,
                IdPoster = file.PublisherId,
                IdSubjectClass = file.IdSubjectClass,
                Size = file.Size
            };
        }

        public FileInfo ConvertDocumentToFileInfo(Document doc)
        {
            return new FileInfo(doc.Id, doc.DisplayName, Guid.NewGuid(), doc.User.DisplayName, doc.Content, doc.CreatedAt, doc.Size, doc.IdFolder, doc.Folder.DisplayName, doc.IdSubjectClass);
        }

        public Folder ConvertFileInfoToFolder(FileInfo file)
        {
            return new Folder()
            {
                Id = (Guid)file.FolderId,
                DisplayName = file.FolderName,
                IdSubjectClass = file.IdSubjectClass
            };
        }

        #endregion Convert

        #region Create

        public async Task<int> SaveFileOfSubjectClassToDatabaseAsync(FileInfo file)
        {
            db().Documents.Add(ConvertFileInfoToDocument(file));
            return await db().SaveChangesAsync();
        }

        public async Task<int> SaveFolderOfSubjectClassToDatabaseAsync(FileInfo file)
        {

            db().Folders.Add(ConvertFileInfoToFolder(file));
            return await db().SaveChangesAsync();
        }

        #endregion

        #region Read

        public List<Document> GetListFilesOfSubjectClass(Guid? idSubjectClass)
        {
            return db().Documents.Where(file => file.IdSubjectClass == idSubjectClass).ToList();
        }

        public List<Folder> GetListFoldersOfSubjectClass(Guid? idSubjectClass)
        {
            return db().Folders.Where(folder => folder.IdSubjectClass == idSubjectClass).ToList();
        }

        public List<Folder> GetListFoldersHasFilesOfSubjectClass(Guid? idSubjectClass)
        {
            return GetListFilesOfSubjectClass(idSubjectClass).Select(file => file.Folder).Distinct().ToList();
        }

        public List<Folder> GetListSingleFoldersOfSubjectClass(Guid? idSubjectClass)
        {
            return GetListFoldersOfSubjectClass(idSubjectClass).Except(GetListFoldersHasFilesOfSubjectClass(idSubjectClass)).ToList();
        }

        #endregion Read

        #region Delete

        public async Task<int> DeleteFileAsync(FileInfo file)
        {
            var doc = db().Documents.FirstOrDefault(document => document.Id == file.Id);
            db().Documents.Remove(doc);
            return await db().SaveChangesAsync();
        }

        public async Task<int> DeleteFolderAsync(FileInfo file)
        {
            db().Folders.Remove(ConvertFileInfoToFolder(file));
            return await db().SaveChangesAsync();
        }

        #endregion
    }
}

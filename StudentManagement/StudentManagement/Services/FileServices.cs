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
            return new FileInfo(doc.Id, doc.DisplayName, Guid.NewGuid(), doc.User.DisplayName, doc.Content, doc.CreatedAt, doc.Size, doc.IdFolder, doc.Folder.DisplayName);
        }

        #endregion Convert

        #region Create

        public async Task<int> SaveFileOfSubjectClassToDatabase(FileInfo file)
        {
            DataProvider.Instance.Database.Documents.Add(ConvertFileInfoToDocument(file));
            return await DataProvider.Instance.Database.SaveChangesAsync();
        }

        #endregion

        #region Read

        public List<Document> GetListFilesOfSubjectClass(Guid? idSubjectClass)
        {
            return DataProvider.Instance.Database.Documents.Where(file => file.IdSubjectClass == idSubjectClass).ToList();
        }

        public List<Folder> GetListFoldersOfSubjectClass(Guid? idSubjectClass)
        {
            return DataProvider.Instance.Database.Folders.Where(folder => folder.IdSubjectClass == idSubjectClass).ToList();
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
    }
}

using FamilyTreeProject.Core.Contracts;
using FamilyTreeProject.Data.Common;

namespace FamilyTreeProject.Data.Json
{
    public class JsonUnitOfWork: FileUnitOfWork
    {
        public JsonUnitOfWork(string path)
        {
            Requires.NotNullOrEmpty("path", path);
            
            Initialize(new JsonFileStore(path));
        }

        public JsonUnitOfWork(IFileStore store) : base(store) {}
    }
}